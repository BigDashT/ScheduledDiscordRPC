// ================================================
// MODELS.cs
//
// Plain-data model for the app. An AppConfig is the single object persisted as JSON to
// %APPDATA%\ScheduledDiscordRPC\config.json by ConfigManager, and is the in-memory source of
// truth MainForm reads from / writes to.
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A single Discord Rich Presence button (label + URL). Discord allows at most 2 per presence.
/// </summary>
public class ButtonConfig
{
    public string Label { get; set; } = "";
    public string Url { get; set; } = "";
}

/// <summary>
/// The Rich Presence fields sent to Discord when a schedule is active. Mirrors (a subset of)
/// DiscordRPC.RichPresence so it can be persisted as plain JSON independent of the RPC library's
/// own types.
/// </summary>
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

/// <summary>How often (if ever) a Schedule recurs.</summary>
public enum RecurrenceType
{
    None,
    Daily,
    Weekday,     // Every weekday (Mon-Fri)
    Weekly,
    Monthly
}

/// <summary>
/// Describes the recurrence rule for a Schedule. Not all fields apply to every RecurrenceType —
/// see MainForm.IsDateActive for how each combination is interpreted.
/// </summary>
public class RecurrencePattern
{
    public RecurrenceType Type { get; set; } = RecurrenceType.None;

    /// <summary>
    /// Repeat interval in units of the recurrence type (e.g. "every 2 weeks" for Weekly).
    /// Note: the current Edit Schedule dialog does not expose a control for this and always
    /// saves 1; the engine still honors other values if set programmatically or via a hand-edited
    /// config.json.
    /// </summary>
    public int Interval { get; set; } = 1;

    // Weekly / Weekday
    public List<DayOfWeek> DaysOfWeek { get; set; } = new();

    // Monthly
    public int? DayOfMonth { get; set; }
    public int? WeekOfMonth { get; set; }        // 1 = 1st, 2 = 2nd, 3 = 3rd, 4 = 4th, -1 = Last
    public DayOfWeek? DayOfWeekForMonth { get; set; }

    public DateTime? EndDate { get; set; }       // null = forever
}

/// <summary>
/// One scheduled Rich Presence entry: when it applies (Start/End + recurrence) and what to show
/// while it's active.
/// </summary>
public class Schedule
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "New Schedule";

    /// <summary>First occurrence's start. For recurring schedules, only the time-of-day (and, for
    /// Weekly, the day-of-week list) is significant — the date anchors when the pattern began.</summary>
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public bool IsAllDay { get; set; }

    public RecurrencePattern Recurrence { get; set; } = new RecurrencePattern();

    public RichPresenceConfig Presence { get; set; } = new();

    /// <summary>Short human-readable summary of the recurrence rule, shown in the schedule grid.</summary>
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

    /// <summary>Deep copy used when opening the Edit dialog, so cancelling never mutates the original.</summary>
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

/// <summary>Root object persisted to config.json — the entire app's state.</summary>
public class AppConfig
{
    public string ClientId { get; set; } = "";
    public List<Schedule> Schedules { get; set; } = new();
    public bool RunOnStartup { get; set; } = false;

    /// <summary>
    /// Presence to show when no schedule is currently active. Reserved for future use — there is
    /// currently no UI to set this, so it is always null in practice; MainForm already falls back
    /// to clearing the presence when both this and the active schedule are null.
    /// </summary>
    public RichPresenceConfig? DefaultPresence { get; set; }
}

/// <summary>
/// System.Text.Json source-generation context for AppConfig. Using this instead of the default
/// reflection-based (de)serialization avoids reflection at startup (faster, no metadata scanning)
/// and — importantly — keeps JSON (de)serialization trim-safe, since the README's publish
/// instructions enable "Trim unused assemblies" and the reflection-based serializer can silently
/// drop trimmed members in that configuration.
/// </summary>
[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(AppConfig))]
internal partial class AppConfigJsonContext : JsonSerializerContext
{
}
