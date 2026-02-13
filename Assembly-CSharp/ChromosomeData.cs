// Decompiled with JetBrains decompiler
// Type: ChromosomeData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
[Serializable]
public class ChromosomeData
{
  public List<string> loci = new List<string>();
  public List<int> super_loci = new List<int>();
  public List<int> void_loci = new List<int>();
  [DefaultValue("chromosome_medium")]
  public string chromosome_type = "chromosome_medium";
}
