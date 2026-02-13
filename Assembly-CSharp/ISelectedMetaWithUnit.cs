// Decompiled with JetBrains decompiler
// Type: ISelectedMetaWithUnit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public interface ISelectedMetaWithUnit
{
  SelectedMetaUnitElement unit_element { get; }

  GameObject unit_element_separator { get; }

  string unit_title_locale_key { get; }

  int last_dirty_stats_unit { get; set; }

  Actor last_unit { get; set; }

  bool checkUnitElement()
  {
    if (!this.hasUnit())
    {
      this.setUnitElementVisible(false);
      return true;
    }
    this.setUnitElementVisible(true);
    Actor unit = this.getUnit();
    UiUnitAvatarElement avatar = this.unit_element.getAvatar();
    if (this.unitChanged(unit) || avatar.avatarLoader.actorStateChanged())
    {
      this.unit_element.show(unit, this.unit_title_locale_key);
      this.last_dirty_stats_unit = unit.getStatsDirtyVersion();
      this.last_unit = unit;
      return true;
    }
    avatar.updateTileSprite();
    return false;
  }

  void setUnitElementVisible(bool pState)
  {
    ((Component) this.unit_element).gameObject.SetActive(pState);
    this.unit_element_separator.SetActive(pState);
  }

  void avatarTouch()
  {
    if (!this.hasUnit())
      return;
    Actor unit = this.getUnit();
    SelectedUnit.select(unit);
    SelectedObjects.setNanoObject((NanoObject) unit);
    PowerTabController.showTabSelectedUnit();
    ToolbarButtons.instance.shake();
  }

  bool hasUnit();

  Actor getUnit();

  bool unitChanged(Actor pActor)
  {
    return pActor.getStatsDirtyVersion() != this.last_dirty_stats_unit || pActor != this.last_unit;
  }

  void clearLastUnit()
  {
    this.last_unit = (Actor) null;
    this.last_dirty_stats_unit = -1;
  }
}
