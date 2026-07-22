using System.ComponentModel;
using System.Text.Json;
using Globomantics.Mcp.Server.Calendar;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using my.mcp.server.Calender;

[McpServerToolType]
public class GetTimeOffTool
{
    [McpServerTool(Name = "plan_time_off")]
    [Description("Plans and provides time off information for an employee including work holiday calendars, planned time off, and eligible absence types. Use this when an employee asks about holidays, time off, scheduling, or sabbatical eligibility.")]
    public static async Task<string> PlanTimeOff(
        [Description("The employee's ID (e.g. EMP-001)")] string employeeId,
        IHrmAbsenceApi absenceApi,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(employeeId))
            throw new McpException("Employee ID is required.");

        var timeOffRequests = await absenceApi.GetTimeOffRequestsAsync(cancellationToken);
        var employeeTimeOff = timeOffRequests
            .Where(r => r.EmployeeId == employeeId)
            .ToList();

        var calendarJson = CalendarResources.WorkCalendarsResource();

        // Include eligibility directly — don't rely on Claude reading a resource link
        var eligibleAbsenceTypes = new[]
        {
            new { type = "Vacation", eligible = true, notes = "Accrued based on tenure" },
            new { type = "SickLeave", eligible = true, notes = "Up to 10 days per year" },
            new { type = "PersonalDay", eligible = true, notes = "Up to 2 personal holidays, not subject to accrual" },
            new { type = "Sabbatical", eligible = false, notes = "Requires 7+ years of tenure. Section 5 of Time Off Policy." }
        };

        var result = new
        {
            employeeId,
            location = "India",
            eligibleAbsenceTypes,
            workCalendars = JsonSerializer.Deserialize<object>(calendarJson),
            plannedTimeOff = employeeTimeOff,
            policyDocumentUri = "globomantics://hrm/documents/time-off-policy"
        };

        return JsonSerializer.Serialize(result, McpJsonUtilities.DefaultOptions);
    }
}