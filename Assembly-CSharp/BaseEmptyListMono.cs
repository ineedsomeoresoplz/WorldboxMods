// Decompiled with JetBrains decompiler
// Type: BaseEmptyListMono
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BaseEmptyListMono : MonoBehaviour
{
  internal NanoObject meta_object;
  internal MonoBehaviour element;
  private bool has_element;
  private bool has_object;
  public RectTransform rect_transform;
  internal string debug_original_name;

  public void Awake() => this.rect_transform = ((Component) this).GetComponent<RectTransform>();

  public void assignObject(NanoObject pObject)
  {
    this.meta_object = pObject;
    this.has_object = true;
  }

  public void assignElement(MonoBehaviour pElement)
  {
    this.element = pElement;
    this.has_element = true;
  }

  public bool hasElement() => this.has_element;

  public void clearElement()
  {
    this.element = (MonoBehaviour) null;
    this.has_element = false;
  }

  public void clearObject()
  {
    this.meta_object = (NanoObject) null;
    this.has_object = false;
  }

  public bool hasObject() => this.has_object;

  public void debugUpdateName(bool tVisible)
  {
    if (string.IsNullOrEmpty(this.debug_original_name))
      this.debug_original_name = ((Object) ((Component) this).gameObject).name;
    if (tVisible)
      ((Object) ((Component) this).gameObject).name = $"(v) ({((Component) this).gameObject.transform.childCount.ToString()}) {this.debug_original_name}";
    else
      ((Object) ((Component) this).gameObject).name = $"(i) ({((Component) this).gameObject.transform.childCount.ToString()}) {this.debug_original_name}";
  }
}
