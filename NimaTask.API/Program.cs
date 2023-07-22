var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("Starting Application ...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();


    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();


    var nlogdbcon = builder.Configuration.GetConnectionString("NLogDBContext");
    builder.Services.AddDbContext<NlogDbContext>(x => x.UseSqlServer(nlogdbcon));

    var nimataskdbcon = builder.Configuration.GetConnectionString("NimaTaskDBContext");

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    logger.Error(ex, "Application Error");
}
finally
{
    LogManager.Shutdown();
}