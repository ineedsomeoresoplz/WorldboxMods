// Decompiled with JetBrains decompiler
// Type: InAppManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Security;

#nullable disable
[ObfuscateLiterals]
public class InAppManager : MonoBehaviour, IDetailedStoreListener, IStoreListener
{
  public static InAppManager instance;
  private static bool _initialized = false;
  private IAppleExtensions _apple;
  private IGooglePlayStoreExtensions _googleplay;
  internal IStoreController controller;
  private IExtensionProvider extensions;
  internal static CrossPlatformValidator validator;
  public static bool last_availableToPurchase;
  public static string last_transactionID;
  public static bool last_hasReceipt;
  public static bool last_tValidPurchase;
  public static bool last_tPurchasePending;
  internal static bool googleAccount = true;
  private static ConfigurationBuilder _builder;
  public static bool restore_ui_buffering;
  public static string restore_message;
  public static string validator_message;

  private IAppleExtensions apple
  {
    get
    {
      if (this._apple == null)
        this._apple = this.extensions.GetExtension<IAppleExtensions>();
      return this._apple;
    }
  }

  private IGooglePlayStoreExtensions googleplay
  {
    get
    {
      if (this._googleplay == null)
        this._googleplay = this.extensions.GetExtension<IGooglePlayStoreExtensions>();
      return this._googleplay;
    }
  }

  private void Start()
  {
    Debug.Log((object) "InAppManager::Start");
    if (Object.op_Inequality((Object) InAppManager.instance, (Object) null))
    {
      Debug.LogError((object) "Multiple in-app managers have been instantiated.");
    }
    else
    {
      InAppManager.instance = this;
      if (PlayerConfig.instance != null && !PlayerConfig.instance.data.pPossible0507)
        return;
      InAppManager.activatePrem();
      Debug.Log((object) "InAppManager::End");
    }
  }

  private static bool checkGoogleAccount()
  {
    if (!InAppManager.googleAccount)
    {
      Debug.Log((object) "google account missing");
      ErrorWindow.errorMessage = "A Google Account is missing or you're not logged in with one.";
      ScrollWindow.get("error_with_reason").clickShow();
    }
    return InAppManager.googleAccount;
  }

  private static void debugPremium()
  {
  }

  private void InitializePurchasing(int pWhere = -1)
  {
    Debug.Log((object) $"InitializePurchasing {pWhere}");
    if (this.IsInitialized() || InAppManager._initialized)
      return;
    InAppManager._initialized = true;
    InAppManager.instance = this;
    InAppManager._builder = ConfigurationBuilder.Instance((IPurchasingModule) StandardPurchasingModule.Instance(), Array.Empty<IPurchasingModule>());
    InAppManager._builder.logUnavailableProducts = true;
    ConfigurationBuilder builder = InAppManager._builder;
    IDs ids1 = new IDs();
    ids1.Add("premium", new string[1]{ "GooglePlay" });
    ids1.Add("premium", new string[1]{ "AppleAppStore" });
    IDs ids2 = ids1;
    builder.AddProduct("premium", (ProductType) 1, ids2);
    try
    {
      InAppManager.validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
      Debug.Log((object) "validator assigned");
      InAppManager.validator_message = (string) null;
    }
    catch (NotImplementedException ex)
    {
      Debug.Log((object) "validator not assigned");
      Debug.LogError((object) ("Cross Platform Validator Not Implemented: " + ex.Message));
      InAppManager.validator_message = "Validator not implemented";
    }
    catch (Exception ex)
    {
      Debug.Log((object) "validator not assigned");
      Debug.LogError((object) ("Validator Exception: " + ex.Message));
      InAppManager.validator_message = ex.Message;
    }
    UnityPurchasing.Initialize((IDetailedStoreListener) this, InAppManager._builder);
  }

  public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
  {
    this.controller = controller;
    this.extensions = extensions;
    this.apple.RegisterPurchaseDeferredListener(new Action<Product>(this.OnAskToBuy));
    this.checkPremium();
    this.checkWorldnet();
    Discounts.checkDiscounts();
  }

