// Decompiled with JetBrains decompiler
// Type: ActorSelectedContainerTraits
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class ActorSelectedContainerTraits : 
  SelectedContainerTraits<ActorTrait, ActorTraitButton, ActorTraitsContainer, ActorTraitsEditor>
{
  protected override MetaType meta_type => MetaType.Unit;

  protected override IReadOnlyCollection<ActorTrait> getTraits() => SelectedUnit.unit.getTraits();

  protected override bool canEditTraits() => SelectedUnit.unit.asset.can_edit_traits;
}
