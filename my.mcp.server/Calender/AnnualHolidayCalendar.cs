namespace Globomantics.Mcp.Server.Calendar;

public class AnnualHolidayCalendar
{
    public int Year { get; set; }
    public WorkLocation Location { get; set; }
    public List<Holiday> Holidays { get; set; } = new();

    public static AnnualHolidayCalendar CreateForYear(int year, WorkLocation location)
    {
        var holidays = location == WorkLocation.UnitedStates
            ? GetUsHolidays(year)
            : GetIndiaHolidays(year);

        return new AnnualHolidayCalendar
        {
            Year = year,
            Location = location,
            Holidays = holidays
        };
    }

    private static List<Holiday> GetUsHolidays(int year) => new()
    {
        new Holiday { Date = new DateOnly(year, 1, 1),  Name = "New Year's Day" },
        new Holiday { Date = new DateOnly(year, 7, 4),  Name = "Independence Day" },
        new Holiday { Date = new DateOnly(year, 11, 11), Name = "Veterans Day" },
        new Holiday { Date = new DateOnly(year, 12, 25), Name = "Christmas Day" }
    };

    private static List<Holiday> GetIndiaHolidays(int year) => new()
    {
        new Holiday { Date = new DateOnly(year, 1, 26),  Name = "Republic Day" },
        new Holiday { Date = new DateOnly(year, 8, 15),  Name = "Independence Day" },
        new Holiday { Date = new DateOnly(year, 10, 2),  Name = "Gandhi Jayanti" },
        new Holiday { Date = new DateOnly(year, 12, 25), Name = "Christmas Day" }
    };
}

public class Holiday
{
    public DateOnly Date { get; set; }
    public string Name { get; set; } = string.Empty;
}