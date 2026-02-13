// Decompiled with JetBrains decompiler
// Type: PhenotypeLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PhenotypeLibrary : AssetLibrary<PhenotypeAsset>
{
  private Dictionary<int, PhenotypeAsset> _phenotypes_assets_by_index = new Dictionary<int, PhenotypeAsset>();
  public const int PHENOTYPE_SHADES = 4;
  public const int PHENOTYPE_NONE = 0;
  public static PhenotypeAsset default_asset;
  public static PhenotypeAsset default_green;

  public override void init()
  {
    base.init();
    PhenotypeAsset pAsset1 = new PhenotypeAsset();
    pAsset1.id = "skin_pale";
    pAsset1.shades_from = "#FFF9F4";
    pAsset1.shades_to = "#ECD9D5";
    PhenotypeLibrary.default_asset = this.add(pAsset1);
    PhenotypeAsset pAsset2 = new PhenotypeAsset();
    pAsset2.id = "skin_light";
    pAsset2.shades_from = "#F5D6BA";
    pAsset2.shades_to = "#F3C6BE";
    this.add(pAsset2);
    PhenotypeAsset pAsset3 = new PhenotypeAsset();
    pAsset3.id = "skin_medium";
    pAsset3.shades_from = "#C5966D";
    pAsset3.shades_to = "#C06859";
    this.add(pAsset3);
    PhenotypeAsset pAsset4 = new PhenotypeAsset();
    pAsset4.id = "skin_dark";
    pAsset4.shades_from = "#7C5A3C";
    pAsset4.shades_to = "#593731";
    this.add(pAsset4);
    PhenotypeAsset pAsset5 = new PhenotypeAsset();
    pAsset5.id = "skin_black";
    pAsset5.shades_from = "#46372A";
    pAsset5.shades_to = "#34211E";
    this.add(pAsset5);
    PhenotypeAsset pAsset6 = new PhenotypeAsset();
    pAsset6.id = "skin_mixed";
    pAsset6.shades_from = "#E9BA90";
    pAsset6.shades_to = "#34211E";
    this.add(pAsset6);
    PhenotypeAsset pAsset7 = new PhenotypeAsset();
    pAsset7.id = "skin_red";
    pAsset7.shades_from = "#DB4D48";
    pAsset7.shades_to = "#7F2E39";
    this.add(pAsset7);
    PhenotypeAsset pAsset8 = new PhenotypeAsset();
    pAsset8.id = "skin_yellow";
    pAsset8.shades_from = "#FFE18E";
    pAsset8.shades_to = "#AB8B51";
    this.add(pAsset8);
    PhenotypeAsset pAsset9 = new PhenotypeAsset();
    pAsset9.id = "skin_green";
    pAsset9.shades_from = "#9DB361";
    pAsset9.shades_to = "#425338";
    this.add(pAsset9);
    PhenotypeAsset pAsset10 = new PhenotypeAsset();
    pAsset10.id = "skin_blue";
    pAsset10.shades_from = "#7FC1C7";
    pAsset10.shades_to = "#37617A";
    this.add(pAsset10);
    PhenotypeAsset pAsset11 = new PhenotypeAsset();
    pAsset11.id = "skin_purple";
    pAsset11.shades_from = "#C29FD5";
    pAsset11.shades_to = "#5C54A2";
    this.add(pAsset11);
    PhenotypeAsset pAsset12 = new PhenotypeAsset();
    pAsset12.id = "skin_pink";
    pAsset12.shades_from = "#FFAFC0";
    pAsset12.shades_to = "#D1539F";
    this.add(pAsset12);
    PhenotypeAsset pAsset13 = new PhenotypeAsset();
    pAsset13.id = "white_gray";
    pAsset13.shades_from = "#FAFAFA";
    pAsset13.shades_to = "#B0B0B0";
    this.add(pAsset13);
    PhenotypeAsset pAsset14 = new PhenotypeAsset();
    pAsset14.id = "mid_gray";
    pAsset14.shades_from = "#BBBBBB";
    pAsset14.shades_to = "#606060";
    this.add(pAsset14);
    PhenotypeAsset pAsset15 = new PhenotypeAsset();
    pAsset15.id = "gray_black";
    pAsset15.shades_from = "#7A7A7A";
    pAsset15.shades_to = "#3E3E3E";
    this.add(pAsset15);
    PhenotypeAsset pAsset16 = new PhenotypeAsset();
    pAsset16.id = "bright_red";
    pAsset16.shades_from = "#FF9A9A";
    pAsset16.shades_to = "#FF2222";
    this.add(pAsset16);
    PhenotypeAsset pAsset17 = new PhenotypeAsset();
    pAsset17.id = "bright_orange";
    pAsset17.shades_from = "#FFC191";
    pAsset17.shades_to = "#FF7316";
    this.add(pAsset17);
    PhenotypeAsset pAsset18 = new PhenotypeAsset();
    pAsset18.id = "bright_yellow";
    pAsset18.shades_from = "#FFEBA3";
    pAsset18.shades_to = "#FFCE1F";
    this.add(pAsset18);
    PhenotypeAsset pAsset19 = new PhenotypeAsset();
    pAsset19.id = "bright_green";
    pAsset19.shades_from = "#C5FFA0";
    pAsset19.shades_to = "#1CDA2C";
    this.add(pAsset19);
    PhenotypeAsset pAsset20 = new PhenotypeAsset();
    pAsset20.id = "bright_teal";
    pAsset20.shades_from = "#A3FFE0";
    pAsset20.shades_to = "#0DDCA4";
    this.add(pAsset20);
    PhenotypeAsset pAsset21 = new PhenotypeAsset();
    pAsset21.id = "bright_blue";
    pAsset21.shades_from = "#A8E3FF";
    pAsset21.shades_to = "#1CA4FF";
    this.add(pAsset21);
    PhenotypeAsset pAsset22 = new PhenotypeAsset();
    pAsset22.id = "bright_violet";
    pAsset22.shades_from = "#D6B7FF";
    pAsset22.shades_to = "#7142F3";
    this.add(pAsset22);
    PhenotypeAsset pAsset23 = new PhenotypeAsset();
    pAsset23.id = "bright_purple";
    pAsset23.shades_from = "#F3B9FF";
    pAsset23.shades_to = "#BE4FDD";
    this.add(pAsset23);
    PhenotypeAsset pAsset24 = new PhenotypeAsset();
    pAsset24.id = "bright_pink";
    pAsset24.shades_from = "#FFB6E3";
    pAsset24.shades_to = "#FF5FC2";
    this.add(pAsset24);
    PhenotypeAsset pAsset25 = new PhenotypeAsset();
    pAsset25.id = "bright_salmon";
    pAsset25.shades_from = "#FFAFBC";
    pAsset25.shades_to = "#FF537E";
    this.add(pAsset25);
    PhenotypeAsset pAsset26 = new PhenotypeAsset();
    pAsset26.id = "dark_red";
    pAsset26.shades_from = "#EC2D2D";
    pAsset26.shades_to = "#5F1414";
    this.add(pAsset26);
    PhenotypeAsset pAsset27 = new PhenotypeAsset();
    pAsset27.id = "dark_orange";
    pAsset27.shades_from = "#FF7B16";
    pAsset27.shades_to = "#69340B";
    this.add(pAsset27);
    PhenotypeAsset pAsset28 = new PhenotypeAsset();
    pAsset28.id = "dark_yellow";
    pAsset28.shades_from = "#FFC61A";
    pAsset28.shades_to = "#986411";
    this.add(pAsset28);
    PhenotypeAsset pAsset29 = new PhenotypeAsset();
    pAsset29.id = "dark_green";
    pAsset29.shades_from = "#72C727";
    pAsset29.shades_to = "#2A6016";
    PhenotypeLibrary.default_green = this.add(pAsset29);
    PhenotypeAsset pAsset30 = new PhenotypeAsset();
    pAsset30.id = "dark_teal";
    pAsset30.shades_from = "#1FDEA5";
    pAsset30.shades_to = "#0F6557";
    this.add(pAsset30);
    PhenotypeAsset pAsset31 = new PhenotypeAsset();
    pAsset31.id = "dark_blue";
    pAsset31.shades_from = "#2182EB";
    pAsset31.shades_to = "#163487";
    this.add(pAsset31);
    PhenotypeAsset pAsset32 = new PhenotypeAsset();
    pAsset32.id = "dark_violet";
    pAsset32.shades_from = "#6F47DF";
    pAsset32.shades_to = "#321A75";
    this.add(pAsset32);
    PhenotypeAsset pAsset33 = new PhenotypeAsset();
    pAsset33.id = "dark_purple";
    pAsset33.shades_from = "#A341E1";
    pAsset33.shades_to = "#491C5F";
    this.add(pAsset33);
    PhenotypeAsset pAsset34 = new PhenotypeAsset();
    pAsset34.id = "dark_pink";
    pAsset34.shades_from = "#F84EE7";
    pAsset34.shades_to = "#652265";
    this.add(pAsset34);
    PhenotypeAsset pAsset35 = new PhenotypeAsset();
    pAsset35.id = "dark_salmon";
    pAsset35.shades_from = "#F35688";
    pAsset35.shades_to = "#6F2949";
    this.add(pAsset35);
    PhenotypeAsset pAsset36 = new PhenotypeAsset();
    pAsset36.id = "toxic_green";
    pAsset36.shades_from = "#CBFF2E";
    pAsset36.shades_to = "#10F023";
    this.add(pAsset36);
    PhenotypeAsset pAsset37 = new PhenotypeAsset();
    pAsset37.id = "aqua";
    pAsset37.shades_from = "#77D8D0";
    pAsset37.shades_to = "#3779A5";
    this.add(pAsset37);
    PhenotypeAsset pAsset38 = new PhenotypeAsset();
    pAsset38.id = "swamp";
    pAsset38.shades_from = "#784D85";
    pAsset38.shades_to = "#367F58";
    this.add(pAsset38);
    PhenotypeAsset pAsset39 = new PhenotypeAsset();
    pAsset39.id = "jungle";
    pAsset39.shades_from = "#03803D";
    pAsset39.shades_to = "#2EB829";
    this.add(pAsset39);
    PhenotypeAsset pAsset40 = new PhenotypeAsset();
    pAsset40.id = "polar";
    pAsset40.shades_from = "#E8EEF5";
    pAsset40.shades_to = "#95C2E7";
    this.add(pAsset40);
    PhenotypeAsset pAsset41 = new PhenotypeAsset();
    pAsset41.id = "savanna";
    pAsset41.shades_from = "#FFB52B";
    pAsset41.shades_to = "#D06643";
    this.add(pAsset41);
    PhenotypeAsset pAsset42 = new PhenotypeAsset();
    pAsset42.id = "corrupted";
    pAsset42.shades_from = "#7E7987";
    pAsset42.shades_to = "#483162";
    this.add(pAsset42);
    PhenotypeAsset pAsset43 = new PhenotypeAsset();
    pAsset43.id = "infernal";
    pAsset43.shades_from = "#FF5100";
    pAsset43.shades_to = "#FF002B";
    this.add(pAsset43);
    PhenotypeAsset pAsset44 = new PhenotypeAsset();
    pAsset44.id = "lemon";
    pAsset44.shades_from = "#FFDB27";
    pAsset44.shades_to = "#BDF741";
    this.add(pAsset44);
    PhenotypeAsset pAsset45 = new PhenotypeAsset();
    pAsset45.id = "desert";
    pAsset45.shades_from = "#D3C0AB";
    pAsset45.shades_to = "#B68637";
    this.add(pAsset45);
    PhenotypeAsset pAsset46 = new PhenotypeAsset();
    pAsset46.id = "crystal";
    pAsset46.shades_from = "#1CFFCE";
    pAsset46.shades_to = "#15A1FF";
    this.add(pAsset46);
    PhenotypeAsset pAsset47 = new PhenotypeAsset();
    pAsset47.id = "candy";
    pAsset47.shades_from = "#FF658C";
    pAsset47.shades_to = "#D166FF";
    this.add(pAsset47);
    PhenotypeAsset pAsset48 = new PhenotypeAsset();
    pAsset48.id = "soil";
    pAsset48.shades_from = "#533028";
    pAsset48.shades_to = "#160C15";
    this.add(pAsset48);
    PhenotypeAsset pAsset49 = new PhenotypeAsset();
    pAsset49.id = "wood";
    pAsset49.shades_from = "#9D582A";
    pAsset49.shades_to = "#4C2828";
    this.add(pAsset49);
    PhenotypeAsset pAsset50 = new PhenotypeAsset();
    pAsset50.id = "pink_yellow_mushroom";
    pAsset50.shades_from = "#F12C78";
    pAsset50.shades_to = "#FFAC26";
    this.add(pAsset50);
    PhenotypeAsset pAsset51 = new PhenotypeAsset();
    pAsset51.id = "black_blue";
    pAsset51.shades_from = "#34345B";
    pAsset51.shades_to = "#222229";
    this.add(pAsset51);
    PhenotypeAsset pAsset52 = new PhenotypeAsset();
    pAsset52.id = "magical";
    pAsset52.shades_from = "#24C695";
    pAsset52.shades_to = "#9D2EA7";
    this.add(pAsset52);
  }

  public override void linkAssets()
  {
    base.linkAssets();
    int num = 1;
    foreach (PhenotypeAsset pAsset in this.list)
    {
      this.createShades(pAsset);
      pAsset.phenotype_index = num++;
      this._phenotypes_assets_by_index.Add(pAsset.phenotype_index, pAsset);
    }
  }

  public PhenotypeAsset getAssetByPhenotypeIndex(int pIndex)
  {
    PhenotypeAsset defaultAsset;
    this._phenotypes_assets_by_index.TryGetValue(pIndex, out defaultAsset);
    if (defaultAsset == null)
      defaultAsset = PhenotypeLibrary.default_asset;
    return defaultAsset;
  }

  public void createShades(PhenotypeAsset pAsset)
  {
    Color pFrom = Toolbox.makeColor(pAsset.shades_from);
    Color pTo = Toolbox.makeColor(pAsset.shades_to);
    float num = 0.333333343f;
    for (int index = 0; index < 4; ++index)
    {
      float amount = (float) (1.0 - (double) index * (double) num);
      if ((double) amount > 1.0)
        amount = 1f;
      Color color = Toolbox.blendColor(pFrom, pTo, amount);
      pAsset.colors[index] = Color32.op_Implicit(color);
    }
  }
}
