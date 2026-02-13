// Decompiled with JetBrains decompiler
// Type: SelectedNano`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SelectedNano<TNanoObject> : SelectedNanoBase where TNanoObject : NanoObject, IFavoriteable
{
  [SerializeField]
  private GameObject _container_banners_parent;
  private ISelectedContainerTrait _container_traits;
  private ISelectedTabBanners<TNanoObject> _container_banners;
  private int _last_dirty_stats;
  private TNanoObject _last_nano;

  protected virtual TNanoObject nano_object
  {
    get => SelectedObjects.getSelectedNanoObject() as TNanoObject;
  }

  protected override void Awake()
  {
    base.Awake();
    this._container_traits = ((Component) this).GetComponentInChildren<ISelectedContainerTrait>();
    if (!Object.op_Inequality((Object) this._container_banners_parent, (Object) null))
      return;
    this._container_banners = this._container_banners_parent.GetComponentInChildren<ISelectedTabBanners<TNanoObject>>();
  }

  private void OnDisable() => this.clearLastObject();

  public override void update()
  {
    if (this._last_nano.isRekt())
      this._last_nano = default (TNanoObject);
    this.updateElements(this.nano_object);
  }

  protected virtual void updateElements(TNanoObject pNano)
  {
    if (pNano.isRekt())
    {
      this.clearLastObject();
    }
    else
    {
      this.updateFavoriteIcon(pNano.isFavorite());
      if (this.isNanoChanged(pNano))
        this.updateElementsOnChange(pNano);
      this.updateElementsAlways(pNano);
      this._last_dirty_stats = pNano.getStatsDirtyVersion();
      this._last_nano = pNano;
    }
  }

  protected virtual void updateElementsOnChange(TNanoObject pNano)
  {
    this.showStatsGeneral(pNano);
    this.updateTraits();
    this.updateBanners(pNano);
    this.checkAchievements(pNano);
    World.world.selected_buttons.clearHighlightedButton();
  }

  protected virtual void checkAchievements(TNanoObject pNano)
  {
  }

  protected virtual void updateElementsAlways(TNanoObject pNano) => this.recalcTabSize();

  protected virtual void showStatsGeneral(TNanoObject pNano)
  {
  }

  private void updateBanners(TNanoObject pNano)
  {
    if (this._container_banners == null)
      return;
    this._container_banners.update(pNano);
    if (this._container_banners.countVisibleBanners() > 0)
      this._container_banners_parent.gameObject.SetActive(true);
    else
      this._container_banners_parent.gameObject.SetActive(false);
  }

  protected virtual void updateTraits()
  {
    if (this._container_traits == null)
      return;
    this._container_traits.update((NanoObject) this.nano_object);
  }

  protected virtual void clearLastObject()
  {
    this._last_nano = default (TNanoObject);
    this._last_dirty_stats = -1;
  }

  protected void recalcTabSize()
  {
    ScrollRectExtended scrollRect = PowerTabController.instance.scrollRect;
    if (scrollRect.isDragged() || (double) Mathf.Abs(scrollRect.velocity.x) > 1.0)
    {
      this._powers_tab.sortButtons();
    }
    else
    {
      float num = scrollRect.isHorizontalScrollAvailable() ? scrollRect.horizontalNormalizedPosition : 0.0f;
      Vector2 velocity = scrollRect.velocity;
      if (this._powers_tab.recalc())
        this._powers_tab.sortButtons();
      scrollRect.horizontalNormalizedPosition = num;
      scrollRect.velocity = velocity;
    }
  }

  protected bool isNanoChanged(TNanoObject pNano)
  {
    return pNano.getStatsDirtyVersion() != this._last_dirty_stats || (object) pNano != (object) this._last_nano;
  }
}
