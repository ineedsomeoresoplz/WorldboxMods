// Decompiled with JetBrains decompiler
// Type: StoryLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class StoryLibrary : AssetLibrary<StoryAsset>
{
  public override void init()
  {
    StoryAsset pAsset = new StoryAsset();
    pAsset.id = "story_1";
    this.add(pAsset);
    this.t.addTemplate("pron_obj", "word_concept", "comma", "word_concept", "word_action", "word_concept", "word_creature", "period", "pron_obj", "word_concept", "pron_poss_adj", "word_place", "question_mark", "pron_obj", "word_concept", "pron_poss_adj", "word_place", "exclamation_mark");
  }

  public override void editorDiagnostic()
  {
    base.editorDiagnostic();
    if (!Config.editor_maxim)
      return;
    this.test();
  }

  private void test()
  {
    string[] randomTemplate = this.get("story_1").getRandomTemplate();
    for (int index = 0; index < 10; ++index)
    {
      Language pLanguage = new Language();
      LanguageStructure languageStructure = new LanguageStructure();
      pLanguage.data = new LanguageData();
      pLanguage.data.structure = languageStructure;
      string example1 = StoryLibrary.generateExample(pLanguage, randomTemplate);
      string example2 = StoryLibrary.generateExample(pLanguage, randomTemplate);
      string example3 = StoryLibrary.generateExample(pLanguage, randomTemplate);
      string str = $"S:{languageStructure.syllables_start.AsString<string>()}, |M:{languageStructure.syllables_mid.AsString<string>()}, |E:{languageStructure.syllables_ends.AsString<string>()}";
      Debug.Log((object) $"Example Language {$"{index} : "}{str}");
      Debug.Log((object) $"Example {1.ToString()}: {example1}");
      Debug.Log((object) $"Example {2.ToString()}: {example2}");
      Debug.Log((object) $"Example {3.ToString()}: {example3}");
    }
  }

  public static string getTestText(Language pLanguage)
  {
    string[] randomTemplate = AssetManager.story_library.get("story_1").getRandomTemplate();
    if (pLanguage.data.structure == null)
    {
      LanguageStructure languageStructure = new LanguageStructure();
      pLanguage.data.structure = languageStructure;
    }
    return StoryLibrary.generateExample(pLanguage, randomTemplate);
  }

  private static string generateExample(Language pLanguage, string[] pTemplate)
  {
    // ISSUE: unable to decompile the method.
  }
}
