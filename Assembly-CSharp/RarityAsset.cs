// Decompiled with JetBrains decompiler
// Type: RarityAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class RarityAsset : Asset, ILocalizedAsset
{
  public string color_hex;
  public string material_path;
  public string rarity_trait_string;
  [NonSerialized]
  public ContainerItemColor color_container;

  public string getLocaleID() => this.rarity_trait_string;
}
