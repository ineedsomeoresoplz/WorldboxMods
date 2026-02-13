// Decompiled with JetBrains decompiler
// Type: ItemLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[ObfuscateLiterals]
public class ItemLibrary : ItemAssetLibrary<EquipmentAsset>
{
  [NonSerialized]
  public List<EquipmentAsset> pot_weapon_assets_all = new List<EquipmentAsset>();
  [NonSerialized]
  public Dictionary<string, List<EquipmentAsset>> pot_equipment_by_groups_all = new Dictionary<string, List<EquipmentAsset>>();
  [NonSerialized]
  public List<EquipmentAsset> pot_weapon_assets_unlocked = new List<EquipmentAsset>();
  [NonSerialized]
  public Dictionary<string, List<EquipmentAsset>> pot_equipment_by_groups_unlocked = new Dictionary<string, List<EquipmentAsset>>();
  [NonSerialized]
  public Dictionary<string, List<EquipmentAsset>> equipment_by_subtypes = new Dictionary<string, List<EquipmentAsset>>();
  public static readonly string[] default_weapon_pool = new string[6]
  {
    "sword",
    "axe",
    "hammer",
    "spear",
    "bow",
    "stick"
  };
  public static EquipmentAsset base_attack;
  private const string TEMPLATE_EQUIPMENT = "$equipment";
  private const string TEMPLATE_ARMOR = "$armor";
  private const string TEMPLATE_BOOTS = "$boots";
  private const string TEMPLATE_HELMET = "$helmet";
  private const string TEMPLATE_ACCESSORY = "$accessory";
  private const string TEMPLATE_RING = "$ring";
  private const string TEMPLATE_AMULET = "$amulet";
  private const string TEMPLATE_WEAPON = "$weapon";
  private const string TEMPLATE_MELEE = "$melee";
  private const string TEMPLATE_RANGE = "$range";
  private const string TEMPLATE_BOW = "$bow";
  private const string TEMPLATE_SWORD = "$sword";
  private const string TEMPLATE_AXE = "$axe";
  private const string TEMPLATE_HAMMER = "$hammer";
  private const string TEMPLATE_SPEAR = "$spear";

  public override void init()
  {
    base.init();
    this.initTemplates();
    this.initNormalEquipment();
    this.initNormalWeapons();
    this.initWeaponsUnique();
    this.initBoats();
    this.initBaseAttacks();
  }

  public override void post_init()
  {
    foreach (EquipmentAsset equipmentAsset in this.list)
    {
      if (equipmentAsset.is_pool_weapon)
        equipmentAsset.path_gameplay_sprite = "items/weapons/w_" + equipmentAsset.id;
      if (string.IsNullOrEmpty(equipmentAsset.path_icon))
      {
        equipmentAsset.path_icon = "ui/Icons/items/icon_" + equipmentAsset.id;
        int num = 0;
        if (equipmentAsset.cost_resource_id_1 != "none")
        {
          ResourceAsset resourceAsset = AssetManager.resources.get(equipmentAsset.cost_resource_id_1);
          num += resourceAsset.money_cost;
        }
        if (equipmentAsset.cost_resource_id_2 != "none")
        {
          ResourceAsset resourceAsset = AssetManager.resources.get(equipmentAsset.cost_resource_id_2);
          num += resourceAsset.money_cost;
        }
        equipmentAsset.cost_coins_resources = num;
      }
    }
  }

  public override void linkAssets()
  {
    base.linkAssets();
    foreach (EquipmentAsset equipmentAsset in this.list)
    {
      if (equipmentAsset.item_modifier_ids != null)
      {
        equipmentAsset.item_modifiers = new ItemModAsset[equipmentAsset.item_modifier_ids.Length];
        for (int index = 0; index < equipmentAsset.item_modifier_ids.Length; ++index)
        {
          string itemModifierId = equipmentAsset.item_modifier_ids[index];
          ItemModAsset itemModAsset = AssetManager.items_modifiers.get(itemModifierId);
          if (itemModAsset == null)
            BaseAssetLibrary.logAssetError("ItemLibrary: Item Modifier Asset <e>not found</e>", itemModifierId);
          else
            equipmentAsset.item_modifiers[index] = itemModAsset;
        }
      }
    }
    this.fillSubtypesAndGroups();
    this.fillUnlockedPools();
    foreach (BaseAugmentationAsset augmentationAsset in this.list)
      augmentationAsset.linkSpells();
  }

  public override EquipmentAsset add(EquipmentAsset pAsset)
  {
    EquipmentAsset equipmentAsset = base.add(pAsset);
    if (equipmentAsset.base_stats == null)
      equipmentAsset.base_stats = new BaseStats();
    return equipmentAsset;
  }

  public string getEquipmentType(EquipmentType pType)
  {
    switch (pType)
    {
      case EquipmentType.Weapon:
        return "weapon";
      case EquipmentType.Helmet:
        return "helmet";
      case EquipmentType.Armor:
        return "armor";
      case EquipmentType.Boots:
        return "boots";
      case EquipmentType.Ring:
        return "ring";
      case EquipmentType.Amulet:
        return "amulet";
      default:
        return (string) null;
    }
  }

  private void fillSubtypesAndGroups()
  {
    foreach (EquipmentAsset equipmentAsset in this.list)
    {
      if (!this.equipment_by_subtypes.ContainsKey(equipmentAsset.equipment_subtype))
        this.equipment_by_subtypes.Add(equipmentAsset.equipment_subtype, new List<EquipmentAsset>());
      this.equipment_by_subtypes[equipmentAsset.equipment_subtype].Add(equipmentAsset);
      if (equipmentAsset.is_pool_weapon)
        this.pot_weapon_assets_all.Add(equipmentAsset);
      if (!equipmentAsset.is_pool_weapon)
      {
        string groupId = equipmentAsset.group_id;
        if (!this.pot_equipment_by_groups_all.ContainsKey(groupId))
          this.pot_equipment_by_groups_all.Add(groupId, new List<EquipmentAsset>());
        this.pot_equipment_by_groups_all[groupId].Add(equipmentAsset);
      }
    }
  }

  private void fillUnlockedPools()
  {
    foreach (string pID in GameProgress.instance.data.unlocked_equipment)
    {
      EquipmentAsset equipmentAsset = this.get(pID);
      if (equipmentAsset != null)
      {
        if (equipmentAsset.is_pool_weapon && !this.pot_weapon_assets_unlocked.Contains(equipmentAsset))
          this.pot_weapon_assets_unlocked.Add(equipmentAsset);
        if (!equipmentAsset.is_pool_weapon)
        {
          string groupId = equipmentAsset.group_id;
          if (!this.pot_equipment_by_groups_unlocked.ContainsKey(groupId))
            this.pot_equipment_by_groups_unlocked.Add(groupId, new List<EquipmentAsset>());
          List<EquipmentAsset> equipmentAssetList = this.pot_equipment_by_groups_unlocked[groupId];
          if (!equipmentAssetList.Contains(equipmentAsset))
            equipmentAssetList.Add(equipmentAsset);
        }
      }
    }
  }

  public string addToGameplayReport(string pWhat)
  {
    string str1 = $"{string.Empty}{pWhat}\n";
    foreach (EquipmentAsset equipmentAsset in this.list)
    {
      if (equipmentAsset.has_locales && !equipmentAsset.isTemplateAsset())
      {
        string translatedName = equipmentAsset.getTranslatedName();
        string translatedDescription = equipmentAsset.getTranslatedDescription();
        string str2 = "\n" + translatedName + "\n";
        if (!string.IsNullOrEmpty(translatedDescription))
          str2 = $"{str2}1: {translatedDescription}";
        str1 += str2;
      }
    }
    return str1 + "\n\n";
  }

  public void loadSprites()
  {
    foreach (EquipmentAsset equipmentAsset in this.list)
    {
      if (equipmentAsset.is_pool_weapon)
      {
        equipmentAsset.gameplay_sprites = SpriteTextureLoader.getSpriteList(equipmentAsset.path_gameplay_sprite);
        if (equipmentAsset.gameplay_sprites.Length == 0)
          Debug.LogError((object) ("Weapon Texture is Missing: " + equipmentAsset.path_gameplay_sprite));
      }
    }
  }

