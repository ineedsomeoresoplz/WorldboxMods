// Decompiled with JetBrains decompiler
// Type: OnomasticsEvolutionAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.ComponentModel;

#nullable disable
[Serializable]
public class OnomasticsEvolutionAsset : Asset
{
  public string from;
  public string to;
  public char[] not_surrounded_by;
  [DefaultValue(true)]
  public bool reverse = true;
  public OnomasticsReplacerDelegate replacer;
}
