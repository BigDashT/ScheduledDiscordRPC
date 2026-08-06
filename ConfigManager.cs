// ================================================
// CONFIGMANAGER.cs
//
// Load/save for the single AppConfig persisted at %APPDATA%\ScheduledDiscordRPC\config.json.
using System.Text.Json;

public static class ConfigManager
{
    private static readonly string ConfigPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ScheduledDiscordRPC", "config.json");

    /// <summary>
    /// Loads AppConfig from disk. Returns a fresh, empty AppConfig if the file doesn't exist yet,
    /// or if it can't be read/parsed (e.g. corrupted by a crash or manual edit) — in the latter
    /// case the unreadable file is preserved under a ".bak" name first, so a later Save() doesn't
    /// silently overwrite the user's real schedules with an empty config, and so the file is still
    /// around to inspect or report.
    /// </summary>
    public static AppConfig Load()
    {
        if (!File.Exists(ConfigPath)) return new AppConfig();
        try
        {
            var json = File.ReadAllText(ConfigPath);
            return JsonSerializer.Deserialize(json, AppConfigJsonContext.Default.AppConfig) ?? new AppConfig();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Warning: failed to load config, backing up and starting fresh → {ex.Message}");
            BackupCorruptConfig();
            return new AppConfig();
        }
    }

    /// <summary>
    /// Saves AppConfig to disk. Writes to a temp file and swaps it into place atomically so an
    /// interruption mid-write (crash, power loss) can't leave a half-written, corrupt config.json.
    /// Failures are swallowed (after logging) rather than crashing this background app — the next
    /// successful save will catch up.
    /// </summary>
    public static void Save(AppConfig config)
    {
        try
        {
            var dir = Path.GetDirectoryName(ConfigPath)!;
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var json = JsonSerializer.Serialize(config, AppConfigJsonContext.Default.AppConfig);

            var tempPath = ConfigPath + ".tmp";
            File.WriteAllText(tempPath, json);
            File.Move(tempPath, ConfigPath, overwrite: true);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Warning: failed to save config → {ex.Message}");
        }
    }

    private static void BackupCorruptConfig()
    {
        try
        {
            var backupPath = ConfigPath + $".corrupt-{DateTime.Now:yyyyMMddHHmmss}.bak";
            File.Copy(ConfigPath, backupPath, overwrite: true);
        }
        catch
        {
            // Best-effort only — if we can't even back it up, there's nothing more we can safely
            // do here without risking losing more data.
        }
    }
}
