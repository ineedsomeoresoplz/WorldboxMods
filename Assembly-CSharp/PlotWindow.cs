// Decompiled with JetBrains decompiler
// Type: PlotWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PlotWindow : WindowMetaGeneric<Plot, PlotData>
{
  [SerializeField]
  private GameObject content_author;
  [SerializeField]
  private PrefabUnitElement author_element;
  public StatsIcon text_info_age;
  public StatsIcon text_info_members;
  public StatsIcon text_info_power;
  public StatsIcon text_info_dead;
  public Text text_description;
  public StatBar bar;

  public override MetaType meta_type => MetaType.Plot;

  protected override Plot meta_object => SelectedMetas.selected_plot;

  public override void startShowingWindow()
  {
    base.startShowingWindow();
    AchievementLibrary.plots_explorer.check();
  }

  protected override void showTopPartInformation()
  {
    Plot metaObject = this.meta_object;
    if (metaObject == null)
      return;
    if (metaObject.getAsset().needs_to_be_explored)
      metaObject.getAsset().unlock();
    float progress = metaObject.getProgress();
    float progressMax = metaObject.getProgressMax();
    this.bar.setBar(progress, progressMax, "/" + progressMax.ToText(), pFloat: true);
    this.text_description.text = metaObject.getAsset().get_formatted_description(metaObject);
    ((Component) this.text_description).GetComponent<LocalizedText>().checkTextFont();
    ((Component) this.text_description).GetComponent<LocalizedText>().checkSpecialLanguages();
    this.showAuthor();
  }

  private void showAuthor()
  {
    Actor author = this.meta_object.getAuthor();
    if (author.isRekt())
    {
      this.content_author.SetActive(false);
    }
    else
    {
      this.content_author.SetActive(true);
      this.author_element.show(author);
    }
  }

  internal override void showStatsRows()
  {
    Plot metaObject = this.meta_object;
    if (metaObject == null)
      return;
    this.tryShowPastNames();
    this.showStatRow("started_by", (object) metaObject.data.founder_name, MetaType.Unit, metaObject.data.founder_id);
    this.showStatRow("started_at", (object) metaObject.getFoundedDate());
  }
}
