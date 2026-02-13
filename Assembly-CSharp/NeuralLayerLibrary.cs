// Decompiled with JetBrains decompiler
// Type: NeuralLayerLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class NeuralLayerLibrary : AssetLibrary<NeuralLayerAsset>
{
  private Dictionary<NeuroLayer, NeuralLayerAsset> _dict_enum_assets = new Dictionary<NeuroLayer, NeuralLayerAsset>();
  public NeuralLayerAsset[] layers_array;

  public override void init()
  {
    base.init();
    NeuralLayerAsset pAsset1 = new NeuralLayerAsset();
    pAsset1.id = "neuro_layer_0";
    pAsset1.layer = NeuroLayer.Layer_0_Minimal;
    pAsset1.color_hex = "#B0E0E6";
    this.add(pAsset1);
    NeuralLayerAsset pAsset2 = new NeuralLayerAsset();
    pAsset2.id = "neuro_layer_1";
    pAsset2.layer = NeuroLayer.Layer_1_Low;
    pAsset2.color_hex = "#87CEEB";
    pAsset2.chance_to_go_to_next_layer = 0.3f;
    this.add(pAsset2);
    NeuralLayerAsset pAsset3 = new NeuralLayerAsset();
    pAsset3.id = "neuro_layer_2";
    pAsset3.layer = NeuroLayer.Layer_2_Moderate;
    pAsset3.color_hex = "#FFD700";
    pAsset3.chance_to_go_to_next_layer = 0.2f;
    this.add(pAsset3);
    NeuralLayerAsset pAsset4 = new NeuralLayerAsset();
    pAsset4.id = "neuro_layer_3";
    pAsset4.layer = NeuroLayer.Layer_3_High;
    pAsset4.color_hex = "#FF4500";
    pAsset4.chance_to_go_to_next_layer = 0.1f;
    this.add(pAsset4);
    NeuralLayerAsset pAsset5 = new NeuralLayerAsset();
    pAsset5.id = "neuro_layer_4";
    pAsset5.layer = NeuroLayer.Layer_4_Critical;
    pAsset5.critical = true;
    pAsset5.color_hex = "#FF0000";
    this.add(pAsset5);
  }

  public override void linkAssets()
  {
    base.linkAssets();
    this.layers_array = new NeuralLayerAsset[this.list.Count];
    foreach (NeuralLayerAsset neuralLayerAsset in this.list)
    {
      this._dict_enum_assets.Add(neuralLayerAsset.layer, neuralLayerAsset);
      this.layers_array[(int) neuralLayerAsset.layer] = neuralLayerAsset;
    }
  }

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (NeuralLayerAsset pAsset in this.list)
    {
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
      this.checkLocale((Asset) pAsset, pAsset.getDescriptionID());
    }
  }

  public NeuralLayerAsset getWithID(NeuroLayer pLayerID) => this._dict_enum_assets[pLayerID];
}
