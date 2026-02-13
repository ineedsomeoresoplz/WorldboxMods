// Decompiled with JetBrains decompiler
// Type: LinguisticsAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class LinguisticsAsset : Asset
{
  public string simple_text;
  public WordType word_type = WordType.None;
  public bool symbols_around;
  public string symbols_around_left;
  public string symbols_around_right;
  public bool add_space;
  public bool next_uppercase;
  public string[] array;
  public bool word_group;
  private List<string[]> _pot_patterns = new List<string[]>();

  public void addPattern(int pRate, params string[] pPattern)
  {
    this._pot_patterns.AddTimes<string[]>(pRate, pPattern);
  }

  public string getRandom() => this.array.GetRandom<string>();

  public string[] getRandomPattern()
  {
    return this._pot_patterns.Count == 0 ? (string[]) null : this._pot_patterns.GetRandom<string[]>();
  }

  public string getLocaleID() => throw new NotImplementedException();

  public string getDescriptionID() => throw new NotImplementedException();

  public string getDescriptionID2() => throw new NotImplementedException();
}
