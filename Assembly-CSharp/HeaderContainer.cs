// Decompiled with JetBrains decompiler
// Type: HeaderContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class HeaderContainer : MonoBehaviour, ILayoutController
{
  public RectTransform header_transform;
  public RectTransform content_transform;
  public RectTransform runes_container;
  public VerticalLayoutGroup content;
  private VerticalLayoutGroup _vertical_layout_group;
  private LayoutElement _layout_element;
  private int _default_top_padding;
  private RectOffset _default_padding;

  private void Awake()
  {
    if (Object.op_Equality((Object) this.content, (Object) null))
      this.content = ((Component) this.content_transform).GetComponent<VerticalLayoutGroup>();
    this._vertical_layout_group = ((Component) this).GetComponent<VerticalLayoutGroup>();
    this._default_padding = ((LayoutGroup) this._vertical_layout_group).padding;
    this._layout_element = ((Component) this).GetComponent<LayoutElement>();
    this._default_top_padding = ((LayoutGroup) this.content).padding.top;
  }

  public void SetLayoutVertical()
  {
    if (!Application.isPlaying)
      return;
    int num1 = this._default_top_padding;
    if (!this.hasAnyElementActive())
    {
      if ((double) this._layout_element.preferredHeight != 0.0)
      {
        ((LayoutGroup) this._vertical_layout_group).padding = new RectOffset(0, 0, 0, 0);
        this._layout_element.preferredHeight = 0.0f;
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.header_transform);
      }
    }
    else
    {
      if ((double) this._layout_element.preferredHeight >= 0.0)
      {
        ((LayoutGroup) this._vertical_layout_group).padding = this._default_padding;
        this._layout_element.preferredHeight = -1f;
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.header_transform);
      }
      int num2 = num1;
      Rect rect = this.header_transform.rect;
      int height = (int) ((Rect) ref rect).height;
      num1 = num2 + height;
    }
    if (((LayoutGroup) this.content).padding.top == num1)
      return;
    ((LayoutGroup) this.content).padding.top = num1;
    LayoutRebuilder.ForceRebuildLayoutImmediate(this.content_transform);
    this.StartCoroutine(this.toggleRunes());
  }

  public void SetLayoutHorizontal()
  {
  }

  private IEnumerator toggleRunes()
  {
    yield return (object) null;
    bool flag = this.hasAnyElementActive();
    ((Component) this.runes_container).gameObject.SetActive(flag);
    if (flag)
    {
      RectTransform runesContainer = this.runes_container;
      double x = (double) ((Transform) this.runes_container).localPosition.x;
      Rect rect = this.header_transform.rect;
      double num = -(double) ((Rect) ref rect).height;
      Vector3 vector3 = Vector2.op_Implicit(new Vector2((float) x, (float) num));
      ((Transform) runesContainer).localPosition = vector3;
    }
  }

  private bool hasAnyElementActive()
  {
    for (int index = 0; index < ((Component) this).transform.childCount; ++index)
    {
      if (((Component) ((Component) this).transform.GetChild(index)).gameObject.activeInHierarchy)
        return true;
    }
    return false;
  }
}
