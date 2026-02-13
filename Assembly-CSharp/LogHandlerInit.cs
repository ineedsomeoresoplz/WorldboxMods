// Decompiled with JetBrains decompiler
// Type: LogHandlerInit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class LogHandlerInit : MonoBehaviour
{
  private void Awake()
  {
    try
    {
      LogHandler.init();
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex);
      throw;
    }
  }
}
