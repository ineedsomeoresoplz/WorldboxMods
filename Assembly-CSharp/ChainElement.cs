// Decompiled with JetBrains decompiler
// Type: ChainElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
[RequireComponent(typeof (CanvasGroup))]
public class ChainElement : TraitButton<GeneAsset>
{
  public Image chain_left;
  public Image chain_right;
  public Image chain_up;
  public Image chain_down;
  internal int locus_index = -1;

  protected GeneAsset gene => this.augmentation_asset;

  public GeneAsset getGeneAsset() => this.gene;

  public override void load(GeneAsset pAsset)
  {
    base.load(pAsset);
    ((Object) ((Component) this).gameObject).name = this.gene.id;
    this.colorChains();
  }

  public void colorChains()
  {
    if (!this.gene.show_genepool_nucleobases)
    {
      this.hideChains();
    }
    else
    {
      this.showChain(this.chain_left, true, this.gene.genetic_code_left);
      this.showChain(this.chain_right, true, this.gene.genetic_code_right);
      this.showChain(this.chain_up, true, this.gene.genetic_code_up);
      this.showChain(this.chain_down, true, this.gene.genetic_code_down);
    }
  }

  protected void hideChains()
  {
    this.hideChain(this.chain_left);
    this.hideChain(this.chain_right);
    this.hideChain(this.chain_up);
    this.hideChain(this.chain_down);
  }

  protected void showChain(Image pChainImage, bool pShow, char pGeneticCode, Color? pColor = null)
  {
    ((Component) pChainImage).gameObject.SetActive(pShow);
    if (pColor.HasValue)
    {
      this.colorChain(pChainImage, pColor.Value);
    }
    else
    {
      if (!pShow)
        return;
      this.colorChain(pChainImage, NucleobaseHelper.getColor(pGeneticCode));
    }
  }

  protected void hideChain(Image pChain) => ((Component) pChain).gameObject.SetActive(false);

  protected void colorChain(Image pChain, Color pColor) => ((Graphic) pChain).color = pColor;
}
