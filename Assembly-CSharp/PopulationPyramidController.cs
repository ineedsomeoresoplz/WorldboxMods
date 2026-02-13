// Decompiled with JetBrains decompiler
// Type: PopulationPyramidController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PopulationPyramidController : MonoBehaviour
{
  [SerializeField]
  private MetaType _meta_type = MetaType.Special;
  [SerializeField]
  private Transform _population_pyramid_container;
  [SerializeField]
  private PopulationPyramidRow _population_pyramid_row;
  private Dictionary<int, GenderCount> _age_data = new Dictionary<int, GenderCount>();
  private List<int> _age_groups = new List<int>(20);
  private int _max_amount;
  private ObjectPoolGenericMono<PopulationPyramidRow> _pool_rows;
  private int _max_life_span;
  private int _age_steps;
  private IEnumerable<Actor> _units;

  private void Awake()
  {
    if (this._pool_rows != null)
      return;
    for (int index = 0; index < this._population_pyramid_container.childCount; ++index)
      Object.Destroy((Object) ((Component) this._population_pyramid_container.GetChild(index)).gameObject);
    this._pool_rows = new ObjectPoolGenericMono<PopulationPyramidRow>(this._population_pyramid_row, this._population_pyramid_container);
  }

  private void OnEnable() => this.load();

  private void OnDisable() => this.clearBars();

  internal void load()
  {
    this.calculateLifespan();
    this.calculateAgeData();
    this.calculateAgeGroups();
    this.StartCoroutine(this.showBars());
  }

  private void clearBars()
  {
    this._units = (IEnumerable<Actor>) null;
    this._pool_rows.clear();
  }

  private void calculateLifespan()
  {
    IMetaObject metaObject = (IMetaObject) AssetManager.meta_type_library.getAsset(this._meta_type).get_selected();
    this._units = metaObject.getUnits();
    this._max_life_span = metaObject.getMaxPossibleLifespan();
    foreach (Actor unit in this._units)
    {
      if (unit.isAlive())
      {
        int age = unit.getAge();
        if (age > this._max_life_span)
          this._max_life_span = age;
      }
    }
    this._max_life_span = Mathf.CeilToInt((float) this._max_life_span / 20f) * 20;
    this._max_life_span = Mathf.Clamp(this._max_life_span, 40, 100);
    this._age_steps = this._max_life_span / 10;
  }

  private void calculateAgeData()
  {
    this._max_amount = 0;
    this._age_data.Clear();
    foreach (Actor unit in this._units)
    {
      if (unit.isAlive())
      {
        int key = unit.getAge() / this._age_steps;
        if (!this._age_data.ContainsKey(key))
          this._age_data[key] = new GenderCount();
        int num = 0;
        if (unit.isSexMale())
          num = ++this._age_data[key].males;
        if (unit.isSexFemale())
          num = ++this._age_data[key].females;
        if (num > this._max_amount)
          this._max_amount = num;
      }
    }
  }

  private void calculateAgeGroups()
  {
    this._age_groups.Clear();
    int num = 10;
    foreach (int key in this._age_data.Keys)
    {
      if (key > num)
        num = key;
    }
    while (num >= 0)
      this._age_groups.Add(num--);
  }

  private IEnumerator showBars()
  {
    foreach (int ageGroup in this._age_groups)
    {
      if (this._age_data.ContainsKey(ageGroup) || ageGroup <= 10)
      {
        PopulationPyramidRow next = this._pool_rows.getNext();
        next.setAgeGroup(ageGroup * this._age_steps, (ageGroup + 1) * this._age_steps - 1);
        if (this._age_data.ContainsKey(ageGroup))
        {
          int pAmount = this._age_data[ageGroup].males + this._age_data[ageGroup].females;
          next.setMaleCount(this._age_data[ageGroup].males, this._max_amount);
          next.setFemaleCount(this._age_data[ageGroup].females, this._max_amount);
          next.setColorTextBasedOnAmount(pAmount);
        }
        else
        {
          next.setColorTextBasedOnAmount(0);
          next.setMaleCount(0, this._max_amount);
          next.setFemaleCount(0, this._max_amount);
        }
        yield return (object) CoroutineHelper.wait_for_next_frame;
      }
    }
  }
}
