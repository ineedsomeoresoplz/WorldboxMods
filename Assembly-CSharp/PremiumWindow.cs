// Decompiled with JetBrains decompiler
// Type: PremiumWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PremiumWindow : MonoBehaviour
{
  public Transform buttons_transform;

  public void Awake()
  {
    this.clearButtons();
    this.addButtons();
  }

  private void addButtons()
  {
    Dictionary<string, PowerButton> dictionary = new Dictionary<string, PowerButton>();
    foreach (PowerButton premiumButton in GodPower.premium_buttons)
      dictionary.TryAdd(premiumButton.godPower.id, premiumButton);
    foreach (PowerButton powerButton1 in dictionary.Values)
    {
      ((Component) powerButton1).gameObject.SetActive(false);
      PowerButton powerButton2 = Object.Instantiate<PowerButton>(powerButton1, this.buttons_transform);
      ((Object) ((Component) powerButton2).transform).name = ((Object) ((Component) powerButton1).transform).name;
      powerButton2.type = PowerButtonType.Shop;
      powerButton2.destroyLockIcon();
      ((Component) powerButton2).GetComponent<RectTransform>().pivot = ((Component) powerButton1).GetComponent<RectTransform>().pivot;
      IconRotationAnimation rotationAnimation = ((Component) powerButton2).gameObject.AddComponent<IconRotationAnimation>();
      rotationAnimation.delay = Randy.randomFloat(1f, 10f);
      rotationAnimation.randomDelay = true;
      ((Component) powerButton1).gameObject.SetActive(true);
      ((Component) powerButton2).gameObject.SetActive(true);
    }
  }

  private void clearButtons()
  {
    while (this.buttons_transform.childCount > 0)
    {
      GameObject gameObject = ((Component) this.buttons_transform.GetChild(0)).gameObject;
      gameObject.transform.SetParent((Transform) null);
      Object.Destroy((Object) gameObject);
    }
  }
}
