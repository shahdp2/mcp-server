namespace my.mcp.server.Calender;

public enum TimeOffDayType
{
    FullDay,
    HalfDay
}

public enum TimeOffRequestType
{
    Vacation,
    SickLeave,
    PersonalDay
}

public interface IHrmAbsenceApi
{
    Task<IEnumerable<TimeOffRequest>> GetTimeOffRequestsAsync(CancellationToken cancellationToken);
}

public class TimeOffRequest
{
    public string EmployeeId { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public TimeOffDayType DayType { get; set; }
    public TimeOffRequestType RequestType { get; set; }
}

// Mock implementation
public class MockHrmAbsenceApi : IHrmAbsenceApi
{
    public Task<IEnumerable<TimeOffRequest>> GetTimeOffRequestsAsync(CancellationToken cancellationToken)
    {
        var requests = new List<TimeOffRequest>
        {
            new() { EmployeeId = "emp-001", StartDate = new DateOnly(2026, 7, 4),  EndDate = new DateOnly(2026, 7, 4),  DayType = TimeOffDayType.FullDay, RequestType = TimeOffRequestType.Vacation },
            new() { EmployeeId = "emp-002", StartDate = new DateOnly(2026, 8, 15), EndDate = new DateOnly(2026, 8, 19), DayType = TimeOffDayType.FullDay, RequestType = TimeOffRequestType.Vacation }
        };
        return Task.FromResult<IEnumerable<TimeOffRequest>>(requests);
    }
}