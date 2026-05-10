// ================================================
// MODELS.cs
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

public class ButtonConfig
{
    public string Label { get; set; } = "";
    public string Url { get; set; } = "";
}

public class RichPresenceConfig
{
    public string Details { get; set; } = "";
    public string State { get; set; } = "";
    public string LargeImageKey { get; set; } = "";
    public string LargeImageText { get; set; } = "";
    public string SmallImageKey { get; set; } = "";
    public string SmallImageText { get; set; } = "";
    public List<ButtonConfig> Buttons { get; set; } = new();
}

public enum RecurrenceType
{
    None,
    Daily,
    Weekday,     // Every weekday (Mon-Fri)
    Weekly,
    Monthly
}

public class RecurrencePattern
{
    public RecurrenceType Type { get; set; } = RecurrenceType.None;
    public int Interval { get; set; } = 1;

    // Weekly / Weekday
    public List<DayOfWeek> DaysOfWeek { get; set; } = new();

    // Monthly
    public int? DayOfMonth { get; set; }
    public int? WeekOfMonth { get; set; }        // 1 = 1st, 2 = 2nd, 3 = 3rd, 4 = 4th, -1 = Last
    public DayOfWeek? DayOfWeekForMonth { get; set; }

    public DateTime? EndDate { get; set; }       // null = forever
}

public class Schedule
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "New Schedule";

    public DateTime Start { get; set; }          // First occurrence
    public DateTime End { get; set; }
    public bool IsAllDay { get; set; }

    public RecurrencePattern Recurrence { get; set; } = new RecurrencePattern();

    public RichPresenceConfig Presence { get; set; } = new();

    public string RecurrenceSummary
    {
        get
        {
            if (Recurrence.Type == RecurrenceType.None) return "Does not repeat";
            string freq = Recurrence.Type switch
            {
                RecurrenceType.Daily => "Daily",
                RecurrenceType.Weekday => "Every weekday",
                RecurrenceType.Weekly => "Weekly",
                RecurrenceType.Monthly => "Monthly",
                _ => ""
            };
            if (Recurrence.Interval > 1) freq = $"Every {Recurrence.Interval} {freq.ToLower()}";
            return freq;
        }
    }

    public Schedule Clone()
    {
        var clone = (Schedule)MemberwiseClone();
        clone.Recurrence = new RecurrencePattern
        {
            Type = Recurrence.Type,
            Interval = Recurrence.Interval,
            DaysOfWeek = new List<DayOfWeek>(Recurrence.DaysOfWeek),
            DayOfMonth = Recurrence.DayOfMonth,
            WeekOfMonth = Recurrence.WeekOfMonth,
            DayOfWeekForMonth = Recurrence.DayOfWeekForMonth,
            EndDate = Recurrence.EndDate
        };
        clone.Presence = new RichPresenceConfig
        {
            Details = Presence.Details,
            State = Presence.State,
            LargeImageKey = Presence.LargeImageKey,
            LargeImageText = Presence.LargeImageText,
            SmallImageKey = Presence.SmallImageKey,
            SmallImageText = Presence.SmallImageText,
            Buttons = Presence.Buttons.Select(b => new ButtonConfig { Label = b.Label, Url = b.Url }).ToList()
        };
        return clone;
    }
}

public class AppConfig
{
    public string ClientId { get; set; } = "";
    public List<Schedule> Schedules { get; set; } = new();
    public bool RunOnStartup { get; set; } = false;
    public RichPresenceConfig? DefaultPresence { get; set; }
}