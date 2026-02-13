// Decompiled with JetBrains decompiler
// Type: BehCheckPlotBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehCheckPlotBase : BehaviourActionActor
{
  public override bool shouldRetry(Actor pActor)
  {
    if (base.shouldRetry(pActor))
      return true;
    if (pActor.hasPlot())
    {
      PlotRetryAction plotRetryAction = pActor.plot.getAsset().getPlotGroup().plot_retry_action;
      if ((plotRetryAction != null ? (plotRetryAction() ? 1 : 0) : 0) != 0)
        return true;
    }
    return false;
  }

  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.uses_plots = true;
    this.uses_clans = true;
  }

  public override BehResult execute(Actor pActor)
  {
    return !pActor.hasClan() || !pActor.plot.isActive() ? BehResult.Stop : BehResult.Continue;
  }

  protected bool isBasePlotCheckOk(Actor pActor)
  {
    if (!pActor.hasPlot() || !pActor.isKingdomCiv())
      return false;
    Plot plot = pActor.plot;
    if (!plot.isActive())
      return false;
    PlotAsset asset = plot.getAsset();
    if (!asset.isAllowedByWorldLaws())
      return false;
    PlotCheckerDelegate checkShouldContinue = asset.check_should_continue;
    return checkShouldContinue == null || checkShouldContinue(pActor);
  }
}
