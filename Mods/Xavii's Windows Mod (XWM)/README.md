# Xavii's Windows Mod (XWM)
In-game UI studio plus a production runtime framework for loading, managing, animating, and persisting `.xwm` windows.

## Core Controls
- `F8`: toggle XWM Studio.
- `F7`: toggle Runtime Hub.
- `F6`: toggle visibility for all currently loaded runtime windows.
- `Ctrl+Shift+R`: reload all currently loaded runtime windows.
- `Ctrl+Shift+S`: force-save workspace runtime state.

## Studio
- Explorer, live preview, and properties workflow.
- Drag to move, Shift-drag to resize, Ctrl to snap.
- Undo/redo in UI and keybinds (`Ctrl+Z`, `Ctrl+Shift+Z`, `Ctrl+Y`).
- Color wheel property editing.
- Per-type property filtering.
- Export to `.xwm` into target mod `XWM` folder.

## Runtime Hub
- Cross-mod browser for every `.xwm` file found in `Mods/*/XWM`.
- One-click actions per file: `Load`, `Show`, `Hide`, `Reload`, `Destroy`, `Auto On/Off`.
- Mod-scoped bulk actions: `Prewarm Mod`, `Show Mod`, `Hide Mod`, `Unload Mod`.
- Live filter/search and loaded-only view.
- Autoload master toggle plus per-file autoload entries.

## Workspace Persistence
- Runtime state is auto-saved to:
  - `Mods/Xavii's Windows Mod (XWM)/XWM/workspace_state.json`
- Restored state includes:
  - visibility
  - position
  - size
  - opacity
  - scale
  - rotation
  - sibling order

## Autoload Profile
- Profile file:
  - `Mods/Xavii's Windows Mod (XWM)/XWM/autoload_profile.json`
- When enabled, entries are loaded automatically on startup.
- Per-file autoload can be toggled from Runtime Hub.

## Runtime Usage
```csharp
using UnityEngine;
using XaviiWindowsMod.API;

var handle = XwmFiles.GetOrLoad("com.your.mod", "window_name", show: true);

handle.ConnectButtonClick("PlayButton", () => Debug.Log("Play"));
handle.ConnectTyping("SearchBox", value => Debug.Log(value));
handle.ConnectKeyDown(KeyCode.Escape, () => handle.Hide());
handle.ConnectKeyCombo(KeyCode.K, () => Debug.Log("Ctrl+K"), KeyCode.LeftControl);

handle.SetText("TitleLabel", "Hello");
handle.SetFontType("TitleLabel", "Default");
handle.SetTextScaled("TitleLabel", true);
handle.SetTextWrapped("TitleLabel", true);
handle.SetPosition("PanelRoot", new Vector2(120, 80));
handle.SetTextAll("ResultItem", "Updated");
handle.SetInteractableAll("ResultItem", true);

var closeButton = handle.Find("#CloseButton");
var labels = handle.FindAll("@TextLabel");
var actionable = handle.FindAll("type:TextButton & active:true");

handle.Opacity = 0.95f;
handle.BringToFront();
```

## Tween/Animation API
```csharp
using UnityEngine;
using XaviiWindowsMod.API;

var handle = XwmFiles.GetOrLoad("com.your.mod", "window_name", show: true);

handle.FadeTo(0.0f, 0.25f, XwmEase.OutCubic, () => handle.Hide());
handle.MoveTo(new Vector2(260, 140), 0.35f, XwmEase.OutBack);
handle.ResizeTo(new Vector2(980, 640), 0.35f, XwmEase.OutCubic);
handle.ScaleTo(new Vector2(1.05f, 1.05f), 0.2f, XwmEase.OutQuad);

var title = handle.Get("TitleLabel");
title.MoveTo(new Vector2(24, 30), 0.2f, XwmEase.OutQuad);
title.FadeTo(1f, 0.2f, XwmEase.OutQuad);
```

## Selector Syntax
- `#MyId` exact element id
- `.MyName` exact element name
- `@TextButton` exact type
- `id:MyId`, `name:MyName`, `type:TextBox`, `t:Frame`
- `contains:Search` checks id/name/type substring
- `path:XWM_Runtime` checks hierarchy path substring
- `text:Submit` checks element text
- `active:true`, `visible:false`, `interactable:true`
- `has:image`, `has:text`, `has:button`, `has:input`
- `|` for OR groups, `&` (or space) for AND tokens

## File and Batch API
```csharp
using XaviiWindowsMod.API;

bool exists = XwmFiles.Exists("com.your.mod", "window_name");
string path = XwmFiles.ResolvePath("com.your.mod", "window_name");

var names = XwmFiles.ListFiles("com.your.mod");
var paths = XwmFiles.ListFilePaths("com.your.mod");
var mods = XwmFiles.ListModTargets();
var all = XwmFiles.ListAllFiles();

XwmFiles.PrewarmAll("com.your.mod");
XwmFiles.ShowByMod("com.your.mod");
XwmFiles.HideByMod("com.your.mod");
XwmFiles.DestroyByMod("com.your.mod");

XwmFiles.ReloadAllLoaded(true);
XwmFiles.CopyFile("com.your.mod", "window_name", "window_name_copy", overwrite: false);
XwmFiles.RenameFile("com.your.mod", "window_name_copy", "window_name_v2", overwrite: true);
```

## Integration Notes
- `.xwm` files live under `Mods/<Your Mod>/XWM/`.
- Runtime and studio UI are hosted under `XWM_Root` canvas.
- Coordinates are top-left oriented: positive X right, positive Y downward.
- `WindowSystem.CloseAll()` still controls primitive `WindowSystem` windows.
