// Decompiled with JetBrains decompiler
// Type: BrushWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BrushWindow : MonoBehaviour
{
  public Transform circles;
  public Transform squares;
  public Transform diamonds;
  public Transform special;
  public BrushSelectButton button_prefab;

  public void Awake()
  {
    foreach (BrushData pBrushData in AssetManager.brush_library.list)
    {
      if (pBrushData.show_in_brush_window)
      {
        Transform transform;
        switch (pBrushData.group)
        {
          case BrushGroup.Circles:
            transform = this.circles;
            break;
          case BrushGroup.Squares:
            transform = this.squares;
            break;
          case BrushGroup.Diamonds:
            transform = this.diamonds;
            break;
          case BrushGroup.Special:
            transform = this.special;
            break;
          default:
            continue;
        }
        Object.Instantiate<BrushSelectButton>(this.button_prefab, transform).setup(pBrushData);
      }
    }
  }

  public void selectBrush(GameObject pObject)
  {
    Config.current_brush = ((Object) pObject.transform).name;
    ((Component) this).GetComponent<ScrollWindow>().clickHide();
  }
}
