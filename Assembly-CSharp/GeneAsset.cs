// Decompiled with JetBrains decompiler
// Type: GeneAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class GeneAsset : BaseTrait<GeneAsset>
{
  private const string CHARS_FOR_CODONS = "ACGT";
  public bool is_stat_gene = true;
  public bool can_drop_and_grab = true;
  public bool is_empty;
  public bool for_generation;
  public bool is_bad;
  public bool is_simple;
  public bool is_bonus_male;
  public bool is_bonus_female;
  public bool show_genepool_nucleobases = true;
  public bool synergy_sides_always;
  private string _genetic_code;
  [NonSerialized]
  public char genetic_code_right;
  [NonSerialized]
  public char genetic_code_left;
  [NonSerialized]
  public char genetic_code_up;
  [NonSerialized]
  public char genetic_code_down;
  private string _cached_sequence;
  private string _cached_sequence_locked;
  private BaseStats _cached_half_stats;
  private BaseStats _cached_half_stats_meta;

  protected override HashSet<string> progress_elements => this._progress_data?.unlocked_genes;

  public override string typed_id => "gene";

  public GeneAsset() => this.group_id = "genes";

  public override BaseCategoryAsset getGroup() => (BaseCategoryAsset) null;

  public string getSequence()
  {
    if (this.is_bad)
      return this.getHarmfulSequence();
    return !this.show_genepool_nucleobases || !this.isAvailable() ? this.getLockedSequence() : this.getColoredSequence();
  }

  private string getHarmfulSequence() => InsultStringGenerator.getDNASequenceBad();

  public string getColoredSequence()
  {
    if (string.IsNullOrEmpty(this._cached_sequence))
      this._cached_sequence = NucleobaseHelper.getColoredSequence(this._genetic_code);
    return this._cached_sequence;
  }

  public string getLockedSequence() => "??? ??? ??? ??? ??? ???";

  public BaseStats getHalfStats()
  {
    BaseStats halfStats = this._cached_half_stats;
    if (halfStats == null)
    {
      halfStats = new BaseStats();
      this._cached_half_stats = halfStats;
      foreach (BaseStatsContainer baseStatsContainer in this.base_stats.getList().ToArray())
      {
        float num = !Mathf.Approximately(Mathf.Floor(baseStatsContainer.value), baseStatsContainer.value) ? baseStatsContainer.value * 0.5f : Mathf.Floor(baseStatsContainer.value * 0.5f);
        halfStats[baseStatsContainer.id] = num;
      }
      halfStats.normalize();
    }
    return halfStats;
  }

  public BaseStats getHalfStatsMeta()
  {
    BaseStats halfStatsMeta = this._cached_half_stats_meta;
    if (halfStatsMeta == null)
    {
      halfStatsMeta = new BaseStats();
      this._cached_half_stats_meta = halfStatsMeta;
      foreach (BaseStatsContainer baseStatsContainer in this.base_stats_meta.getList().ToArray())
      {
        float num = !Mathf.Approximately(Mathf.Floor(baseStatsContainer.value), baseStatsContainer.value) ? baseStatsContainer.value * 0.5f : Mathf.Floor(baseStatsContainer.value * 0.5f);
        halfStatsMeta[baseStatsContainer.id] = num;
      }
    }
    halfStatsMeta.normalize();
    return halfStatsMeta;
  }

  public void generateDNA(long pSeed)
  {
    this._genetic_code = this.generateRandomCodonString(pSeed, 15);
    this.genetic_code_left = this._genetic_code[0];
    string geneticCode = this._genetic_code;
    this.genetic_code_right = geneticCode[geneticCode.Length - 1];
    this.genetic_code_up = this._genetic_code[8];
    this.genetic_code_down = this._genetic_code[10];
  }

  private string generateRandomCodonString(long pSeed, int pLength)
  {
    Random random = new Random((int) pSeed);
    string randomCodonString = "";
    for (int index = 0; index < pLength; ++index)
    {
      randomCodonString += "ACGT"[random.Next("ACGT".Length)].ToString();
      if ((index + 1) % 3 == 0 && index + 1 < pLength)
        randomCodonString += " ";
    }
    return randomCodonString;
  }

  protected override bool isDebugUnlockedAll() => DebugConfig.isOn(DebugOption.UnlockAllGenes);
}
