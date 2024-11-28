using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationMS.Controllers
{
  [ApiController]
  [Route("[controller]/[action]")]
  public class DistributedLockController : ControllerBase
  {
    private readonly DaprClient _daprClient;
    private const string LOCK_STORE_NAME = "redis-lock";

    public DistributedLockController(DaprClient daprClient)
    {
      _daprClient = daprClient;
    }

    [HttpPost("lock")]
    public async Task<IActionResult> AcquireLock([FromBody] DistributedLockModel distributedLockModel)
    {
      var resourceId = "resource-123"; // Kilitlenecek kaynak
      var lockOwner = distributedLockModel.LockOwner; // Kilit sahibini tanımla
      var expiryInSeconds = 15; // Kilidin geçerlilik süresi

      var response = await _daprClient.Lock(
          LOCK_STORE_NAME,
          resourceId,
          lockOwner,
          expiryInSeconds);

      if (response.Success)
      {
        return Ok("Kilit alındı. Kilit sahibi: " + response.LockOwner);
      }
      return BadRequest("Kilit alınamadı");
    }

    [HttpPost("unlock")]
    public async Task<IActionResult> ReleaseLock([FromBody] DistributedLockModel distributedLockModel)
    {
      var resourceId = "resource-123"; // Kilitlenen kaynak
      var lockOwner = distributedLockModel.LockOwner; // Kilit sahibini tanımla

      var response = await _daprClient.Unlock(
          LOCK_STORE_NAME,
          resourceId,
          lockOwner);

      if (response.status == LockStatus.LockBelongsToOthers)
      {
        return Ok("Süreç başka servis üzerinden devam ediyor. İşlem yapamazsınız.");
      }
      else if (response.status == LockStatus.LockDoesNotExist)
      {
        return Ok("Bu işlem için bir kilit bulunamadı.");
      }
      else if (response.status == LockStatus.Success)
      {
        return Ok("Kilit başarıyla açıldı");
      }
      else if (response.status == LockStatus.InternalError)
      {
        return Ok("Bir hata oluştu");
      }

      return BadRequest("Kilit serbest bırakılamadı" + response.status.ToString());
    }

    public class DistributedLockModel
    {
      public string LockOwner { get; set; }
    }
  }
}