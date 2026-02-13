// Decompiled with JetBrains decompiler
// Type: SpellHolder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class SpellHolder
{
  private List<SpellAsset> _spells = new List<SpellAsset>();
  private HashSet<SpellAsset> _spells_hashset = new HashSet<SpellAsset>();

  public bool hasAny() => this._spells.Count > 0;

  public SpellAsset getRandomSpell() => this._spells.GetRandom<SpellAsset>();

  public bool hasSpell(SpellAsset pSpell) => this._spells_hashset.Contains(pSpell);

  public void reset()
  {
    this._spells.Clear();
    this._spells_hashset.Clear();
  }

  public void mergeWith(SpellHolder pListSpells) => this.mergeWith(pListSpells.spells);

  public void mergeWith(IReadOnlyList<SpellAsset> pSpells)
  {
    this._spells.AddRange((IEnumerable<SpellAsset>) pSpells);
    this._spells_hashset.UnionWith((IEnumerable<SpellAsset>) pSpells);
  }

  public void mergeWith(List<string> pSpellIDs)
  {
    foreach (string pSpellId in pSpellIDs)
    {
      SpellAsset pSpell = AssetManager.spells.get(pSpellId);
      if (pSpell != null)
        this.addSpell(pSpell);
    }
  }

  public void addSpell(SpellAsset pSpell)
  {
    this._spells.Add(pSpell);
    this._spells_hashset.Add(pSpell);
  }

  public IReadOnlyCollection<SpellAsset> getSpellsHashset()
  {
    return (IReadOnlyCollection<SpellAsset>) this._spells_hashset;
  }

  public IReadOnlyList<SpellAsset> spells => (IReadOnlyList<SpellAsset>) this._spells;
}
