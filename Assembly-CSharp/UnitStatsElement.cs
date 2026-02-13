// Decompiled with JetBrains decompiler
// Type: UnitStatsElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UnitStatsElement : UnitElement, IStatsElement, IRefreshElement
{
  [SerializeField]
  private Image _icon_sex;
  [SerializeField]
  private Image _icon_species;
  [SerializeField]
  private Sprite _default_creature_icon;
  private StatsIconContainer _stats_icons;

  public void setIconValue(
    string pName,
    float pMainVal,
    float? pMax = null,
    string pColor = "",
    bool pFloat = false,
    string pEnding = "",
    char pSeparator = '/')
  {
    this._stats_icons.setIconValue(pName, pMainVal, pMax, pColor, pFloat, pEnding, pSeparator);
  }

  protected override void Awake()
  {
    this._stats_icons = ((Component) this).gameObject.AddOrGetComponent<StatsIconContainer>();
    base.Awake();
  }

  protected override IEnumerator showContent()
  {
    // ISSUE: reference to a compiler-generated field
    int num1 = this.\u003C\u003E1__state;
    UnitStatsElement unitStatsElement = this;
    if (num1 != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (unitStatsElement.actor == null || !unitStatsElement.actor.isAlive())
      return false;
    unitStatsElement.actor.updateStats();
    if (unitStatsElement.actor.asset.inspect_stats)
    {
      unitStatsElement.showAttribute("i_diplomacy", (int) unitStatsElement.actor.stats["diplomacy"]);
      unitStatsElement.showAttribute("i_stewardship", (int) unitStatsElement.actor.stats["stewardship"]);
      unitStatsElement.showAttribute("i_intelligence", (int) unitStatsElement.actor.stats["intelligence"]);
      unitStatsElement.showAttribute("i_warfare", (int) unitStatsElement.actor.stats["warfare"]);
      int stat = (int) unitStatsElement.actor.stats["damage"];
      int pMainVal = (int) ((double) stat * (double) unitStatsElement.actor.stats["damage_range"]);
      // ISSUE: explicit non-virtual call
      __nonvirtual (unitStatsElement.setIconValue("i_damage", (float) pMainVal, new float?((float) stat), "", false, "", '-'));
      // ISSUE: explicit non-virtual call
      __nonvirtual (unitStatsElement.setIconValue("i_armor", unitStatsElement.actor.stats["armor"], new float?(), "", false, "%", '/'));
      // ISSUE: explicit non-virtual call
      __nonvirtual (unitStatsElement.setIconValue("i_speed", unitStatsElement.actor.stats["speed"], new float?(), "", false, "", '/'));
      // ISSUE: explicit non-virtual call
      __nonvirtual (unitStatsElement.setIconValue("i_critical_chance", unitStatsElement.actor.stats["critical_chance"] * 100f, new float?(), "", false, "%", '/'));
      // ISSUE: explicit non-virtual call
      __nonvirtual (unitStatsElement.setIconValue("i_attack_speed", unitStatsElement.actor.stats["attack_speed"], new float?(), "", true, "", '/'));
    }
    if (unitStatsElement.actor.asset.inspect_kills)
    {
      // ISSUE: explicit non-virtual call
      __nonvirtual (unitStatsElement.setIconValue("i_kills", (float) unitStatsElement.actor.data.kills, new float?(), "", false, "", '/'));
    }
    if (unitStatsElement.actor.asset.inspect_children)
    {
      // ISSUE: explicit non-virtual call
      __nonvirtual (unitStatsElement.setIconValue("i_births", (float) unitStatsElement.actor.data.births, new float?(), "", false, "", '/'));
      // ISSUE: explicit non-virtual call
      __nonvirtual (unitStatsElement.setIconValue("i_children", (float) unitStatsElement.actor.current_children_count, new float?((float) unitStatsElement.actor.getMaxOffspring()), "", false, "", '/'));
    }
    // ISSUE: explicit non-virtual call
    __nonvirtual (unitStatsElement.setIconValue("i_renown", (float) unitStatsElement.actor.data.renown, new float?(), "", false, "", '/'));
    if (unitStatsElement.actor.asset.inspect_experience)
    {
      int num2 = unitStatsElement.actor.hasTrait("immortal") ? 1 : 0;
      StatsIcon iconViaId = unitStatsElement._stats_icons.getIconViaId("i_lifespan");
      if (num2 != 0)
      {
        unitStatsElement._stats_icons.setText("i_lifespan", "???", "#F3961F");
      }
      else
      {
        iconViaId.enable_animation = true;
        // ISSUE: explicit non-virtual call
        __nonvirtual (unitStatsElement.setIconValue("i_lifespan", unitStatsElement.actor.stats["lifespan"], new float?(), "", false, "", '/'));
      }
      // ISSUE: explicit non-virtual call
      __nonvirtual (unitStatsElement.setIconValue("i_level", (float) unitStatsElement.actor.data.level, new float?(), "", false, "", '/'));
      // ISSUE: explicit non-virtual call
      __nonvirtual (unitStatsElement.setIconValue("i_experience", (float) unitStatsElement.actor.data.experience, new float?((float) unitStatsElement.actor.getExpToLevelup()), "", false, "", '/'));
    }
    Sprite sprite = unitStatsElement.actor.asset.getSpriteIcon();
    if (Object.op_Equality((Object) sprite, (Object) null) || unitStatsElement.actor.asset.is_boat)
      sprite = unitStatsElement._default_creature_icon;
    unitStatsElement._icon_species.sprite = sprite;
    unitStatsElement._icon_sex.sprite = !unitStatsElement.actor.asset.inspect_sex ? sprite : (!unitStatsElement.actor.isSexMale() ? SpriteTextureLoader.getSprite("ui/icons/IconFemale") : SpriteTextureLoader.getSprite("ui/icons/IconMale"));
    return false;
  }

  private void showAttribute(string pName, int pValue)
  {
    StatsIcon pIcon;
    this._stats_icons.TryGetValue(pName, out pIcon);
    if (Object.op_Equality((Object) pIcon, (Object) null))
      return;
    ((Component) pIcon).gameObject.SetActive(true);
    if (pValue < 4)
      pIcon.setValue((float) pValue, pColor: "#FB2C21");
    else if (pValue >= 20)
      pIcon.setValue((float) pValue, pColor: "#43FF43");
    else
      pIcon.setValue((float) pValue);
  }

  GameObject IStatsElement.get_gameObject() => ((Component) this).gameObject;
}
