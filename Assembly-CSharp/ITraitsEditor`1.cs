// Decompiled with JetBrains decompiler
// Type: ITraitsEditor`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public interface ITraitsEditor<TTrait> : IAugmentationsEditor where TTrait : BaseTrait<TTrait>
{
  ITraitsOwner<TTrait> getTraitsOwner();

  void scrollToGroupStarter(GameObject pTraitButton);

  void scrollToGroupStarter(GameObject pTraitButton, bool pIgnoreTooltipCheck);

  WindowMetaTab getEditorTab();
}
