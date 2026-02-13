// Decompiled with JetBrains decompiler
// Type: AchievementGoodie
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class AchievementGoodie : MonoBehaviour
{
  [SerializeField]
  private Image _icon;
  [SerializeField]
  private Text _name;

  public void load(BaseUnlockableAsset pAsset, bool pUnlocked)
  {
    if (pUnlocked)
      this.loadUnlocked(pAsset);
    else
      this.loadLocked(pAsset);
  }

  private void loadLocked(BaseUnlockableAsset pAsset)
  {
    this._icon.sprite = pAsset.getSprite();
    ((Graphic) this._icon).color = Toolbox.color_black;
  }

  private void loadUnlocked(BaseUnlockableAsset pAssets)
  {
    this._icon.sprite = pAssets.getSprite();
    ((Component) this._name).GetComponent<LocalizedText>().setKeyAndUpdate(pAssets.getLocaleID());
    switch (pAssets)
    {
      case ActorAsset actorAsset:
        ((Graphic) this._name).color = AssetManager.kingdoms.get(actorAsset.kingdom_id_wild).default_kingdom_color.getColorText();
        break;
      case BaseAugmentationAsset augmentationAsset:
        BaseCategoryAsset group = augmentationAsset.getGroup();
        ((Graphic) this._name).color = group != null ? group.getColor() : Toolbox.color_white;
        break;
      default:
        ((Graphic) this._name).color = Toolbox.color_white;
        break;
    }
  }
}
