// Decompiled with JetBrains decompiler
// Type: KingdomSelectedContainerTraits
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class KingdomSelectedContainerTraits : 
  SelectedContainerTraits<KingdomTrait, KingdomTraitButton, KingdomTraitsContainer, KingdomTraitsEditor>
{
  protected override MetaType meta_type => MetaType.Kingdom;

  protected override IReadOnlyCollection<KingdomTrait> getTraits()
  {
    return SelectedMetas.selected_kingdom.getTraits();
  }

  protected override bool canEditTraits() => true;
}
