# Xavii's Better Time Mod (XBTM)

## Overview
XBTM slows the simulation clock so that a full in-game day lasts five real-time minutes at the normal `x1` speed, keeps units and world events running at their usual pace, and restricts the era wheel to only Age of Hope (for daytime) and Age of Moon (for nighttime).

## Installation
Drop this folder into your Worldbox `Mods` directory. No additional dependencies are required beyond NeoModLoader.

## Usage
- The mod automatically activates when the world starts or when a save loads.
- Time speed controls (`x2`, `x3`, etc.) still work, but a day now takes `5 minutes / time scale multiplier` real seconds.
- Pausing or unpausing the world pauses the custom clock as usual.
- The era wheel and any era-changing UI will now only ever select Age of Hope (day) or Age of Moon (night).

## Notes
- The mod keeps unit and event timing untouched by only tampering with world time; every other system still uses the vanilla values, so behavior feels natural.
- Because only Hope and Moon are ever used, custom age playlists and random age advances are effectively disabled while this mod is active.
- Any other mods remotely based around ages besides Hope and Moon may conflict with XBTM.
- This mod is NOT sugguested to be used alongside XMM (another mod in the Xavii's Mod collection), as XMM's future updates (which will include time-based traits) will work faster with vanilla time. That said, you can still use them together, as it'll work the same.