# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project follows [Semantic Versioning](https://semver.org/).

## [1.1.0] - 2026-08-06

A bug-fix and hardening release — no changes to `config.json`'s format, so existing schedules
carry over with no action needed.

### Fixed
- **Small image / button changes could silently fail to reach Discord.** The check used to decide
  whether a presence update needed to be sent only compared Details, State, and the large image —
  changing only the small image or buttons between schedules could leave Discord showing stale
  data.
- **"Every N weeks" recurrence could skip valid days.** A leftover day-count check (instead of a
  week-count check) could incorrectly exclude a scheduled weekday when a weekly schedule used
  multiple days-of-week together with an interval greater than 1.
- **Overnight schedules on specific weekdays could cut off early.** A schedule spanning midnight
  (e.g. Friday 22:00 → Saturday 06:00) was evaluated against the wrong day during the early-morning
  portion, so it could stop applying right at midnight instead of continuing until its actual end
  time.
- **The tray/window icon silently fell back to a generic icon.** The custom icon was loaded from a
  loose file path that isn't reliably present next to the running executable; it's now extracted
  directly from the compiled exe instead.
- **A corrupted `config.json` could permanently wipe all schedules.** A read/parse failure used to
  silently fall back to an empty configuration, and the next save would then overwrite the
  (possibly still-recoverable) original file. Unreadable config files are now backed up before a
  fresh one is created.
- **Config saves were not crash-safe.** Writes now go to a temporary file and are swapped into
  place atomically, so an interruption mid-save (crash, power loss) can no longer leave a corrupt
  `config.json`.
- **Unhandled errors could make the app vanish from the tray with no explanation.** Added
  top-level error handling so unexpected failures are reported before the app closes, instead of
  disappearing silently.
- Fixed resource leaks: the tray icon, its context menu, the polling timer, the Discord RPC
  client, and the Windows Registry handle used for the startup toggle are now disposed properly.
  This also fixes the tray icon occasionally lingering ("ghosting") after the app closed.
- Presence updates no longer wait for a Discord reconnect to work — Discord commonly took up to
  30 seconds after startup to be reflected; it now applies immediately once connected.
- The app no longer reconnects to Discord every time the Client ID field loses focus if the value
  didn't actually change.

### Changed
- Config (de)serialization now uses a source-generated JSON layer instead of reflection-based
  `System.Text.Json`, which is both faster at startup and safe under the trimmed single-file
  publish described in the README (reflection-based JSON can silently drop members when trimmed).
- Removed unused/contradictory build configuration around `appicon.ico` in the project file.
- Removed two leftover empty event handlers with no effect.

### Added
- Extensive code comments and XML documentation across the codebase, particularly around the
  scheduling engine, to make the reasoning behind non-obvious logic (like the overnight-wrap
  handling above) clear for future changes.

## [1.0.0] - 2026-05-10

Initial release.

- Teams-style recurring schedule editor (daily, weekly, monthly, "every 3rd Tuesday", all-day,
  overnight, etc.)
- Full Discord Rich Presence support: Details, State, large/small images, up to 2 buttons
- System tray icon with status and quick exit
- Optional run-on-Windows-startup
- Lightweight 30-second polling to stay well within Discord rate limits
- Persistent config stored at `%APPDATA%\ScheduledDiscordRPC\config.json`
