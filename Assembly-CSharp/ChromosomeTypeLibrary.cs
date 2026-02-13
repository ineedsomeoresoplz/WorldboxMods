// Decompiled with JetBrains decompiler
// Type: ChromosomeTypeLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class ChromosomeTypeLibrary : AssetLibrary<ChromosomeTypeAsset>
{
  public const int LOCI_PER_ROW = 6;

  public override void init()
  {
    base.init();
    ChromosomeTypeAsset pAsset1 = new ChromosomeTypeAsset();
    pAsset1.id = "chromosome_big";
    pAsset1.amount_loci_min_amplifier = 1;
    pAsset1.amount_loci_max_amplifier = 4;
    pAsset1.amount_loci_min_empty = 4;
    pAsset1.amount_loci_max_empty = 8;
    pAsset1.amount_loci = 30;
    pAsset1.name = "chromosome_big";
    pAsset1.description = "some chromosome";
    this.add(pAsset1);
    ChromosomeTypeAsset pAsset2 = new ChromosomeTypeAsset();
    pAsset2.id = "chromosome_medium";
    pAsset2.amount_loci_min_amplifier = 1;
    pAsset2.amount_loci_max_amplifier = 3;
    pAsset2.amount_loci_min_empty = 3;
    pAsset2.amount_loci_max_empty = 5;
    pAsset2.amount_loci = 24;
    pAsset2.name = "chromosome_medium";
    pAsset2.description = "some chromosome";
    this.add(pAsset2);
    ChromosomeTypeAsset pAsset3 = new ChromosomeTypeAsset();
    pAsset3.id = "chromosome_small";
    pAsset3.amount_loci_min_amplifier = 1;
    pAsset3.amount_loci_max_amplifier = 3;
    pAsset3.amount_loci_min_empty = 2;
    pAsset3.amount_loci_max_empty = 4;
    pAsset3.amount_loci = 18;
    pAsset3.name = "chromosome_small";
    pAsset3.description = "some chromosome";
    this.add(pAsset3);
    ChromosomeTypeAsset pAsset4 = new ChromosomeTypeAsset();
    pAsset4.id = "chromosome_tiny";
    pAsset4.amount_loci_min_amplifier = 0;
    pAsset4.amount_loci_max_amplifier = 2;
    pAsset4.amount_loci_min_empty = 1;
    pAsset4.amount_loci_max_empty = 3;
    pAsset4.amount_loci = 12;
    pAsset4.name = "chromosome_tiny";
    pAsset4.description = "some chromosome";
    this.add(pAsset4);
  }

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (ChromosomeTypeAsset pAsset in this.list)
    {
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
      this.checkLocale((Asset) pAsset, pAsset.getDescriptionID());
    }
  }
}
