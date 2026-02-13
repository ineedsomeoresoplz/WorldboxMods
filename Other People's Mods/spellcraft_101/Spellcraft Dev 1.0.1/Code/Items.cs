using System;
using NCMS;
using UnityEngine;
using ReflectionUtility;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using NCMS.Utils;

namespace spellcraft
{
    class Items
    {
        public static void init()
        {


          //EFFECTS
          StatusEffect redShield = new StatusEffect();
			    redShield.id = "redShield";
			    redShield.texture = "redShield";
			    redShield.animated = true;
			    redShield.animation_speed = 0.1f;
          redShield.baseStats.armor = 60;
          redShield.baseStats.damage = 100;
          redShield.baseStats.knockbackReduction = 100f;
			    redShield.duration = 20f;
          redShield.removeStatus.Add("burning");
		      redShield.oppositeStatus.Add("frozen");
			    AssetManager.status.add(redShield);


          //WEAPONS
          ItemAsset ColdStaff = AssetManager.items.clone("Cold Staff", "_range");
          ColdStaff.id = "Cold Staff";
          ColdStaff.name_templates = Toolbox.splitStringIntoList(new string[]
          {
            "sword_name#30",
            "sword_name_king#3",
            "weapon_name_city",
            "weapon_name_kingdom",
            "weapon_name_culture",
            "weapon_name_enemy_king",
            "weapon_name_enemy_kingdom"
          });
          ColdStaff.materials = List.Of<string>(new string[]{"mythril"});
          ColdStaff.projectile = "freeze_orb";
          ColdStaff.baseStats.projectiles = 7;
          ColdStaff.baseStats.range = 25f;
          ColdStaff.baseStats.accuracy = -10;
          ColdStaff.baseStats.attackSpeed = -25f;
          ColdStaff.baseStats.damage = -5;
          ColdStaff.equipment_value = 400;
          ColdStaff.slash = "punch";
          ColdStaff.attackAction = (WorldAction)Delegate.Combine(ColdStaff.attackAction, new WorldAction(ColdStaffAttack));
          AssetManager.items.list.AddItem(ColdStaff);
          Localization.addLocalization("item_Cold Staff", "Cold Staff");
          addStaffSprite(ColdStaff.id, ColdStaff.materials[0]);

          ItemAsset FlameStaff = AssetManager.items.clone("Flame Staff", "_range");
          FlameStaff.id = "Flame Staff";
          FlameStaff.name_templates = Toolbox.splitStringIntoList(new string[]
          {
            "sword_name#30",
            "sword_name_king#3",
            "weapon_name_city",
            "weapon_name_kingdom",
            "weapon_name_culture",
            "weapon_name_enemy_king",
            "weapon_name_enemy_kingdom"
          });
          FlameStaff.materials = List.Of<string>(new string[]{"mythril"});
          FlameStaff.projectile = "fireball";
          FlameStaff.baseStats.projectiles = 2;
          FlameStaff.baseStats.range = 25f;
          FlameStaff.baseStats.accuracy = -30;
          FlameStaff.baseStats.attackSpeed = -60f;
          FlameStaff.baseStats.damage = 35;
          FlameStaff.equipment_value = 400;
          FlameStaff.slash = "punch";
          FlameStaff.attackAction = (WorldAction)Delegate.Combine(FlameStaff.attackAction, new WorldAction(FlameStaffAttack));
          AssetManager.items.list.AddItem(FlameStaff);
          Localization.addLocalization("item_Flame Staff", "Flame Staff");
          addStaffSprite(FlameStaff.id, FlameStaff.materials[0]);

          ItemAsset AirbindStaff = AssetManager.items.clone("Airbind Staff", "_range");
          AirbindStaff.id = "Airbind Staff";
          AirbindStaff.name_templates = Toolbox.splitStringIntoList(new string[]
          {
            "sword_name#30",
            "sword_name_king#3",
            "weapon_name_city",
            "weapon_name_kingdom",
            "weapon_name_culture",
            "weapon_name_enemy_king",
            "weapon_name_enemy_kingdom"
          });
          AirbindStaff.materials = List.Of<string>(new string[]{"mythril"});
          AirbindStaff.projectile = "plasma_ball";
          AirbindStaff.baseStats.projectiles = 1;
          AirbindStaff.baseStats.range = 35f;
          AirbindStaff.baseStats.accuracy = 50;
          AirbindStaff.baseStats.attackSpeed = 30f;
          AirbindStaff.baseStats.damage = -10;
          AirbindStaff.equipment_value = 400;
          AirbindStaff.slash = "punch";
          AirbindStaff.attackAction = (WorldAction)Delegate.Combine(AirbindStaff.attackAction, new WorldAction(AirbindStaffAttack));
          AssetManager.items.list.AddItem(AirbindStaff);
          Localization.addLocalization("item_Airbind Staff", "Airbind Staff");
          addStaffSprite(AirbindStaff.id, AirbindStaff.materials[0]);
          

        //ACCESSORIES

          ItemAsset rubyamulet = AssetManager.items.clone("Ruby Amulet", "_accessory");
          rubyamulet.id = "Ruby Amulet";
          rubyamulet.name_templates = List.Of<string>(new string[]{"amulet_name"});
          rubyamulet.equipmentType = EquipmentType.Amulet;
          rubyamulet.baseStats.mod_damage = 100f;
          rubyamulet.materials = List.Of<string>(new string[]{"iron"});
          rubyamulet.equipment_value = 400;
          AssetManager.items.list.AddItem(rubyamulet);
          Localization.addLocalization("item_Ruby Amulet", "Ruby Amulet");

          ItemAsset emeraldamulet = AssetManager.items.clone("Emerald Amulet", "_accessory");
          emeraldamulet.id = "Emerald Amulet";
          emeraldamulet.name_templates = List.Of<string>(new string[]{"amulet_name"});
          emeraldamulet.equipmentType = EquipmentType.Amulet;
          emeraldamulet.baseStats.accuracy = 50;
          emeraldamulet.materials = List.Of<string>(new string[]{"iron"});
          emeraldamulet.equipment_value = 400;
          AssetManager.items.list.AddItem(emeraldamulet);
          Localization.addLocalization("item_Emerald Amulet", "Emerald Amulet");

          ItemAsset topazamulet = AssetManager.items.clone("Topaz Amulet", "_accessory");
          topazamulet.id = "Topaz Amulet";
          topazamulet.name_templates = List.Of<string>(new string[]{"amulet_name"});
          topazamulet.equipmentType = EquipmentType.Amulet;
          topazamulet.baseStats.crit = 50f;
          topazamulet.materials = List.Of<string>(new string[]{"iron"});
          topazamulet.equipment_value = 400;
          AssetManager.items.list.AddItem(topazamulet);
          Localization.addLocalization("item_Topaz Amulet", "Topaz Amulet");

          ItemAsset amethystamulet = AssetManager.items.clone("Amethyst Amulet", "_accessory");
          amethystamulet.id = "Amethyst Amulet";
          amethystamulet.name_templates = List.Of<string>(new string[]{"amulet_name"});
          amethystamulet.equipmentType = EquipmentType.Amulet;
          amethystamulet.baseStats.targets = 2;
          amethystamulet.baseStats.areaOfEffect = 15f;
          amethystamulet.materials = List.Of<string>(new string[]{"iron"});
          amethystamulet.equipment_value = 400;
          AssetManager.items.list.AddItem(amethystamulet);
          Localization.addLocalization("item_Amethyst Amulet", "Amethyst Amulet");

          ItemAsset diamondamulet = AssetManager.items.clone("Diamond Amulet", "_accessory");
          diamondamulet.id = "Diamond Amulet";
          diamondamulet.name_templates = List.Of<string>(new string[]{"amulet_name"});
          diamondamulet.equipmentType = EquipmentType.Amulet;
          diamondamulet.baseStats.speed = 50f;
          diamondamulet.baseStats.attackSpeed = 50f;
          diamondamulet.materials = List.Of<string>(new string[]{"iron"});
          diamondamulet.equipment_value = 400;
          AssetManager.items.list.AddItem(diamondamulet);
          Localization.addLocalization("item_Diamond Amulet", "Diamond Amulet");

          ItemAsset sapphireamulet = AssetManager.items.clone("Sapphire Amulet", "_accessory");
          sapphireamulet.id = "Sapphire Amulet";
          sapphireamulet.name_templates = List.Of<string>(new string[]{"amulet_name"});
          sapphireamulet.equipmentType = EquipmentType.Amulet;
          sapphireamulet.baseStats.intelligence = 25;
          sapphireamulet.materials = List.Of<string>(new string[]{"iron"});
          sapphireamulet.equipment_value = 400;
          AssetManager.items.list.AddItem(sapphireamulet);
          Localization.addLocalization("item_Sapphire Amulet", "Sapphire Amulet");

          ItemAsset jadeamulet = AssetManager.items.clone("Jade Amulet", "_accessory");
          jadeamulet.id = "Jade Amulet";
          jadeamulet.name_templates = List.Of<string>(new string[]{"amulet_name"});
          jadeamulet.equipmentType = EquipmentType.Amulet;
          jadeamulet.baseStats.mod_health = 100f;
          jadeamulet.materials = List.Of<string>(new string[]{"iron"});
          jadeamulet.equipment_value = 400;
          AssetManager.items.list.AddItem(jadeamulet);
          Localization.addLocalization("item_Jade Amulet", "Jade Amulet");

          ItemAsset sunstoneamulet = AssetManager.items.clone("Sunstone Amulet", "_accessory");
          sunstoneamulet.id = "Sunstone Amulet";
          sunstoneamulet.name_templates = List.Of<string>(new string[]{"amulet_name"});
          sunstoneamulet.equipmentType = EquipmentType.Amulet;
          sunstoneamulet.baseStats.knockback = 10f;
          sunstoneamulet.materials = List.Of<string>(new string[]{"iron"});
          sunstoneamulet.equipment_value = 400;
          AssetManager.items.list.AddItem(sunstoneamulet);
          Localization.addLocalization("item_Sunstone Amulet", "Sunstone Amulet");

          ItemAsset morganiteamulet = AssetManager.items.clone("Morganite Amulet", "_accessory");
          morganiteamulet.id = "Morganite Amulet";
          morganiteamulet.name_templates = List.Of<string>(new string[]{"amulet_name"});
          morganiteamulet.equipmentType = EquipmentType.Amulet;
          morganiteamulet.baseStats.knockbackReduction = 10f;
          morganiteamulet.materials = List.Of<string>(new string[]{"iron"});
          morganiteamulet.equipment_value = 400;
          AssetManager.items.list.AddItem(morganiteamulet);
          Localization.addLocalization("item_Morganite Amulet", "Morganite Amulet");

          ItemAsset turquoiseamulet = AssetManager.items.clone("Turquoise Amulet", "_accessory");
          turquoiseamulet.id = "Turquoise Amulet";
          turquoiseamulet.name_templates = List.Of<string>(new string[]{"amulet_name"});
          turquoiseamulet.equipmentType = EquipmentType.Amulet;
          turquoiseamulet.baseStats.dodge = 10f;
          turquoiseamulet.materials = List.Of<string>(new string[]{"iron"});
          turquoiseamulet.equipment_value = 400;
          AssetManager.items.list.AddItem(turquoiseamulet);
          Localization.addLocalization("item_Turquoise Amulet", "Turquoise Amulet");

          ItemAsset citrineamulet = AssetManager.items.clone("Citrine Amulet", "_accessory");
          citrineamulet.id = "Citrine Amulet";
          citrineamulet.name_templates = List.Of<string>(new string[]{"amulet_name"});
          citrineamulet.equipmentType = EquipmentType.Amulet;
          citrineamulet.baseStats.diplomacy = 25;
          citrineamulet.materials = List.Of<string>(new string[]{"iron"});
          citrineamulet.equipment_value = 400;
          AssetManager.items.list.AddItem(citrineamulet);
          Localization.addLocalization("item_Citrine Amulet", "Citrine Amulet");

          ItemAsset zirconamulet = AssetManager.items.clone("Zircon Amulet", "_accessory");
          zirconamulet.id = "Zircon Amulet";
          zirconamulet.name_templates = List.Of<string>(new string[]{"amulet_name"});
          zirconamulet.equipmentType = EquipmentType.Amulet;
          zirconamulet.baseStats.warfare = 25;
          zirconamulet.materials = List.Of<string>(new string[]{"iron"});
          zirconamulet.equipment_value = 400;
          AssetManager.items.list.AddItem(zirconamulet);
          Localization.addLocalization("item_Zircon Amulet", "Zircon Amulet");

          ItemAsset ioliteamulet = AssetManager.items.clone("Iolite Amulet", "_accessory");
          ioliteamulet.id = "Iolite Amulet";
          ioliteamulet.name_templates = List.Of<string>(new string[]{"amulet_name"});
          ioliteamulet.equipmentType = EquipmentType.Amulet;
          ioliteamulet.baseStats.stewardship = 25;
          ioliteamulet.materials = List.Of<string>(new string[]{"iron"});
          ioliteamulet.equipment_value = 400;
          AssetManager.items.list.AddItem(ioliteamulet);
          Localization.addLocalization("item_Iolite Amulet", "Iolite Amulet");

          ItemAsset peridotamulet = AssetManager.items.clone("Peridot Amulet", "_accessory");
          peridotamulet.id = "Peridot Amulet";
          peridotamulet.name_templates = List.Of<string>(new string[]{"amulet_name"});
          peridotamulet.equipmentType = EquipmentType.Amulet;
          peridotamulet.baseStats.projectiles = 2;
          peridotamulet.materials = List.Of<string>(new string[]{"iron"});
          peridotamulet.equipment_value = 400;
          AssetManager.items.list.AddItem(peridotamulet);
          Localization.addLocalization("item_Peridot Amulet", "Peridot Amulet");

          ItemAsset rubyartifact = AssetManager.items.clone("Ruby Artifact", "_accessory");
          rubyartifact.id = "Ruby Artifact";
          rubyartifact.name_templates = List.Of<string>(new string[]{"amulet_name"});
          rubyartifact.equipmentType = EquipmentType.Amulet;
          rubyartifact.materials = List.Of<string>(new string[]{"adamantine"});
          rubyartifact.equipment_value = 500;
          AssetManager.items.list.AddItem(rubyartifact);
          Localization.addLocalization("item_Ruby Artifact", "Ruby Artifact");

        } //BORING SPRITE STUFF
          public static void addItemSprite(string id, string material)
    	{
          var dictItems = Reflection.GetField(typeof(ActorAnimationLoader), null, "dictItems") as Dictionary<string, Sprite>;
          var sprite = Resources.Load<Sprite>("actors/races/items/w_" + id + "_" + material);
          dictItems.Add(sprite.name, sprite);
        }
          public static void addStaffSprite(string id, string material)
        {
          var dictItems = Reflection.GetField(typeof(ActorAnimationLoader), null, "dictItems") as Dictionary<string, Sprite>;
          var sprite = Resources.Load<Sprite>("actors/races/items/Staff/w_" + id + "_" + material);
          dictItems.Add(sprite.name, sprite);
        }
        //ITEM ABILS
        public static bool ColdStaffAttack(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          ActionLibrary.coldAuraEffect(a, pTile);
          ActionLibrary.addFrozenEffectOnTarget(a, pTile);
          }
      		return true;
        
        }        
        public static bool FlameStaffAttack(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          ActionLibrary.deathBomb(a, pTile);
          ActionLibrary.addBurningEffectOnTarget(a, pTile);
          }
      		return true;
        
        }        
        public static bool AirbindStaffAttack(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          if(Toolbox.randomChance(0.25f)){
             ActionLibrary.castLightning(a, pTile);
             if(Toolbox.randomChance(0.05f)){
              ActionLibrary.castTornado(a, pTile);
             }
          }
          }
      		return true;
        }        
    }
}
