// Decompiled with JetBrains decompiler
// Type: TraitRainLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class TraitRainLibrary : AssetLibrary<TraitRainAsset>
{
  public override void init()
  {
    base.init();
    TraitRainAsset pAsset1 = new TraitRainAsset();
    pAsset1.id = "traits_delta_rain_edit";
    this.add(pAsset1);
    this.t.get_list = (RainListGetter) (() => PlayerConfig.instance.data.trait_editor_delta);
    this.t.get_state = (RainStateGetter) (() => PlayerConfig.instance.data.trait_editor_delta_state);
    this.t.set_state = (RainStateSetter) (pState => PlayerConfig.instance.data.trait_editor_delta_state = pState);
    this.t.path_art = "ui/illustrations/art_trait_rain_delta";
    this.t.path_art_void = "ui/illustrations/art_trait_rain_delta_void";
    TraitRainAsset pAsset2 = new TraitRainAsset();
    pAsset2.id = "traits_gamma_rain_edit";
    this.add(pAsset2);
    this.t.get_list = (RainListGetter) (() => PlayerConfig.instance.data.trait_editor_gamma);
    this.t.get_state = (RainStateGetter) (() => PlayerConfig.instance.data.trait_editor_gamma_state);
    this.t.set_state = (RainStateSetter) (pState => PlayerConfig.instance.data.trait_editor_gamma_state = pState);
    this.t.path_art = "ui/illustrations/art_trait_rain_gamma";
    this.t.path_art_void = "ui/illustrations/art_trait_rain_gamma_void";
    TraitRainAsset pAsset3 = new TraitRainAsset();
    pAsset3.id = "traits_omega_rain_edit";
    this.add(pAsset3);
    this.t.get_list = (RainListGetter) (() => PlayerConfig.instance.data.trait_editor_omega);
    this.t.get_state = (RainStateGetter) (() => PlayerConfig.instance.data.trait_editor_omega_state);
    this.t.set_state = (RainStateSetter) (pState => PlayerConfig.instance.data.trait_editor_omega_state = pState);
    this.t.path_art = "ui/illustrations/art_trait_rain_omega";
    this.t.path_art_void = "ui/illustrations/art_trait_rain_omega_void";
  }
}
