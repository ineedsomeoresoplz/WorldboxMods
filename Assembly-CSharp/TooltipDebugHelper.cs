// Decompiled with JetBrains decompiler
// Type: TooltipDebugHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class TooltipDebugHelper
{
  private static GameObject _debug_canvas;

  public static void checkCreate()
  {
    if (!DebugConfig.isOn(DebugOption.DebugTooltipUI))
      return;
    MapBox.on_world_loaded += new Action(TooltipDebugHelper.loadButtons);
    HotkeyLibrary.cancel.just_pressed_action += new HotkeyAction(TooltipDebugHelper.killButtons);
  }

  public static void killButtons(HotkeyAsset pAsset)
  {
    Object.Destroy((Object) TooltipDebugHelper._debug_canvas);
    TooltipDebugHelper._debug_canvas = (GameObject) null;
  }

  public static void loadButtons()
  {
    // ISSUE: unable to decompile the method.
  }
}
