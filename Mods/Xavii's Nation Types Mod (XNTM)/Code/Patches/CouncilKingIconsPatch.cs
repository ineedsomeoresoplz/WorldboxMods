using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using XNTM.Code.Utils;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(QuantumSpriteLibrary), "drawLeaders")]
    public static class CouncilKingIconsPatch
    {
        private static readonly MethodInfo DrawQuantumSpriteMethod = AccessTools.Method(
            typeof(QuantumSpriteLibrary),
            "drawQuantumSprite",
            new[]
            {
                typeof(QuantumSpriteAsset),
                typeof(Vector3),
                typeof(WorldTile),
                typeof(Kingdom),
                typeof(City),
                typeof(BattleContainer),
                typeof(float),
                typeof(bool),
                typeof(float)
            });

        private static readonly Sprite KingSpriteNormal = SpriteTextureLoader.getSprite("civ/icons/minimap_king_normal");
        private static readonly Sprite KingSpriteAngry = SpriteTextureLoader.getSprite("civ/icons/minimap_king_angry");
        private static readonly Sprite KingSpriteSurprised = SpriteTextureLoader.getSprite("civ/icons/minimap_king_surprised");
        private static readonly Sprite KingSpriteHappy = SpriteTextureLoader.getSprite("civ/icons/minimap_king_happy");

        private static void Postfix()
        {
            if (!PlayerConfig.optionBoolEnabled("map_kings_leaders"))
                return;
            if (World.world == null || AssetManager.quantum_sprites == null || DrawQuantumSpriteMethod == null)
                return;

            QuantumSpriteAsset kingsAsset = AssetManager.quantum_sprites.get("kings");
            if (kingsAsset == null)
                return;

            foreach (Kingdom kingdom in World.world.kingdoms)
            {
                if (kingdom == null || !CouncilManager.IsCouncilNation(kingdom))
                    continue;

                ColorAsset kingdomColor = kingdom.getColor();
                if (kingdomColor == null)
                    continue;

                List<Actor> rulers = CouncilManager.GetRulers(kingdom);
                if (rulers == null || rulers.Count == 0)
                    continue;

                for (int i = 0; i < rulers.Count; i++)
                {
                    Actor actor = rulers[i];
                    if (actor == null || actor.isRekt() || actor.isInMagnet() || actor.isKing())
                        continue;
                    if (actor.current_zone == null || !actor.current_zone.visible)
                        continue;

                    Sprite stateSprite = GetKingStateSprite(actor, kingdom);
                    if (stateSprite == null)
                        continue;

                    Vector3 position = actor.current_position;
                    position.y -= 3f;

                    QuantumSprite marker = DrawQuantumSpriteMethod.Invoke(
                        null,
                        new object[] { kingsAsset, position, null, kingdom, actor.city, null, 1f, false, -1f }) as QuantumSprite;
                    if (marker == null)
                        continue;

                    marker.setSprite(DynamicSprites.getIcon(stateSprite, kingdomColor));
                }
            }
        }

        private static Sprite GetKingStateSprite(Actor actor, Kingdom kingdom)
        {
            if (actor.has_attack_target)
                return KingSpriteAngry;
            if (actor.hasPlot())
                return KingSpriteSurprised;
            if (kingdom.hasEnemies())
                return KingSpriteNormal;
            return KingSpriteHappy;
        }
    }
}
