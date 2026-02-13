// Decompiled with JetBrains decompiler
// Type: PlotManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class PlotManager : MetaSystemManager<Plot, PlotData>
{
  public PlotManager() => this.type_id = "plot";

  public override Plot loadObject(PlotData pData)
  {
    return AssetManager.plots_library.get(pData.plot_type_id) == null ? (Plot) null : base.loadObject(pData);
  }

  public override void startCollectHistoryData()
  {
  }

  public override void clearLastYearStats()
  {
  }

  public void cancelPlot(Plot pPlot) => pPlot.finishPlot(PlotState.Cancelled, (Actor) null);

  public bool tryStartPlot(Actor pActor, PlotAsset pPlotAsset, bool pForced = true)
  {
    bool flag;
    if (pPlotAsset.try_to_start_advanced != null)
    {
      flag = pPlotAsset.try_to_start_advanced(pActor, pPlotAsset, pForced);
    }
    else
    {
      World.world.plots.newPlot(pActor, pPlotAsset, pForced);
      flag = true;
    }
    return flag;
  }

  public Plot newPlot(Actor pAuthor, PlotAsset pAsset, bool pForced)
  {
    ++World.world.game_stats.data.plotsStarted;
    ++World.world.map_stats.plotsStarted;
    Plot plot = this.newObject();
    plot.newPlot(pAuthor, pAsset, pForced);
    plot.data.founder_name = pAuthor.getName();
    if (pForced)
      return plot;
    pAuthor.spendMoney(pAsset.money_cost);
    return plot;
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    this.updateProgress(pElapsed);
    this.updateAnimations(pElapsed);
    this.checkForgottenPlots();
  }

  private void updateProgress(float pElapsed)
  {
    if (World.world.isPaused())
      return;
    foreach (Plot plot in (CoreSystemManager<Plot, PlotData>) this)
      plot.updateProgress(pElapsed);
  }

  private void updateAnimations(float pElapsed)
  {
    foreach (Plot plot in (CoreSystemManager<Plot, PlotData>) this)
      plot.updateAnimations(pElapsed);
  }

  private void checkForgottenPlots()
  {
    foreach (Plot pPlot in (CoreSystemManager<Plot, PlotData>) this)
    {
      if (pPlot.last_update_progress != 0.0 && pPlot.isActive() && ((double) World.world.getWorldTimeElapsedSince(pPlot.last_update_progress) > (double) SimGlobals.m.forgotten_plot_time || pPlot.isAuthorDead()))
        this.cancelPlot(pPlot);
    }
  }

  public override void removeObject(Plot pPlot) => base.removeObject(pPlot);

  protected override void updateDirtyUnits()
  {
    List<Actor> unitsOnlyAlive = World.world.units.units_only_alive;
    for (int index = 0; index < unitsOnlyAlive.Count; ++index)
    {
      Actor pActor = unitsOnlyAlive[index];
      Plot plot = pActor.plot;
      if (plot != null && plot.isDirtyUnits())
        plot.listUnit(pActor);
    }
    foreach (Plot pPlot in (CoreSystemManager<Plot, PlotData>) this)
    {
      if (pPlot.isActive() && pPlot.isDirtyUnits() && pPlot.units.Count == 0)
        this.cancelPlot(pPlot);
    }
  }

  public bool isPlotTypeAlreadyRunning(Actor pActor, PlotAsset pPlotAsset)
  {
    foreach (Plot plot in (CoreSystemManager<Plot, PlotData>) this)
    {
      if (plot.isActive() && plot.isSameType(pPlotAsset))
        return true;
    }
    return false;
  }
}
