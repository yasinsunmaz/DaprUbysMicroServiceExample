namespace PersonMS
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      builder.Services.AddControllers().AddDapr();
      builder.Services.AddDaprClient();
      builder.Services.AddHttpClient();

      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();

      var app = builder.Build();

      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      //app.UseHttpsRedirection(); //Olduğunda RabbtiMq bağlantı sorunu yaşanıyor
      app.UseAuthorization();
      app.UseCloudEvents();
      app.MapControllers();

      app.MapSubscribeHandler();
      // TODO: Enabling the Debugger
      //#if DEBUG
      //      Debugger.Launch();
      //#endif
      app.Run();
    }
  }
}