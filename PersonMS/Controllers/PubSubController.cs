using Dapr;
using Microsoft.AspNetCore.Mvc;

namespace PersonMS.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class PubSubController : ControllerBase
  {
    [Topic("rabbitmq-pubsub", "subscribe-message")]
    [HttpPost("subscribe-pubsub-message")]
    public async Task<IActionResult> DaprGetPubSubMessage([FromBody] PubSubBase? message)
    {
      try
      {
        if (message == null || string.IsNullOrWhiteSpace(message.Message))
        {
          return BadRequest("The message cannot be null or empty.");
        }

        System.IO.File.AppendAllText("pubsub-result.txt", message.Message + Environment.NewLine);
        return Ok("Mesaj geldi = " + message?.Message);
      }
      catch (Exception ex)
      {
        return BadRequest("Mesaj işlenirken hata oluştu.");
      }
    }

    public class PubSubBase
    {
      public string? Message { get; set; }
    }
  }
}