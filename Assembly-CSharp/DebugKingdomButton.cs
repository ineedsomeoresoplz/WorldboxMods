// Decompiled with JetBrains decompiler
// Type: DebugKingdomButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class DebugKingdomButton : MonoBehaviour
{
  [SerializeField]
  private Button _button;
  [SerializeField]
  private Image _image;
  internal KingdomAsset kingdom_asset;
  [SerializeField]
  private Image _discrepancy_bad;
  [SerializeField]
  private Image _discrepancy_have;
  [SerializeField]
  private Image _discrepancy_normal;

  public Image image => this._image;

  public void setAsset(KingdomAsset pAsset)
  {
    this.kingdom_asset = pAsset;
    this._image.sprite = this.kingdom_asset.getSprite();
    this.setupTooltip();
    if (this.kingdom_asset.assets_discrepancies_bad != null)
      ((Component) this._discrepancy_have).gameObject.SetActive(true);
    else
      ((Component) this._discrepancy_have).gameObject.SetActive(false);
  }

  public void checkSelected(KingdomAsset pAssetMain)
  {
    ((Component) this._discrepancy_bad).gameObject.SetActive(false);
    ((Component) this._discrepancy_normal).gameObject.SetActive(false);
    if (this.kingdom_asset == pAssetMain)
    {
      ((Graphic) this.image).color = Color.white;
    }
    else
    {
      if (this.kingdom_asset.assets_discrepancies != null && this.kingdom_asset.assets_discrepancies.Contains(pAssetMain.id))
        ((Component) this._discrepancy_normal).gameObject.SetActive(true);
      if (pAssetMain.assets_discrepancies_bad != null && pAssetMain.assets_discrepancies_bad.Contains(this.kingdom_asset.id))
        ((Component) this._discrepancy_bad).gameObject.SetActive(true);
      if (pAssetMain.isFoe(this.kingdom_asset))
        ((Graphic) this.image).color = new Color(0.2f, 0.2f, 0.2f);
      else
        ((Graphic) this.image).color = Color.white;
    }
  }

  public void setupTooltip()
  {
    TipButton tipButton;
    if (!((Component) this).TryGetComponent<TipButton>(ref tipButton))
      return;
    tipButton.hoverAction = new TooltipAction(this.showTooltip);
  }

  private void showTooltip()
  {
    Tooltip.show((object) ((Component) this).gameObject, "debug_kingdom_assets", new TooltipData()
    {
      kingdom_asset = this.kingdom_asset
    });
  }

  public static void getTooltipDescription(
    KingdomAsset pAsset,
    out string pDescription,
    out string pDescription2)
  {
    pDescription = string.Empty;
    pDescription2 = string.Empty;
    if (pAsset.list_tags.Count > 0)
    {
      pDescription += "--- OWN TAGS ---\n".ColorHex(ColorStyleLibrary.m.color_text_grey_dark);
      foreach (string listTag in pAsset.list_tags)
        pDescription += (listTag + "\n").ColorHex(ColorStyleLibrary.m.color_text_grey);
    }
    if (pAsset.friendly_tags.Count > 0)
    {
      pDescription += "--- FRIENDLY TAGS ---\n".ColorHex(ColorStyleLibrary.m.color_text_grey_dark);
      foreach (string friendlyTag in pAsset.friendly_tags)
        pDescription += (friendlyTag + "\n").ColorHex("#43FF43");
    }
    if (pAsset.enemy_tags.Count > 0)
    {
      pDescription += "#--- ENEMY TAGS ---\n".ColorHex(ColorStyleLibrary.m.color_text_grey_dark);
      foreach (string enemyTag in pAsset.enemy_tags)
        pDescription += (enemyTag + "\n").ColorHex("#FB2C21");
    }
    if (pAsset.assets_discrepancies == null || pAsset.assets_discrepancies.Count <= 0)
      return;
    pDescription2 = $"!! Discrepancies {pAsset.assets_discrepancies.Count}!!\n".ColorHex("#D85BC5");
    int num1 = 0;
    foreach (string assetsDiscrepancy in pAsset.assets_discrepancies)
    {
      pDescription2 = assetsDiscrepancy.Contains(pAsset.id) || pAsset.id.Contains(assetsDiscrepancy) ? pDescription2 + assetsDiscrepancy.ColorHex("#FB2C21") : pDescription2 + assetsDiscrepancy;
      if (pDescription2.Length > 150)
      {
        int num2 = pAsset.assets_discrepancies.Count - num1;
        pDescription2 += $" and {num2} more...!!!".ColorHex("#8CFF99");
        break;
      }
      if (num1 < pAsset.assets_discrepancies.Count - 1)
        pDescription2 += ", ";
      ++num1;
    }
  }
}
