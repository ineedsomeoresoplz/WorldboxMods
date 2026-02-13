// Decompiled with JetBrains decompiler
// Type: ArmyMetaBanners
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ArmyMetaBanners : ArmyElement, IBaseMetaBanners
{
  [SerializeField]
  private CityBanner _banner_city;
  [SerializeField]
  private AllianceBanner _banner_alliance;
  [SerializeField]
  private KingdomBanner _banner_kingdom;
  protected List<MetaBannerElement> banners = new List<MetaBannerElement>();
  private const float DELAY = 0.025f;
  private int _visible_banners;

  public int visible_banners => this._visible_banners;

  protected override void Awake()
  {
    base.Awake();
    this.banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_kingdom,
      check = (Func<bool>) (() => this.army.hasKingdom()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.army.getKingdom())
    });
    this.banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_alliance,
      check = (Func<bool>) (() => this.army.hasKingdom() && this.army.getKingdom().hasAlliance()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.army.getKingdom().getAlliance())
    });
    this.banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_city,
      check = (Func<bool>) (() => this.army.hasCity()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.army.getCity())
    });
    this.enableClickAnimation();
  }

  protected override IEnumerator showContent()
  {
    ArmyMetaBanners armyMetaBanners = this;
    armyMetaBanners.banners.Sort((Comparison<MetaBannerElement>) ((x, y) => ((Component) x.banner).transform.GetSiblingIndex().CompareTo(((Component) y.banner).transform.GetSiblingIndex())));
    yield return (object) new WaitForSecondsRealtime(0.025f);
    foreach (MetaBannerElement banner in armyMetaBanners.banners)
    {
      if (banner.check())
      {
        armyMetaBanners.track_objects.Add(banner.nano());
        // ISSUE: explicit non-virtual call
        __nonvirtual (armyMetaBanners.metaBannerShow(banner));
      }
    }
  }

  protected override void clear()
  {
    base.clear();
    this._visible_banners = 0;
    foreach (MetaBannerElement banner in this.banners)
      this.metaBannerHide(banner);
  }

  public void metaBannerShow(MetaBannerElement pAsset)
  {
    ((Component) pAsset.banner).gameObject.SetActive(true);
    pAsset.banner.load(pAsset.nano());
    ++this._visible_banners;
  }

  public void metaBannerHide(MetaBannerElement pAsset)
  {
    if (!((Component) pAsset.banner).gameObject.activeSelf)
      return;
    ((Component) pAsset.banner).gameObject.SetActive(false);
  }

  public IReadOnlyCollection<MetaBannerElement> getBanners()
  {
    return (IReadOnlyCollection<MetaBannerElement>) this.banners;
  }
}
