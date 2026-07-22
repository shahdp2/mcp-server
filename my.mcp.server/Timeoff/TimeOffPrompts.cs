using System.ComponentModel;
using Microsoft.Extensions.AI;
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
}