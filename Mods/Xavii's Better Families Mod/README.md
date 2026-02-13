# Xavii's Better Families Mod

## Overview
Xavii's Better Families Mod rewires how families behave in WorldBox so lineages feel more believable, relationships are safer and smarter, and family identity matters beyond simple grouping.

## Implemented Family Expansions
1. Deep kinship detection now blocks close-relative pairings beyond vanilla parent/child checks.
2. Family-wide relatedness logic now recognizes sibling, grandparent, and shared-ancestor ties more consistently.
3. Courtship now enforces widow cooldowns to prevent instant rematching.
4. Courtship now applies stricter age-gap sanity limits for sapients.
5. Cross-city sapient courtship is constrained by island and compatibility checks.
6. Lover finding now ranks candidates by city, culture, language, family prestige, happiness, and age compatibility.
7. Couple formation now proactively unifies parents into coherent family structures.
8. Reproduction-time family assignment now uses smart family anchoring instead of always forcing a new family.
9. Family merging now considers combined size and dynamic family caps before consolidation.
10. Sapient family size limits now scale dynamically with cities, housing, mood, hunger, and prestige.
11. Family alpha selection now uses leadership scoring instead of pure age-first fallback.
12. Cadet branch creation now triggers under dynastic pressure for overcrowded sapient families.
13. Dependent children now migrate with cadet branch founders when new family branches form.
14. Child-follow behavior now uses guardian logic rather than random founder-only targeting.
15. Orphan guardians are tracked and refreshed continuously.
16. Orphaned children can be reintegrated into guardian family/city contexts.
17. Family defense now reacts to attacks in real time, not only after death events.
18. Family feud memory now persists hostility toward killers over time.
19. Birth aftermath now boosts sibling/grandparent emotional reactions and refreshes family prestige state.
20. Trait inheritance now adds stronger parent-linked inheritance passes and shared-trait reinforcement.
21. Family stability now suppresses or boosts births depending on hunger, homelessness, crowding, mood, and prestige.
22. Adults without families now attempt lineage restoration from parents or ancestor-family records.

## Installation
1. Drop `Xavii's Better Families Mod` into your WorldBox `Mods` directory.
2. Launch WorldBox through NeoModLoader.
3. Enable the mod if it is not already enabled.

## Compatibility Notes
- Built for `targetGameBuild: 719`.
- Uses Harmony patches on family, lover, birth, inheritance, and death flows.
- Mods that heavily overwrite the same methods may conflict.

## Latest Changes
- Added complete family-system overhaul runtime and Harmony patch set.
- Added dynastic family anchoring, dynamic cap logic, and alpha leadership scoring.
- Added courtship safeguards, widow cooldowns, and smarter partner selection.
- Added orphan guardianship, child-target improvements, and cadet branch pressure splitting.
- Added enhanced inheritance pass, family feud persistence, and attack-triggered family defense.
- Fixed a world-load crash by replacing direct island checks with null-safe same-island validation in family and guardian logic.
