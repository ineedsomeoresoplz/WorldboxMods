// Decompiled with JetBrains decompiler
// Type: AugmentationEditorButton`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class AugmentationEditorButton<TAugmentationButton, TAugmentation> : MonoBehaviour
  where TAugmentationButton : AugmentationButton<TAugmentation>
  where TAugmentation : BaseAugmentationAsset
{
  public TAugmentationButton augmentation_button;
  public Image selected_icon;
}
