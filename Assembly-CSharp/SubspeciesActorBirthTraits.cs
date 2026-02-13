// Decompiled with JetBrains decompiler
// Type: SubspeciesActorBirthTraits
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
public class SubspeciesActorBirthTraits : ITraitsOwner<ActorTrait>
{
  private ActorAsset _asset;
  private readonly HashSet<ActorTrait> _traits = new HashSet<ActorTrait>();
  private Subspecies _subspecies;

  public void init(ActorAsset pActorAsset, Subspecies pSubspecies)
  {
    this._asset = pActorAsset;
    this.setSubspecies(pSubspecies);
    if (this._asset.traits != null)
    {
      foreach (string trait in this._asset.traits)
        this.addTrait(trait);
    }
    if (!WorldLawLibrary.world_law_mutant_box.isEnabled())
      return;
    int num = Randy.randomInt(1, 4);
    for (int index = 0; index < num; ++index)
    {
      ActorTrait random = AssetManager.traits.pot_traits_mutation_box.GetRandom<ActorTrait>();
      if (random.isAvailable())
        this.addTrait(random, true);
    }
  }

  public void reset() => this._traits.Clear();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public IReadOnlyCollection<ActorTrait> getTraits()
  {
    return (IReadOnlyCollection<ActorTrait>) this._traits;
  }

  public bool hasTraits() => this._traits.Count > 0;

  public List<string> getTraitsAsStrings()
  {
    return Toolbox.getListForSave<ActorTrait>((IReadOnlyCollection<ActorTrait>) this._traits);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasTrait(ActorTrait pTrait) => this._traits.Contains(pTrait);

  internal bool hasOppositeTrait(ActorTrait pTrait) => pTrait.hasOppositeTrait(this._traits);

  public bool addTrait(string pTraitID, bool pRemoveOpposites = false)
  {
    ActorTrait pTrait = AssetManager.traits.get(pTraitID);
    return pTrait != null && this.addTrait(pTrait, pRemoveOpposites);
  }

  public bool addTrait(ActorTrait pTrait, bool pRemoveOpposites = false)
  {
    if (this.hasTrait(pTrait))
      return false;
    if (pTrait.traits_to_remove != null)
      this.removeTraits((ICollection<ActorTrait>) pTrait.traits_to_remove);
    if (pRemoveOpposites)
      this.removeOppositeTraits(pTrait);
    else if (this.hasOppositeTrait(pTrait))
      return false;
    this._traits.Add(pTrait);
    return true;
  }

  public bool removeTrait(ActorTrait pTrait) => this._traits.Remove(pTrait);

  public void removeTraits(ICollection<ActorTrait> pTraits)
  {
    foreach (ActorTrait pTrait in (IEnumerable<ActorTrait>) pTraits)
      this._traits.Remove(pTrait);
  }

  private void removeOppositeTraits(ActorTrait pTrait)
  {
    if (!pTrait.hasOppositeTraits<ActorTrait>())
      return;
    this.removeTraits((ICollection<ActorTrait>) pTrait.opposite_traits);
  }

  public void sortTraits(IReadOnlyCollection<ActorTrait> pTraits)
  {
    if (!this._traits.SetEquals((IEnumerable<ActorTrait>) pTraits))
      return;
    this._traits.Clear();
    foreach (ActorTrait pTrait in (IEnumerable<ActorTrait>) pTraits)
      this._traits.Add(pTrait);
  }

  public void traitModifiedEvent()
  {
  }

  public void fillTraitAssetsFromStringList(IEnumerable<string> pList)
  {
    this._traits.Clear();
    if (pList == null)
      return;
    foreach (string p in pList)
    {
      ActorTrait actorTrait = AssetManager.traits.get(p);
      if (actorTrait != null)
        this._traits.Add(actorTrait);
    }
  }

  public ActorAsset getActorAsset() => this._asset;

  public void setSubspecies(Subspecies pSubspecies) => this._subspecies = pSubspecies;
}
