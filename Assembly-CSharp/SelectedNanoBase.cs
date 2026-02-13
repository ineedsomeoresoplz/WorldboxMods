// Decompiled with JetBrains decompiler
// Type: SelectedNanoBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SelectedNanoBase : MonoBehaviour
{
  public StatsIconContainer[] stats_icons;
  protected PowersTab _powers_tab;
  [SerializeField]
  private Image _favorite_icon;
  [SerializeField]
  protected Image icon_left;
  [SerializeField]
  protected Image icon_right;
  [SerializeField]
  protected Text name_field;

  protected virtual void Awake()
  {
    this._powers_tab = ((Component) this).GetComponent<PowersTab>();
    if (this.stats_icons != null && this.stats_icons.Length != 0)
      return;
    Debug.LogError((object) ("SelectedNano: No StatsIconContainer found in children of " + ((Object) ((Component) this).gameObject).name));
  }

  public virtual void update()
  {
  }

  protected PowerTabAsset getPowerTabAsset()
  {
    return AssetManager.power_tab_library.get(this.getPowerTabAssetID());
  }

  protected virtual string getPowerTabAssetID() => throw new NotImplementedException();

  public void clickFavoriteMeta()
  {
    ICoreObject coreObject = this.getPowerTabAsset().meta_type.getAsset().get_selected() as ICoreObject;
    coreObject.switchFavorite();
    if (coreObject.isFavorite())
    {
      string text = LocalizedTextManager.getText("favorited");
      WorldTip.instance.showToolbarText(text);
    }
    this.updateFavoriteIcon(coreObject.isFavorite());
  }

  public void clickFavoriteUnit()
  {
    Actor actor = (Actor) this.getPowerTabAsset().meta_type.getAsset().get_selected();
    actor.switchFavorite();
    if (actor.isFavorite())
    {
      string text = LocalizedTextManager.getText("tip_favorite_icon");
      WorldTip.instance.showToolbarText(text);
    }
    this.updateFavoriteIcon(actor.isFavorite());
  }

  protected void updateFavoriteIcon(bool pStatus)
  {
    if (Object.op_Equality((Object) this._favorite_icon, (Object) null))
      return;
    if (pStatus)
      ((Graphic) this._favorite_icon).color = ColorStyleLibrary.m.favorite_selected;
    else
      ((Graphic) this._favorite_icon).color = ColorStyleLibrary.m.favorite_not_selected;
  }

  protected void setIconValue(
    string pName,
    float pMainVal,
    float? pMax = null,
    string pColor = "",
    bool pFloat = false,
    string pEnding = "",
    char pSeparator = '/')
  {
    foreach (StatsIconContainer statsIcon in this.stats_icons)
      statsIcon.setIconValue(pName, pMainVal, pMax, pColor, pFloat, pEnding, pSeparator);
  }
}
