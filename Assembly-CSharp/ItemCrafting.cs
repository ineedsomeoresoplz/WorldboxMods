// Decompiled with JetBrains decompiler
// Type: ItemCrafting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public static class ItemCrafting
{
  private static readonly EquipmentType[] list_equipments = new EquipmentType[5]
  {
    EquipmentType.Helmet,
    EquipmentType.Armor,
    EquipmentType.Boots,
    EquipmentType.Amulet,
    EquipmentType.Ring
  };

  public static bool tryToCraftRandomWeapon(Actor pActor, City pCity)
  {
    int itemMakingSkill = pActor.asset.item_making_skill;
    if (pActor.hasCulture() && pActor.culture.hasTrait("weaponsmith_mastery"))
      itemMakingSkill += CultureTraitLibrary.getValue("weaponsmith_mastery");
    return ItemCrafting.craftItem(pActor, pActor.getName(), EquipmentType.Weapon, itemMakingSkill, pCity);
  }

  public static bool tryToCraftRandomArmor(Actor pActor, City pCity)
  {
    int itemMakingSkill = pActor.asset.item_making_skill;
    if (pActor.hasCulture() && pActor.culture.hasTrait("armorsmith_mastery"))
      itemMakingSkill += CultureTraitLibrary.getValue("armorsmith_mastery");
    EquipmentType random = ItemCrafting.list_equipments.GetRandom<EquipmentType>();
    return ItemCrafting.craftItem(pActor, pActor.getName(), random, itemMakingSkill, pCity);
  }

  public static bool tryToCraftRandomEquipment(Actor pActor, City pCity)
  {
    return ItemCrafting.tryToCraftRandomArmor(pActor, pCity) | ItemCrafting.tryToCraftRandomWeapon(pActor, pCity);
  }

  public static bool craftItem(
    Actor pActor,
    string pCreatorName,
    EquipmentType pType,
    int pTries,
    City pCity)
  {
    string key = (string) null;
    if (pType == EquipmentType.Weapon)
    {
      if (pActor.hasCulture())
        key = pActor.culture.getPreferredWeaponSubtypeIDs();
      if (string.IsNullOrEmpty(key))
        key = ItemLibrary.default_weapon_pool.GetRandom<string>();
    }
    else
      key = AssetManager.items.getEquipmentType(pType);
    EquipmentAsset pItemAsset = (EquipmentAsset) null;
    ActorEquipmentSlot slot = pActor.equipment.getSlot(pType);
    Item obj = slot.getItem();
    if (obj != null && obj.isCursed())
      return false;
    int equipmentValue = obj != null ? obj.asset.equipment_value : 0;
    if (pType == EquipmentType.Weapon && pActor.hasCulture() && pActor.culture.hasPreferredWeaponsToCraft() && Randy.randomBool())
      pItemAsset = ItemCrafting.getItemAssetToCraft(pActor, pActor.culture.getPreferredWeaponAssets(), pCity, equipmentValue, true);
    if (pItemAsset == null)
    {
      List<EquipmentAsset> equipmentBySubtype = AssetManager.items.equipment_by_subtypes[key];
      pItemAsset = ItemCrafting.getItemAssetToCraft(pActor, equipmentBySubtype, pCity, equipmentValue);
    }
    if (pItemAsset == null)
      return false;
    Item pItem1 = World.world.items.generateItem(pItemAsset, pActor.kingdom, pCreatorName, pTries, pActor);
    if (slot.isEmpty())
    {
      slot.setItem(pItem1, pActor);
    }
    else
    {
      Item pItem2 = slot.getItem();
      slot.takeAwayItem();
      pCity.tryToPutItem(pItem2);
      slot.setItem(pItem1, pActor);
    }
    pActor.spendMoney(pItemAsset.get_total_cost);
    if (pItemAsset.cost_resource_id_1 != "none")
      pCity.takeResource(pItemAsset.cost_resource_id_1, pItemAsset.cost_resource_1);
    if (pItemAsset.cost_resource_id_2 != "none")
      pCity.takeResource(pItemAsset.cost_resource_id_2, pItemAsset.cost_resource_2);
    return true;
  }

  public static EquipmentAsset getItemAssetToCraft(
    Actor pActor,
    List<EquipmentAsset> pItemList,
    City pCity,
    int pCurrentItemValue,
    bool pShuffle = false)
  {
    if (pShuffle)
      pItemList.Shuffle<EquipmentAsset>();
    for (int index = pItemList.Count - 1; index >= 0; --index)
    {
      EquipmentAsset pItem = pItemList[index];
      if (pItem.equipment_value > pCurrentItemValue && ItemCrafting.hasEnoughResourcesToCraft(pActor, pItem, pCity))
        return pItem;
    }
    return (EquipmentAsset) null;
  }

  private static bool hasEnoughResourcesToCraft(Actor pActor, EquipmentAsset pAsset, City pCity)
  {
    int getTotalCost = pAsset.get_total_cost;
    return pActor.hasEnoughMoney(getTotalCost) && (!(pAsset.cost_resource_id_1 != "none") || pAsset.cost_resource_1 <= pCity.getResourcesAmount(pAsset.cost_resource_id_1)) && (!(pAsset.cost_resource_id_2 != "none") || pAsset.cost_resource_2 <= pCity.getResourcesAmount(pAsset.cost_resource_id_2));
  }
}
