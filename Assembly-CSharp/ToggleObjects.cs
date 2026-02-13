// Decompiled with JetBrains decompiler
// Type: ToggleObjects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
[RequireComponent(typeof (Button))]
public class ToggleObjects : MonoBehaviour
{
  [SerializeField]
  private List<GameObject> _elements;

  private void Awake()
  {
    // ISSUE: method pointer
    ((UnityEvent) ((Component) this).GetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(toggle)));
  }

  private void toggle()
  {
    if (this._elements == null)
      return;
    foreach (GameObject element in this._elements)
      element.SetActive(!element.activeSelf);
  }
}
