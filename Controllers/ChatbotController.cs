/*using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Builder;

namespace ArtisanELearningSystem.Controllers
{
    public class ChatbotController : Controller
    {
        private readonly IBotFrameworkHttpAdapter _adapter;
        private readonly IBot _bot;

        public ChatbotController(IBotFrameworkHttpAdapter adapter, IBot bot)
        {
            _adapter = adapter;
            _bot = bot;
        }

        [HttpPost]
        public async Task PostAsync(CancellationToken cancellationToken)
        {
            // Process the incoming activity using the bot
            await _adapter.ProcessAsync(Request, Response, _bot, cancellationToken);
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
*/