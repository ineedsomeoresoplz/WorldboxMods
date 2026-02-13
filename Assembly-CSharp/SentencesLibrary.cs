// Decompiled with JetBrains decompiler
// Type: SentencesLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class SentencesLibrary : AssetLibrary<SentenceAsset>
{
  public override void init()
  {
    SentenceAsset pAsset1 = new SentenceAsset();
    pAsset1.id = "declarative";
    this.add(pAsset1);
    this.t.addTemplate("pron_obj", "word_concept", "comma", "word_concept", "word_action", "word_concept", "word_creature");
    this.t.addTemplate("pron_obj", "word_concept", "pron_poss_adj", "word_place");
    this.t.addTemplate("pron_obj", "word_concept", "pron_poss_adj", "word_place");
    SentenceAsset pAsset2 = new SentenceAsset();
    pAsset2.id = "interrogative";
    this.add(pAsset2);
    SentenceAsset pAsset3 = new SentenceAsset();
    pAsset3.id = "imperative";
    this.add(pAsset3);
    SentenceAsset pAsset4 = new SentenceAsset();
    pAsset4.id = "exclamatory";
    this.add(pAsset4);
  }
}
