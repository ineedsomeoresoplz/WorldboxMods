// Decompiled with JetBrains decompiler
// Type: MusicBoxDebug
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MusicBoxDebug
{
  internal List<DebugMusicBoxData> list = new List<DebugMusicBoxData>();

  public void add(string pPath, float pX, float pY, EventInstance pInstance)
  {
    pX += Randy.randomFloat(-0.5f, 0.5f);
    pY += Randy.randomFloat(-0.5f, 0.5f);
    this.list.Add(new DebugMusicBoxData()
    {
      timer = 3f,
      path = pPath,
      x = pX,
      y = pY,
      instance = pInstance
    });
  }

  public void update()
  {
    for (int index = this.list.Count - 1; index >= 0; --index)
    {
      DebugMusicBoxData debugMusicBoxData = this.list[index];
      debugMusicBoxData.timer -= Time.deltaTime;
      if ((double) debugMusicBoxData.timer <= 0.0)
        this.list.RemoveAt(index);
    }
  }

  public void clear() => this.list.Clear();
}
