// Decompiled with JetBrains decompiler
// Type: AchievementPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class AchievementPopup : MonoBehaviour
{
  private static AchievementPopup _instance;
  [SerializeField]
  private Image _icon_left;
  [SerializeField]
  private Image _icon_right;
  [SerializeField]
  private Text _popup_text;
  [SerializeField]
  private Text _popup_description;
  [SerializeField]
  private AchievementGoodie _goodie_prefab;
  [SerializeField]
  private Transform _goodies_parent;
  private ObjectPoolGenericMono<AchievementGoodie> _goodie_pool;
  private Tweener _tween;

  private void Awake()
  {
    AchievementPopup._instance = this;
    this.hide();
  }

  internal static void show(string pAchievementID)
  {
    AchievementPopup._instance.showByID(pAchievementID);
  }

  internal static void show(Achievement pAchievement)
  {
    AchievementPopup._instance.showByID(pAchievement.id);
  }

  private void Update() => World.world.spawnCongratulationFireworks();

  internal void showByID(string pAchievementID)
  {
    if (this._tween != null && ((Tween) this._tween).active)
      return;
    ((Component) this).gameObject.SetActive(true);
    this.checkPool();
    Achievement achievement = AssetManager.achievements.get(pAchievementID);
    Sprite icon = achievement.getIcon();
    if (Object.op_Inequality((Object) icon, (Object) null))
    {
      this._icon_left.sprite = icon;
      this._icon_right.sprite = icon;
    }
    ((Component) this._popup_text).GetComponent<LocalizedText>().setKeyAndUpdate(achievement.getLocaleID());
    ((Component) this._popup_description).GetComponent<LocalizedText>().setKeyAndUpdate(achievement.getDescriptionID());
    double height1 = (double) Screen.height;
    Rect safeArea = Screen.safeArea;
    double height2 = (double) ((Rect) ref safeArea).height;
    this._tween = (Tweener) TweenSettingsExtensions.OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOLocalMoveY(((Component) this).transform, 0.0f - (float) (height1 - height2) / CanvasMain.instance.canvas_ui.scaleFactor, 1f, false), (Ease) 27), 0.2f), new TweenCallback(this.tweenHide));
    if (!achievement.unlocks_something)
      return;
    foreach (BaseUnlockableAsset unlockAsset in achievement.unlock_assets)
    {
      if (unlockAsset.show_for_unlockables_ui)
        this._goodie_pool.getNext().load(unlockAsset, true);
    }
  }

  public void forceHide()
  {
    if (this._tween != null)
      TweenExtensions.Kill((Tween) this._tween, false);
    this._tween = (Tweener) TweenSettingsExtensions.OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOLocalMoveY(((Component) this).transform, 200f, 0.5f, false), (Ease) 27), new TweenCallback(this.hide));
  }

  private void tweenHide()
  {
    this._tween = (Tweener) TweenSettingsExtensions.OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOLocalMoveY(((Component) this).transform, 200f, 1f, false), 4f), (Ease) 27), new TweenCallback(this.hide));
  }

  private void hide()
  {
    ((Component) this).gameObject.SetActive(false);
    this._goodie_pool?.clear();
  }

  private void checkPool()
  {
    if (this._goodie_pool != null)
      return;
    this._goodie_pool = new ObjectPoolGenericMono<AchievementGoodie>(this._goodie_prefab, this._goodies_parent);
  }
}
