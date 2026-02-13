// Decompiled with JetBrains decompiler
// Type: SpawnEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SpawnEffect : BaseEffect
{
  private string _event;
  private SpriteAnimation _animation;
  private bool _eventUsed;
  private WorldTile _tile;

  internal override void create()
  {
    base.create();
    this._animation = ((Component) this).GetComponent<SpriteAnimation>();
  }

  internal override void spawnOnTile(WorldTile pTile) => this.prepare(pTile);

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    if (this._eventUsed || this._animation.currentFrameIndex != 14)
      return;
    this._eventUsed = true;
    if (!(this._event == "crabzilla"))
      return;
    GodPower godPower = AssetManager.powers.get("crabzilla");
    World.world.units.createNewUnit(godPower.actor_asset_id, this._tile, pSpawnHeight: godPower.actor_spawn_height);
  }

  public void setEvent(string pEvent, WorldTile pTile)
  {
    this._tile = pTile;
    this._eventUsed = false;
    this._event = pEvent;
  }
}
