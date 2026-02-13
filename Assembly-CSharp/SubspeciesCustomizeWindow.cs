// Decompiled with JetBrains decompiler
// Type: SubspeciesCustomizeWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class SubspeciesCustomizeWindow : 
  GenericCustomizeWindow<Subspecies, SubspeciesData, SubspeciesBanner>
{
  protected override MetaType meta_type => MetaType.Subspecies;

  protected override Subspecies meta_object => SelectedMetas.selected_subspecies;

  protected override void onBannerChange()
  {
    this.image_banner_option_1.sprite = this.meta_object.getSpriteBackground();
    this.image_banner_option_2.sprite = this.meta_object.getSpriteIcon();
  }

  protected override void updateColorsBanner()
  {
  }
}
