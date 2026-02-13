// Decompiled with JetBrains decompiler
// Type: Chromosome
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Chromosome
{
  private const string IMAGE_PATH_NORMAL = "chromosomes/normal/";
  private const string IMAGE_PATH_GOLD = "chromosomes/golden/";
  private const string STRING_UNKOWN = "???????";
  private const string COLOR_BOUND = "#444444";
  private const string COLORED_UNKOWN_TEXT = "<color=#444444>???????</color>";
  public readonly List<GeneAsset> genes = new List<GeneAsset>();
  private readonly BaseStats _merged_base_stats_male = new BaseStats();
  private readonly BaseStats _merged_base_stats_female = new BaseStats();
  private readonly BaseStats _merged_base_stats = new BaseStats();
  private readonly BaseStats _merged_base_stats_meta = new BaseStats();
  private static readonly (int, int)[] DIRECTIONS = new (int, int)[4]
  {
    (0, -1),
    (0, 1),
    (-1, 0),
    (1, 0)
  };
  private bool _dirty = true;
  private Sprite _cached_sprite;
  private int _cached_sprite_index = -1;
  public string chromosome_type;
  private readonly List<int> _loci_amplifiers = new List<int>();
  private readonly List<int> _loci_empty = new List<int>();
  private readonly BaseStats[] _base_stats_all = new BaseStats[4];
  private readonly int _columns;

  public Chromosome(string pType, bool pNew)
  {
    this.chromosome_type = pType;
    if (pNew)
    {
      int amountLoci = this.getAsset().amount_loci;
      GeneAsset geneAsset = AssetManager.gene_library.get("empty");
      for (int index = 0; index < amountLoci; ++index)
        this.genes.Add(geneAsset);
      this.generateAmplifiers(pType);
    }
    this._base_stats_all[0] = this._merged_base_stats;
    this._base_stats_all[1] = this._merged_base_stats_meta;
    this._base_stats_all[2] = this._merged_base_stats_male;
    this._base_stats_all[3] = this._merged_base_stats_female;
    this._columns = this.getAsset().amount_loci / 6;
  }

  public bool isLocusAmplifier(int pX, int pY) => this.isLocusAmplifier(this.getIndexFrom(pX, pY));

  public bool isLocusAmplifier(int pLocusIndex) => this._loci_amplifiers.Contains(pLocusIndex);

  public bool isVoidLocus(int pLocusIndex) => this._loci_empty.Contains(pLocusIndex);

  public bool isSpecialLocusAt(int pX, int pY) => this.isSpecialLocus(this.getIndexFrom(pX, pY));

  public bool isVoidLocusAt(int pX, int pY) => this.isVoidLocus(this.getIndexFrom(pX, pY));

  public bool isAllSidesVoidLocus(int pX, int pY)
  {
    int num1 = this.countBounds(pX, pY);
    int num2 = 0;
    if (this.isVoidLocusAt(pX - 1, pY))
      ++num2;
    if (this.isVoidLocusAt(pX + 1, pY))
      ++num2;
    if (this.isVoidLocusAt(pX, pY + 1))
      ++num2;
    if (this.isVoidLocusAt(pX, pY - 1))
      ++num2;
    return num2 == num1;
  }

  private bool isAmplifierLocusAt(int pX, int pY)
  {
    return this.isLocusAmplifier(this.getIndexFrom(pX, pY));
  }

  private bool isForcedSynergyAt(int pX, int pY)
  {
    return this.isAmplifierLocusAt(pX, pY) || this.getGeneAt(pX, pY).synergy_sides_always;
  }

  public bool isForcedSynergyLeft(int pX, int pY)
  {
    if (this.hasBoundLeft(pX, pY))
      return false;
    (int num1, int num2) = this.getDirectionOffset(GeneDirection.Left);
    return this.isForcedSynergyAt(pX + num1, pY + num2);
  }

  public bool isForcedSynergyRight(int pX, int pY)
  {
    if (this.hasBoundRight(pX, pY))
      return false;
    (int num1, int num2) = this.getDirectionOffset(GeneDirection.Right);
    return this.isForcedSynergyAt(pX + num1, pY + num2);
  }

  public bool isForcedSynergyUp(int pX, int pY)
  {
    if (this.hasBoundUp(pX, pY))
      return false;
    (int num1, int num2) = this.getDirectionOffset(GeneDirection.Up);
    return this.isForcedSynergyAt(pX + num1, pY + num2);
  }

  public bool isForcedSynergyDown(int pX, int pY)
  {
    if (this.hasBoundDown(pX, pY))
      return false;
    (int num1, int num2) = this.getDirectionOffset(GeneDirection.Down);
    return this.isForcedSynergyAt(pX + num1, pY + num2);
  }

  public LocusType getLocusType(int pLocusIndex)
  {
    if (this.isLocusAmplifier(pLocusIndex))
      return LocusType.Amplifier;
    return this.isVoidLocus(pLocusIndex) ? LocusType.Empty : LocusType.Standard;
  }

  public void fillStatsForTooltip(LocusElement pLocus, BaseStats pStatsContainer)
  {
    int locusIndex = pLocus.locus_index;
    if (this.isVoidLocus(locusIndex))
      return;
    GeneAsset geneAsset = pLocus.getGeneAsset();
    if (geneAsset.is_bonus_male)
      this.combineBonusesForSides(locusIndex, pStatsContainer);
    else if (geneAsset.is_bonus_female)
      this.combineBonusesForSides(locusIndex, pStatsContainer);
    else
      this.getBonusesFromGene(pLocus.locus_index, pStatsContainer, pCombineMeta: true);
    pStatsContainer.normalize();
  }

  private void generateAmplifiers(string pType)
  {
    ChromosomeTypeAsset chromosomeTypeAsset = AssetManager.chromosome_type_library.get(pType);
    using (ListPool<int> list = new ListPool<int>())
    {
      for (int index = 0; index < chromosomeTypeAsset.amount_loci; ++index)
        list.Add(index);
      list.Shuffle<int>();
      int num1 = Randy.randomInt(chromosomeTypeAsset.amount_loci_min_amplifier, chromosomeTypeAsset.amount_loci_max_amplifier);
      int num2 = Randy.randomInt(chromosomeTypeAsset.amount_loci_min_empty, chromosomeTypeAsset.amount_loci_max_empty);
      for (int index = 0; index < num1; ++index)
        this._loci_amplifiers.Add(list.Pop<int>());
      for (int index = 0; index < num2; ++index)
        this._loci_empty.Add(list.Pop<int>());
    }
  }

  public bool canAddGene(GeneAsset pAsset) => this.countEmpty() != 0;

  public void setGene(GeneAsset pAsset, int pIndex) => this.genes[pIndex] = pAsset;

  public GeneAsset getGene(int pIndex) => this.genes[pIndex];

  public ChromosomeTypeAsset getAsset()
  {
    return AssetManager.chromosome_type_library.get(this.chromosome_type);
  }

  public void load(ChromosomeData pData)
  {
    this.chromosome_type = pData.chromosome_type;
    foreach (string locus in pData.loci)
    {
      GeneAsset geneAsset = AssetManager.gene_library.get(locus);
      if (geneAsset != null)
        this.genes.Add(geneAsset);
    }
    this._loci_amplifiers.AddRange((IEnumerable<int>) pData.super_loci);
    this._loci_empty.AddRange((IEnumerable<int>) pData.void_loci);
  }

  public ChromosomeData getDataForSave()
  {
    ChromosomeData dataForSave = new ChromosomeData();
    foreach (GeneAsset gene in this.genes)
      dataForSave.loci.Add(gene.id);
    dataForSave.super_loci.AddRange((IEnumerable<int>) this._loci_amplifiers);
    dataForSave.void_loci.AddRange((IEnumerable<int>) this._loci_empty);
    dataForSave.chromosome_type = this.chromosome_type;
    return dataForSave;
  }

  public void addGene(GeneAsset pGeneAsset)
  {
    for (int index = 0; index < this.genes.Count; ++index)
    {
      if (this.genes[index].is_empty && this.canAddToLocus(index))
      {
        this.genes[index] = pGeneAsset;
        break;
      }
    }
    this.setDirty();
  }

  public bool isSpecialLocus(int pIndex)
  {
    return this._loci_amplifiers.Contains(pIndex) || this._loci_empty.Contains(pIndex);
  }

  public bool canAddToLocus(int pIndex)
  {
    return !this._loci_amplifiers.Contains(pIndex) && !this._loci_empty.Contains(pIndex);
  }

  public int countNonEmpty()
  {
    int num = 0;
    for (int index = 0; index < this.genes.Count; ++index)
    {
      if (!this.genes[index].is_empty && this.canAddToLocus(index))
        ++num;
    }
    return num;
  }

  public int countEmpty()
  {
    int num = 0;
    for (int index = 0; index < this.genes.Count; ++index)
    {
      if (this.genes[index].is_empty && this.canAddToLocus(index))
        ++num;
    }
    return num;
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

  public BaseStats getStatsMale()
  {
    if (this._dirty)
      this.recalculate();
    return this._merged_base_stats_male;
  }

  public BaseStats getStatsFemale()
  {
    if (this._dirty)
      this.recalculate();
    return this._merged_base_stats_female;
  }

  public void setDirty() => this._dirty = true;

  public void recalculate()
  {
    if (!this._dirty)
      return;
    this._dirty = false;
    this.clearAllBaseStats();
    BaseStats mergedBaseStats = this._merged_base_stats;
    BaseStats mergedBaseStatsMeta = this._merged_base_stats_meta;
    BaseStats mergedBaseStatsMale = this._merged_base_stats_male;
    BaseStats mergedBaseStatsFemale = this._merged_base_stats_female;
    for (int pLocusIndex = 0; pLocusIndex < this.genes.Count; ++pLocusIndex)
    {
      if (!this.isVoidLocus(pLocusIndex))
        this.getBonusesFromGene(pLocusIndex, mergedBaseStats, mergedBaseStatsMeta);
    }
    for (int index = 0; index < this.genes.Count; ++index)
    {
      GeneAsset gene = this.genes[index];
      if (!this.isVoidLocus(index))
      {
        if (gene.is_bonus_male)
          this.combineBonusesForSides(index, mergedBaseStatsMale);
        if (gene.is_bonus_female)
          this.combineBonusesForSides(index, mergedBaseStatsFemale);
      }
    }
  }

  private void combineBonusesForSides(int pLocusIndex, BaseStats pBaseStatsMain)
  {
    (int pX, int pY) = this.getXYFromIndex(pLocusIndex);
    int num1 = this.isNextToBad(pLocusIndex) ? 1 : 0;
    this.getBonusesFromGene(pX, pY + 1, pBaseStatsMain);
    this.getBonusesFromGene(pX, pY - 1, pBaseStatsMain);
    this.getBonusesFromGene(pX - 1, pY, pBaseStatsMain);
    this.getBonusesFromGene(pX + 1, pY, pBaseStatsMain);
    if (num1 == 0)
      return;
    foreach (BaseStatsContainer baseStatsContainer in pBaseStatsMain.getList().ToArray())
    {
      float num2 = !Mathf.Approximately(Mathf.Floor(baseStatsContainer.value), baseStatsContainer.value) ? baseStatsContainer.value * 0.5f : Mathf.Floor(baseStatsContainer.value * 0.5f);
      pBaseStatsMain[baseStatsContainer.id] = num2;
    }
    pBaseStatsMain.normalize();
  }

  private void getBonusesFromGene(
    int pX,
    int pY,
    BaseStats pBaseStatsMain,
    BaseStats pBaseStatsMeta = null,
    bool pCombineMeta = false)
  {
    if (this.getGeneAt(pX, pY) == null)
      return;
    this.getBonusesFromGene(this.getIndexFrom(pX, pY), pBaseStatsMain, pBaseStatsMeta, pCombineMeta);
  }

  private void getBonusesFromGene(
    int pLocusIndex,
    BaseStats pBaseStatsMain,
    BaseStats pBaseStatsMeta = null,
    bool pCombineMeta = false)
  {
    GeneAsset gene = this.genes[pLocusIndex];
    bool flag = this.hasFullSynergy(pLocusIndex);
    int num = this.isNextToBad(pLocusIndex) ? 1 : 0;
    if (num != 0)
      flag = false;
    if (num != 0)
    {
      pBaseStatsMain.mergeStats(gene.getHalfStats());
      if (pCombineMeta)
        pBaseStatsMain.mergeStats(gene.getHalfStatsMeta());
      else
        pBaseStatsMeta?.mergeStats(gene.getHalfStatsMeta());
    }
    else
    {
      pBaseStatsMain.mergeStats(gene.base_stats);
      if (pCombineMeta)
        pBaseStatsMain.mergeStats(gene.base_stats_meta);
      pBaseStatsMeta?.mergeStats(gene.base_stats_meta);
      if (!flag || gene.synergy_sides_always)
        return;
      pBaseStatsMain.mergeStats(gene.base_stats);
      if (pCombineMeta)
        pBaseStatsMain.mergeStats(gene.base_stats_meta);
      else
        pBaseStatsMeta?.mergeStats(gene.base_stats_meta);
    }
  }

  private void clearAllBaseStats()
  {
    foreach (BaseStats baseStats in this._base_stats_all)
      baseStats.clear();
  }

  public bool hasFullSynergyAt(int pX, int pY)
  {
    int num1 = 0;
    if (this.isAllSidesVoidLocus(pX, pY) || this.isNextToBad(pX, pY) || this.isNextToBadAmplifier(pX, pY))
      return false;
    if (this.hasSynergyConnectionLeft(pX, pY))
      ++num1;
    if (this.hasSynergyConnectionRight(pX, pY))
      ++num1;
    if (this.hasSynergyConnectionUp(pX, pY))
      ++num1;
    if (this.hasSynergyConnectionDown(pX, pY))
      ++num1;
    int num2 = 0;
    if (!this.hasBoundLeft(pX, pY))
      ++num2;
    if (!this.hasBoundRight(pX, pY))
      ++num2;
    if (!this.hasBoundUp(pX, pY))
      ++num2;
    if (!this.hasBoundDown(pX, pY))
      ++num2;
    return num1 == num2;
  }

  public bool hasFullSynergy(int pLocusIndex)
  {
    (int, int) xyFromIndex = this.getXYFromIndex(pLocusIndex);
    return this.hasFullSynergyAt(xyFromIndex.Item1, xyFromIndex.Item2);
  }

  public bool hasAnySynergy(int pLocusIndex)
  {
    (int pFromX, int pFromY) = this.getXYFromIndex(pLocusIndex);
    return this.hasSynergyConnectionLeft(pFromX, pFromY) || this.hasSynergyConnectionRight(pFromX, pFromY) || this.hasSynergyConnectionUp(pFromX, pFromY) || this.hasSynergyConnectionDown(pFromX, pFromY);
  }

  public string getSynergyTooltipText(int pLocusIndex)
  {
    (int num1, int num2) = this.getXYFromIndex(pLocusIndex);
    GeneAsset geneAt = this.getGeneAt(num1, num2);
    using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
    {
      bool flag1 = this.isBadAt(num1, num2);
      if (this.hasAnySynergy(pLocusIndex) && !flag1)
        stringBuilderPool.Append(Toolbox.coloredString(LocalizedTextManager.getText("sequence_synergy"), "#FFFFAA"));
      else
        stringBuilderPool.Append(LocalizedTextManager.getText("sequence_synergy"));
      stringBuilderPool.Append("\n");
      int num3 = this.hasSynergyConnectionLeft(num1, num2) ? 1 : 0;
      bool flag2 = this.hasSynergyConnectionRight(num1, num2);
      int num4 = this.hasSynergyConnectionUp(num1, num2) ? 1 : 0;
      bool flag3 = this.hasSynergyConnectionDown(num1, num2);
      GeneAsset geneLeft = this.getGeneLeft(num1, num2);
      GeneAsset geneRight = this.getGeneRight(num1, num2);
      GeneAsset geneUp = this.getGeneUp(num1, num2);
      GeneAsset geneDown = this.getGeneDown(num1, num2);
      bool flag4 = this.isForcedSynergyAt(num1, num2);
      if (num4 != 0)
      {
        if (flag1 || this.isBadAt(num1, num2 - 1) || this.hasAmplifierBad(num1, num2 - 1))
          stringBuilderPool.Append(this.getBadConnectionString());
        else if (flag4)
          stringBuilderPool.Append(NucleobaseHelper.getColoredNucleobaseFull(geneUp.genetic_code_down));
        else
          stringBuilderPool.Append(NucleobaseHelper.getColoredNucleobaseFull(geneAt.genetic_code_up));
      }
      else if (this.hasBoundUp(num1, num2) || this.isConnectionDeniedUp(num1, num2))
        stringBuilderPool.Append("<color=#444444>???????</color>");
      else
        stringBuilderPool.Append(this.getNotConnectedText(geneAt.genetic_code_up, World.world.getCurSessionTime()));
      stringBuilderPool.Append("\n");
      if (num3 != 0)
      {
        if (flag1 || this.isBadAt(num1 - 1, num2) || this.hasAmplifierBad(num1 - 1, num2))
          stringBuilderPool.Append(this.getBadConnectionString());
        else if (flag4)
          stringBuilderPool.Append(NucleobaseHelper.getColoredNucleobaseFull(geneLeft.genetic_code_right));
        else
          stringBuilderPool.Append(NucleobaseHelper.getColoredNucleobaseFull(geneAt.genetic_code_left));
      }
      else if (this.hasBoundLeft(num1, num2) || this.isConnectionDeniedLeft(num1, num2))
        stringBuilderPool.Append("<color=#444444>???????</color>");
      else
        stringBuilderPool.Append(this.getNotConnectedText(geneAt.genetic_code_left, World.world.getCurSessionTime()));
      stringBuilderPool.Append(" ... ");
      if (flag2)
      {
        if (flag1 || this.isBadAt(num1 + 1, num2) || this.hasAmplifierBad(num1 + 1, num2))
          stringBuilderPool.Append(this.getBadConnectionString());
        else if (flag4)
          stringBuilderPool.Append(NucleobaseHelper.getColoredNucleobaseFull(geneRight.genetic_code_left));
        else
          stringBuilderPool.Append(NucleobaseHelper.getColoredNucleobaseFull(geneAt.genetic_code_right));
      }
      else if (this.hasBoundRight(num1, num2) || this.isConnectionDeniedRight(num1, num2))
        stringBuilderPool.Append("<color=#444444>???????</color>");
      else
        stringBuilderPool.Append(this.getNotConnectedText(geneAt.genetic_code_right, World.world.getCurSessionTime()));
      stringBuilderPool.Append("\n");
      if (flag3)
      {
        if (flag1 || this.isBadAt(num1, num2 + 1) || this.hasAmplifierBad(num1, num2 + 1))
          stringBuilderPool.Append(this.getBadConnectionString());
        else if (flag4)
          stringBuilderPool.Append(NucleobaseHelper.getColoredNucleobaseFull(geneDown.genetic_code_up));
        else
          stringBuilderPool.Append(NucleobaseHelper.getColoredNucleobaseFull(geneAt.genetic_code_down));
      }
      else if (this.hasBoundDown(num1, num2) || this.isConnectionDeniedDown(num1, num2))
        stringBuilderPool.Append("<color=#444444>???????</color>");
      else
        stringBuilderPool.Append(this.getNotConnectedText(geneAt.genetic_code_down, World.world.getCurSessionTime()));
      stringBuilderPool.Append("\n");
      stringBuilderPool.Append("\n");
      return stringBuilderPool.ToString();
    }
  }

  private string getBadConnectionString() => InsultStringGenerator.getBadConnectionString();

  private string getNotConnectedText(char pChar, double pTime)
  {
    string fullNucleobaseName = NucleobaseHelper.getFullNucleobaseName(pChar);
    string colorHex = NucleobaseHelper.getColorHex(pChar, true);
    using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
    {
      for (int index = 0; index < fullNucleobaseName.Length; ++index)
        stringBuilderPool.Append(fullNucleobaseName[index]);
      int num = (int) pChar * 100;
      int index1 = (int) ((pTime + (double) num) * 8.0 % (double) fullNucleobaseName.Length);
      stringBuilderPool[index1] = '?';
      return Toolbox.coloredString(stringBuilderPool.ToString(), colorHex);
    }
  }

  private int getIndexFrom(int pX, int pY) => pX + pY * 6;

  public (int, int) getXYFromIndex(int pIndex) => (pIndex % 6, pIndex / 6);

  public Sprite getSpriteNormal()
  {
    Sprite[] spriteList = SpriteTextureLoader.getSpriteList("chromosomes/normal/");
    if (this._cached_sprite_index == -1)
      this._cached_sprite_index = Randy.randomInt(0, spriteList.Length - 1);
    this._cached_sprite = spriteList[this._cached_sprite_index];
    return this._cached_sprite;
  }

  public Sprite getSpriteGolden()
  {
    Sprite[] spriteList = SpriteTextureLoader.getSpriteList("chromosomes/golden/");
    if (this._cached_sprite_index == -1)
      this._cached_sprite_index = Randy.randomInt(0, spriteList.Length - 1);
    this._cached_sprite = spriteList[this._cached_sprite_index];
    return this._cached_sprite;
  }

  public void cloneFrom(Chromosome pParentChromosome)
  {
    this.genes.AddRange((IEnumerable<GeneAsset>) pParentChromosome.genes);
    this._loci_empty.AddRange((IEnumerable<int>) pParentChromosome._loci_empty);
    this._loci_amplifiers.AddRange((IEnumerable<int>) pParentChromosome._loci_amplifiers);
    this.setDirty();
  }

  public void mutateRandomGene()
  {
    using (ListPool<int> list = new ListPool<int>())
    {
      for (int pIndex = 0; pIndex < this.genes.Count; ++pIndex)
      {
        if (!this.isSpecialLocus(pIndex))
          list.Add(pIndex);
      }
      int random = list.GetRandom<int>();
      this.setGene(AssetManager.gene_library.getRandomGeneForMutation(), random);
      this.setDirty();
    }
  }

  public bool hasGene(GeneAsset pAsset)
  {
    List<GeneAsset> genes = this.genes;
    // ISSUE: explicit non-virtual call
    return genes != null && __nonvirtual (genes.Contains(pAsset));
  }

  public GeneAsset getGeneAtDirectionFrom(int pFromX, int pFromY, GeneDirection pDirection)
  {
    (int num1, int num2) = this.getDirectionOffset(pDirection);
    return !this.isCoordinatesValid(pFromX + num1, pFromY + num2) ? (GeneAsset) null : this.genes[this.getIndexFrom(pFromX + num1, pFromY + num2)];
  }

  public GeneAsset getGeneAt(int pFromX, int pFromY)
  {
    if (!this.isCoordinatesValid(pFromX, pFromY))
      return (GeneAsset) null;
    int indexFrom = this.getIndexFrom(pFromX, pFromY);
    return !this.isIndexValid(indexFrom) ? (GeneAsset) null : this.genes[indexFrom];
  }

  public GeneAsset getGeneLeft(int pFromX, int pFromY)
  {
    return this.getGeneAtDirectionFrom(pFromX, pFromY, GeneDirection.Left);
  }

  public GeneAsset getGeneRight(int pFromX, int pFromY)
  {
    return this.getGeneAtDirectionFrom(pFromX, pFromY, GeneDirection.Right);
  }

  public GeneAsset getGeneUp(int pFromX, int pFromY)
  {
    return this.getGeneAtDirectionFrom(pFromX, pFromY, GeneDirection.Up);
  }

  public GeneAsset getGeneDown(int pFromX, int pFromY)
  {
    return this.getGeneAtDirectionFrom(pFromX, pFromY, GeneDirection.Down);
  }

  private bool isIndexValid(int pIndex) => pIndex >= 0 && pIndex < this.genes.Count;

  private bool isCoordinatesValid(int pX, int pY)
  {
    return pX >= 0 && pY >= 0 && pX < 6 && pY < this._columns;
  }

  public (int, int) getDirectionOffset(GeneDirection pDirection)
  {
    switch (pDirection)
    {
      case GeneDirection.Up:
        return Chromosome.DIRECTIONS[0];
      case GeneDirection.Down:
        return Chromosome.DIRECTIONS[1];
      case GeneDirection.Left:
        return Chromosome.DIRECTIONS[2];
      case GeneDirection.Right:
        return Chromosome.DIRECTIONS[3];
      default:
        return (0, 0);
    }
  }

  public bool canBeConnectedTo(int pFromX, int pFromY, int pToX, int pToY)
  {
    GeneAsset geneAt1 = this.getGeneAt(pFromX, pFromY);
    GeneAsset geneAt2 = this.getGeneAt(pToX, pToY);
    if (geneAt1 == null || geneAt2 == null || geneAt1.is_empty)
      return false;
    int num = geneAt2.is_empty ? 1 : 0;
    return false;
  }

  public int countBounds(int pX, int pY)
  {
    int num = 0;
    if (this.isCoordinatesValid(pX - 1, pY))
      ++num;
    if (this.isCoordinatesValid(pX + 1, pY))
      ++num;
    if (this.isCoordinatesValid(pX, pY - 1))
      ++num;
    if (this.isCoordinatesValid(pX, pY + 1))
      ++num;
    return num;
  }

  public bool hasSynergyConnectionLeft(int pFromX, int pFromY)
  {
    return !this.hasBoundLeft(pFromX, pFromY) && this.hasSynergyConnection(pFromX, pFromY, GeneDirection.Left);
  }

  public bool hasSynergyConnectionRight(int pFromX, int pFromY)
  {
    return !this.hasBoundRight(pFromX, pFromY) && this.hasSynergyConnection(pFromX, pFromY, GeneDirection.Right);
  }

  public bool hasSynergyConnectionUp(int pFromX, int pFromY)
  {
    return !this.hasBoundUp(pFromX, pFromY) && this.hasSynergyConnection(pFromX, pFromY, GeneDirection.Up);
  }

  public bool hasSynergyConnectionDown(int pFromX, int pFromY)
  {
    return !this.hasBoundDown(pFromX, pFromY) && this.hasSynergyConnection(pFromX, pFromY, GeneDirection.Down);
  }

  public bool isAllLociSynergy()
  {
    int pIndex = -1;
    foreach (GeneAsset gene in this.genes)
    {
      ++pIndex;
      if (!gene.is_empty && !gene.synergy_sides_always)
      {
        if (gene.is_bad)
          return false;
        (int, int) xyFromIndex = this.getXYFromIndex(pIndex);
        if (!this.hasAllSynergiesAt(xyFromIndex.Item1, xyFromIndex.Item2, false))
          return false;
      }
    }
    return true;
  }

  public bool hasAllSynergiesAt(int pFromX, int pFromY, bool pCheckBounds = true)
  {
    if (this.isAllSidesVoidLocus(pFromX, pFromY))
      return false;
    int num1 = pCheckBounds ? (this.hasSynergyConnectionLeft(pFromX, pFromY) ? 1 : 0) : (this.hasBoundLeft(pFromX, pFromY) ? 1 : (this.hasSynergyConnection(pFromX, pFromY, GeneDirection.Left) ? 1 : 0));
    bool flag1 = pCheckBounds ? this.hasSynergyConnectionRight(pFromX, pFromY) : this.hasBoundRight(pFromX, pFromY) || this.hasSynergyConnection(pFromX, pFromY, GeneDirection.Right);
    bool flag2 = pCheckBounds ? this.hasSynergyConnectionUp(pFromX, pFromY) : this.hasBoundUp(pFromX, pFromY) || this.hasSynergyConnection(pFromX, pFromY, GeneDirection.Up);
    bool flag3 = pCheckBounds ? this.hasSynergyConnectionDown(pFromX, pFromY) : this.hasBoundDown(pFromX, pFromY) || this.hasSynergyConnection(pFromX, pFromY, GeneDirection.Down);
    int num2 = flag1 ? 1 : 0;
    return (num1 & num2 & (flag2 ? 1 : 0) & (flag3 ? 1 : 0)) != 0;
  }

  public bool hasSynergyConnection(int pFromX, int pFromY, GeneDirection pDirection)
  {
    GeneAsset geneAt1 = this.getGeneAt(pFromX, pFromY);
    bool flag1 = this.isAmplifierLocusAt(pFromX, pFromY);
    bool flag2 = false;
    if (geneAt1.synergy_sides_always)
      flag1 = true;
    if (geneAt1.is_bad)
      flag2 = true;
    if (!flag1 && geneAt1.is_empty)
      return false;
    (int num1, int num2) = this.getDirectionOffset(pDirection);
    GeneAsset geneAt2 = this.getGeneAt(pFromX + num1, pFromY + num2);
    bool flag3 = this.isAmplifierLocusAt(pFromX + num1, pFromY + num2);
    if (geneAt2 != null && geneAt2.synergy_sides_always)
      flag3 = true;
    if (geneAt2 != null && geneAt2.is_bad)
      flag2 = true;
    if (!flag3 && (geneAt2 == null || geneAt2.is_empty) || !flag2 & flag1 & flag3)
      return false;
    switch (pDirection)
    {
      case GeneDirection.Up:
        if (flag1 | flag3 || (int) geneAt1.genetic_code_up == (int) geneAt2.genetic_code_down)
          return true;
        break;
      case GeneDirection.Down:
        if (flag1 | flag3 || (int) geneAt1.genetic_code_down == (int) geneAt2.genetic_code_up)
          return true;
        break;
      case GeneDirection.Left:
        if (flag1 | flag3 || (int) geneAt1.genetic_code_left == (int) geneAt2.genetic_code_right)
          return true;
        break;
      case GeneDirection.Right:
        if (flag1 | flag3 || (int) geneAt1.genetic_code_right == (int) geneAt2.genetic_code_left)
          return true;
        break;
    }
    return false;
  }

  public bool isConnectionDeniedUp(int pFromX, int pFromY)
  {
    return this.hasBoundAt(pFromX, pFromY - 1) || this.isForcedSynergyUp(pFromX, pFromY) && this.isForcedSynergyAt(pFromX, pFromY);
  }

  public bool isConnectionDeniedDown(int pFromX, int pFromY)
  {
    return this.hasBoundAt(pFromX, pFromY + 1) || this.isForcedSynergyDown(pFromX, pFromY) && this.isForcedSynergyAt(pFromX, pFromY);
  }

  public bool isConnectionDeniedLeft(int pFromX, int pFromY)
  {
    return this.hasBoundAt(pFromX - 1, pFromY) || this.isForcedSynergyLeft(pFromX, pFromY) && this.isForcedSynergyAt(pFromX, pFromY);
  }

  public bool isConnectionDeniedRight(int pFromX, int pFromY)
  {
    return this.hasBoundAt(pFromX + 1, pFromY) || this.isForcedSynergyRight(pFromX, pFromY) && this.isForcedSynergyAt(pFromX, pFromY);
  }

  public bool hasBoundAt(int pX, int pY)
  {
    return !this.isCoordinatesValid(pX, pY) || this.isVoidLocusAt(pX, pY);
  }

  public bool hasBoundLeft(int pX, int pY) => this.hasBoundAt(pX - 1, pY);

  public bool hasBoundRight(int pX, int pY) => this.hasBoundAt(pX + 1, pY);

  public bool hasBoundUp(int pX, int pY) => this.hasBoundAt(pX, pY - 1);

  public bool hasBoundDown(int pX, int pY) => this.hasBoundAt(pX, pY + 1);

  public void fillEmptyLoci()
  {
    for (int index = 0; index < this.genes.Count; ++index)
    {
      GeneAsset gene = this.genes[index];
      if (!this.isSpecialLocus(index) && gene.is_empty)
        this.setGene(AssetManager.gene_library.getRandomSimpleGene(), index);
    }
  }

  public bool isNextToBad(int pLocusIndex)
  {
    (int, int) xyFromIndex = this.getXYFromIndex(pLocusIndex);
    return this.isNextToBad(xyFromIndex.Item1, xyFromIndex.Item2);
  }

  public bool isNextToBad(int pX, int pY)
  {
    foreach ((int num1, int num2) in Chromosome.DIRECTIONS)
    {
      if (this.isBadAt(pX + num1, pY + num2))
        return true;
    }
    return false;
  }

  public bool hasGenesAround(int pIndex)
  {
    (int, int) xyFromIndex = this.getXYFromIndex(pIndex);
    return this.hasGenesAround(xyFromIndex.Item1, xyFromIndex.Item2);
  }

  public bool hasGenesAround(int pX, int pY)
  {
    foreach ((int num1, int num2) in Chromosome.DIRECTIONS)
    {
      int num3 = pX + num1;
      int num4 = pY + num2;
      if (this.isAmplifierLocusAt(num3, num4))
        return true;
      GeneAsset geneAt = this.getGeneAt(num3, num4);
      if (geneAt != null && !geneAt.is_empty)
        return true;
    }
    return false;
  }

  public bool isNextToBadAmplifier(int pX, int pY)
  {
    foreach ((int num1, int num2) in Chromosome.DIRECTIONS)
    {
      if (this.hasAmplifierBad(pX + num1, pY + num2))
        return true;
    }
    return false;
  }

  public bool isBadAt(int pX, int pY)
  {
    if (this.isVoidLocusAt(pX, pY))
      return false;
    GeneAsset geneAt = this.getGeneAt(pX, pY);
    return geneAt != null && geneAt.is_bad;
  }

  public bool hasAmplifierBad(int pX, int pY)
  {
    return this.isLocusAmplifier(pX, pY) && this.isNextToBad(pX, pY);
  }

  public void shuffleGenes()
  {
    GeneAsset geneForGeneration = GeneLibrary.gene_for_generation;
    using (ListPool<GeneAsset> list = new ListPool<GeneAsset>())
    {
      for (int index = 0; index < this.genes.Count; ++index)
      {
        GeneAsset gene = this.genes[index];
        if (!gene.is_empty)
        {
          list.Add(gene);
          this.genes[index] = geneForGeneration;
        }
      }
      list.Shuffle<GeneAsset>();
      for (int index = 0; index < this.genes.Count && list.Count != 0; ++index)
      {
        if (this.genes[index].for_generation)
          this.genes[index] = list.Pop<GeneAsset>();
      }
      this.setDirty();
    }
  }
}
