using AuthService.Api.Extensions;
using AuthService.ApplicationLib.Extensions;
using AuthService.InfrastructureLib.Extensions;
using VidensVejen.BuildingBlocksLib.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddDaprClient();

builder.Services.AddHandlerDI();
builder.Services.AddDatabaseDI(builder.Configuration);
builder.Services.AddRepositoryDI();
builder.Services.AddQueriesDI();
builder.Services.AddUnitOfWorkDI();
builder.Services.AddServiceDI();
builder.Services.AddBackgroundWorkersDI();
builder.Services.AddInboxStrategiesDI();
builder.Services.AddEventPublisherDI();
builder.Services.AddAuthenticationDI();
builder.Services.AddInternalServicesDI();


builder.Services.AddVidensVejenAuthentication(builder);
builder.Services.AddVidensVejenAuthorization();

builder.Services.AddControllers();

var serviceName = builder.Environment.ApplicationName[..^4];

builder.Services.AddOpenApi(options =>
    options.WithCustomApiInfo(serviceName));

var app = builder.Build();
app.Services.ApplyDatabaseMigrations();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.AddCustomScalar(serviceName);
}

//app.UseHttpsRedirection();

app.UseCloudEvents();
app.UseAuthorization();
app.MapSubscribeHandler();
app.MapControllers();

app.Run();
