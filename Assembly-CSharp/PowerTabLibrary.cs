// Decompiled with JetBrains decompiler
// Type: PowerTabLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class PowerTabLibrary : AssetLibrary<PowerTabAsset>
{
  public override void init()
  {
    this.addMainTabs();
    this.addSelectionTabs();
    PowerTabAsset pAsset1 = new PowerTabAsset();
    pAsset1.id = "selected_unit";
    pAsset1.meta_type = MetaType.Unit;
    pAsset1.window_id = "unit";
    pAsset1.on_main_tab_select = (PowerTabAction) (_ =>
    {
      if (!SelectedUnit.isSet())
        return;
      SelectedUnit.clear();
    });
    pAsset1.on_main_info_click = (PowerTabAction) (_ =>
    {
      ActionLibrary.openUnitWindow(SelectedUnit.unit);
      ScrollWindow.getCurrentWindow().tabs.showTab("MainTab");
    });
    pAsset1.on_update_check_active = (PowerTabActionCheck) (_ => SelectedUnit.isSet());
    pAsset1.get_localized_worldtip = new PowerTabWorldtipAction(this.getWorldTipTextMetaName);
    pAsset1.get_power_tab = (PowerTabGetter) (() => PowerTabController.instance.tab_selected_unit);
    this.add(pAsset1);
    PowerTabAsset pAsset2 = new PowerTabAsset();
    pAsset2.id = "multiple_units";
    pAsset2.on_update_check_active = (PowerTabActionCheck) (_ => SelectedUnit.isSet());
    pAsset2.on_main_tab_select = (PowerTabAction) (_ =>
    {
      if (!SelectedUnit.isSet())
        return;
      SelectedUnit.clear();
    });
    pAsset2.on_main_info_click = (PowerTabAction) (_ =>
    {
      ActionLibrary.openUnitWindow(SelectedUnit.unit);
      ScrollWindow.getCurrentWindow().tabs.showTab("MainTab");
    });
    pAsset2.get_localized_worldtip = new PowerTabWorldtipAction(this.getWorldTipTextAmount);
    pAsset2.get_power_tab = (PowerTabGetter) (() => PowerTabController.instance.tab_multiple_units);
    this.add(pAsset2);
    PowerTabAsset pAsset3 = new PowerTabAsset();
    pAsset3.id = "selected_building";
    this.add(pAsset3);
  }

  private void addMainTabs()
  {
    PowerTabAsset pAsset1 = new PowerTabAsset();
    pAsset1.id = "main";
    pAsset1.tab_type_main = true;
    this.add(pAsset1);
    PowerTabAsset pAsset2 = new PowerTabAsset();
    pAsset2.id = "creation";
    pAsset2.locale_key = "tab_world_creation";
    pAsset2.icon_path = "ui/Icons/power_tabs/icon_tab_drawings";
    pAsset2.tab_type_main = true;
    this.add(pAsset2);
    PowerTabAsset pAsset3 = new PowerTabAsset();
    pAsset3.id = "noosphere";
    pAsset3.locale_key = "tab_noosphere";
    pAsset3.icon_path = "ui/Icons/power_tabs/icon_tab_noosphere";
    pAsset3.tab_type_main = true;
    this.add(pAsset3);
    PowerTabAsset pAsset4 = new PowerTabAsset();
    pAsset4.id = "units";
    pAsset4.locale_key = "tab_world_creatures";
    pAsset4.icon_path = "ui/Icons/power_tabs/icon_tab_creatures";
    pAsset4.tab_type_main = true;
    this.add(pAsset4);
    PowerTabAsset pAsset5 = new PowerTabAsset();
    pAsset5.id = "nature";
    pAsset5.locale_key = "tab_nature";
    pAsset5.icon_path = "ui/Icons/power_tabs/icon_tab_nature";
    pAsset5.tab_type_main = true;
    this.add(pAsset5);
    PowerTabAsset pAsset6 = new PowerTabAsset();
    pAsset6.id = "destruction";
    pAsset6.locale_key = "tab_explosions";
    pAsset6.icon_path = "ui/Icons/power_tabs/icon_tab_bombs";
    pAsset6.tab_type_main = true;
    this.add(pAsset6);
    PowerTabAsset pAsset7 = new PowerTabAsset();
    pAsset7.id = "other";
    pAsset7.locale_key = "tab_other";
    pAsset7.icon_path = "ui/Icons/power_tabs/icon_tab_other";
    pAsset7.tab_type_main = true;
    this.add(pAsset7);
  }

  private void addSelectionTabs()
  {
    PowerTabAsset pAsset1 = new PowerTabAsset();
    pAsset1.id = "selected_army";
    pAsset1.meta_type = MetaType.Army;
    pAsset1.window_id = "army";
    pAsset1.get_power_tab = (PowerTabGetter) (() => PowerTabController.instance.tab_selected_army);
    pAsset1.on_update_check_active = new PowerTabActionCheck(this.defaultOnUpdateCheckActive);
    pAsset1.on_main_tab_select = new PowerTabAction(this.defaultMainTabSelect);
    pAsset1.on_main_info_click = new PowerTabAction(this.defaultMainInfoClick);
    pAsset1.get_localized_worldtip = new PowerTabWorldtipAction(this.getWorldTipTextMetaName);
    this.add(pAsset1);
    PowerTabAsset pAsset2 = new PowerTabAsset();
    pAsset2.id = "selected_family";
    pAsset2.meta_type = MetaType.Family;
    pAsset2.window_id = "family";
    pAsset2.get_power_tab = (PowerTabGetter) (() => PowerTabController.instance.tab_selected_family);
    pAsset2.on_update_check_active = new PowerTabActionCheck(this.defaultOnUpdateCheckActive);
    pAsset2.on_main_tab_select = new PowerTabAction(this.defaultMainTabSelect);
    pAsset2.on_main_info_click = new PowerTabAction(this.defaultMainInfoClick);
    pAsset2.get_localized_worldtip = new PowerTabWorldtipAction(this.getWorldTipTextMetaName);
    this.add(pAsset2);
    PowerTabAsset pAsset3 = new PowerTabAsset();
    pAsset3.id = "selected_subspecies";
    pAsset3.meta_type = MetaType.Subspecies;
    pAsset3.window_id = "subspecies";
    pAsset3.get_power_tab = (PowerTabGetter) (() => PowerTabController.instance.tab_selected_subspecies);
    pAsset3.on_update_check_active = new PowerTabActionCheck(this.defaultOnUpdateCheckActive);
    pAsset3.on_main_tab_select = new PowerTabAction(this.defaultMainTabSelect);
    pAsset3.on_main_info_click = new PowerTabAction(this.defaultMainInfoClick);
    pAsset3.get_localized_worldtip = new PowerTabWorldtipAction(this.getWorldTipTextMetaName);
    this.add(pAsset3);
    PowerTabAsset pAsset4 = new PowerTabAsset();
    pAsset4.id = "selected_language";
    pAsset4.meta_type = MetaType.Language;
    pAsset4.window_id = "language";
    pAsset4.get_power_tab = (PowerTabGetter) (() => PowerTabController.instance.tab_selected_language);
    pAsset4.on_update_check_active = new PowerTabActionCheck(this.defaultOnUpdateCheckActive);
    pAsset4.on_main_tab_select = new PowerTabAction(this.defaultMainTabSelect);
    pAsset4.on_main_info_click = new PowerTabAction(this.defaultMainInfoClick);
    pAsset4.get_localized_worldtip = new PowerTabWorldtipAction(this.getWorldTipTextMetaName);
    this.add(pAsset4);
    PowerTabAsset pAsset5 = new PowerTabAsset();
    pAsset5.id = "selected_culture";
    pAsset5.meta_type = MetaType.Culture;
    pAsset5.window_id = "culture";
    pAsset5.get_power_tab = (PowerTabGetter) (() => PowerTabController.instance.tab_selected_culture);
    pAsset5.on_update_check_active = new PowerTabActionCheck(this.defaultOnUpdateCheckActive);
    pAsset5.on_main_tab_select = new PowerTabAction(this.defaultMainTabSelect);
    pAsset5.on_main_info_click = new PowerTabAction(this.defaultMainInfoClick);
    pAsset5.get_localized_worldtip = new PowerTabWorldtipAction(this.getWorldTipTextMetaName);
    this.add(pAsset5);
    PowerTabAsset pAsset6 = new PowerTabAsset();
    pAsset6.id = "selected_religion";
    pAsset6.meta_type = MetaType.Religion;
    pAsset6.window_id = "religion";
    pAsset6.get_power_tab = (PowerTabGetter) (() => PowerTabController.instance.tab_selected_religion);
    pAsset6.on_update_check_active = new PowerTabActionCheck(this.defaultOnUpdateCheckActive);
    pAsset6.on_main_tab_select = new PowerTabAction(this.defaultMainTabSelect);
    pAsset6.on_main_info_click = new PowerTabAction(this.defaultMainInfoClick);
    pAsset6.get_localized_worldtip = new PowerTabWorldtipAction(this.getWorldTipTextMetaName);
    this.add(pAsset6);
    PowerTabAsset pAsset7 = new PowerTabAsset();
    pAsset7.id = "selected_clan";
    pAsset7.meta_type = MetaType.Clan;
    pAsset7.window_id = "clan";
    pAsset7.get_power_tab = (PowerTabGetter) (() => PowerTabController.instance.tab_selected_clan);
    pAsset7.on_update_check_active = new PowerTabActionCheck(this.defaultOnUpdateCheckActive);
    pAsset7.on_main_tab_select = new PowerTabAction(this.defaultMainTabSelect);
    pAsset7.on_main_info_click = new PowerTabAction(this.defaultMainInfoClick);
    pAsset7.get_localized_worldtip = new PowerTabWorldtipAction(this.getWorldTipTextMetaName);
    this.add(pAsset7);
    PowerTabAsset pAsset8 = new PowerTabAsset();
    pAsset8.id = "selected_city";
    pAsset8.meta_type = MetaType.City;
    pAsset8.window_id = "city";
    pAsset8.get_power_tab = (PowerTabGetter) (() => PowerTabController.instance.tab_selected_city);
    pAsset8.on_update_check_active = new PowerTabActionCheck(this.defaultOnUpdateCheckActive);
    pAsset8.on_main_tab_select = new PowerTabAction(this.defaultMainTabSelect);
    pAsset8.on_main_info_click = new PowerTabAction(this.defaultMainInfoClick);
    pAsset8.get_localized_worldtip = new PowerTabWorldtipAction(this.getWorldTipTextMetaName);
    this.add(pAsset8);
    PowerTabAsset pAsset9 = new PowerTabAsset();
    pAsset9.id = "selected_kingdom";
    pAsset9.meta_type = MetaType.Kingdom;
    pAsset9.window_id = "kingdom";
    pAsset9.get_power_tab = (PowerTabGetter) (() => PowerTabController.instance.tab_selected_kingdom);
    pAsset9.on_update_check_active = new PowerTabActionCheck(this.defaultOnUpdateCheckActive);
    pAsset9.on_main_tab_select = new PowerTabAction(this.defaultMainTabSelect);
    pAsset9.on_main_info_click = new PowerTabAction(this.defaultMainInfoClick);
    pAsset9.get_localized_worldtip = new PowerTabWorldtipAction(this.getWorldTipTextMetaName);
    this.add(pAsset9);
    PowerTabAsset pAsset10 = new PowerTabAsset();
    pAsset10.id = "selected_alliance";
    pAsset10.meta_type = MetaType.Alliance;
    pAsset10.window_id = "alliance";
    pAsset10.get_power_tab = (PowerTabGetter) (() => PowerTabController.instance.tab_selected_alliance);
    pAsset10.on_update_check_active = new PowerTabActionCheck(this.defaultOnUpdateCheckActive);
    pAsset10.on_main_tab_select = new PowerTabAction(this.defaultMainTabSelect);
    pAsset10.on_main_info_click = new PowerTabAction(this.defaultMainInfoClick);
    pAsset10.get_localized_worldtip = new PowerTabWorldtipAction(this.getWorldTipTextMetaName);
    this.add(pAsset10);
  }

  private void defaultMainTabSelect(PowerTabAsset pAsset)
  {
    SelectedObjects.unselectNanoObject();
    pAsset.meta_type.getAsset().window_action_clear();
  }

  private bool defaultOnUpdateCheckActive(PowerTabAsset pAsset)
  {
    return SelectedObjects.isNanoObjectSet();
  }

  private void defaultMainInfoClick(PowerTabAsset pAsset)
  {
    ScrollWindow.showWindow(pAsset.window_id);
    ScrollWindow.getCurrentWindow().tabs.showTab("MainTab");
  }

  private string getWorldTipTextMetaName(PowerTabAsset pAsset)
  {
    NanoObject selectedNanoObject = SelectedObjects.getSelectedNanoObject();
    string text = LocalizedTextManager.getText("show_tab_" + pAsset.id);
    string colorText = selectedNanoObject.getColor().color_text;
    string newValue = selectedNanoObject.name.ColorHex(colorText);
    return text.Replace("$name$", newValue);
  }

  private string getWorldTipTextAmount(PowerTabAsset pAsset)
  {
    int num = SelectedUnit.countSelected();
    return LocalizedTextManager.getText("show_tab_" + pAsset.id).Replace("$amount$", num.ToString());
  }

  public override void editorDiagnosticLocales()
  {
    foreach (PowerTabAsset pAsset in this.list)
    {
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
      this.checkLocale((Asset) pAsset, pAsset.getDescriptionID());
    }
    base.editorDiagnosticLocales();
  }
}
