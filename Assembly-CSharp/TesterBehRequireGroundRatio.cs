// Decompiled with JetBrains decompiler
// Type: TesterBehRequireGroundRatio
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class TesterBehRequireGroundRatio : BehaviourActionTester
{
  private float _ratio;
  private RequireCondition _cond;

  public TesterBehRequireGroundRatio(float pRatio, RequireCondition pCondition = RequireCondition.AtLeast)
  {
    this._ratio = pRatio;
    this._cond = pCondition;
  }

  public override BehResult execute(AutoTesterBot pObject)
  {
    float num = BehaviourActionBase<AutoTesterBot>.world.islands_calculator.realGroundRatio();
    switch (this._cond)
    {
      case RequireCondition.AtLeast:
        if ((double) num >= (double) this._ratio)
          break;
        goto default;
      case RequireCondition.AtMost:
        if ((double) num <= (double) this._ratio)
          break;
        goto default;
      case RequireCondition.Exactly:
        if ((double) num != (double) this._ratio)
          goto default;
        break;
      default:
        pObject.wait = 1.5f;
        return BehResult.Stop;
    }
    return BehResult.Continue;
  }
}
