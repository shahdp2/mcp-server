using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using my.mcp.server.Calender;

[McpServerToolType]
public class GetTimeOffTool
{
    [McpServerTool]
    [Description("Gets time off information for an employee including work holiday calendars, planned time off, and their work location. Use this when an employee asks about holidays, time off, or scheduling.")]
    public static async Task<IList<Content>> GetTimeOff(
        [Description("The employee's ID (e.g. EMP-001)")] string employeeId,
        IHrmAbsenceApi absenceApi,
        CancellationToken cancellationToken)
    {
        var contents = new List<Content>();

        // Text content block with employee info
        var employeeInfo = new
        {
            employeeId,
            location = "India", // In production, fetched from backend API
            message = "Here is your time off information including work calendars and planned absences."
        };

        contents.Add(new TextContent
        {
            Text = JsonSerializer.Serialize(employeeInfo, McpJsonUtilities.DefaultOptions)
        });

        // Embedded resource: work calendars (reuses CalendarResources logic)
        var calendarJson = CalendarResources.WorkCalendarsResource();
        contents.Add(new EmbeddedResourceContent
        {
            Resource = new TextResourceContents
            {
                Uri = "globomantics://hrm/calendars/work",
                MimeType = "application/json",
                Text = calendarJson
            }
        });

        // Embedded resource: employee's planned time off
        var timeOffRequests = await absenceApi.GetTimeOffRequestsAsync(cancellationToken);
        var employeeTimeOff = timeOffRequests.Where(r => r.EmployeeId == employeeId).ToList();

        contents.Add(new EmbeddedResourceContent
        {
            Resource = new TextResourceContents
            {
                Uri = $"globomantics://hrm/timeoff/{employeeId}",
                MimeType = "application/json",
                Text = JsonSerializer.Serialize(employeeTimeOff, McpJsonUtilities.DefaultOptions)
            }
        });

        return contents;
    }
}