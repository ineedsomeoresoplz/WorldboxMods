// Decompiled with JetBrains decompiler
// Type: PremiumElementsChecker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PremiumElementsChecker : MonoBehaviour
{
  public GameObject premiumButtonCorner;
  public GameObject adsButton;
  private static PremiumElementsChecker instance;
  internal float insterAdTimer = 25f;

  private void Awake() => PremiumElementsChecker.instance = this;

  internal static bool goodForInterstitialAd() => DebugConfig.isOn(DebugOption.TestAds);

  public static void setInterstitialAdTimer(int howLong = 80 /*0x50*/)
  {
    if (DebugConfig.isOn(DebugOption.TestAds))
      howLong = 15;
    if (howLong > 100)
      howLong = 100;
    PremiumElementsChecker.instance.insterAdTimer = (float) howLong;
  }

  private void Update()
  {
  }

  public static void checkElements()
  {
    if (Config.hasPremium)
    {
      if (Object.op_Inequality((Object) PremiumElementsChecker.instance.premiumButtonCorner, (Object) null))
        PremiumElementsChecker.instance.premiumButtonCorner.SetActive(false);
      if (Object.op_Inequality((Object) PremiumElementsChecker.instance.adsButton, (Object) null))
        PremiumElementsChecker.instance.adsButton.SetActive(false);
    }
    else if (Object.op_Inequality((Object) PremiumElementsChecker.instance.premiumButtonCorner, (Object) null))
      PremiumElementsChecker.instance.premiumButtonCorner.SetActive(true);
    foreach (PowerButton powerButton in PowerButton.power_buttons)
      powerButton.checkLockIcon();
  }

  public static void toggleActive(bool pState)
  {
    if (Object.op_Inequality((Object) PremiumElementsChecker.instance.premiumButtonCorner, (Object) null))
      PremiumElementsChecker.instance.premiumButtonCorner.SetActive(pState);
    if (!Object.op_Inequality((Object) PremiumElementsChecker.instance.adsButton, (Object) null))
      return;
    PremiumElementsChecker.instance.adsButton.SetActive(pState);
  }
}
