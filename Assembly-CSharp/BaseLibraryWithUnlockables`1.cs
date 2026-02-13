// Decompiled with JetBrains decompiler
// Type: BaseLibraryWithUnlockables`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class BaseLibraryWithUnlockables<T> : AssetLibrary<T>, ILibraryWithUnlockables where T : BaseUnlockableAsset
{
  [JsonIgnore]
  public IEnumerable<BaseUnlockableAsset> elements_list
  {
    get => (IEnumerable<BaseUnlockableAsset>) this.list;
  }

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (T obj in this.list)
    {
      BaseUnlockableAsset pAsset = (BaseUnlockableAsset) obj;
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
    }
  }
}
