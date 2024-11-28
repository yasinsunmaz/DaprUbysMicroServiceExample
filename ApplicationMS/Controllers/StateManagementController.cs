using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationMS.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class StateManagementController : ControllerBase
  {
    private static string STATE_STORE_NAME = "statestore"; //Dapr'ın kullanacağı component,  component adı ile aynı olmalıdır.
    private readonly DaprClient _daprClient;

    public StateManagementController(DaprClient daprClient)
    {
      _daprClient = daprClient;
    }

    [HttpGet]
    [Route("statemanagement-add/{cacheKey}")]
    public async Task<IActionResult> StateManagementAdd(string cacheKey, string? value)
    {
      if (!string.IsNullOrWhiteSpace(cacheKey))
      {
        await _daprClient.SaveStateAsync(STATE_STORE_NAME, cacheKey, value);

        return Ok(cacheKey + " keyine '" + value + "' değeri ile cache eklendi");
      }
      else
      {
        return Ok("Cache keyi gereklidir");
      }
    }

    [HttpGet]
    [Route("statemanagement-get")]
    public async Task<IActionResult> StateManagementGet(string? cacheKey)
    {
      if (string.IsNullOrWhiteSpace(cacheKey))
      {
        return Ok("Cache keyi gereklidir");
      }

      string value = await _daprClient.GetStateAsync<string>(STATE_STORE_NAME, cacheKey);
      if (!string.IsNullOrWhiteSpace(value))
      {
        return Ok(value);
      }
      else
      {
        return Ok("Cache bulunamadı");
      }
    }

    [HttpGet]
    [Route("statemanagement-delete")]
    public async Task<IActionResult> StateManagementDelete(string cacheKey)
    {
      try
      {
        await _daprClient.DeleteStateAsync(STATE_STORE_NAME, cacheKey);
        return Ok("'" + cacheKey + "'" + " keyi cacheden temizlendi");
      }
      catch (Exception ex)
      {
        return Ok("Cache silinemedi. Hata: " + ex.Message);
      }
    }
  }
}