// Decompiled with JetBrains decompiler
// Type: Discounts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using Proyecto26;
using System;
using UnityEngine;
using UnityEngine.Purchasing;

#nullable disable
internal static class Discounts
{
  private static ProductMetadata localPriceData;
  private static string platform;

  internal static void checkDiscounts()
  {
    try
    {
      Discounts.checkPlatform();
      if (Object.op_Inequality((Object) InAppManager.instance, (Object) null) && InAppManager.instance?.controller?.products != null)
      {
        Product product = InAppManager.instance.controller.products.WithID("premium");
        if (product != null)
          Discounts.discountRequest(product.metadata);
        else
          Debug.Log((object) "DC:no req/prod");
      }
      else
        Debug.Log((object) "DC:np");
    }
    catch (Exception ex)
    {
      Debug.Log((object) "DC:err");
      Debug.Log((object) ex);
    }
  }

  private static void discountRequest(ProductMetadata pProductMeta)
  {
    if (Discounts.platform.Length < 2 || pProductMeta == null)
      return;
    string str1 = $"https://currency.superworldbox.com/discounts/{Discounts.platform}.json?{Toolbox.cacheBuster()}";
    string str2 = JsonConvert.SerializeObject((object) pProductMeta, new JsonSerializerSettings()
    {
      DefaultValueHandling = (DefaultValueHandling) 3
    });
    if (string.IsNullOrEmpty(str2) || str2 == "{}")
      return;
    RestClient.Post(str1, str2).Then((Action<ResponseHelper>) (response =>
    {
      string text = response.Text;
      if (string.IsNullOrEmpty(text) || text == "{}" || text.Substring(0, 1) != "{")
        return;
      Debug.Log((object) text);
      DiscountData discountData = JsonConvert.DeserializeObject<DiscountData>(text);
      Debug.Log((object) "DS:Setting");
      if (!string.IsNullOrEmpty(discountData.discount) && !string.IsNullOrEmpty(discountData.price_current) && !string.IsNullOrEmpty(discountData.price_old))
      {
        LocalizedTextPrice.discount = discountData.discount;
        LocalizedTextPrice.price_current = discountData.price_current;
        LocalizedTextPrice.price_old = discountData.price_old;
        Debug.Log((object) "DS:Set");
      }
      else
        Debug.Log((object) "DS:NSet");
    })).Catch((Action<Exception>) (err =>
    {
      Debug.Log((object) "DS:err");
      Debug.Log((object) err.Message);
    }));
  }

  private static void checkPlatform()
  {
    RuntimePlatform platform = Application.platform;
    if (platform <= 11)
    {
      switch ((int) platform)
      {
        case 0:
          Discounts.platform = "mac";
          return;
        case 1:
          Discounts.platform = "mac";
          return;
        case 2:
          Discounts.platform = "pc";
          return;
        case 3:
        case 4:
        case 5:
        case 6:
          break;
        case 7:
          Discounts.platform = "pc";
          return;
        case 8:
          Discounts.platform = "ios";
          return;
        default:
          if (platform == 11)
          {
            Discounts.platform = "android";
            return;
          }
          break;
      }
    }
    else if (platform != 13)
    {
      if (platform == 16 /*0x10*/)
      {
        Discounts.platform = "linux";
        return;
      }
    }
    else
    {
      Discounts.platform = "linux";
      return;
    }
    Discounts.platform = "unknown";
  }
}
