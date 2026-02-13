// Decompiled with JetBrains decompiler
// Type: SelectedUnit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public static class SelectedUnit
{
  private static Actor _unit_main;
  private static HashSet<Actor> _units_hashset = new HashSet<Actor>();
  private static List<Actor> _units_list = new List<Actor>();
  private static SelectedUnitClearEvent _on_clear_events;
  private static int _selection_version;

  public static HashSet<Actor> getAllSelected() => SelectedUnit._units_hashset;

  public static List<Actor> getAllSelectedList() => SelectedUnit._units_list;

  public static bool multipleSelected() => SelectedUnit._units_hashset.Count > 1;

  public static int countSelected() => SelectedUnit._units_hashset.Count;

  public static bool isSet() => SelectedUnit._units_hashset.Count != 0;

  public static void selectMultiple(ListPool<Actor> pActors)
  {
    // ISSUE: unable to decompile the method.
  }

  public static bool select(Actor pActor, bool pSetMainUnit = true)
  {
    if (pSetMainUnit)
      SelectedUnit.makeMainSelected(pActor);
    if (SelectedUnit._units_hashset.Add(pActor))
      SelectedUnit.hashsetChanged();
    return SelectedUnit.isSet();
  }

  public static void unselect(Actor pActor)
  {
    if (!SelectedUnit._units_hashset.Remove(pActor))
      return;
    SelectedUnit.hashsetChanged();
  }

  public static void clear()
  {
    SelectedUnit._units_hashset.Clear();
    SelectedUnit.hashsetChanged();
    SelectedUnit.clearMain();
    SelectedUnitClearEvent onClearEvents = SelectedUnit._on_clear_events;
    if (onClearEvents == null)
      return;
    onClearEvents();
  }

  private static void hashsetChanged()
  {
    SelectedUnit._units_list.Clear();
    SelectedUnit._units_list.AddRange((IEnumerable<Actor>) SelectedUnit._units_hashset);
    SelectedUnit._units_list.Shuffle<Actor>();
    ++SelectedUnit._selection_version;
  }

  public static bool isMainSelected(Actor pActor)
  {
    return SelectedUnit.isSet() && SelectedUnit._unit_main == pActor;
  }

  public static Actor unit => SelectedUnit._unit_main;

  public static bool isSelected(Actor pActor)
  {
    return SelectedUnit.isSet() && SelectedUnit._units_hashset.Contains(pActor);
  }

  public static void subscribeClearEvent(SelectedUnitClearEvent pEvent)
  {
    SelectedUnit._on_clear_events += pEvent;
  }

  public static void removeSelected(Actor pActor)
  {
    if (SelectedUnit._units_hashset.Remove(pActor))
      SelectedUnit.hashsetChanged();
    if (SelectedUnit._unit_main != pActor)
      return;
    SelectedUnit.clearMain();
    SelectedUnit.trySelectNewMain();
  }

  private static void clearMain() => SelectedUnit._unit_main = (Actor) null;

  private static void trySelectNewMain()
  {
    if (SelectedUnit._units_hashset.Count == 0)
      SelectedUnit.clear();
    else
      SelectedUnit.makeMainSelected(SelectedUnit._units_hashset.GetRandom<Actor>());
  }

  public static void nextMainUnit()
  {
    if (!SelectedUnit.isSet())
      return;
    SelectedUnit.makeMainSelected(SelectedUnit._units_list.LoopNext<Actor>(SelectedUnit._unit_main));
  }

  public static void killSelected()
  {
    // ISSUE: unable to decompile the method.
  }

  public static void makeMainSelected(Actor pActor)
  {
    if (SelectedUnit._unit_main != pActor)
      pActor.makeSpawnSound(true);
    SelectedUnit._unit_main = pActor;
  }

  public static int getSelectionVersion() => SelectedUnit._selection_version;
}
