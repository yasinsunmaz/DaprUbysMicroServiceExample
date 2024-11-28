using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace ApplicationMS.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class PubSubController : ControllerBase
  {
    private readonly DaprClient _daprClient;
    private readonly string PubSubComponentName = "rabbitmq-pubsub";
    private readonly string PubSubTopicName = "subscribe-message";

    public PubSubController(DaprClient daprClient)
    {
      _daprClient = daprClient;
    }

    [HttpPost("publish-message")]
    public async Task<IActionResult> PublishMessage(string? message)
    {
      await PublishMessageUsingHttp(message);
      await PublishMessageUsingDaprSdk(message);
      return Ok("Mesaj gönderildi. Mesajınız = " + message);
    }

    private async Task PublishMessageUsingHttp(string? message)
    {
      var httpClient = new HttpClient();
      httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
      var jsonString = JsonSerializer.Serialize(new PubSubBase { Message = message });
      var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
      await httpClient.PostAsync($"http://localhost:3711/v1.0/publish/" + PubSubComponentName + "/" + PubSubTopicName, content);
    }

    private async Task PublishMessageUsingDaprSdk(string? message)
    {
      var messagePubSub = new PubSubBase { Message = message };
      await _daprClient.PublishEventAsync(PubSubComponentName, PubSubTopicName, messagePubSub);
    }

    public class PubSubBase
    {
      public string? Message { get; set; }
    }
  }
}