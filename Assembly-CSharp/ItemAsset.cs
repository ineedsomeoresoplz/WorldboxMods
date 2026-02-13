// Decompiled with JetBrains decompiler
// Type: ItemAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

#nullable disable
[Serializable]
public class ItemAsset : 
  BaseAugmentationAsset,
  IDescriptionAsset,
  ILocalizedAsset,
  IMultiLocalesAsset
{
  public string path_gameplay_sprite;
  [NonSerialized]
  public Sprite[] gameplay_sprites;
  public string material;
  public bool colored;
  public int minimum_city_storage_resource_1;
  public int cost_gold;
  public int cost_coins_resources;
  public int cost_resource_1;
  [DefaultValue("none")]
  public string cost_resource_id_1 = "none";
  public int cost_resource_2;
  [DefaultValue("none")]
  public string cost_resource_id_2 = "none";
  [DefaultValue(1)]
  public int rarity = 1;
  public int equipment_value;
  public bool metallic;
  [DefaultValue(1)]
  public int mod_rank = 1;
  public string mod_type;
  [DefaultValue(true)]
  public bool mod_can_be_given = true;
  public string translation_key;
  public bool animated;
  public string pool;
  [DefaultValue("")]
  public string path_slash_animation = string.Empty;
  public string projectile;
  [DefaultValue(Rarity.R0_Normal)]
  public Rarity quality;
  [DefaultValue(WeaponType.Melee)]
  public WeaponType attack_type;
  public EquipmentType equipment_type;
  public string equipment_subtype;
  public string name_class;
  public List<string> name_templates;
  public string[] item_modifier_ids;
  [NonSerialized]
  public ItemModAsset[] item_modifiers;
  public bool is_pool_weapon;
  public int pool_rate;
  [DefaultValue(100)]
  public int durability = 100;
  [DefaultValue(1)]
  public int rigidity_rating = 1;

  protected override HashSet<string> progress_elements => this._progress_data?.unlocked_equipment;

  [JsonIgnore]
  public int get_total_cost => this.cost_gold + this.cost_coins_resources;

  public override BaseCategoryAsset getGroup()
  {
    return (BaseCategoryAsset) AssetManager.item_groups.get(this.group_id);
  }

  public string getLocaleRarity(Rarity pRarity) => this.name_class + Item.getQualityString(pRarity);

  public override string getLocaleID()
  {
    return !this.has_locales ? (string) null : this.translation_key ?? "item_" + (this.equipment_subtype ?? this.id);
  }

  public string getDescriptionID()
  {
    return !this.has_locales || this.isMod() ? (string) null : this.id + "_description";
  }

  public string getMaterialID()
  {
    return !this.has_locales || this.isMod() ? (string) null : "item_mat_" + this.material;
  }

  public IEnumerable<string> getLocaleIDs()
  {
    ItemAsset itemAsset = this;
    if (itemAsset.has_locales)
    {
      yield return itemAsset.getLocaleID();
      if (!itemAsset.isMod())
      {
        // ISSUE: explicit non-virtual call
        yield return __nonvirtual (itemAsset.getDescriptionID());
        if (itemAsset.material != "basic")
          yield return itemAsset.getMaterialID();
        foreach (Rarity pRarity in Enum.GetValues(typeof (Rarity)))
          yield return itemAsset.getLocaleRarity(pRarity);
      }
    }
  }

  public string getTranslatedName()
  {
    string translatedName = LocalizedTextManager.getText(this.getLocaleID());
    if (!string.IsNullOrEmpty(this.material) && this.material != "basic")
    {
      string text = LocalizedTextManager.getText(this.getMaterialID());
      translatedName = $"{translatedName} ({text})";
    }
    return translatedName;
  }

  public string getTranslatedDescription() => LocalizedTextManager.getText(this.getDescriptionID());

  public void setCost(
    int pGoldCost,
    string pResourceID_1 = "none",
    int pCostResource_1 = 0,
    string pResourceID_2 = "none",
    int pCostResource_2 = 0)
  {
    this.cost_gold = pGoldCost;
    this.cost_resource_id_1 = pResourceID_1;
    this.cost_resource_1 = pCostResource_1;
    this.cost_resource_id_2 = pResourceID_2;
    this.cost_resource_2 = pCostResource_2;
  }

  public bool isMod() => this.mod_type != null;

  public string getRandomNameTemplate(Actor pActor = null)
  {
    foreach (string pID in this.name_templates.LoopRandom<string>())
    {
      NameGeneratorAsset nameGeneratorAsset = AssetManager.name_generator.get(pID);
      if (nameGeneratorAsset.check == null || nameGeneratorAsset.check(pActor))
        return pID;
    }
    return (string) null;
  }

  public override bool unlock(bool pSaveData = true)
  {
    if (!base.unlock(pSaveData))
      return false;
    EquipmentAsset equipmentAsset = this as EquipmentAsset;
    List<EquipmentAsset> weaponAssetsUnlocked = AssetManager.items.pot_weapon_assets_unlocked;
    if (this.is_pool_weapon && !weaponAssetsUnlocked.Contains(equipmentAsset))
      weaponAssetsUnlocked.Add(equipmentAsset);
    Dictionary<string, List<EquipmentAsset>> byGroupsUnlocked = AssetManager.items.pot_equipment_by_groups_unlocked;
    if (!this.is_pool_weapon)
    {
      if (!byGroupsUnlocked.ContainsKey(this.group_id))
        byGroupsUnlocked.Add(this.group_id, new List<EquipmentAsset>());
      List<EquipmentAsset> equipmentAssetList = byGroupsUnlocked[this.group_id];
      if (!equipmentAssetList.Contains(equipmentAsset))
        equipmentAssetList.Add(equipmentAsset);
    }
    return true;
  }

  protected override bool isDebugUnlockedAll() => DebugConfig.isOn(DebugOption.UnlockAllEquipment);
}
