
namespace CyberGuardBot
{
    public class User
    {
        public string Name { get; set; } = "";

        public DateTime FirstInteraction { get; private set; }

        private int _questionsAsked;

        public int QuestionsAsked => _questionsAsked;

        public User()
        {
            FirstInteraction = DateTime.Now;
        }

        public void IncrementQuestionCount()
        {
            Interlocked.Increment(ref _questionsAsked);
        }
    }
}