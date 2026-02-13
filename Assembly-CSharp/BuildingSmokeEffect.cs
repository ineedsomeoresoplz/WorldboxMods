// Decompiled with JetBrains decompiler
// Type: BuildingSmokeEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BuildingSmokeEffect : BaseBuildingComponent
{
  private float smokeTimer;
  private Vector3 centerTopVec;

  internal override void create(Building pBuilding)
  {
    base.create(pBuilding);
    Sprite sprite = this.building.asset.building_sprites.animation_data[0].main[0];
    this.centerTopVec = new Vector3();
    ref Vector3 local1 = ref this.centerTopVec;
    Vector2Int pos1 = this.building.current_tile.pos;
    double x = (double) ((Vector2Int) ref pos1).x;
    local1.x = (float) x;
    ref Vector3 local2 = ref this.centerTopVec;
    Vector2Int pos2 = this.building.current_tile.pos;
    double y = (double) ((Vector2Int) ref pos2).y;
    Rect rect = sprite.rect;
    double num1 = (double) ((Rect) ref rect).height * (double) this.building.asset.scale_base.y;
    double num2 = y + num1;
    local2.y = (float) num2;
  }

  public override void update(float pElapsed)
  {
    if (!this.building.asset.smoke || this.building.isUnderConstruction())
      return;
    if ((double) this.smokeTimer > 0.0)
    {
      this.smokeTimer -= Time.deltaTime;
    }
    else
    {
      this.smokeTimer = this.building.asset.smoke_interval;
      World.world.particles_smoke.spawn(this.centerTopVec.x, this.centerTopVec.y, true);
    }
  }
}
