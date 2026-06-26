using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CyberGuardBot
{
    public partial class MainForm : Form
    {
        private readonly ChatBot bot;
        private readonly QuizGame quiz;

        public MainForm()
        {
            InitializeComponent();

            bot = new ChatBot();
            bot.Logger = SaveMessage;
            bot.TasksChanged += () => RefreshTasks();
            bot.QuizStartRequested += () =>
            {
                tabs.SelectedTab = tabQuiz;
                StartQuiz();
            };

            quiz = new QuizGame();

            txtInput.KeyDown += TxtInput_KeyDown;

            // Populate topics list (from ResponseSystem)
            try
            {
                var resp = new ResponseSystem();
                lstTopics.Items.Clear();
                foreach (var topic in resp.GetTopics())
                {
                    lstTopics.Items.Add(topic);
                }
                lstTopics.DoubleClick += LstTopics_DoubleClick;
            }
            catch
            {
                // Topics aren't critical — ignore failure.
            }

            UpdateMemoryDisplay();
            RefreshTasks();
            RefreshActivityLog();

            rtbChat.SelectionColor = Color.Lime;
            rtbChat.AppendText(
@"  ██████╗██╗   ██╗██████╗ ███████╗██████╗
 ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗
 ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝
 ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗
 ╚██████╗   ██║   ██████╔╝███████╗██║  ██║
  ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝

");

            AppendBotMessage($"{ChatBot.TimeGreeting()}! I'm CyberGuard. How can I help?");
            AppendBotMessage(
                "Try chatting ('hi', 'how are you'), ask a security question, " +
                "or run a command: 'add task - enable 2FA', 'start quiz', 'show activity log'.");
        }

        // ===============================
        // CHAT TAB
        // ===============================

        private async void btnSend_Click(object sender, EventArgs e)
        {
            string input = txtInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Please enter a message.");
                return;
            }

            AppendUserMessage(input);
            txtInput.Clear();

            ShowTypingIndicator();
            btnSend.Enabled = false;

            try
            {
                // Small delay so the typing indicator is actually visible
                // even when the bot replies instantly. Makes it feel alive.
                var processing = bot.ProcessMessage(input);
                await Task.WhenAll(processing, Task.Delay(450));

                HideTypingIndicator();
                AppendBotMessage(processing.Result);
                UpdateMemoryDisplay();
                RefreshActivityLog();
            }
            catch (Exception ex)
            {
                HideTypingIndicator();
                AppendBotMessage("Sorry, something went wrong. Please try again.");
                System.Diagnostics.Debug.WriteLine(ex);
            }
            finally
            {
                btnSend.Enabled = true;
                txtInput.Focus();
            }
        }

        private int typingIndicatorStart = -1;

        private void ShowTypingIndicator()
        {
            typingIndicatorStart = rtbChat.TextLength;
            rtbChat.SelectionStart = typingIndicatorStart;
            rtbChat.SelectionColor = Color.Gray;
            rtbChat.AppendText($"CyberGuard is typing...{Environment.NewLine}");
            rtbChat.ScrollToCaret();
        }

        private void HideTypingIndicator()
        {
            if (typingIndicatorStart < 0) return;
            int length = rtbChat.TextLength - typingIndicatorStart;
            rtbChat.Select(typingIndicatorStart, length);
            rtbChat.SelectedText = "";
            typingIndicatorStart = -1;
        }

        private void TxtInput_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSend.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        private void LstTopics_DoubleClick(object? sender, EventArgs e)
        {
            if (lstTopics.SelectedItem is string topic)
            {
                txtInput.Text = topic;
                btnSend.PerformClick();
            }
        }

        private void AppendUserMessage(string message)
        {
            rtbChat.SelectionColor = Color.Cyan;
            rtbChat.AppendText($"[{DateTime.Now:HH:mm}] You: {message}{Environment.NewLine}");
            rtbChat.SelectionStart = rtbChat.Text.Length;
            rtbChat.ScrollToCaret();
        }

        private void AppendBotMessage(string message)
        {
            rtbChat.SelectionColor = Color.Magenta;
            rtbChat.AppendText(
                $"[{DateTime.Now:HH:mm}] CyberGuard: {message}{Environment.NewLine}{Environment.NewLine}");
            rtbChat.SelectionStart = rtbChat.Text.Length;
            rtbChat.ScrollToCaret();
        }

        private void SaveMessage(string message)
        {
            try
            {
                System.IO.File.AppendAllText(
                    "messages.txt",
                    message + Environment.NewLine);
            }
            catch
            {
                // Logging failure must never crash the chat.
            }
        }

        private void btnSaveName_Click(object sender, EventArgs e)
        {
            bot.Memory.UserName = txtName.Text;
            UpdateMemoryDisplay();
            AppendBotMessage($"Nice to meet you, {bot.Memory.UserName}!");
        }

        private void UpdateMemoryDisplay()
        {
            lblStoredName.Text = "Name: " + bot.Memory.UserName;
            lblFavouriteTopic.Text = "Favourite: " + bot.Memory.FavouriteTopic;
            lblLastTopic.Text = "Last Topic: " + bot.Memory.LastTopic;
        }

        // ===============================
        // TASKS TAB
        // ===============================

        private void btnAddTask_Click(object? sender, EventArgs e)
        {
            string title = txtTaskTitle.Text.Trim();

            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Please enter a task title.");
                return;
            }

            var task = new TaskItem
            {
                Title = title,
                Description = string.IsNullOrWhiteSpace(txtTaskDesc.Text)
                    ? "Cybersecurity task."
                    : txtTaskDesc.Text.Trim(),
                ReminderDate = chkHasReminder.Checked
                    ? dtReminder.Value.Date
                    : (DateTime?)null
            };

            bot.Tasks.Add(task);
            bot.Log.Record($"Task added via UI: '{task.Title}'" +
                (task.ReminderDate.HasValue
                    ? $" (reminder {task.ReminderDate:yyyy-MM-dd})"
                    : ""));

            txtTaskTitle.Clear();
            txtTaskDesc.Clear();
            chkHasReminder.Checked = false;

            RefreshTasks();
            RefreshActivityLog();
        }

        private TaskItem? GetSelectedTask()
        {
            return (lstTasks.SelectedItem as TaskListEntry)?.Task;
        }

        private void btnCompleteTask_Click(object? sender, EventArgs e)
        {
            var t = GetSelectedTask();
            if (t == null)
            {
                MessageBox.Show("Select a task first.");
                return;
            }

            bot.Tasks.MarkCompleted(t.Id);
            bot.Log.Record($"Task #{t.Id} marked completed via UI");
            RefreshTasks();
            RefreshActivityLog();
        }

        private void btnDeleteTask_Click(object? sender, EventArgs e)
        {
            var t = GetSelectedTask();
            if (t == null)
            {
                MessageBox.Show("Select a task first.");
                return;
            }

            bot.Tasks.Delete(t.Id);
            bot.Log.Record($"Task #{t.Id} deleted via UI");
            RefreshTasks();
            RefreshActivityLog();
        }

        private void RefreshTasks()
        {
            if (lstTasks.InvokeRequired)
            {
                lstTasks.BeginInvoke(new Action(RefreshTasks));
                return;
            }

            // Show each task as a TaskListEntry wrapper. The wrapper's
            // ToString() controls what's displayed; GetSelectedTask()
            // recovers the underlying TaskItem from the selection.
            lstTasks.Items.Clear();
            foreach (var t in bot.Tasks.GetAll())
            {
                lstTasks.Items.Add(new TaskListEntry(t));
            }
        }

        // Wrapper so the ListBox shows a formatted line while still
        // letting us recover the underlying TaskItem on selection.
        private class TaskListEntry
        {
            public TaskItem Task { get; }
            public TaskListEntry(TaskItem t) { Task = t; }
            public override string ToString()
            {
                return $"#{Task.Id}  {Task.DisplayLine()} - {Task.Description}";
            }
        }

        // ===============================
        // QUIZ TAB
        // ===============================

        private void btnQuizStart_Click(object? sender, EventArgs e)
        {
            StartQuiz();
        }

        private void StartQuiz()
        {
            quiz.Start();
            answeredCount = 0;
            bot.Log.Record("Quiz started");
            lblQuizFeedback.Text = "";

            btnQuizStart.Enabled = false;
            btnQuizStart.Text = "Quiz in progress...";
            btnQuizSubmit.Enabled = true;

            ShowCurrentQuestion();
            RefreshActivityLog();
        }

        private void ShowCurrentQuestion()
        {
            var q = quiz.Current();
            if (q == null)
            {
                EndQuiz();
                return;
            }

            lblQuizPrompt.Text = $"Q{quiz.Score + GetAnsweredCount() + 1}/{quiz.Total}:  {q.Prompt}";

            var radios = new[] { rbOpt0, rbOpt1, rbOpt2, rbOpt3 };
            for (int i = 0; i < radios.Length; i++)
            {
                if (i < q.Options.Length)
                {
                    radios[i].Visible = true;
                    radios[i].Text = q.Options[i];
                    radios[i].Checked = false;
                }
                else
                {
                    radios[i].Visible = false;
                }
            }

            lblQuizScore.Text = $"Score: {quiz.Score}";
        }

        // QuizGame doesn't expose answered count directly, but the
        // index used inside Current() = score + wrong answers. Easier to
        // just track our own counter via a field for the prompt label.
        private int answeredCount = 0;
        private int GetAnsweredCount() => answeredCount;

        private void btnQuizSubmit_Click(object? sender, EventArgs e)
        {
            int picked = -1;
            var radios = new[] { rbOpt0, rbOpt1, rbOpt2, rbOpt3 };
            for (int i = 0; i < radios.Length; i++)
            {
                if (radios[i].Visible && radios[i].Checked) { picked = i; break; }
            }

            if (picked < 0)
            {
                MessageBox.Show("Pick an answer first.");
                return;
            }

            var q = quiz.Current();
            if (q == null) return;

            bool correct = quiz.Answer(picked);
            answeredCount++;

            lblQuizFeedback.ForeColor = correct ? Color.LightGreen : Color.Salmon;
            lblQuizFeedback.Text = (correct ? "Correct! " : "Not quite. ") + q.Explanation;

            lblQuizScore.Text = $"Score: {quiz.Score}";

            if (quiz.Finished)
            {
                EndQuiz();
                return;
            }

            ShowCurrentQuestion();
        }

        private void EndQuiz()
        {
            bot.Log.Record($"Quiz completed: {quiz.Score}/{quiz.Total}");
            RefreshActivityLog();

            lblQuizPrompt.Text = "Quiz complete!";
            foreach (var r in new[] { rbOpt0, rbOpt1, rbOpt2, rbOpt3 })
                r.Visible = false;

            lblQuizFeedback.ForeColor = Color.Cyan;
            lblQuizFeedback.Text = quiz.FinalFeedback();
            lblQuizScore.Text = $"Final Score: {quiz.Score}/{quiz.Total}";

            btnQuizSubmit.Enabled = false;
            btnQuizStart.Enabled = true;
            btnQuizStart.Text = "Play Again";

            answeredCount = 0;
        }

        // ===============================
        // ACTIVITY LOG TAB
        // ===============================

        private void btnRefreshLog_Click(object? sender, EventArgs e)
        {
            RefreshActivityLog();
        }

        private void RefreshActivityLog()
        {
            if (lstActivity.InvokeRequired)
            {
                lstActivity.BeginInvoke(new Action(RefreshActivityLog));
                return;
            }

            lstActivity.Items.Clear();

            int count = chkShowMoreLog.Checked ? 1000 : 10;

            foreach (var e in bot.Log.Recent(count))
            {
                lstActivity.Items.Add(e.ToString());
            }

            if (lstActivity.Items.Count == 0)
            {
                lstActivity.Items.Add("(No activity yet — add a task or start the quiz.)");
            }
        }
    }
}
