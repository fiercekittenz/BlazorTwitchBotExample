// Blazor Twitch Bot Example by twitch.tv/fiercekittenz
using Microsoft.AspNetCore.SignalR;
using System;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace BlazorTwitchBotExample.Server
{
   /// <summary>
   /// This class represents a single Twitch connection and bot instance.
   /// This would be where you would put your code for your particular bot's needs.
   /// </summary>
   public class TwitchBot
   {
      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="twitchBotHub">Dependency injected reference to the TwitchBotHub.</param>
      public TwitchBot(IHubContext<TwitchBotHub> twitchBotHub)
      {
         _twitchBotHub = twitchBotHub;
      }

      /// <summary>
      /// Establishes the connection to the Twitch API.
      /// </summary>
      public void Connect(string botName, string botAccessToken, string channelName)
      {
         // Make sure all of the required credentials are available and valid!
         if (String.IsNullOrEmpty(botName) ||
             String.IsNullOrEmpty(botAccessToken) ||
             String.IsNullOrEmpty(channelName))
         {
            SendChatMessage("Unable to connect to Twitch. No credentials found.");
            return;
         }

         SendChatMessage("Connecting to Twitch...");

         // If the TwitchClient isn't null, then it could be connected. Disconnect all
         // event handlers and force the bot to disconnect from Twitch before
         // establishing a new connection.
         if (_twitchClient != null)
         {
            _twitchClient.OnConnected -= TwitchClient_OnConnected;
            _twitchClient.OnDisconnected -= TwitchClient_OnDisconnected;
            _twitchClient.OnMessageReceived -= TwitchClient_OnMessageReceived;
            _twitchClient.Disconnect();
         }

         // Setup the credentials with the provided information.
         ConnectionCredentials connectionCredentials = new ConnectionCredentials(botName, botAccessToken);

         // Next create the client options and instantiate a WebSocketClient using these options.
         var clientOptions = new ClientOptions
         {
            MessagesAllowedInPeriod = 750,
            ThrottlingPeriod = TimeSpan.FromSeconds(30)
         };
         WebSocketClient customClient = new WebSocketClient(clientOptions);

         // Create the TwitchClient instance with your preferences and websocket client.
         _twitchClient = new TwitchClient(customClient);

         // Technically, your TwitchClient instance can join more than one channel; however,
         // if you are performing a lot of tasks that are responsible for reading and
         // parsing chat messages, it would be more performant to have multiple bots and
         // TwitchClient connections.
         _twitchClient.Initialize(connectionCredentials, channelName);

         _twitchClient.OnConnected += TwitchClient_OnConnected;
         _twitchClient.OnDisconnected += TwitchClient_OnDisconnected;
         _twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
         _twitchClient.Connect();
      }

      /// <summary>
      /// Event handler for the TwitchClient OnMessageReceived event.
      /// </summary>
      private void TwitchClient_OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
      {
         // We want to take the contents from chat and format them and send them to the front-end via the hub.
         SendChatMessage($"{e.ChatMessage.DisplayName}: {e.ChatMessage.Message}");
      }

      /// <summary>
      /// Event handler for the TwitchClient OnDisconnected event.
      /// </summary>
      private void TwitchClient_OnDisconnected(object sender, TwitchLib.Communication.Events.OnDisconnectedEventArgs e)
      {
         SendChatMessage("Disconnected from Twitch.");
      }

      /// <summary>
      /// Event handler for the TwitchClient OnConnected event.
      /// </summary>
      private void TwitchClient_OnConnected(object sender, TwitchLib.Client.Events.OnConnectedArgs e)
      {
         SendChatMessage("Connected to Twitch!");
      }

      /// <summary>
      /// Sends the provided message to the hub.
      /// </summary>
      /// <param name="messageToSend">The message to send.</param>
      private void SendChatMessage(string messageToSend)
      {
         // You can further define who you will send the message to by creating
         // groups under Clients or target a specific client; however, this
         // example code is going to broadcast to all connected clients.
         _twitchBotHub.Clients.All.SendAsync("ChatMessage", messageToSend);
      }

      /// <summary>
      /// Connection to Twitch API.
      /// </summary>
      public TwitchClient _twitchClient { get; private set; } = null;

      /// <summary>
      /// Reference to the TwitchBotHub used for accessing connected clients.
      /// </summary>
      public IHubContext<TwitchBotHub> _twitchBotHub { get; private set; } = null;
   }
}
