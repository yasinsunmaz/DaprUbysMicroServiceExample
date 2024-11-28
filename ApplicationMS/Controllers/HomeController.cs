using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationMS.Controllers
{
  [ApiController]
  [Route("[controller]/[action]")]
  public class HomeController : ControllerBase
  {
    private readonly DaprClient _daprClient;

    public HomeController(DaprClient daprClient)
    {
      _daprClient = daprClient;
    }

    [HttpGet]
    public async Task<IActionResult> DaprServiceInovationHttpClientMethod()
    {
      HttpClient httpClient = new HttpClient();
      var response = await httpClient.GetAsync(@"http://localhost:3711/v1.0/invoke/personsidecar/method/home/persons");

      return Ok(await response.Content.ReadAsStringAsync());
    }

    [HttpGet]
    public async Task<IActionResult> DaprServiceInvocationDaprSdkMethod()
    {
      var result = await _daprClient.InvokeMethodAsync<ReponseDto>(HttpMethod.Get, "personsidecar", @"/home/persons");
      return Ok(result);
    }
  }

  public class ReponseDto
  {
    public string? Message { get; set; }
  }
}