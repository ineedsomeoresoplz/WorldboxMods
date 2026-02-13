// Decompiled with JetBrains decompiler
// Type: IBaseMetaBanners
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public interface IBaseMetaBanners
{
  void metaBannerShow(MetaBannerElement pAsset);

  void metaBannerHide(MetaBannerElement pAsset);

  IReadOnlyCollection<MetaBannerElement> getBanners();

  void enableClickAnimation()
  {
    foreach (MetaBannerElement banner in (IEnumerable<MetaBannerElement>) this.getBanners())
      ((Component) banner.banner).GetComponent<TipButton>().showOnClick = true;
  }
}
