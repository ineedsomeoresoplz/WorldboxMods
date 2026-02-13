// Decompiled with JetBrains decompiler
// Type: TesterBehResetTweens
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class TesterBehResetTweens : BehaviourActionTester
{
  public override BehResult execute(AutoTesterBot pObject)
  {
    Tooltip.tweenTime = 0.0f;
    PremiumUnlockAnimation.scaleTime = 0.0f;
    PremiumUnlockAnimation.delayTime = 0.0f;
    PowersTab.scale_time = 0.0f;
    PowersTab.buttonScaleTime = 0.0f;
    ButtonAnimation.scaleTime = 0.0f;
    ButtonResource.scaleTime = 0.0f;
    UiButtonHoverAnimation.scaleTime = 0.0f;
    return BehResult.Continue;
  }
}
