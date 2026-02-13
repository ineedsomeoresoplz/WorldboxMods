// Decompiled with JetBrains decompiler
// Type: LoyaltyAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class LoyaltyAsset : Asset, IMultiLocalesAsset
{
  public string translation_key;
  public string translation_key_negative;
  public LoyaltyDelegateCalc calc;

  public IEnumerable<string> getLocaleIDs()
  {
    yield return this.translation_key;
    if (!string.IsNullOrEmpty(this.translation_key_negative))
      yield return this.translation_key_negative;
  }

  public string getTranslationKey(int pValue)
  {
    return pValue > 0 || string.IsNullOrEmpty(this.translation_key_negative) ? this.translation_key : this.translation_key_negative;
  }
}
