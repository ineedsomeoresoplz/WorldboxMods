// Decompiled with JetBrains decompiler
// Type: TraitGroupElement`3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class TraitGroupElement<TTrait, TTraitButton, TTraitEditorButton> : 
  AugmentationCategory<TTrait, TTraitButton, TTraitEditorButton>
  where TTrait : BaseTrait<TTrait>
  where TTraitButton : TraitButton<TTrait>
  where TTraitEditorButton : TraitEditorButton<TTraitButton, TTrait>
{
  protected override bool isUnlocked(TTraitButton pButton)
  {
    return pButton.getElementAsset().isAvailable();
  }
}
