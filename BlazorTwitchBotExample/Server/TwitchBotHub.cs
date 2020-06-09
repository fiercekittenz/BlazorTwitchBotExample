// Blazor Twitch Bot Example by twitch.tv/fiercekittenz
using Microsoft.AspNetCore.SignalR;

namespace BlazorTwitchBotExample.Server
{
   /// <summary>
   /// Represents the back-end of the SignalR connection. This is called a "hub"
   /// and it can push and pull data (bidirectional) to and from the front-end.
   /// </summary>
   public class TwitchBotHub : Hub
   {
      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="botInstance">Dependency injected instance of the TwitchBot</param>
      public TwitchBotHub(TwitchBot botInstance)
      {
         BotInstance = botInstance;
      }

      /// <summary>
      /// Method that can be called from a SignalR connection on the front-end to establish
      /// a connection with Twitch using the provided credentials.
      /// </summary>
      /// <param name="botName">The name of the bot.</param>
      /// <param name="botAccessToken">The access (oauth) token for the bot account.</param>
      /// <param name="channelName">The name of the Twitch channel the bot should join.</param>
      public void ConnectToTwitch(string botName, string botAccessToken, string channelName)
      {
         if (BotInstance != null)
         {
            BotInstance.Connect(botName, botAccessToken, channelName);
         }
      }

      /// <summary>
      /// Reference to the running instance of the TwitchBot. 
      /// 
      /// Note that this is a singleton instance, but that is purely to keep this
      /// example simple and easy to follow. Should you want to have multiple streamers 
      /// use your web service, you will need to have multiple instances assigned to 
      /// each logged in individual.
      /// </summary>
      public TwitchBot BotInstance { get; private set; } = null;
   }
}
