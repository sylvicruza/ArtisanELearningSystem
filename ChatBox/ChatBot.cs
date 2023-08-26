using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ArtisanELearningSystem.ChatBox
{
    public class ChatBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            // Handle incoming message activity
            var text = turnContext.Activity.Text;
            var replyText = $"You said: '{text}'";
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText), cancellationToken);
        }

        protected override async Task OnConversationUpdateActivityAsync(ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            // Greet new members when they join the conversation
            foreach (var member in turnContext.Activity.MembersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text("Hello and welcome! I am your chatbot. How can I assist you?"), cancellationToken);
                }
            }
        }
    }
}
