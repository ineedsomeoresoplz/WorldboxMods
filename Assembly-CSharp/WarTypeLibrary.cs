// Decompiled with JetBrains decompiler
// Type: WarTypeLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class WarTypeLibrary : AssetLibrary<WarTypeAsset>
{
  public static WarTypeAsset normal;
  public static WarTypeAsset spite;
  public static WarTypeAsset inspire;
  public static WarTypeAsset rebellion;
  public static WarTypeAsset whisper_of_war;
  public static WarTypeAsset clash;

  public override void init()
  {
    base.init();
    WarTypeAsset pAsset1 = new WarTypeAsset();
    pAsset1.id = "normal";
    pAsset1.name_template = "war_conquest";
    pAsset1.localized_type = "war_type_conquest";
    pAsset1.localized_war_name = "war_name_conquest";
    pAsset1.path_icon = "wars/war_conquest";
    pAsset1.kingdom_for_name_attacker = true;
    pAsset1.alliance_join = true;
    pAsset1.can_end_with_plot = true;
    WarTypeLibrary.normal = this.add(pAsset1);
    WarTypeAsset pAsset2 = new WarTypeAsset();
    pAsset2.id = "spite";
    pAsset2.name_template = "war_spite";
    pAsset2.localized_type = "war_type_spite";
    pAsset2.localized_war_name = "war_name_spite";
    pAsset2.path_icon = "wars/war_spite";
    pAsset2.kingdom_for_name_attacker = true;
    pAsset2.forced_war = true;
    pAsset2.total_war = true;
    pAsset2.alliance_join = false;
    WarTypeLibrary.spite = this.add(pAsset2);
    WarTypeAsset pAsset3 = new WarTypeAsset();
    pAsset3.id = "inspire";
    pAsset3.name_template = "war_inspire";
    pAsset3.localized_type = "war_type_inspire";
    pAsset3.localized_war_name = "war_name_inspire";
    pAsset3.path_icon = "wars/war_rebellion";
    pAsset3.kingdom_for_name_attacker = false;
    pAsset3.alliance_join = false;
    pAsset3.rebellion = true;
    pAsset3.can_end_with_plot = true;
    WarTypeLibrary.inspire = this.add(pAsset3);
    WarTypeAsset pAsset4 = new WarTypeAsset();
    pAsset4.id = "rebellion";
    pAsset4.name_template = "war_rebellion";
    pAsset4.localized_type = "war_type_rebellion";
    pAsset4.localized_war_name = "war_name_rebellion";
    pAsset4.path_icon = "wars/war_rebellion";
    pAsset4.kingdom_for_name_attacker = false;
    pAsset4.alliance_join = false;
    pAsset4.rebellion = true;
    pAsset4.can_end_with_plot = true;
    WarTypeLibrary.rebellion = this.add(pAsset4);
    WarTypeAsset pAsset5 = new WarTypeAsset();
    pAsset5.id = "whisper_of_war";
    pAsset5.name_template = "war_whisper";
    pAsset5.localized_type = "war_type_whisper";
    pAsset5.localized_war_name = "war_name_whisper";
    pAsset5.path_icon = "wars/war_whisper";
    pAsset5.kingdom_for_name_attacker = true;
    pAsset5.alliance_join = true;
    WarTypeLibrary.whisper_of_war = this.add(pAsset5);
  }

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (WarTypeAsset pAsset in this.list)
    {
      foreach (string localeId in pAsset.getLocaleIDs())
        this.checkLocale((Asset) pAsset, localeId);
    }
  }
}