  public void checkPremium()
  {
    bool flag1 = true;
    bool flag2 = false;
    Product product = this.controller.products.WithID("premium");
    Debug.Log((object) "[cp]");
    Debug.Log((object) ("[avl] " + product.availableToPurchase.ToString()));
    Debug.Log((object) ("[txi] " + product.transactionID));
    Debug.Log((object) ("[hrc] " + product.hasReceipt.ToString()));
    InAppManager.last_tValidPurchase = flag1;
    InAppManager.last_tPurchasePending = flag2;
    Config.lockGameControls = false;
    if (!flag1 & flag2)
    {
      Debug.Log((object) "[nvp] pp");
    }
    else
    {
      Debug.Log((object) ("[vp] 3 " + flag1.ToString()));
      bool hasReceipt = product.hasReceipt;
      Debug.Log((object) ("[hr] " + hasReceipt.ToString()));
      if (flag1 && product.hasReceipt || PlayerConfig.instance.data.premium)
      {
        Debug.Log((object) "[phr]");
        InAppManager.activatePrem();
      }
      else
      {
        Debug.Log((object) ("[vp] 4 " + flag1.ToString()));
        hasReceipt = product.hasReceipt;
        Debug.Log((object) ("[hr] " + hasReceipt.ToString()));
        Debug.Log((object) ("[hp] " + PlayerConfig.instance.data.premium.ToString()));
      }
      Debug.Log((object) "[cpd]");
    }
  }

  public void checkWorldnet()
  {
  }

  public static void consumePremium()
  {
  }

  private void OnAskToBuy(Product item)
  {
    Debug.Log((object) ("Purchase deferred: " + item.definition.id));
    Config.lockGameControls = false;
  }

