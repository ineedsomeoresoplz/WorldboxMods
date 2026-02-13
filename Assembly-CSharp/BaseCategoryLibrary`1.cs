// Decompiled with JetBrains decompiler
// Type: BaseCategoryLibrary`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class BaseCategoryLibrary<T> : AssetLibrary<T> where T : BaseCategoryAsset
{
  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (T obj in this.list)
    {
      BaseCategoryAsset pAsset = (BaseCategoryAsset) obj;
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
    }
  }
}
