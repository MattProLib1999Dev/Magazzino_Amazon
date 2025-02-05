using Amazon;
using Amazon.AccessTokenComponent;
using Amazon.Appunti.Handlers.Abstract;
using Amazon.DAL.Handlers.PasswordHasher.Abstract;
using Amazon.DoubleOptInComponent.Abastract;
using Amazon.DoubleOptInComponent.Dummy;
using Amazon.Handlers;
using Amazon.Handlers.Abstratc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
AddLogging(builder.Logging, builder.Configuration);
// Add services to the container.
AddServices(builder.Services);
// Supponiamo che tu stia usando la tua interfaccia `IDatabase`



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Services.AddScoped<IProdottoService, ProdottoService>();
builder.Services.AddScoped<IProdottiRepository>();
builder.Services.AddScoped<IDatabase, FakeDatabase>();
builder.Services.AddSingleton<FakeDatabase>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProdottoService, ProdottoService>();

// Il logging viene configurato automaticamente nella maggior parte delle applicazioni ASP.NET Core,
// ma se non è configurato, aggiungilo:
builder.Services.AddLogging();



//amazan add cors 
builder.Services.AddCors(options => {
    options.AddPolicy(name: "AmazonDotnetApp", configurePolicy: policyBuilder => {
        policyBuilder.WithOrigins("http://localhost:5056");
        policyBuilder.AllowAnyMethod();
    });
});
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
 

var app = builder.Build();

try
{
    using (var serviceScope = app.Services.CreateScope())
    {
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        //await dbContext.Database.MigrateAsync();
        await dbContext.Database.EnsureCreatedAsync();
    }

    // Middleware pipeline configuration

    app.Run();
}
catch (Exception e)
{
    app.Logger.LogCritical(e, "An exception occurred during the service startup");
}
finally
{
    // Flush logs or else you lose very important exception logs.
    // if you use Serilog you can do it via
    // await Log.CloseAndFlushAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors();
}

app.Use((ctx, next) => {
    ctx.Response.Headers["Access-Control_Allow-Origin"] = "";
    return next();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AmazonDotnetApp");

app.MapControllers();

app.Run();

static void AddLogging(ILoggingBuilder logging, ConfigurationManager configuration)
{
    var logger = new LoggerConfiguration()
        .WriteTo.Console()
        .ReadFrom.Configuration(configuration)
        .Enrich.FromLogContext()
        .CreateLogger();
}

static void AddServices(IServiceCollection services)
{
    services.AddMemoryCache();
    services.AddSingleton<IAccountDataSource, FakeDatabase>();
    services.AddScoped<IAccountHandler, AccountHandlers>();
    services.AddScoped<IAccessTokenManager, DummyAccessToken>();
    services.AddSingleton<IDevelopparePassworHasher, PBKDF2_PasswordHasher>();
    services.AddScoped<IDoubleOptInManager, DummyDoubleOptIn>();
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));


}