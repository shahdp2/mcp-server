using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using ModelContextProtocol;
using ModelContextProtocol.Server;

namespace Globomantics.Mcp.Server.Calendar;

[McpServerResourceType]
public class CalendarResources
{
    [McpServerResource(
        UriTemplate = "globomantics://hrm/calendars/work",
        Name = "work-calendars.json",
        Title = "Work Holiday Calendars",
        MimeType = "application/json")]
    [Description("Returns the holiday calendars for different work locations (United States and India).")]
    public static string WorkCalendarsResource()
    {
        var usCalendar = AnnualHolidayCalendar.CreateForYear(DateTime.Now.Year, WorkLocation.UnitedStates);
        var inCalendar = AnnualHolidayCalendar.CreateForYear(DateTime.Now.Year, WorkLocation.India);

        var workCalendarResource = new
        {
            US = usCalendar,
            IN = inCalendar
        };

        return JsonSerializer.Serialize(workCalendarResource, McpJsonUtilities.DefaultOptions);
    }
}

[JsonConverter(typeof(JsonStringEnumConverter<WorkLocation>))]
public enum WorkLocation
{
    UnitedStates,
    India
}