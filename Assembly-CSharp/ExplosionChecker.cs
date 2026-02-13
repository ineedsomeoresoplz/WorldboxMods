// Decompiled with JetBrains decompiler
// Type: ExplosionChecker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ExplosionChecker
{
  private const float TIMER = 1f;
  private Dictionary<int, ExplosionMemoryData> data = new Dictionary<int, ExplosionMemoryData>();
  private List<int> _to_remove = new List<int>(16 /*0x10*/);

  public bool checkNearby(WorldTile pTile, int pRange)
  {
    int num = pRange * 10000000 + pTile.x * 1000 + pTile.y;
    if (this.data.ContainsKey(num) || this.isNearbyOthers(pTile, (float) (pRange / 3)))
      return true;
    this.add(num, pTile, pRange);
    this.updateNearbyTimers(pTile, (float) pRange);
    return false;
  }

  private void updateNearbyTimers(WorldTile pTile, float pRange)
  {
    float num1 = 1f;
    float num2 = pRange;
    float num3 = num1 + (float) (this.data.Count / 10);
    float num4 = num2 + (float) (this.data.Count / 5);
    float num5 = Mathf.Clamp(num3, 1f, 5f);
    float num6 = Mathf.Clamp(num4, pRange, pRange * 5f);
    foreach (int key in this.data.Keys)
    {
      ExplosionMemoryData explosionMemoryData = this.data[key];
      if ((double) Toolbox.Dist(pTile.x, pTile.y, explosionMemoryData.x, explosionMemoryData.y) < (double) num6)
        explosionMemoryData.timer = num5;
    }
  }

  private bool isNearbyOthers(WorldTile pTile, float pRange)
  {
    foreach (ExplosionMemoryData explosionMemoryData in this.data.Values)
    {
      if ((double) Toolbox.Dist(pTile.x, pTile.y, explosionMemoryData.x, explosionMemoryData.y) < (double) pRange)
        return true;
    }
    return false;
  }

  private void add(int pID, WorldTile pTile, int pRange)
  {
    ExplosionMemoryData explosionMemoryData = new ExplosionMemoryData();
    explosionMemoryData.range = pRange;
    explosionMemoryData.x = pTile.x;
    explosionMemoryData.y = pTile.y;
    float num = Mathf.Clamp(1f + (float) (this.data.Count / 10), 1f, 5f);
    explosionMemoryData.timer = num;
    this.data.Add(pID, explosionMemoryData);
  }

  public void update(float pElapsed)
  {
    Bench.bench("explosion_checker", "game_total");
    foreach (int key in this.data.Keys)
    {
      ExplosionMemoryData explosionMemoryData = this.data[key];
      explosionMemoryData.timer -= pElapsed;
      if ((double) explosionMemoryData.timer <= 0.0)
        this._to_remove.Add(key);
    }
    if (this._to_remove.Count > 0)
    {
      for (int index = 0; index < this._to_remove.Count; ++index)
        this.data.Remove(this._to_remove[index]);
      this._to_remove.Clear();
    }
    Bench.benchEnd("explosion_checker", "game_total");
  }

  public void clear() => this.data.Clear();

  public static void debug(DebugTool pTool)
  {
    pTool.setText("explosion_checker", (object) MapBox.instance.explosion_checker.data.Count);
  }
}
