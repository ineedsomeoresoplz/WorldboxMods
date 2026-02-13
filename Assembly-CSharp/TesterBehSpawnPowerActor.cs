// Decompiled with JetBrains decompiler
// Type: TesterBehSpawnPowerActor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class TesterBehSpawnPowerActor : TesterBehSpawnPower
{
  private int _limit;

  public TesterBehSpawnPowerActor(string pPower, int pLimit = -1)
    : base()
  {
    this._power = pPower;
    this._limit = pLimit;
  }

  public override BehResult execute(AutoTesterBot pObject)
  {
    if (this._limit > -1)
    {
      GodPower godPower = AssetManager.powers.get(this._power);
      if (AssetManager.actor_library.get(godPower.actor_asset_id).units.Count >= this._limit)
        return BehResult.Continue;
    }
    return base.execute(pObject);
  }
}
