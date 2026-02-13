// Decompiled with JetBrains decompiler
// Type: WindowPremiumRestoreHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using UnityEngine.UI;

#nullable disable
public class WindowPremiumRestoreHelper : MonoBehaviour
{
  [SerializeField]
  private Text _text_console;
  private float _restore_timeout;
  private static string COLOR_LEFT_DARK = "#45B714";
  private const float RESTORE_TIMEOUT = 6f;
  private bool _show_caret = true;
  private readonly string[] _restore_phrases = new string[31 /*0x1F*/]
  {
    "Restoring",
    "Verifying integrity",
    "Authenticating deities",
    "Decrypting receipts",
    "Syncing time",
    "Validating purpose",
    "Loading configs",
    "Rebuilding indexes",
    "Rechecking derps",
    "Clearing temps",
    "Allocating memory",
    "Running diagnostics",
    "Parsing metadata",
    "Linking modules",
    "Thinking",
    "Untangling",
    "Melting",
    "Cooking",
    "Resurrecting skeletons",
    "Negotiating with entropy",
    "Reattaching soul bindings",
    "Refreshing mythos",
    "Loading universal constants",
    "Aligning timelines",
    "Resetting divine counters",
    "Auditing reality logs",
    "Reinitializing worldframe",
    "Binding laws of physics",
    "Sealing causality breaches",
    "Decoding fate instructions",
    "Sanitizing memory cache"
  };
  private int _restore_index;

  private void OnEnable() => this.updateConsoleText();

  public void startRestoreTimeout()
  {
    this._restore_timeout = 6f;
    this._restore_index = 0;
    this._restore_phrases.Shuffle<string>();
    this.updateConsoleText();
  }

  private void Update()
  {
    if ((double) this._restore_timeout > 0.0)
    {
      this._restore_timeout -= Time.deltaTime;
      if (Time.frameCount % Randy.randomInt(15, 40) != 0)
        return;
    }
    else if (Time.frameCount % 30 != 0)
      return;
    this.updateConsoleText();
  }

  private void updateConsoleText()
  {
    this._show_caret = !this._show_caret;
    if (InAppManager.restore_ui_buffering || (double) this._restore_timeout > 0.0)
      this.showTerminalLoading();
    else
      this.showTerminalInfo();
  }

  private void showTerminalLoading()
  {
    if (this._restore_index < 14 && this._restore_index < this._restore_phrases.Length)
      ++this._restore_index;
    using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
    {
      for (int index = 0; index < this._restore_index; ++index)
      {
        float progress = Mathf.Clamp01((float) (index + 1) / (float) this._restore_phrases.Length);
        int num = Mathf.RoundToInt(progress * 100f);
        int count1 = Mathf.RoundToInt(progress * 4f);
        int count2 = 4 - count1;
        string barColor = this.getBarColor(progress);
        string pString1 = $"[{new string('█', count1)}{new string('░', count2)}]".ColorHex(barColor);
        string pString2 = this._restore_phrases[index].ToLower().PadRight(27);
        string str1 = $"{num,2}%".ColorHex("#FFFF66");
        string pColorHex1 = index < this._restore_index - 3 ? "#558855" : WindowPremiumRestoreHelper.COLOR_LEFT_DARK;
        string str2 = this.user.ColorHex(pColorHex1);
        string pColorHex2 = pColorHex1;
        string str3 = pString2.ColorHex(pColorHex2);
        string pColorHex3 = pColorHex1;
        string str4 = pString1.ColorHex(pColorHex3);
        stringBuilderPool.AppendLine($"{str2} {str3} {str4} {str1}");
      }
      stringBuilderPool.AppendLine(">".ColorHex(WindowPremiumRestoreHelper.COLOR_LEFT_DARK));
      this._text_console.text = stringBuilderPool.ToString();
    }
  }

