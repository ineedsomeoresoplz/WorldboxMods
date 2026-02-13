// Decompiled with JetBrains decompiler
// Type: AotTypeEnforcer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json.Utilities;
using UnityEngine;

#nullable disable
public class AotTypeEnforcer : MonoBehaviour
{
  public void Awake()
  {
    AotHelper.EnsureType<CustomDataContainer<int>>();
    AotHelper.EnsureType<CustomDataContainer<float>>();
    AotHelper.EnsureType<CustomDataContainer<bool>>();
    AotHelper.EnsureType<CustomDataContainer<string>>();
    AotHelper.EnsureList<int>();
    AotHelper.EnsureList<float>();
    AotHelper.EnsureList<bool>();
    AotHelper.EnsureList<string>();
  }
}
