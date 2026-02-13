// Decompiled with JetBrains decompiler
// Type: WorldLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using RSG;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
internal class WorldLoader : MonoBehaviour
{
  public static WorldLoader instance;
  public static Dictionary<string, Map> mapCache = new Dictionary<string, Map>();
  public static Dictionary<string, Promise<Dictionary<string, Map>>> listCache = new Dictionary<string, Promise<Dictionary<string, Map>>>();
}