  private void initBaseAttacks()
  {
    ItemLibrary.base_attack = this.clone("base_attack", "$melee");
    this.t.has_locales = false;
    this.t.path_slash_animation = "effects/slashes/slash_base";
    this.t.show_in_meta_editor = false;
    this.t.show_in_knowledge_window = false;
    this.clone("hands", "$melee");
    this.t.has_locales = false;
    this.t.path_slash_animation = "effects/slashes/slash_punch";
    this.t.show_in_meta_editor = false;
    this.t.show_in_knowledge_window = false;
    this.clone("fire_hands", "hands");
    this.t.has_locales = false;
    this.t.item_modifier_ids = AssetLibrary<EquipmentAsset>.a<string>("flame");
    this.t.show_in_meta_editor = false;
    this.t.show_in_knowledge_window = false;
    this.clone("jaws", "$melee");
    this.t.has_locales = false;
    this.t.path_slash_animation = "effects/slashes/slash_jaws";
    this.t.show_in_meta_editor = false;
    this.t.show_in_knowledge_window = false;
    this.clone("claws", "$melee");
    this.t.has_locales = false;
    this.t.path_slash_animation = "effects/slashes/slash_claws";
    this.t.show_in_meta_editor = false;
    this.t.show_in_knowledge_window = false;
    this.clone("snowball", "$range");
    this.t.has_locales = false;
    this.t.path_slash_animation = "effects/slashes/slash_punch";
    this.t.base_stats["range"] = 6f;
    this.t.base_stats["critical_chance"] = 0.1f;
    this.t.base_stats["critical_damage_multiplier"] = 0.5f;
    this.t.projectile = "snowball";
    this.t.base_stats["projectiles"] = 1f;
    this.t.item_modifier_ids = AssetLibrary<EquipmentAsset>.a<string>("ice");
    this.t.show_in_meta_editor = false;
    this.t.show_in_knowledge_window = false;
    this.clone("bite", "$melee");
    this.t.has_locales = false;
    this.t.path_slash_animation = "effects/slashes/slash_jaws";
    this.t.attack_type = WeaponType.Melee;
    this.t.show_in_meta_editor = false;
    this.t.show_in_knowledge_window = false;
    this.clone("rocks", "$range");
    this.t.has_locales = false;
    this.t.projectile = "rock";
    this.t.path_slash_animation = "effects/slashes/slash_punch";
    this.t.base_stats["range"] = 15f;
    this.t.base_stats["critical_chance"] = 0.1f;
    this.t.base_stats["projectiles"] = 1f;
    this.t.base_stats["critical_damage_multiplier"] = 0.5f;
    this.t.show_in_meta_editor = false;
    this.t.show_in_knowledge_window = false;
  }

  private void initBoats()
  {
    this.clone("boat_cannonball", "$range");
    this.t.has_locales = false;
    this.t.path_slash_animation = "effects/slashes/slash_cannonball";
    this.t.base_stats["damage"] = 50f;
    this.t.base_stats["range"] = 14f;
    this.t.base_stats["targets"] = 4f;
    this.t.base_stats["area_of_effect"] = 3f;
    this.t.base_stats["critical_chance"] = 0.1f;
    this.t.base_stats["critical_damage_multiplier"] = 0.5f;
    this.t.projectile = "cannonball";
    this.t.base_stats["projectiles"] = 1f;
    this.t.show_in_meta_editor = false;
    this.t.show_in_knowledge_window = false;
    this.clone("boat_arrow", "$range");
    this.t.has_locales = false;
    this.t.path_slash_animation = "effects/slashes/slash_bow";
    this.t.base_stats["damage"] = 30f;
    this.t.base_stats["range"] = 9f;
    this.t.base_stats["targets"] = 4f;
    this.t.base_stats["area_of_effect"] = 0.0f;
    this.t.base_stats["accuracy"] = 4f;
    this.t.base_stats["critical_chance"] = 0.2f;
    this.t.base_stats["critical_damage_multiplier"] = 0.5f;
    this.t.base_stats["attack_speed"] = 0.5f;
    this.t.projectile = "arrow";
    this.t.base_stats["projectiles"] = 5f;
    this.t.show_in_meta_editor = false;
    this.t.show_in_knowledge_window = false;
    this.clone("boat_snowball", "$range");
    this.t.has_locales = false;
    this.t.path_slash_animation = "effects/slashes/slash_snowball";
    this.t.base_stats["damage"] = 50f;
    this.t.base_stats["range"] = 14f;
    this.t.base_stats["targets"] = 4f;
    this.t.base_stats["area_of_effect"] = 3f;
    this.t.base_stats["critical_chance"] = 0.1f;
    this.t.base_stats["critical_damage_multiplier"] = 0.5f;
    this.t.projectile = "snowball";
    this.t.base_stats["projectiles"] = 1f;
    this.t.show_in_meta_editor = false;
    this.t.show_in_knowledge_window = false;
    this.clone("boat_plasma_ball", "$range");
    this.t.has_locales = false;
    this.t.path_slash_animation = "effects/slashes/slash_plasma_ball";
    this.t.base_stats["damage"] = 65f;
    this.t.base_stats["range"] = 20f;
    this.t.base_stats["targets"] = 4f;
    this.t.base_stats["area_of_effect"] = 2f;
    this.t.base_stats["critical_chance"] = 0.2f;
    this.t.base_stats["critical_damage_multiplier"] = 0.5f;
    this.t.projectile = "plasma_ball";
    this.t.base_stats["projectiles"] = 1f;
    this.t.show_in_meta_editor = false;
    this.t.show_in_knowledge_window = false;
    this.clone("boat_necro_ball", "$range");
    this.t.has_locales = false;
    this.t.path_slash_animation = "effects/slashes/slash_necro_ball";
    this.t.base_stats["damage"] = 45f;
    this.t.base_stats["range"] = 12f;
    this.t.base_stats["targets"] = 4f;
    this.t.base_stats["area_of_effect"] = 3f;
    this.t.base_stats["critical_chance"] = 0.1f;
    this.t.base_stats["critical_damage_multiplier"] = 0.5f;
    this.t.projectile = "skull";
    this.t.base_stats["projectiles"] = 3f;
    this.t.show_in_meta_editor = false;
    this.t.show_in_knowledge_window = false;
    this.clone("boat_fireball", "$range");
    this.t.has_locales = false;
    this.t.path_slash_animation = "effects/slashes/slash_cannonball";
    this.t.base_stats["damage"] = 30f;
    this.t.base_stats["range"] = 12f;
    this.t.base_stats["targets"] = 4f;
    this.t.base_stats["area_of_effect"] = 2f;
    this.t.base_stats["critical_chance"] = 0.1f;
    this.t.base_stats["critical_damage_multiplier"] = 0.5f;
    this.t.projectile = "fireball";
    this.t.base_stats["projectiles"] = 1f;
    this.t.show_in_meta_editor = false;
    this.t.show_in_knowledge_window = false;
    this.clone("boat_freeze_ball", "$range");
    this.t.has_locales = false;
    this.t.path_slash_animation = "effects/slashes/slash_snowball";
    this.t.base_stats["damage"] = 30f;
    this.t.base_stats["range"] = 12f;
    this.t.base_stats["targets"] = 4f;
    this.t.base_stats["area_of_effect"] = 2f;
    this.t.base_stats["critical_chance"] = 0.1f;
    this.t.base_stats["critical_damage_multiplier"] = 0.5f;
    this.t.projectile = "freeze_orb";
    this.t.base_stats["projectiles"] = 3f;
    this.t.show_in_meta_editor = false;
    this.t.show_in_knowledge_window = false;
    this.clone("boat_acid_ball", "$range");
    this.t.has_locales = false;
    this.t.path_slash_animation = "effects/slashes/slash_acid_ball";
    this.t.base_stats["damage"] = 50f;
    this.t.base_stats["range"] = 14f;
    this.t.base_stats["targets"] = 4f;
    this.t.base_stats["area_of_effect"] = 4f;
    this.t.base_stats["critical_chance"] = 0.1f;
    this.t.base_stats["critical_damage_multiplier"] = 0.5f;
    this.t.projectile = "acid_ball";
    this.t.base_stats["projectiles"] = 5f;
    this.t.show_in_meta_editor = false;
    this.t.show_in_knowledge_window = false;
  }

