// Decompiled with JetBrains decompiler
// Type: StatsRowsContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class StatsRowsContainer : MonoBehaviour
{
  protected StatsWindow stats_window;
  private ObjectPoolGenericMono<KeyValueField> _stats_pool;
  protected List<KeyValueField> stats_rows = new List<KeyValueField>();

  private void Awake() => this.init();

  private void OnEnable()
  {
    this.showStats();
    this.StartCoroutine(this.showRows());
  }

  protected virtual void init()
  {
    this.stats_window = ((Component) this).GetComponentInParent<StatsWindow>();
    this._stats_pool = new ObjectPoolGenericMono<KeyValueField>(Resources.Load<KeyValueField>("ui/KeyValueFieldStats"), ((Component) this).transform);
  }

  protected virtual void showStats() => this.stats_window.showStatsRows();

  private IEnumerator showRows()
  {
    foreach (Component statsRow in this.stats_rows)
    {
      statsRow.gameObject.SetActive(true);
      yield return (object) CoroutineHelper.wait_for_next_frame;
    }
  }

  private void OnDisable()
  {
    this._stats_pool.clear();
    this.stats_rows.Clear();
  }

  internal KeyValueField getStatRow(string pKey)
  {
    KeyValueField next = this._stats_pool.getNext();
    ((Object) ((Component) next).gameObject).name = "[KV] " + pKey;
    ((Component) next).gameObject.SetActive(false);
    this.stats_rows.Add(next);
    return next;
  }

  internal KeyValueField showStatRow(
    string pId,
    object pValue,
    string pColor,
    MetaType pMetaType = MetaType.None,
    long pMetaId = -1,
    bool pColorText = false,
    string pIconPath = null,
    string pTooltipId = null,
    TooltipDataGetter pTooltipData = null,
    bool pLocalize = true)
  {
    KeyValueField statRow = this.getStatRow(pId);
    bool flag = !string.IsNullOrEmpty(pIconPath);
    if (flag)
    {
      Sprite sprite = SpriteTextureLoader.getSprite("ui/Icons/" + pIconPath);
      statRow.icon.sprite = sprite;
    }
    ((Component) statRow.icon).gameObject.SetActive(flag);
    string pText = pLocalize ? LocalizedTextManager.getText(pId) : pId;
    statRow.name_text.text = !string.IsNullOrEmpty(pId) ? (!pColorText ? pText : Toolbox.coloredString(pText, pColor)) : "";
    statRow.value.text = string.IsNullOrEmpty(pColor) ? pValue.ToString() : Toolbox.coloredString(pValue.ToString(), pColor);
    statRow.setMetaForTooltip(pMetaType, pMetaId, pTooltipId, pTooltipData);
    return statRow;
  }
}
