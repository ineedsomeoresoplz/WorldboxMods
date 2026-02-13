// Decompiled with JetBrains decompiler
// Type: CityElementNeutralHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class CityElementNeutralHandler : CityElement
{
  [SerializeField]
  private GameObject _layout_element_content_meta;
  [SerializeField]
  private GameObject _layout_element_wants;
  [SerializeField]
  private GameObject _layout_element_ruler;

  private void checkNeutralElements()
  {
    if (!this.meta_object.isNeutral())
      return;
    this._layout_element_content_meta.SetActive(false);
    this._layout_element_wants.SetActive(false);
    this._layout_element_ruler.SetActive(false);
  }

  protected override IEnumerator showContent()
  {
    this.checkNeutralElements();
    return base.showContent();
  }
}
