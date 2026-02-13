// Decompiled with JetBrains decompiler
// Type: KnowledgeElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class KnowledgeElement : MonoBehaviour
{
  [SerializeField]
  private LocalizedText _localized_text;
  [SerializeField]
  private Image _icon_left;
  [SerializeField]
  private Image _icon_right;
  [SerializeField]
  private EasterEggBanner _icon_easter_left;
  [SerializeField]
  private EasterEggBanner _icon_easter_right;
  [SerializeField]
  private StatBar _progress_bar;
  [SerializeField]
  private RunningIcons _running_icons;
  private CubeOverview _cube_overview_big;
  private WindowMetaTab _cube_tab;
  private KnowledgeAsset _asset;
  private int _running_icon_latest_index;
  private ILibraryWithUnlockables _library;
  private List<BaseUnlockableAsset> _assets_list = new List<BaseUnlockableAsset>();
  private int _items;
  private bool _initialized;

  private void OnEnable()
  {
    if (!this._initialized)
      return;
    this.resetBar();
  }

  private void Start()
  {
    this.init(this._asset);
    this.resetBar();
  }

  public void setAsset(KnowledgeAsset pAsset) => this._asset = pAsset;

  public void setCube(CubeOverview pBigCube, WindowMetaTab pCubeTab)
  {
    this._cube_overview_big = pBigCube;
    this._cube_tab = pCubeTab;
  }

  private void init(KnowledgeAsset pAsset)
  {
    // ISSUE: unable to decompile the method.
  }

  private void checkEasterEggsSprite()
  {
    if (string.IsNullOrEmpty(this._asset.path_icon_easter_egg))
    {
      ((Component) this._icon_easter_left).gameObject.SetActive(false);
      ((Component) this._icon_easter_right).gameObject.SetActive(false);
    }
    else
    {
      Sprite sprite = SpriteTextureLoader.getSprite(this._asset.path_icon_easter_egg);
      this._icon_easter_left.main_image.sprite = sprite;
      this._icon_easter_right.main_image.sprite = sprite;
    }
  }

  private void resetBar()
  {
    int pVal = this._asset.countUnlockedByPlayer();
    int num = this._asset.countTotal();
    this._progress_bar.setBar((float) pVal, (float) num, "/" + num.ToText());
  }

  private void nextItem(Transform pButton)
  {
    BaseUnlockableAsset nextAsset = this.getNextAsset();
    this._asset.load_button(pButton, nextAsset);
    ButtonTipLoader tipButtonLoader = this._asset.tip_button_loader;
    if (tipButtonLoader == null)
      return;
    tipButtonLoader(pButton, nextAsset);
  }

  private BaseUnlockableAsset getNextAsset()
  {
    ++this._running_icon_latest_index;
    int index = Toolbox.loopIndex(this._running_icon_latest_index, this._assets_list.Count);
    this._running_icon_latest_index = index;
    return this._assets_list[index];
  }

  private void prevItem(Transform pButton)
  {
    BaseUnlockableAsset prevAsset = this.getPrevAsset();
    this._asset.load_button(pButton, prevAsset);
    ButtonTipLoader tipButtonLoader = this._asset.tip_button_loader;
    if (tipButtonLoader == null)
      return;
    tipButtonLoader(pButton, prevAsset);
  }

  private BaseUnlockableAsset getPrevAsset()
  {
    --this._running_icon_latest_index;
    int num = Toolbox.loopIndex(this._running_icon_latest_index, this._assets_list.Count);
    this._running_icon_latest_index = num;
    return this._assets_list[Toolbox.loopIndex(num - this._items + 1, this._assets_list.Count)];
  }
}
