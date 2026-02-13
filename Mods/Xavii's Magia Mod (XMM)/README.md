# Xavii's Magia Mod

Adds elemental affinities, mana inheritance, and the sacred Orl trait to Worldbox.

## Features
- Eight elemental traits (Fire, Air, Water, Earth, Light, Dark, Orl, None) each supplying three combat-ready spells pulled from Worldbox's existing spell palette but Orl. Orl provides a single passive spell called Rem. Orl units are granted three automatic reincarnations, taking over a nearby child (or forcing a new baby) while keeping their old powers, favorite status, and stats.
- Orl reincarnations now crush the competition: every time an Orl loses a life it recalibrates to be twice as powerful as the strongest living actor in the world, and those doublings stack for every past death so a soul on its final body remains the apex force on the map.
- The Hero trait refuses to reincarnate, triples its stats every ten years, gains every elemental affinity, and can work with the Mentor specialty to seal the Demon Lord without killing him while logging its rise and fall.
- The Demon Lord trait mirrors Orl yet grants all affinities, an endless string of rebirths, and a 1-mana Judgement Day that costs 0.5 seconds of cooldown; Demon Lords and Heroes now log their milestones and trigger a Tsar-sized detonation on their deaths.
- Mentor and Sealed traits coordinate the Hero's ability to freeze a Demon Lord for eighty years, keeping him inactive, uncounted, and still aging until the seal breaks.
- No Element units spawn with only the `magic_none` trait, wield WorldBox's default fighting logic, and keep their max mana locked at zero so they never tap into Magia.
- Mana inheritance: organically born children begin with the sum of their parents' maximum mana pools, letting powerful families stay magical.
- Player-driven stacking: newborn and spawned actors receive exactly one element (except Orl; Orl is manually assigned), but you can manually assign extra magic traits later and the actor will gain every trait's spell list.
- Level-tuned offense: attack spells scale with actor level so veterans of each bloodline hit harder while newcomers still feel balanced.
- Spellcasting now shows Charging Magia before launch and Magia during the cast, every playable spell has its own charge timer (passives skip the delay), and combat-ready casters fire their spells whenever they're able.
- Spellcraft-inspired mastery: mage ranks now demand both level and kills, granting rising cast success while unlocking a fourth late-game spell for each core element (Burst, Whirl, Purity, Shockwave).

## Using the Mod
1. Install by dropping the entire `Xavii's Magia Mod (XMM)` folder into the `Mods` directory.
2. Launch Worldbox so the mod adds its traits, spells, and logging assets during startup.
3. Spawn or breed units to see magic inheritance in action; newborns inherit only one elemental trait from their parents while spawn units pick a random elemental trait (never Orl).
4. Open the trait editor and scroll to "Special - No Touch" to add additional elemental traits.

## Notes
- The `None` trait represents a magically neutral child that still participates in the magic system but grants no spells.
- Orl and Demon Lord reincarnations now keep clean current names while their previous incarnations are tracked in the unit's reincarnation history row.
- You can tweak `xmm_config.json` (created next to the mod DLL on first launch) to control behavior. The file now includes direct keys for `GodTimeAgeScale`, `GodTimeBabyMultiplier`, `GodTimeTeenMultiplier`, `GodTimeYoungAdultMultiplier`, `GodTimeAdultMultiplier`, `GodTimeElderMultiplier`, an editable `MageRankDefinition` list, and `AffinitySpawnRate` named values (`pyro`, `aero`, `aqua`, `terra`, `haro`, `barku`, `none`).

I'm looking for a UI artist and a programmer who already know how to work with and extend the existing WorldBox UI (since I don't). If you're interested in joining the project, DM me on Discord. This is NOT a paid project, but you will receive full credit for your work. Also, if you think some translations are too robotic or inaccurate, or a language you want isn't available, feel free to reach out if you'd like to help with translations as well.

Feel free to check out the official discord for this Mod and my many other endeavors. You can suggest features and report bugs here!
> https://discord.com/invite/Ckvd2SC3aM
