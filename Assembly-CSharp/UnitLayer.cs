// Decompiled with JetBrains decompiler
// Type: UnitLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UnitLayer : MapLayer
{
  private List<WorldTile> prevTiles;
  private float interval = 0.1f;
  private Color32 dead = Color32.op_Implicit(new Color(0.0f, 0.0f, 0.0f, 0.5f));
  private Color32 _color_clear = Color32.op_Implicit(Color.clear);

  internal override void create()
  {
    this.dead = Color32.op_Implicit(Toolbox.makeColor("#393939"));
    this.prevTiles = new List<WorldTile>();
    base.create();
  }

  internal override void clear()
  {
    this.prevTiles.Clear();
    base.clear();
  }

  protected override void UpdateDirty(float pElapsed)
  {
    if (MapBox.isRenderGameplay())
      this.timer = 0.0f;
    else if ((double) this.timer > 0.0)
    {
      this.timer -= pElapsed;
    }
    else
    {
      this.timer = this.interval;
      for (int index = 0; index < this.prevTiles.Count; ++index)
        this.pixels[this.prevTiles[index].data.tile_id] = this._color_clear;
      this.prevTiles.Clear();
      bool flag1 = PlayerConfig.optionBoolEnabled("marks_boats");
      bool flag2 = Zones.showCultureZones();
      if (World.world.isAnyPowerSelected() && !Zones.isPowerForceMapMode())
        flag2 = false;
      bool flag3 = Zones.showClanZones();
      bool flag4 = Zones.showAllianceZones();
      List<Actor> simpleList = World.world.units.getSimpleList();
      for (int index = 0; index < simpleList.Count; ++index)
      {
        Actor actor = simpleList[index];
        if (!actor.asset.visible_on_minimap && actor.asset.color.HasValue && !actor.is_inside_building)
        {
          this.prevTiles.Add(actor.current_tile);
          if (!actor.isAlive())
            this.pixels[actor.current_tile.data.tile_id] = this.dead;
          else if (flag2)
          {
            if (actor.hasCulture())
              this.pixels[actor.current_tile.data.tile_id] = actor.culture.getColor().getColorUnit32();
          }
          else if (flag3)
          {
            if (actor.hasClan())
              this.pixels[actor.current_tile.data.tile_id] = actor.clan.getColor().getColorUnit32();
          }
          else
          {
            if (flag4)
            {
              Alliance alliance = World.world.alliances.get(actor.kingdom.data.allianceID);
              if (alliance != null)
              {
                this.pixels[actor.current_tile.data.tile_id] = alliance.getColor().getColorUnit32();
                continue;
              }
            }
            if ((actor.asset.is_boat || actor.isSapient()) && actor.hasKingdom() && actor.isKingdomCiv())
            {
              if (!flag1 || !actor.asset.draw_boat_mark)
                this.pixels[actor.current_tile.data.tile_id] = actor.kingdom.getColor().getColorUnit32();
            }
            else
              this.pixels[actor.current_tile.data.tile_id] = actor.asset.color.Value;
          }
        }
      }
      this.updatePixels();
    }
  }
}
