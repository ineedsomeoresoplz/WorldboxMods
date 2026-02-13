// Decompiled with JetBrains decompiler
// Type: CoroutineHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class CoroutineHelper
{
  public static WaitForSecondsRealtime wait_for_0_5_s => new WaitForSecondsRealtime(0.5f);

  public static WaitForSecondsRealtime wait_for_0_01_s => new WaitForSecondsRealtime(0.01f);

  public static WaitForSecondsRealtime wait_for_0_05_s => new WaitForSecondsRealtime(0.05f);

  public static WaitForSecondsRealtime wait_for_0_025_s => new WaitForSecondsRealtime(0.025f);

  public static YieldInstruction wait_for_end_of_frame
  {
    get => (YieldInstruction) new WaitForEndOfFrame();
  }

  public static YieldInstruction wait_for_next_frame => (YieldInstruction) null;
}
