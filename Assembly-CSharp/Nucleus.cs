// Decompiled with JetBrains decompiler
// Type: Nucleus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Nucleus
{
  public readonly List<Chromosome> chromosomes = new List<Chromosome>();
  private readonly BaseStats _merged_base_stats_male = new BaseStats();
  private readonly BaseStats _merged_base_stats_female = new BaseStats();
  private readonly BaseStats _merged_base_stats = new BaseStats();
  private readonly BaseStats _merged_base_stats_meta = new BaseStats();
  private bool _dirty = true;
  private readonly BaseStats[] _base_stats_all = new BaseStats[4];
  public readonly List<string> pot_possible_attributes = new List<string>();

  public Nucleus()
  {
    this._base_stats_all[0] = this._merged_base_stats;
    this._base_stats_all[1] = this._merged_base_stats_meta;
    this._base_stats_all[2] = this._merged_base_stats_male;
    this._base_stats_all[3] = this._merged_base_stats_female;
  }

  public BaseStats base_stats_male => this._merged_base_stats_male;

  public BaseStats base_stats_female => this._merged_base_stats_female;

  public void createFrom(ActorAsset pActorAsset)
  {
    this.reset();
    Chromosome chromosome = (Chromosome) null;
    using (ListPool<string> genesFromBaseStats = this.generateGenesFromBaseStats(pActorAsset.genome_parts))
    {
      genesFromBaseStats.Shuffle<string>();
      bool flag = WorldLawLibrary.world_law_gene_spaghetti.isEnabled();
      for (int index = 0; index < genesFromBaseStats.Count; ++index)
      {
        string pID = genesFromBaseStats[index];
        if (flag && Randy.randomChance(0.777f))
        {
          string randomNormalGene = AssetManager.gene_library.getRandomNormalGene();
          if (!string.IsNullOrEmpty(randomNormalGene))
            pID = randomNormalGene;
        }
        GeneAsset geneAsset = AssetManager.gene_library.get(pID);
        if (geneAsset == null)
          Debug.LogError((object) ("GeneAsset not found: " + pID));
        else if (chromosome != null && chromosome.canAddGene(geneAsset))
        {
          chromosome.addGene(geneAsset);
        }
        else
        {
          List<string> chromosomesFirst = pActorAsset.chromosomes_first;
          // ISSUE: explicit non-virtual call
          Chromosome pChromosome = new Chromosome(((chromosomesFirst != null ? (__nonvirtual (chromosomesFirst.Count) > 0 ? 1 : 0) : 0) == 0 || this.chromosomes.Count != 0 ? (Asset) AssetManager.chromosome_type_library.list.GetRandom<ChromosomeTypeAsset>() : (Asset) AssetManager.chromosome_type_library.get(pActorAsset.chromosomes_first.GetRandom<string>())).id, true);
          this.addChromosome(pChromosome);
          chromosome = pChromosome;
          chromosome.addGene(geneAsset);
        }
      }
      this.fillAllEmptyLoci();
      this.shuffleGenesBetweenChromosomes();
      this.setDirty();
    }
  }

  public void fillAllEmptyLoci()
  {
    foreach (Chromosome chromosome in this.chromosomes)
      chromosome.fillEmptyLoci();
  }

  private void shuffleGenesBetweenChromosomes()
  {
    using (ListPool<GeneAsset> list = new ListPool<GeneAsset>())
    {
      for (int index1 = 0; index1 < this.chromosomes.Count; ++index1)
      {
        Chromosome chromosome = this.chromosomes[index1];
        for (int index2 = 0; index2 < chromosome.genes.Count; ++index2)
        {
          GeneAsset gene = chromosome.genes[index2];
          if (!gene.is_empty)
          {
            chromosome.genes[index2] = GeneLibrary.gene_for_generation;
            list.Add(gene);
          }
        }
      }
      list.Shuffle<GeneAsset>();
      for (int index3 = 0; index3 < this.chromosomes.Count; ++index3)
      {
        Chromosome chromosome = this.chromosomes[index3];
        for (int index4 = 0; index4 < chromosome.genes.Count; ++index4)
        {
          if (chromosome.genes[index4].for_generation)
            chromosome.genes[index4] = list.Pop<GeneAsset>();
        }
      }
    }
  }

  private bool isStatGene(string pID)
  {
    return !(pID == "bonus_male") && !(pID == "bonus_female") && !(pID == "bonus_sex_random") && !(pID == "bad");
  }

  private ListPool<string> generateGenesFromBaseStats(HashSet<GenomePart> pGeneTemplateStats)
  {
    ListPool<string> pResultGenes = new ListPool<string>();
    Dictionary<string, float> pDictRemainingValues = new Dictionary<string, float>();
    this.fillRemainingValues(pGeneTemplateStats, pDictRemainingValues, pResultGenes);
    bool flag1 = true;
    int num = 300;
    while (flag1)
    {
      flag1 = false;
      if (num-- < 0)
      {
        Debug.LogError((object) "generateGenesFromBaseStats infinite loop");
        break;
      }
      foreach (GenomePart geneTemplateStat in pGeneTemplateStats)
      {
        GenomePart tGenomePart = geneTemplateStat;
        if (this.isStatGene(tGenomePart.id))
        {
          List<GeneAsset> genesWithStat = AssetManager.gene_library.getGenesWithStat(tGenomePart.id);
          if (Randy.randomChance(0.8f))
            genesWithStat.Sort((Comparison<GeneAsset>) ((pG1, pG2) => Nucleus.sortByStatValue(pG1, pG2, tGenomePart.id)));
          else
            genesWithStat.Shuffle<GeneAsset>();
          for (int index = 0; index < genesWithStat.Count; ++index)
          {
            GeneAsset geneAsset = genesWithStat[index];
            bool flag2 = true;
            foreach (BaseStatsContainer baseStatsContainer in geneAsset.base_stats.getList())
            {
              if (!pDictRemainingValues.ContainsKey(baseStatsContainer.id) || (double) pDictRemainingValues[baseStatsContainer.id] < (double) baseStatsContainer.value)
              {
                flag2 = false;
                break;
              }
            }
            foreach (BaseStatsContainer baseStatsContainer in geneAsset.base_stats_meta.getList())
            {
              if (!pDictRemainingValues.ContainsKey(baseStatsContainer.id) || (double) pDictRemainingValues[baseStatsContainer.id] < (double) baseStatsContainer.value)
              {
                flag2 = false;
                break;
              }
            }
            if (flag2)
            {
              foreach (BaseStatsContainer baseStatsContainer in geneAsset.base_stats.getList())
                pDictRemainingValues[baseStatsContainer.id] -= baseStatsContainer.value;
              foreach (BaseStatsContainer baseStatsContainer in geneAsset.base_stats_meta.getList())
                pDictRemainingValues[baseStatsContainer.id] -= baseStatsContainer.value;
              pResultGenes.Add(geneAsset.id);
              flag1 = true;
              break;
            }
          }
          if (flag1)
            break;
        }
      }
    }
    return pResultGenes;
  }

  private void fillRemainingValues(
    HashSet<GenomePart> pGeneTemplateStats,
    Dictionary<string, float> pDictRemainingValues,
    ListPool<string> pResultGenes)
  {
    foreach (GenomePart geneTemplateStat in pGeneTemplateStats)
    {
      string id = geneTemplateStat.id;
      if (this.isStatGene(id))
        pDictRemainingValues.Add(id, geneTemplateStat.value);
      else
        this.addSpecialGenes(id, (int) geneTemplateStat.value, pResultGenes);
    }
  }

  private void addSpecialGenes(string pSpecialGeneID, int pAmount, ListPool<string> pResultGenes)
  {
    for (int index = 0; index < pAmount; ++index)
    {
      if (pSpecialGeneID == "bonus_sex_random")
      {
        if (Randy.randomBool())
          pResultGenes.Add("bonus_male");
        else
          pResultGenes.Add("bonus_female");
      }
      else
        pResultGenes.Add(pSpecialGeneID);
    }
  }

  private static int sortByStatValue(GeneAsset pA, GeneAsset pB, string pStatID)
  {
    if (!pA.base_stats.hasStat(pStatID) && !pB.base_stats.hasStat(pStatID))
      return 0;
    if (!pA.base_stats.hasStat(pStatID))
      return 1;
    return !pB.base_stats.hasStat(pStatID) ? -1 : pB.base_stats[pStatID].CompareTo(pA.base_stats[pStatID]);
  }

  public List<ChromosomeData> getListForSave()
  {
    List<ChromosomeData> listForSave = new List<ChromosomeData>();
    foreach (Chromosome chromosome in this.chromosomes)
    {
      ChromosomeData dataForSave = chromosome.getDataForSave();
      listForSave.Add(dataForSave);
    }
    return listForSave;
  }

  public BaseStats getStats()
  {
    if (this._dirty)
      this.recalculate();
    return this._merged_base_stats;
  }

  public BaseStats getStatsMeta()
  {
    if (this._dirty)
      this.recalculate();
    return this._merged_base_stats_meta;
  }

  private void recalculate()
  {
    if (!this._dirty)
      return;
    this._dirty = false;
    BaseStats mergedBaseStats = this._merged_base_stats;
    BaseStats mergedBaseStatsMeta = this._merged_base_stats_meta;
    BaseStats mergedBaseStatsMale = this._merged_base_stats_male;
    BaseStats mergedBaseStatsFemale = this._merged_base_stats_female;
    this.clearAllBaseStats();
    foreach (Chromosome chromosome in this.chromosomes)
    {
      chromosome.recalculate();
      mergedBaseStats.mergeStats(chromosome.getStats());
      mergedBaseStatsMeta.mergeStats(chromosome.getStatsMeta());
      mergedBaseStatsMale.mergeStats(chromosome.getStatsMale());
      mergedBaseStatsFemale.mergeStats(chromosome.getStatsFemale());
    }
    this.pot_possible_attributes.Clear();
    this.pot_possible_attributes.AddTimes<string>((int) mergedBaseStats["intelligence"], "intelligence");
    this.pot_possible_attributes.AddTimes<string>((int) mergedBaseStats["warfare"], "warfare");
    this.pot_possible_attributes.AddTimes<string>((int) mergedBaseStats["stewardship"], "stewardship");
    this.pot_possible_attributes.AddTimes<string>((int) mergedBaseStats["diplomacy"], "diplomacy");
  }

  public void setDirty() => this._dirty = true;

  public void addChromosome(Chromosome pChromosome) => this.chromosomes.Add(pChromosome);

  public void reset()
  {
    this.setDirty();
    this.chromosomes.Clear();
  }

  private void clearAllBaseStats()
  {
    foreach (BaseStats baseStats in this._base_stats_all)
      baseStats.clear();
  }

  public void cloneFrom(Nucleus pParentsSubspeciesNucleus)
  {
    this.chromosomes.Clear();
    foreach (Chromosome chromosome in pParentsSubspeciesNucleus.chromosomes)
    {
      Chromosome pChromosome = new Chromosome(chromosome.chromosome_type, false);
      pChromosome.cloneFrom(chromosome);
      this.addChromosome(pChromosome);
    }
    this.setDirty();
  }

  public void doRandomGeneMutations(int pMutationAmount)
  {
    foreach (Chromosome chromosome in this.chromosomes)
    {
      for (int index = 0; index < pMutationAmount; ++index)
        chromosome.mutateRandomGene();
    }
  }

  public void unstableGenomeEvent()
  {
    foreach (Chromosome chromosome in this.chromosomes)
      chromosome.shuffleGenes();
    this.setDirty();
  }
}
