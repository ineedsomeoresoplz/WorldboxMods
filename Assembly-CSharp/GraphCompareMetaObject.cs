// Decompiled with JetBrains decompiler
// Type: GraphCompareMetaObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class GraphCompareMetaObject : MonoBehaviour, IDropHandler, IEventSystemHandler
{
  private GraphCompareWindow _graph_window;
  private GraphController _graph_controller;
  private MultiBannerPool _pool_drop_banners;
  public NanoObject current_item;
  public GameObject empty_drop_icon;
  public LocalizedText meta_title;
  public Text meta_name;
  private IBanner _current_banner;
  public static bool disable_raycasts;
  private bool _disable_raycasts;
  private List<Graphic> _raycast_children = new List<Graphic>();
  private bool _initialized;

  public void Awake() => this.init();

  private void init()
  {
    if (this._initialized)
      return;
    this._initialized = true;
    this._graph_window = ((Component) this).GetComponentInParent<GraphCompareWindow>();
    this._graph_controller = this._graph_window.graph_controller;
    this._pool_drop_banners = this._graph_window.getDropBannerPool();
  }

  public void OnEnable()
  {
    if (this.current_item != null)
      return;
    this.empty_drop_icon.SetActive(true);
    this.meta_title.setKeyAndUpdate("graph_drop_to_compare");
    ((Component) this.meta_name).gameObject.SetActive(false);
  }

  public void Update()
  {
    if (this._disable_raycasts == GraphCompareMetaObject.disable_raycasts)
      return;
    this._disable_raycasts = GraphCompareMetaObject.disable_raycasts;
    if (GraphCompareMetaObject.disable_raycasts)
      this.disableRaycastChildren();
    else
      this.enableRaycastChildren();
  }

  public void disableRaycastChildren()
  {
    this._raycast_children.Clear();
    foreach (Graphic componentsInChild in ((Component) this).GetComponentsInChildren<Graphic>())
    {
      if (!Object.op_Equality((Object) ((Component) componentsInChild).gameObject, (Object) ((Component) this).gameObject) && componentsInChild.raycastTarget)
      {
        this._raycast_children.Add(componentsInChild);
        componentsInChild.raycastTarget = false;
      }
    }
  }

  public void enableRaycastChildren()
  {
    foreach (Graphic raycastChild in this._raycast_children)
      raycastChild.raycastTarget = true;
    this._raycast_children.Clear();
  }

  public void OnDrop(PointerEventData pEventData)
  {
    if (Object.op_Equality((Object) pEventData.pointerDrag, (Object) null))
      return;
    BannerBase component1 = pEventData.pointerDrag.GetComponent<BannerBase>();
    if (Object.op_Equality((Object) component1, (Object) null))
      return;
    GraphCompareMetaSelector component2 = pEventData.pointerDrag.GetComponent<GraphCompareMetaSelector>();
    if (Object.op_Equality((Object) component2, (Object) null) || !component2.isBeingDragged())
      return;
    component2.OnEndDrag(pEventData);
    SoundBox.click();
    this.setObjectAndUpdate(component1.GetNanoObject());
    ((AbstractEventData) pEventData).Use();
  }

  public void empty()
  {
    this.init();
    this.clearObject();
    this.empty_drop_icon.SetActive(true);
  }

  public void clear()
  {
    this.init();
    Config.selected_objects_graph.Remove(this.current_item);
    this.clearObject();
    this.empty_drop_icon.SetActive(true);
  }

  public void clearAndSetObject(NanoObject pObject)
  {
    this.clear();
    this.setObject(pObject);
  }

  public void setObject(NanoObject pObject)
  {
    if (pObject.isRekt())
      return;
    this.empty_drop_icon.SetActive(false);
    this.current_item = pObject;
    this._current_banner = this._graph_window.setupBanner(this.current_item, ((Component) this).transform, this._pool_drop_banners);
    this._current_banner.jump();
    // ISSUE: method pointer
    ((UnityEvent) this._current_banner.GetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(removeOnClick)));
    if (!Config.selected_objects_graph.Contains(this.current_item))
      Config.selected_objects_graph.Add(this.current_item);
    Color colorText = this.current_item.getColor().getColorText();
    ((Graphic) this.meta_title.text).color = colorText;
    this.meta_title.setKeyAndUpdate(AssetManager.meta_customization_library.getAsset(this.current_item.getMetaType()).localization_title);
    ((Component) this.meta_name).gameObject.SetActive(true);
    this.meta_name.text = this.current_item.name;
    ((Graphic) this.meta_name).color = colorText;
  }

  private void setObjectAndUpdate(NanoObject pObject)
  {
    string activeCategory = this._graph_controller.getActiveCategory();
    this.clearAndSetObject(pObject);
    this._graph_window.loadNoosItems(true);
    this._graph_controller.resetAndUpdateGraph();
    this._graph_controller.tryEnableCategory(activeCategory);
  }

  private void removeOnClick()
  {
    SoundBox.click();
    if (!InputHelpers.mouseSupported && !Tooltip.isShowingFor((object) this._current_banner))
      this._current_banner.showTooltip();
    else
      this.setObjectAndUpdate((NanoObject) null);
  }

  private void clearObject()
  {
    if (this.current_item == null)
      return;
    this.releaseChild();
    this.current_item = (NanoObject) null;
    ((Graphic) this.meta_title.text).color = Toolbox.color_text_default;
    ((Graphic) this.meta_name).color = Toolbox.color_text_default;
    ((Component) this.meta_name).gameObject.SetActive(false);
  }

  private void releaseChild()
  {
    if (this._current_banner == null)
      return;
    // ISSUE: method pointer
    ((UnityEvent) this._current_banner.GetComponent<Button>().onClick).RemoveListener(new UnityAction((object) this, __methodptr(removeOnClick)));
    this._pool_drop_banners.resetParent(this._current_banner);
    this._pool_drop_banners.release(this._current_banner);
    this._current_banner = (IBanner) null;
  }
}
