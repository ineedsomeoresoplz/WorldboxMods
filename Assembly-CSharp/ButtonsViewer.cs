// Decompiled with JetBrains decompiler
// Type: ButtonsViewer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ButtonsViewer : MonoBehaviour
{
  private List<PowerButton> buttons;
  private Transform content;
  private float lastX;
  private float lastY;
  private Canvas canvas;

  private void Start()
  {
    this.content = ((Component) this).transform.parent;
    this.canvas = CanvasMain.instance.canvas_ui;
    this.buttons = new List<PowerButton>();
    int childCount = ((Component) this).transform.childCount;
    for (int index = 0; index < ((Component) this).transform.childCount; ++index)
    {
      GameObject gameObject = ((Component) ((Component) this).transform.GetChild(index)).gameObject;
      if (gameObject.HasComponent<PowerButton>() && gameObject.activeSelf)
        this.buttons.Add(gameObject.GetComponent<PowerButton>());
      else if (!gameObject.HasComponent<Image>() || !gameObject.activeSelf)
        Object.Destroy((Object) gameObject);
    }
  }

  private void Update()
  {
    if ((double) this.lastX == (double) this.content.position.x && (double) this.lastY == (double) this.content.position.y)
      return;
    this.lastX = this.content.position.x;
    this.lastY = this.content.position.y;
    int num1 = 0;
    int num2 = 0;
    bool flag = false;
    for (int index = 0; index < this.buttons.Count; ++index)
    {
      PowerButton button = this.buttons[index];
      if (flag)
      {
        ++num2;
        ((Component) button).gameObject.SetActive(false);
      }
      else
      {
        ++num1;
        Vector3[] vector3Array = new Vector3[4];
        button.rect_transform.GetWorldCorners(vector3Array);
        float num3 = Mathf.Max(new float[4]
        {
          vector3Array[0].x,
          vector3Array[1].x,
          vector3Array[2].x,
          vector3Array[3].x
        });
        float num4 = Mathf.Min(new float[4]
        {
          vector3Array[0].x,
          vector3Array[1].x,
          vector3Array[2].x,
          vector3Array[3].x
        });
        if ((double) num3 < 0.0 || (double) num4 > (double) Screen.width)
        {
          ((Component) button).gameObject.SetActive(false);
          if ((double) num4 > (double) Screen.width)
            flag = true;
        }
        else
          ((Component) button).gameObject.SetActive(true);
      }
    }
  }
}
