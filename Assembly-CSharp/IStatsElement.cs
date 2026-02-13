// Decompiled with JetBrains decompiler
// Type: IStatsElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public interface IStatsElement : IRefreshElement
{
  void setIconValue(
    string pName,
    float pMainVal,
    float? pMax = null,
    string pColor = "",
    bool pFloat = false,
    string pEnding = "",
    char pSeparator = '/');

  GameObject gameObject { get; }
}
