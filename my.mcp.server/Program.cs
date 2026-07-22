using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using my.mcp.server.Calender;
using my.mcp.server.Document;

var builder = Host.CreateEmptyApplicationBuilder(settings: null);

builder.Logging.ClearProviders();
builder.Logging.AddConsole(consoleLogOptions =>
{
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});

// Register mock services for DI
builder.Services.AddSingleton<IHrmAbsenceApi, MockHrmAbsenceApi>();
builder.Services.AddSingleton<IHrmDocumentService, MockHrmDocumentService>();

builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly()
    .WithResourcesFromAssembly()
    .WithPromptsFromAssembly();

var app = builder.Build();

await app.RunAsync();