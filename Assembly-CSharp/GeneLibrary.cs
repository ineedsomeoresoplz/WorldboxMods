// Decompiled with JetBrains decompiler
// Type: GeneLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class GeneLibrary : BaseTraitLibrary<GeneAsset>
{
  [NonSerialized]
  private Dictionary<string, List<GeneAsset>> _cached_stat_genes_dictionary = new Dictionary<string, List<GeneAsset>>();
  [NonSerialized]
  private List<GeneAsset> _gene_assets_simple = new List<GeneAsset>();
  [NonSerialized]
  private List<GeneAsset> _gene_assets_mutations = new List<GeneAsset>();
  public static GeneAsset gene_for_generation;

  protected override List<string> getDefaultTraitsForMeta(ActorAsset pAsset) => (List<string>) null;

  public override void init()
  {
    base.init();
    this.addSpecial();
    this.addBaseStats();
    this.addFightStats();
    this.addBonusStats();
    this.addAttributes();
    GeneLibrary.gene_for_generation = this.get("temp_for_generation");
  }

  public string getRandomNormalGene()
  {
    int num = 10;
    foreach (GeneAsset geneAsset in this.list.LoopRandom<GeneAsset>())
    {
      if (num > 0)
      {
        --num;
        if (geneAsset.isAvailable() && !geneAsset.is_empty && !geneAsset.for_generation)
          return geneAsset.id;
      }
      else
        break;
    }
    return (string) null;
  }

  private void addBonusStats()
  {
    GeneAsset pAsset1 = new GeneAsset();
    pAsset1.id = "attack_speed";
    this.add(pAsset1);
    this.t.base_stats["attack_speed"] = 1f;
    this.t.setUnlockedWithAchievement("achievementGenesExplorer");
    GeneAsset pAsset2 = new GeneAsset();
    pAsset2.id = "scale_plus";
    this.add(pAsset2);
    this.t.base_stats["scale"] = 0.03f;
    this.t.setUnlockedWithAchievement("achievementSimpleStupidGenetics");
    GeneAsset pAsset3 = new GeneAsset();
    pAsset3.id = "scale_minus";
    this.add(pAsset3);
    this.t.base_stats["scale"] = -0.01f;
    this.t.setUnlockedWithAchievement("achievementAntWorld");
  }

  private void addFightStats()
  {
    GeneAsset pAsset1 = new GeneAsset();
    pAsset1.id = "armor_1";
    pAsset1.is_simple = true;
    this.add(pAsset1);
    this.t.base_stats["armor"] = 1f;
    GeneAsset pAsset2 = new GeneAsset();
    pAsset2.id = "armor_2";
    this.add(pAsset2);
    this.t.base_stats["armor"] = 6f;
    GeneAsset pAsset3 = new GeneAsset();
    pAsset3.id = "armor_3";
    this.add(pAsset3);
    this.t.base_stats["armor"] = 10f;
    GeneAsset pAsset4 = new GeneAsset();
    pAsset4.id = "damage_1";
    pAsset4.is_simple = true;
    this.add(pAsset4);
    this.t.base_stats["damage"] = 1f;
    GeneAsset pAsset5 = new GeneAsset();
    pAsset5.id = "damage_2";
    this.add(pAsset5);
    this.t.base_stats["damage"] = 6f;
    GeneAsset pAsset6 = new GeneAsset();
    pAsset6.id = "damage_3";
    this.add(pAsset6);
    this.t.base_stats["damage"] = 10f;
  }

  private void addBaseStats()
  {
    GeneAsset pAsset1 = new GeneAsset();
    pAsset1.id = "birth_rate_1";
    this.add(pAsset1);
    this.t.base_stats["birth_rate"] = 1f;
    GeneAsset pAsset2 = new GeneAsset();
    pAsset2.id = "offspring_1";
    this.add(pAsset2);
    this.t.base_stats["offspring"] = 1f;
    GeneAsset pAsset3 = new GeneAsset();
    pAsset3.id = "offspring_2";
    this.add(pAsset3);
    this.t.base_stats["offspring"] = 3f;
    GeneAsset pAsset4 = new GeneAsset();
    pAsset4.id = "offspring_3";
    this.add(pAsset4);
    this.t.base_stats["offspring"] = 5f;
    GeneAsset pAsset5 = new GeneAsset();
    pAsset5.id = "offspring_4";
    this.add(pAsset5);
    this.t.base_stats["offspring"] = 10f;
    GeneAsset pAsset6 = new GeneAsset();
    pAsset6.id = "lifespan_1";
    this.add(pAsset6);
    this.t.base_stats["lifespan"] = 5f;
    GeneAsset pAsset7 = new GeneAsset();
    pAsset7.id = "lifespan_2";
    this.add(pAsset7);
    this.t.base_stats["lifespan"] = 20f;
    GeneAsset pAsset8 = new GeneAsset();
    pAsset8.id = "lifespan_3";
    this.add(pAsset8);
    this.t.base_stats["lifespan"] = 50f;
    GeneAsset pAsset9 = new GeneAsset();
    pAsset9.id = "lifespan_4";
    this.add(pAsset9);
    this.t.base_stats["lifespan"] = 100f;
    GeneAsset pAsset10 = new GeneAsset();
    pAsset10.id = "health_1";
    pAsset10.is_simple = true;
    this.add(pAsset10);
    this.t.base_stats["health"] = 1f;
    GeneAsset pAsset11 = new GeneAsset();
    pAsset11.id = "health_2";
    pAsset11.is_simple = true;
    this.add(pAsset11);
    this.t.base_stats["health"] = 10f;
    GeneAsset pAsset12 = new GeneAsset();
    pAsset12.id = "health_3";
    this.add(pAsset12);
    this.t.base_stats["health"] = 50f;
    GeneAsset pAsset13 = new GeneAsset();
    pAsset13.id = "health_4";
    this.add(pAsset13);
    this.t.base_stats["health"] = 100f;
    GeneAsset pAsset14 = new GeneAsset();
    pAsset14.id = "health_5";
    this.add(pAsset14);
    this.t.base_stats["health"] = 300f;
    GeneAsset pAsset15 = new GeneAsset();
    pAsset15.id = "stamina_1";
    pAsset15.is_simple = true;
    this.add(pAsset15);
    this.t.base_stats["stamina"] = 10f;
    GeneAsset pAsset16 = new GeneAsset();
    pAsset16.id = "stamina_2";
    this.add(pAsset16);
    this.t.base_stats["stamina"] = 50f;
    GeneAsset pAsset17 = new GeneAsset();
    pAsset17.id = "stamina_3";
    this.add(pAsset17);
    this.t.base_stats["stamina"] = 100f;
    GeneAsset pAsset18 = new GeneAsset();
    pAsset18.id = "speed_1";
    this.add(pAsset18);
    this.t.base_stats["speed"] = 1f;
    GeneAsset pAsset19 = new GeneAsset();
    pAsset19.id = "speed_2";
    this.add(pAsset19);
    this.t.base_stats["speed"] = 2f;
    GeneAsset pAsset20 = new GeneAsset();
    pAsset20.id = "speed_3";
    this.add(pAsset20);
    this.t.base_stats["speed"] = 5f;
  }

  private void addAttributes()
  {
    GeneAsset pAsset1 = new GeneAsset();
    pAsset1.id = "diplomacy_1";
    pAsset1.is_simple = true;
    this.add(pAsset1);
    this.t.base_stats["diplomacy"] = 1f;
    GeneAsset pAsset2 = new GeneAsset();
    pAsset2.id = "diplomacy_2";
    this.add(pAsset2);
    this.t.base_stats["diplomacy"] = 2f;
    GeneAsset pAsset3 = new GeneAsset();
    pAsset3.id = "diplomacy_3";
    this.add(pAsset3);
    this.t.base_stats["diplomacy"] = 3f;
    GeneAsset pAsset4 = new GeneAsset();
    pAsset4.id = "warfare_1";
    pAsset4.is_simple = true;
    this.add(pAsset4);
    this.t.base_stats["warfare"] = 1f;
    GeneAsset pAsset5 = new GeneAsset();
    pAsset5.id = "warfare_2";
    this.add(pAsset5);
    this.t.base_stats["warfare"] = 2f;
    GeneAsset pAsset6 = new GeneAsset();
    pAsset6.id = "warfare_3";
    this.add(pAsset6);
    this.t.base_stats["warfare"] = 3f;
    GeneAsset pAsset7 = new GeneAsset();
    pAsset7.id = "stewardship_1";
    pAsset7.is_simple = true;
    this.add(pAsset7);
    this.t.base_stats["stewardship"] = 1f;
    GeneAsset pAsset8 = new GeneAsset();
    pAsset8.id = "stewardship_2";
    this.add(pAsset8);
    this.t.base_stats["stewardship"] = 2f;
    GeneAsset pAsset9 = new GeneAsset();
    pAsset9.id = "stewardship_3";
    this.add(pAsset9);
    this.t.base_stats["stewardship"] = 3f;
    GeneAsset pAsset10 = new GeneAsset();
    pAsset10.id = "intelligence_1";
    pAsset10.is_simple = true;
    this.add(pAsset10);
    this.t.base_stats["intelligence"] = 1f;
    GeneAsset pAsset11 = new GeneAsset();
    pAsset11.id = "intelligence_2";
    this.add(pAsset11);
    this.t.base_stats["intelligence"] = 2f;
    GeneAsset pAsset12 = new GeneAsset();
    pAsset12.id = "intelligence_3";
    this.add(pAsset12);
    this.t.base_stats["intelligence"] = 3f;
  }

  private void addSpecial()
  {
    GeneAsset pAsset1 = new GeneAsset();
    pAsset1.id = "empty";
    pAsset1.path_icon = "ui/Icons/iconEmptyLocus";
    pAsset1.is_empty = true;
    pAsset1.show_in_knowledge_window = false;
    pAsset1.needs_to_be_explored = false;
    pAsset1.show_genepool_nucleobases = false;
    pAsset1.can_drop_and_grab = false;
    this.add(pAsset1);
    GeneAsset pAsset2 = new GeneAsset();
    pAsset2.id = "temp_for_generation";
    pAsset2.path_icon = "ui/Icons/iconEmptyLocus";
    pAsset2.for_generation = true;
    pAsset2.is_empty = true;
    pAsset2.show_in_knowledge_window = false;
    pAsset2.needs_to_be_explored = false;
    pAsset2.show_genepool_nucleobases = false;
    pAsset2.can_drop_and_grab = false;
    pAsset2.has_description_1 = false;
    pAsset2.has_description_2 = false;
    pAsset2.has_localized_id = false;
    this.add(pAsset2);
    GeneAsset pAsset3 = new GeneAsset();
    pAsset3.id = "bad";
    pAsset3.is_stat_gene = false;
    pAsset3.is_bad = true;
    pAsset3.is_simple = true;
    pAsset3.needs_to_be_explored = true;
    pAsset3.synergy_sides_always = true;
    pAsset3.show_genepool_nucleobases = false;
    this.add(pAsset3);
    GeneAsset pAsset4 = new GeneAsset();
    pAsset4.id = "bonus_male";
    pAsset4.is_stat_gene = false;
    pAsset4.show_genepool_nucleobases = false;
    pAsset4.synergy_sides_always = true;
    pAsset4.is_bonus_male = true;
    this.add(pAsset4);
    GeneAsset pAsset5 = new GeneAsset();
    pAsset5.id = "bonus_female";
    pAsset5.is_stat_gene = false;
    pAsset5.show_genepool_nucleobases = false;
    pAsset5.synergy_sides_always = true;
    pAsset5.is_bonus_female = true;
    this.add(pAsset5);
    GeneAsset pAsset6 = new GeneAsset();
    pAsset6.id = "mutagenic";
    pAsset6.is_stat_gene = false;
    pAsset6.synergy_sides_always = true;
    pAsset6.show_genepool_nucleobases = false;
    this.add(pAsset6);
    this.t.base_stats_meta["mutation"] = 1f;
  }

  public override GeneAsset add(GeneAsset pAsset)
  {
    GeneAsset geneAsset = base.add(pAsset);
    geneAsset.has_description_1 = false;
    geneAsset.has_description_2 = false;
    return geneAsset;
  }

  public GeneAsset getRandomSimpleGene() => this._gene_assets_simple.GetRandom<GeneAsset>();

  public GeneAsset getRandomGeneForMutation() => this._gene_assets_mutations.GetRandom<GeneAsset>();

  public void regenerateBasicDNACodesWithLifeSeed(long pLifeSeed)
  {
    foreach (GeneAsset geneAsset in this.list)
    {
      if (geneAsset.show_genepool_nucleobases)
      {
        long pSeed = pLifeSeed + (long) geneAsset.getIndexID();
        geneAsset.generateDNA(pSeed);
      }
    }
  }

  public override void linkAssets()
  {
    base.linkAssets();
    foreach (GeneAsset geneAsset in this.list)
    {
      if (geneAsset.is_simple)
        this._gene_assets_simple.Add(geneAsset);
    }
    foreach (GeneAsset geneAsset in this.list)
    {
      if (!geneAsset.is_empty)
        this._gene_assets_mutations.Add(geneAsset);
    }
  }

  public List<GeneAsset> getGenesWithStat(string pStatID)
  {
    if (!this._cached_stat_genes_dictionary.ContainsKey(pStatID))
    {
      List<GeneAsset> geneAssetList = this.filterGenes(pStatID);
      this._cached_stat_genes_dictionary[pStatID] = geneAssetList;
    }
    return this._cached_stat_genes_dictionary[pStatID];
  }

  private List<GeneAsset> filterGenes(string pStatID)
  {
    List<GeneAsset> geneAssetList = new List<GeneAsset>();
    foreach (GeneAsset geneAsset in AssetManager.gene_library.list)
    {
      if (!geneAsset.is_empty && (geneAsset.base_stats.hasStat(pStatID) || geneAsset.base_stats_meta.hasStat(pStatID)))
        geneAssetList.Add(geneAsset);
    }
    return geneAssetList;
  }

  protected override string icon_path => "ui/Icons/genes/";
}
