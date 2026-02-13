// Decompiled with JetBrains decompiler
// Type: SelectedMeta`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SelectedMeta<TMeta, TMetaData> : SelectedNano<TMeta>
  where TMeta : MetaObject<TMetaData>, IFavoriteable
  where TMetaData : MetaObjectData
{
  [SerializeField]
  protected BannerGeneric<TMeta, TMetaData> banner;

  protected virtual MetaType meta_type { get; }

  protected string window_id => AssetManager.meta_type_library.getAsset(this.meta_type).window_name;

  protected override void updateElements(TMeta pNano)
  {
    if (pNano.isRekt())
      return;
    base.updateElements(pNano);
    this.checkShowBanner();
  }

  protected override void showStatsGeneral(TMeta pMeta)
  {
    this.setName(pMeta);
    this.setTitleIcons(pMeta);
    this.showGeneralIcons(pMeta);
  }

  protected virtual void setName(TMeta pMeta)
  {
    this.name_field.text = pMeta.name;
    ((Graphic) this.name_field).color = pMeta.getColor().getColorText();
  }

  protected virtual void setTitleIcons(TMeta pMeta)
  {
    this.icon_right.sprite = pMeta.getSpriteIcon();
  }

  protected virtual void checkShowBanner() => this.banner.load((NanoObject) this.nano_object);

  protected void showGeneralIcons(TMeta pMeta)
  {
    foreach (StatsIconContainer statsIcon in this.stats_icons)
      statsIcon.showGeneralIcons<TMeta, TMetaData>(pMeta);
  }

  public void openInfoTab()
  {
    ScrollWindow.showWindow(this.window_id);
    ScrollWindow.getCurrentWindow().tabs.showTab("Info");
  }

  public void openTraitsEditorTab()
  {
    ScrollWindow.showWindow(this.window_id);
    ScrollWindow.getCurrentWindow().tabs.showTab("Traits");
  }

  public void openFamiliesTab()
  {
    ScrollWindow.showWindow(this.window_id);
    ScrollWindow.getCurrentWindow().tabs.showTab("Families");
  }

  public void openInterestingPeopleTab()
  {
    ScrollWindow.showWindow(this.window_id);
    ScrollWindow.getCurrentWindow().tabs.showTab("Interesting People");
  }

  public void openPyramidTab()
  {
    ScrollWindow.showWindow(this.window_id);
    ScrollWindow.getCurrentWindow().tabs.showTab("Pyramid");
  }

  public void openStatisticsTab()
  {
    ScrollWindow.showWindow(this.window_id);
    ScrollWindow.getCurrentWindow().tabs.showTab("Statistics");
  }
}
