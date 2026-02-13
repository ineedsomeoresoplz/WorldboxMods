// Decompiled with JetBrains decompiler
// Type: BaseStatsHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine.UI;

#nullable disable
public static class BaseStatsHelper
{
  public static BaseStats _base_stats_tooltip_helper = new BaseStats();
  private static List<BaseStatsContainer> _stats_container_positive = new List<BaseStatsContainer>();
  private static List<BaseStatsContainer> _stats_container_negative = new List<BaseStatsContainer>();

  public static BaseStats getTotalStatsFrom(BaseStats pBaseStats, BaseStats pBaseStatsMeta)
  {
    BaseStatsHelper._base_stats_tooltip_helper.clear();
    BaseStatsHelper._base_stats_tooltip_helper.mergeStats(pBaseStats);
    BaseStatsHelper._base_stats_tooltip_helper.mergeStats(pBaseStatsMeta);
    return BaseStatsHelper._base_stats_tooltip_helper;
  }

  public static void showItemMods(Text pTextFieldDescription, Text pTextFieldValues, Item pItem)
  {
    // ISSUE: unable to decompile the method.
  }

  public static void showItemModsRows(BaseStatsHelper.KeyValueFieldGetter pFieldsFabric, Item pItem)
  {
    // ISSUE: unable to decompile the method.
  }

  private static ListPool<TooltipModContainerInfo> getItemModsBase(Item pItem)
  {
    // ISSUE: unable to decompile the method.
  }

  private static void addStatValues(
    Text pStatsField,
    Text pValuesField,
    string pStats,
    string pValues)
  {
    pStatsField.text += pStats;
    pValuesField.text += pValues;
  }

  private static void addLineBreak(Text pStatsField, Text pValuesField)
  {
    pStatsField.text += "\n";
    pValuesField.text += "\n";
  }

  private static int sortByPluses(
    TooltipModContainerInfo pContainer1,
    TooltipModContainerInfo pContainer2)
  {
    return pContainer2.pluses.CompareTo(pContainer1.pluses);
  }

  public static void showBaseStats(
    Text pStatsField,
    Text pValuesField,
    BaseStats pBaseStats,
    bool pAddPlus = true)
  {
    BaseStatsHelper.calcBaseStatsBase(pBaseStats);
    foreach (BaseStatsContainer pContainer in BaseStatsHelper._stats_container_positive)
      BaseStatsHelper.showBaseStatLine(pStatsField, pValuesField, pContainer, pAddPlus: pAddPlus);
    foreach (BaseStatsContainer pContainer in BaseStatsHelper._stats_container_negative)
      BaseStatsHelper.showBaseStatLine(pStatsField, pValuesField, pContainer, pAddPlus: pAddPlus);
  }

  public static void showBaseStatsRows(
    BaseStatsHelper.KeyValueFieldGetter pFieldsFabric,
    BaseStats pBaseStats,
    bool pAddPlus = true)
  {
    BaseStatsHelper.calcBaseStatsBase(pBaseStats);
    foreach (BaseStatsContainer pContainer in BaseStatsHelper._stats_container_positive)
      BaseStatsHelper.showBaseStatRow(pFieldsFabric, pContainer, pAddPlus: pAddPlus);
    foreach (BaseStatsContainer pContainer in BaseStatsHelper._stats_container_negative)
      BaseStatsHelper.showBaseStatRow(pFieldsFabric, pContainer, pAddPlus: pAddPlus);
  }

  private static void calcBaseStatsBase(BaseStats pBaseStats)
  {
    BaseStatsHelper._stats_container_positive.Clear();
    BaseStatsHelper._stats_container_negative.Clear();
    foreach (BaseStatsContainer pContainer in pBaseStats.getList())
    {
      if (!pContainer.asset.hidden || DebugConfig.isOn(DebugOption.ShowHiddenStats))
        BaseStatsHelper.queueStatContainer(pContainer);
    }
    BaseStatsHelper._stats_container_positive.Sort(new Comparison<BaseStatsContainer>(BaseStatsHelper.sortByRank));
  }

  private static int sortByRank(BaseStatsContainer pContainerA, BaseStatsContainer pContainerB)
  {
    BaseStatAsset asset = pContainerA.asset;
    return pContainerB.asset.sort_rank.CompareTo(asset.sort_rank);
  }

  private static void queueStatContainer(BaseStatsContainer pContainer)
  {
    if ((double) pContainer.value > 0.0)
      BaseStatsHelper._stats_container_positive.Add(pContainer);
    if ((double) pContainer.value >= 0.0)
      return;
    BaseStatsHelper._stats_container_negative.Add(pContainer);
  }

  private static void showBaseStatLine(
    Text pStatsField,
    Text pValuesField,
    BaseStatsContainer pContainer,
    bool pAddColor = true,
    bool pAddPlus = true,
    string pMainColor = "#43FF43",
    bool pForceZero = false)
  {
    string tId;
    float tValue;
    BaseStatAsset tAsset;
    BaseStatsHelper.calcBaseStatLineBase(pContainer, out tId, out tValue, out tAsset);
    if (!tAsset.hidden)
    {
      BaseStatsHelper.addItemText(pStatsField, pValuesField, tId, tValue, tAsset.show_as_percents, pAddColor, pAddPlus, pMainColor, pForceZero);
    }
    else
    {
      if (pStatsField.text.Length > 0)
        BaseStatsHelper.addLineBreak(pStatsField, pValuesField);
      string pText = tId;
      string text = tValue.ToText();
      if (tAsset.show_as_percents)
        text += " %";
      pValuesField.text += Toolbox.coloredText(text, ColorStyleLibrary.m.color_text_grey);
      pStatsField.text += Toolbox.coloredText(pText, ColorStyleLibrary.m.color_text_grey);
    }
  }

