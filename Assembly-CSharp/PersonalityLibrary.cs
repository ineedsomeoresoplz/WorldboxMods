// Decompiled with JetBrains decompiler
// Type: PersonalityLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class PersonalityLibrary : AssetLibrary<PersonalityAsset>
{
  public override void init()
  {
    base.init();
    PersonalityAsset pAsset1 = new PersonalityAsset();
    pAsset1.id = "administrator";
    this.add(pAsset1);
    this.t.base_stats["personality_diplomatic"] = 0.1f;
    this.t.base_stats["personality_administration"] = 0.5f;
    this.t.base_stats["personality_aggression"] = 0.1f;
    PersonalityAsset pAsset2 = new PersonalityAsset();
    pAsset2.id = "militarist";
    this.add(pAsset2);
    this.t.base_stats["personality_diplomatic"] = 0.05f;
    this.t.base_stats["personality_administration"] = 0.1f;
    this.t.base_stats["personality_aggression"] = 0.5f;
    PersonalityAsset pAsset3 = new PersonalityAsset();
    pAsset3.id = "diplomat";
    this.add(pAsset3);
    this.t.base_stats["personality_diplomatic"] = 0.5f;
    this.t.base_stats["personality_aggression"] = 0.05f;
    this.t.base_stats["personality_administration"] = 0.2f;
    PersonalityAsset pAsset4 = new PersonalityAsset();
    pAsset4.id = "balanced";
    this.add(pAsset4);
    this.t.base_stats["personality_administration"] = 0.1f;
    this.t.base_stats["personality_diplomatic"] = 0.1f;
    this.t.base_stats["personality_aggression"] = 0.1f;
    PersonalityAsset pAsset5 = new PersonalityAsset();
    pAsset5.id = "wildcard";
    this.add(pAsset5);
    this.t.base_stats["personality_rationality"] = -1f;
  }

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (PersonalityAsset pAsset in this.list)
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
  }
}
