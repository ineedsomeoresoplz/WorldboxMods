// Decompiled with JetBrains decompiler
// Type: GraphController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ChartAndGraph;
using db;
using db.tables;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class GraphController : MonoBehaviour
{
  public static MinMax min_max;
  public GraphChart chart;
  [SerializeField]
  private MetaType _meta_type = MetaType.City;
  private GraphCategoriesContainer _container_graph_categories;
  private GraphTimeScaleContainer _container_time_scale;
  public bool clear_on_enable;
  public bool multi_chart;
  private VerticalAxis _vertical_axis;
  private HorizontalAxis _horizontal_axis;
  private List<HistoryDataAsset> _list_categories = new List<HistoryDataAsset>();
  private Dictionary<string, bool> _category_enabled = new Dictionary<string, bool>();
  private Dictionary<string, MinMax> _min_max_categories = new Dictionary<string, MinMax>();
  private long _min_timestamp = long.MinValue;
  private long _max_timestamp = long.MaxValue;
  private HashSet<MetaType> _current_types = new HashSet<MetaType>();
  private List<NanoObject> _current_objects = new List<NanoObject>();
  private Dictionary<NanoObject, HistoryTable> _last_data = new Dictionary<NanoObject, HistoryTable>();
  private GraphTimeAsset _current_sample;
  private HistoryInterval _current_interval;
  private Dictionary<string, CategoryData> _current_datas = new Dictionary<string, CategoryData>();
  private bool _events_hooked;
  private bool _loaded;
  private bool _categories_loaded;
  private long _last_timestamp = -1;

  private void Awake()
  {
    this._container_time_scale = ((Component) ((Component) this).transform).GetComponentInChildren<GraphTimeScaleContainer>();
    this._vertical_axis = ((Component) ((Component) this).transform).GetComponentInChildren<VerticalAxis>();
    this._horizontal_axis = ((Component) ((Component) this).transform).GetComponentInChildren<HorizontalAxis>();
    this._container_graph_categories = ((Component) ((Component) this).transform).GetComponentInChildren<GraphCategoriesContainer>();
  }

  internal List<NanoObject> getObjects() => this._current_objects;

  internal List<HistoryDataAsset> getCategories() => this._list_categories;

  internal bool hasCategory(HistoryDataAsset pCategory)
  {
    return this._list_categories.Contains(pCategory);
  }

  internal bool hasCategory(string pCategory)
  {
    return this.hasCategory(AssetManager.history_data_library.get(pCategory));
  }

  private static string getCategoryName(string pCategory)
  {
    return GraphHelpers.getCategoryName(pCategory);
  }

  private NanoObject extractObject(string pCategory)
  {
    if (!pCategory.Contains('|'))
      return (NanoObject) null;
    string str1 = pCategory.Split('|', StringSplitOptions.None)[0];
    string str2 = pCategory.Split('|', StringSplitOptions.None)[1];
    string str3 = pCategory.Split('|', StringSplitOptions.None)[2];
    foreach (NanoObject currentObject in this._current_objects)
    {
      if (currentObject.getType() == str2 && currentObject.getTypeID() == str3)
        return currentObject;
    }
    return (NanoObject) null;
  }

  internal bool isCategoryEnabled(string pCategory)
  {
    return this._category_enabled[GraphController.getCategoryName(pCategory)];
  }

  internal string getActiveCategory()
  {
    foreach (string key in this._category_enabled.Keys)
    {
      if (this._category_enabled[key])
        return key;
    }
    return (string) null;
  }

  private void loadCategories()
  {
    if (this._categories_loaded)
      return;
    this._categories_loaded = true;
    this._list_categories.Clear();
    HashSet<HistoryDataAsset> historyDataAssetSet = new HashSet<HistoryDataAsset>();
    foreach (MetaType currentType in this._current_types)
    {
      HistoryMetaDataAsset[] assets = AssetManager.history_meta_data_library.getAssets(currentType);
      HashSet<HistoryDataAsset> other = new HashSet<HistoryDataAsset>();
      foreach (HistoryMetaDataAsset historyMetaDataAsset in assets)
        other.UnionWith((IEnumerable<HistoryDataAsset>) historyMetaDataAsset.categories);
      if (historyDataAssetSet.Count == 0)
        historyDataAssetSet.UnionWith((IEnumerable<HistoryDataAsset>) other);
      else
        historyDataAssetSet.IntersectWith((IEnumerable<HistoryDataAsset>) other);
    }
    foreach (NanoObject currentObject in this._current_objects)
    {
      foreach (HistoryDataAsset historyDataAsset in historyDataAssetSet)
      {
        if (!this.hasCategory(historyDataAsset))
          this.addCategory(historyDataAsset, historyDataAsset.enabled_default);
        this.colorCategory(historyDataAsset, currentObject, this.multi_chart);
      }
    }
  }

  internal void addCategory(HistoryDataAsset pAsset, bool pEnabled = false)
  {
    this._list_categories.Add(pAsset);
    this._category_enabled[pAsset.id] = pEnabled;
  }

  internal void disableAllCategories(string pExcept = null)
  {
    foreach (HistoryDataAsset category in this.getCategories())
    {
      if (!(category.id == pExcept))
        this.setCategoryEnabled(category.id, false, false);
    }
  }

  internal void pickRandomCategory()
  {
    using (ListPool<string> list = GraphHelpers.bestCategories(this._min_max_categories))
    {
      if (list.Count == 0)
        return;
      this.tryEnableCategory(list.GetRandom<string>());
    }
  }

  internal void tryEnableCategory(string pCategory)
  {
    if (string.IsNullOrEmpty(pCategory) || !this.hasCategory(pCategory))
      return;
    this._container_graph_categories.setCategoryEnabled(pCategory, true);
  }

  internal void setCategoryEnabled(string pCategory, bool pIsOn, bool pUpdateGraph = true)
  {
    this._category_enabled[pCategory] = pIsOn;
    foreach (string categoryName in ((GraphChartBase) this.chart).DataSource.CategoryNames)
    {
      if (categoryName.StartsWith(pCategory + "|"))
        ((GraphChartBase) this.chart).DataSource.SetCategoryEnabled(categoryName, pIsOn);
    }
    if (!pUpdateGraph)
      return;
    this.updateGraph();
  }

  private void hookEvents()
  {
    if (this._events_hooked)
      return;
    this._events_hooked = true;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: method pointer
    ((UnityEvent<GraphChartBase.GraphEventArgs>) ((GraphChartBase) this.chart).PointHovered).AddListener(GraphController.\u003C\u003Ec.\u003C\u003E9__39_0 ?? (GraphController.\u003C\u003Ec.\u003C\u003E9__39_0 = new UnityAction<GraphChartBase.GraphEventArgs>((object) GraphController.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003ChookEvents\u003Eb__39_0))));
    if (this.multi_chart)
    {
      // ISSUE: method pointer
      ((UnityEvent<GraphChartBase.GraphEventArgs>) ((GraphChartBase) this.chart).PointHovered).AddListener(new UnityAction<GraphChartBase.GraphEventArgs>((object) this, __methodptr(multiChartHover)));
    }
    else
    {
      // ISSUE: method pointer
      ((UnityEvent<GraphChartBase.GraphEventArgs>) ((GraphChartBase) this.chart).PointHovered).AddListener(new UnityAction<GraphChartBase.GraphEventArgs>((object) this, __methodptr(singleChartHover)));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: method pointer
    ((GraphChartBase) this.chart).NonHovered.AddListener(GraphController.\u003C\u003Ec.\u003C\u003E9__39_1 ?? (GraphController.\u003C\u003Ec.\u003C\u003E9__39_1 = new UnityAction((object) GraphController.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003ChookEvents\u003Eb__39_1))));
  }

  private void multiChartHover(GraphChartBase.GraphEventArgs pArgs)
  {
    long tYear = (long) pArgs.Value.x;
    string tCategoryName = GraphController.getCategoryName(pArgs.Category);
    if (Tooltip.anyActive())
    {
      Tooltip active = Tooltip.findActive((Predicate<Tooltip>) (pTooltip => !(pTooltip.asset.id != "graph_multi_resource") && !(pTooltip.data.tip_name != tCategoryName) && pTooltip.data.custom_data_long["year"] == tYear));
      if (Object.op_Inequality((Object) active, (Object) null))
      {
        active.reposition();
        return;
      }
    }
    CustomDataContainer<string> customDataContainer1 = new CustomDataContainer<string>();
    CustomDataContainer<long> customDataContainer2 = new CustomDataContainer<long>();
    customDataContainer2["year"] = tYear;
    foreach (string categoryName in ((GraphChartBase) this.chart).DataSource.CategoryNames)
    {
      if (this.isCategoryEnabled(categoryName))
      {
        NanoObject nanoObject = this.extractObject(categoryName);
        string name = nanoObject.name;
        (long tValue, long tPrevious) = this.getCategoryValueAtTime(categoryName, (long) pArgs.Value.x);
        customDataContainer2[name] = tValue;
        customDataContainer2[name + "_previous"] = tPrevious;
        customDataContainer1[name] = Toolbox.colorToHex(Color32.op_Implicit(nanoObject.getColor().getColorText()));
      }
    }
    Tooltip.show((object) pArgs.Position, "graph_multi_resource", new TooltipData()
    {
      tip_name = tCategoryName,
      custom_data_long = customDataContainer2,
      custom_data_string = customDataContainer1
    });
  }

  private void singleChartHover(GraphChartBase.GraphEventArgs pArgs)
  {
    Tooltip.hideTooltip();
    CustomDataContainer<long> customDataContainer = new CustomDataContainer<long>();
    customDataContainer["year"] = (long) pArgs.Value.x;
    foreach (string categoryName1 in ((GraphChartBase) this.chart).DataSource.CategoryNames)
    {
      if (this.isCategoryEnabled(categoryName1))
      {
        string categoryName2 = GraphController.getCategoryName(categoryName1);
        (long tValue, long tPrevious) = this.getCategoryValueAtTime(categoryName1, (long) pArgs.Value.x);
        customDataContainer[categoryName2] = tValue;
        customDataContainer[categoryName2 + "_previous"] = tPrevious;
      }
    }
    NanoObject nanoObject = this.extractObject(pArgs.Category);
    Tooltip.show((object) pArgs.Position, "graph_resource", new TooltipData()
    {
      custom_data_long = customDataContainer,
      nano_object = nanoObject
    });
  }

  public void resetAndUpdateGraph()
  {
    this._loaded = false;
    this._categories_loaded = false;
    this._current_interval = HistoryInterval.None;
    this._container_time_scale.resetTimeScale();
    this.updateGraph();
    this._container_time_scale.calcBounds();
  }

  public bool randomTimeScale()
  {
    if (!this._container_time_scale.randomizeTimeScale())
      return false;
    this.updateGraph();
    return true;
  }

  public void forceUpdateGraph() => this.updateGraph();

  private void updateGraph()
  {
    if (Config.disable_db || !Config.graphs)
      return;
    ((ScrollableChartData) ((GraphChartBase) this.chart).DataSource).StartBatch();
    this.loadGraph();
    if (this._container_time_scale.resetTimeScale())
      this.clearChartData();
    this.loadSample();
    this.loadCategoryAndCharts();
    this.adjustCharts();
    ((ScrollableChartData) ((GraphChartBase) this.chart).DataSource).EndBatch();
  }

  private void loadGraph()
  {
    if (this._loaded)
      return;
    this._loaded = true;
    ((AxisBase) ((Component) this.chart).GetComponent<HorizontalAxis>()).CustomNumberFormatWorldbox = new Func<double, int, string>(GraphHelpers.horizontalFormatYears);
    ((AnyChart) this.chart).CustomNumberFormat = new Func<double, int, string>(GraphHelpers.verticalFormat);
    ((Behaviour) this._vertical_axis).enabled = true;
    ((Behaviour) this._horizontal_axis).enabled = true;
    if (this.multi_chart)
      this.loadMultiChart();
    else
      this.loadSingleChart();
    this.hookEvents();
  }

  private void loadSingleChart()
  {
    this.selectContainer(AssetManager.meta_type_library.getAsset(this._meta_type).get_selected());
  }

  private void loadMultiChart()
  {
    this._current_types.Clear();
    this._current_objects.Clear();
    this._last_data.Clear();
    foreach (NanoObject pMetaObject in Config.selected_objects_graph)
    {
      if (pMetaObject != null && pMetaObject.isAlive())
        this.addContainer(pMetaObject);
    }
    this.clearChartData();
    this._category_enabled.Clear();
  }

  private void showCategory(string pCategory, NanoObject pObject)
  {
    string type = pObject.getType();
    string typeId = pObject.getTypeID();
    CategoryData currentData = this._current_datas[typeId];
    string str = $"{pCategory}|{type}|{typeId}";
    for (LinkedListNode<Dictionary<string, long>> linkedListNode = currentData.Last; linkedListNode != null; linkedListNode = linkedListNode.Previous)
    {
      if (linkedListNode.Value.ContainsKey(pCategory))
      {
        long num1 = linkedListNode.Value[pCategory];
        long num2 = linkedListNode.Value["timestamp"];
        bool flag = false;
        LinkedListNode<Dictionary<string, long>> previous = linkedListNode.Previous;
        long num3 = previous != null ? previous.Value[pCategory] : 0L;
        LinkedListNode<Dictionary<string, long>> next = linkedListNode.Next;
        long num4 = next != null ? next.Value[pCategory] : 0L;
        if (num1 == num3 && num1 == num4)
          flag = true;
        ((GraphChartBase) this.chart).DataSource.AddPointToCategory(str, (double) num2, (double) num1, flag ? 0.0 : -1.0);
      }
    }
  }

  private (long tValue, long tPrevious) getCategoryValueAtTime(string pCategory, long pTime)
  {
    string categoryName = GraphController.getCategoryName(pCategory);
    CategoryData currentData = this._current_datas[pCategory.Split('|', StringSplitOptions.None).Last<string>()];
    long num1 = 0;
    long num2 = 0;
    bool flag = false;
    for (LinkedListNode<Dictionary<string, long>> linkedListNode = currentData.Last; linkedListNode != null; linkedListNode = linkedListNode.Previous)
    {
      if (linkedListNode.Value.ContainsKey(categoryName))
      {
        if (flag)
        {
          num2 = linkedListNode.Value[categoryName];
          break;
        }
        long num3 = linkedListNode.Value["timestamp"];
        if (num3 <= pTime)
        {
          if (num3 <= pTime)
            num1 = linkedListNode.Value[categoryName];
          flag = true;
        }
      }
    }
    return (num1, num2);
  }

  private void colorCategory(
    HistoryDataAsset pHistoryDataAsset,
    NanoObject pObject,
    bool pColorFromObject = false)
  {
    string type = pObject.getType();
    string typeId = pObject.getTypeID();
    string id = pHistoryDataAsset.id;
    string str = $"{id}|{type}|{typeId}";
    float num1 = 2f;
    MaterialTiling materialTiling = new MaterialTiling();
    materialTiling.EnableTiling = false;
    bool flag = true;
    Material chartLineMaterial;
    Material innerFillMaterial;
    if (pColorFromObject)
    {
      Color colorText = pObject.getColor().getColorText();
      chartLineMaterial = HistoryDataAsset.getChartLineMaterial(colorText);
      innerFillMaterial = HistoryDataAsset.getChartInnerFillMaterial(colorText);
    }
    else
    {
      chartLineMaterial = pHistoryDataAsset.getChartLineMaterial();
      innerFillMaterial = pHistoryDataAsset.getChartInnerFillMaterial();
    }
    ((GraphChartBase) this.chart).DataSource.AddCategory(str, chartLineMaterial, (double) num1, materialTiling, innerFillMaterial, flag, (Material) null, 0.0, false);
    ((GraphChartBase) this.chart).DataSource.SetCategoryEnabled(str, this.isCategoryEnabled(id));
    ((GraphChartBase) this.chart).DataSource.Set2DCategoryPrefabs(str, (ChartItemEffect) null, pHistoryDataAsset.getHoverPointMaterial());
    int num2 = 10;
    ((GraphChartBase) this.chart).DataSource.SetCategoryPoint(str, pHistoryDataAsset.getChartPointMaterial(), (double) num2);
  }

  private MinMax getMinMax(string pCategoryName)
  {
    long pMin = long.MaxValue;
    long pMax = long.MinValue;
    bool flag = false;
    string key1 = pCategoryName.Split('|', StringSplitOptions.None)[0];
    string str = pCategoryName.Split('|', StringSplitOptions.None)[1];
    string key2 = pCategoryName.Split('|', StringSplitOptions.None)[2];
    if (this._current_datas.Count == 0 || !this._current_datas.ContainsKey(key2))
      return new MinMax(0L, 0L);
    for (LinkedListNode<Dictionary<string, long>> linkedListNode = this._current_datas[key2].Last; linkedListNode != null; linkedListNode = linkedListNode.Previous)
    {
      Dictionary<string, long> dictionary = linkedListNode.Value;
      if (dictionary.ContainsKey(key1))
      {
        long num1 = dictionary[key1];
        long num2 = dictionary["timestamp"];
        if (!flag || num2 >= this._min_timestamp)
        {
          if (num1 < pMin)
            pMin = num1;
          if (num1 > pMax)
            pMax = num1;
          flag = true;
        }
        else
          break;
      }
    }
    return !flag ? new MinMax(0L, 0L) : new MinMax(pMin, pMax);
  }

  internal void adjustCharts()
  {
    long num1 = long.MaxValue;
    long num2 = 0;
    this._min_max_categories.Clear();
    foreach (string categoryName in ((GraphChartBase) this.chart).DataSource.CategoryNames)
    {
      MinMax minMax = this.getMinMax(categoryName);
      this._min_max_categories.Add(categoryName, minMax);
      if (this.isCategoryEnabled(categoryName))
      {
        if (minMax.max > num2)
          num2 = minMax.max;
        if (minMax.min < num1)
          num1 = minMax.min;
      }
    }
    long num3 = GraphHelpers.calculateNiceMaxAxisSize((double) num2 * 1.05);
    int num4 = GraphHelpers.findVerticalDivision(num3);
    long pValue;
    if (num1 >= 0L)
    {
      pValue = 0L;
    }
    else
    {
      pValue = GraphHelpers.calculateNiceMaxAxisSize((double) -num1 * 1.05);
      if (pValue < num3)
      {
        long num5 = num3 / (long) num4;
        int num6 = Mathf.CeilToInt((float) pValue / (float) num5);
        pValue = (long) num6 * num5;
        num4 += num6;
      }
      else
      {
        int verticalDivision = GraphHelpers.findVerticalDivision(pValue);
        long num7 = pValue / (long) verticalDivision;
        int num8 = Mathf.CeilToInt((float) num3 / (float) num7);
        num3 = (long) num8 * num7;
        num4 = verticalDivision + num8;
      }
    }
    ((ScrollableChartData) ((GraphChartBase) this.chart).DataSource).VerticalViewOrigin = (double) -pValue;
    ((ScrollableChartData) ((GraphChartBase) this.chart).DataSource).VerticalViewSize = (double) (num3 + pValue);
    ((ScrollableChartData) ((GraphChartBase) this.chart).DataSource).HorizontalViewOrigin = (double) GraphTimeLibrary.getMinTime(this._current_sample);
    ((ScrollableChartData) ((GraphChartBase) this.chart).DataSource).HorizontalViewSize = (double) (GraphTimeLibrary.getMaxTime(this._current_sample) - GraphTimeLibrary.getMinTime(this._current_sample));
    ((ChartDivisionInfo) ((AxisBase) this._horizontal_axis).MainDivisions).Total = 5;
    ((ChartDivisionInfo) ((AxisBase) this._horizontal_axis).MainDivisions).FractionDigits = 2;
    ((ChartDivisionInfo) ((AxisBase) this._vertical_axis).MainDivisions).Total = num4;
    GraphController.min_max = new MinMax(-pValue, num3);
  }

  private void loadSample()
  {
    GraphTimeScale currentScale = this._container_time_scale.getCurrentScale();
    this._current_sample = AssetManager.graph_time_library.get(currentScale.ToString());
    bool flag = false;
    this._current_interval = this._current_sample.interval;
    foreach (NanoObject currentObject in this._current_objects)
    {
      string typeId = currentObject.getTypeID();
      CategoryData pData;
      if (!this._current_datas.TryGetValue(typeId, out pData))
      {
        pData = new CategoryData();
        this._current_datas[typeId] = pData;
      }
      if (DBGetter.getData(pData, currentObject, this._current_interval, this._last_data[currentObject]))
        flag = true;
    }
    if (flag)
      this.clearChartData();
    this._min_timestamp = GraphTimeLibrary.getMinTime(this._current_sample);
    this._max_timestamp = GraphTimeLibrary.getMaxTime(this._current_sample);
  }

  private void clearChartData()
  {
    this._categories_loaded = false;
    ((ScrollableChartData) ((GraphChartBase) this.chart).DataSource).Clear();
  }

  private void loadCategoryAndCharts()
  {
    this.loadCategories();
    foreach (NanoObject currentObject in this._current_objects)
    {
      foreach (Asset listCategory in this._list_categories)
        this.showCategory(listCategory.id, currentObject);
    }
    this._container_graph_categories.apply();
  }

  private void selectContainer(NanoObject pMetaObject)
  {
    MetaType metaType = pMetaObject.getMetaType();
    if (!this._current_types.Contains(metaType))
    {
      this._category_enabled.Clear();
      this.clearChartData();
    }
    else if (!this._current_objects.Contains(pMetaObject))
      this.clearChartData();
    this._current_types.Clear();
    this._current_objects.Clear();
    this._last_data.Clear();
    this._current_types.Add(metaType);
    this._current_objects.Add(pMetaObject);
    foreach (HistoryMetaDataAsset asset in AssetManager.history_meta_data_library.getAssets(metaType))
    {
      this._last_data[pMetaObject] = asset.collector(pMetaObject);
      this._last_data[pMetaObject].timestamp = (long) Date.getCurrentYear();
    }
  }

  private void addContainer(NanoObject pMetaObject)
  {
    MetaType metaType = pMetaObject.getMetaType();
    this._current_types.Add(metaType);
    this._current_objects.Add(pMetaObject);
    foreach (HistoryMetaDataAsset asset in AssetManager.history_meta_data_library.getAssets(metaType))
    {
      this._last_data[pMetaObject] = asset.collector(pMetaObject);
      this._last_data[pMetaObject].timestamp = (long) Date.getCurrentYear();
    }
  }

  private void clearGraph()
  {
    this.clearChartData();
    this._category_enabled.Clear();
    this._list_categories.Clear();
    this._loaded = false;
    this._container_graph_categories.apply();
  }

  internal void load()
  {
    this._loaded = false;
    if (this.multi_chart)
    {
      this._current_interval = HistoryInterval.None;
      this._container_time_scale.resetTimeScale();
    }
    if (this.clear_on_enable)
      this.clearGraph();
    if (this._last_timestamp != (long) Date.getMonthsSince(0.0))
    {
      this._last_timestamp = (long) Date.getMonthsSince(0.0);
      foreach (CategoryData categoryData in this._current_datas.Values)
        categoryData.Dispose();
      this._current_datas.Clear();
      this.clearChartData();
    }
    this.updateGraph();
    this._container_time_scale.calcBounds();
  }

  private void clear()
  {
    this._current_types.Clear();
    this._current_objects.Clear();
    this._last_data.Clear();
  }

  private void OnEnable() => this.load();

  private void OnDisable() => this.clear();
}
