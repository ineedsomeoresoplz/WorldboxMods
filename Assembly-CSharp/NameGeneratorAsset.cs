// Decompiled with JetBrains decompiler
// Type: NameGeneratorAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
[Serializable]
public class NameGeneratorAsset : Asset
{
  public static string[] vowels_all = new string[6]
  {
    "a",
    "e",
    "i",
    "o",
    "u",
    "y"
  };
  public static string[] consonants_all = new string[20]
  {
    "b",
    "c",
    "d",
    "f",
    "g",
    "h",
    "j",
    "k",
    "l",
    "m",
    "n",
    "p",
    "q",
    "r",
    "s",
    "t",
    "v",
    "w",
    "x",
    "z"
  };
  public static string[] consonants_sounds = new string[19]
  {
    "b",
    "c",
    "d",
    "f",
    "g",
    "h",
    "ph",
    "ch",
    "k",
    "l",
    "m",
    "n",
    "p",
    "r",
    "s",
    "t",
    "v",
    "w",
    "sh"
  };
  public string[] special1;
  public string[] special2;
  public string[] vowels;
  public string[] consonants;
  public string[] parts;
  public string[] addition_start;
  public string[] addition_ending;
  public List<string> onomastics_templates = new List<string>();
  public List<string> part_groups;
  public List<string> part_groups2;
  public List<string> part_groups3;
  public Dictionary<string, string> dict_parts;
  public bool use_dictionary;
  public List<string[]> templates;
  [DefaultValue(1)]
  public int max_vowels_in_row = 1;
  [DefaultValue(1)]
  public int max_consonants_in_row = 1;
  [DefaultValue(0.5f)]
  public float add_addition_chance = 0.5f;
  public NameGeneratorCheck check;
  public NameGeneratorReplacer replacer;
  public NameGeneratorReplacerKingdom replacer_kingdom;
  public NameGeneratorFinalizer finalizer;

  public bool hasOnomastics() => this.onomastics_templates.Count > 0;

  public void addOnomastic(string pOnomastic)
  {
    if (this.onomastics_templates == null)
      this.onomastics_templates = new List<string>();
    this.onomastics_templates.Add(pOnomastic);
  }

  public void addTemplate(string pTemplateString)
  {
    if (this.templates == null)
      this.templates = new List<string[]>();
    this.templates.Add(pTemplateString.Split(',', StringSplitOptions.None));
  }

  public void addPartGroup(string pGroupString)
  {
    if (this.part_groups == null)
      this.part_groups = new List<string>();
    this.part_groups.Add(pGroupString);
  }

  public void addPartGroup2(string pGroupString)
  {
    if (this.part_groups2 == null)
      this.part_groups2 = new List<string>();
    this.part_groups2.Add(pGroupString);
  }

  public void addPartGroup3(string pGroupString)
  {
    if (this.part_groups3 == null)
      this.part_groups3 = new List<string>();
    this.part_groups3.Add(pGroupString);
  }

  public void addDictPart(string pID, string pListString)
  {
    if (this.dict_parts == null)
      this.dict_parts = new Dictionary<string, string>();
    this.dict_parts.Add(pID, pListString);
  }
}
