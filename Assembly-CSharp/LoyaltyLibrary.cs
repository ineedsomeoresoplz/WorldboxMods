// Decompiled with JetBrains decompiler
// Type: LoyaltyLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class LoyaltyLibrary : AssetLibrary<LoyaltyAsset>
{
  public override void init()
  {
    // ISSUE: unable to decompile the method.
  }

  public override void editorDiagnosticLocales()
  {
    foreach (LoyaltyAsset pAsset in this.list)
    {
      foreach (string localeId in pAsset.getLocaleIDs())
        this.checkLocale((Asset) pAsset, localeId);
    }
    base.editorDiagnosticLocales();
  }
}
