// Decompiled with JetBrains decompiler
// Type: InterestingPeopleTab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class InterestingPeopleTab : WindowMetaElementBase
{
  private const float TWEEN_DURATION = 0.15f;
  public InterestingPeopleElement most_kills;
  public InterestingPeopleElement most_children;
  public InterestingPeopleElement most_births;
  public InterestingPeopleElement oldest;
  public InterestingPeopleElement fastest;
  public InterestingPeopleElement strongest;
  public InterestingPeopleElement weakest;
  public InterestingPeopleElement smartest;
  public InterestingPeopleElement dumbest;
  public InterestingPeopleElement richest;
  public InterestingPeopleElement most_known;
  public InterestingPeopleElement biggest_level;
  public InterestingPeopleElement happiest;
  public InterestingPeopleElement saddest;
  public InterestingPeopleElement hungriest;
  public InterestingPeopleElement fullest;
  public InterestingPeopleElement youngest;
  public InterestingPeopleElement most_health;
  public InterestingPeopleElement lowest_health;
  private readonly List<Actor> _unit_most_kills = new List<Actor>();
  private readonly List<Actor> _unit_most_children = new List<Actor>();
  private readonly List<Actor> _unit_most_births = new List<Actor>();
  private readonly List<Actor> _unit_oldest = new List<Actor>();
  private readonly List<Actor> _unit_fastest = new List<Actor>();
  private readonly List<Actor> _unit_strongest = new List<Actor>();
  private readonly List<Actor> _unit_weakest = new List<Actor>();
  private readonly List<Actor> _unit_smartest = new List<Actor>();
  private readonly List<Actor> _unit_dumbest = new List<Actor>();
  private readonly List<Actor> _unit_richest = new List<Actor>();
  private readonly List<Actor> _unit_most_known = new List<Actor>();
  private readonly List<Actor> _unit_biggest_level = new List<Actor>();
  private readonly List<Actor> _unit_saddest = new List<Actor>();
  private readonly List<Actor> _unit_happiest = new List<Actor>();
  private readonly List<Actor> _unit_hungriest = new List<Actor>();
  private readonly List<Actor> _unit_fullest = new List<Actor>();
  private readonly List<Actor> _unit_youngest = new List<Actor>();
  private readonly List<Actor> _unit_most_health = new List<Actor>();
  private readonly List<Actor> _unit_lowest_health = new List<Actor>();
  private List<Actor>[] _all_unit_lists;
  private InterestingPeopleElement[] _all_elements;
  private IInterestingPeopleWindow _interesting_people_window;
  private List<Tweener> _tweeners = new List<Tweener>();

  protected override void Awake()
  {
    this._interesting_people_window = ((Component) this).GetComponentInParent<IInterestingPeopleWindow>();
    this._all_elements = new InterestingPeopleElement[19]
    {
      this.biggest_level,
      this.fastest,
      this.fullest,
      this.happiest,
      this.hungriest,
      this.most_births,
      this.most_children,
      this.most_kills,
      this.most_known,
      this.oldest,
      this.richest,
      this.saddest,
      this.smartest,
      this.dumbest,
      this.strongest,
      this.weakest,
      this.youngest,
      this.most_health,
      this.lowest_health
    };
    this._all_unit_lists = new List<Actor>[19]
    {
      this._unit_biggest_level,
      this._unit_fastest,
      this._unit_fullest,
      this._unit_happiest,
      this._unit_hungriest,
      this._unit_most_births,
      this._unit_most_children,
      this._unit_most_kills,
      this._unit_most_known,
      this._unit_oldest,
      this._unit_richest,
      this._unit_saddest,
      this._unit_smartest,
      this._unit_dumbest,
      this._unit_strongest,
      this._unit_weakest,
      this._unit_youngest,
      this._unit_most_health,
      this._unit_lowest_health
    };
    base.Awake();
  }

  protected override IEnumerator showContent()
  {
    return this.renderElements(this._interesting_people_window.getInterestingUnitsList());
  }

  private IEnumerator renderElements(IEnumerable<Actor> pList)
  {
    // ISSUE: unable to decompile the method.
  }

  private IEnumerator render(
    List<Actor> pActor,
    InterestingPeopleElement pElement,
    int pValue,
    int pMinValue = 2)
  {
    if (pValue < pMinValue || pActor.Count == 0)
    {
      ((Component) pElement).gameObject.SetActive(false);
    }
    else
    {
      ((Component) pElement).gameObject.SetActive(true);
      foreach (Actor pActor1 in pActor)
      {
        if (pActor1.isAlive())
        {
          pElement.show(pActor1, pValue);
          yield return (object) new WaitForSecondsRealtime(0.025f);
        }
      }
    }
  }

  private void finishTweens()
  {
    foreach (Tween tweener in this._tweeners)
      TweenExtensions.Kill(tweener, true);
    this._tweeners.Clear();
  }

  protected override void clear()
  {
    base.clear();
    this.finishTweens();
    foreach (Component allElement in this._all_elements)
      allElement.gameObject.SetActive(false);
    foreach (List<Actor> allUnitList in this._all_unit_lists)
      allUnitList.Clear();
  }
}
