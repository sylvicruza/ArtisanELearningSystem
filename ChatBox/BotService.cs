/*using BotSharp.Core.Abstractions;
using BotSharp.Core.Agents;
using BotSharp.Core.Engines;
using BotSharp.Core.Intents;
using BotSharp.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtisanELearningSystem.ChatBox
{
    public class BotService
    {
        private IAgentStorage _agentStorage;
        private IBotPlatform _platform;

        public BotService(IAgentStorage agentStorage, IBotPlatform platform)
        {
            _agentStorage = agentStorage;
            _platform = platform;
        }

        public async Task<string> GetBotResponse(string message)
        {
            // Load the bot agent
            var agent = await _agentStorage.LoadAgentByBotIdAsync("your-bot-id");

            // Create user session
            var session = new UserSession();

            // Process user input
            var userMessage = new MessageModel
            {
                Platform = "Web",
                Message = message,
                SessionId = session.SessionId
            };

            // Get bot response
            var response = await _platform.GetAgentResponseAsync(agent, userMessage, session);

            return response.Response.Speech;
        }
    }
}
*/