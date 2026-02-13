// Decompiled with JetBrains decompiler
// Type: TaxonomyRowsContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine.Events;

#nullable disable
public class TaxonomyRowsContainer : StatsRowsContainer
{
  protected override void showStats()
  {
    this.showTaxonomicRank("taxonomy_kingdom");
    this.showTaxonomicRank("taxonomy_phylum");
    this.showTaxonomicRank("taxonomy_class");
    this.showTaxonomicRank("taxonomy_order");
    this.showTaxonomicRank("taxonomy_family");
    this.showTaxonomicRank("taxonomy_genus");
    StatsWindow.tryToShowMetaSpecies("species", SelectedMetas.selected_subspecies.data.species_id, (StatsRowsContainer) this);
  }

  private void showTaxonomicRank(string pTaxonomyRank)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TaxonomyRowsContainer.\u003C\u003Ec__DisplayClass1_0 cDisplayClass10 = new TaxonomyRowsContainer.\u003C\u003Ec__DisplayClass1_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass10.\u003C\u003E4__this = this;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass10.pTaxonomyRank = pTaxonomyRank;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass10.tSubspecies = SelectedMetas.selected_subspecies;
    // ISSUE: reference to a compiler-generated field
    string colorForTaxonomy = ColorStyleLibrary.m.getColorForTaxonomy(cDisplayClass10.pTaxonomyRank);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    string taxonomyRank = AssetManager.actor_library.get(cDisplayClass10.tSubspecies.data.species_id).getTaxonomyRank(cDisplayClass10.pTaxonomyRank);
    if (string.IsNullOrEmpty(taxonomyRank))
      return;
    string upper = Toolbox.firstLetterToUpper(taxonomyRank);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    cDisplayClass10.tField = this.showStatRow(cDisplayClass10.pTaxonomyRank, (object) upper, colorForTaxonomy, pColorText: true);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: method pointer
    cDisplayClass10.tField.on_hover_value = new UnityAction((object) cDisplayClass10, __methodptr(\u003CshowTaxonomicRank\u003Eb__0));
    // ISSUE: reference to a compiler-generated field
    // ISSUE: method pointer
    cDisplayClass10.tField.on_hover_value_out = new UnityAction((object) null, __methodptr(hideTooltip));
  }

  private void showTooltipTaxonomy(string pRankType, Subspecies pSpecies, KeyValueField pField)
  {
    TooltipData pData = new TooltipData()
    {
      subspecies = pSpecies,
      tip_name = pRankType
    };
    Tooltip.show((object) pField, "taxonomy", pData);
  }
}
