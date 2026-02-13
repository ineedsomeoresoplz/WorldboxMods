// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehCityActorFindBuilding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
namespace ai.behaviours;

public class BehCityActorFindBuilding : BehCityActor
{
  private string _type;
  private string[] _types;
  private bool _only_free_tile;

  public BehCityActorFindBuilding(string pType, bool pFreeTile = true)
  {
    this._type = pType;
    this._only_free_tile = pFreeTile;
    if (!pType.Contains(","))
      return;
    this._types = pType.Split(',', StringSplitOptions.None);
  }

  public override BehResult execute(Actor pActor)
  {
    if (this._types != null)
      this._type = this._types.GetRandom<string>();
    pActor.beh_building_target = ActorTool.findNewBuildingTarget(pActor, this._type);
    return BehResult.Continue;
  }
}
