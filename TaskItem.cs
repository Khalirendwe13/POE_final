using System;

namespace CyberGuardBot
{
    // Represents one cybersecurity task the user wants to track.
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string DisplayLine()
        {
            string status = IsCompleted ? "[DONE] " : "[ ] ";

            string reminder = ReminderDate.HasValue
                ? $"  (Remind: {ReminderDate.Value:yyyy-MM-dd})"
                : "";

            return $"{status}{Title}{reminder}";
        }
    }
}
