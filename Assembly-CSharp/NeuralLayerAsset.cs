// Decompiled with JetBrains decompiler
// Type: NeuralLayerAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;

#nullable disable
[Serializable]
public class NeuralLayerAsset : Asset, IDescriptionAsset, ILocalizedAsset
{
  public NeuroLayer layer;
  public string color_hex;
  public bool critical;
  public float chance_to_go_to_next_layer;

  public string getLocaleID() => this.id;

  public string getDescriptionID() => this.id + "_priority";

  [JsonIgnore]
  public string debug_string
  {
    get
    {
      return $"{this.getLocaleID().Localize()} - {this.getDescriptionID().Localize()} - ({this.layer})";
    }
  }
}
