using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using NimaTask.API.Middlewares;
using NimaTask.API.Middlewares.IPBlockService;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("Starting Application ...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();


    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please insert JWT with Bearer into field",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement {
   {
     new OpenApiSecurityScheme
     {
       Reference = new OpenApiReference
       {
         Type = ReferenceType.SecurityScheme,
         Id = "Bearer"
       }
      },
      new string[] { }
    }
});
    });

    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddTransient<IIPBlockingService, IPBlockingService>();
    builder.Services.AddScoped<IPBlockActionFilter>();

    var nlogdbcon = builder.Configuration.GetConnectionString("NLogDBContext");
    builder.Services.AddDbContext<NlogDbContext>(x => x.UseSqlServer(nlogdbcon));

    var nimataskdbcon = builder.Configuration.GetConnectionString("NimaTaskDBContext");
    builder.Services.AddDbContext<NimaTaskDbContext>(x => x.UseSqlServer(nimataskdbcon));
    builder.Services.AddHttpContextAccessor();

    builder.Services.Configure<CookiePolicyOptions>(options =>
    {
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
    });


    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.SaveToken = true;
        cfg.TokenValidationParameters = new TokenValidationParameters()
        {
            NameClaimType = ClaimTypes.NameIdentifier,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        };
    }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options => builder.Configuration.Bind(options));

    var app = builder.Build();

    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });

    app.UseMiddleware<IPMiddelware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //NimaTaskDbContextSeed seed = new NimaTaskDbContextSeed(builder.Services.BuildServiceProvider());
    //seed.Seed();

    app.UseHttpsRedirection();

    app.UseAuthorization();
    app.UseAuthentication();

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