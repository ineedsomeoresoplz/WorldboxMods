// Decompiled with JetBrains decompiler
// Type: AugmentationCategory`3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class AugmentationCategory<TAugmentation, TAugmentationButton, TAugmentationEditorButton> : 
  MonoBehaviour
  where TAugmentation : BaseAugmentationAsset
  where TAugmentationButton : AugmentationButton<TAugmentation>
  where TAugmentationEditorButton : AugmentationEditorButton<TAugmentationButton, TAugmentation>
{
  public Text title;
  public Text counter;
  public RectTransform height;
  public Transform augmentation_buttons_transform;
  public BaseCategoryAsset asset;
  public List<TAugmentationEditorButton> augmentation_buttons = new List<TAugmentationEditorButton>();

  public void clearDebug()
  {
    for (int index = 0; index < this.augmentation_buttons_transform.childCount; ++index)
      Object.Destroy((Object) ((Component) this.augmentation_buttons_transform.GetChild(index)).gameObject);
  }

  public void hideCounter()
  {
    this.counter.text = "";
    ((Component) this.counter).gameObject.SetActive(false);
  }

  public void updateCounter()
  {
    int num = 0;
    foreach (TAugmentationEditorButton augmentationButton in this.augmentation_buttons)
    {
      if (augmentationButton.augmentation_button.isSelected())
        ++num;
    }
    string str = this.augmentation_buttons.Count.ToString();
    this.counter.text = $"{num}/{str}";
  }

  protected virtual bool isUnlocked(TAugmentationButton pButton)
  {
    throw new NotImplementedException();
  }

  private void LateUpdate() => this.updateValues();

  private void updateValues()
  {
    Vector2 sizeDelta = this.height.sizeDelta;
    sizeDelta.y = ((Component) this.augmentation_buttons_transform).GetComponent<RectTransform>().sizeDelta.y + 15f;
    this.height.sizeDelta = sizeDelta;
  }

  public int countActiveButtons()
  {
    int num = 0;
    foreach (TAugmentationEditorButton augmentationButton in this.augmentation_buttons)
    {
      if (((Component) (object) augmentationButton).gameObject.activeSelf)
        ++num;
    }
    return num;
  }

  public bool hasAugmentation(TAugmentation pTrait)
  {
    foreach (TAugmentationEditorButton augmentationButton in this.augmentation_buttons)
    {
      if ((object) augmentationButton.augmentation_button.getElementAsset() == (object) pTrait)
        return true;
    }
    return false;
  }
}
