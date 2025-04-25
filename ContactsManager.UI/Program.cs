using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;
using Repositories;
using RepositoryContracts;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using CRUDEXAMPLE.Filters.ActionFilters;
using CRUDEXAMPLE;
using CRUDEXAMPLE.MiddleWare;

var builder = WebApplication.CreateBuilder(args);

//serilog
builder.Host.UseSerilog((HostBuilderContext context , IServiceProvider service , LoggerConfiguration logger) => {
    logger.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(service);
});
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
//builder.Logging.AddEventLog();
var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();
builder.Services.AddControllersWithViews(options => {
//options.Filters.Add<ResponseHeaderActionFilter>();
    options.Filters.Add(new ResponseHeaderActionFilter(logger) { Key = "Key-From-Global", Value = "Vlaue-From-Global", Order = 2 });
});
//builder.Services.AddHttpLogging(options => options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All); 

builder.Services.ConfigureServices(builder.Configuration , builder.Environment.EnvironmentName);

var app = builder.Build();
//app.Logger.LogDebug("From-Debug");
//app.Logger.LogInformation("From-Information");
//app.Logger.LogError("From-Error");
//app.Logger.LogWarning("From-Warning");
//app.Logger.LogCritical("From-Critical");

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHanelingMiddleware();
}
app.UseSerilogRequestLogging();

if (builder.Environment.IsEnvironment("test") == false)
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot",wkhtmltopdfRelativePath: "Rotativa");
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // reads Identity Cookie
app.UseAuthorization(); // Validates access permissions of the users 
app.MapControllers();
app.Run();

public partial class Program { }