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
        [Description("Type of time off to request")] TimeOffType timeOffType,
        [Description("Whether this is a full day or half day")] DayType dayType,
        [Description("The dates to request off in YYYY-MM-DD format")] string[] dates,
        IHrmAbsenceApi absenceApi,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(employeeId))
            throw new McpException("Employee ID is required. Ask the employee for their ID.");

        if (dates == null || dates.Length == 0)
            throw new McpException("At least one date is required for a time off request.");

        // Validate dates
        var parsedDates = new List<DateOnly>();
        foreach (var date in dates)
        {
            if (!DateOnly.TryParse(date, out var parsed))
                throw new McpException($"Invalid date format: '{date}'. Use YYYY-MM-DD format.");
            parsedDates.Add(parsed);
        }

        // Map user-friendly enums to API values
        var requests = parsedDates.Select(d => new TimeOffRequest
        {
            EmployeeId = employeeId,
            StartDate = d,
            EndDate = d,
            DayType = dayType == DayType.FullDay ? TimeOffDayType.FullDay : TimeOffDayType.HalfDay,
            RequestType = MapTimeOffType(timeOffType)
        }).ToList();

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

    private static TimeOffRequestType MapTimeOffType(TimeOffType type) => type switch
    {
        TimeOffType.Vacation => TimeOffRequestType.Vacation,
        TimeOffType.SickLeave => TimeOffRequestType.SickLeave,
        TimeOffType.PersonalDay => TimeOffRequestType.PersonalDay,
        _ => throw new McpException($"Unknown time off type: {type}. Valid types are: Vacation, SickLeave, PersonalDay.")
    };
}

[JsonConverter(typeof(JsonStringEnumConverter<TimeOffType>))]
public enum TimeOffType
{
    Vacation,
    SickLeave,
    PersonalDay
}

[JsonConverter(typeof(JsonStringEnumConverter<DayType>))]
public enum DayType
{
    FullDay,
    HalfDay
}