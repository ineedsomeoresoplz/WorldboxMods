// Decompiled with JetBrains decompiler
// Type: ChromosomeTypeAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class ChromosomeTypeAsset : Asset, IDescriptionAsset, ILocalizedAsset
{
  public int amount_loci;
  public int amount_loci_min_amplifier;
  public int amount_loci_max_amplifier;
  public int amount_loci_min_empty;
  public int amount_loci_max_empty;
  public string name;
  public string description;

  public string getLocaleID() => this.id;

  public string getDescriptionID() => this.id + "_description";
}
