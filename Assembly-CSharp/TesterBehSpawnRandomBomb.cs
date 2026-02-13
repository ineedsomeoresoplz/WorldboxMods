// Decompiled with JetBrains decompiler
// Type: TesterBehSpawnRandomBomb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class TesterBehSpawnRandomBomb : TesterBehSpawnPower
{
  internal static string[] events;

  public TesterBehSpawnRandomBomb()
    : base()
  {
    if (TesterBehSpawnRandomBomb.events != null)
      return;
    TesterBehSpawnRandomBomb.events = new string[6]
    {
      "bomb",
      "grenade",
      "napalm_bomb",
      "atomic_bomb",
      "antimatter_bomb",
      "czar_bomba"
    };
  }

  public override BehResult execute(AutoTesterBot pObject)
  {
    this._power = TesterBehSpawnRandomBomb.events.GetRandom<string>();
    return base.execute(pObject);
  }
}
