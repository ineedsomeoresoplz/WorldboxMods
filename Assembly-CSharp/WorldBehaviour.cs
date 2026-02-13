// Decompiled with JetBrains decompiler
// Type: WorldBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class WorldBehaviour
{
  private float _timer;
  private WorldBehaviourAsset _asset;

  internal void clear()
  {
    this._timer = this._asset.interval;
    if (this._asset.action_world_clear == null)
      return;
    this._asset.action_world_clear();
  }

  public WorldBehaviour(WorldBehaviourAsset pAsset)
  {
    this._asset = pAsset;
    this._timer = this._asset.interval;
  }

  public void timerClear() => this._timer = 0.0f;

  public void update(float pElapsed)
  {
    if (MapBox.isRenderMiniMap() && !this._asset.enabled_on_minimap || World.world.isPaused() && this._asset.stop_when_world_on_pause)
      return;
    if ((double) this._timer > 0.0)
    {
      this._timer -= pElapsed;
      if ((double) this._timer > 0.0)
        return;
    }
    this._timer += this._asset.interval + Randy.randomFloat(0.0f, this._asset.interval_random);
    this._asset.action();
  }
}
