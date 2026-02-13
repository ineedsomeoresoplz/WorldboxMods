# Xavii's Output Mod (XOM)

**Capture every WorldBox console message and archive it for per-session inspection.**

## What it does
- Hooks into `Application.logMessageReceivedThreaded` so every `Debug.Log`, `Warning`, and `Error` is mirrored into an externally readable file.
- Creates a `Logs` folder inside the mod directory and writes a new `console_YYYY-MM-DD_HH-MM-SS.log` file each time the mod loads.
- Marks the previously newest log file with a `LATEST_` prefix so you can quickly identify the most recent archived session.
- Leaves the streaming writer open for the lifetime of the mod so logs are appended in real time and flushed automatically.

## Installation
1. Place the `Xavii's Output Mod (XOM)` folder inside your WorldBox `Mods` directory.
2. Ensure the mod loader recognizes the mod by checking that `mod.json` is valid and `icon.png` exists.
3. Start WorldBox; the mod will begin logging automatically.

## Where to find logs
- Logs are saved in `Mods/Xavii's Output Mod (XOM)/Logs/`.
- File names follow the pattern `console_<timestamp>.log` (UTC timestamps using `yyyy-MM-dd_HH-mm-ss`).
- The file that was newest when the latest session started is renamed to `LATEST_console_<timestamp>.log` before a new logger is created.
- You can view the files with any text editor while WorldBox is running (the writer opens the file with `FileShare.Read`).

## Notes for modders
- The mod is written as a single `BasicMod<T>` derivative. No additional assets or dependencies are required.
- If you want to customize the log retention policy or add rotation, extend the `LogCapture` logic inside `Code/XOM.cs`.
