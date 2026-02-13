// Decompiled with JetBrains decompiler
// Type: QuantumSpriteManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class QuantumSpriteManager
{
  public static float arrow_middle_current;
  public static float highlight_animation;
  private static float _enemy_anim_timer = 0.0f;
  private static bool _enemy_anim_timer_positive = true;
  private static bool _initiated = false;

  public static void update()
  {
    if (!QuantumSpriteManager._initiated)
    {
      QuantumSpriteManager._initiated = true;
      QuantumSpriteManager.createSystems();
    }
    Bench.bench("quantum_sprites_scale", "game_total");
    QuantumSpriteManager.updateScaleEffect();
    Bench.benchEnd("quantum_sprites_scale", "game_total");
    Bench.bench("quantum_sprites", "game_total");
    QuantumSpriteManager.updateArrowEffect();
    QuantumSpriteManager.updateEnemyEffect();
    QuantumSpriteManager.updateSystems();
    Bench.benchEnd("quantum_sprites", "game_total");
  }

  public static void hideAll()
  {
    foreach (QuantumSpriteAsset quantumSpriteAsset in AssetManager.quantum_sprites.list)
      quantumSpriteAsset.group_system?.clearFull();
  }

  private static void createSystems()
  {
    foreach (QuantumSpriteAsset pAsset in AssetManager.quantum_sprites.list)
    {
      QuantumSpriteGroupSystem spriteGroupSystem = new GameObject().AddComponent<QuantumSpriteGroupSystem>();
      spriteGroupSystem.create(pAsset);
      pAsset.group_system = spriteGroupSystem;
      pAsset.group_system.turn_off_renderer = pAsset.turn_off_renderer;
      if (Config.preload_quantum_sprites && pAsset.default_amount != 0)
      {
        for (int index = 0; index < pAsset.default_amount; ++index)
          spriteGroupSystem.getNext();
        spriteGroupSystem.clearFull();
      }
    }
  }

  private static void updateSystems()
  {
    bool flag = World.world.quality_changer.isLowRes();
    QuantumSpriteAsset[] array = AssetManager.quantum_sprites.getArray();
    int length = array.Length;
    for (int index = 0; index < length; ++index)
    {
      QuantumSpriteAsset pAsset = array[index];
      Bench.bench(pAsset.id, "quantum_sprites");
      pAsset.group_system.prepare();
      if (flag && !pAsset.render_map || !flag && !pAsset.render_gameplay)
      {
        pAsset.group_system.update(0.0f);
      }
      else
      {
        if (pAsset.debug_option != DebugOption.Nothing)
        {
          if (DebugConfig.isOn(pAsset.debug_option))
            pAsset.draw_call(pAsset);
        }
        else
          pAsset.draw_call(pAsset);
        pAsset.group_system.update(0.0f);
      }
      Bench.benchEnd(pAsset.id, "quantum_sprites", true, (long) pAsset.group_system.count_active_debug);
    }
  }

  private static void updateEnemyEffect()
  {
    if ((double) QuantumSpriteManager._enemy_anim_timer > 0.0)
    {
      QuantumSpriteManager._enemy_anim_timer -= Time.deltaTime;
    }
    else
    {
      QuantumSpriteManager._enemy_anim_timer = 0.02f;
      if (QuantumSpriteManager._enemy_anim_timer_positive)
      {
        ++QuantumSpriteManager.highlight_animation;
        if ((double) QuantumSpriteManager.highlight_animation <= 10.0)
          return;
        QuantumSpriteManager._enemy_anim_timer_positive = !QuantumSpriteManager._enemy_anim_timer_positive;
      }
      else
      {
        --QuantumSpriteManager.highlight_animation;
        if ((double) QuantumSpriteManager.highlight_animation != 0.0)
          return;
        QuantumSpriteManager._enemy_anim_timer_positive = !QuantumSpriteManager._enemy_anim_timer_positive;
      }
    }
  }

  private static void updateArrowEffect()
  {
    QuantumSpriteManager.arrow_middle_current += 10f * Time.deltaTime;
    if ((double) QuantumSpriteManager.arrow_middle_current < 5.0)
      return;
    QuantumSpriteManager.arrow_middle_current -= 5f;
  }

  private static void updateScaleEffect()
  {
    WorldTile worldTile = (WorldTile) null;
    if (InputHelpers.mouseSupported)
      worldTile = World.world.getMouseTilePos();
    City city1 = (City) null;
    Kingdom kingdom1 = (Kingdom) null;
    Alliance alliance1 = (Alliance) null;
    if (worldTile != null && !World.world.isBusyWithUI())
    {
      city1 = worldTile.zone.city;
      kingdom1 = worldTile.zone.city?.kingdom;
      alliance1 = kingdom1?.getAlliance();
    }
    float num = 0.1f;
    Kingdom kingdom2 = (Kingdom) null;
    if (World.world.isSelectedPower("relations"))
      kingdom2 = SelectedMetas.selected_kingdom;
    foreach (City city2 in (CoreSystemManager<City, CityData>) World.world.cities)
    {
      Kingdom kingdom3 = city2.kingdom;
      if (!kingdom3.wild)
      {
        bool flag = false;
        if (Zones.isBordersEnabled())
        {
          if (Zones.showCityZones())
          {
            if (city2 == city1 || kingdom3 == kingdom2)
            {
              flag = true;
              city2.setCursorOver();
            }
          }
          else if (Zones.showKingdomZones())
          {
            if (city2.kingdom == kingdom1 || kingdom3 == kingdom2)
            {
              flag = true;
              kingdom3.setCursorOver();
            }
          }
          else if (Zones.showAllianceZones() && kingdom1 != null)
          {
            Kingdom kingdom4 = city2.kingdom;
            if (kingdom4 != null)
            {
              Alliance alliance2 = kingdom4.getAlliance();
              if (kingdom4.hasAlliance())
              {
                if (alliance2 == alliance1 || kingdom3 == kingdom2)
                {
                  flag = true;
                  city2.setCursorOver();
                  kingdom3.setCursorOver();
                }
              }
              else if (city2.kingdom == kingdom1 || kingdom3 == kingdom2)
              {
                flag = true;
                city2.setCursorOver();
                kingdom3.setCursorOver();
              }
            }
            else
              continue;
          }
        }
        if (city2.isCursorOver())
          flag = true;
        if (city2.kingdom.isCursorOver())
          flag = true;
        if (city2.kingdom.hasAlliance() && city2.kingdom.getAlliance().isCursorOver())
          flag = true;
        if (flag)
          city2.mark_scale_effect += num;
        else
          city2.mark_scale_effect -= num;
        city2.mark_scale_effect = Mathf.Clamp(city2.mark_scale_effect, 0.5f, 0.75f);
      }
    }
  }
}
