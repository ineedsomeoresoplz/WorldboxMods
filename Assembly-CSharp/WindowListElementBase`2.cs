// Decompiled with JetBrains decompiler
// Type: WindowListElementBase`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class WindowListElementBase<TMetaObject, TData> : 
  MonoBehaviour,
  IPointerMoveHandler,
  IEventSystemHandler
  where TMetaObject : CoreSystemObject<TData>
  where TData : BaseSystemData
{
  [HideInInspector]
  public TMetaObject meta_object;
  [SerializeField]
  private BannerGeneric<TMetaObject, TData> _main_banner;
  [SerializeField]
  private GameObject _icon_favorite;
  [SerializeField]
  private Image _icon_species;

  private void Awake() => this.create();

  private void create()
  {
    this.initMonoFields();
    this.initTooltip();
  }

  protected virtual void initMonoFields()
  {
    if (!Object.op_Equality((Object) this._main_banner, (Object) null))
      return;
    BannerGeneric<TMetaObject, TData>[] allRecursive = ((Component) this).gameObject.transform.FindAllRecursive<BannerGeneric<TMetaObject, TData>>((Func<Transform, bool>) (p => ((Component) p).gameObject.activeInHierarchy));
    if (allRecursive.Length == 1)
      this._main_banner = allRecursive[0];
    else
      Debug.LogError((object) $"WindowListElementBase: Failed to auto-find main banner. Assign manually. Found : {allRecursive.Length.ToString()} of type {typeof (BannerGeneric<TMetaObject, TData>)?.ToString()}");
  }

  private void initTooltip()
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: method pointer
    ((Component) this).GetComponent<Button>().OnHoverOut(WindowListElementBase<TMetaObject, TData>.\u003C\u003Ec.\u003C\u003E9__7_0 ?? (WindowListElementBase<TMetaObject, TData>.\u003C\u003Ec.\u003C\u003E9__7_0 = new UnityAction((object) WindowListElementBase<TMetaObject, TData>.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CinitTooltip\u003Eb__7_0))));
  }

  public void click()
  {
    if (!InputHelpers.mouseSupported && !Tooltip.isShowingFor((object) this))
    {
      this.tooltipAction();
    }
    else
    {
      MetaType metaType = this.meta_object.getMetaType();
      MetaTypeAsset asset = AssetManager.meta_type_library.getAsset(metaType);
      asset.set_selected((NanoObject) this.meta_object);
      if (asset.get_selected() == null)
        return;
      ScrollWindow.showWindow(asset.window_name);
    }
  }

  internal virtual void show(TMetaObject pObject)
  {
    this.meta_object = pObject;
    this.loadBanner();
    this.toggleFavorited(this.meta_object.isFavorite());
    if (!Object.op_Inequality((Object) this._icon_species, (Object) null))
      return;
    this._icon_species.sprite = this.getActorAsset().getSpriteIcon();
  }

  protected virtual void loadBanner() => this._main_banner.load((NanoObject) this.meta_object);

  protected virtual void tooltipAction() => throw new NotImplementedException();

  public void toggleFavorited(bool pState)
  {
    if (!Object.op_Inequality((Object) this._icon_favorite, (Object) null))
      return;
    this._icon_favorite.SetActive(pState);
  }

  protected virtual void OnDisable() => this.meta_object = default (TMetaObject);

  public void OnPointerMove(PointerEventData pData)
  {
    if (!InputHelpers.mouseSupported || Tooltip.anyActive())
      return;
    this.tooltipAction();
  }

  protected virtual ActorAsset getActorAsset() => throw new NotImplementedException();
}
