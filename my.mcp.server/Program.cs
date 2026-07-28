using my.mcp.server.Calender;
using my.mcp.server.Document;


var builder = WebApplication.CreateBuilder(args);

// Register mock services for DI
builder.Services.AddSingleton<IHrmAbsenceApi, MockHrmAbsenceApi>();
builder.Services.AddSingleton<IHrmDocumentService, MockHrmDocumentService>();

builder.Services.AddMcpServer()
    .WithHttpTransport(options =>
    {
        options.Stateless = true;
    })
    .WithToolsFromAssembly()
    .WithResourcesFromAssembly()
    .WithPromptsFromAssembly();

var app = builder.Build();


// Health check endpoint
app.MapGet("/health", () => "OK");

// MCP endpoint
app.MapMcp("/mcp");

app.Run();