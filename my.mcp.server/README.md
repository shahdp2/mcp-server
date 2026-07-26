# MCP Server
A Model Context Protocol (MCP) server for the HR domain, built with C# and .NET 10.

## Features

- **Resources** — Work holiday calendars (US and India) and HR policy documents
- **Tools** — Plan time off, request time off, echo
- **Prompts** — Suggest time off, next scheduled holiday

## Installation

```json
{
  "servers": {
    "globomantics-mcp": {
      "command": "dotnet",
      "args": ["tool", "run", "DeepShah.Mcp.Server"]
    }
  }
}
```

## Built With

- C# / .NET 10
- ModelContextProtocol SDK 1.4.0
- Claude Desktop / VS Code
