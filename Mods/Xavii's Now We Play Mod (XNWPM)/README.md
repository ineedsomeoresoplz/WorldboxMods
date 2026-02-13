# Xavii's Now We Play Mod (XNWPM)

**Now you and your friends can all play God together with lightweight join-code matchmaking. The host auto-requests UPnP/NAT-PMP mapping, so in most routers nobody has to forward ports.**

## Overview
- Hosts a small TCP session on port **29101** with a shareable join code; the host automatically tries UPnP/NAT-PMP to open the port so friends can connect without manual router tweaks.
- The in-game overlay shows who is hosting, who is connected, and the join code, so everyone can stay synced to the same world.

## Installation
1. Copy the entire `Xavii's Now We Play Mod (XNWPM)` folder into `WorldboxMods/Mods`.
2. Launch Worldbox using NeoModLoader; the mod automatically attaches itself to the game object.
3. Press `H` while in-game (or open the menu) to keep the wireframe overlay visible for hosting/joining.

## Usage
### Hosting
1. Open the XNWPM control window (it docks in the top-right by default).
2. Adjust the **Max players** field (2–16) and click **Create session**.
3. The overlay shows **Join code: XXXX-XXXX...**. Click **Copy join code** and send it to friends (any chat works). The host auto-opens port 29101 when the router supports UPnP/NAT-PMP.
4. Keep Worldbox running; the host replays all approved God commands to every client to keep worlds deterministic.
5. When finished, click **Stop hosting** to close the session.

### Joining
1. Paste the host's **join code** into the **Join code** field and click **Join session**.
2. The overlay will update with the connection status and any debug messages.
3. Commands are sent to the host, who replays them for everybody; each client just executes what the host approves.
4. To leave a session, hit **Leave session**; the host can stay online for the remaining players.

### Connectivity notes
- UPnP/NAT-PMP is attempted automatically to open **29101/TCP**. If your router supports it, no manual forwarding is needed.
- If automatic mapping fails (symmetric NAT, UPnP off), the host may still need to forward **29101/TCP** once; the join code stays the same.
- Mobile hotspots often work because the modem does its own mapping; strict carrier-grade NATs may still block direct TCP sessions.
- Session ownership stays with the host; if the host stops the session, everyone is disconnected and needs a new join code.

### World consistency
- The lobby binds to one world identity (path or metadata). Everyone in the session must use that world; creating/loading a new one is blocked for the rest of the lobby.
- If the host tries to switch worlds mid-session, the lobby stops immediately so remotes never diverge. If a client attempts to load another world, they are ejected to keep the remaining players aligned.
- Because the host executes every God power and only forwards those deterministic commands to clients, random unit behavior never needs to be replayed twice—this avoids the "non-deterministic units" problem other attempts ran into.

## Troubleshooting
- **World identity mismatch**: if you see a log about a world change right after clicking join, it means the host switched worlds or you tried to load another map while in the lobby; reload the same world and rejoin.
- **Cannot connect**: ensure UPnP is enabled on the router or manually forward **29101/TCP** to the host and allow Worldbox through firewalls.
- **Host quits**: the session closes when the host stops; generate and share a fresh join code to resume.
- **Lag or dropped controls**: lower the max player count or close bandwidth-heavy apps; the host relays every command, so their connection is the bottleneck.
