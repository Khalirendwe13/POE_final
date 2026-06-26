using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberGuardBot
{
    // Records actions the bot has taken so the user can review them later.
    // Backs Task 4 of the POE (Activity Log Feature).
    public class ActivityLog
    {
        private readonly List<LogEntry> entries = new List<LogEntry>();

        public IReadOnlyList<LogEntry> Entries => entries;

        public void Record(string action)
        {
            entries.Add(new LogEntry(DateTime.Now, action));
        }

        // Returns the most recent N actions (default 10) for the
        // "Show activity log" command.
        public IEnumerable<LogEntry> Recent(int count = 10)
        {
            return entries
                .AsEnumerable()
                .Reverse()
                .Take(count);
        }

        public void Clear()
        {
            entries.Clear();
        }

        public class LogEntry
        {
            public DateTime Timestamp { get; }
            public string Action { get; }

            public LogEntry(DateTime timestamp, string action)
            {
                Timestamp = timestamp;
                Action = action;
            }

            public override string ToString()
            {
                return $"[{Timestamp:yyyy-MM-dd HH:mm}] {Action}";
            }
        }
    }
}

