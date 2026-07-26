using System.ComponentModel;
using Globomantics.Mcp.Server.Calendar;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;


[McpServerPromptType]
public class TimeOffPrompts
{
    [McpServerPrompt(Name = "suggest-time-off")]
    [Description("Suggests the best days for an employee to take time off based on their work calendar and planned absences.")]
    public static ChatMessage SuggestTimeOff(
        [Description("The employee's ID (e.g. EMP-001)")] string employeeId)
    {
        return new ChatMessage(ChatRole.User,
            $"I am employee {employeeId}. Based on my work calendar and any planned time off, " +
            $"suggest the best days for me to take time off. " +
            $"Consider upcoming holidays and try to maximize my time off by combining them with weekends.");
    }

    [McpServerPrompt(Name = "next-scheduled-holiday")]
    [Description("Helps an employee find the next scheduled holiday at their work location.")]
    public static IList<PromptMessage> NextScheduledHoliday(
        [Description("The work location (e.g. UnitedStates or India)")] string workLocation,
        [Description("The year to check (optional, defaults to current year)")] string year = "")
    {
        // Always treat as string and validate yourself
        int workYear = string.IsNullOrWhiteSpace(year)
            ? DateTime.Now.Year
            : int.TryParse(year, out var parsed) ? parsed : DateTime.Now.Year;

        var calendarJson = CalendarResources.WorkCalendarsResource();

        return new List<PromptMessage>
        {
            // Assistant instruction message
            new PromptMessage
            {
                Role = Role.Assistant,
                Content = new TextContentBlock
                {
                    Text = "I am an HR assistant helping you find upcoming holidays at your work location. " +
                           "I will use the work calendar to find the next scheduled holiday."
                }
            },
            // Embedded calendar resource
            new PromptMessage
            {
                Role = Role.User,
                    Content = new TextContentBlock
                    {
                        Text = calendarJson
                    }
            },
            // User question
            new PromptMessage
            {
                Role = Role.User,
                Content = new TextContentBlock
                {
                    Text = $"What is the next scheduled holiday for {workYear} at my work location: {workLocation}?"
                }
            }
        };
    }
}