  private static void showBaseStatRow(
    BaseStatsHelper.KeyValueFieldGetter pFieldsFabric,
    BaseStatsContainer pContainer,
    bool pAddColor = true,
    bool pAddPlus = true,
    string pMainColor = "#43FF43",
    bool pForceZero = false)
  {
    string tId;
    float tValue;
    BaseStatAsset tAsset;
    BaseStatsHelper.calcBaseStatLineBase(pContainer, out tId, out tValue, out tAsset);
    BaseStatsHelper.addItemTextRow(pFieldsFabric(tId), tId, tValue, tAsset.show_as_percents, pAddColor, pAddPlus, pMainColor, pForceZero);
  }

  private static void calcBaseStatLineBase(
    BaseStatsContainer pContainer,
    out string tId,
    out float tValue,
    out BaseStatAsset tAsset)
  {
    tAsset = pContainer.asset;
    tId = tAsset.getLocaleID();
    tValue = pContainer.value;
    if ((double) tAsset.tooltip_multiply_for_visual_number != 1.0)
      tValue *= tAsset.tooltip_multiply_for_visual_number;
    if (!tAsset.hidden || !DebugConfig.isOn(DebugOption.ShowHiddenStats))
      return;
    tId = "[HIDDEN] " + tId;
  }

  private static void addItemText(
    Text pStatsField,
    Text pValuesField,
    string pID,
    float pValue,
    bool pPercent = false,
    bool pAddColor = true,
    bool pAddPlus = true,
    string pMainColor = "#43FF43",
    bool pForceZero = false)
  {
    string pValString;
    BaseStatsHelper.addItemTextBase(pValue, out pValString, pPercent, pForceZero);
    if (!pAddColor)
      BaseStatsHelper.addLineText(pStatsField, pValuesField, pID, pValString, pPercent: pPercent);
    else if ((double) pValue > 0.0)
    {
      if (pAddPlus)
        pValString = "+" + pValString;
      BaseStatsHelper.addLineText(pStatsField, pValuesField, pID, pValString, pMainColor, pPercent);
    }
    else
      BaseStatsHelper.addLineText(pStatsField, pValuesField, pID, pValString, "#FB2C21", pPercent);
  }

  private static void addItemTextRow(
    KeyValueField pField,
    string pID,
    float pValue,
    bool pPercent = false,
    bool pAddColor = true,
    bool pAddPlus = true,
    string pMainColor = "#43FF43",
    bool pForceZero = false)
  {
    string pValString;
    BaseStatsHelper.addItemTextBase(pValue, out pValString, pPercent, pForceZero);
    if (!pAddColor)
      BaseStatsHelper.addRowText(pField, pID, pValString, pPercent: pPercent);
    else if ((double) pValue > 0.0)
    {
      if (pAddPlus)
        pValString = "+" + pValString;
      BaseStatsHelper.addRowText(pField, pID, pValString, pMainColor, pPercent);
    }
    else
      BaseStatsHelper.addRowText(pField, pID, pValString, "#FB2C21", pPercent);
  }

  private static void addItemTextBase(
    float pValue,
    out string pValString,
    bool pPercent = false,
    bool pForceZero = false)
  {
    pValString = pValue.ToText();
    if ((double) pValue == 0.0 && !pForceZero || !pPercent)
      return;
    pValString += "%";
  }

  private static void addLineIntText(
    Text pStatsField,
    Text pValuesField,
    string pID,
    int pValue,
    string pColor = null)
  {
    BaseStatsHelper.addLineText(pStatsField, pValuesField, pID, pValue.ToText(), pColor);
  }

  private static void addLineText(
    Text pStatsField,
    Text pValuesField,
    string pID,
    string pValue,
    string pColor = null,
    bool pPercent = false)
  {
    if (pStatsField.text.Length > 0)
      BaseStatsHelper.addLineBreak(pStatsField, pValuesField);
    if (pValue.Length > 21)
      pValue = pValue.Substring(0, 20) + "...";
    string text = LocalizedTextManager.getText(pID);
    if (pPercent)
      text += " %";
    if (!string.IsNullOrEmpty(pColor))
    {
      pStatsField.text += text;
      pValuesField.text += Toolbox.coloredText(pValue, pColor);
    }
    else
    {
      pStatsField.text += text;
      pValuesField.text += pValue;
    }
  }

  private static void addRowText(
    KeyValueField pField,
    string pID,
    string pValue,
    string pColor = null,
    bool pPercent = false)
  {
    if (pValue.Length > 21)
      pValue = pValue.Substring(0, 20) + "...";
    string pText;
    if (pID.Contains("[HIDDEN]"))
    {
      pText = pID;
      pColor = ColorStyleLibrary.m.color_text_grey;
    }
    else
      pText = LocalizedTextManager.getText(pID);
    if (pPercent)
      pText += " %";
    if (!string.IsNullOrEmpty(pColor))
    {
      pField.name_text.text = Toolbox.coloredText(pText, pColor);
      pField.value.text = Toolbox.coloredText(pValue, pColor);
    }
    else
    {
      pField.name_text.text = pText;
      pField.value.text = pValue;
    }
  }

  public delegate KeyValueField KeyValueFieldGetter(string pID);
}
