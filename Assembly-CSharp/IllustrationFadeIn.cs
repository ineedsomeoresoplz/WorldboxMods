// Decompiled with JetBrains decompiler
// Type: IllustrationFadeIn
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
public class IllustrationFadeIn : MonoBehaviour
{
  public float scale_start = 1.5f;
  public float scale_end = 1f;
  public float duration = 1f;
  public Ease ease_type = (Ease) 12;

  private void Awake()
  {
    Button button;
    if (!((Component) this).TryGetComponent<Button>(ref button))
      button = ((Component) this).gameObject.AddComponent<Button>();
    // ISSUE: method pointer
    ((UnityEvent) button.onClick).AddListener(new UnityAction((object) this, __methodptr(onCLick)));
    ((Graphic) ((Component) this).GetComponent<Image>()).raycastTarget = true;
  }

  private void OnEnable() => this.startTween();

  public void startTween()
  {
    Vector3 vector3_1;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_1).\u002Ector(this.scale_start, this.scale_start, this.scale_start);
    Vector3 vector3_2;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_2).\u002Ector(this.scale_end, this.scale_end, this.scale_end);
    ShortcutExtensions.DOKill((Component) ((Component) this).transform, false);
    TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.From<Vector3, Vector3, VectorOptions>(ShortcutExtensions.DOScale(((Component) this).transform, vector3_2, this.duration), vector3_1, true, false), this.ease_type);
  }

  public void onCLick() => this.startTween();
}
