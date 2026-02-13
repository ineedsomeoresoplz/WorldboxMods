// Decompiled with JetBrains decompiler
// Type: UiCreature
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
public class UiCreature : MonoBehaviour
{
  public bool doFall;
  public bool doRotate;
  public bool doScale = true;
  public bool doFly;
  public bool doPlayPunch;
  public bool changeParent = true;
  public string doSfx = "none";
  private Tweener tweener_scale;
  private Tweener tweener_rotation;
  private Tweener tweener_move;
  private Vector3 _init_scale = Vector3.one;
  internal bool dropped;
  public string achievement = "";
  private Vector3 _initial_pos;
  private Quaternion _initial_rotation;
  private Transform _original_parent;
  private bool _forced_complete;

  private void Awake()
  {
    this._original_parent = ((Component) this).transform.parent;
    this._init_scale = ((Component) this).transform.localScale;
    this._initial_pos = ((Component) this).transform.localPosition;
    this._initial_rotation = ((Component) this).transform.rotation;
  }

  private void Start()
  {
    if (((Component) this).gameObject.HasComponent<Button>())
      return;
    // ISSUE: method pointer
    ((UnityEvent) ((Component) this).gameObject.AddComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(click)));
  }

  private void killTweens(bool pComplete = false)
  {
    if (pComplete)
      this._forced_complete = true;
    TweenExtensions.Kill((Tween) this.tweener_scale, pComplete);
    TweenExtensions.Kill((Tween) this.tweener_rotation, pComplete);
    TweenExtensions.Kill((Tween) this.tweener_move, pComplete);
    this._forced_complete = false;
  }

  internal void resetPosition()
  {
    this.killTweens();
    this.dropped = false;
    ((Component) this).transform.rotation = this._initial_rotation;
    ((Component) this).transform.localPosition = this._initial_pos;
    ((Component) this).transform.localScale = this._init_scale;
    ((Component) this).gameObject.SetActive(true);
  }

  public void click()
  {
    if (this.dropped)
      return;
    this.killTweens();
    if (((Component) this).HasComponent<HoveringIcon>())
      ((Component) this).GetComponent<HoveringIcon>().clear();
    if (((Component) this).HasComponent<LivingIcon>())
      ((Component) this).GetComponent<LivingIcon>().kill();
    if (!string.IsNullOrEmpty(this.achievement))
      AchievementLibrary.unlock(this.achievement);
    if (this.doPlayPunch)
      MusicBox.playSound("event:/SFX/OTHER/Punch");
    if (this.doSfx != "none" && !string.IsNullOrEmpty(this.doSfx) && this.doSfx.Contains("event:"))
      MusicBox.playSound(this.doSfx);
    if (this.doScale)
    {
      ((Component) this).transform.localScale = Vector3.op_Multiply(this._init_scale, 1.2f);
      this.tweener_scale = (Tweener) TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(((Component) this).transform, this._init_scale, 0.3f), (Ease) 27);
    }
    if (this.doFall)
      this.fall();
    if (this.doFly)
      this.flyAway();
    if (!this.doRotate)
      return;
    if (Randy.randomBool())
      this.tweener_rotation = (Tweener) TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(ShortcutExtensions.DORotate(((Component) this).transform, new Vector3(0.0f, 0.0f, Randy.randomFloat(90f, 180f)), 1f, (RotateMode) 0), (Ease) 9);
    else
      this.tweener_rotation = (Tweener) TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(ShortcutExtensions.DORotate(((Component) this).transform, new Vector3(0.0f, 0.0f, Randy.randomFloat(-180f, -90f)), 1f, (RotateMode) 0), (Ease) 9);
  }

  private void flyAway()
  {
    this.dropped = true;
    if (this.changeParent)
      ((Component) this).transform.parent = ((Component) CanvasMain.instance.canvas_tooltip).transform;
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(((Component) this).transform.position.x + Randy.randomFloat(-200f, 200f), 1000f, 0.0f);
    this.tweener_move = (Tweener) TweenSettingsExtensions.OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOMove(((Component) this).transform, vector3, 0.6f, false), (Ease) 5), new TweenCallback(this.completeFly));
  }

  private void fall()
  {
    this.dropped = true;
    if (this.changeParent)
      ((Component) this).transform.SetParent(((Component) CanvasMain.instance.canvas_tooltip).transform);
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(((Component) this).transform.position.x + Randy.randomFloat(-4f, 4f), ((Component) this).transform.position.y - (float) Screen.height, 0.0f);
    this.tweener_move = (Tweener) TweenSettingsExtensions.OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOMove(((Component) this).transform, vector3, 0.6f, false), (Ease) 5), new TweenCallback(this.completeFall));
  }

  private void completeFly()
  {
    ((Component) this).transform.SetParent(this._original_parent);
    ((Component) this).gameObject.SetActive(false);
  }

  private void completeFall()
  {
    if (this._forced_complete)
    {
      ((Component) this).gameObject.SetActive(false);
    }
    else
    {
      ((Component) this).transform.SetParent(this._original_parent);
      MusicBox.playSound("event:/SFX/HIT/HitStone");
      ((Component) this).gameObject.SetActive(false);
    }
  }

  private void OnEnable() => this.resetPosition();

  private void OnDisable() => this.killTweens(true);
}
