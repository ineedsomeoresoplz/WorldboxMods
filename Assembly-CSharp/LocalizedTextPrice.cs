// Decompiled with JetBrains decompiler
// Type: LocalizedTextPrice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

#nullable disable
public class LocalizedTextPrice : MonoBehaviour
{
  public static string price_current = "???";
  public static string price_old = string.Empty;
  public static string discount = string.Empty;
  public Text text_old_price;
  public Text text_current_price;
  public GameObject discount_bg;
  public Text text_percent;
  private const string IN_APP_ID = "premium";

  internal void updateText(bool pCheckText = true)
  {
    if (!string.IsNullOrEmpty(LocalizedTextPrice.discount))
      this.showDiscount(LocalizedTextPrice.discount);
    string str = "";
    if (Object.op_Inequality((Object) InAppManager.instance, (Object) null) && InAppManager.instance?.controller?.products != null)
    {
      Product product = InAppManager.instance.controller.products.WithID("premium");
      if (product != null)
        str = product.metadata.localizedPriceString;
    }
    else
      str = LocalizedTextPrice.price_current;
    this.text_current_price.text = str;
    if (string.IsNullOrEmpty(LocalizedTextPrice.price_old))
      return;
    this.text_old_price.text = LocalizedTextPrice.price_old;
    ((Component) this.text_old_price).gameObject.SetActive(true);
  }

  private void showDiscount(string pString)
  {
    this.text_percent.text = pString;
    this.discount_bg.gameObject.SetActive(true);
  }

  private void setDefault()
  {
    this.discount_bg.gameObject.SetActive(false);
    ((Component) this.text_current_price).gameObject.SetActive(true);
    this.text_current_price.text = "??";
    ((Component) this.text_old_price).gameObject.SetActive(false);
  }

  private void OnEnable()
  {
    this.setDefault();
    this.updateText();
  }
}