  private void initNormalEquipment()
  {
    this.initArmors();
    this.initBoots();
    this.initHelmets();
    this.initRings();
    this.initAmulets();
  }

  private void initAmulets()
  {
    this.clone("amulet_bone", "$amulet");
    this.t.material = "bone";
    this.t.equipment_value = 5;
    this.t.setCost(0, "bones", 1, "gems", 1);
    this.t.rigidity_rating = 1;
    this.t.base_stats["critical_chance"] = 0.02f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("amulet_copper", "$amulet");
    this.t.material = "copper";
    this.t.equipment_value = 10;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "common_metals", 1, "gems", 1);
    this.t.rigidity_rating = 2;
    this.t.base_stats["critical_chance"] = 0.03f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("amulet_bronze", "$amulet");
    this.t.material = "bronze";
    this.t.equipment_value = 15;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "common_metals", 1, "gems", 1);
    this.t.rigidity_rating = 3;
    this.t.base_stats["critical_chance"] = 0.04f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("amulet_silver", "$amulet");
    this.t.material = "silver";
    this.t.equipment_value = 20;
    this.t.setCost(0, "silver", 1, "gems", 1);
    this.t.rigidity_rating = 2;
    this.t.base_stats["critical_chance"] = 0.05f;
    this.t.base_stats["mana"] = 10f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("amulet_iron", "$amulet");
    this.t.material = "iron";
    this.t.equipment_value = 30;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "common_metals", 2, "gems", 1);
    this.t.rigidity_rating = 4;
    this.t.base_stats["critical_chance"] = 0.06f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("amulet_steel", "$amulet");
    this.t.material = "steel";
    this.t.equipment_value = 40;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "common_metals", 3, "gems", 1);
    this.t.rigidity_rating = 5;
    this.t.base_stats["critical_chance"] = 0.07f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("amulet_mythril", "$amulet");
    this.t.material = "mythril";
    this.t.equipment_value = 50;
    this.t.setCost(0, "mythril", 1, "gems", 1);
    this.t.rigidity_rating = 6;
    this.t.base_stats["critical_chance"] = 0.08f;
    this.t.base_stats["mana"] = 15f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("amulet_adamantine", "$amulet");
    this.t.material = "adamantine";
    this.t.equipment_value = 70;
    this.t.setCost(0, "adamantine", 1, "gems", 1);
    this.t.rigidity_rating = 7;
    this.t.base_stats["critical_chance"] = 0.1f;
    this.t.base_stats["mana"] = 20f;
    this.t.base_stats["stamina"] = 5f;
  }

  private void initHelmets()
  {
    this.clone("helmet_leather", "$helmet");
    this.t.material = "leather";
    this.t.equipment_value = 5;
    this.t.setCost(0, "leather", 1);
    this.t.rigidity_rating = 1;
    this.t.base_stats["armor"] = 2f;
    this.t.base_stats["stamina"] = 10f;
    this.t.base_stats["speed"] = 1f;
    this.clone("helmet_copper", "$helmet");
    this.t.material = "copper";
    this.t.equipment_value = 10;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "common_metals", 1);
    this.t.rigidity_rating = 2;
    this.t.base_stats["armor"] = 3f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("helmet_bronze", "$helmet");
    this.t.material = "bronze";
    this.t.equipment_value = 15;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "common_metals", 2);
    this.t.rigidity_rating = 3;
    this.t.base_stats["armor"] = 4f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("helmet_silver", "$helmet");
    this.t.material = "silver";
    this.t.equipment_value = 20;
    this.t.metallic = true;
    this.t.setCost(0, "silver", 1);
    this.t.rigidity_rating = 2;
    this.t.base_stats["armor"] = 5f;
    this.t.base_stats["mana"] = 15f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("helmet_iron", "$helmet");
    this.t.material = "iron";
    this.t.equipment_value = 30;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "common_metals", 3);
    this.t.rigidity_rating = 4;
    this.t.base_stats["armor"] = 6f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("helmet_steel", "$helmet");
    this.t.material = "steel";
    this.t.equipment_value = 40;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "common_metals", 4);
    this.t.rigidity_rating = 5;
    this.t.base_stats["armor"] = 7f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("helmet_mythril", "$helmet");
    this.t.material = "mythril";
    this.t.equipment_value = 50;
    this.t.metallic = true;
    this.t.setCost(0, "mythril", 1);
    this.t.rigidity_rating = 6;
    this.t.base_stats["armor"] = 8f;
    this.t.base_stats["mana"] = 25f;
    this.t.base_stats["stamina"] = 10f;
    this.clone("helmet_adamantine", "$helmet");
    this.t.material = "adamantine";
    this.t.equipment_value = 70;
    this.t.metallic = true;
    this.t.setCost(0, "adamantine", 1);
    this.t.rigidity_rating = 7;
    this.t.base_stats["armor"] = 10f;
    this.t.base_stats["mana"] = 35f;
    this.t.base_stats["stamina"] = 10f;
  }

  private void initArmors()
  {
    this.clone("armor_leather", "$armor");
    this.t.material = "leather";
    this.t.equipment_value = 5;
    this.t.setCost(0, "leather", 1);
    this.t.rigidity_rating = 1;
    this.t.base_stats["armor"] = 2f;
    this.t.base_stats["stamina"] = 20f;
    this.t.base_stats["speed"] = 1f;
    this.clone("armor_copper", "$armor");
    this.t.material = "copper";
    this.t.equipment_value = 10;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "common_metals", 1);
    this.t.rigidity_rating = 2;
    this.t.base_stats["armor"] = 3f;
    this.t.base_stats["stamina"] = 15f;
    this.clone("armor_bronze", "$armor");
    this.t.material = "bronze";
    this.t.equipment_value = 15;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "common_metals", 2);
    this.t.rigidity_rating = 3;
    this.t.base_stats["armor"] = 4f;
    this.t.base_stats["stamina"] = 10f;
    this.clone("armor_silver", "$armor");
    this.t.material = "silver";
    this.t.equipment_value = 20;
    this.t.metallic = true;
    this.t.setCost(0, "silver", 1);
    this.t.rigidity_rating = 2;
    this.t.base_stats["armor"] = 5f;
    this.t.base_stats["mana"] = 15f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("armor_iron", "$armor");
    this.t.material = "iron";
    this.t.equipment_value = 30;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "common_metals", 3);
    this.t.rigidity_rating = 4;
    this.t.base_stats["armor"] = 6f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("armor_steel", "$armor");
    this.t.material = "steel";
    this.t.equipment_value = 40;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "common_metals", 4);
    this.t.rigidity_rating = 5;
    this.t.base_stats["armor"] = 7f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("armor_mythril", "$armor");
    this.t.material = "mythril";
    this.t.equipment_value = 50;
    this.t.metallic = true;
    this.t.setCost(0, "mythril", 1);
    this.t.rigidity_rating = 6;
    this.t.base_stats["armor"] = 8f;
    this.t.base_stats["mana"] = 25f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("armor_adamantine", "$armor");
    this.t.material = "adamantine";
    this.t.equipment_value = 70;
    this.t.metallic = true;
    this.t.setCost(0, "adamantine", 1);
    this.t.rigidity_rating = 7;
    this.t.base_stats["armor"] = 10f;
    this.t.base_stats["mana"] = 35f;
    this.t.base_stats["stamina"] = 5f;
  }

  private void initBoots()
  {
    this.clone("boots_leather", "$boots");
    this.t.material = "leather";
    this.t.equipment_value = 5;
    this.t.setCost(0, "leather", 1);
    this.t.rigidity_rating = 1;
    this.t.base_stats["armor"] = 2f;
    this.t.base_stats["stamina"] = 10f;
    this.t.base_stats["speed"] = 1f;
    this.clone("boots_copper", "$boots");
    this.t.material = "copper";
    this.t.equipment_value = 10;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "common_metals", 1);
    this.t.rigidity_rating = 2;
    this.t.base_stats["armor"] = 3f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("boots_bronze", "$boots");
    this.t.material = "bronze";
    this.t.equipment_value = 15;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "common_metals", 2);
    this.t.rigidity_rating = 3;
    this.t.base_stats["armor"] = 4f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("boots_silver", "$boots");
    this.t.material = "silver";
    this.t.equipment_value = 20;
    this.t.metallic = true;
    this.t.setCost(0, "silver", 1);
    this.t.rigidity_rating = 2;
    this.t.base_stats["armor"] = 5f;
    this.t.base_stats["mana"] = 15f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("boots_iron", "$boots");
    this.t.material = "iron";
    this.t.equipment_value = 30;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "common_metals", 3);
    this.t.rigidity_rating = 4;
    this.t.base_stats["armor"] = 6f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("boots_steel", "$boots");
    this.t.material = "steel";
    this.t.equipment_value = 40;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "common_metals", 4);
    this.t.rigidity_rating = 5;
    this.t.base_stats["armor"] = 7f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("boots_mythril", "$boots");
    this.t.material = "mythril";
    this.t.equipment_value = 50;
    this.t.metallic = true;
    this.t.setCost(0, "mythril", 1);
    this.t.rigidity_rating = 6;
    this.t.base_stats["armor"] = 8f;
    this.t.base_stats["mana"] = 25f;
    this.t.base_stats["stamina"] = 10f;
    this.clone("boots_adamantine", "$boots");
    this.t.material = "adamantine";
    this.t.equipment_value = 70;
    this.t.metallic = true;
    this.t.setCost(0, "adamantine", 1);
    this.t.rigidity_rating = 7;
    this.t.base_stats["armor"] = 10f;
    this.t.base_stats["mana"] = 35f;
    this.t.base_stats["stamina"] = 10f;
  }

  private void initRings()
  {
    this.clone("ring_bone", "$ring");
    this.t.material = "bone";
    this.t.equipment_value = 5;
    this.t.setCost(0, "bones", 1, "gems", 1);
    this.t.rigidity_rating = 1;
    this.t.base_stats["critical_chance"] = 0.02f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("ring_copper", "$ring");
    this.t.material = "copper";
    this.t.equipment_value = 10;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "common_metals", 1, "gems", 1);
    this.t.rigidity_rating = 2;
    this.t.base_stats["critical_chance"] = 0.03f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("ring_bronze", "$ring");
    this.t.material = "bronze";
    this.t.equipment_value = 15;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "common_metals", 1, "gems", 1);
    this.t.rigidity_rating = 3;
    this.t.base_stats["critical_chance"] = 0.04f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("ring_silver", "$ring");
    this.t.material = "silver";
    this.t.equipment_value = 20;
    this.t.setCost(0, "silver", 1, "gems", 1);
    this.t.rigidity_rating = 2;
    this.t.base_stats["critical_chance"] = 0.05f;
    this.t.base_stats["mana"] = 10f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("ring_iron", "$ring");
    this.t.material = "iron";
    this.t.equipment_value = 30;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "common_metals", 2, "gems", 1);
    this.t.rigidity_rating = 4;
    this.t.base_stats["critical_chance"] = 0.06f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("ring_steel", "$ring");
    this.t.material = "steel";
    this.t.equipment_value = 40;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "common_metals", 3, "gems", 1);
    this.t.rigidity_rating = 5;
    this.t.base_stats["critical_chance"] = 0.07f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("ring_mythril", "$ring");
    this.t.material = "mythril";
    this.t.equipment_value = 50;
    this.t.setCost(0, "mythril", 1, "gems", 1);
    this.t.rigidity_rating = 6;
    this.t.base_stats["critical_chance"] = 0.08f;
    this.t.base_stats["mana"] = 15f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("ring_adamantine", "$ring");
    this.t.material = "adamantine";
    this.t.equipment_value = 70;
    this.t.setCost(0, "adamantine", 1, "gems", 1);
    this.t.rigidity_rating = 7;
    this.t.base_stats["critical_chance"] = 0.1f;
    this.t.base_stats["mana"] = 20f;
    this.t.base_stats["stamina"] = 5f;
  }

  private void initTemplates()
  {
    EquipmentAsset equipmentAsset = new EquipmentAsset();
    equipmentAsset.id = "$equipment";
    equipmentAsset.pool = "equipment";
    equipmentAsset.equipment_subtype = "basic";
    EquipmentAsset pAsset = equipmentAsset;
    this.t = equipmentAsset;
    this.add(pAsset);
    this.initTemplatesEquipment();
    this.initTemplatesWeapons();
  }

  private void initTemplatesEquipment()
  {
    this.clone("$armor", "$equipment");
    this.t.equipment_type = EquipmentType.Armor;
    this.t.name_class = "item_class_armor";
    this.t.equipment_subtype = "armor";
    this.t.name_templates = AssetLibrary<EquipmentAsset>.l<string>("armor_name");
    this.t.group_id = "armor";
    this.clone("$boots", "$equipment");
    this.t.equipment_type = EquipmentType.Boots;
    this.t.name_class = "item_class_armor";
    this.t.equipment_subtype = "boots";
    this.t.name_templates = AssetLibrary<EquipmentAsset>.l<string>("boots_name");
    this.t.group_id = "boots";
    this.clone("$helmet", "$equipment");
    this.t.equipment_type = EquipmentType.Helmet;
    this.t.name_class = "item_class_armor";
    this.t.equipment_subtype = "helmet";
    this.t.name_templates = AssetLibrary<EquipmentAsset>.l<string>("helmet_name");
    this.t.group_id = "helmet";
    this.clone("$accessory", "$equipment");
    this.t.name_class = "item_class_accessory";
    this.clone("$ring", "$accessory");
    this.t.equipment_subtype = "ring";
    this.t.name_templates = AssetLibrary<EquipmentAsset>.l<string>("ring_name");
    this.t.equipment_type = EquipmentType.Ring;
    this.t.group_id = "ring";
    this.clone("$amulet", "$accessory");
    this.t.equipment_type = EquipmentType.Amulet;
    this.t.equipment_subtype = "amulet";
    this.t.name_templates = AssetLibrary<EquipmentAsset>.l<string>("amulet_name");
    this.t.group_id = "amulet";
  }

  private void initTemplatesWeapons()
  {
    this.clone("$weapon", "$equipment");
    this.t.material = "basic";
    this.t.group_id = "sword";
    this.t.equipment_type = EquipmentType.Weapon;
    this.t.path_slash_animation = "effects/slashes/slash_base";
    this.t.name_class = "item_class_weapon";
    this.t.base_stats["damage_range"] = 0.5f;
    this.clone("$melee", "$weapon");
    this.t.pool = "melee";
    this.clone("$range", "$weapon");
    this.t.pool = "range";
    this.t.attack_type = WeaponType.Range;
    this.t.base_stats["projectiles"] = 1f;
    this.t.base_stats["damage_range"] = 0.6f;
    this.clone("$bow", "$range");
    this.t.equipment_subtype = "bow";
    this.t.is_pool_weapon = true;
    this.t.pool_rate = 10;
    this.t.projectile = "arrow";
    this.t.path_slash_animation = "effects/slashes/slash_bow";
    this.t.base_stats["critical_chance"] = 0.1f;
    this.t.base_stats["critical_damage_multiplier"] = 0.5f;
    this.t.base_stats["recoil"] = 1f;
    this.t.name_templates = new List<string>();
    this.t.name_templates.AddTimes<string>(30, "bow_name");
    this.t.name_templates.Add("weapon_name_city");
    this.t.name_templates.Add("weapon_name_kingdom");
    this.t.name_templates.Add("weapon_name_culture");
    this.t.name_templates.Add("weapon_name_enemy_king");
    this.t.name_templates.Add("weapon_name_enemy_kingdom");
    this.t.group_id = "bow";
    this.clone("$sword", "$melee");
    this.t.equipment_subtype = "sword";
    this.t.is_pool_weapon = true;
    this.t.pool_rate = 10;
    this.t.path_slash_animation = "effects/slashes/slash_sword";
    this.t.base_stats["damage"] = 1f;
    this.t.base_stats["damage_range"] = 0.8f;
    this.t.name_templates = new List<string>();
    this.t.name_templates.AddTimes<string>(30, "sword_name");
    this.t.name_templates.AddTimes<string>(3, "sword_name_king");
    this.t.name_templates.Add("weapon_name_city");
    this.t.name_templates.Add("weapon_name_kingdom");
    this.t.name_templates.Add("weapon_name_culture");
    this.t.name_templates.Add("weapon_name_enemy_king");
    this.t.name_templates.Add("weapon_name_enemy_kingdom");
    this.t.group_id = "sword";
    this.clone("$axe", "$melee");
    this.t.equipment_subtype = "axe";
    this.t.is_pool_weapon = true;
    this.t.pool_rate = 10;
    this.t.path_slash_animation = "effects/slashes/slash_axe";
    this.t.base_stats["damage_range"] = 0.6f;
    this.t.name_templates = new List<string>();
    this.t.name_templates.AddTimes<string>(30, "axe_name");
    this.t.name_templates.AddTimes<string>(3, "axe_name_king");
    this.t.name_templates.Add("weapon_name_city");
    this.t.name_templates.Add("weapon_name_kingdom");
    this.t.name_templates.Add("weapon_name_culture");
    this.t.name_templates.Add("weapon_name_enemy_king");
    this.t.name_templates.Add("weapon_name_enemy_kingdom");
    this.t.group_id = "axe";
    this.clone("$hammer", "$melee");
    this.t.equipment_subtype = "hammer";
    this.t.is_pool_weapon = true;
    this.t.pool_rate = 10;
    this.t.path_slash_animation = "effects/slashes/slash_hammer";
    this.t.base_stats["targets"] = 2f;
    this.t.base_stats["damage_range"] = 0.1f;
    this.t.name_templates = new List<string>();
    this.t.name_templates.AddTimes<string>(30, "hammer_name");
    this.t.name_templates.Add("weapon_name_city");
    this.t.name_templates.Add("weapon_name_kingdom");
    this.t.name_templates.Add("weapon_name_culture");
    this.t.name_templates.Add("weapon_name_enemy_king");
    this.t.name_templates.Add("weapon_name_enemy_kingdom");
    this.t.group_id = "hammer";
    this.t.item_modifier_ids = AssetLibrary<EquipmentAsset>.a<string>("stun");
    this.clone("$spear", "$melee");
    this.t.equipment_subtype = "spear";
    this.t.is_pool_weapon = true;
    this.t.pool_rate = 10;
    this.t.path_slash_animation = "effects/slashes/slash_spear";
    this.t.base_stats["range"] = 1f;
    this.t.base_stats["damage_range"] = 0.7f;
    this.t.name_templates = new List<string>();
    this.t.name_templates.AddTimes<string>(30, "spear_name");
    this.t.name_templates.Add("weapon_name_city");
    this.t.name_templates.Add("weapon_name_kingdom");
    this.t.name_templates.Add("weapon_name_culture");
    this.t.name_templates.Add("weapon_name_enemy_king");
    this.t.name_templates.Add("weapon_name_enemy_kingdom");
    this.t.group_id = "spear";
  }

  private void initNormalWeapons()
  {
    this.initWeaponsBasic();
    this.initWeaponsAdvanced();
  }

  private void initWeaponsAdvanced()
  {
    this.initWeaponsBows();
    this.initWeaponsSwords();
    this.initWeaponsAxes();
    this.initWeaponsSpears();
    this.initWeaponsHammers();
  }

  private void initWeaponsBasic()
  {
    this.clone("stick_wood", "$melee");
    this.t.equipment_subtype = "stick";
    this.t.material = "wood";
    this.t.is_pool_weapon = true;
    this.t.pool_rate = 10;
    this.t.path_slash_animation = "effects/slashes/slash_punch";
    this.t.name_templates = new List<string>();
    this.t.name_templates.AddTimes<string>(30, "stick_name");
    this.t.name_templates.Add("weapon_name_city");
    this.t.name_templates.Add("weapon_name_kingdom");
    this.t.name_templates.Add("weapon_name_culture");
    this.t.name_templates.Add("weapon_name_enemy_king");
    this.t.name_templates.Add("weapon_name_enemy_kingdom");
    this.t.group_id = "staff";
    this.t.equipment_value = 1;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "wood", 1);
    this.t.rigidity_rating = 1;
    this.t.base_stats["damage"] = 1f;
    this.t.base_stats["stamina"] = 5f;
    this.t.base_stats["mana"] = 5f;
  }

  private void initWeaponsSwords()
  {
    this.clone("sword_wood", "$sword");
    this.t.material = "wood";
    this.t.equipment_value = 1;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "wood", 1);
    this.t.rigidity_rating = 1;
    this.t.base_stats["damage"] = 1f;
    this.t.base_stats["stamina"] = 15f;
    this.clone("sword_stone", "$sword");
    this.t.material = "stone";
    this.t.equipment_value = 10;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "stone", 1);
    this.t.rigidity_rating = 2;
    this.t.base_stats["damage"] = 3f;
    this.t.base_stats["speed"] = -2f;
    this.clone("sword_copper", "$sword");
    this.t.material = "copper";
    this.t.equipment_value = 10;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 10;
    this.t.setCost(0, "common_metals", 1);
    this.t.rigidity_rating = 2;
    this.t.base_stats["damage"] = 3f;
    this.t.base_stats["stamina"] = 10f;
    this.clone("sword_bronze", "$sword");
    this.t.material = "bronze";
    this.t.equipment_value = 15;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 10;
    this.t.setCost(0, "common_metals", 2);
    this.t.rigidity_rating = 3;
    this.t.base_stats["damage"] = 4f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("sword_silver", "$sword");
    this.t.material = "silver";
    this.t.equipment_value = 20;
    this.t.metallic = true;
    this.t.setCost(0, "silver", 1);
    this.t.rigidity_rating = 2;
    this.t.base_stats["damage"] = 5f;
    this.t.base_stats["mana"] = 15f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("sword_iron", "$sword");
    this.t.material = "iron";
    this.t.equipment_value = 30;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 10;
    this.t.setCost(0, "common_metals", 3);
    this.t.rigidity_rating = 4;
    this.t.base_stats["damage"] = 6f;
    this.t.base_stats["critical_chance"] = 0.05f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("sword_steel", "$sword");
    this.t.material = "steel";
    this.t.equipment_value = 40;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 10;
    this.t.setCost(0, "common_metals", 4);
    this.t.rigidity_rating = 5;
    this.t.base_stats["damage"] = 7f;
    this.t.base_stats["speed"] = 1f;
    this.t.base_stats["critical_chance"] = 0.05f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("sword_mythril", "$sword");
    this.t.material = "mythril";
    this.t.equipment_value = 50;
    this.t.metallic = true;
    this.t.setCost(0, "mythril", 1);
    this.t.rigidity_rating = 6;
    this.t.base_stats["damage"] = 8f;
    this.t.base_stats["critical_chance"] = 0.05f;
    this.t.base_stats["mana"] = 25f;
    this.t.base_stats["stamina"] = 10f;
    this.clone("sword_adamantine", "$sword");
    this.t.material = "adamantine";
    this.t.equipment_value = 70;
    this.t.metallic = true;
    this.t.setCost(0, "adamantine", 1);
    this.t.rigidity_rating = 7;
    this.t.base_stats["damage"] = 10f;
    this.t.base_stats["speed"] = 1f;
    this.t.base_stats["critical_chance"] = 0.15f;
    this.t.base_stats["mana"] = 35f;
    this.t.base_stats["stamina"] = 10f;
  }

  private void initWeaponsBows()
  {
    this.clone("bow_wood", "$bow");
    this.t.material = "wood";
    this.t.equipment_value = 1;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "wood", 1);
    this.t.rigidity_rating = 1;
    this.t.base_stats["damage"] = 1f;
    this.t.base_stats["range"] = 6f;
    this.t.base_stats["stamina"] = 15f;
    this.clone("bow_copper", "$bow");
    this.t.material = "copper";
    this.t.equipment_value = 10;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 10;
    this.t.setCost(0, "common_metals", 1);
    this.t.rigidity_rating = 2;
    this.t.base_stats["damage"] = 3f;
    this.t.base_stats["range"] = 6f;
    this.t.base_stats["stamina"] = 10f;
    this.clone("bow_bronze", "$bow");
    this.t.material = "bronze";
    this.t.equipment_value = 15;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 10;
    this.t.setCost(0, "common_metals", 2);
    this.t.rigidity_rating = 3;
    this.t.base_stats["damage"] = 4f;
    this.t.base_stats["range"] = 7f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("bow_silver", "$bow");
    this.t.material = "silver";
    this.t.equipment_value = 20;
    this.t.metallic = true;
    this.t.setCost(0, "silver", 1);
    this.t.rigidity_rating = 2;
    this.t.base_stats["damage"] = 5f;
    this.t.base_stats["range"] = 8f;
    this.t.base_stats["mana"] = 15f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("bow_iron", "$bow");
    this.t.material = "iron";
    this.t.equipment_value = 30;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 10;
    this.t.setCost(0, "common_metals", 3);
    this.t.rigidity_rating = 4;
    this.t.base_stats["damage"] = 6f;
    this.t.base_stats["critical_chance"] = 0.05f;
    this.t.base_stats["range"] = 9f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("bow_steel", "$bow");
    this.t.material = "steel";
    this.t.equipment_value = 40;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 10;
    this.t.setCost(0, "common_metals", 4);
    this.t.rigidity_rating = 5;
    this.t.base_stats["damage"] = 7f;
    this.t.base_stats["speed"] = 1f;
    this.t.base_stats["critical_chance"] = 0.05f;
    this.t.base_stats["range"] = 10f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("bow_mythril", "$bow");
    this.t.material = "mythril";
    this.t.equipment_value = 50;
    this.t.metallic = true;
    this.t.setCost(0, "mythril", 1);
    this.t.rigidity_rating = 6;
    this.t.base_stats["damage"] = 8f;
    this.t.base_stats["range"] = 11f;
    this.t.base_stats["critical_chance"] = 0.05f;
    this.t.base_stats["mana"] = 25f;
    this.t.base_stats["stamina"] = 10f;
    this.clone("bow_adamantine", "$bow");
    this.t.material = "adamantine";
    this.t.equipment_value = 70;
    this.t.metallic = true;
    this.t.setCost(0, "adamantine", 1);
    this.t.rigidity_rating = 7;
    this.t.base_stats["damage"] = 10f;
    this.t.base_stats["range"] = 12f;
    this.t.base_stats["speed"] = 1f;
    this.t.base_stats["critical_chance"] = 0.15f;
    this.t.base_stats["mana"] = 35f;
    this.t.base_stats["stamina"] = 10f;
  }

  private void initWeaponsAxes()
  {
    this.clone("axe_wood", "$axe");
    this.t.material = "wood";
    this.t.equipment_value = 1;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "wood", 1);
    this.t.rigidity_rating = 1;
    this.t.base_stats["damage"] = 1f;
    this.t.base_stats["stamina"] = 15f;
    this.clone("axe_stone", "$axe");
    this.t.material = "stone";
    this.t.equipment_value = 10;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "stone", 1);
    this.t.rigidity_rating = 2;
    this.t.base_stats["damage"] = 3f;
    this.t.base_stats["speed"] = -2f;
    this.clone("axe_copper", "$axe");
    this.t.material = "copper";
    this.t.equipment_value = 10;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 10;
    this.t.setCost(0, "common_metals", 1);
    this.t.rigidity_rating = 2;
    this.t.base_stats["damage"] = 3f;
    this.t.base_stats["stamina"] = 10f;
    this.clone("axe_bronze", "$axe");
    this.t.material = "bronze";
    this.t.equipment_value = 15;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 10;
    this.t.setCost(0, "common_metals", 2);
    this.t.rigidity_rating = 3;
    this.t.base_stats["damage"] = 4f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("axe_silver", "$axe");
    this.t.material = "silver";
    this.t.equipment_value = 20;
    this.t.metallic = true;
    this.t.setCost(0, "silver", 1);
    this.t.rigidity_rating = 2;
    this.t.base_stats["damage"] = 5f;
    this.t.base_stats["mana"] = 15f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("axe_iron", "$axe");
    this.t.material = "iron";
    this.t.equipment_value = 30;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 10;
    this.t.setCost(0, "common_metals", 3);
    this.t.rigidity_rating = 4;
    this.t.base_stats["damage"] = 6f;
    this.t.base_stats["critical_chance"] = 0.05f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("axe_steel", "$axe");
    this.t.material = "steel";
    this.t.equipment_value = 40;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 10;
    this.t.setCost(0, "common_metals", 4);
    this.t.rigidity_rating = 5;
    this.t.base_stats["damage"] = 7f;
    this.t.base_stats["speed"] = 1f;
    this.t.base_stats["critical_chance"] = 0.05f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("axe_mythril", "$axe");
    this.t.material = "mythril";
    this.t.equipment_value = 50;
    this.t.metallic = true;
    this.t.setCost(0, "mythril", 1);
    this.t.rigidity_rating = 6;
    this.t.base_stats["damage"] = 8f;
    this.t.base_stats["critical_chance"] = 0.05f;
    this.t.base_stats["mana"] = 25f;
    this.t.base_stats["stamina"] = 10f;
    this.clone("axe_adamantine", "$axe");
    this.t.material = "adamantine";
    this.t.equipment_value = 70;
    this.t.metallic = true;
    this.t.setCost(0, "adamantine", 1);
    this.t.rigidity_rating = 7;
    this.t.base_stats["damage"] = 10f;
    this.t.base_stats["speed"] = 1f;
    this.t.base_stats["critical_chance"] = 0.15f;
    this.t.base_stats["mana"] = 35f;
    this.t.base_stats["stamina"] = 10f;
  }

  private void initWeaponsSpears()
  {
    this.clone("spear_wood", "$spear");
    this.t.material = "wood";
    this.t.equipment_value = 1;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "wood", 1);
    this.t.rigidity_rating = 1;
    this.t.base_stats["damage"] = 1f;
    this.t.base_stats["stamina"] = 15f;
    this.clone("spear_stone", "$spear");
    this.t.material = "stone";
    this.t.equipment_value = 10;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "stone", 1);
    this.t.rigidity_rating = 2;
    this.t.base_stats["damage"] = 3f;
    this.t.base_stats["speed"] = -2f;
    this.clone("spear_copper", "$spear");
    this.t.material = "copper";
    this.t.equipment_value = 10;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 10;
    this.t.setCost(0, "common_metals", 1);
    this.t.rigidity_rating = 2;
    this.t.base_stats["damage"] = 3f;
    this.t.base_stats["stamina"] = 10f;
    this.clone("spear_bronze", "$spear");
    this.t.material = "bronze";
    this.t.equipment_value = 15;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 10;
    this.t.setCost(0, "common_metals", 2);
    this.t.rigidity_rating = 3;
    this.t.base_stats["damage"] = 4f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("spear_silver", "$spear");
    this.t.material = "silver";
    this.t.equipment_value = 20;
    this.t.metallic = true;
    this.t.setCost(0, "silver", 1);
    this.t.rigidity_rating = 2;
    this.t.base_stats["damage"] = 5f;
    this.t.base_stats["mana"] = 15f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("spear_iron", "$spear");
    this.t.material = "iron";
    this.t.equipment_value = 30;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 10;
    this.t.setCost(0, "common_metals", 3);
    this.t.rigidity_rating = 4;
    this.t.base_stats["damage"] = 6f;
    this.t.base_stats["critical_chance"] = 0.05f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("spear_steel", "$spear");
    this.t.material = "steel";
    this.t.equipment_value = 40;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 10;
    this.t.setCost(0, "common_metals", 4);
    this.t.rigidity_rating = 5;
    this.t.base_stats["damage"] = 7f;
    this.t.base_stats["speed"] = 1f;
    this.t.base_stats["critical_chance"] = 0.05f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("spear_mythril", "$spear");
    this.t.material = "mythril";
    this.t.equipment_value = 50;
    this.t.metallic = true;
    this.t.setCost(0, "mythril", 1);
    this.t.rigidity_rating = 6;
    this.t.base_stats["damage"] = 8f;
    this.t.base_stats["critical_chance"] = 0.05f;
    this.t.base_stats["mana"] = 25f;
    this.t.base_stats["stamina"] = 10f;
    this.clone("spear_adamantine", "$spear");
    this.t.material = "adamantine";
    this.t.equipment_value = 70;
    this.t.metallic = true;
    this.t.setCost(0, "adamantine", 1);
    this.t.rigidity_rating = 7;
    this.t.base_stats["damage"] = 10f;
    this.t.base_stats["speed"] = 1f;
    this.t.base_stats["critical_chance"] = 0.15f;
    this.t.base_stats["mana"] = 35f;
    this.t.base_stats["stamina"] = 10f;
  }

  private void initWeaponsHammers()
  {
    this.clone("hammer_wood", "$hammer");
    this.t.material = "wood";
    this.t.equipment_value = 1;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "wood", 1);
    this.t.rigidity_rating = 1;
    this.t.base_stats["damage"] = 1f;
    this.t.base_stats["stamina"] = 15f;
    this.clone("hammer_stone", "$hammer");
    this.t.material = "stone";
    this.t.equipment_value = 10;
    this.t.minimum_city_storage_resource_1 = 15;
    this.t.setCost(0, "stone", 1);
    this.t.rigidity_rating = 2;
    this.t.base_stats["damage"] = 3f;
    this.t.base_stats["speed"] = -2f;
    this.clone("hammer_copper", "$hammer");
    this.t.material = "copper";
    this.t.equipment_value = 10;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 10;
    this.t.setCost(0, "common_metals", 1);
    this.t.rigidity_rating = 2;
    this.t.base_stats["damage"] = 3f;
    this.t.base_stats["stamina"] = 10f;
    this.clone("hammer_bronze", "$hammer");
    this.t.material = "bronze";
    this.t.equipment_value = 15;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 10;
    this.t.setCost(0, "common_metals", 2);
    this.t.rigidity_rating = 3;
    this.t.base_stats["damage"] = 4f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("hammer_silver", "$hammer");
    this.t.material = "silver";
    this.t.equipment_value = 20;
    this.t.metallic = true;
    this.t.setCost(0, "silver", 1);
    this.t.rigidity_rating = 2;
    this.t.base_stats["damage"] = 5f;
    this.t.base_stats["mana"] = 15f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("hammer_iron", "$hammer");
    this.t.material = "iron";
    this.t.equipment_value = 30;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 10;
    this.t.setCost(0, "common_metals", 3);
    this.t.rigidity_rating = 4;
    this.t.base_stats["damage"] = 6f;
    this.t.base_stats["critical_chance"] = 0.05f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("hammer_steel", "$hammer");
    this.t.material = "steel";
    this.t.equipment_value = 40;
    this.t.metallic = true;
    this.t.minimum_city_storage_resource_1 = 10;
    this.t.setCost(0, "common_metals", 4);
    this.t.rigidity_rating = 5;
    this.t.base_stats["damage"] = 7f;
    this.t.base_stats["speed"] = 1f;
    this.t.base_stats["critical_chance"] = 0.05f;
    this.t.base_stats["stamina"] = 5f;
    this.clone("hammer_mythril", "$hammer");
    this.t.material = "mythril";
    this.t.equipment_value = 50;
    this.t.metallic = true;
    this.t.setCost(0, "mythril", 1);
    this.t.rigidity_rating = 6;
    this.t.base_stats["damage"] = 8f;
    this.t.base_stats["critical_chance"] = 0.05f;
    this.t.base_stats["mana"] = 25f;
    this.t.base_stats["stamina"] = 10f;
    this.clone("hammer_adamantine", "$hammer");
    this.t.material = "adamantine";
    this.t.equipment_value = 70;
    this.t.metallic = true;
    this.t.setCost(0, "adamantine", 1);
    this.t.rigidity_rating = 7;
    this.t.base_stats["damage"] = 10f;
    this.t.base_stats["speed"] = 1f;
    this.t.base_stats["critical_chance"] = 0.15f;
    this.t.base_stats["mana"] = 35f;
    this.t.base_stats["stamina"] = 10f;
  }

  private void initWeaponsUnique()
  {
    this.clone("alien_blaster", "$range");
    this.t.setUnlockedWithAchievement("achievementEquipmentExplorer");
    this.t.equipment_subtype = "alien_blaster";
    this.t.setCost(100, "adamantine", 10, "gems", 20);
    this.t.rigidity_rating = 7;
    this.t.is_pool_weapon = true;
    this.t.pool_rate = 1;
    this.t.path_icon = "ui/Icons/items/icon_alien_blaster";
    this.t.material = "basic";
    this.t.projectile = "plasma_ball";
    this.t.path_slash_animation = "effects/slashes/slash_punch";
    this.t.base_stats["range"] = 20f;
    this.t.base_stats["critical_chance"] = 0.1f;
    this.t.base_stats["targets"] = 1f;
    this.t.base_stats["critical_damage_multiplier"] = 0.5f;
    this.t.base_stats["damage_range"] = 0.6f;
    this.t.base_stats["mana"] = 20f;
    this.t.base_stats["stamina"] = 20f;
    this.t.equipment_value = 500;
    this.t.base_stats["damage"] = 30f;
    this.t.name_templates = AssetLibrary<EquipmentAsset>.l<string>("blaster_name");
    this.t.group_id = "firearm";
    this.clone("shotgun", "$range");
    this.t.setUnlockedWithAchievement("achievementTLDR");
    this.t.equipment_subtype = "shotgun";
    this.t.setCost(100, "adamantine", 10, "mythril", 5);
    this.t.rigidity_rating = 6;
    this.t.is_pool_weapon = true;
    this.t.pool_rate = 1;
    this.t.path_icon = "ui/Icons/items/icon_shotgun";
    this.t.projectile = "shotgun_bullet";
    this.t.path_slash_animation = "effects/slashes/slash_punch";
    this.t.base_stats["projectiles"] = 12f;
    this.t.base_stats["range"] = 10f;
    this.t.base_stats["targets"] = 1f;
    this.t.base_stats["damage"] = 10f;
    this.t.base_stats["damage_range"] = 0.9f;
    this.t.base_stats["mana"] = 5f;
    this.t.base_stats["stamina"] = 10f;
    this.t.equipment_value = 600;
    this.t.name_templates = AssetLibrary<EquipmentAsset>.l<string>("shotgun_name");
    this.t.group_id = "firearm";
    this.clone("flame_hammer", "$weapon");
    this.t.setUnlockedWithAchievement("achievementGodlySmithing");
    this.t.equipment_subtype = "flame_hammer";
    this.t.setCost(10, "dragon_scales", 3);
    this.t.is_pool_weapon = true;
    this.t.animated = true;
    this.t.pool_rate = 2;
    this.t.material = "basic";
    this.t.path_slash_animation = "effects/slashes/slash_hammer";
    this.t.rigidity_rating = 6;
    this.t.base_stats["damage"] = 20f;
    this.t.base_stats["speed"] = 1f;
    this.t.base_stats["critical_chance"] = 0.15f;
    this.t.base_stats["targets"] = 3f;
    this.t.base_stats["mana"] = 15f;
    this.t.base_stats["stamina"] = 15f;
    this.t.equipment_value = 400;
    this.t.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_hammer_name");
    this.t.item_modifier_ids = AssetLibrary<EquipmentAsset>.a<string>("flame");
    this.t.group_id = "hammer";
    this.clone("ice_hammer", "$weapon");
    this.t.setUnlockedWithAchievement("achievementMakeWilhelmScream");
    this.t.equipment_subtype = "ice_hammer";
    this.t.setCost(10, "mythril", 10, "gems", 2);
    this.t.rigidity_rating = 6;
    this.t.is_pool_weapon = true;
    this.t.animated = true;
    this.t.pool_rate = 2;
    this.t.material = "basic";
    this.t.path_slash_animation = "effects/slashes/slash_hammer";
    this.t.base_stats["damage"] = 20f;
    this.t.base_stats["speed"] = 1f;
    this.t.base_stats["critical_chance"] = 0.15f;
    this.t.base_stats["targets"] = 3f;
    this.t.base_stats["mana"] = 15f;
    this.t.base_stats["stamina"] = 15f;
    this.t.equipment_value = 400;
    this.t.name_templates = AssetLibrary<EquipmentAsset>.l<string>("ice_hammer_name");
    this.t.item_modifier_ids = AssetLibrary<EquipmentAsset>.a<string>("ice", "stun");
    this.t.group_id = "hammer";
    this.clone("flame_sword", "$weapon");
    this.t.equipment_subtype = "flame_sword";
    this.t.setCost(0, "dragon_scales", 2);
    this.t.is_pool_weapon = true;
    this.t.animated = true;
    this.t.pool_rate = 2;
    this.t.material = "basic";
    this.t.path_slash_animation = "effects/slashes/slash_sword";
    this.t.rigidity_rating = 6;
    this.t.base_stats["damage"] = 33f;
    this.t.base_stats["targets"] = 2f;
    this.t.base_stats["critical_damage_multiplier"] = 0.1f;
    this.t.base_stats["mana"] = 15f;
    this.t.base_stats["stamina"] = 15f;
    this.t.equipment_value = 300;
    this.t.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
    this.t.item_modifier_ids = AssetLibrary<EquipmentAsset>.a<string>("flame");
    this.t.group_id = "sword";
    this.t.base_stats.addTag("building_immunity_fire");
    this.clone("necromancer_staff", "$range");
    this.t.equipment_subtype = "necromancer_staff";
    this.t.setCost(10, "mythril", 2, "gems", 3);
    this.t.is_pool_weapon = true;
    this.t.pool_rate = 1;
    this.t.material = "basic";
    this.t.projectile = "skull";
    this.t.path_slash_animation = "effects/slashes/slash_punch";
    this.t.rigidity_rating = 5;
    this.t.base_stats["range"] = 13f;
    this.t.base_stats["critical_chance"] = 0.1f;
    this.t.base_stats["targets"] = 1f;
    this.t.base_stats["critical_damage_multiplier"] = 0.5f;
    this.t.base_stats["mana"] = 40f;
    this.t.equipment_value = 500;
    this.t.base_stats["damage"] = 30f;
    this.t.name_templates = AssetLibrary<EquipmentAsset>.l<string>("necromancer_staff_name");
    this.t.group_id = "staff";
    this.t.addSpell("spawn_skeleton");
    this.t.addSpell("cast_curse");
    this.clone("evil_staff", "$range");
    this.t.equipment_subtype = "evil_staff";
    this.t.setCost(20, "mythril", 3, "gems", 2);
    this.t.is_pool_weapon = true;
    this.t.durability = 300;
    this.t.pool_rate = 1;
    this.t.material = "basic";
    this.t.projectile = "red_orb";
    this.t.path_slash_animation = "effects/slashes/slash_punch";
    this.t.rigidity_rating = 5;
    this.t.base_stats["range"] = 13f;
    this.t.base_stats["critical_chance"] = 0.1f;
    this.t.base_stats["targets"] = 1f;
    this.t.base_stats["critical_damage_multiplier"] = 0.5f;
    this.t.base_stats["mana"] = 40f;
    this.t.equipment_value = 500;
    this.t.base_stats["projectiles"] = 20f;
    this.t.base_stats["damage"] = 10f;
    this.t.name_templates = AssetLibrary<EquipmentAsset>.l<string>("evil_staff_name");
    this.t.item_modifier_ids = AssetLibrary<EquipmentAsset>.a<string>("flame");
    this.t.group_id = "staff";
    this.t.base_stats.addTag("building_immunity_fire");
    this.t.addSpell("cast_fire");
    this.clone("white_staff", "$range");
    this.t.equipment_subtype = "white_staff";
    this.t.setCost(20, "mythril", 3, "gems", 2);
    this.t.is_pool_weapon = true;
    this.t.durability = 300;
    this.t.pool_rate = 3;
    this.t.material = "basic";
    this.t.projectile = "freeze_orb";
    this.t.path_slash_animation = "effects/slashes/slash_punch";
    this.t.rigidity_rating = 5;
    this.t.base_stats["range"] = 18f;
    this.t.base_stats["critical_chance"] = 0.1f;
    this.t.base_stats["targets"] = 1f;
    this.t.base_stats["critical_damage_multiplier"] = 0.5f;
    this.t.base_stats["damage"] = 35f;
    this.t.base_stats["mana"] = 40f;
    this.t.equipment_value = 500;
    this.t.name_templates = AssetLibrary<EquipmentAsset>.l<string>("white_staff_name");
    this.t.item_modifier_ids = AssetLibrary<EquipmentAsset>.a<string>("ice");
    this.t.group_id = "staff";
    this.t.addSpell("cast_blood_rain");
    this.t.addSpell("summon_lightning");
    this.clone("plague_doctor_staff", "$weapon");
    this.t.equipment_subtype = "plague_doctor_staff";
    this.t.setCost(5, "mythril", 2, "gems", 1);
    this.t.is_pool_weapon = true;
    this.t.pool_rate = 3;
    this.t.material = "basic";
    this.t.path_slash_animation = "effects/slashes/slash_punch";
    this.t.rigidity_rating = 5;
    this.t.base_stats["critical_chance"] = 0.1f;
    this.t.base_stats["targets"] = 3f;
    this.t.base_stats["critical_damage_multiplier"] = 0.5f;
    this.t.base_stats["damage"] = 35f;
    this.t.base_stats["mana"] = 40f;
    this.t.equipment_value = 200;
    this.t.name_templates = AssetLibrary<EquipmentAsset>.l<string>("plague_doctor_staff_name");
    this.t.group_id = "staff";
    this.t.addSpell("cast_fire");
    this.t.addSpell("cast_cure");
    this.clone("druid_staff", "$range");
    this.t.equipment_subtype = "druid_staff";
    this.t.setCost(7, "mythril", 3, "gems", 1);
    this.t.is_pool_weapon = true;
    this.t.pool_rate = 3;
    this.t.material = "basic";
    this.t.projectile = "green_orb";
    this.t.path_slash_animation = "effects/slashes/slash_punch";
    this.t.rigidity_rating = 5;
    this.t.base_stats["range"] = 20f;
    this.t.base_stats["critical_chance"] = 0.1f;
    this.t.base_stats["targets"] = 1f;
    this.t.base_stats["critical_damage_multiplier"] = 0.3f;
    this.t.base_stats["damage"] = 12f;
    this.t.base_stats["mana"] = 40f;
    this.t.equipment_value = 300;
    this.t.base_stats["projectiles"] = 2f;
    this.t.name_templates = AssetLibrary<EquipmentAsset>.l<string>("druid_staff_name");
    this.t.item_modifier_ids = AssetLibrary<EquipmentAsset>.a<string>("slowness");
    this.t.group_id = "staff";
    this.t.addSpell("cast_blood_rain");
    this.t.addSpell("spawn_vegetation");
  }
}
