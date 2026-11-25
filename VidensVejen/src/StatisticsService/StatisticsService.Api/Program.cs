using StatisticsService.ApplicationLib.Extensions;
using StatisticsService.InfrastructureLib.Extensions;
using VidensVejen.BuildingBlocksLib.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddDatabaseDI(builder.Configuration);
builder.Services.AddHandlerDI();
builder.Services.AddRepositoryDI();
builder.Services.AddQueryDI();
builder.Services.AddUnitOfWorkDI();
builder.Services.AddBackgroundWorkerDI();
builder.Services.AddInboxStrategiesDI();
builder.Services.AddDaprClient();

builder.Services.AddControllers();

var serviceName = builder.Environment.ApplicationName[..^4];

builder.Services.AddOpenApi(options =>
    options.WithCustomApiInfo(serviceName));

var app = builder.Build();
app.Services.ApplyDatabaseMigrations();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.AddCustomScalar(serviceName);
}

//app.UseHttpsRedirection();

app.UseCloudEvents();
app.UseAuthorization();

app.MapControllers();
app.MapSubscribeHandler();

app.Run();
