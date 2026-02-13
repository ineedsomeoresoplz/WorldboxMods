// Decompiled with JetBrains decompiler
// Type: GameProgressData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
[Serializable]
public class GameProgressData
{
  [NonSerialized]
  internal List<HashSet<string>> all_hashsets = new List<HashSet<string>>();
  public HashSet<string> achievements;
  public HashSet<string> unlocked_traits_actor;
  public HashSet<string> unlocked_traits_culture;
  public HashSet<string> unlocked_traits_language;
  public HashSet<string> unlocked_traits_subspecies;
  public HashSet<string> unlocked_traits_clan;
  public HashSet<string> unlocked_traits_kingdom;
  public HashSet<string> unlocked_traits_religion;
  public HashSet<string> unlocked_equipment;
  public HashSet<string> unlocked_genes;
  public HashSet<string> unlocked_actors;
  public HashSet<string> unlocked_plots;
  public int saveVersion = 2;

  public GameProgressData() => this.setDefaultValues();

  public void setDefaultValues()
  {
    if (this.achievements == null)
      this.achievements = new HashSet<string>();
    if (this.unlocked_traits_actor == null)
      this.unlocked_traits_actor = new HashSet<string>();
    if (this.unlocked_traits_culture == null)
      this.unlocked_traits_culture = new HashSet<string>();
    if (this.unlocked_traits_language == null)
      this.unlocked_traits_language = new HashSet<string>();
    if (this.unlocked_traits_subspecies == null)
      this.unlocked_traits_subspecies = new HashSet<string>();
    if (this.unlocked_traits_clan == null)
      this.unlocked_traits_clan = new HashSet<string>();
    if (this.unlocked_traits_kingdom == null)
      this.unlocked_traits_kingdom = new HashSet<string>();
    if (this.unlocked_traits_religion == null)
      this.unlocked_traits_religion = new HashSet<string>();
    if (this.unlocked_equipment == null)
      this.unlocked_equipment = new HashSet<string>();
    if (this.unlocked_genes == null)
      this.unlocked_genes = new HashSet<string>();
    if (this.unlocked_actors == null)
      this.unlocked_actors = new HashSet<string>();
    if (this.unlocked_plots != null)
      return;
    this.unlocked_plots = new HashSet<string>();
  }

  public void prepare()
  {
    this.all_hashsets.Clear();
    this.all_hashsets.Add(this.unlocked_traits_actor);
    this.all_hashsets.Add(this.unlocked_traits_culture);
    this.all_hashsets.Add(this.unlocked_traits_language);
    this.all_hashsets.Add(this.unlocked_traits_subspecies);
    this.all_hashsets.Add(this.unlocked_traits_clan);
    this.all_hashsets.Add(this.unlocked_traits_kingdom);
    this.all_hashsets.Add(this.unlocked_traits_religion);
    this.all_hashsets.Add(this.unlocked_equipment);
    this.all_hashsets.Add(this.unlocked_genes);
    this.all_hashsets.Add(this.unlocked_actors);
    this.all_hashsets.Add(this.unlocked_plots);
  }

  [Preserve]
  [Obsolete("use .unlocked_traits_actor instead", true)]
  public List<string> unlocked_traits
  {
    set
    {
      if (value == null)
        return;
      if (this.unlocked_traits_actor == null)
        this.unlocked_traits_actor = new HashSet<string>((IEnumerable<string>) value);
      else
        this.unlocked_traits_actor.UnionWith((IEnumerable<string>) value);
    }
  }
}
