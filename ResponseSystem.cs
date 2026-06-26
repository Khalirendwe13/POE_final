using System;
using System.Collections.Generic;

namespace CyberGuardBot
{
    public class ResponseSystem
    {
        private readonly Dictionary<string, List<string>> responses;
        private readonly Dictionary<string, List<string>> smallTalk;
        private readonly Random rand = new Random();

        public ResponseSystem()
        {
            responses = new Dictionary<string, List<string>>
            {
                ["password"] = new List<string>
                {
                    "Use strong passwords with letters, numbers, and symbols.",
                    "Never reuse passwords across multiple websites.",
                    "Use a password manager for better protection.",
                    "Create passwords with at least 12 characters."
                },
                ["phishing"] = new List<string>
                {
                    "Do not click suspicious links in emails.",
                    "Always verify the sender email address.",
                    "Phishing scams often pretend to be banks or companies.",
                    "Avoid downloading unknown email attachments."
                },
                ["privacy"] = new List<string>
                {
                    "Review your privacy settings regularly.",
                    "Avoid oversharing personal information online.",
                    "Enable two-factor authentication for accounts.",
                    "Use secure websites with HTTPS."
                },
                ["scam"] = new List<string>
                {
                    "Scammers often create urgency to pressure victims.",
                    "Never share banking details with strangers.",
                    "Verify suspicious messages before responding.",
                    "Be careful of offers that seem too good to be true."
                },
                ["malware"] = new List<string>
                {
                    "Keep your antivirus software updated.",
                    "Avoid downloading software from unknown websites.",
                    "Scan USB devices before opening files.",
                    "Update your operating system regularly."
                },
                ["safe browsing"] = new List<string>
                {
                    "Always look for HTTPS in websites.",
                    "Avoid public WiFi for sensitive transactions.",
                    "Do not download files from unknown websites.",
                    "Keep your browser updated."
                }
            };

            // Small talk: greetings, manners, identity. Each key maps to
            // a list of varied replies so the bot doesn't feel scripted.
            smallTalk = new Dictionary<string, List<string>>
            {
                ["greeting"] = new List<string>
                {
                    "Hey there! How can I help you stay safe online today?",
                    "Hi! Ready to talk cybersecurity?",
                    "Hello! What's on your mind?",
                    "Hey! Ask me about passwords, phishing, or any security topic."
                },
                ["how_are_you"] = new List<string>
                {
                    "I'm running secure and ready to help! How about you?",
                    "All systems green. What can I help you with?",
                    "Doing great, thanks for asking! Any cybersecurity questions?",
                    "I'm well! Got a security question for me?"
                },
                ["who_are_you"] = new List<string>
                {
                    "I'm CyberGuard, your cybersecurity awareness assistant.",
                    "I'm CyberGuard — I help people stay safe online.",
                    "Name's CyberGuard. I answer cybersecurity questions, manage tasks, and run quizzes."
                },
                ["thanks"] = new List<string>
                {
                    "Anytime! Stay safe out there.",
                    "You're welcome!",
                    "Happy to help. Let me know if you need anything else.",
                    "No problem!"
                },
                ["bye"] = new List<string>
                {
                    "Take care, and stay secure!",
                    "Goodbye! Lock your screen on the way out.",
                    "See you later — stay safe online!",
                    "Bye! Don't forget to use strong passwords."
                },
                ["compliment"] = new List<string>
                {
                    "Thanks! That means a lot.",
                    "Appreciate it! I'm just doing my job.",
                    "Glad I could help!"
                }
            };
        }

        public string GetResponse(string input)
        {
            foreach (var key in responses.Keys)
            {
                if (input.Contains(key))
                {
                    var options = responses[key];
                    return options[rand.Next(options.Count)];
                }
            }

            return SuggestNextStep();
        }

        // Returns a small-talk reply if the input is a greeting, "how are
        // you", thanks, etc. Returns null when the input isn't small talk
        // so the caller can fall through to topic detection.
        public string? GetSmallTalk(string input)
        {
            string? key = ClassifySmallTalk(input);
            if (key == null) return null;

            var options = smallTalk[key];
            return options[rand.Next(options.Count)];
        }

        private static string? ClassifySmallTalk(string text)
        {
            if (ContainsAny(text, "hello", "hi ", "hey", "yo ", "howdy") ||
                text == "hi" || text == "hey" || text == "yo")
                return "greeting";

            if (ContainsAny(text, "how are you", "how's it going", "how you doing",
                            "how are u", "you ok", "you good"))
                return "how_are_you";

            if (ContainsAny(text, "who are you", "what are you", "your name",
                            "what's your name", "whats your name"))
                return "who_are_you";

            if (ContainsAny(text, "thank you", "thanks", "thx", "appreciate it", "cheers"))
                return "thanks";

            if (ContainsAny(text, "bye", "goodbye", "see you", "see ya", "later",
                            "good night", "goodnight"))
                return "bye";

            if (ContainsAny(text, "good job", "well done", "nice job", "you're great",
                            "you are great", "love you", "you rock", "awesome bot"))
                return "compliment";

            return null;
        }

        private static bool ContainsAny(string text, params string[] needles)
        {
            foreach (var n in needles)
            {
                if (text.Contains(n)) return true;
            }
            return false;
        }

        private string SuggestNextStep()
        {
            string[] suggestions =
            {
                "I didn't quite catch that. Try asking about passwords, phishing, or privacy — " +
                "or say 'start quiz' or 'show tasks'.",

                "Not sure I follow. You could ask 'how do I make a strong password?', " +
                "say 'add task - enable 2FA', or 'start the quiz'.",

                "Hmm, can you rephrase? I know about phishing, scams, malware, safe browsing, " +
                "and I can manage tasks or run a quiz."
            };

            return suggestions[rand.Next(suggestions.Length)];
        }

        public string GetAnotherTip(string topic)
        {
            if (responses.ContainsKey(topic))
            {
                var options = responses[topic];
                return "Here is another tip:\n\n" + options[rand.Next(options.Count)];
            }
            return "Please mention a cybersecurity topic first.";
        }

        public string DetectTopic(string input)
        {
            foreach (var key in responses.Keys)
            {
                if (input.Contains(key)) return key;
            }
            return "";
        }

        public IEnumerable<string> GetTopics()
        {
            return responses.Keys;
        }
    }
}
