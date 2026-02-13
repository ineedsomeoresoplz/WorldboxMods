// Decompiled with JetBrains decompiler
// Type: AssetsDebugManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class AssetsDebugManager
{
  public static ActorSex actors_sex;

  public static void changeSex()
  {
    if (AssetsDebugManager.actors_sex == ActorSex.Female)
      AssetsDebugManager.actors_sex = ActorSex.Male;
    else
      AssetsDebugManager.actors_sex = ActorSex.Female;
  }

  public static void newKingdomColors()
  {
    foreach (KingdomAsset kingdomAsset in AssetManager.kingdoms.list)
      kingdomAsset.debug_color_asset = AssetManager.kingdom_colors_library.list.GetRandom<ColorAsset>();
  }

  public static void setRandomKingdomColor(string pKingdomAssetId)
  {
    AssetManager.kingdoms.get(pKingdomAssetId).debug_color_asset = AssetManager.kingdom_colors_library.list.GetRandom<ColorAsset>();
  }

  public static void newSkinColors()
  {
    foreach (ActorAsset pAsset in AssetManager.actor_library.list)
    {
      if (pAsset.use_phenotypes)
        AssetsDebugManager.setRandomSkinColor(pAsset);
    }
  }

  public static void setRandomSkinColor(ActorAsset pAsset)
  {
    string randomSkinColor = AssetsDebugManager.getRandomSkinColor(pAsset);
    pAsset.debug_phenotype_colors = randomSkinColor;
  }

  private static string getRandomSkinColor(ActorAsset pAsset)
  {
    return pAsset.phenotypes_list == null || pAsset.phenotypes_list.Count == 0 ? (string) null : pAsset.phenotypes_list.GetRandom<string>();
  }
}
