// Decompiled with JetBrains decompiler
// Type: ITraitsOwner`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public interface ITraitsOwner<TTrait> where TTrait : BaseTrait<TTrait>
{
  bool hasTrait(TTrait pTraitId);

  bool addTrait(TTrait pTraitId, bool pRemoveOpposites = false);

  bool removeTrait(TTrait pTrait);

  IReadOnlyCollection<TTrait> getTraits();

  bool hasTraits();

  void sortTraits(IReadOnlyCollection<TTrait> pTraits);

  void traitModifiedEvent();

  ActorAsset getActorAsset();
}
