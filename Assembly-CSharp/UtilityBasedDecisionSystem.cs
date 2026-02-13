// Decompiled with JetBrains decompiler
// Type: UtilityBasedDecisionSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class UtilityBasedDecisionSystem
{
  private const int MAX_POSSIBLE_DECISIONS = 1024 /*0x0400*/;
  private readonly DecisionAsset[] _actions = new DecisionAsset[1024 /*0x0400*/];
  private readonly float[] _factors = new float[1024 /*0x0400*/];
  private readonly float[] _chances = new float[1024 /*0x0400*/];
  public static Dictionary<string, int> debug_counter = new Dictionary<string, int>();
  private DecisionAsset[] _all_assets = new DecisionAsset[1024 /*0x0400*/];
  private int _counter_all_assets;
  private int _counter_possible;
  private int _highest_priority;
  private readonly DecisionAsset[][] _priority_array = new DecisionAsset[Enum.GetValues(typeof (NeuroLayer)).Length][];
  private readonly int[] _priority_array_counters = new int[Enum.GetValues(typeof (NeuroLayer)).Length];
  private bool _do_priority_levels;

  public UtilityBasedDecisionSystem()
  {
    int index = 0;
    for (int length = this._priority_array.Length; index < length; ++index)
    {
      this._priority_array[index] = new DecisionAsset[1024 /*0x0400*/];
      this._priority_array_counters[index] = 0;
    }
  }

  public DecisionAsset useOn(Actor pActor, bool pGameplay = true)
  {
    this.clear();
    ActorAsset asset = pActor.asset;
    this._do_priority_levels = !pActor.isAbleToSkipPriorityLevels() || Randy.randomChance(0.8f);
    this.registerBasicDecisionLists(pActor, pGameplay);
    if (asset.hasDecisions())
      this.registerDecisionArray(pActor, asset.getDecisions(), asset.decisions_counter, pGameplay);
    if (pActor.decisions_counter > 0)
      this.registerDecisionArray(pActor, pActor.decisions, pActor.decisions_counter, pGameplay);
    this.calculateFactors(pActor);
    if (this._counter_possible == 0)
      return (DecisionAsset) null;
    if (!pGameplay)
      this.calculateChances();
    DecisionAsset pAsset = this.chooseBestAction();
    if (pGameplay)
      pActor.setDecisionCooldown(pAsset);
    return pAsset;
  }

  private void registerBasicDecisionLists(Actor pActor, bool pGameplay)
  {
    if (pActor.asset.is_boat)
      return;
    DecisionsLibrary decisionsLibrary = AssetManager.decisions_library;
    if (pActor.isAnimal())
      this.registerDecisionArray(pActor, decisionsLibrary.list_only_animal, pGameplay: pGameplay);
    else if (pActor.isKingdomCiv())
    {
      this.registerDecisionArray(pActor, decisionsLibrary.list_only_civ, pGameplay: pGameplay);
      if (pActor.hasCity())
        this.registerDecisionArray(pActor, decisionsLibrary.list_only_city, pGameplay: pGameplay);
    }
    if (pActor.isBaby())
      this.registerDecisionArray(pActor, decisionsLibrary.list_only_children, pGameplay: pGameplay);
    this.registerDecisionArray(pActor, decisionsLibrary.list_others, pGameplay: pGameplay);
  }

  private void registerDecisionArray(
    Actor pActor,
    DecisionAsset[] pList,
    int pLength = -1,
    bool pGameplay = true)
  {
    if (pLength == -1)
      pLength = pList.Length;
    if (pGameplay)
      this.registerDecisionArrayGameplay(pActor, pList, pLength);
    else
      this.registerDecisionArraySimulation(pActor, pList, pLength);
  }

  private void registerDecisionArrayGameplay(Actor pActor, DecisionAsset[] pArray, int pLength)
  {
    NeuralLayerAsset[] layersArray = AssetManager.neural_layers.layers_array;
    DecisionChecks pChecks = new DecisionChecks(pActor);
    for (int index = 0; index < pLength; ++index)
    {
      DecisionAsset p = pArray[index];
      if ((!this._do_priority_levels || p.priority_int_cached >= this._highest_priority) && !pActor.isDecisionOnCooldown(p.decision_index, (double) p.cooldown) && pActor.isDecisionEnabled(p.decision_index) && p.isPossible(ref pChecks))
      {
        if (p.action_check_launch != null && !p.action_check_launch(pActor))
        {
          if (p.cooldown_on_launch_failure)
            pActor.setDecisionCooldown(p);
        }
        else
        {
          this._all_assets[this._counter_all_assets++] = p;
          if (layersArray[p.priority_int_cached].critical)
            this._do_priority_levels = true;
          if (this._do_priority_levels && p.priority_int_cached > this._highest_priority)
            this._highest_priority = p.priority_int_cached;
          int priorityArrayCounter = this._priority_array_counters[p.priority_int_cached];
          this._priority_array[p.priority_int_cached][priorityArrayCounter] = p;
          ++this._priority_array_counters[p.priority_int_cached];
        }
      }
    }
  }

  private void calculateFactors(Actor pActor)
  {
    DecisionAsset[] pPriorityArray;
    int pLength;
    if (this._do_priority_levels)
    {
      pPriorityArray = this._priority_array[this._highest_priority];
      pLength = this._priority_array_counters[this._highest_priority];
    }
    else
    {
      pPriorityArray = this._all_assets;
      pLength = this._counter_all_assets;
    }
    this.calculateFactorsFrom(pPriorityArray, pLength, pActor);
  }

  private void calculateFactorsFrom(DecisionAsset[] pPriorityArray, int pLength, Actor pActor)
  {
    DecisionAsset[] actions = this._actions;
    float[] factors = this._factors;
    for (int index = 0; index < pLength; ++index)
    {
      DecisionAsset pPriority = pPriorityArray[index];
      float num = pPriority.weight;
      if (pPriority.has_weight_custom)
        num = pPriority.weight_calculate_custom(pActor);
      actions[this._counter_possible] = pPriority;
      factors[this._counter_possible] = num;
      ++this._counter_possible;
    }
  }

  private void registerDecisionArraySimulation(Actor pActor, DecisionAsset[] pArray, int pLength)
  {
    DecisionAsset[] actions = this._actions;
    float[] factors = this._factors;
    DecisionChecks pChecks = new DecisionChecks(pActor);
    for (int index = 0; index < pLength; ++index)
    {
      DecisionAsset p = pArray[index];
      if (p.isPossible(ref pChecks))
      {
        float num = p.weight;
        if (p.has_weight_custom)
          num = p.weight_calculate_custom(pActor);
        actions[this._counter_possible] = p;
        factors[this._counter_possible] = num;
        ++this._counter_possible;
      }
    }
  }

  public void clear()
  {
    this.clearPriorityArray();
    this._counter_possible = 0;
    this._highest_priority = 0;
    this._counter_all_assets = 0;
  }

  private void clearPriorityArray()
  {
    int index = 0;
    for (int length = this._priority_array.Length; index < length; ++index)
    {
      this._priority_array[index].Clear<DecisionAsset>();
      this._priority_array_counters[index] = 0;
    }
  }

  private void calculateChances(float pRandomnessFactor = 1f)
  {
    float[] chances = this._chances;
    float[] factors = this._factors;
    int index = 0;
    for (int counterPossible = this._counter_possible; index < counterPossible; ++index)
    {
      float num = (float) Math.Pow(Math.E, (double) factors[index] * (double) pRandomnessFactor);
      chances[index] = num;
    }
  }

  public DecisionAsset chooseBestAction(float pRandomnessFactor = 1f)
  {
    float[] chances = this._chances;
    DecisionAsset[] actions = this._actions;
    this.calculateChances(pRandomnessFactor);
    float num1 = Randy.random() * this.sum();
    float num2 = 0.0f;
    int index = 0;
    for (int counterPossible = this._counter_possible; index < counterPossible; ++index)
    {
      num2 += chances[index];
      if ((double) num1 < (double) num2)
        return actions[index];
    }
    return this._counter_possible <= 0 ? (DecisionAsset) null : actions[this._counter_possible - 1];
  }

  private float sum()
  {
    float[] chances = this._chances;
    float num = 0.0f;
    int index = 0;
    for (int counterPossible = this._counter_possible; index < counterPossible; ++index)
      num += chances[index];
    return num;
  }

  public string getFactorString(DecisionAsset pAsset)
  {
    return this._factors[pAsset.decision_index].ToString("F3");
  }

  public string getChanceString(DecisionAsset pAsset)
  {
    return this._chances[pAsset.decision_index].ToString("F3");
  }

  public string getOrderString(DecisionAsset pAsset)
  {
    int index = 0;
    for (int counterPossible = this._counter_possible; index < counterPossible; ++index)
    {
      if (this._actions[index] == pAsset)
        return $"{index.ToString()}/{this._counter_possible.ToString()}";
    }
    return "??";
  }

  public void debug(Actor pActor, DebugTool pTool)
  {
    this.useOnDebug(pActor);
    int index = 0;
    for (int counterPossible = this._counter_possible; index < counterPossible; ++index)
    {
      DecisionAsset action = this._actions[index];
      float factor = this._factors[index];
      pTool.setText(action.id, (object) factor.ToString("F3"));
    }
  }

  private void useOnDebug(Actor pActor)
  {
    ActorAsset asset = pActor.asset;
    this.clear();
    this.registerBasicDecisionLists(pActor, false);
    if (!asset.hasDecisions())
      return;
    this.registerDecisionArraySimulation(pActor, asset.getDecisions(), asset.decisions_counter);
  }

  public int getCounter() => this._counter_possible;

  public DecisionAsset[] getActions() => this._actions;
}
