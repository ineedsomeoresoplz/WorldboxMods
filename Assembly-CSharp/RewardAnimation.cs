// Decompiled with JetBrains decompiler
// Type: RewardAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class RewardAnimation : MonoBehaviour
{
  public Image boxSprite;
  public GameObject rewardTexts;
  public Text Text_free_power_unlocked;
  public Text Text_free_power_tap_to_unlock;
  private IconRotationAnimation _rotation_animation;
  public GameObject rewardedPowerIcon;
  private SpriteAnimation _sprite_animation;
  internal RewardAnimationState state;
  public LocalizedText bottomButtonText;
  private Vector3 _original_pos = Vector3.zero;
  public bool quickReward;
  private Tweener _icon_move_tween;
  private Tweener _icon_scale_tween;
  private Tweener _text_tween;
  public float rewardedPowerScaleTime = 0.45f;
  public float moveTime1 = 0.25f;
  public float moveTime2 = 0.25f;
  public float moveTime3 = 1.5f;
  public float moveTime4 = 1.5f;
  private Transform _icon_transform;
  private Transform _text_transform;

  private void Awake()
  {
    this._icon_transform = this.rewardedPowerIcon.transform;
    this._text_transform = this.rewardTexts.transform;
    this._rotation_animation = ((Component) this.boxSprite).GetComponent<IconRotationAnimation>();
    this._sprite_animation = ((Component) this.boxSprite).GetComponent<SpriteAnimation>();
    this._sprite_animation.Awake();
    if (!Vector3.op_Equality(this._original_pos, Vector3.zero))
      return;
    this._original_pos = this._icon_transform.localPosition;
  }

  public void OnEnable()
  {
    if (Vector3.op_Equality(this._original_pos, Vector3.zero))
      this._original_pos = this._icon_transform.localPosition;
    this.bottomButtonText.key = "free_power_button_open_in";
    this.bottomButtonText.updateText();
    this.resetAnim();
  }

  private void OnDisable()
  {
    TweenExtensions.Kill((Tween) this._icon_scale_tween, false);
    TweenExtensions.Kill((Tween) this._icon_move_tween, false);
    TweenExtensions.Kill((Tween) this._text_tween, false);
  }

  public void resetAnim()
  {
    this.state = RewardAnimationState.Idle;
    this._sprite_animation.resetAnim(3);
    ((Behaviour) this._rotation_animation).enabled = true;
    ShortcutExtensions.DOKill((Component) this._icon_transform, false);
    this.rewardedPowerIcon.SetActive(false);
    this.rewardTexts.SetActive(false);
    ((Component) this.Text_free_power_unlocked).gameObject.SetActive(false);
    ((Component) this.Text_free_power_tap_to_unlock).gameObject.SetActive(true);
  }

  private void Update()
  {
    if (this.quickReward && this._sprite_animation.currentFrameIndex < 7)
    {
      this._sprite_animation.currentFrameIndex = 7;
      this.showRewards(false);
      this.moveStageThree();
    }
    if (this.state != RewardAnimationState.Play && this.state != RewardAnimationState.Open)
      return;
    this._sprite_animation.update(Time.deltaTime);
    if (this._sprite_animation.currentFrameIndex <= 6 || this.state == RewardAnimationState.Open)
      return;
    this.showRewards();
  }

  private void showRewards(bool pStart = true)
  {
    this.state = RewardAnimationState.Open;
    this.rewardedPowerIcon.SetActive(true);
    TweenExtensions.Kill((Tween) this._text_tween, false);
    this._text_transform.localScale = new Vector3(0.5f, 0.5f);
    this._text_tween = (Tweener) TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(this._text_transform, new Vector3(1f, 1f, 1f), 0.3f), (Ease) 27);
    this.rewardTexts.gameObject.SetActive(true);
    ((Component) this.Text_free_power_unlocked).gameObject.SetActive(true);
    ((Component) this.Text_free_power_tap_to_unlock).gameObject.SetActive(false);
    this.bottomButtonText.key = "get_it";
    this.bottomButtonText.updateText();
    ShortcutExtensions.DOKill((Component) this._icon_transform, false);
    this._icon_transform.localPosition = this._original_pos;
    this._icon_transform.localScale = new Vector3(0.02f, 0.1f, 1f);
    if (pStart)
    {
      Vector3 vector3;
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3).\u002Ector(this._original_pos.x, this._original_pos.y, 0.0f);
      vector3.y += 22f;
      this._icon_move_tween = (Tweener) TweenSettingsExtensions.OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOLocalMove(this._icon_transform, vector3, this.moveTime1, false), (Ease) 21), new TweenCallback(this.moveStageTwo));
    }
    this._icon_scale_tween = (Tweener) TweenSettingsExtensions.OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(this._icon_transform, new Vector3(0.75f, 0.75f, 1f), this.rewardedPowerScaleTime), (Ease) 32 /*0x20*/), new TweenCallback(this.scaleStageTwo));
  }

  private void moveStageTwo()
  {
    TweenExtensions.Kill((Tween) this._icon_move_tween, false);
    this._icon_move_tween = (Tweener) TweenSettingsExtensions.OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOLocalMove(this._icon_transform, this._original_pos, this.moveTime2, false), (Ease) 7), new TweenCallback(this.moveStageThree));
  }

  private void moveStageThree()
  {
    TweenExtensions.Kill((Tween) this._icon_move_tween, false);
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(this._original_pos.x, this._original_pos.y, 1f);
    vector3.y += 3f;
    this._icon_move_tween = (Tweener) TweenSettingsExtensions.OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOLocalMove(this._icon_transform, vector3, this.moveTime3, false), (Ease) 7), new TweenCallback(this.moveStageFour));
  }

  private void moveStageFour()
  {
    TweenExtensions.Kill((Tween) this._icon_move_tween, false);
    this._icon_move_tween = (Tweener) TweenSettingsExtensions.OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOLocalMove(this._icon_transform, this._original_pos, this.moveTime4, false), (Ease) 7), new TweenCallback(this.moveStageThree));
  }

  private void scaleStageTwo()
  {
  }

  public void clickAnimation()
  {
    if (this._sprite_animation.currentFrameIndex > 5)
      return;
    this._sprite_animation.resetAnim();
    ((Behaviour) this._rotation_animation).enabled = false;
    ((Component) this._rotation_animation).transform.localScale = new Vector3(1f, 1f, 1f);
    if (this.state != RewardAnimationState.Idle)
      this.resetAnim();
    this.state = RewardAnimationState.Play;
  }
}
