// Decompiled with JetBrains decompiler
// Type: IconClickRotator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class IconClickRotator : MonoBehaviour
{
  private Quaternion _startRotation;
  private Coroutine _rotationRoutine;

  private void Awake()
  {
    // ISSUE: method pointer
    ((UnityEvent) ((Component) this).gameObject.AddOrGetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(click)));
    ((Component) this).gameObject.AddOrGetComponent<ScrollableButton>();
    this._startRotation = ((Component) this).transform.rotation;
  }

  private void click() => this.startRandomRotation();

  private void startRandomRotation()
  {
    if (this._rotationRoutine != null)
      this.StopCoroutine(this._rotationRoutine);
    this._rotationRoutine = this.StartCoroutine(this.RotateTo(Quaternion.Euler(0.0f, 0.0f, Random.Range(-180f, 180f)), 0.2f));
  }

  private IEnumerator RotateTo(Quaternion targetRotation, float duration)
  {
    IconClickRotator iconClickRotator = this;
    float time = 0.0f;
    Quaternion initialRotation = ((Component) iconClickRotator).transform.rotation;
    while ((double) time < (double) duration)
    {
      ((Component) iconClickRotator).transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, time / duration);
      time += Time.deltaTime;
      yield return (object) null;
    }
    ((Component) iconClickRotator).transform.rotation = targetRotation;
  }
}
