// Decompiled with JetBrains decompiler
// Type: SaveSlotWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SaveSlotWindow : MonoBehaviour
{
  public GameObject buttonsContainer;
  private List<BoxPreview> previews = new List<BoxPreview>();
  public GameObject slotButtonPrefabNew;
  public ScrollRect scroll_rect;

  private void checkChildren()
  {
    if (this.previews.Count > 0)
      return;
    this.previews.AddRange((IEnumerable<BoxPreview>) this.buttonsContainer.GetComponentsInChildren<BoxPreview>());
  }

  private void OnEnable()
  {
    this.checkChildren();
    this.prepareLoadPreviews();
  }

  private void prepareLoadPreviews()
  {
    SaveManager.clearCurrentSelectedWorld();
    for (int index = 0; index < this.previews.Count; ++index)
      this.previews[index].setSlot(index + 1);
  }
}
