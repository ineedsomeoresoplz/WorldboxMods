// Decompiled with JetBrains decompiler
// Type: StatIconDarkToggle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class StatIconDarkToggle : MonoBehaviour
{
  private Color _original_color;
  private Image _background;
  private const int INDEX_MAX = 3;
  private const float SHADE_FACTOR = 0.5f;
  private int _switched_index;

  private void changeColor()
  {
    if (Object.op_Equality((Object) this._background, (Object) null))
      return;
    ++this._switched_index;
    if (this._switched_index >= 3)
      this._switched_index = 0;
    float num = (float) (1.0 - (double) this._switched_index / 3.0 * 0.5);
    Color color;
    // ISSUE: explicit constructor call
    ((Color) ref color).\u002Ector(this._original_color.r * num, this._original_color.g * num, this._original_color.b * num, this._original_color.a);
    ((Graphic) this._background).color = color;
  }

  private void Awake()
  {
    // ISSUE: method pointer
    ((UnityEvent) ((Component) this).gameObject.AddOrGetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(click)));
    this._background = ((Component) this).GetComponent<Image>();
    if (Object.op_Inequality((Object) this._background, (Object) null))
      this._original_color = ((Graphic) this._background).color;
    else
      this._original_color = Color.white;
  }

  private void click() => this.changeColor();
}
