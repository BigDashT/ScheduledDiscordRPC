
<p align="center">
<img src="https://i.imgur.com/2uFlMMT.png" width="100" height="100" border="10"/>
</p>

# Scheduled Discord RPC - RPC for Employed People!

**Create multiple rich presence profiles and have them displayed in Discord on a configurable, Microsoft Teams style scheduler!**

Runs in the background (system tray) and will automatically update your Discord rich presence based on set schedules. Use it to let people know you're busy at work, on holidays or free to do whatever!

This project uses components from [discord-rpc-csharp](https://github.com/Lachee/discord-rpc-csharp), which is licensed under MIT

---

## ✨ Features

- **Teams-like Scheduling**: Scheduler supports recurring events (daily, weekly, monthly, "every 3rd Tuesday", all-day, overnight, etc.), if you are employed the teams-like interface will be immediately familiar to you!
- **Full Rich Presence support**: Details, State, Large/Small Images, up to 2 buttons with URLs
- **System tray icon** with quick status and exit
- **Run on Windows startup** (optional)
- **Lightweight** — polls every 30 seconds only to avoid rate limits, very low CPU/memory usage (30MB at most)
- **Persistent config** stored in `%APPDATA%\ScheduledDiscordRPC\config.json`

---

## 🚀 How to Use

Before setting up, make sure you have a standalone Discord client (not in browser) and have enabled sharing your activity in Discord settings.
Share my activity must be enabled and set to an option other than "Do not share"

### 1. Download the latest `ScheduledDiscordRPC.zip` from the Releases page. Extract and run.
The app will run in the system tray, right click for menu.
### 2. Get your Discord Client ID (one-time)
1. Go to the [Discord Developer Portal](https://discord.com/developers/applications)
2. Create a **New Application** (name it whatever you want), note that your application name will be shown at the top of your presence.
3. Go to **Rich Presence → Visualizer** (or just copy the **Application ID** / Client ID)
4. Paste this ID into the app when it first starts (or in Settings later)
5. If you want to use any images, upload them to your application and give them a memorable name

### 3. Add Your First Schedule
1. Double-click the tray icon (or right-click → **Show**)
2. Click **Add Schedule**
3. **Schedule tab**:
   - Give it a name (e.g. "Work Hours")
   - Set Start / End time (use "All day" if needed)
   - Choose repeat pattern (Does not repeat / Daily / Weekly / Monthly / etc.)
4. **Presence tab**:
   - Fill in **Details** and **State**
   - Optional: Large/Small Image Key + Text (upload assets in Discord Developer Portal first), Large/Small Image Key must contain a value associated with an image you uploaded to your application
   - Optional: Button 1 / Button 2 text + URL
5. Click **Save**

The app will now automatically apply the correct Rich Presence whenever the current time matches any active schedule.

### Managing Schedules
- **Edit** → double-click a row or click **Edit**
- **Delete** → select row → **Delete**
- **Refresh** → forces immediate re-evaluation of schedules

### Tray Icon Behavior
- Hover: shows current status
- Right-click → **Show** / **Exit** / toggle auto-start

### Windows Startup
Check the box **"Run on Windows startup"** in the main window. The app adds itself to the registry (Current User\Run).

---

## 🛠️ Building from Source

### Prerequisites
- **Visual Studio 2022** (may work on other visual studio versions or similar IDEs, have not tested)
- **.NET 8.0 SDK** (installed automatically with Visual Studio)

### Step-by-step Instructions

1. **Clone the repository**
   ```bash
   gh repo clone BigDashT/ScheduledDiscordRPC

2. **Open the project**
   - Open Visual Studio 2022.
   - **File → Open → Project/Solution**
   - Select the `ScheduledDiscordRPC.sln` file in the cloned folder.

3. **Install the DiscordRichPresence NuGet package**
   - In **Solution Explorer**, right-click the project **`ScheduledDiscordRPC`** → **Manage NuGet Packages…**
   - Go to the **Browse** tab.
   - Search for **`DiscordRichPresence`**
   - Install the package by **Lachee** (the official one).

4. **Build the application**

   1. In Solution Explorer, right-click the project **`ScheduledDiscordRPC`** → **Publish…**
   2. In the “Which local target would you like to publish to?” dialog, select **Folder**
   3. Click **Next** (or **Finish**).
   4. Choose a destination folder (e.g. `C:\Publish\ScheduledDiscordRPC`) → click **Finish**.
   5. On the Publish profile page, click the **gear icon** (Show all settings) or the pencil icon to edit.
   6. In the settings:
      - Make sure **Configuration** is **Release**
      - **Deployment mode**: **Framework-Dependent**
      - **Target runtime**: pick your platform (e.g. `win-x86`)
      - Check **Produce single file**
      - Leave **Trim unused assemblies** *unchecked* — the .NET SDK does not support trimming
        Windows Forms apps and the build will fail with error `NETSDK1175` if you enable it
      - Do **not** switch Deployment mode to **Self-contained**: that bundles the entire .NET
        desktop runtime (WPF included, even though this app doesn't use it) and balloons the
        output to 100+ MB across several files instead of one small exe
   7. Click **Save**.
   8. Click the big **Publish** button.

   The final single-file executable will be in the folder you chose, named `ScheduledDiscordRPC.exe`
   (roughly 1-2 MB). Running it requires the [.NET 8 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)
   — Windows will offer to install it automatically the first time the app is run if it's missing.

---

## 🔧 Troubleshooting

- **Rich Presence not showing**: Make sure Discord is running and you entered the correct Client ID. Also make sure "Show activity" is enabled.
- **Status not changing**: Check that at least one schedule is active (green icon in grid).
- **Why don't my buttons show?** Known Discord bug, you can't see your own buttons. Others can see them.
- **It was working before, now it isn't suddenly!** You might have gotten a timeout from Discord because of connecting/changing presence a lot. Disconnect, wait 5-10 minutes, try to connect again. Restarting Discord might help too.
---

**Enjoy never manually updating your Discord status again!** 🎉

Any questions or feature requests? Just open an issue.

