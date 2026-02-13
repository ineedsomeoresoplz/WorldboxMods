// Decompiled with JetBrains decompiler
// Type: ITraitWindow`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public interface ITraitWindow<TTrait, TTraitButton> : IAugmentationsWindow<ITraitsEditor<TTrait>>
  where TTrait : BaseTrait<TTrait>
  where TTraitButton : TraitButton<TTrait>
{
  TraitsContainer<TTrait, TTraitButton> getContainer()
  {
    return this.GetComponentInChildren<TraitsContainer<TTrait, TTraitButton>>();
  }

  void reloadTraits(bool pAnimated = true) => this.getContainer().reloadTraits(pAnimated);

  ITraitsOwner<TTrait> getTraitsOwner() => this.getEditor().getTraitsOwner();

  IReadOnlyCollection<TTrait> getTraits() => this.getTraitsOwner().getTraits();

  void sortTraits(IReadOnlyCollection<TTrait> pTraits) => this.getTraitsOwner().sortTraits(pTraits);

  bool hasTraits() => this.getTraitsOwner().hasTraits();
}
