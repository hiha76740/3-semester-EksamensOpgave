using VidensVejen.Yarp.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var corsPolicyName = "BlazorCors";
builder.Services.AddVidensVejenCors(corsPolicyName);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("YARP"));

// call AddVidensVejenAuthentication

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapDefaultEndpoints();
}
else
{
    app.MapHealthChecks("/health");
}

app.UseCors(corsPolicyName);

// Call UseAuthentication
// Call UseAuthorization

app.MapReverseProxy();

app.Run();
