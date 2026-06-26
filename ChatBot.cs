using System;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace CyberGuardBot
{
    public class ChatBot
    {
        private readonly ResponseSystem responseSystem;
        private readonly SentimentAnalyzer sentimentAnalyzer;
        private readonly MemoryManager memory;
        private readonly NlpRouter nlp;
        private readonly TaskRepository tasks;
        private readonly ActivityLog log;

        private string currentTopic = "";

        // Holds a pending task between "add task" and the user's "yes/no"
        // answer to the reminder follow-up question.
        private TaskItem? pendingTaskAwaitingReminder;

        public delegate void MessageLogger(string message);
        public MessageLogger? Logger;

        public MemoryManager Memory => memory;
        public TaskRepository Tasks => tasks;
        public ActivityLog Log => log;

        // Fired whenever the bot adds/changes a task so the GUI can refresh.
        public event Action? TasksChanged;

        // Fired when a quiz start command comes through chat so the GUI
        // can switch to the quiz tab.
        public event Action? QuizStartRequested;

        public ChatBot()
        {
            responseSystem = new ResponseSystem();
            sentimentAnalyzer = new SentimentAnalyzer();
            memory = new MemoryManager();
            nlp = new NlpRouter();
            tasks = new TaskRepository();
            log = new ActivityLog();

            PlayVoiceGreeting();
        }

        public async Task<string> ProcessMessage(string input)
        {
            return await Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(input))
                    return "Please enter a question.";

                string cleaned = input.ToLower();
                Logger?.Invoke(cleaned);

                // Pending reminder follow-up takes priority so the bot
                // feels like it's holding a conversation.
                if (pendingTaskAwaitingReminder != null)
                {
                    string? reply = HandleReminderFollowUp(cleaned);
                    if (reply != null) return reply;
                }

                // ----- NLP command routing (Part 3) -----
                var intent = nlp.Parse(input);

                switch (intent.Intent)
                {
                    case NlpIntent.AddTask:
                        return HandleAddTask(intent);

                    case NlpIntent.SetReminder:
                        return HandleSetReminder(intent);

                    case NlpIntent.ShowTasks:
                        return HandleShowTasks();

                    case NlpIntent.CompleteTask:
                        return HandleCompleteTask(intent);

                    case NlpIntent.DeleteTask:
                        return HandleDeleteTask(intent);

                    case NlpIntent.StartQuiz:
                        log.Record("Quiz started from chat");
                        QuizStartRequested?.Invoke();
                        return "Starting the cybersecurity quiz! Switch to the Quiz tab to play.";

                    case NlpIntent.ShowActivityLog:
                        return HandleShowLog(10);

                    case NlpIntent.ShowMoreLog:
                        return HandleShowLog(50);
                }

                // ----- Small talk (greetings, how-are-you, thanks, bye) -----
                string? chat = responseSystem.GetSmallTalk(cleaned);
                if (chat != null)
                {
                    return Personalise(chat);
                }

                // ----- Part 1/2 memory + conversational features -----
                if (cleaned.Contains("my name is"))
                {
                    string[] parts = cleaned.Split(' ');
                    memory.UserName = Capitalise(parts.Last());
                    log.Record($"Stored user name: {memory.UserName}");
                    return $"{TimeGreeting()}, {memory.UserName}! I'll remember your name.";
                }

                if (cleaned.Contains("interested in password"))
                {
                    memory.FavouriteTopic = "Password Safety";
                    return "Great! I'll remember that password safety is your favourite topic.";
                }
                if (cleaned.Contains("interested in privacy"))
                {
                    memory.FavouriteTopic = "Privacy";
                    return "Great! I'll remember that privacy is your favourite topic.";
                }
                if (cleaned.Contains("interested in phishing"))
                {
                    memory.FavouriteTopic = "Phishing";
                    return "Great! I'll remember that phishing awareness is your favourite topic.";
                }

                if (cleaned.Contains("another tip") ||
                    cleaned.Contains("tell me more") ||
                    cleaned.Contains("explain more"))
                {
                    return responseSystem.GetAnotherTip(currentTopic);
                }

                string sentiment = sentimentAnalyzer.DetectSentiment(cleaned);

                string topic = responseSystem.DetectTopic(cleaned);
                if (!string.IsNullOrEmpty(topic))
                {
                    currentTopic = topic;
                    memory.LastTopic = topic;
                }

                string response = responseSystem.GetResponse(cleaned);

                if (sentiment == "worried")
                    response = "It is understandable to feel worried about cybersecurity threats.\n\n" + response;
                else if (sentiment == "frustrated")
                    response = "Cybersecurity can sometimes feel overwhelming.\n\n" + response;
                else if (sentiment == "curious")
                    response = "I'm glad you're curious about cybersecurity.\n\n" + response;

                if (!string.IsNullOrEmpty(memory.FavouriteTopic))
                {
                    response +=
                        $"\n\nSince you are interested in {memory.FavouriteTopic}, this advice may help you even more.";
                }

                return response;
            });
        }

        // ===============================
        // Task / reminder handlers
        // ===============================

        private string HandleAddTask(NlpResult intent)
        {
            string title = intent.Subject ?? "Untitled task";

            var task = new TaskItem
            {
                Title = title,
                Description = BuildAutoDescription(title),
                ReminderDate = intent.Date
            };

            tasks.Add(task);
            log.Record($"Task added: '{task.Title}'" +
                (task.ReminderDate.HasValue
                    ? $" (reminder {task.ReminderDate:yyyy-MM-dd})"
                    : ""));
            TasksChanged?.Invoke();

            if (intent.Date.HasValue)
            {
                return $"Task added: '{task.Title}'. I'll remind you on {intent.Date:yyyy-MM-dd}.";
            }

            // No reminder time was given — ask for one.
            pendingTaskAwaitingReminder = task;
            return $"Task added: '{task.Title}'. Would you like to set a reminder for this task?";
        }

        private string? HandleReminderFollowUp(string cleaned)
        {
            if (pendingTaskAwaitingReminder == null) return null;

            if (cleaned.StartsWith("no") || cleaned.Contains("no thanks") || cleaned.Contains("not now"))
            {
                string title = pendingTaskAwaitingReminder.Title;
                pendingTaskAwaitingReminder = null;
                return $"Okay, no reminder set for '{title}'.";
            }

            // Try to parse the timeframe from the reply.
            var followIntent = new NlpResult();
            var router = new NlpRouter();
            var parsed = router.Parse("remind me " + cleaned);

            DateTime? when = parsed.Date;

            if (cleaned.StartsWith("yes") && !when.HasValue)
            {
                return "Sure — when should I remind you? You can say e.g. 'in 3 days', 'tomorrow', or 'on 2026-07-01'.";
            }

            if (when.HasValue)
            {
                var task = pendingTaskAwaitingReminder;
                task.ReminderDate = when;
                tasks.UpdateReminder(task.Id, when);
                log.Record($"Reminder set for '{task.Title}' on {when:yyyy-MM-dd}");
                TasksChanged?.Invoke();
                pendingTaskAwaitingReminder = null;
                return $"Got it! I'll remind you about '{task.Title}' on {when:yyyy-MM-dd}.";
            }

            return null;
        }

        private string HandleSetReminder(NlpResult intent)
        {
            string title = intent.Subject ?? "Reminder";

            var task = new TaskItem
            {
                Title = title,
                Description = BuildAutoDescription(title),
                ReminderDate = intent.Date
            };

            tasks.Add(task);
            log.Record($"Reminder set: '{title}'" +
                (intent.Date.HasValue
                    ? $" on {intent.Date:yyyy-MM-dd}"
                    : ""));
            TasksChanged?.Invoke();

            if (intent.Date.HasValue)
                return $"Reminder set for '{title}' on {intent.Date:yyyy-MM-dd}.";

            return $"Reminder noted for '{title}'. (No date detected — open the Tasks tab to set one.)";
        }

        private string HandleShowTasks()
        {
            var all = tasks.GetAll();
            if (all.Count == 0)
                return "You don't have any tasks yet. Try: 'Add task - enable two-factor authentication'.";

            var sb = new StringBuilder();
            sb.AppendLine("Your tasks:");
            foreach (var t in all)
            {
                sb.AppendLine($"  #{t.Id} {t.DisplayLine()}");
            }
            return sb.ToString().TrimEnd();
        }

        private string HandleCompleteTask(NlpResult intent)
        {
            if (!int.TryParse(intent.Subject, out int id))
                return "Which task? Say e.g. 'mark task 3 as done'.";

            tasks.MarkCompleted(id);
            log.Record($"Task #{id} marked completed");
            TasksChanged?.Invoke();
            return $"Marked task #{id} as completed.";
        }

        private string HandleDeleteTask(NlpResult intent)
        {
            if (!int.TryParse(intent.Subject, out int id))
                return "Which task? Say e.g. 'delete task 3'.";

            tasks.Delete(id);
            log.Record($"Task #{id} deleted");
            TasksChanged?.Invoke();
            return $"Deleted task #{id}.";
        }

        private string HandleShowLog(int count)
        {
            var recent = log.Recent(count);
            var sb = new StringBuilder();
            sb.AppendLine("Here's a summary of recent actions:");
            int i = 1;
            foreach (var e in recent)
            {
                sb.AppendLine($"  {i}. {e}");
                i++;
            }
            if (i == 1) sb.AppendLine("  (No actions yet.)");
            return sb.ToString().TrimEnd();
        }

        private static string BuildAutoDescription(string title)
        {
            string lower = title.ToLowerInvariant();
            if (lower.Contains("two-factor") || lower.Contains("2fa"))
                return "Enable two-factor authentication on your accounts for an extra layer of security.";
            if (lower.Contains("password"))
                return "Update or strengthen your passwords; use a password manager.";
            if (lower.Contains("privacy"))
                return "Review account privacy settings to ensure your data is protected.";
            if (lower.Contains("phishing"))
                return "Stay alert for phishing emails and report suspicious messages.";
            if (lower.Contains("update") || lower.Contains("backup"))
                return "Keep software up to date and back up important data regularly.";
            return "Cybersecurity task to keep your accounts and data safe.";
        }

        // Adds the user's name to a small-talk reply if we know it,
        // so responses feel personal once the user has introduced themself.
        private string Personalise(string reply)
        {
            if (string.IsNullOrWhiteSpace(memory.UserName))
                return reply;

            // 30% of the time, prefix with the user's name to keep it
            // natural rather than every-message-uses-name awkward.
            if (new Random().Next(100) < 30)
                return $"{memory.UserName}, {char.ToLower(reply[0])}{reply.Substring(1)}";

            return reply;
        }

        // Picks "Good morning / afternoon / evening" based on the clock.
        public static string TimeGreeting()
        {
            int h = DateTime.Now.Hour;
            if (h < 12) return "Good morning";
            if (h < 18) return "Good afternoon";
            return "Good evening";
        }

        private static string Capitalise(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public void SaveChatLog(string message)
        {
            File.AppendAllText(
                "ChatLog.txt",
                DateTime.Now + " : " + message + Environment.NewLine);
        }

        public void PlayVoiceGreeting()
        {
            try
            {
                string[] candidateDirs = new[]
                {
                    AppDomain.CurrentDomain.BaseDirectory,
                    Environment.CurrentDirectory,
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources")
                };

                string? found = null;
                foreach (var dir in candidateDirs)
                {
                    if (!Directory.Exists(dir)) continue;
                    found = Directory.EnumerateFiles(dir, "greeting*wav").FirstOrDefault();
                    if (found != null) break;
                }

                if (found != null)
                {
                    using var player = new SoundPlayer(found);
                    player.Load();
                    player.Play();
                }
            }
            catch
            {
                // Audio failure must never crash the app.
            }
        }
    }
}
