// Decompiled with JetBrains decompiler
// Type: ArmyWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ArmyWindow : WindowMetaGeneric<Army, ArmyData>
{
  [SerializeField]
  private Image _race_top_left;
  [SerializeField]
  private Image _race_top_right;

  public override MetaType meta_type => MetaType.Army;

  protected override Army meta_object => SelectedMetas.selected_army;

  protected override void showTopPartInformation()
  {
    base.showTopPartInformation();
    Army metaObject = this.meta_object;
    if (metaObject == null)
      return;
    this._race_top_left.sprite = metaObject.getSpriteIcon();
    this._race_top_right.sprite = metaObject.getSpriteIcon();
  }

  internal override void showStatsRows()
  {
    Army metaObject = this.meta_object;
    if (metaObject == null)
      return;
    this.tryShowPastNames();
    this.showStatRow("founded", (object) metaObject.getFoundedDate(), MetaType.None, -1L, "iconAge", (string) null, (TooltipDataGetter) null);
    this.showStatRow("males", (object) metaObject.countMales(), MetaType.None, -1L, "iconMale", (string) null, (TooltipDataGetter) null);
    this.showStatRow("females", (object) metaObject.countFemales(), MetaType.None, -1L, "iconFemale", (string) null, (TooltipDataGetter) null);
    this.showStatRow("deaths", (object) metaObject.getTotalDeaths(), MetaType.None, -1L, "iconDead", (string) null, (TooltipDataGetter) null);
    this.showStatRow("kills", (object) metaObject.getTotalKills(), MetaType.None, -1L, "iconKills", (string) null, (TooltipDataGetter) null);
    this.tryToShowActor("captain", pObject: metaObject.getCaptain(), pIconPath: "iconCaptain");
    this.tryShowPastCaptains();
    this.tryToShowMetaCity("village", pObject: metaObject.getCity());
    this.tryToShowMetaKingdom(pObject: metaObject.getKingdom());
  }

  private void tryShowPastCaptains()
  {
    List<LeaderEntry> pastCaptains = this.meta_object.data.past_captains;
    // ISSUE: explicit non-virtual call
    if ((pastCaptains != null ? (__nonvirtual (pastCaptains.Count) > 1 ? 1 : 0) : 0) == 0)
      return;
    this.showStatRow("past_captains", (object) this.meta_object.data.past_captains.Count, MetaType.None, -1L, "iconCaptain", "past_rulers", new TooltipDataGetter(this.getTooltipPastCaptains));
  }

  private TooltipData getTooltipPastCaptains()
  {
    return new TooltipData()
    {
      tip_name = "past_captains",
      meta_type = MetaType.Army,
      past_rulers = new ListPool<LeaderEntry>((ICollection<LeaderEntry>) this.meta_object.data.past_captains)
    };
  }
}
