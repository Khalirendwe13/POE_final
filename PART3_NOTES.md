# Part 3 / POE — Cybersecurity Awareness Chatbot (100 Marks)

> **GUI ONLY.** A console app submitted for Part 3 scores **zero**. Must be WPF/WinForms
> with controls, graphics rendering, and styles. Build on the existing Part 1 + Part 2 GUI.
> Parts 1 & 2 features (dynamic responses, keyword recognition, sentiment detection) must still work.

## The 4 new features

### Task 1 — Task Assistant with Reminders (GUI)  [15 marks]
- Let users manage cybersecurity tasks, e.g. "Enable two-factor authentication".
- Each task has: **title, description, optional reminder** (e.g. "Remind me in 7 days").
- User can specify a date/timeframe for the reminder.
- **View & manage:** display all tasks (title, description, reminder); allow **delete** and
  **mark as completed**.
- Must integrate with the existing chatbot (not a separate disconnected screen).

Example:
```
User: Add task - Review privacy settings
Bot:  Task added with the description "Review account privacy settings...". Want a reminder?
User: Yes, remind me in 3 days.
Bot:  Got it! I'll remind you in 3 days.
```

### Task 1b — Database Integration (Task Storage)  [15 marks, separate rubric line]
- Integrate a **simple database (MySQL)** to store the user's tasks.
- Tasks must **save AND load** from the DB consistently.
- Full marks = robust, error-handled **CRUD** (add, read, mark complete, delete) that syncs
  between GUI and DB.

### Task 2 — Cybersecurity Mini-Game (Quiz)  [15 marks]
- **More than 10** questions (10+ for full marks).
- Topics: phishing, password safety, safe browsing, social engineering.
- Mix **multiple-choice AND true/false**.
- **One question at a time.**
- **Immediate feedback** after each answer (right/wrong + short explanation).
- **Track score**, show final score with feedback message
  ("Great job! You're a cybersecurity pro!" / "Keep learning to stay safe online!").
- Can use lists/dictionaries.

### Task 3 — NLP Simulation  [10 marks]
- Recognise differently-worded requests/commands.
- Use **keyword detection** ("task", "quiz", "reminder", "password", "phishing"...).
- Use simple string manipulation (`string.Contains()`) and optionally **regex**.
- e.g. "Add a task to enable 2FA" -> recognise "add task" + "2FA" -> create task.
- Minimise the "I didn't quite understand that" fallback.

Examples:
```
User: Remind me to update my password tomorrow.
Bot:  Reminder set for 'Update my password' on tomorrow's date.

User: Add a task to enable two-factor authentication.
Bot:  Task added: 'Enable two-factor authentication.' Want a reminder?

User: What have you done for me?
Bot:  Here's a summary of recent actions: ...
```

### Task 4 — Activity Log  [10 marks]
- Store a log (list/dictionary) of actions the bot took, with timestamps/short descriptions.
- Log: tasks added/updated/completed, reminders set, quiz started/completed, NLP-recognised actions.
- Commands "Show activity log" / "What have you done for me?" display the log.
- **Show only the last 5–10 actions**; optional "Show more" for full history.

## Rubric (POE = 100 marks)
| Criterion | Marks |
|---|---|
| Correct Submission (README, folder structure) | 5 |
| GitHub + Releases with Tags (6+ commits, **3+ release tags**) | 10 |
| Task Assistant with Reminders (GUI) | 15 |
| Task Assistant **Database Integration** (CRUD sync GUI<->DB) | 15 |
| Cybersecurity Mini-Game (Quiz) GUI (10+ Qs) | 15 |
| NLP Simulation GUI | 10 |
| Activity Log Feature GUI (limit 5–10, show more) | 10 |
| Combining Parts 1, 2 & 3 (cohesive integration) | 10 |
| Video Presentation (8–10 min, voice-over) | 10 |
| No GitHub used | −5% |

## Submission
- Submit **only the GitHub link** on ARC (source, README, all files).
- **YouTube unlisted link**, voice-over, explaining code structure/logic/techniques.
- GitHub: **6+ meaningful commits**, **3+ release tags** with version notes.
