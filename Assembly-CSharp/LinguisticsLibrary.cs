// Decompiled with JetBrains decompiler
// Type: LinguisticsLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class LinguisticsLibrary : AssetLibrary<LinguisticsAsset>
{
  private const string C = "C";
  private const string V = "V";

  public override void init()
  {
    this.addPronounce();
    this.addWordGroups();
    this.addUnique();
    this.addMain();
  }

  private void addPronounce()
  {
    LinguisticsAsset pAsset1 = new LinguisticsAsset();
    pAsset1.id = "pron_subj";
    pAsset1.word_group = true;
    pAsset1.add_space = true;
    pAsset1.array = AssetLibrary<LinguisticsAsset>.a<string>("pron_subj_i", "pron_subj_we", "pron_subj_you", "pron_subj_it", "pron_subj_they");
    this.add(pAsset1);
    LinguisticsAsset pAsset2 = new LinguisticsAsset();
    pAsset2.id = "pron_obj";
    pAsset2.word_group = true;
    pAsset2.add_space = true;
    pAsset2.array = AssetLibrary<LinguisticsAsset>.a<string>("pron_obj_me", "pron_obj_us", "pron_obj_you", "pron_obj_it", "pron_obj_them");
    this.add(pAsset2);
    LinguisticsAsset pAsset3 = new LinguisticsAsset();
    pAsset3.id = "pron_poss_adj";
    pAsset3.word_group = true;
    pAsset3.add_space = true;
    pAsset3.array = AssetLibrary<LinguisticsAsset>.a<string>("pron_poss_my", "pron_poss_our", "pron_poss_your", "pron_poss_its", "pron_poss_their");
    this.add(pAsset3);
    LinguisticsAsset pAsset4 = new LinguisticsAsset();
    pAsset4.id = "pron_posspr";
    pAsset4.word_group = true;
    pAsset4.add_space = true;
    pAsset4.array = AssetLibrary<LinguisticsAsset>.a<string>("pron_poss_mine", "pron_poss_ours", "pron_poss_yours", "pron_poss_theirs");
    this.add(pAsset4);
  }

  private void addWordGroups()
  {
    LinguisticsAsset pAsset1 = new LinguisticsAsset();
    pAsset1.id = "word_concept";
    pAsset1.word_group = true;
    pAsset1.add_space = true;
    pAsset1.word_type = WordType.Concept;
    pAsset1.array = AssetLibrary<LinguisticsAsset>.a<string>("concept_love", "concept_death", "concept_nature");
    this.add(pAsset1);
    LinguisticsAsset pAsset2 = new LinguisticsAsset();
    pAsset2.id = "word_action";
    pAsset2.word_group = true;
    pAsset2.add_space = true;
    pAsset2.word_type = WordType.Action;
    pAsset2.array = AssetLibrary<LinguisticsAsset>.a<string>("action_run", "action_walk", "action_fight");
    this.add(pAsset2);
    LinguisticsAsset pAsset3 = new LinguisticsAsset();
    pAsset3.id = "word_object";
    pAsset3.word_group = true;
    pAsset3.add_space = true;
    pAsset3.word_type = WordType.Object;
    pAsset3.array = AssetLibrary<LinguisticsAsset>.a<string>("object_sword", "object_shield", "object_bow");
    this.add(pAsset3);
    LinguisticsAsset pAsset4 = new LinguisticsAsset();
    pAsset4.id = "word_place";
    pAsset4.word_group = true;
    pAsset4.add_space = true;
    pAsset4.word_type = WordType.Place;
    pAsset4.array = AssetLibrary<LinguisticsAsset>.a<string>("place_forest", "place_mountain", "place_hill");
    this.add(pAsset4);
    LinguisticsAsset pAsset5 = new LinguisticsAsset();
    pAsset5.id = "word_creature";
    pAsset5.word_group = true;
    pAsset5.add_space = true;
    pAsset5.word_type = WordType.Creature;
    pAsset5.array = AssetLibrary<LinguisticsAsset>.a<string>("creature_king", "creature_queen", "creature_prince");
    this.add(pAsset5);
  }

  private void addMain()
  {
    LinguisticsAsset pAsset1 = new LinguisticsAsset();
    pAsset1.id = "vowel";
    pAsset1.array = AssetLibrary<LinguisticsAsset>.a<string>("a", "e", "i", "o", "u", "y");
    this.add(pAsset1);
    LinguisticsAsset pAsset2 = new LinguisticsAsset();
    pAsset2.id = "diphthongs";
    pAsset2.array = AssetLibrary<LinguisticsAsset>.a<string>("ai", "ei", "oi", "au", "ou", "ia", "io", "ua", "ue");
    this.add(pAsset2);
    LinguisticsAsset pAsset3 = new LinguisticsAsset();
    pAsset3.id = "consonant";
    pAsset3.array = AssetLibrary<LinguisticsAsset>.a<string>("p", "b", "t", "d", "k", "g", "f", "v", "s", "z", "h", "m", "n", "l", "r", "w", "y", "j");
    this.add(pAsset3);
    LinguisticsAsset pAsset4 = new LinguisticsAsset();
    pAsset4.id = "onset1";
    pAsset4.array = AssetLibrary<LinguisticsAsset>.a<string>("p", "b", "t", "d", "k", "g", "f", "v", "s", "z", "sh", "zh", "m", "n", "l", "r", "w", "y", "ch", "j");
    this.add(pAsset4);
    LinguisticsAsset pAsset5 = new LinguisticsAsset();
    pAsset5.id = "onset2";
    pAsset5.array = AssetLibrary<LinguisticsAsset>.a<string>("pr", "br", "tr", "dr", "kr", "gr", "fr", "vr", "shr", "thr", "pl", "bl", "kl", "gl", "fl", "vl", "tw", "dw", "kw", "gw", "sw", "sk", "st", "sp");
    this.add(pAsset5);
    LinguisticsAsset pAsset6 = new LinguisticsAsset();
    pAsset6.id = "coda1";
    pAsset6.array = AssetLibrary<LinguisticsAsset>.a<string>("n", "m", "l", "r", "s", "sh", "z");
    this.add(pAsset6);
    LinguisticsAsset pAsset7 = new LinguisticsAsset();
    pAsset7.id = "coda2";
    pAsset7.array = AssetLibrary<LinguisticsAsset>.a<string>("nd", "nt", "nk", "mp", "lt", "ld", "lp", "lf", "rk", "rt", "rs", "rz", "st", "sk");
    this.add(pAsset7);
    LinguisticsAsset pAsset8 = new LinguisticsAsset();
    pAsset8.id = "syllable_starts";
    this.add(pAsset8);
    this.t.addPattern(50, "C", "V");
    this.t.addPattern(25, "C", "C", "V");
    this.t.addPattern(20, "C", "V", "C");
    this.t.addPattern(5, "V");
    LinguisticsAsset pAsset9 = new LinguisticsAsset();
    pAsset9.id = "syllable_mids";
    this.add(pAsset9);
    this.t.addPattern(40, "C", "V");
    this.t.addPattern(30, "C", "V", "C");
    this.t.addPattern(10, "C", "C", "V");
    this.t.addPattern(20, "V");
    LinguisticsAsset pAsset10 = new LinguisticsAsset();
    pAsset10.id = "syllable_ends";
    this.add(pAsset10);
    this.t.addPattern(40, "C", "V", "C");
    this.t.addPattern(30, "V", "C");
    this.t.addPattern(20, "C", "V");
    this.t.addPattern(10, "V");
  }

  private void addUnique()
  {
    LinguisticsAsset pAsset1 = new LinguisticsAsset();
    pAsset1.id = "comma";
    pAsset1.simple_text = ",";
    this.add(pAsset1);
    LinguisticsAsset pAsset2 = new LinguisticsAsset();
    pAsset2.id = "period";
    pAsset2.simple_text = ".";
    pAsset2.next_uppercase = true;
    this.add(pAsset2);
    LinguisticsAsset pAsset3 = new LinguisticsAsset();
    pAsset3.id = "semicolon";
    pAsset3.simple_text = ";";
    pAsset3.next_uppercase = true;
    this.add(pAsset3);
    LinguisticsAsset pAsset4 = new LinguisticsAsset();
    pAsset4.id = "colon";
    pAsset4.simple_text = ":";
    this.add(pAsset4);
    LinguisticsAsset pAsset5 = new LinguisticsAsset();
    pAsset5.id = "dash";
    pAsset5.add_space = true;
    pAsset5.simple_text = "—";
    this.add(pAsset5);
    LinguisticsAsset pAsset6 = new LinguisticsAsset();
    pAsset6.id = "hyphen";
    pAsset6.simple_text = "-";
    this.add(pAsset6);
    LinguisticsAsset pAsset7 = new LinguisticsAsset();
    pAsset7.id = "ellipsis";
    pAsset7.simple_text = "...";
    pAsset7.next_uppercase = true;
    this.add(pAsset7);
    LinguisticsAsset pAsset8 = new LinguisticsAsset();
    pAsset8.id = "question_mark";
    pAsset8.simple_text = "?";
    pAsset8.next_uppercase = true;
    this.add(pAsset8);
    LinguisticsAsset pAsset9 = new LinguisticsAsset();
    pAsset9.id = "exclamation_mark";
    pAsset9.simple_text = "!";
    pAsset9.next_uppercase = true;
    this.add(pAsset9);
    LinguisticsAsset pAsset10 = new LinguisticsAsset();
    pAsset10.id = "space";
    pAsset10.simple_text = " ";
    this.add(pAsset10);
    LinguisticsAsset pAsset11 = new LinguisticsAsset();
    pAsset11.id = "quotation_marks";
    pAsset11.symbols_around = true;
    pAsset11.add_space = true;
    pAsset11.symbols_around_left = "“";
    pAsset11.symbols_around_right = "”";
    this.add(pAsset11);
    LinguisticsAsset pAsset12 = new LinguisticsAsset();
    pAsset12.id = "parentheses";
    pAsset12.symbols_around = true;
    pAsset12.add_space = true;
    pAsset12.symbols_around_left = "(";
    pAsset12.symbols_around_right = ")";
    this.add(pAsset12);
    LinguisticsAsset pAsset13 = new LinguisticsAsset();
    pAsset13.id = "brackets";
    pAsset13.symbols_around = true;
    pAsset13.add_space = true;
    pAsset13.symbols_around_left = "[";
    pAsset13.symbols_around_right = "]";
    this.add(pAsset13);
    LinguisticsAsset pAsset14 = new LinguisticsAsset();
    pAsset14.id = "braces";
    pAsset14.symbols_around = true;
    pAsset14.add_space = true;
    pAsset14.symbols_around_left = "{";
    pAsset14.symbols_around_right = "}";
    this.add(pAsset14);
    LinguisticsAsset pAsset15 = new LinguisticsAsset();
    pAsset15.id = "apostrophe";
    pAsset15.add_space = true;
    pAsset15.simple_text = "'";
    this.add(pAsset15);
  }
}
