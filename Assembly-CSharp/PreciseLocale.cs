// Decompiled with JetBrains decompiler
// Type: PreciseLocale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
public class PreciseLocale
{
  private static PreciseLocale.PlatformBridge _platform;

  private static PreciseLocale.PlatformBridge platform
  {
    get
    {
      if (PreciseLocale._platform == null)
        PreciseLocale._platform = (PreciseLocale.PlatformBridge) new PreciseLocale.PreciseLocaleWindows();
      return PreciseLocale._platform;
    }
  }

  public static string GetRegion() => PreciseLocale.platform.GetRegion();

  public static string GetLanguageID() => PreciseLocale.platform.GetLanguageID();

  public static string GetLanguage() => PreciseLocale.platform.GetLanguage();

  public static string GetCurrencyCode() => PreciseLocale.platform.GetCurrencyCode();

  public static string GetCurrencySymbol() => PreciseLocale.platform.GetCurrencySymbol();

  private interface PlatformBridge
  {
    string GetRegion();

    string GetLanguage();

    string GetLanguageID();

    string GetCurrencyCode();

    string GetCurrencySymbol();
  }

  private class EditorBridge : PreciseLocale.PlatformBridge
  {
    public string GetRegion() => "US";

    public string GetLanguage() => "en";

    public string GetLanguageID() => "en_US";

    public string GetCurrencyCode() => "USD";

    public string GetCurrencySymbol() => "$";
  }

  private class PreciseLocaleWindows : PreciseLocale.PlatformBridge
  {
    [DllImport("PreciseLocale")]
    private static extern IntPtr _getLanguage();

    [DllImport("PreciseLocale")]
    private static extern IntPtr _getRegion();

    [DllImport("PreciseLocale")]
    private static extern IntPtr _getCurrencyCode();

    [DllImport("PreciseLocale")]
    private static extern IntPtr _getCurrencySymbol();

    public string GetLanguage()
    {
      return Marshal.PtrToStringUni(PreciseLocale.PreciseLocaleWindows._getLanguage());
    }

    public string GetLanguageID() => $"{this.GetLanguage()}_{this.GetRegion()}";

    public string GetRegion()
    {
      return Marshal.PtrToStringUni(PreciseLocale.PreciseLocaleWindows._getRegion());
    }

    public string GetCurrencyCode()
    {
      return Marshal.PtrToStringUni(PreciseLocale.PreciseLocaleWindows._getCurrencyCode());
    }

    public string GetCurrencySymbol()
    {
      return Marshal.PtrToStringUni(PreciseLocale.PreciseLocaleWindows._getCurrencySymbol());
    }
  }
}
