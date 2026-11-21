using ExhibitionService.ApplicationLib.Extensions;
using ExhibitionService.InfrastructureLib.Extensions;
using VidensVejen.BuildingBlocksLib.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddDatabaseDI(builder.Configuration);

builder.Services.AddHandlerDI();
builder.Services.AddRepositoryDI();
builder.Services.AddQueriesDI();

builder.Services.AddControllers();

var serviceName = builder.Environment.ApplicationName[..^4];

builder.Services.AddOpenApi(options =>
    options.WithCustomApiInfo(serviceName));

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.AddCustomScalar(serviceName);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
