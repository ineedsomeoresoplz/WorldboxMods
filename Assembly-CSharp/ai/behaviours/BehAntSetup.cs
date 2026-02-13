// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehAntSetup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehAntSetup : BehaviourActionActor
{
  private static string[] _ant_tile_types = new string[8]
  {
    "deep_ocean",
    "close_ocean",
    "shallow_waters",
    "sand",
    "soil_low",
    "soil_high",
    "hills",
    "mountains"
  };

  public override BehResult execute(Actor pActor)
  {
    string pResult;
    pActor.data.get("tile_type1", out pResult);
    if (string.IsNullOrEmpty(pResult))
    {
      string randomTileType1 = BehAntSetup.getRandomTileType(pActor.current_tile?.Type?.id);
      string randomTileType2 = BehAntSetup.getRandomTileType(randomTileType1);
      pActor.data.set("tile_type1", randomTileType1);
      pActor.data.set("tile_type2", randomTileType2);
    }
    return BehResult.Continue;
  }

  public static string getRandomTileType(string pExclude = null)
  {
    string random = BehAntSetup._ant_tile_types.GetRandom<string>();
    while (random == pExclude)
      random = BehAntSetup._ant_tile_types.GetRandom<string>();
    return random;
  }
}
