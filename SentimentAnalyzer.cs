namespace CyberGuardBot
{
    public class SentimentAnalyzer
    {
        public string DetectSentiment(string input)
        {
            input = input.ToLower();

            if (input.Contains("worried") ||
                input.Contains("scared") ||
                input.Contains("afraid"))
            {
                return "worried";
            }

            if (input.Contains("frustrated") ||
                input.Contains("angry"))
            {
                return "frustrated";
            }

            if (input.Contains("curious") ||
                input.Contains("interested"))
            {
                return "curious";
            }

            return "neutral";
        }
    }
}