// Decompiled with JetBrains decompiler
// Type: TopPremiumButtonMover
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;

#nullable disable
public class TopPremiumButtonMover : MonoBehaviour
{
  public LocalizedText button_text;
  private float target_pos = -1f;
  private float pos_hide;
  private float pos_show = -45f;
  private DOTween _tween;

  private void Update()
  {
    if (this.shouldShow())
    {
      if ((double) this.target_pos == (double) this.pos_show)
        return;
      this.target_pos = this.pos_show;
      this.updateRandomText();
      ((Component) ((Component) this).transform).GetComponentInChildren<LocalizedTextPrice>().updateText();
      ShortcutExtensions.DOLocalMoveY(((Component) this).transform, this.target_pos, 0.5f, false);
    }
    else
    {
      if ((double) this.target_pos == (double) this.pos_hide)
        return;
      this.target_pos = this.pos_hide;
      ShortcutExtensions.DOLocalMoveY(((Component) this).transform, this.target_pos, 0.5f, false);
    }
  }

  private void updateRandomText()
  {
    int num = Randy.randomInt(1, 5);
    if (num > 1)
      this.button_text.key = "premium_get_it_" + num.ToString();
    this.button_text.updateText();
  }

  private bool shouldShow()
  {
    bool flag = false;
    if (Config.hasPremium)
      return false;
    string selectedPowerId = World.world.getSelectedPowerID();
    if (!string.IsNullOrEmpty(selectedPowerId) && AssetManager.powers.get(selectedPowerId).requires_premium)
      flag = true;
    return flag;
  }
}
