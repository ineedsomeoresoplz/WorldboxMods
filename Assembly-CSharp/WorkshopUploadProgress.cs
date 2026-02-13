// Decompiled with JetBrains decompiler
// Type: WorkshopUploadProgress
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
internal class WorkshopUploadProgress : IProgress<float>
{
  internal float lastvalue;

  public void Report(float value)
  {
    if ((double) this.lastvalue >= (double) value)
      return;
    this.lastvalue = value;
    WorkshopMaps.uploadProgress = this.lastvalue;
    Debug.Log((object) value);
  }
}
