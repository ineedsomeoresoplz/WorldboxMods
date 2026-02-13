// Decompiled with JetBrains decompiler
// Type: WindowFavorites
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WindowFavorites : WindowListBaseActor
{
  private List<Actor> _temp_list_actor = new List<Actor>();

  protected override void setupSortingTabs()
  {
    this.sorting_tab.tryAddButton("ui/Icons/iconAge", "sort_by_age", new SortButtonAction(((WindowListBaseActor) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Actor>(WindowFavorites.sortByAge)));
    this.sorting_tab.tryAddButton("ui/Icons/iconRenown", "sort_by_renown", new SortButtonAction(((WindowListBaseActor) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Actor>(WindowFavorites.sortByRenown)));
    this.sorting_tab.tryAddButton("ui/Icons/iconLevels", "sort_by_level", new SortButtonAction(((WindowListBaseActor) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Actor>(WindowFavorites.sortByLevel)));
    this.sorting_tab.tryAddButton("ui/Icons/iconKills", "sort_by_kills", new SortButtonAction(((WindowListBaseActor) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Actor>(WindowFavorites.sortByKills)));
    this.sorting_tab.tryAddButton("ui/Icons/iconKingdom", "sort_by_kingdom", new SortButtonAction(((WindowListBaseActor) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Actor>(WindowFavorites.sortByKingdom)));
  }

  protected override void show()
  {
    base.show();
    if (!Object.op_Inequality((Object) this._title_counter, (Object) null))
      return;
    this._title_counter.text = this._temp_list_actor.Count.ToString();
  }

  protected override List<Actor> getObjects()
  {
    this._temp_list_actor.Clear();
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
    {
      if (unit.isAlive() && unit.isFavorite())
        this._temp_list_actor.Add(unit);
    }
    return this._temp_list_actor;
  }

  public static int sortByRenown(Actor pObject1, Actor pObject2)
  {
    return pObject2.data.renown.CompareTo(pObject1.data.renown);
  }

  public static int sortByKingdom(Actor pActor1, Actor pActor2)
  {
    return pActor2.kingdom.CompareTo((NanoObject) pActor1.kingdom);
  }

  public static int sortByAge(Actor pActor1, Actor pActor2)
  {
    return pActor2.getAge().CompareTo(pActor1.getAge());
  }

  public static int sortByLevel(Actor pActor1, Actor pActor2)
  {
    return pActor2.data.level.CompareTo(pActor1.data.level);
  }

  public static int sortByKills(Actor pActor1, Actor pActor2)
  {
    return pActor2.data.kills.CompareTo(pActor1.data.kills);
  }
}
