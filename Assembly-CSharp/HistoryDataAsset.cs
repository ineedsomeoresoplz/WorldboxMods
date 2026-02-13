// Decompiled with JetBrains decompiler
// Type: HistoryDataAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ChartAndGraph;
using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[Serializable]
public class HistoryDataAsset : Asset, IDescriptionAsset, ILocalizedAsset
{
  public string localized_key;
  public string localized_key_description;
  public string statistics_asset;
  public string color_hex;
  public string tooltip_color_hex;
  public string path_icon;
  public bool enabled_default;
  private Material _material_point;
  private Material _material_line;
  private Material _material_gradient;
  private ChartItemEffect _hover_prefab;
  public bool average;
  public bool max;
  public bool sum;
  public GraphCategoryGroup category_group = GraphCategoryGroup.General;

  public Material getChartPointMaterial()
  {
    if (Object.op_Equality((Object) this._material_point, (Object) null))
    {
      this._material_point = HistoryDataAsset.cloneMaterial("materials/graph/graph_base_point");
      this._material_point.SetTexture("_MainTex", (Texture) Resources.Load<Texture2D>(this.path_icon));
    }
    return this._material_point;
  }

  public ChartItemEffect getHoverPointMaterial()
  {
    if (Object.op_Equality((Object) this._hover_prefab, (Object) null))
    {
      this._hover_prefab = HistoryDataAsset.clonePrefab("Prefabs/graph/PointHover", GameObject.Find("Charts").transform);
      ((Object) ((Component) this._hover_prefab).gameObject).name = "Hover " + this.id;
      ((Component) this._hover_prefab).GetComponent<Image>().sprite = SpriteTextureLoader.getSprite(this.path_icon);
      ((Component) this._hover_prefab).gameObject.SetActive(false);
    }
    return this._hover_prefab;
  }

  public Material getChartLineMaterial()
  {
    if (Object.op_Equality((Object) this._material_line, (Object) null))
      this._material_line = HistoryDataAsset.getChartLineMaterial(this.getColorMain());
    return this._material_line;
  }

  public static Material getChartLineMaterial(Color pColor)
  {
    Material chartLineMaterial = HistoryDataAsset.cloneMaterial("materials/graph/graph_base_line");
    chartLineMaterial.SetColor("_Color", pColor);
    return chartLineMaterial;
  }

  public Material getChartInnerFillMaterial()
  {
    if (Object.op_Equality((Object) this._material_gradient, (Object) null))
      this._material_gradient = HistoryDataAsset.getChartInnerFillMaterial(this.getColorMain());
    return this._material_gradient;
  }

  public static Material getChartInnerFillMaterial(Color pColor)
  {
    Material innerFillMaterial = HistoryDataAsset.cloneMaterial("materials/graph/graph_base_gradient");
    Color color1 = pColor;
    color1.a = 0.4f;
    Color color2 = pColor;
    color2.a = 0.1f;
    innerFillMaterial.SetColor("_ColorFrom", color1);
    innerFillMaterial.SetColor("_ColorTo", color2);
    return innerFillMaterial;
  }

  public string getLocaleID() => this.localized_key ?? this.id;

  public string getDescriptionID()
  {
    string pKey = this.getLocaleID() + "_description";
    if (!string.IsNullOrEmpty(this.localized_key_description))
      pKey = this.localized_key_description;
    return LocalizedTextManager.stringExists(pKey) ? pKey : this.getLocaleID() + "_description";
  }

  public Color getColorMain() => Toolbox.makeColor(this.color_hex);

  private static Material cloneMaterial(string pPath)
  {
    return Object.Instantiate<Material>(Resources.Load<Material>(pPath));
  }

  private static ChartItemEffect clonePrefab(string pPath, Transform pParentTransform)
  {
    return Object.Instantiate<ChartItemEffect>(Resources.Load<ChartItemEffect>(pPath), pParentTransform);
  }
}
