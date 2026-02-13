// Decompiled with JetBrains decompiler
// Type: LanguageSelectedContainerTraits
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class LanguageSelectedContainerTraits : 
  SelectedContainerTraits<LanguageTrait, LanguageTraitButton, LanguageTraitsContainer, LanguageTraitsEditor>
{
  protected override MetaType meta_type => MetaType.Language;

  protected override IReadOnlyCollection<LanguageTrait> getTraits()
  {
    return SelectedMetas.selected_language.getTraits();
  }

  protected override bool canEditTraits() => true;
}
