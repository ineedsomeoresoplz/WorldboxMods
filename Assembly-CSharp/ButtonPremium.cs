// Decompiled with JetBrains decompiler
// Type: ButtonPremium
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ButtonPremium : MonoBehaviour
{
  public void clickPremium()
  {
    PlayerConfig.setFirebaseProp("clicked_buy_premium", "yes");
    Analytics.LogEvent("clicked_buy_premium");
    if (Application.internetReachability == null)
      ScrollWindow.showWindow("premium_purchase_error");
    else
      InAppManager.instance.buyPremium();
  }
}
