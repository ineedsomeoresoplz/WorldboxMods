// Decompiled with JetBrains decompiler
// Type: WorkshopMapElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Steamworks.Ugc;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WorkshopMapElement : MonoBehaviour
{
  private WorkshopMapData data;
  public Image image;
  public Text textName;
  public Text textKingdoms;
  public Text textCities;
  public Text textPopulation;
  public Text textMobs;
  public Text textUpvotes;
  public Text textComments;
  public Image mainBackground;
  public Image ayeIcon;

  public void load(WorkshopMapData pData)
  {
    this.data = pData;
    this.textName.text = this.data.meta_data_map.mapStats.name;
    this.textKingdoms.text = this.data.meta_data_map.kingdoms.ToString();
    this.textCities.text = this.data.meta_data_map.cities.ToString();
    this.textPopulation.text = this.data.meta_data_map.population.ToString();
    this.textMobs.text = this.data.meta_data_map.mobs.ToString();
    this.textUpvotes.text = ((Item) ref this.data.workshop_item).VotesUp.ToString();
    this.textComments.text = ((Item) ref this.data.workshop_item).NumComments.ToString();
    this.image.sprite = this.data.sprite_small_preview;
    if (((Item) ref this.data.workshop_item).Owner.Id.ToString() == Config.steam_id)
    {
      ((Graphic) this.textName).color = Toolbox.makeColor("#3DDEFF");
      ((Component) this.ayeIcon).gameObject.SetActive(true);
    }
    else
    {
      ((Graphic) this.textName).color = Toolbox.makeColor("#FF9B1C");
      ((Component) this.ayeIcon).gameObject.SetActive(false);
    }
    ((Object) ((Component) this).gameObject).name = "WorkshopMapElement " + this.data.meta_data_map.mapStats.name;
  }

  public void clickWorkshopMap()
  {
    SaveManager.currentWorkshopMapData = this.data;
    ScrollWindow.showWindow("steam_workshop_play_world");
  }
}
