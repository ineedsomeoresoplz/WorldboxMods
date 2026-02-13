// Decompiled with JetBrains decompiler
// Type: ItemTools
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class ItemTools
{
  public static int s_value;
  public static Rarity s_quality;

  public static void calcItemValues(Item pItem, BaseStats pStats)
  {
    pStats.clear();
    ItemTools.s_value = 0;
    ItemTools.s_quality = Rarity.R0_Normal;
    pItem.action_attack_target = (AttackAction) null;
    ItemTools.mergeStatsWithItem(pStats, pItem);
  }

  public static void mergeStatsWithItem(
    BaseStats pStats,
    Item pItem,
    bool pCalcGlobalValue = true,
    float pMultiplier = 1f)
  {
    // ISSUE: unable to decompile the method.
  }

  private static void addAction(Item pItem, AttackAction pAction)
  {
    if (pAction == null)
      return;
    if (pItem.action_attack_target == null)
      pItem.action_attack_target = pAction;
    else
      pItem.action_attack_target += pAction;
  }

  public static void mergeStats(
    BaseStats pStats,
    ItemAsset pAsset,
    bool pCalcGlobalValue = true,
    float pMultiplier = 1f)
  {
    if (pAsset == null)
      return;
    pStats.mergeStats(pAsset.base_stats, pMultiplier);
    if (!pCalcGlobalValue)
      return;
    if (pAsset.quality > ItemTools.s_quality)
      ItemTools.s_quality = pAsset.quality;
    ItemTools.s_value += pAsset.equipment_value + pAsset.mod_rank * 2;
  }

  public static void getTooltipTitle(EquipmentAsset pAsset, out string pName, out string pMaterial)
  {
    string str1 = pAsset.getLocaleID().Localize();
    string str2 = "";
    if (!string.IsNullOrEmpty(pAsset.material) && pAsset.material != "basic")
      str2 = $"{str2}({LocalizedTextManager.getText(pAsset.getMaterialID())}) ";
    pName = str1;
    pMaterial = str2;
  }

  public static void getAssetIds(
    string pItemIdWithMaterial,
    out string pItemAssetId,
    out string pMaterialAssetId)
  {
    pItemAssetId = (string) null;
    pMaterialAssetId = (string) null;
    int length = pItemIdWithMaterial.LastIndexOf('_');
    if (length == -1)
      return;
    pItemAssetId = pItemIdWithMaterial.Substring(0, length);
    pMaterialAssetId = pItemIdWithMaterial.Substring(length + 1);
  }
}
