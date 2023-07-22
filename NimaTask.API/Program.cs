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
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    var nlogdbcon = builder.Configuration.GetConnectionString("NLogDBContext");
    builder.Services.AddDbContext<NlogDbContext>(x => x.UseSqlServer(nlogdbcon));

    var nimataskdbcon = builder.Configuration.GetConnectionString("NimaTaskDBContext");
    builder.Services.AddDbContext<NimaTaskDbContext>(x => x.UseSqlServer(nimataskdbcon));

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //NimaTaskDbContextSeed seed = new NimaTaskDbContextSeed(builder.Services.BuildServiceProvider());
    //seed.Seed();

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