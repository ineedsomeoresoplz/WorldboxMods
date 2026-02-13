// Decompiled with JetBrains decompiler
// Type: UiGameStat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UiGameStat : MonoBehaviour
{
  public Text nameText;
  public Text valueText;
  private LocalizedText _localized_text;
  internal long lastStat;
  internal Tweener curTween;
  private StatisticsAsset _asset;
  private float _timeout;

  private void Awake()
  {
    this._localized_text = ((Component) this.nameText).GetComponent<LocalizedText>();
  }

  public void setAsset(StatisticsAsset pAsset) => this._asset = pAsset;

  private void Update()
  {
    if ((double) this._timeout > 0.0)
    {
      this._timeout -= Time.deltaTime;
    }
    else
    {
      this._timeout = 1f;
      this.updateText();
    }
  }

  internal void updateText()
  {
    if (!Config.game_loaded || LocalizedTextManager.instance == null || this._asset == null)
      return;
    if (StatsHelper.getStat(this._asset.id) > 0L)
    {
      long stat = StatsHelper.getStat(this._asset.id);
      if (stat != this.lastStat)
      {
        this.checkDestroyTween();
        float duration = 0.95f;
        this.curTween = (Tweener) this.valueText.DORandomCounter(this.lastStat, stat, duration);
        this.lastStat = stat;
      }
    }
    else
      this.valueText.text = StatsHelper.getStatistic(this._asset.id);
    this._localized_text.setKeyAndUpdate(this._asset.getLocaleID());
    if (LocalizedTextManager.current_language.isRTL())
    {
      this.nameText.alignment = (TextAnchor) 5;
      this.valueText.alignment = (TextAnchor) 3;
    }
    else
    {
      this.nameText.alignment = (TextAnchor) 3;
      this.valueText.alignment = (TextAnchor) 5;
    }
  }

  private void OnEnable() => this.updateText();

  private void OnDisable()
  {
    this.checkDestroyTween();
    this.lastStat = 0L;
  }

  private void checkDestroyTween()
  {
    if (this.curTween == null || !((Tween) this.curTween).active)
      return;
    TweenExtensions.Complete((Tween) this.curTween, false);
    TweenExtensions.Kill((Tween) this.curTween, false);
    this.curTween = (Tweener) null;
  }
}
