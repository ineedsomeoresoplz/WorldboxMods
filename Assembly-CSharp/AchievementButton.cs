// Decompiled with JetBrains decompiler
// Type: AchievementButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class AchievementButton : MonoBehaviour
{
  private Achievement _achievement;
  [SerializeField]
  private Image _icon;
  [SerializeField]
  private Image _background_completed;
  [SerializeField]
  private Image _background_legendary;
  [SerializeField]
  private GameObject _background_default;
  [SerializeField]
  private GameObject _icon_medal;

  public void Load(Achievement pAchievement)
  {
    this._achievement = pAchievement;
    Sprite icon = this._achievement.getIcon();
    if (Object.op_Inequality((Object) icon, (Object) null))
    {
      this._icon.sprite = icon;
      if (!AchievementLibrary.isUnlocked(this._achievement))
      {
        ((Graphic) this._icon).color = Color.black;
        this._background_default.SetActive(true);
        ((Behaviour) ((Component) this._background_completed).GetComponent<Image>()).enabled = false;
        this._icon_medal.SetActive(false);
      }
    }
    if (pAchievement.unlocks_something)
      ((Component) this._background_legendary).gameObject.SetActive(true);
    else
      ((Component) this._background_legendary).gameObject.SetActive(false);
    ((Object) this).name = this._achievement.id;
  }

  private void Start()
  {
    ((Component) this).transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
    Button component = ((Component) this).GetComponent<Button>();
    // ISSUE: method pointer
    ((UnityEvent) component.onClick).AddListener(new UnityAction((object) this, __methodptr(showTooltip)));
    // ISSUE: method pointer
    component.OnHover(new UnityAction((object) this, __methodptr(showHoverTooltip)));
    // ISSUE: method pointer
    component.OnHoverOut(new UnityAction((object) null, __methodptr(hideTooltip)));
  }

  private void showHoverTooltip()
  {
    if (!Config.tooltips_active)
      return;
    this.showTooltip();
  }

  private void showTooltip()
  {
    Tooltip.show((object) this, "achievement", new TooltipData()
    {
      achievement = this._achievement
    });
    ((Component) this).transform.localScale = new Vector3(1f, 1f, 1f);
    ShortcutExtensions.DOKill((Component) ((Component) this).transform, false);
    TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(((Component) this).transform, 0.8f, 0.1f), (Ease) 26);
  }
}
