// Decompiled with JetBrains decompiler
// Type: KingdomCustomizeWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class KingdomCustomizeWindow : GenericCustomizeWindow<Kingdom, KingdomData, KingdomBanner>
{
  protected override MetaType meta_type => MetaType.Kingdom;

  protected override Kingdom meta_object => SelectedMetas.selected_kingdom;

  protected override void onBannerChange()
  {
    this.meta_object.getActorAsset();
    this.image_banner_option_1.sprite = this.meta_object.getElementBackground();
    this.image_banner_option_2.sprite = this.meta_object.getElementIcon();
  }
}
