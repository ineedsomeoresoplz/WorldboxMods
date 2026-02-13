// Decompiled with JetBrains decompiler
// Type: Sfx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Sfx : MonoBehaviour
{
  private static Dictionary<string, List<SoundController>> dict;
  private static List<SoundController> listAll;

  [Obsolete("Sound system moved to MusicBox")]
  public static void timeout(string pName) => Debug.LogWarning((object) "Don't call SFX.timeout");

  [Obsolete("Check out MusicBox.playSound instead")]
  public static void play(string pName, bool pRestart = true, float pX = -1f, float pY = -1f)
  {
    Debug.LogWarning((object) "Don't call SFX.play");
  }

  [Obsolete("Sound system moved to MusicBox")]
  public static void fadeOut(string pName)
  {
    int num = PlayerConfig.dict["sound"].boolVal ? 1 : 0;
  }

  private void Start()
  {
  }

  private void Update()
  {
  }
}
