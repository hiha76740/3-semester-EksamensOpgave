using ImageService.Api.Extensions;
using ImageService.ApplicationLib.Extensions;
using ImageService.InfrastructureLib.Extensions;
using VidensVejen.BuildingBlocksLib.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddDaprClient();

var themeImageDirectory = builder.SetThemeImageDirectory();

builder.Services.AddHandlerDI();
builder.Services.AddDatabaseDI(builder.Configuration);
builder.Services.AddRepositoryDI();
builder.Services.AddQueryDI();
builder.Services.AddStorageDI(themeImageDirectory);
builder.Services.AddInternalServicesDI();

builder.Services.AddSingleton(builder.Configuration);

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

app.AddStaticImageFiles(themeImageDirectory);

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
