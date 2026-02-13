// Decompiled with JetBrains decompiler
// Type: QueueItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class QueueItem
{
  public object timestamp;
  public string salt = RequestHelper.salt;
  public string version = Application.version;
  public string identifier = Application.identifier;
  public string language = LocalizedTextManager.instance.language;
  public string platform = Application.platform.ToString();
  public int progress;
}