  private void showTerminalInfo()
  {
    using (StringBuilderPool stringBuilderPool1 = new StringBuilderPool())
    {
      PlayerConfigData data = PlayerConfig.instance?.data;
      if (!string.IsNullOrEmpty(InAppManager.restore_message))
        stringBuilderPool1.AppendLine($"{this.user} {InAppManager.restore_message}".blue());
      if (data != null && data.premiumDisabled)
        stringBuilderPool1.AppendLine((this.user + " Premium disabled in debug menu!").red());
      stringBuilderPool1.AppendLine($"{this.user} Premium active: {(data != null ? data.premium.blue() : (string) null)} / {Config.hasPremium.blue()}");
      stringBuilderPool1.AppendLine($"{this.user} web_status: {Application.internetReachability.ToString().blue()}");
      if (Object.op_Equality((Object) InAppManager.instance, (Object) null) || InAppManager.instance.controller == null)
      {
        stringBuilderPool1.AppendLine((this.user + " InAppManager not initialized").red());
      }
      else
      {
        stringBuilderPool1.AppendLine((this.user + " InAppManager initialized").blue());
        if (!InAppManager.googleAccount)
          stringBuilderPool1.AppendLine((this.user + " Google account missing? Not logged in?").red());
        if (InAppManager.validator == null)
        {
          if (!string.IsNullOrEmpty(InAppManager.validator_message))
            stringBuilderPool1.AppendLine($"{this.user} Validator error {InAppManager.validator_message}".red());
          else
            stringBuilderPool1.AppendLine((this.user + " Validator not initialized").red());
        }
        else
          stringBuilderPool1.AppendLine((this.user + " Validator initialized").blue());
        Product product = InAppManager.instance.controller.products.WithID("premium");
        if (product != null)
        {
          stringBuilderPool1.AppendLine($"{this.user} available: {product.availableToPurchase.blue()} has_receipt: {product.hasReceipt.yellow()}");
          if (!product.hasReceipt)
            stringBuilderPool1.AppendLine((this.user + " current user doesn't have a receipt - product not owned").red());
          else
            stringBuilderPool1.AppendLine((this.user + " current user has a receipt!").blue());
          StringBuilderPool stringBuilderPool2 = stringBuilderPool1;
          string user1 = this.user;
          string transactionId = product.transactionID;
          string str1 = (transactionId != null ? transactionId.Truncate(26) : (string) null).yellow();
          string str2 = $"{user1} tx: {str1}";
          stringBuilderPool2.AppendLine(str2);
          stringBuilderPool1.AppendLine($"{this.user} valid: {InAppManager.last_tValidPurchase.blue()} pending: {InAppManager.last_tPurchasePending.blue()}");
          if (product.hasReceipt)
          {
            if (InAppManager.validator != null)
            {
              try
              {
                IPurchaseReceipt[] ipurchaseReceiptArray = InAppManager.validator.Validate(product.receipt);
                int num = 0;
                foreach (IPurchaseReceipt ipurchaseReceipt in ipurchaseReceiptArray)
                {
                  ++num;
                  if (ipurchaseReceipt != null)
                  {
                    StringBuilderPool stringBuilderPool3 = stringBuilderPool1;
                    object[] objArray1 = new object[4]
                    {
                      (object) this.user,
                      (object) num,
                      (object) ipurchaseReceipt.productID.yellow(),
                      null
                    };
                    DateTime dateTime = ipurchaseReceipt.purchaseDate;
                    objArray1[3] = (object) dateTime.ToString("yyyy-MM-dd HH:mmzzz").blue();
                    string str3 = string.Format("{0} {1} re: {2} {3}", objArray1);
                    stringBuilderPool3.AppendLine(str3);
                    stringBuilderPool1.AppendLine($"{this.user} {num} tx: {ipurchaseReceipt.transactionID.yellow()}");
                    if (ipurchaseReceipt is GooglePlayReceipt googlePlayReceipt)
                      stringBuilderPool1.AppendLine($"{this.user} {num} re: {googlePlayReceipt.orderID.yellow()} {googlePlayReceipt.purchaseState.blue()}");
                    if (ipurchaseReceipt is AppleInAppPurchaseReceipt appPurchaseReceipt)
                    {
                      StringBuilderPool stringBuilderPool4 = stringBuilderPool1;
                      object[] objArray2 = new object[4]
                      {
                        (object) this.user,
                        (object) num,
                        (object) appPurchaseReceipt.originalTransactionIdentifier.yellow(),
                        null
                      };
                      dateTime = appPurchaseReceipt.originalPurchaseDate;
                      objArray2[3] = (object) dateTime.ToString("yyyy-MM-dd HH:mmzzz").blue();
                      string str4 = string.Format("{0} {1} re: {2} {3}", objArray2);
                      stringBuilderPool4.AppendLine(str4);
                      StringBuilderPool stringBuilderPool5 = stringBuilderPool1;
                      string user2 = this.user;
                      // ISSUE: variable of a boxed type
                      __Boxed<int> local = (ValueType) num;
                      dateTime = appPurchaseReceipt.cancellationDate;
                      string str5 = dateTime.ToString("yyyy-MM-dd HH:mmzzz").blue();
                      string str6 = $"{user2} {local} re: {str5}";
                      stringBuilderPool5.AppendLine(str6);
                      stringBuilderPool1.AppendLine($"{this.user} {num} re: {appPurchaseReceipt.productType.ToString().yellow()}");
                    }
                  }
                }
              }
              catch (Exception ex)
              {
                stringBuilderPool1.AppendLine($"{this.user} Exception: {ex}");
              }
            }
          }
        }
        else
          stringBuilderPool1.AppendLine((this.user + " Product not found").red());
      }
      stringBuilderPool1.AppendLine($"{this.user} op: {ButtonEvent.premium_restore_opened.blue()} res: {ButtonEvent.premium_restore_action_pressed.blue()} more: {$"{ButtonEvent.premium_more_help_pressed}".blue()}");
      using (ListPool<string> list = new ListPool<string>(6))
      {
        if (!string.IsNullOrEmpty(Config.gs))
        {
          ListPool<string> listPool = list;
          string gs = Config.gs;
          string str = (gs != null ? gs.Truncate(11) : (string) null) ?? this.generateFakeMD5('G');
          listPool.Add(str);
        }
        if (data != null && !data.pPossible0507)
          list.Add(this.generateFakeMD5('P'));
        while (list.Count < 6)
          list.Add(this.generateRandomMD5());
        list.Shuffle<string>();
        for (int index = 0; index < list.Count; index += 2)
          stringBuilderPool1.AppendLine($"{this.user} {list[index].blue()} {list[index + 1].blue()}");
        stringBuilderPool1.AppendLine($"{this.user} OS: {SystemInfo.operatingSystem.blue()}");
        stringBuilderPool1.AppendLine($"{this.user} device: {SystemInfo.deviceModel.blue()}");
        stringBuilderPool1.AppendLine($"{this.user} type: {SystemInfo.deviceType.ToString().ToUpper().Truncate(4).blue()} imode: {Application.installMode.ToString().ToUpper().Truncate(4).blue()} sand: {(Application.sandboxType.ToString().ToUpper().Truncate(4).blue() ?? "").blue()}");
        stringBuilderPool1.AppendLine($"{this.user} v: {Config.versionCodeText.blue()} ({Config.gitCodeText.blue()})");
        if (!Config.hasPremium)
          stringBuilderPool1.AppendLine($"{this.user} {"IF YOU HAVE ISSUES SHOW THIS TO DEVS".red()}");
        else
          stringBuilderPool1.AppendLine($"{this.user} {"ALL GOOD! Enjoy WorldBox".yellow()}");
        if (this._show_caret)
          stringBuilderPool1.AppendLine("> █".ColorHex(WindowPremiumRestoreHelper.COLOR_LEFT_DARK));
        else
          stringBuilderPool1.AppendLine(">".ColorHex(WindowPremiumRestoreHelper.COLOR_LEFT_DARK));
        this._text_console.text = stringBuilderPool1.ToString();
      }
    }
  }

  private string generateRandomMD5(int pLength = 4)
  {
    if (pLength <= 0)
      return string.Empty;
    using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
    {
      for (int index = 0; index < pLength; ++index)
      {
        string str = Randy.randomInt(0, 256 /*0x0100*/).ToString("X2");
        stringBuilderPool.Append(str);
        stringBuilderPool.Append(':');
      }
      return stringBuilderPool.ToString().TrimEnd(':');
    }
  }

  private string generateFakeMD5(char pLetter, int pLength = 4)
  {
    if (pLength <= 0)
      return string.Empty;
    using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
    {
      for (int index = 0; index < pLength; ++index)
      {
        stringBuilderPool.Append(pLetter);
        stringBuilderPool.Append(pLetter);
        stringBuilderPool.Append(':');
      }
      return stringBuilderPool.ToString().TrimEnd(':');
    }
  }

  private string getBarColor(float progress)
  {
    if ((double) progress < 0.30000001192092896)
      return "#FF5555";
    return (double) progress < 0.699999988079071 ? "#FFFF55" : "#55FF55";
  }

  private string user => "w:/box:".ColorHex(WindowPremiumRestoreHelper.COLOR_LEFT_DARK);
}
