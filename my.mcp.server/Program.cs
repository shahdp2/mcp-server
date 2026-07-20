using Globomantics.Mcp.Server.Calendar;
using Globomantics.Mcp.Server.Documents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


var builder = Host.CreateEmptyApplicationBuilder(settings: null);

builder.Logging.ClearProviders();
builder.Logging.AddConsole(consoleLogOptions =>
{
    // Configure all logs to go to stderr
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});

// builder.Services.AddMcpServer()
//     .WithStdioServerTransport()
//     .WithResources<CalendarResources>()
//     .WithResources<DocumentResources>();
//     .WithTools<EchoTool>();

builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly()
    .WithResourcesFromAssembly();


var app = builder.Build();

await app.RunAsync();