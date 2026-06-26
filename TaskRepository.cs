using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;

namespace CyberGuardBot
{
    // Stores and retrieves TaskItems from a local SQLite database.
    // Implements the CRUD operations required by Task 1 (Task Assistant)
    // and the Database Integration rubric item.
    public class TaskRepository
    {
        private readonly string connectionString;

        public TaskRepository()
        {
            string dbPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "cyberguard.db");

            connectionString = $"Data Source={dbPath}";

            EnsureSchema();
        }

        private void EnsureSchema()
        {
            using var conn = new SqliteConnection(connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Tasks (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Description TEXT NOT NULL,
                    ReminderDate TEXT NULL,
                    IsCompleted INTEGER NOT NULL DEFAULT 0,
                    CreatedAt TEXT NOT NULL
                );";
            cmd.ExecuteNonQuery();
        }

        public int Add(TaskItem task)
        {
            using var conn = new SqliteConnection(connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Tasks (Title, Description, ReminderDate, IsCompleted, CreatedAt)
                VALUES ($title, $desc, $reminder, $done, $created);
                SELECT last_insert_rowid();";

            cmd.Parameters.AddWithValue("$title", task.Title);
            cmd.Parameters.AddWithValue("$desc", task.Description);
            cmd.Parameters.AddWithValue(
                "$reminder",
                task.ReminderDate.HasValue
                    ? (object)task.ReminderDate.Value.ToString("o")
                    : DBNull.Value);
            cmd.Parameters.AddWithValue("$done", task.IsCompleted ? 1 : 0);
            cmd.Parameters.AddWithValue("$created", task.CreatedAt.ToString("o"));

            long id = (long)(cmd.ExecuteScalar() ?? 0L);
            task.Id = (int)id;
            return task.Id;
        }

        public List<TaskItem> GetAll()
        {
            var list = new List<TaskItem>();

            using var conn = new SqliteConnection(connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT Id, Title, Description, ReminderDate, IsCompleted, CreatedAt
                FROM Tasks
                ORDER BY IsCompleted ASC, CreatedAt DESC;";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new TaskItem
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.GetString(2),
                    ReminderDate = reader.IsDBNull(3)
                        ? (DateTime?)null
                        : DateTime.Parse(reader.GetString(3)),
                    IsCompleted = reader.GetInt32(4) == 1,
                    CreatedAt = DateTime.Parse(reader.GetString(5))
                });
            }

            return list;
        }

        public void MarkCompleted(int id)
        {
            using var conn = new SqliteConnection(connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Tasks SET IsCompleted = 1 WHERE Id = $id;";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = new SqliteConnection(connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Tasks WHERE Id = $id;";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }

        public void UpdateReminder(int id, DateTime? reminder)
        {
            using var conn = new SqliteConnection(connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Tasks SET ReminderDate = $r WHERE Id = $id;";
            cmd.Parameters.AddWithValue(
                "$r",
                reminder.HasValue
                    ? (object)reminder.Value.ToString("o")
                    : DBNull.Value);
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
