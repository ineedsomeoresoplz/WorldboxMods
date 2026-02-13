// Decompiled with JetBrains decompiler
// Type: GraphCompareWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using db;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class GraphCompareWindow : MonoBehaviour
{
  public GraphCompareMetaObject meta_object_1;
  public GraphCompareMetaObject meta_object_2;
  public GraphCompareMetaObject meta_object_3;
  public GraphController graph_controller;
  [SerializeField]
  private GameObject _empty_list_message;
  [SerializeField]
  private RectTransform _meta_drag_object;
  private ObjectPoolGenericMono<RectTransform> _pool_drag_objects;
  private MultiBannerPool _pool_banners;
  private MultiBannerPool _pool_drop_banners;
  [SerializeField]
  private Button _noos_button;
  [SerializeField]
  private Image _noos_icon;
  [SerializeField]
  private Transform _noos_list_container;
  [SerializeField]
  private Transform _pool_banner_container;
  [SerializeField]
  private Transform _pool_drop_banner_container;
  private MetaTypeAsset _current_asset;
  private List<MetaTypeAsset> _noos_list = new List<MetaTypeAsset>();
  private List<NanoObject> _noos_items = new List<NanoObject>();
  private Coroutine _load_noos_items;
  private const int VISIBLE_ITEMS = 6;
  [SerializeField]
  private CanvasGroup[] _block_during_random;
  private bool _is_randomizing;
  private bool _stop_randomizer;

  private void Awake()
  {
    foreach (Transform transform in this._noos_list_container)
    {
      if (((Object) ((Component) transform).gameObject).name.StartsWith("MetaContainer"))
        Object.Destroy((Object) ((Component) transform).gameObject);
    }
    this._pool_drag_objects = new ObjectPoolGenericMono<RectTransform>(this._meta_drag_object, this._noos_list_container);
    this._pool_banners = new MultiBannerPool(this._pool_banner_container);
    this._pool_drop_banners = new MultiBannerPool(this._pool_drop_banner_container);
    // ISSUE: method pointer
    ((UnityEvent) this._noos_button.onClick).AddListener(new UnityAction((object) this, __methodptr(\u003CAwake\u003Eb__19_0)));
  }

  internal MultiBannerPool getDropBannerPool() => this._pool_drop_banners;

  private void OnEnable()
  {
    ScrollWindow.addCallbackHide(new ScrollWindowNameAction(this.resetPoolsAndParents));
    this.loadNoos();
    if (!this.hasAny())
      return;
    if (Config.selected_objects_graph.Count == 0)
      this.StartCoroutine(this.displayRandom());
    else
      this.StartCoroutine(this.displaySelected());
  }

  private IEnumerator selectNoosCoroutine()
  {
    if (Config.selected_objects_graph.Count != 0)
    {
      this.selectNoos(Config.selected_objects_graph.First());
      SoundBox.click();
      yield return (object) new WaitForEndOfFrame();
    }
  }

  private IEnumerator updateGraph()
  {
    if (Config.selected_objects_graph.Count != 0)
    {
      string activeCategory = this.graph_controller.getActiveCategory();
      this.graph_controller.resetAndUpdateGraph();
      this.graph_controller.tryEnableCategory(activeCategory);
      yield return (object) new WaitForEndOfFrame();
    }
  }

  private IEnumerator displaySelected(bool pUpdate = true)
  {
    if (Config.selected_objects_graph.Count != 0)
    {
      using (ListPool<NanoObject> tSelectedObjects = new ListPool<NanoObject>(3))
      {
        tSelectedObjects.Add(Config.selected_objects_graph[0]);
        tSelectedObjects.Add(Config.selected_objects_graph[1]);
        tSelectedObjects.Add(Config.selected_objects_graph[2]);
        this.meta_object_1.empty();
        this.meta_object_2.empty();
        this.meta_object_3.empty();
        Config.selected_objects_graph.Clear();
        this.meta_object_1.setObject(tSelectedObjects[0]);
        yield return (object) new WaitForEndOfFrame();
        this.meta_object_2.setObject(tSelectedObjects[1]);
        yield return (object) new WaitForEndOfFrame();
        this.meta_object_3.setObject(tSelectedObjects[2]);
        yield return (object) new WaitForEndOfFrame();
        if (pUpdate)
        {
          yield return (object) this.selectNoosCoroutine();
          yield return (object) this.updateGraph();
        }
      }
    }
  }

  private void OnDisable()
  {
    this.clearNoosItems();
    this.clearAsset();
  }

  private void clearAsset() => this._current_asset = (MetaTypeAsset) null;

  private void loadNoos()
  {
    this._noos_list.Clear();
    foreach (HistoryMetaDataAsset historyMetaDataAsset in AssetManager.history_meta_data_library.list)
    {
      MetaTypeAsset metaTypeAsset = AssetManager.meta_type_library.get(historyMetaDataAsset.id);
      if (metaTypeAsset.has_any())
        this._noos_list.Add(metaTypeAsset);
    }
    this.showItems(this.hasAny());
  }

  private bool hasAny() => this._noos_list.Count > 0;

  private void showItems(bool pShow)
  {
    Transform recursive = ((Component) this).transform.FindRecursive("Content");
    for (int index = 0; index < recursive.childCount; ++index)
      ((Component) recursive.GetChild(index)).gameObject.SetActive(pShow);
    this._empty_list_message.SetActive(!pShow);
  }

  private void updateNoosIcon(MetaTypeAsset pAsset)
  {
    this._noos_icon.sprite = SpriteTextureLoader.getSprite("ui/Icons/" + pAsset.icon_list);
  }

  public void clearNoosItems()
  {
    this._noos_items.Clear();
    this._pool_banners.clear();
    this._pool_drag_objects.clear();
  }

  private void resetNoosList()
  {
    ((Component) this._noos_list_container).GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
    ((Component) this._noos_list_container).GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
    this.clearNoosItems();
  }

  private void resetPoolsAndParents(string pID)
  {
    if (pID != "chart_comparer")
      return;
    this.StopAllCoroutines();
    this.clearNoosItems();
    this.meta_object_1.empty();
    this.meta_object_2.empty();
    this.meta_object_3.empty();
    ScrollWindow.removeCallbackHide(new ScrollWindowNameAction(this.resetPoolsAndParents));
  }

  public IEnumerator loadNoosItemsCoroutine(bool pSilent = false)
  {
    // ISSUE: unable to decompile the method.
  }

  public int countNoosItems() => this._noos_items.Count;

  public static int sortByUnits(NanoObject pNanoObject1, NanoObject pNanoObject2)
  {
    return ((IMetaObject) pNanoObject2).countUnits().CompareTo(((IMetaObject) pNanoObject1).countUnits());
  }

  private void nextNoos()
  {
    int num;
    this.selectNoos(this._noos_list[Toolbox.loopIndex(num = this._noos_list.IndexOf(this._current_asset) + 1, this._noos_list.Count)]);
  }

  private void selectNoos(NanoObject pObject)
  {
    this.selectNoos(AssetManager.meta_type_library.get(pObject.getType()));
  }

  private void selectNoos(MetaTypeAsset pAsset)
  {
    if (this._current_asset == pAsset)
      return;
    this.clearNoosItems();
    this._current_asset = pAsset;
    this.updateNoosIcon(this._current_asset);
    this.loadNoosItems();
  }

  public IBanner setupBanner(
    NanoObject pObject,
    Transform pBannerArea,
    MultiBannerPool pBannerPool)
  {
    IBanner next = pBannerPool.getNext(pObject);
    next.load(pObject);
    next.transform.localScale = new Vector3(1f, 1f, 1f);
    next.transform.SetParent(pBannerArea);
    UiButtonHoverAnimation component1 = next.GetComponent<UiButtonHoverAnimation>();
    ((Behaviour) component1).enabled = false;
    component1.scale_size = 1f;
    component1.default_scale = new Vector3(1f, 1f, 1f);
    next.GetComponent<TipButton>().setDefaultScale(pBannerArea.localScale);
    if (!next.HasComponent<LayoutElement>())
      next.AddComponent<LayoutElement>().ignoreLayout = true;
    RectTransform component2 = next.GetComponent<RectTransform>();
    component2.SetAnchor(AnchorPresets.MiddleCenter);
    ((Transform) component2).localScale = new Vector3(1f, 1f, 1f);
    component2.anchoredPosition = new Vector2(0.0f, 0.0f);
    return next;
  }

  private IBanner setupDragBanner(
    NanoObject pObject,
    Transform pBannerArea,
    MultiBannerPool pBannerPool)
  {
    IBanner banner = this.setupBanner(pObject, pBannerArea, pBannerPool);
    if (!banner.HasComponent<GraphCompareMetaSelector>())
    {
      GraphCompareMetaSelector compareMetaSelector = banner.AddComponent<GraphCompareMetaSelector>();
      compareMetaSelector.addWindow(this);
      compareMetaSelector.addDropzones(((Component) this.meta_object_1).GetComponent<RectTransform>(), ((Component) this.meta_object_2).GetComponent<RectTransform>(), ((Component) this.meta_object_3).GetComponent<RectTransform>());
    }
    return banner;
  }

  private ListPool<NanoObject> getPossibleItems()
  {
    ListPool<NanoObject> possibleItems = new ListPool<NanoObject>();
    foreach (MetaTypeAsset noos in this._noos_list)
    {
      foreach (NanoObject nanoObject in noos.get_list())
        possibleItems.Add(nanoObject);
    }
    return possibleItems;
  }

  internal void loadNoosItems(bool pSilent = false)
  {
    if (this._load_noos_items != null)
      this.StopCoroutine(this._load_noos_items);
    this._load_noos_items = this.StartCoroutine(this.loadNoosItemsCoroutine(pSilent));
  }

  private void selectRandom()
  {
    using (ListPool<NanoObject> possibleItems = this.getPossibleItems())
    {
      Config.selected_objects_graph.Clear();
      int pMax = Mathf.Min(possibleItems.Count, 3);
      foreach (NanoObject pObject in possibleItems.LoopRandom<NanoObject>(pMax))
        Config.selected_objects_graph.Add(pObject);
      if (possibleItems.Count > 7)
        return;
      this._stop_randomizer = true;
    }
  }

  public void randomizeSelection()
  {
    if (this._is_randomizing)
    {
      this._stop_randomizer = true;
    }
    else
    {
      this.StopAllCoroutines();
      this.StartCoroutine(this.displayRandom());
    }
  }

  private IEnumerator displayRandom()
  {
    this._is_randomizing = true;
    foreach (CanvasGroup canvasGroup in this._block_during_random)
    {
      canvasGroup.interactable = false;
      canvasGroup.blocksRaycasts = false;
    }
    for (int i = 0; i < 10 && !this._stop_randomizer; ++i)
    {
      this.selectRandom();
      yield return (object) this.displaySelected(false);
      yield return (object) this.randomizeCategories();
      yield return (object) this.updateGraph();
      yield return (object) this.randomNoosItems();
      this.updateNoosIcon(this._noos_list.GetRandom<MetaTypeAsset>());
    }
    yield return (object) this.randomizeCategories();
    yield return (object) this.randomizeTimescale();
    this.clearAsset();
    yield return (object) this.selectNoosCoroutine();
    foreach (CanvasGroup canvasGroup in this._block_during_random)
    {
      canvasGroup.interactable = true;
      canvasGroup.blocksRaycasts = true;
    }
    this._stop_randomizer = false;
    this._is_randomizing = false;
  }

  private IEnumerator randomizeCategories()
  {
    this.graph_controller.pickRandomCategory();
    SoundBox.click();
    yield return (object) new WaitForEndOfFrame();
  }

  private IEnumerator randomizeTimescale()
  {
    if (this.graph_controller.randomTimeScale())
    {
      SoundBox.click();
      yield return (object) new WaitForEndOfFrame();
    }
  }

  public IEnumerator randomNoosItems()
  {
    this.resetNoosList();
    using (ListPool<NanoObject> tPossibleItems = this.getPossibleItems())
    {
      foreach (NanoObject pObject in tPossibleItems.LoopRandom<NanoObject>(Mathf.Min(6, tPossibleItems.Count)))
      {
        RectTransform next = this._pool_drag_objects.getNext();
        ((Object) ((Component) next).gameObject).name = "MetaContainer " + pObject.getID().ToString();
        IBanner banner = this.setupDragBanner(pObject, ((Component) next).transform, this._pool_banners);
        if (Randy.randomBool())
          banner.jump(0.025f, true);
        yield return (object) new WaitForEndOfFrame();
      }
    }
  }
}
