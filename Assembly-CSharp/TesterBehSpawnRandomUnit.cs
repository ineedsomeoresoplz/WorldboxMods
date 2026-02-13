// Decompiled with JetBrains decompiler
// Type: TesterBehSpawnRandomUnit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TesterBehSpawnRandomUnit : BehaviourActionTester
{
  private string[] assets;
  private int _amount;
  private string _location;
  public ActorAssetFilter filter_delegate;
  private static readonly List<string> printers = new List<string>()
  {
    "hexagon",
    "skull",
    "squares",
    "yinyang",
    "island1",
    "star",
    "heart",
    "diamond",
    "aliendrawing",
    "crater",
    "labyrinth",
    "spiral",
    "starfort",
    "code"
  };

  public TesterBehSpawnRandomUnit(int pAmount = 1, string pLocation = "random")
  {
    this._amount = pAmount;
    this._location = pLocation;
    this.filter_delegate = (ActorAssetFilter) (pActorAsset => !pActorAsset.isTemplateAsset() && pActorAsset.has_ai_system && !pActorAsset.is_boat && !pActorAsset.unit_other && !pActorAsset.special && !pActorAsset.id.Contains("zombie"));
  }

  public override BehResult execute(AutoTesterBot pObject)
  {
    if (this.assets == null)
    {
      using (ListPool<string> list = new ListPool<string>())
      {
        foreach (ActorAsset pAsset in AssetManager.actor_library.list)
        {
          if (this.filter_delegate(pAsset))
            list.Add(pAsset.id);
        }
        list.Shuffle<string>();
        this.assets = list.ToArray<string>();
      }
    }
    string random1 = this.assets.GetRandom<string>();
    TileZone random2 = BehaviourActionBase<AutoTesterBot>.world.zone_calculator.zones.GetRandom<TileZone>();
    for (int index = 0; index < this._amount; ++index)
    {
      WorldTile pTile = !(this._location == "tile_target") || pObject.beh_tile_target == null ? random2.tiles.GetRandom<WorldTile>() : pObject.beh_tile_target;
      Actor actor = BehaviourActionBase<AutoTesterBot>.world.units.spawnNewUnit(random1, pTile, pMiracleSpawn: true);
      if (actor == null)
        Debug.LogError((object) ("could not spawn " + random1));
      else if (random1 == "printer")
        actor.data.set("template", TesterBehSpawnRandomUnit.printers.GetRandom<string>());
    }
    return base.execute(pObject);
  }
}
