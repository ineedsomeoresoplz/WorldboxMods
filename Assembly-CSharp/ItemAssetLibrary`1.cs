// Decompiled with JetBrains decompiler
// Type: ItemAssetLibrary`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public abstract class ItemAssetLibrary<T> : BaseLibraryWithUnlockables<T> where T : ItemAsset
{
  public override T add(T pAsset)
  {
    T obj = base.add(pAsset);
    if (obj.base_stats == null)
      obj.base_stats = new BaseStats();
    return obj;
  }

  public override void editorDiagnosticLocales()
  {
    foreach (T obj in this.list)
    {
      ItemAsset pAsset = (ItemAsset) obj;
      if (pAsset.has_locales)
      {
        string localeId = pAsset.getLocaleID();
        this.checkLocale((Asset) pAsset, localeId);
        if (!pAsset.isMod())
        {
          string descriptionId = pAsset.getDescriptionID();
          this.checkLocale((Asset) pAsset, descriptionId);
          if (pAsset.material != "basic")
            this.checkLocale((Asset) pAsset, pAsset.getMaterialID());
          foreach (Rarity pRarity in Enum.GetValues(typeof (Rarity)))
          {
            string localeRarity = pAsset.getLocaleRarity(pRarity);
            this.checkLocale((Asset) pAsset, localeRarity);
          }
        }
      }
    }
  }
}
