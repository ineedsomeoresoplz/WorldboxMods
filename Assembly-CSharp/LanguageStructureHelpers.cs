// Decompiled with JetBrains decompiler
// Type: LanguageStructureHelpers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public static class LanguageStructureHelpers
{
  public const int AMOUNT_TYPES = 7;
  public static readonly WordType[] word_types = new WordType[7]
  {
    WordType.Name,
    WordType.Place,
    WordType.Concept,
    WordType.Action,
    WordType.Object,
    WordType.Creature,
    WordType.Pronoun
  };
  public static readonly string[] possible_word_patterns = new string[12]
  {
    "S",
    "S",
    "S",
    "SE",
    "SE",
    "SE",
    "SE",
    "SE",
    "SE",
    "SME",
    "SME",
    "SMME"
  };
  public static readonly string[] possible_article_separators = new string[12]
  {
    "",
    " ",
    " ",
    " ",
    " ",
    " ",
    " ",
    " ",
    " ",
    "-",
    "'",
    "’"
  };
}
