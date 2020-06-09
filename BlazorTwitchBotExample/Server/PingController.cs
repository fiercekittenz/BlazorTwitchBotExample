// Blazor Twitch Bot Example by twitch.tv/fiercekittenz
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorTwitchBotExample.Server
{
   /// <summary>
   /// Ping Controller
   /// 
   /// Responsible for handling small requests to test connectivity between
   /// the front-end and back-end services. Responds simply with a 200OK
   /// to indicate that the connection is alive.
   /// </summary>

   [Route("ping")]
   public class PingController : Controller
   {
      public IActionResult Index()
      {
         return View();
      }

      [HttpGet]
      [Route("pong")]
      public ActionResult Pong()
      {
         return StatusCode(StatusCodes.Status200OK, "PONG");
      }
   }
}
