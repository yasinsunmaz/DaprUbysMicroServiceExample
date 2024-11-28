using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace PersonMS.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class HomeController : ControllerBase
  {
    private readonly DaprClient _daprClient;

    public HomeController(DaprClient daprClient)
    {
      _daprClient = daprClient;
    }

    [HttpGet]
    [Route("persons")]
    public IActionResult RecivedPersonsHttpClient()
    {
      return Ok(new ReponseDto { Message = "PERSON MS --> Kişi listesi" });
    }
  }

  public class ReponseDto
  {
    public string Message { get; set; }
  }
}