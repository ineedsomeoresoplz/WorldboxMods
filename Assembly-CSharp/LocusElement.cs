// Decompiled with JetBrains decompiler
// Type: LocusElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class LocusElement : ChainElement, IDropHandler, IEventSystemHandler
{
  private Chromosome _chromosome;
  private LocusClickEvent _locus_click_event;
  private Action _chromosome_updated_event;
  private LocusType locus_type;
  public Image sprite_background;
  public Image effect_amplifier;
  public Image effect_locus_amplifier_bad;
  public Sprite sprite_locus_bg_normal;
  public Sprite sprite_locus_bg_synergy;
  public Sprite sprite_locus_bg_bad;
  [SerializeField]
  private LocusDot _dot_left;
  [SerializeField]
  private LocusDot _dot_right;
  [SerializeField]
  private LocusDot _dot_up;
  [SerializeField]
  private LocusDot _dot_down;
  private float _normal_size = 0.8f;
  private float _super_size = 0.8f;
  private int _locus_x;
  private int _locus_y;
  private SpriteAnimation _animation_amplifier;
  private SpriteAnimation _animation_amplifier_bad;
  [SerializeField]
  private GeneButton _gene_button;

  protected override void create()
  {
    base.create();
    this.is_editor_button = false;
    this._animation_amplifier = ((Component) this.effect_amplifier).GetComponent<SpriteAnimation>();
    this._animation_amplifier_bad = ((Component) this.effect_locus_amplifier_bad).GetComponent<SpriteAnimation>();
  }

  protected override void Update()
  {
    base.Update();
    if (!this.isAmplifier())
      return;
    if (((Behaviour) this._animation_amplifier).isActiveAndEnabled)
      this._animation_amplifier.update(Time.deltaTime);
    if (!((Behaviour) this._animation_amplifier_bad).isActiveAndEnabled)
      return;
    this._animation_amplifier_bad.update(Time.deltaTime);
  }

  private void click()
  {
    if (!this.gene.can_drop_and_grab)
      return;
    this._locus_click_event(this);
    this.checkSprite();
    if (InputHelpers.mouseSupported)
      return;
    ((Component) this).GetComponent<TipButton>().hoverAction();
  }

  private void clearLocus()
  {
    this._locus_click_event((LocusElement) null);
    this.checkSprite();
  }

  private void checkSprite()
  {
    int num = this.isEmptyLocus() ? 1 : 0;
    bool flag1 = this.isAmplifier();
    bool flag2 = num == 0 && !flag1;
    bool bad = this._chromosome.isNextToBad(this._locus_x, this._locus_y);
    if (flag1)
    {
      if (bad)
      {
        ((Component) this.effect_amplifier).gameObject.SetActive(false);
        ((Component) this.effect_locus_amplifier_bad).gameObject.SetActive(true);
      }
      else
      {
        ((Component) this.effect_amplifier).gameObject.SetActive(true);
        ((Component) this.effect_locus_amplifier_bad).gameObject.SetActive(false);
      }
    }
    else
    {
      ((Component) this.effect_amplifier).gameObject.SetActive(false);
      ((Component) this.effect_locus_amplifier_bad).gameObject.SetActive(false);
    }
    this.sprite_background.sprite = !this.shouldBeBadLocus() ? (!this.shouldBeGoldenLocus() ? this.sprite_locus_bg_normal : this.sprite_locus_bg_synergy) : this.sprite_locus_bg_bad;
    ((Component) this.sprite_background).gameObject.SetActive(flag2);
    this.checkChainsColors();
    if ((num | (flag1 ? 1 : 0)) != 0)
    {
      ((Component) this._gene_button).gameObject.SetActive(false);
    }
    else
    {
      ((Component) this._gene_button).gameObject.SetActive(true);
      this._gene_button.load(this.gene);
      this._gene_button.is_editor_button = true;
      // ISSUE: method pointer
      this._gene_button.locusChild(new UnityAction((object) this, __methodptr(click)), this.locus_index);
    }
    if (this.isAmplifier())
      ((Component) this).transform.localScale = new Vector3(this._super_size, this._super_size, this._super_size);
    else
      ((Component) this).transform.localScale = new Vector3(this._normal_size, this._normal_size, this._normal_size);
    ((Component) this).GetComponent<TipButton>().setDefaultScale(((Component) this).transform.localScale);
  }

  private bool shouldBeBadChainSide(int pX, int pY, int pOffsetX, int pOffsetY)
  {
    return this.shouldBeBadChain(pX, pY, pX + pOffsetX, pY + pOffsetY);
  }

  private bool shouldBeBadChain(int pX, int pY, int pToX, int pToY)
  {
    if (this.gene.is_bad)
      return true;
    GeneAsset geneAt = this._chromosome.getGeneAt(pToX, pToY);
    return geneAt != null && geneAt.is_bad || this._chromosome.hasAmplifierBad(pX, pY) || this._chromosome.hasAmplifierBad(pToX, pToY);
  }

  private void checkChainsColors()
  {
    int locusX = this._locus_x;
    int locusY = this._locus_y;
    Chromosome chromosome = this._chromosome;
    GeneAsset geneLeft = chromosome.getGeneLeft(locusX, locusY);
    GeneAsset geneRight = chromosome.getGeneRight(locusX, locusY);
    GeneAsset geneUp = chromosome.getGeneUp(locusX, locusY);
    GeneAsset geneDown = chromosome.getGeneDown(locusX, locusY);
    bool flag1 = !chromosome.hasBoundLeft(locusX, locusY);
    bool flag2 = !chromosome.hasBoundRight(locusX, locusY);
    bool flag3 = !chromosome.hasBoundUp(locusX, locusY);
    bool flag4 = !chromosome.hasBoundDown(locusX, locusY);
    bool flag5 = chromosome.hasSynergyConnectionLeft(locusX, locusY);
    bool flag6 = chromosome.hasSynergyConnectionRight(locusX, locusY);
    bool flag7 = chromosome.hasSynergyConnectionUp(locusX, locusY);
    bool flag8 = chromosome.hasSynergyConnectionDown(locusX, locusY);
    if (!flag5)
      this.hideChain(this.chain_left);
    else if (this.shouldBeBadChain(locusX, locusY, locusX - 1, locusY))
      this.showChain(this.chain_left, true, this.gene.genetic_code_left, new Color?(NucleobaseHelper.color_bad));
    else if (chromosome.isForcedSynergyLeft(locusX, locusY))
      this.showChain(this.chain_left, true, this.gene.genetic_code_left);
    else
      this.showChain(this.chain_left, true, geneLeft.genetic_code_right);
    if (!flag6)
      this.hideChain(this.chain_right);
    else if (this.shouldBeBadChain(locusX, locusY, locusX + 1, locusY))
      this.showChain(this.chain_right, true, this.gene.genetic_code_right, new Color?(NucleobaseHelper.color_bad));
    else if (chromosome.isForcedSynergyRight(locusX, locusY))
      this.showChain(this.chain_right, true, this.gene.genetic_code_right);
    else
      this.showChain(this.chain_right, true, geneRight.genetic_code_left);
    if (!flag7)
      this.hideChain(this.chain_up);
    else if (this.shouldBeBadChain(locusX, locusY, locusX, locusY - 1))
      this.showChain(this.chain_up, true, this.gene.genetic_code_up, new Color?(NucleobaseHelper.color_bad));
    else if (chromosome.isForcedSynergyUp(locusX, locusY))
      this.showChain(this.chain_up, true, this.gene.genetic_code_up);
    else
      this.showChain(this.chain_up, true, geneUp.genetic_code_down);
    if (!flag8)
      this.hideChain(this.chain_down);
    else if (this.shouldBeBadChain(locusX, locusY, locusX, locusY + 1))
      this.showChain(this.chain_down, true, this.gene.genetic_code_down, new Color?(NucleobaseHelper.color_bad));
    else if (chromosome.isForcedSynergyDown(locusX, locusY))
      this.showChain(this.chain_down, true, this.gene.genetic_code_down);
    else
      this.showChain(this.chain_down, true, geneDown.genetic_code_up);
    this.showDot(this._dot_left, flag1 && !flag5, this.gene.genetic_code_left);
    this.showDot(this._dot_right, flag2 && !flag6, this.gene.genetic_code_right);
    this.showDot(this._dot_up, flag3 && !flag7, this.gene.genetic_code_up);
    this.showDot(this._dot_down, flag4 && !flag8, this.gene.genetic_code_down);
  }

  public override void load(GeneAsset pAsset)
  {
    throw new NotImplementedException("Use show instead");
  }

  internal override void load(string pElementID)
  {
    throw new NotImplementedException("Use show instead");
  }

  public void show(
    int pLocusIndex,
    Chromosome pChromosome,
    GeneAsset pGene,
    LocusType pLocusType,
    LocusClickEvent pLocusClickEvent)
  {
    base.load(pGene);
    this.clearActions();
    this._chromosome = pChromosome;
    this.locus_index = pLocusIndex;
    (int num1, int num2) = this._chromosome.getXYFromIndex(pLocusIndex);
    this._locus_x = num1;
    this._locus_y = num2;
    this._locus_click_event = pLocusClickEvent;
    this.locus_type = pLocusType;
    ((Object) ((Component) this).gameObject).name = "Locus " + this.gene.id;
    this.colorChains();
    this.checkSprite();
  }

  protected override void clearActions()
  {
    base.clearActions();
    this._chromosome_updated_event = (Action) null;
  }

  public bool shouldBeBadLocus()
  {
    return this.gene.is_bad | this._chromosome.isNextToBad(this._locus_x, this._locus_y);
  }

  public bool shouldBeGoldenLocus()
  {
    return this.isAmplifier() || this.gene.synergy_sides_always || this._chromosome.hasFullSynergy(this.locus_index);
  }

  public bool isAmplifier() => this.locus_type == LocusType.Amplifier;

  public bool isAmplifierBad() => this._chromosome.hasAmplifierBad(this._locus_x, this._locus_y);

  public bool isEmptyLocus() => this.locus_type == LocusType.Empty;

  protected override void fillTooltipData(GeneAsset pElement)
  {
    Tooltip.show((object) this, "gene", this.tooltipDataBuilder());
  }

  protected override TooltipData tooltipDataBuilder()
  {
    return new TooltipData()
    {
      gene = this.gene,
      locus = this,
      chromosome = this._chromosome
    };
  }

  public bool canAddGene() => this._chromosome.canAddToLocus(this.locus_index);

  public bool isSpecialLocus() => this._chromosome.isSpecialLocus(this.locus_index);

  public void OnDrop(PointerEventData pEventData)
  {
    if (Object.op_Equality((Object) pEventData.pointerDrag, (Object) null) || this.isAmplifier())
      return;
    if (!Config.hasPremium)
    {
      ScrollWindow.showWindow("premium_menu");
    }
    else
    {
      GeneButton component = pEventData.pointerDrag.GetComponent<GeneButton>();
      if (Object.op_Equality((Object) component, (Object) null))
        return;
      GeneAsset elementAsset = component.getElementAsset();
      if (!elementAsset.can_drop_and_grab)
        return;
      if (component.locus_index > -1)
        this._chromosome.setGene(this._chromosome.getGene(this.locus_index), component.locus_index);
      GeneAsset geneAsset = this.getGeneAsset();
      this._chromosome.setGene(elementAsset, this.locus_index);
      this._chromosome_updated_event();
      SelectedMetas.selected_subspecies.eventGMO();
      if (elementAsset != geneAsset)
        AchievementLibrary.engineered_evolution.check();
      this.fillTooltipData(this.gene);
    }
  }

  public void addChromosomeUpdatedEvent(Action pChromosomeUpdatedEvent)
  {
    this._chromosome_updated_event = pChromosomeUpdatedEvent;
  }

  protected void showDot(LocusDot pChainDot, bool pShow, char pGeneticCode)
  {
    ((Component) pChainDot).gameObject.SetActive(pShow);
    if (!pShow)
      return;
    pChainDot.colorDot(pGeneticCode);
  }

  protected override void startSignal() => AchievementLibrary.genes_explorer.checkBySignal();

  protected override bool unlockElement()
  {
    int num = base.unlockElement() ? 1 : 0;
    this.isElementUnlocked();
    return num != 0;
  }
}
