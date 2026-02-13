// Decompiled with JetBrains decompiler
// Type: LanguageCustomizeWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class LanguageCustomizeWindow : GenericCustomizeWindow<Language, LanguageData, LanguageBanner>
{
  protected override MetaType meta_type => MetaType.Language;

  protected override Language meta_object => SelectedMetas.selected_language;

  protected override void onBannerChange()
  {
    this.image_banner_option_1.sprite = this.meta_object.getBackgroundSprite();
    this.image_banner_option_2.sprite = this.meta_object.getIconSprite();
  }
}
