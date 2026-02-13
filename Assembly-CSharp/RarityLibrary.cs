// Decompiled with JetBrains decompiler
// Type: RarityLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class RarityLibrary : AssetLibrary<RarityAsset>
{
  public static RarityAsset normal;
  public static RarityAsset rare;
  public static RarityAsset epic;
  public static RarityAsset legendary;

  public override void init()
  {
    base.init();
    RarityAsset pAsset1 = new RarityAsset();
    pAsset1.id = Rarity.R0_Normal.ToString().ToLower();
    pAsset1.color_hex = "#FFFFFF";
    pAsset1.material_path = string.Empty;
    pAsset1.rarity_trait_string = "trait_common";
    RarityLibrary.normal = this.add(pAsset1);
    RarityAsset pAsset2 = new RarityAsset();
    pAsset2.id = Rarity.R1_Rare.ToString().ToLower();
    pAsset2.color_hex = "#66AFFF";
    pAsset2.material_path = "materials/ItemRare";
    pAsset2.rarity_trait_string = "trait_rare";
    RarityLibrary.rare = this.add(pAsset2);
    RarityAsset pAsset3 = new RarityAsset();
    pAsset3.id = Rarity.R2_Epic.ToString().ToLower();
    pAsset3.color_hex = "#FFF15E";
    pAsset3.material_path = "materials/ItemEpic";
    pAsset3.rarity_trait_string = "trait_epic";
    RarityLibrary.epic = this.add(pAsset3);
    RarityAsset pAsset4 = new RarityAsset();
    pAsset4.id = Rarity.R3_Legendary.ToString().ToLower();
    pAsset4.color_hex = "#FF7028";
    pAsset4.material_path = "materials/ItemLegendary";
    pAsset4.rarity_trait_string = "trait_legendary";
    RarityLibrary.legendary = this.add(pAsset4);
  }

  public override void linkAssets()
  {
    base.linkAssets();
    foreach (RarityAsset rarityAsset in this.list)
      rarityAsset.color_container = new ContainerItemColor(rarityAsset.color_hex, rarityAsset.material_path);
  }

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (RarityAsset pAsset in this.list)
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
  }
}
