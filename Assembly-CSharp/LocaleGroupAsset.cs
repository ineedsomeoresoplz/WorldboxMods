// Decompiled with JetBrains decompiler
// Type: LocaleGroupAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class LocaleGroupAsset : Asset
{
  public string[] libraries;
  public List<string> contains = new List<string>();
  public List<string> starts_with_priority = new List<string>();
  public List<string> starts_with = new List<string>();
  public List<string> matches = new List<string>();
  public LocaleGroupChecker checker;
  public Dictionary<string, string> locales = new Dictionary<string, string>();
}
