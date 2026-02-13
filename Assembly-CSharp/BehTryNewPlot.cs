// Decompiled with JetBrains decompiler
// Type: BehTryNewPlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System.Collections.Generic;
using UnityPools;

#nullable disable
public class BehTryNewPlot : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (pActor.hasPlot() || pActor.isFighting())
      return BehResult.Continue;
    using (ListPool<PlotAsset> listPool = new ListPool<PlotAsset>())
    {
      this.fillRandomPlots(pActor, listPool);
      if (listPool.Count == 0)
        return BehResult.Continue;
      this.startPlotFromTheList(pActor, listPool);
      return BehResult.Continue;
    }
  }

  private void fillRandomPlots(Actor pActor, ListPool<PlotAsset> pPotPlots)
  {
    this.fillPlotsToTry(pActor, AssetManager.plots_library.basic_plots, pPotPlots);
    if (pActor.hasReligion() && WorldLawLibrary.world_law_rites.isEnabled())
      this.fillPlotsToTry(pActor, pActor.religion.possible_rites, pPotPlots);
    pPotPlots.Shuffle<PlotAsset>();
  }

  private void fillPlotsToTry(
    Actor pActor,
    List<PlotAsset> pPlotList,
    ListPool<PlotAsset> pPotPossiblePlots)
  {
    for (int index = 0; index < pPlotList.Count; ++index)
    {
      PlotAsset pPlot = pPlotList[index];
      if (pPlot.checkIsPossible(pActor))
        pPotPossiblePlots.AddTimes<PlotAsset>(pPlot.pot_rate, pPlot);
    }
  }

  private void startPlotFromTheList(Actor pActor, ListPool<PlotAsset> pPotList)
  {
    HashSet<PlotAsset> plotAssetSet = UnsafeCollectionPool<HashSet<PlotAsset>, PlotAsset>.Get();
    for (int index = 0; index < pPotList.Count; ++index)
    {
      PlotAsset pPot = pPotList[index];
      if (!plotAssetSet.Contains(pPot))
      {
        if (!BehaviourActionBase<Actor>.world.plots.tryStartPlot(pActor, pPot))
          plotAssetSet.Add(pPot);
        else
          break;
      }
    }
    UnsafeCollectionPool<HashSet<PlotAsset>, PlotAsset>.Release(plotAssetSet);
  }
}
