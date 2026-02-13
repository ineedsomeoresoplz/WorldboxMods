// Decompiled with JetBrains decompiler
// Type: SelectedMetaWithUnit`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class SelectedMetaWithUnit<TMeta, TMetaData> : 
  SelectedMeta<TMeta, TMetaData>,
  ISelectedMetaWithUnit
  where TMeta : MetaObject<TMetaData>, IFavoriteable
  where TMetaData : MetaObjectData
{
  [SerializeField]
  private SelectedMetaUnitElement _unit_element;
  [SerializeField]
  private GameObject _unit_element_separator;

  public SelectedMetaUnitElement unit_element => this._unit_element;

  public GameObject unit_element_separator => this._unit_element_separator;

  private ISelectedMetaWithUnit as_meta_with_unit => (ISelectedMetaWithUnit) this;

  public int last_dirty_stats_unit { get; set; }

  public Actor last_unit { get; set; }

  public virtual string unit_title_locale_key { get; }

  public virtual bool hasUnit() => throw new NotImplementedException();

  public virtual Actor getUnit() => throw new NotImplementedException();

  protected override void updateElementsAlways(TMeta pNano)
  {
    base.updateElementsAlways(pNano);
    this.as_meta_with_unit.checkUnitElement();
    if (!this.hasUnit())
      return;
    this._unit_element.updateBarAndTask(this.getUnit());
  }

  protected override void showStatsGeneral(TMeta pMeta)
  {
    base.showStatsGeneral(pMeta);
    if (!this.hasUnit())
      return;
    this._unit_element.showStats(this.getUnit());
  }

  public void avatarTouchScream() => this.as_meta_with_unit.avatarTouch();

  protected override void clearLastObject()
  {
    base.clearLastObject();
    this.as_meta_with_unit.clearLastUnit();
  }
}
