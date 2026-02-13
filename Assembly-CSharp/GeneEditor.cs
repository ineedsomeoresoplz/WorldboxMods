// Decompiled with JetBrains decompiler
// Type: GeneEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class GeneEditor : MonoBehaviour
{
  [SerializeField]
  private Text _text_unlocked_genes;
  [SerializeField]
  private Transform _transform_chromosomes;
  [SerializeField]
  private Transform _transform_loci;
  [SerializeField]
  private Transform _transform_gene_selector;
  [SerializeField]
  private ChromosomeElement _prefab_chromosome_element;
  [SerializeField]
  private LocusElement _prefab_locus_element;
  [SerializeField]
  private GeneButton _prefab_gene_button;
  private bool _initialized;
  private Dictionary<GeneAsset, GeneButton> _dictionary_gene_buttons = new Dictionary<GeneAsset, GeneButton>();
  private ObjectPoolGenericMono<ChromosomeElement> _pool_elements_chromosomes;
  private ObjectPoolGenericMono<LocusElement> _pool_elements_loci;
  private LocusElement _selected_locus;
  private Chromosome _selected_chromosome;
  public Image selection_locus;
  public Image selection_gene_asset;
  public Text genome_counter_text;
  private SubspeciesWindow _window_subspecies;

  private Subspecies _meta_object => SelectedMetas.selected_subspecies;

  internal void load()
  {
    this.init();
    this.clear();
    this.loadChromosomes();
    this.reloadButtons();
    this.recolorGenePoolButtons();
  }

  private void init()
  {
    if (this._initialized)
      return;
    this._initialized = true;
    this._window_subspecies = ((Component) this).GetComponentInParent<SubspeciesWindow>();
    this._pool_elements_chromosomes = new ObjectPoolGenericMono<ChromosomeElement>(this._prefab_chromosome_element, this._transform_chromosomes);
    this._pool_elements_loci = new ObjectPoolGenericMono<LocusElement>(this._prefab_locus_element, this._transform_loci);
    this.loadGeneButtons();
  }

  private void clear()
  {
    this._pool_elements_chromosomes.clear();
    this._pool_elements_loci.clear();
    this._selected_chromosome = (Chromosome) null;
    this._selected_locus = (LocusElement) null;
  }

  private void OnEnable() => this.load();

  private void OnDisable() => this.clear();

  public void debugRandomizeGenes()
  {
    this._meta_object.addDNAMutationToSeed();
    this._meta_object.generateNucleus();
    this._meta_object.genesChangedEvent();
    this._meta_object.eventGMO();
    this.load();
  }

  public void debugShuffleGenes()
  {
    this._meta_object.unstableGenomeEvent();
    this.load();
  }

  private void loadChromosomes(bool pSelectFirstChromosome = true)
  {
    foreach (Chromosome chromosome in this._meta_object.nucleus.chromosomes)
      this._pool_elements_chromosomes.getNext().show(chromosome, new ChromosomeClickEvent(this.clickChromosome));
    if (!pSelectFirstChromosome || this._meta_object.nucleus.chromosomes.Count <= 0)
      return;
    this.clickChromosome(this._meta_object.nucleus.chromosomes[0]);
  }

  private void recolorGenePoolButtons()
  {
    foreach (ChainElement chainElement in this._dictionary_gene_buttons.Values)
      chainElement.colorChains();
  }

  private void loadGeneButtons()
  {
    foreach (GeneAsset geneAsset in AssetManager.gene_library.list)
    {
      if (!geneAsset.is_empty)
      {
        GeneButton geneButton = Object.Instantiate<GeneButton>(this._prefab_gene_button, this._transform_gene_selector);
        this._dictionary_gene_buttons.Add(geneAsset, geneButton);
        geneButton.load(geneAsset);
        geneButton.is_editor_button = true;
        geneButton.addElementUnlockedAction(new AugmentationUnlockedAction(this.reloadButtons));
        geneButton.addGeneClickCallback(new GeneAssetClickEvent(this.clickGeneAssetAction));
        ((Behaviour) ((Component) geneButton).GetComponent<DraggableLayoutElement>()).enabled = geneAsset.isAvailable();
      }
    }
  }

  public void clickChromosome(Chromosome pChromosome)
  {
    foreach (ChromosomeElement chromosomeElement in (IEnumerable<ChromosomeElement>) this._pool_elements_chromosomes.getListTotal())
    {
      if (((Component) chromosomeElement).gameObject.activeSelf)
      {
        if (chromosomeElement.chromosome == pChromosome)
          ((Graphic) chromosomeElement.image).color = Color.white;
        else
          ((Graphic) chromosomeElement.image).color = Color.gray;
      }
    }
    this._selected_chromosome = pChromosome;
    this.showGenes(pChromosome);
    this.selectFirstNormalLocus();
  }

  private void selectFirstNormalLocus()
  {
    foreach (LocusElement pElement in (IEnumerable<LocusElement>) this._pool_elements_loci.getListTotal())
    {
      if (!pElement.isSpecialLocus())
      {
        this.selectLocus(pElement);
        break;
      }
    }
  }

  internal void selectLocus(LocusElement pElement) => this._selected_locus = pElement;

  private void clickGeneAssetAction(GeneAsset pGeneAsset)
  {
    if (Object.op_Equality((Object) this._selected_locus, (Object) null) || !pGeneAsset.isAvailable())
      return;
    if (pGeneAsset != this._selected_locus.getGeneAsset())
      AchievementLibrary.engineered_evolution.check();
    if (!Config.hasPremium)
    {
      ScrollWindow.showWindow("premium_menu");
    }
    else
    {
      this._selected_chromosome.setGene(pGeneAsset, this._selected_locus.locus_index);
      this.chromosomeUpdatedEvent();
    }
  }

  private void chromosomeUpdatedEvent()
  {
    this._selected_chromosome.setDirty();
    this._selected_chromosome.recalculate();
    this._meta_object.genesChangedEvent();
    this._meta_object.eventGMO();
    this.showGenes(this._selected_chromosome);
    AchievementLibrary.simple_stupid_genetics.check();
    AchievementLibrary.fast_living.check();
    AchievementLibrary.long_living.check();
    AchievementLibrary.master_weaver.check();
    this._pool_elements_chromosomes.clear();
    this.loadChromosomes(false);
  }

  public void showGenes(Chromosome pChromosome)
  {
    this._pool_elements_loci.clear();
    for (int index = 0; index < pChromosome.genes.Count; ++index)
    {
      GeneAsset gene = pChromosome.genes[index];
      LocusElement next = this._pool_elements_loci.getNext();
      next.show(index, pChromosome, gene, pChromosome.getLocusType(index), new LocusClickEvent(this.selectLocus));
      next.addElementUnlockedAction(new AugmentationUnlockedAction(this.reloadButtons));
      next.addChromosomeUpdatedEvent(new Action(this.chromosomeUpdatedEvent));
    }
    this._window_subspecies.updateStats();
  }

  private void updateTextGenome()
  {
    int num = this._selected_chromosome.countNonEmpty();
    int amountLoci = this._selected_chromosome.getAsset().amount_loci;
    this.genome_counter_text.text = $"{num.ToString()} / {amountLoci.ToString()}";
  }

  private void Update()
  {
    if (this._meta_object == null || this._selected_chromosome == null)
      return;
    ((Component) this.selection_gene_asset).gameObject.SetActive(Object.op_Inequality((Object) this._selected_locus, (Object) null));
    ((Component) this.selection_locus).gameObject.SetActive(Object.op_Inequality((Object) this._selected_locus, (Object) null));
    if (!Object.op_Inequality((Object) this._selected_locus, (Object) null))
      return;
    ((Component) this.selection_locus).gameObject.transform.position = ((Component) this._selected_locus).transform.position;
    GeneButton currentGeneAssetButton = this.getCurrentGeneAssetButton();
    ((Component) this.selection_gene_asset).gameObject.transform.position = ((Component) currentGeneAssetButton).transform.position;
    if (Config.isDraggingItem())
      return;
    Object.op_Inequality((Object) currentGeneAssetButton, (Object) null);
  }

  private GeneButton getCurrentGeneAssetButton()
  {
    GeneAsset geneAsset = this._selected_locus.getGeneAsset();
    if (geneAsset == null)
      return (GeneButton) null;
    return this._dictionary_gene_buttons.ContainsKey(geneAsset) ? this._dictionary_gene_buttons[geneAsset] : (GeneButton) null;
  }

  private void reloadButtons()
  {
    int num1 = 0;
    int num2 = 0;
    foreach (GeneButton geneButton in this._dictionary_gene_buttons.Values)
    {
      bool flag = geneButton.getElementAsset().isAvailable();
      ++num2;
      if (flag)
      {
        ++num1;
        ((Graphic) geneButton.image).color = Toolbox.color_white;
      }
      else
        ((Graphic) geneButton.image).color = Toolbox.color_black;
      ((Behaviour) ((Component) geneButton).GetComponent<DraggableLayoutElement>()).enabled = flag;
    }
    this._text_unlocked_genes.text = $"{num1.ToString()}/{num2.ToString()}";
    AchievementLibrary.genes_explorer.checkBySignal();
  }

  protected virtual bool hasGene(GeneAsset pTrait) => this._selected_chromosome.hasGene(pTrait);
}
