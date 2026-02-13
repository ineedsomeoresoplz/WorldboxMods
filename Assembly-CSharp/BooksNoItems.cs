// Decompiled with JetBrains decompiler
// Type: BooksNoItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BooksNoItems : MonoBehaviour
{
  private GameObject _inner;
  private IBooksWindow _books_window;

  private void Awake()
  {
    this._inner = ((Component) ((Component) this).transform.GetChild(0)).gameObject;
    this._books_window = ((Component) this).GetComponentInParent<IBooksWindow>();
  }

  private void OnEnable() => this._inner.SetActive(!this._books_window.hasBooks());
}
