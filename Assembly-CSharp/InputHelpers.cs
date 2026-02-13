// Decompiled with JetBrains decompiler
// Type: InputHelpers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class InputHelpers
{
  public static bool touchSupported => Input.touchSupported;

  public static int touchCount => Input.touchCount;

  public static bool mouseSupported
  {
    get
    {
      if (!Input.mousePresent)
        return false;
      return !Input.touchSupported || Input.touchCount == 0;
    }
  }

  public static bool GetMouseButtonDown(int pButton)
  {
    return pButton != -1 && Input.GetMouseButtonDown(pButton);
  }

  public static bool GetAnyMouseButtonDown()
  {
    return Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2);
  }

  public static int GetAnyMouseButtonDownIndex()
  {
    if (Input.GetMouseButtonDown(0))
      return 0;
    if (Input.GetMouseButtonDown(1))
      return 1;
    return Input.GetMouseButtonDown(2) ? 2 : -1;
  }

  public static bool GetMouseButton(int pButton) => pButton != -1 && Input.GetMouseButton(pButton);

  public static bool GetAnyMouseButton()
  {
    return Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2);
  }

  public static int GetAnyMouseButtonIndex()
  {
    if (Input.GetMouseButton(0))
      return 0;
    if (Input.GetMouseButton(1))
      return 1;
    return Input.GetMouseButton(2) ? 2 : -1;
  }

  public static bool GetMouseButtonUp(int pButton)
  {
    return pButton != -1 && Input.GetMouseButtonUp(pButton);
  }

  public static bool GetAnyMouseButtonUp()
  {
    return Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2);
  }

  public static int GetAnyMouseButtonUpIndex()
  {
    if (Input.GetMouseButtonUp(0))
      return 0;
    if (Input.GetMouseButtonUp(1))
      return 1;
    return Input.GetMouseButtonUp(2) ? 2 : -1;
  }
}
