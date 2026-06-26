using System;
using System.Text.RegularExpressions;

namespace CyberGuardBot
{
    public enum NlpIntent
    {
        Unknown,
        AddTask,
        SetReminder,
        ShowTasks,
        CompleteTask,
        DeleteTask,
        StartQuiz,
        ShowActivityLog,
        ShowMoreLog
    }

    public class NlpResult
    {
        public NlpIntent Intent { get; set; } = NlpIntent.Unknown;
        public string? Subject { get; set; }
        public int? Days { get; set; }
        public DateTime? Date { get; set; }
    }

    // Lightweight keyword + regex based NLP simulator. Recognises
    // common cybersecurity command phrasings so the chat experience
    // feels natural without needing a real NLP library.
    public class NlpRouter
    {
        public NlpResult Parse(string input)
        {
            var result = new NlpResult();
            string text = input.ToLowerInvariant().Trim();

            // Activity log
            if (Contains(text, "show activity log", "activity log", "what have you done", "show log", "show me the log"))
            {
                result.Intent = NlpIntent.ShowActivityLog;
                return result;
            }

            if (Contains(text, "show more", "more log", "full log", "full history"))
            {
                result.Intent = NlpIntent.ShowMoreLog;
                return result;
            }

            // Quiz
            if (Contains(text, "start quiz", "start the quiz", "quiz me", "play quiz", "play the quiz", "begin quiz"))
            {
                result.Intent = NlpIntent.StartQuiz;
                return result;
            }

            // View tasks
            if (Contains(text, "show tasks", "list tasks", "view tasks", "my tasks", "what tasks"))
            {
                result.Intent = NlpIntent.ShowTasks;
                return result;
            }

            // Complete / delete task
            var completeMatch = Regex.Match(text, @"(?:mark|complete|finish|done).*?task\s*(?:#|number\s*)?(\d+)");
            if (completeMatch.Success)
            {
                result.Intent = NlpIntent.CompleteTask;
                result.Subject = completeMatch.Groups[1].Value;
                return result;
            }

            var deleteMatch = Regex.Match(text, @"(?:delete|remove).*?task\s*(?:#|number\s*)?(\d+)");
            if (deleteMatch.Success)
            {
                result.Intent = NlpIntent.DeleteTask;
                result.Subject = deleteMatch.Groups[1].Value;
                return result;
            }

            // Reminder-only (no add-task wording)
            if (Contains(text, "remind me", "set a reminder", "set reminder"))
            {
                result.Intent = NlpIntent.SetReminder;
                result.Subject = ExtractSubject(text);
                ExtractTime(text, result);
                return result;
            }

            // Add task
            if (Contains(text, "add task", "add a task", "create task", "new task", "add a reminder to") ||
                Regex.IsMatch(text, @"^\s*add\s+\w"))
            {
                result.Intent = NlpIntent.AddTask;
                result.Subject = ExtractSubject(text);
                ExtractTime(text, result);
                return result;
            }

            return result;
        }

        private static bool Contains(string text, params string[] keywords)
        {
            foreach (var k in keywords)
            {
                if (text.Contains(k)) return true;
            }
            return false;
        }

        private static string? ExtractSubject(string text)
        {
            // Try "add task - <subject>" or "add a task to <subject>"
            var m = Regex.Match(text,
                @"(?:add (?:a )?task(?: to)?|create (?:a )?task|new task|set (?:a )?reminder(?: to)?|remind me to)\s*[:\-]?\s*(.+)",
                RegexOptions.IgnoreCase);

            if (m.Success)
            {
                string s = m.Groups[1].Value.Trim();
                // Strip the time portion so the subject is just the action.
                s = Regex.Replace(s,
                    @"\s+(?:in\s+\d+\s+(?:day|days|week|weeks|month|months)|tomorrow|today|on\s+\d{4}-\d{2}-\d{2}).*$",
                    "",
                    RegexOptions.IgnoreCase);

                return string.IsNullOrWhiteSpace(s) ? null : s.Trim('.', ' ', '"', '\'');
            }

            return null;
        }

        private static void ExtractTime(string text, NlpResult result)
        {
            var daysMatch = Regex.Match(text, @"in\s+(\d+)\s+day", RegexOptions.IgnoreCase);
            if (daysMatch.Success)
            {
                result.Days = int.Parse(daysMatch.Groups[1].Value);
                result.Date = DateTime.Today.AddDays(result.Days.Value);
                return;
            }

            var weeksMatch = Regex.Match(text, @"in\s+(\d+)\s+week", RegexOptions.IgnoreCase);
            if (weeksMatch.Success)
            {
                int weeks = int.Parse(weeksMatch.Groups[1].Value);
                result.Days = weeks * 7;
                result.Date = DateTime.Today.AddDays(result.Days.Value);
                return;
            }

            if (text.Contains("tomorrow"))
            {
                result.Days = 1;
                result.Date = DateTime.Today.AddDays(1);
                return;
            }

            if (text.Contains("today"))
            {
                result.Days = 0;
                result.Date = DateTime.Today;
                return;
            }

            var isoMatch = Regex.Match(text, @"on\s+(\d{4}-\d{2}-\d{2})", RegexOptions.IgnoreCase);
            if (isoMatch.Success &&
                DateTime.TryParse(isoMatch.Groups[1].Value, out var dt))
            {
                result.Date = dt;
            }
        }
    }
}
