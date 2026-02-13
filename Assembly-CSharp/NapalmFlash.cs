// Decompiled with JetBrains decompiler
// Type: NapalmFlash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class NapalmFlash : BaseEffect
{
  private bool killing;
  private bool bombSpawned;

  internal void spawnFlash(WorldTile pTile)
  {
    this.tile = pTile;
    this.bombSpawned = false;
    this.killing = false;
    this.prepare(pTile, 0.1f);
  }

  public static bool napalmEffect(WorldTile pTile, string pPowerID)
  {
    pTile.startFire(true);
    return true;
  }

  private void Update()
  {
    if ((double) ((Component) this).transform.localScale.x < 1.0 && !this.killing)
    {
      Vector3 localScale = ((Component) this).transform.localScale;
      localScale.x += World.world.elapsed * 0.7f;
      if ((double) localScale.x >= 0.60000002384185791 && !this.bombSpawned)
      {
        this.bombSpawned = true;
        World.world.loopWithBrush(this.tile, Brush.get(12), new PowerActionWithID(NapalmFlash.napalmEffect));
      }
      if ((double) localScale.x >= 0.699999988079071)
      {
        localScale.x = 0.7f;
        this.killing = true;
      }
      localScale.y = localScale.x;
      ((Component) this).transform.localScale = localScale;
    }
    else
    {
      if (!this.killing)
        return;
      Vector3 localScale = ((Component) this).transform.localScale;
      localScale.x -= World.world.elapsed * 1.5f;
      localScale.y = localScale.x;
      if ((double) localScale.x <= 0.0)
      {
        localScale.x = 0.0f;
        this.kill();
      }
      ((Component) this).transform.localScale = localScale;
    }
  }
}
