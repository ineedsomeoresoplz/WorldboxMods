// Decompiled with JetBrains decompiler
// Type: StatisticsRows
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class StatisticsRows : MonoBehaviour
{
  [SerializeField]
  private KeyValueField _stat_prefab;
  private List<(KeyValueField field, MetaType type, string id)> _all_stats = new List<(KeyValueField, MetaType, string)>();

  private void Awake() => this.init();

  private void OnEnable() => this.refreshStats();

  private void OnDisable()
  {
    foreach ((KeyValueField field, MetaType type, string id) allStat in this._all_stats)
      ((Component) allStat.field).gameObject.SetActive(false);
  }

  protected virtual void init() => throw new NotImplementedException();

  protected void addStatRow(StatisticsAsset pAsset)
  {
    string id = pAsset.id;
    MetaType worldStatsMetaType = pAsset.world_stats_meta_type;
    KeyValueField pField = Object.Instantiate<KeyValueField>(this._stat_prefab, ((Component) this).transform);
    UiGameStat component = ((Component) pField).GetComponent<UiGameStat>();
    component.setAsset(pAsset);
    component.updateText();
    bool flag = !string.IsNullOrEmpty(pAsset.path_icon);
    if (flag)
    {
      Sprite icon = pAsset.getIcon();
      pField.icon.sprite = icon;
    }
    ((Component) pField.icon).gameObject.SetActive(flag);
    this.setupField(pField, worldStatsMetaType, id);
    ((Component) pField).gameObject.SetActive(false);
    this._all_stats.Add((pField, worldStatsMetaType, id));
  }

  private void refreshStats()
  {
    foreach ((KeyValueField field, MetaType type, string id) allStat in this._all_stats)
      this.setupField(allStat.field, allStat.type, allStat.id);
    this.StartCoroutine(this.refreshRoutine());
  }

  private void setupField(KeyValueField pField, MetaType pMetaType, string pID)
  {
    StatisticsAsset tAsset = AssetManager.statistics_library.get(pID);
    if (pMetaType.isNone())
    {
      TooltipDataGetter pData = (TooltipDataGetter) (() => new TooltipData()
      {
        tip_name = tAsset.getLocaleID(),
        tip_description = tAsset.getDescriptionID()
      });
      pField.setMetaForTooltip(pMetaType, -1L, "normal", pData);
    }
    else
    {
      KeyValueField keyValueField = pField;
      int pMetaType1 = (int) pMetaType;
      MetaIdGetter getMetaId = tAsset.get_meta_id;
      long pMetaId = getMetaId != null ? getMetaId(tAsset) : -1L;
      keyValueField.setMetaForTooltip((MetaType) pMetaType1, pMetaId);
    }
  }

  private IEnumerator refreshRoutine()
  {
    foreach ((KeyValueField, MetaType, string) allStat in this._all_stats)
    {
      ((Component) allStat.Item1).gameObject.SetActive(true);
      yield return (object) new WaitForSecondsRealtime(0.005f);
    }
  }
}
