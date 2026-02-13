// Decompiled with JetBrains decompiler
// Type: SpellLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;

#nullable disable
[ObfuscateLiterals]
public class SpellLibrary : AssetLibrary<SpellAsset>
{
  public override void init()
  {
    base.init();
    SpellAsset pAsset1 = new SpellAsset();
    pAsset1.id = "teleport";
    pAsset1.chance = 0.3f;
    pAsset1.cast_target = CastTarget.Himself;
    pAsset1.health_ratio = 0.6f;
    pAsset1.can_be_used_in_combat = true;
    pAsset1.cost_mana = 15;
    this.add(pAsset1);
    this.t.addDecision("random_teleport");
    this.t.addDecision("teleport_back_home");
    this.t.action = new AttackAction(ActionLibrary.teleportRandom);
    SpellAsset pAsset2 = new SpellAsset();
    pAsset2.id = "summon_lightning";
    pAsset2.chance = 0.1f;
    pAsset2.min_distance = 6f;
    pAsset2.cost_mana = 5;
    pAsset2.can_be_used_in_combat = true;
    this.add(pAsset2);
    this.t.action = new AttackAction(ActionLibrary.castLightning);
    SpellAsset pAsset3 = new SpellAsset();
    pAsset3.id = "summon_tornado";
    pAsset3.chance = 0.1f;
    pAsset3.min_distance = 6f;
    pAsset3.cost_mana = 10;
    pAsset3.can_be_used_in_combat = true;
    this.add(pAsset3);
    this.t.action = new AttackAction(ActionLibrary.castTornado);
    SpellAsset pAsset4 = new SpellAsset();
    pAsset4.id = "cast_curse";
    pAsset4.chance = 0.2f;
    pAsset4.min_distance = 4f;
    pAsset4.cast_entity = CastEntity.UnitsOnly;
    pAsset4.cost_mana = 3;
    pAsset4.can_be_used_in_combat = true;
    this.add(pAsset4);
    this.t.action = new AttackAction(ActionLibrary.castCurses);
    SpellAsset pAsset5 = new SpellAsset();
    pAsset5.id = "cast_fire";
    pAsset5.chance = 0.2f;
    pAsset5.min_distance = 3f;
    pAsset5.cast_entity = CastEntity.Both;
    pAsset5.can_be_used_in_combat = true;
    pAsset5.cost_mana = 3;
    this.add(pAsset5);
    this.t.addDecision("burn_tumors");
    this.t.action = new AttackAction(ActionLibrary.castFire);
    SpellAsset pAsset6 = new SpellAsset();
    pAsset6.id = "cast_silence";
    pAsset6.chance = 0.2f;
    pAsset6.min_distance = 6f;
    pAsset6.cast_entity = CastEntity.UnitsOnly;
    pAsset6.can_be_used_in_combat = true;
    pAsset6.cost_mana = 5;
    this.add(pAsset6);
    this.t.action = new AttackAction(ActionLibrary.castSpellSilence);
    SpellAsset pAsset7 = new SpellAsset();
    pAsset7.id = "cast_blood_rain";
    pAsset7.chance = 0.3f;
    pAsset7.min_distance = 0.0f;
    pAsset7.health_ratio = 0.9f;
    pAsset7.cast_target = CastTarget.Himself;
    pAsset7.cast_entity = CastEntity.UnitsOnly;
    pAsset7.can_be_used_in_combat = true;
    pAsset7.cost_mana = 2;
    this.add(pAsset7);
    this.t.addDecision("check_heal");
    this.t.action = new AttackAction(ActionLibrary.castBloodRain);
    SpellAsset pAsset8 = new SpellAsset();
    pAsset8.id = "cast_grass_seeds";
    pAsset8.chance = 0.1f;
    pAsset8.min_distance = 0.0f;
    pAsset8.cast_target = CastTarget.Region;
    pAsset8.cast_entity = CastEntity.Tile;
    pAsset8.cost_mana = 4;
    this.add(pAsset8);
    this.t.action = new AttackAction(ActionLibrary.castSpawnGrassSeeds);
    SpellAsset pAsset9 = new SpellAsset();
    pAsset9.id = "spawn_vegetation";
    pAsset9.chance = 0.1f;
    pAsset9.min_distance = 0.0f;
    pAsset9.cast_target = CastTarget.Region;
    pAsset9.cast_entity = CastEntity.Tile;
    pAsset9.cost_mana = 5;
    this.add(pAsset9);
    this.t.addDecision("spawn_fertilizer");
    this.t.action = new AttackAction(ActionLibrary.castSpawnFertilizer);
    SpellAsset pAsset10 = new SpellAsset();
    pAsset10.id = "spawn_skeleton";
    pAsset10.chance = 0.2f;
    pAsset10.min_distance = 0.0f;
    pAsset10.cast_target = CastTarget.Himself;
    pAsset10.can_be_used_in_combat = true;
    pAsset10.cost_mana = 10;
    this.add(pAsset10);
    this.t.addDecision("make_skeleton");
    this.t.action = new AttackAction(ActionLibrary.castSpawnSkeleton);
    SpellAsset pAsset11 = new SpellAsset();
    pAsset11.id = "cast_shield";
    pAsset11.chance = 0.2f;
    pAsset11.cast_target = CastTarget.Himself;
    pAsset11.can_be_used_in_combat = true;
    pAsset11.cost_mana = 5;
    this.add(pAsset11);
    this.t.action = new AttackAction(ActionLibrary.castShieldOnHimself);
    SpellAsset pAsset12 = new SpellAsset();
    pAsset12.id = "cast_cure";
    pAsset12.chance = 0.3f;
    pAsset12.min_distance = 0.0f;
    pAsset12.cast_target = CastTarget.Friendly;
    pAsset12.cast_entity = CastEntity.UnitsOnly;
    pAsset12.cost_mana = 3;
    this.add(pAsset12);
    this.t.addDecision("check_cure");
    this.t.action = new AttackAction(ActionLibrary.castCure);
  }

  public override void linkAssets()
  {
    base.linkAssets();
    foreach (SpellAsset spellAsset in this.list)
    {
      if (spellAsset.decision_ids != null)
      {
        spellAsset.decisions_assets = new DecisionAsset[spellAsset.decision_ids.Count];
        for (int index = 0; index < spellAsset.decision_ids.Count; ++index)
        {
          string decisionId = spellAsset.decision_ids[index];
          DecisionAsset decisionAsset = AssetManager.decisions_library.get(decisionId);
          spellAsset.decisions_assets[index] = decisionAsset;
        }
      }
    }
  }
}
