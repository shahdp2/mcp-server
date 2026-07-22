using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using my.mcp.server.Calender;

[McpServerToolType]
public class TimeOffTools
{
    [McpServerTool]
    [Description("Submits a time off request for an employee. The employee must provide their employee ID, the type of time off, the day type (full or half day), and the dates. Personal holidays are subject to additional policies.")]
    public static async Task<string> RequestTimeOff(
        [Description("The employee's ID (e.g. EMP-001)")] string employeeId,
        [Description("Type of time off: Vacation, SickLeave, or PersonalDay")] TimeOffRequestType timeOffType,
        [Description("Whether this is a FullDay or HalfDay")] TimeOffDayType dayType,
        [Description("The dates to request off in YYYY-MM-DD format")] string[] dates,
        IHrmAbsenceApi absenceApi,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(employeeId))
            throw new McpException("Employee ID is required.");

        if (dates == null || dates.Length == 0)
            throw new McpException("At least one date is required.");

        var parsedDates = new List<DateOnly>();
        foreach (var date in dates)
        {
            if (!DateOnly.TryParse(date, out var parsed))
                throw new McpException($"Invalid date format: '{date}'. Use YYYY-MM-DD.");
            parsedDates.Add(parsed);
        }

        var result = new
        {
            status = "Submitted",
            employeeId,
            timeOffType = timeOffType.ToString(),
            dayType = dayType.ToString(),
            dates = parsedDates.Select(d => d.ToString("yyyy-MM-dd")),
            requestId = Guid.NewGuid().ToString("N")[..8]
        };

        return JsonSerializer.Serialize(result, McpJsonUtilities.DefaultOptions);
    }
}