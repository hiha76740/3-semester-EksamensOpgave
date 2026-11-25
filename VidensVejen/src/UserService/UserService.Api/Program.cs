using UserService.ApplicationLib.Extensions;
using UserService.InfrastructureLib.Extensions;
using VidensVejen.BuildingBlocksLib.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDatabaseDI(builder.Configuration);
builder.Services.AddHandlerDI();
builder.Services.AddRepositoryDI();
builder.Services.AddQueryDI();
builder.Services.AddUnitOfWorkDI();
builder.Services.AddBackgroundWorkerDI();
builder.Services.AddEventPublisherDI();
builder.Services.AddInboxStrategiesDI();
builder.Services.AddDaprClient();

builder.Services.AddControllers();

var serviceName = builder.Environment.ApplicationName[..^4];

builder.Services.AddOpenApi(options =>
    options.WithCustomApiInfo(serviceName));

var app = builder.Build();
app.Services.InitializeAdminDatabase(app.Configuration);

app.MapDefaultEndpoints();

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
