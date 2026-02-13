// Decompiled with JetBrains decompiler
// Type: UnitTextPhrases
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UnitTextPhrases : MonoBehaviour
{
  [SerializeField]
  private RectTransform _size_parent;
  [SerializeField]
  private Text _text;
  private Tweener _text_tweener;

  private void Awake() => this.finish();

  public void startNewTween(string pText, Transform pFollowObject)
  {
    ((Component) this).gameObject.SetActive(true);
    this.killTweens();
    Vector3 vector3_1;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_1).\u002Ector(0.0f, 0.0f, Randy.randomFloat(-30f, 30f));
    ((Transform) this._size_parent).localRotation = Quaternion.Euler(vector3_1);
    this._text.text = pText;
    Vector3 vector3_2;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_2).\u002Ector(0.0f, (float) Randy.randomInt(8, 12), 0.0f);
    if (Object.op_Equality((Object) pFollowObject, (Object) null))
      ((Component) this._text).transform.localPosition = vector3_2;
    else
      ((Component) this._text).transform.position = Vector3.op_Addition(pFollowObject.position, vector3_2);
    this._text.fontSize = Randy.randomInt(7, 9);
    Vector3 vector3_3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_3).\u002Ector(0.0f, Randy.randomFloat(30f, 60f), 0.0f);
    TweenExtensions.Kill((Tween) this._text_tweener, false);
    this._text_tweener = !Object.op_Equality((Object) pFollowObject, (Object) null) ? (Tweener) ShortcutExtensions.DOMove(((Component) this._text).transform, Vector3.op_Addition(vector3_3, pFollowObject.position), 3f, false) : (Tweener) ShortcutExtensions.DOLocalMove(((Component) this._text).transform, vector3_3, 3f, false);
    TweenSettingsExtensions.SetEase<Tweener>(this._text_tweener, (Ease) 9);
    ((Tween) DOTweenModuleUI.DOColor(this._text, Color.white, 1.25f)).onComplete = new TweenCallback(this.doTextFade);
  }

  private void doTextFade()
  {
    ((Tween) DOTweenModuleUI.DOFade(this._text, 0.0f, 2f)).onComplete = new TweenCallback(this.finish);
  }

  public bool isTweening() => TweenExtensions.IsActive((Tween) this._text_tweener);

  private void finish()
  {
    this.killTweens();
    ((Graphic) this._text).color = Toolbox.color_white_transparent;
    ((Component) this).gameObject.SetActive(false);
  }

  private void killTweens()
  {
    Tweener textTweener = this._text_tweener;
    if (textTweener != null)
      TweenExtensions.Kill((Tween) textTweener, false);
    ShortcutExtensions.DOKill((Component) this._text, false);
  }
}
