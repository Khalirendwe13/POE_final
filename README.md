# CyberGuard — Cybersecurity Awareness Chatbot (PROG6221 POE)

A WinForms desktop chatbot that teaches cybersecurity awareness. Built across three
parts of PROG6221 (Programming 2A) and submitted as the Portfolio of Evidence.

## Features

### Part 1 — Console foundations (now integrated into the GUI)
- ASCII art banner, voice greeting on startup.
- Personalised greeting, name handling.
- Basic keyword-based responses for password safety, phishing, privacy,
  scams, malware, and safe browsing.
- Input validation, polished output.

### Part 2 — GUI + smarter conversation
- Windows Forms GUI (themed, dark mode).
- Keyword recognition with multiple random responses per topic.
- Conversational flow ("tell me more", "another tip").
- Memory and recall (name, favourite topic, last topic).
- Sentiment detection (worried, frustrated, curious).
- Topics list with double-click to ask.

### Part 3 / POE — Advanced features
- **Tabbed GUI** — Chat, Tasks, Quiz, Activity Log all in one window.
- **Task Assistant with Reminders** — add tasks (title, description,
  optional reminder date), mark complete, delete. Available both
  through the chat ("add task - enable 2FA") and the Tasks tab.
- **SQLite database** — every task is persisted to `cyberguard.db`
  (auto-created on first run). Full CRUD: add, read, update, delete.
- **Cybersecurity quiz mini-game** — 12 questions (MCQ + true/false),
  one at a time, immediate feedback with explanations, score tracking,
  final feedback message.
- **NLP simulation** — keyword + regex routing recognises phrasings like
  "remind me to update my password tomorrow", "what tasks do I have",
  "start the quiz", "show activity log", "mark task 3 done".
- **Activity Log** — all bot actions are recorded with timestamps.
  Shows the last 10 by default; "Show more" reveals full history.

## Project structure
```
ActivityLog.cs       In-memory log of bot actions (Part 3 Task 4).
ChatBot.cs           Central message processor, wires NLP + tasks + log.
MainForm.cs          GUI code-behind for all four tabs.
MainForm.Designer.cs WinForms designer (tabs, controls).
MemoryManager.cs     User name / favourite / last topic memory.
NlpRouter.cs         Keyword/regex intent parser (Part 3 Task 3).
Program.cs           Application entry point.
QuizGame.cs          Quiz question bank + scoring (Part 3 Task 2).
ResponseSystem.cs    Keyword-to-response dictionary (Part 2).
SentimentAnalyzer.cs Sentiment keyword detection (Part 2).
TaskItem.cs          Task DTO (Part 3 Task 1).
TaskRepository.cs    SQLite-backed CRUD for tasks (Part 3 Task 1/DB).
User.cs              User model.
```

## Build & run

Requires .NET 10 SDK on Windows (WinForms target).

```
dotnet build past_2.csproj
dotnet run --project past_2.csproj
```

On first launch the SQLite file `cyberguard.db` is created next to the
executable. Delete it to reset task storage.

## Usage tips
Chat commands you can try:
- `Add task - enable two-factor authentication`
- `Remind me to update my password tomorrow`
- `Show tasks`
- `Mark task 1 as done`
- `Delete task 2`
- `Start quiz` (also switches you to the Quiz tab)
- `Show activity log` / `What have you done for me?`

## Submission checklist
- [x] WinForms GUI (no console)
- [x] Integrates Parts 1 + 2 features
- [x] Task Assistant + Reminders
- [x] SQLite database with full CRUD
- [x] Quiz with 12 questions, MCQ + T/F, immediate feedback, scoring
- [x] NLP simulation via keyword + regex
- [x] Activity Log (last 10 + show more)
- [ ] 6+ Git commits
- [ ] 3+ release tags
- [ ] 8–10 min unlisted YouTube video with voice-over
