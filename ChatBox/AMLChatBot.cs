/*using AIMLbot;

namespace ArtisanELearningSystem.ChatBox
{
    public class AIMLChatBot
    {
        private Bot _aimbot;
        private User _myUser;

        public AIMLChatBot(string userId)
        {
            _aimbot = new Bot();
            _myUser = new User(userId, _aimbot);
            Initialize();
        }

        // Loads all the AIML files in the AIML folder
        private void Initialize()
        {
            //_aimbot.loadSettings();
          //  _aimbot.isAcceptingUserInput = false;
         //   _aimbot.loadAIMLFromFiles();
          //  _aimbot.isAcceptingUserInput = true;
        }

        // Given an input string, finds a response using AIMLbot library
        public string GetOutput(string input)
        {
            Request request = new Request(input, _myUser, _aimbot);
            Result result = _aimbot.Chat(request);
            return result.Output;
        }
    }
}
*/