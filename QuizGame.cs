using System;
using System.Collections.Generic;

namespace CyberGuardBot
{
    // Cybersecurity quiz mini-game for Task 2.
    // Holds the question bank, tracks the user's score, and exposes
    // one-question-at-a-time progression for the GUI.
    public class QuizGame
    {
        private readonly List<QuizQuestion> questions;
        private int currentIndex = -1;
        private int score = 0;

        public int Score => score;
        public int Total => questions.Count;
        public bool Finished => currentIndex >= questions.Count;

        public QuizGame()
        {
            questions = BuildQuestions();
        }

        public void Start()
        {
            currentIndex = 0;
            score = 0;
        }

        public QuizQuestion? Current()
        {
            if (currentIndex < 0 || currentIndex >= questions.Count)
                return null;

            return questions[currentIndex];
        }

        // Returns true if the answer was correct.
        public bool Answer(int chosenIndex)
        {
            var q = Current();
            if (q == null) return false;

            bool correct = chosenIndex == q.CorrectIndex;
            if (correct) score++;

            currentIndex++;
            return correct;
        }

        public string FinalFeedback()
        {
            double pct = (double)score / questions.Count;

            if (pct >= 0.8)
                return $"Great job! You scored {score}/{questions.Count}. You're a cybersecurity pro!";

            if (pct >= 0.5)
                return $"Nice effort! You scored {score}/{questions.Count}. Keep brushing up on the topics.";

            return $"You scored {score}/{questions.Count}. Keep learning to stay safe online!";
        }

        private static List<QuizQuestion> BuildQuestions()
        {
            return new List<QuizQuestion>
            {
                new QuizQuestion(
                    "What should you do if you receive an email asking for your password?",
                    new[] { "Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it" },
                    2,
                    "Reporting phishing emails helps prevent scams and protects others."),

                new QuizQuestion(
                    "True or False: Using the same password for every site is safe if the password is strong.",
                    new[] { "True", "False" },
                    1,
                    "False. One breach exposes every account. Use unique passwords per site."),

                new QuizQuestion(
                    "Which of these makes the strongest password?",
                    new[] { "password123", "your birthday", "A long passphrase with symbols", "your pet's name" },
                    2,
                    "Long passphrases mixed with symbols are far harder to crack."),

                new QuizQuestion(
                    "Two-factor authentication (2FA) protects you by...",
                    new[] { "Requiring two passwords", "Adding a second verification step", "Encrypting your hard drive", "Hiding your IP" },
                    1,
                    "2FA adds a second step (a code, key, or prompt) so a stolen password isn't enough."),

                new QuizQuestion(
                    "True or False: HTTPS in a URL means the website is completely safe.",
                    new[] { "True", "False" },
                    1,
                    "False. HTTPS encrypts traffic but doesn't prove the site itself is trustworthy."),

                new QuizQuestion(
                    "A stranger calls claiming to be IT support and asks for your password. This is...",
                    new[] { "Normal", "Social engineering", "A bug report", "A phishing test you must pass" },
                    1,
                    "Social engineering. Real IT will never ask for your password."),

                new QuizQuestion(
                    "Which is the safest place to store many passwords?",
                    new[] { "A sticky note on your monitor", "A reused master password", "A reputable password manager", "Your browser autofill on a shared PC" },
                    2,
                    "A password manager generates and stores unique strong passwords behind one strong master password."),

                new QuizQuestion(
                    "True or False: Public Wi-Fi is safe for online banking as long as the site uses HTTPS.",
                    new[] { "True", "False" },
                    1,
                    "False. Use a VPN or mobile data for sensitive transactions on public Wi-Fi."),

                new QuizQuestion(
                    "You receive a USB stick in the mail you didn't order. You should...",
                    new[] { "Plug it in to see what's on it", "Plug it into a spare PC", "Discard it without plugging it in", "Mail it back" },
                    2,
                    "Unknown USB devices can carry malware. Don't plug them in."),

                new QuizQuestion(
                    "What's a sign an email might be phishing?",
                    new[] { "Urgent threats to close your account", "Misspelled sender domain", "Generic greetings like 'Dear customer'", "All of the above" },
                    3,
                    "Phishing emails often combine urgency, lookalike domains, and generic greetings."),

                new QuizQuestion(
                    "True or False: Software updates are mostly cosmetic and can be ignored.",
                    new[] { "True", "False" },
                    1,
                    "False. Updates frequently patch security vulnerabilities."),

                new QuizQuestion(
                    "Which permission should make you most cautious when installing a mobile app?",
                    new[] { "Access to the internet", "Access to your SMS and contacts for a flashlight app", "Access to storage for a camera app", "Access to vibration" },
                    1,
                    "A flashlight has no business reading SMS and contacts. Question excessive permissions."),
            };
        }
    }

    public class QuizQuestion
    {
        public string Prompt { get; }
        public string[] Options { get; }
        public int CorrectIndex { get; }
        public string Explanation { get; }

        public QuizQuestion(string prompt, string[] options, int correctIndex, string explanation)
        {
            Prompt = prompt;
            Options = options;
            CorrectIndex = correctIndex;
            Explanation = explanation;
        }
    }
}
