// Decompiled with JetBrains decompiler
// Type: EmptyLogElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EmptyLogElement : MonoBehaviour
{
  private WorldLogElement _log_element;
  public RectTransform rect_transform;
  private WorldLogElement _element;
  private WorldLogMessage _message;

  public void load(WorldLogMessage pMessage) => this._message = pMessage;

  public void setElement(WorldLogElement pElement)
  {
    this._element = pElement;
    if (Object.op_Equality((Object) this._element, (Object) null))
      return;
    this._element.showMessage(this._message);
    ((Component) pElement).transform.SetParent(((Component) this).transform);
  }

  public WorldLogElement getElement() => this._element;
}
