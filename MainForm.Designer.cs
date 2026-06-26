namespace CyberGuardBot
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabChat;
        private System.Windows.Forms.TabPage tabTasks;
        private System.Windows.Forms.TabPage tabQuiz;
        private System.Windows.Forms.TabPage tabLog;

        // Chat tab
        private System.Windows.Forms.RichTextBox rtbChat;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnSaveName;
        private System.Windows.Forms.Label lblStoredName;
        private System.Windows.Forms.Label lblFavouriteTopic;
        private System.Windows.Forms.Label lblLastTopic;
        private System.Windows.Forms.ListBox lstTopics;

        // Tasks tab
        private System.Windows.Forms.ListBox lstTasks;
        private System.Windows.Forms.TextBox txtTaskTitle;
        private System.Windows.Forms.TextBox txtTaskDesc;
        private System.Windows.Forms.DateTimePicker dtReminder;
        private System.Windows.Forms.CheckBox chkHasReminder;
        private System.Windows.Forms.Button btnAddTask;
        private System.Windows.Forms.Button btnCompleteTask;
        private System.Windows.Forms.Button btnDeleteTask;
        private System.Windows.Forms.Label lblTaskTitle;
        private System.Windows.Forms.Label lblTaskDesc;

        // Quiz tab
        private System.Windows.Forms.Label lblQuizPrompt;
        private System.Windows.Forms.RadioButton rbOpt0;
        private System.Windows.Forms.RadioButton rbOpt1;
        private System.Windows.Forms.RadioButton rbOpt2;
        private System.Windows.Forms.RadioButton rbOpt3;
        private System.Windows.Forms.Button btnQuizStart;
        private System.Windows.Forms.Button btnQuizSubmit;
        private System.Windows.Forms.Label lblQuizFeedback;
        private System.Windows.Forms.Label lblQuizScore;

        // Activity log tab
        private System.Windows.Forms.ListBox lstActivity;
        private System.Windows.Forms.Button btnRefreshLog;
        private System.Windows.Forms.CheckBox chkShowMoreLog;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tabs = new System.Windows.Forms.TabControl();
            tabChat = new System.Windows.Forms.TabPage();
            tabTasks = new System.Windows.Forms.TabPage();
            tabQuiz = new System.Windows.Forms.TabPage();
            tabLog = new System.Windows.Forms.TabPage();

            rtbChat = new System.Windows.Forms.RichTextBox();
            txtInput = new System.Windows.Forms.TextBox();
            btnSend = new System.Windows.Forms.Button();
            lblTitle = new System.Windows.Forms.Label();
            txtName = new System.Windows.Forms.TextBox();
            btnSaveName = new System.Windows.Forms.Button();
            lblStoredName = new System.Windows.Forms.Label();
            lblFavouriteTopic = new System.Windows.Forms.Label();
            lblLastTopic = new System.Windows.Forms.Label();
            lstTopics = new System.Windows.Forms.ListBox();

            lstTasks = new System.Windows.Forms.ListBox();
            txtTaskTitle = new System.Windows.Forms.TextBox();
            txtTaskDesc = new System.Windows.Forms.TextBox();
            dtReminder = new System.Windows.Forms.DateTimePicker();
            chkHasReminder = new System.Windows.Forms.CheckBox();
            btnAddTask = new System.Windows.Forms.Button();
            btnCompleteTask = new System.Windows.Forms.Button();
            btnDeleteTask = new System.Windows.Forms.Button();
            lblTaskTitle = new System.Windows.Forms.Label();
            lblTaskDesc = new System.Windows.Forms.Label();

            lblQuizPrompt = new System.Windows.Forms.Label();
            rbOpt0 = new System.Windows.Forms.RadioButton();
            rbOpt1 = new System.Windows.Forms.RadioButton();
            rbOpt2 = new System.Windows.Forms.RadioButton();
            rbOpt3 = new System.Windows.Forms.RadioButton();
            btnQuizStart = new System.Windows.Forms.Button();
            btnQuizSubmit = new System.Windows.Forms.Button();
            lblQuizFeedback = new System.Windows.Forms.Label();
            lblQuizScore = new System.Windows.Forms.Label();

            lstActivity = new System.Windows.Forms.ListBox();
            btnRefreshLog = new System.Windows.Forms.Button();
            chkShowMoreLog = new System.Windows.Forms.CheckBox();

            SuspendLayout();

            // ===== Form =====
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
            ClientSize = new System.Drawing.Size(1100, 720);
            Name = "MainForm";
            Text = "CyberGuard Bot";

            // ===== Title =====
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.Cyan;
            lblTitle.Location = new System.Drawing.Point(20, 10);
            lblTitle.Text = "CYBERGUARD";
            Controls.Add(lblTitle);

            // ===== Tabs =====
            tabs.Location = new System.Drawing.Point(20, 60);
            tabs.Size = new System.Drawing.Size(1060, 640);
            tabs.Controls.Add(tabChat);
            tabs.Controls.Add(tabTasks);
            tabs.Controls.Add(tabQuiz);
            tabs.Controls.Add(tabLog);

            tabChat.Text = "Chat";
            tabChat.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            tabTasks.Text = "Tasks";
            tabTasks.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            tabQuiz.Text = "Quiz";
            tabQuiz.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            tabLog.Text = "Activity Log";
            tabLog.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);

            Controls.Add(tabs);

            // ===== Chat tab =====
            rtbChat.BackColor = System.Drawing.Color.Black;
            rtbChat.ForeColor = System.Drawing.Color.White;
            rtbChat.Location = new System.Drawing.Point(310, 15);
            rtbChat.Size = new System.Drawing.Size(720, 480);
            rtbChat.ReadOnly = true;
            rtbChat.Name = "rtbChat";
            tabChat.Controls.Add(rtbChat);

            txtInput.Location = new System.Drawing.Point(310, 510);
            txtInput.Size = new System.Drawing.Size(580, 27);
            txtInput.Name = "txtInput";
            tabChat.Controls.Add(txtInput);

            btnSend.Location = new System.Drawing.Point(905, 505);
            btnSend.Size = new System.Drawing.Size(125, 36);
            btnSend.Text = "SEND";
            btnSend.BackColor = System.Drawing.Color.DarkCyan;
            btnSend.ForeColor = System.Drawing.Color.White;
            btnSend.UseVisualStyleBackColor = false;
            btnSend.Click += btnSend_Click;
            tabChat.Controls.Add(btnSend);

            txtName.Location = new System.Drawing.Point(15, 20);
            txtName.Size = new System.Drawing.Size(170, 27);
            txtName.Name = "txtName";
            tabChat.Controls.Add(txtName);

            btnSaveName.Location = new System.Drawing.Point(190, 17);
            btnSaveName.Size = new System.Drawing.Size(100, 32);
            btnSaveName.Text = "Save Name";
            btnSaveName.BackColor = System.Drawing.Color.Teal;
            btnSaveName.ForeColor = System.Drawing.Color.White;
            btnSaveName.UseVisualStyleBackColor = false;
            btnSaveName.Click += btnSaveName_Click;
            tabChat.Controls.Add(btnSaveName);

            lblStoredName.AutoSize = true;
            lblStoredName.ForeColor = System.Drawing.Color.White;
            lblStoredName.Location = new System.Drawing.Point(15, 60);
            lblStoredName.Text = "Name: ";
            tabChat.Controls.Add(lblStoredName);

            lblFavouriteTopic.AutoSize = true;
            lblFavouriteTopic.ForeColor = System.Drawing.Color.White;
            lblFavouriteTopic.Location = new System.Drawing.Point(15, 90);
            lblFavouriteTopic.Text = "Favourite: ";
            tabChat.Controls.Add(lblFavouriteTopic);

            lblLastTopic.AutoSize = true;
            lblLastTopic.ForeColor = System.Drawing.Color.White;
            lblLastTopic.Location = new System.Drawing.Point(15, 120);
            lblLastTopic.Text = "Last Topic: ";
            tabChat.Controls.Add(lblLastTopic);

            lstTopics.Location = new System.Drawing.Point(15, 160);
            lstTopics.Size = new System.Drawing.Size(275, 340);
            lstTopics.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            lstTopics.ForeColor = System.Drawing.Color.White;
            tabChat.Controls.Add(lstTopics);

            // ===== Tasks tab =====
            lblTaskTitle.AutoSize = true;
            lblTaskTitle.ForeColor = System.Drawing.Color.White;
            lblTaskTitle.Location = new System.Drawing.Point(15, 20);
            lblTaskTitle.Text = "Title:";
            tabTasks.Controls.Add(lblTaskTitle);

            txtTaskTitle.Location = new System.Drawing.Point(15, 45);
            txtTaskTitle.Size = new System.Drawing.Size(400, 27);
            tabTasks.Controls.Add(txtTaskTitle);

            lblTaskDesc.AutoSize = true;
            lblTaskDesc.ForeColor = System.Drawing.Color.White;
            lblTaskDesc.Location = new System.Drawing.Point(15, 80);
            lblTaskDesc.Text = "Description:";
            tabTasks.Controls.Add(lblTaskDesc);

            txtTaskDesc.Location = new System.Drawing.Point(15, 105);
            txtTaskDesc.Size = new System.Drawing.Size(400, 100);
            txtTaskDesc.Multiline = true;
            tabTasks.Controls.Add(txtTaskDesc);

            chkHasReminder.Location = new System.Drawing.Point(15, 220);
            chkHasReminder.Size = new System.Drawing.Size(150, 25);
            chkHasReminder.Text = "Set reminder";
            chkHasReminder.ForeColor = System.Drawing.Color.White;
            tabTasks.Controls.Add(chkHasReminder);

            dtReminder.Location = new System.Drawing.Point(170, 220);
            dtReminder.Size = new System.Drawing.Size(245, 27);
            dtReminder.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            tabTasks.Controls.Add(dtReminder);

            btnAddTask.Location = new System.Drawing.Point(15, 260);
            btnAddTask.Size = new System.Drawing.Size(120, 38);
            btnAddTask.Text = "Add Task";
            btnAddTask.BackColor = System.Drawing.Color.DarkCyan;
            btnAddTask.ForeColor = System.Drawing.Color.White;
            btnAddTask.UseVisualStyleBackColor = false;
            btnAddTask.Click += btnAddTask_Click;
            tabTasks.Controls.Add(btnAddTask);

            lstTasks.Location = new System.Drawing.Point(440, 20);
            lstTasks.Size = new System.Drawing.Size(595, 480);
            lstTasks.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            lstTasks.ForeColor = System.Drawing.Color.White;
            tabTasks.Controls.Add(lstTasks);

            btnCompleteTask.Location = new System.Drawing.Point(440, 510);
            btnCompleteTask.Size = new System.Drawing.Size(180, 38);
            btnCompleteTask.Text = "Mark Completed";
            btnCompleteTask.BackColor = System.Drawing.Color.Teal;
            btnCompleteTask.ForeColor = System.Drawing.Color.White;
            btnCompleteTask.UseVisualStyleBackColor = false;
            btnCompleteTask.Click += btnCompleteTask_Click;
            tabTasks.Controls.Add(btnCompleteTask);

            btnDeleteTask.Location = new System.Drawing.Point(640, 510);
            btnDeleteTask.Size = new System.Drawing.Size(180, 38);
            btnDeleteTask.Text = "Delete Task";
            btnDeleteTask.BackColor = System.Drawing.Color.Firebrick;
            btnDeleteTask.ForeColor = System.Drawing.Color.White;
            btnDeleteTask.UseVisualStyleBackColor = false;
            btnDeleteTask.Click += btnDeleteTask_Click;
            tabTasks.Controls.Add(btnDeleteTask);

            // ===== Quiz tab =====
            lblQuizPrompt.Location = new System.Drawing.Point(20, 20);
            lblQuizPrompt.Size = new System.Drawing.Size(1000, 80);
            lblQuizPrompt.ForeColor = System.Drawing.Color.White;
            lblQuizPrompt.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblQuizPrompt.Text = "Click 'Start Quiz' to begin.";
            tabQuiz.Controls.Add(lblQuizPrompt);

            rbOpt0.Location = new System.Drawing.Point(40, 120);
            rbOpt0.Size = new System.Drawing.Size(900, 30);
            rbOpt0.ForeColor = System.Drawing.Color.White;
            tabQuiz.Controls.Add(rbOpt0);

            rbOpt1.Location = new System.Drawing.Point(40, 160);
            rbOpt1.Size = new System.Drawing.Size(900, 30);
            rbOpt1.ForeColor = System.Drawing.Color.White;
            tabQuiz.Controls.Add(rbOpt1);

            rbOpt2.Location = new System.Drawing.Point(40, 200);
            rbOpt2.Size = new System.Drawing.Size(900, 30);
            rbOpt2.ForeColor = System.Drawing.Color.White;
            tabQuiz.Controls.Add(rbOpt2);

            rbOpt3.Location = new System.Drawing.Point(40, 240);
            rbOpt3.Size = new System.Drawing.Size(900, 30);
            rbOpt3.ForeColor = System.Drawing.Color.White;
            tabQuiz.Controls.Add(rbOpt3);

            btnQuizStart.Location = new System.Drawing.Point(40, 290);
            btnQuizStart.Size = new System.Drawing.Size(140, 38);
            btnQuizStart.Text = "Start Quiz";
            btnQuizStart.BackColor = System.Drawing.Color.DarkCyan;
            btnQuizStart.ForeColor = System.Drawing.Color.White;
            btnQuizStart.UseVisualStyleBackColor = false;
            btnQuizStart.Click += btnQuizStart_Click;
            tabQuiz.Controls.Add(btnQuizStart);

            btnQuizSubmit.Location = new System.Drawing.Point(200, 290);
            btnQuizSubmit.Size = new System.Drawing.Size(140, 38);
            btnQuizSubmit.Text = "Submit Answer";
            btnQuizSubmit.BackColor = System.Drawing.Color.Teal;
            btnQuizSubmit.ForeColor = System.Drawing.Color.White;
            btnQuizSubmit.UseVisualStyleBackColor = false;
            btnQuizSubmit.Enabled = false;
            btnQuizSubmit.Click += btnQuizSubmit_Click;
            tabQuiz.Controls.Add(btnQuizSubmit);

            lblQuizFeedback.Location = new System.Drawing.Point(20, 350);
            lblQuizFeedback.Size = new System.Drawing.Size(1000, 120);
            lblQuizFeedback.ForeColor = System.Drawing.Color.LightGreen;
            tabQuiz.Controls.Add(lblQuizFeedback);

            lblQuizScore.Location = new System.Drawing.Point(20, 490);
            lblQuizScore.Size = new System.Drawing.Size(400, 30);
            lblQuizScore.ForeColor = System.Drawing.Color.Cyan;
            lblQuizScore.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            lblQuizScore.Text = "Score: 0";
            tabQuiz.Controls.Add(lblQuizScore);

            // ===== Activity log tab =====
            lstActivity.Location = new System.Drawing.Point(20, 20);
            lstActivity.Size = new System.Drawing.Size(1015, 510);
            lstActivity.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            lstActivity.ForeColor = System.Drawing.Color.White;
            tabLog.Controls.Add(lstActivity);

            btnRefreshLog.Location = new System.Drawing.Point(20, 545);
            btnRefreshLog.Size = new System.Drawing.Size(140, 38);
            btnRefreshLog.Text = "Refresh";
            btnRefreshLog.BackColor = System.Drawing.Color.DarkCyan;
            btnRefreshLog.ForeColor = System.Drawing.Color.White;
            btnRefreshLog.UseVisualStyleBackColor = false;
            btnRefreshLog.Click += btnRefreshLog_Click;
            tabLog.Controls.Add(btnRefreshLog);

            chkShowMoreLog.Location = new System.Drawing.Point(180, 550);
            chkShowMoreLog.Size = new System.Drawing.Size(220, 30);
            chkShowMoreLog.Text = "Show more (full history)";
            chkShowMoreLog.ForeColor = System.Drawing.Color.White;
            chkShowMoreLog.CheckedChanged += (s, e) => RefreshActivityLog();
            tabLog.Controls.Add(chkShowMoreLog);

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
