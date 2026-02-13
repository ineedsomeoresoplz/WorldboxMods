// Decompiled with JetBrains decompiler
// Type: TesterBehRequireUnits
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class TesterBehRequireUnits : BehaviourActionTester
{
  private string _actor_asset_id;
  private int _amount;
  private RequireCondition _cond;

  public TesterBehRequireUnits(string pActorAssetID, int pAmount, RequireCondition pCondition = RequireCondition.AtLeast)
  {
    this._actor_asset_id = pActorAssetID;
    this._amount = pAmount;
    this._cond = pCondition;
  }

  public override BehResult execute(AutoTesterBot pObject)
  {
    int count = AssetManager.actor_library.get(this._actor_asset_id).units.Count;
    switch (this._cond)
    {
      case RequireCondition.AtLeast:
        if (count >= this._amount)
          break;
        goto default;
      case RequireCondition.AtMost:
        if (count <= this._amount)
          break;
        goto default;
      case RequireCondition.Exactly:
        if (count != this._amount)
          goto default;
        break;
      default:
        pObject.wait = 1.5f;
        return BehResult.Stop;
    }
    return BehResult.Continue;
  }
}