  public string getDebugInfo()
  {
    Product product = this.controller.products.WithID("premium");
    return $"{$"{$"hasReceipt: {product.hasReceipt.ToString()}"}\nreceipt: {product.receipt}"}\nprem? {PlayerConfig.instance.data.premium.ToString()}";
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static void activatePrem(bool pShowWindowUnlocked = false)
  {
    Debug.Log((object) "[ap]");
    PlayerConfig.instance.data.premium = true;
    PlayerConfig.saveData();
    Config.hasPremium = true;
    PremiumElementsChecker.checkElements();
    PlayerConfig.setFirebaseProp("have_premium", "yes");
    if (pShowWindowUnlocked)
    {
      if (!PlayerConfig.instance.data.tutorialFinished)
      {
        ScrollWindow.queueWindow("premium_unlocked");
      }
      else
      {
        ScrollWindow.hideAllEvent(false);
        ScrollWindow.showWindow("premium_unlocked");
      }
    }
    InAppManager.debugPremium();
  }

  private void activateSub(bool pShowWindowUnlocked = false)
  {
    if (!pShowWindowUnlocked || !PlayerConfig.instance.data.tutorialFinished)
      return;
    ScrollWindow.hideAllEvent(false);
    ScrollWindow.showWindow("worldnet_sub");
  }

  public void OnInitializeFailed(InitializationFailureReason error)
  {
    Debug.Log((object) ("Cannot initialize IAP system: " + error.ToString()));
    Config.lockGameControls = false;
  }

  public void OnInitializeFailed(InitializationFailureReason error, string message)
  {
    Debug.Log((object) ("Cannot initialize IAP system: " + error.ToString()));
    Debug.Log((object) message);
    Config.lockGameControls = false;
  }

  public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
  {
    Config.lockGameControls = true;
    Debug.Log((object) ("Purchase OK: " + args?.purchasedProduct?.definition?.id));
    Debug.Log((object) ("Args: " + args?.purchasedProduct?.ToString()));
    if (args?.purchasedProduct?.definition?.id == "premium")
    {
      Debug.Log((object) "[pp] process premium");
      return this.ProcessPremium(args);
    }
    if (args?.purchasedProduct?.definition?.id == "worldnet")
    {
      Debug.Log((object) "[pw] process worldnet");
      return this.ProcessWorldnet(args);
    }
    Debug.Log((object) "[pn] process nothing to be done");
    return (PurchaseProcessingResult) 0;
  }

  public PurchaseProcessingResult ProcessPremium(PurchaseEventArgs args)
  {
    bool flag1 = true;
    bool flag2 = false;
    Debug.Log((object) "[prp]");
    if (!flag1 & flag2)
    {
      Debug.Log((object) "[np] 3");
      Config.lockGameControls = false;
      return (PurchaseProcessingResult) 1;
    }
    string id = args?.purchasedProduct?.definition?.id;
    Debug.Log((object) ("[vp] 2 " + flag1.ToString()));
    Debug.Log((object) ("[lc] " + id));
    if (flag1 && string.Equals(id, "premium", StringComparison.OrdinalIgnoreCase))
    {
      InAppManager.activatePrem(true);
      Debug.Log((object) $"[ppp] '{id}'");
    }
    else
      Debug.Log((object) "[np] 4");
    Config.lockGameControls = false;
    InAppManager.debugPremium();
    return (PurchaseProcessingResult) 0;
  }

  public PurchaseProcessingResult ProcessWorldnet(PurchaseEventArgs args)
  {
    bool flag1 = true;
    bool flag2 = false;
    PlayerConfig.instance.data.worldnet = "";
    if (!flag1 & flag2)
    {
      Debug.Log((object) "purchase pending");
      Config.lockGameControls = false;
      return (PurchaseProcessingResult) 1;
    }
    Debug.Log((object) "check if valid");
    Config.lockGameControls = false;
    if (flag1 && string.Equals(args.purchasedProduct.definition.id, "worldnet", StringComparison.OrdinalIgnoreCase))
    {
      Debug.Log((object) "valid!");
      this.setWorldnetSubscription(args.purchasedProduct.transactionID);
      this.activateSub(true);
      Debug.Log((object) $"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
    }
    Debug.Log((object) "we are here");
    if (Config.lockGameControls)
      Debug.Log((object) "lockgamecontrosl locked");
    else
      Debug.Log((object) "lockgamecontrosl not locked");
    return (PurchaseProcessingResult) 0;
  }

  public void setWorldnetSubscription(string pTransactionID)
  {
    PlayerConfig.instance.data.worldnet = pTransactionID;
    PlayerConfig.saveData();
  }

  public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
  {
    Debug.Log((object) ("[D] WORLDBOX PURCHASE FAILED  " + p.ToString()));
    Config.lockGameControls = false;
    ScrollWindow.showWindow("premium_purchase_error");
    this.InitializePurchasing(50);
  }

  public void OnPurchaseFailed(Product i, PurchaseFailureDescription p)
  {
    Debug.Log((object) ("[X] WORLDBOX PURCHASE FAILED  " + p.message));
    this.OnPurchaseFailed(i, p.reason);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static void checkPremiumReceipt(
    string pReceipt,
    ref bool validPurchase,
    ref bool purchasePending)
  {
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static void checkWorldnetReceipt(
    string pReceipt,
    ref bool validPurchase,
    ref bool purchasePending)
  {
  }

  public bool buyPremium()
  {
    InAppManager.activatePrem(true);
    Config.lockGameControls = false;
    return true;
  }

  public bool buyWorldNet() => false;

  private bool BuyProductID(string productId)
  {
    if (this.IsInitialized())
    {
      Product product = this.controller.products.WithID(productId);
      if (product == null)
        return false;
      if (product.availableToPurchase)
      {
        Debug.Log((object) $"Purchasing product asychronously: '{product.definition.id}'");
        Config.lockGameControls = true;
        this.controller.InitiatePurchase(product);
        if (ScrollWindow.windowLoaded("premium_menu") && ScrollWindow.isCurrentWindow("premium_menu"))
          ScrollWindow.get("premium_menu").clickHide();
        return true;
      }
      ScrollWindow.showWindow(productId + "_purchase_error");
      Debug.Log((object) "BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
      Config.lockGameControls = false;
      return false;
    }
    ScrollWindow.showWindow(productId + "_purchase_error");
    Debug.Log((object) "BuyProductID FAIL. Not initialized.");
    Config.lockGameControls = false;
    return false;
  }

  public void RestorePurchases()
  {
    InAppManager.restore_message = "Restoring...";
    InAppManager.restore_ui_buffering = true;
    if (Config.isEditor)
      InAppManager.restore_ui_buffering = false;
    if (this.IsInitialized())
      return;
    this.InitializePurchasing(66);
    Debug.Log((object) "RestorePurchases FAIL. Not initialized.");
    InAppManager.restore_message = "IAP not initialized, failed to restore purchases.";
  }

  private bool IsInitialized() => this.controller != null && this.extensions != null;
}
