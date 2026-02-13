// Decompiled with JetBrains decompiler
// Type: ColorAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class ColorAsset : Asset
{
  private static int _create_last_index_id = 1000;
  public int index_id;
  public string color_main;
  public string color_main_2;
  public string color_banner;
  public string color_text;
  public bool favorite;
  [NonSerialized]
  public Color32 k_color_0;
  [NonSerialized]
  public Color32 k_color_1;
  [NonSerialized]
  public Color32 k_color_2;
  [NonSerialized]
  public Color32 k_color_3;
  [NonSerialized]
  public Color32 k_color_4;
  [NonSerialized]
  public Color32 k2_color_0;
  [NonSerialized]
  public Color32 k2_color_1;
  [NonSerialized]
  public Color32 k2_color_2;
  [NonSerialized]
  public Color32 k2_color_3;
  [NonSerialized]
  public Color32 k2_color_4;
  private Color32 _color_main_32;
  private Color32 _color_main_second_32;
  private Color32 _color_unit_32;
  private Color32 _color_border_inside_alpha_32;
  private Color _color_main;
  private Color _color_main_second;
  private Color _color_text;
  private Color _color_minimap_element;
  private Color _color_border_out_capture;
  private Color _color_banner;
  private static readonly List<ColorAsset> _all_colors_list = new List<ColorAsset>();
  private static readonly Dictionary<string, ColorAsset> _all_colors_dict = new Dictionary<string, ColorAsset>();
  public const byte ALPHA_BORDER_INSIDE_BYTE = 170;
  private bool _initialized;
  private Material _material_line;
  private Material _material_gradient;

  public static List<ColorAsset> getAllColorsList() => ColorAsset._all_colors_list;

  public ColorAsset()
  {
  }

  public static bool isColorAssetExists(string pColorMain)
  {
    return ColorAsset._all_colors_dict.ContainsKey(pColorMain);
  }

  public static ColorAsset getExistingColorAsset(string pColorMain)
  {
    ColorAsset existingColorAsset;
    ColorAsset._all_colors_dict.TryGetValue(pColorMain, out existingColorAsset);
    return existingColorAsset;
  }

  public static ColorAsset tryMakeNewColorAsset(string pColorMain)
  {
    ColorAsset colorAsset;
    ColorAsset._all_colors_dict.TryGetValue(pColorMain, out colorAsset);
    if (colorAsset == null)
      colorAsset = new ColorAsset(pColorMain);
    return colorAsset;
  }

  private ColorAsset(string pColorMain)
  {
    this.setMainHexColors(pColorMain, pColorMain, pColorMain);
    this.index_id = ColorAsset._create_last_index_id++;
    ColorAsset.saveToGlobalList(this);
  }

  public static void saveToGlobalList(ColorAsset pAsset, bool pMustBeGlobal = false)
  {
    if (ColorAsset.isColorAssetExists(pAsset.color_main))
    {
      if (!pMustBeGlobal)
        return;
      Debug.LogError((object) $"ColorAsset with same <b>color_main</b> already exists in global list: {pAsset.id} {pAsset.index_id.ToString()} {pAsset.color_main}");
    }
    else
    {
      ColorAsset._all_colors_list.Add(pAsset);
      ColorAsset._all_colors_dict.Add(pAsset.color_main, pAsset);
    }
  }

  private void setMainHexColors(string pColorMain, string pColorMain2, string pColorBanner)
  {
    this.color_main = pColorMain;
    this.color_main_2 = pColorMain2;
    this.color_banner = pColorBanner;
    this.color_text = pColorMain;
  }

  public void setEditorColors(Color pMain, Color pMain2, Color pBanner, Color pText)
  {
    this.initColor();
  }

  public void initColor()
  {
    if (this._initialized)
      return;
    this._initialized = true;
    this._color_main = Toolbox.makeColor(this.color_main);
    this._color_main_32 = Color32.op_Implicit(Toolbox.makeColor(this.color_main));
    this._color_main_second = Toolbox.makeColor(this.color_main_2);
    this._color_main_second_32 = Color32.op_Implicit(Toolbox.makeColor(this.color_main_2));
    this._color_text = Toolbox.makeColor(this.color_text);
    this._color_banner = Toolbox.makeColor(this.color_banner);
    Color color = Color32.op_Implicit(this._color_main_32);
    this._color_border_inside_alpha_32 = Color32.op_Implicit(new Color(color.r, color.g, color.b));
    this._color_border_inside_alpha_32.a = (byte) 170;
    Color32 color32;
    // ISSUE: explicit constructor call
    ((Color32) ref color32).\u002Ector((byte) 30, (byte) 30, (byte) 30, byte.MaxValue);
    this._color_border_out_capture = new Color(this._color_main_second.r, this._color_main_second.g, this._color_main_second.b, 0.8f);
    this._color_unit_32 = Color32.op_Implicit(Color.Lerp(this._color_main_second, Color.white, 0.3f));
    this._color_unit_32.a = byte.MaxValue;
    this.k_color_0 = Color32.op_Implicit(this._color_text);
    this.k_color_0 = this.checkIfColorTooDark(this.k_color_0);
    this._color_minimap_element = Color32.op_Implicit(Color32.Lerp(this.k_color_0, Color32.op_Implicit(Color.white), 0.2f));
    this.k_color_1 = Color32.Lerp(this.k_color_0, color32, 0.13f);
    this.k_color_2 = Color32.Lerp(this.k_color_0, color32, 0.350000024f);
    this.k_color_3 = Color32.Lerp(this.k_color_0, color32, 0.51f);
    this.k_color_4 = Color32.Lerp(this.k_color_0, color32, 0.659999967f);
    this.k2_color_0 = this._color_main_32;
    this.k2_color_0 = this.checkIfColorTooDark(this.k2_color_0);
    this.k2_color_1 = Color32.Lerp(this.k2_color_0, color32, 0.13f);
    this.k2_color_2 = Color32.Lerp(this.k2_color_0, color32, 0.350000024f);
    this.k2_color_3 = Color32.Lerp(this.k2_color_0, color32, 0.51f);
    this.k2_color_4 = Color32.Lerp(this.k2_color_0, color32, 0.659999967f);
  }

  public Material getChartLineMaterial()
  {
    if (Object.op_Equality((Object) this._material_line, (Object) null))
    {
      this._material_line = this.cloneMaterial("materials/graph/graph_base_line");
      this._material_line.SetColor("_Color", this.getColorText());
    }
    return this._material_line;
  }

  public Material getChartInnerFillMaterial()
  {
    if (Object.op_Equality((Object) this._material_gradient, (Object) null))
    {
      this._material_gradient = this.cloneMaterial("materials/graph/graph_base_gradient");
      Color colorText1 = this.getColorText();
      colorText1.a = 0.4f;
      Color colorText2 = this.getColorText();
      colorText2.a = 0.1f;
      this._material_gradient.SetColor("_ColorFrom", colorText1);
      this._material_gradient.SetColor("_ColorTo", colorText2);
    }
    return this._material_gradient;
  }

  private Material cloneMaterial(string pPath)
  {
    return Object.Instantiate<Material>(Resources.Load<Material>(pPath));
  }

  private Color32 checkIfColorTooDark(Color32 pColor)
  {
    if (pColor.r < (byte) 128 /*0x80*/ && pColor.g < (byte) 128 /*0x80*/ && pColor.b < (byte) 128 /*0x80*/)
    {
      pColor.r += (byte) 50;
      pColor.g += (byte) 50;
      pColor.b += (byte) 50;
    }
    return pColor;
  }

  private Color32 getDarkerColor(Color32 pColor, byte pValue)
  {
    pColor.r += pValue;
    pColor.g += pValue;
    pColor.b += pValue;
    return pColor;
  }

  public Color32 getColorUnit32() => this._color_unit_32;

  public Color32 getColorMain32() => this._color_main_32;

  public Color32 getColorBorderInsideAlpha32() => this._color_border_inside_alpha_32;

  public Color32 getColorMainSecond32() => this._color_main_second_32;

  public Color getColorMainSecond() => this._color_main_second;

  public Color getColorMain() => this._color_main;

  public Color getColorText() => this._color_text;

  public ref Color getColorTextRef() => ref this._color_text;

  public Color getColorMinimapElements() => this._color_minimap_element;

  public Color getColorBorderOut_capture() => this._color_border_out_capture;

  public Color getColorBanner() => this._color_banner;
}
