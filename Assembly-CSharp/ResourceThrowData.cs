// Decompiled with JetBrains decompiler
// Type: ResourceThrowData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public readonly struct ResourceThrowData
{
  public readonly Vector2 position_start;
  public readonly Vector2 position_end;
  public readonly string resource_asset_id;
  public readonly int resource_amount;
  public readonly double start_time;
  public readonly double end_time;
  public readonly long building_target_id;
  public readonly float height;

  public ResourceThrowData(
    Vector2 pPositionStart,
    Vector2 pPositionEnd,
    float pDuration,
    string pResourceAssetId,
    int pResourceAmount,
    long pBuildingTargetId,
    float pHeight)
  {
    this.position_start = pPositionStart;
    this.position_end = pPositionEnd;
    this.resource_asset_id = pResourceAssetId;
    this.resource_amount = pResourceAmount;
    this.building_target_id = pBuildingTargetId;
    this.height = pHeight;
    this.start_time = World.world.getCurSessionTime();
    this.end_time = this.start_time + (double) pDuration;
  }

  public bool isFinished() => World.world.getCurSessionTime() >= this.end_time;

  public float getRatio()
  {
    return (float) ((World.world.getCurSessionTime() - this.start_time) / (this.end_time - this.start_time));
  }
}
