using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PluralDemo;
using PluralDemo.DbContexts;
using PluralDemo.Services;
using Serilog;

// Create a new Logger
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("log/citylog.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Use Serilog to log on file
builder.Host.UseSerilog();

// Add services to the container.
// The Default accept is Json, then we accept Xml as well

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson()
.AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

// Depending on DEBUG/PROD we inject the according Mail Service
#if DEBUG
builder.Services.AddTransient<ISendMail, LocalMailService>();
#else
builder.Services.AddTransient<ISendMail, CloudMailService>();
#endif

// This was our initial DataStore
builder.Services.AddSingleton<CityDataStore>();

// Inject the DbContext
var connectionString = builder.Configuration.GetConnectionString("SQlServerConnection");
builder.Services.AddDbContext<CityInfoContext>(options => options.UseSqlServer(connectionString));

// Inject the repository
builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

