// Decompiled with JetBrains decompiler
// Type: KingdomMetaBanners
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class KingdomMetaBanners : KingdomElement, IBaseMetaBanners
{
  [SerializeField]
  private CityBanner _banner_city;
  [SerializeField]
  private CultureBanner _banner_culture;
  [SerializeField]
  private AllianceBanner _banner_alliance;
  [SerializeField]
  private LanguageBanner _banner_language;
  [SerializeField]
  private ReligionBanner _banner_religion;
  [SerializeField]
  private ClanBanner _banner_clan;
  [SerializeField]
  private SubspeciesBanner _banner_subspecies;
  protected List<MetaBannerElement> banners = new List<MetaBannerElement>();
  private const float DELAY = 0.025f;
  protected int visible_banners;

  protected override void Awake()
  {
    base.Awake();
    this.banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_city,
      check = (Func<bool>) (() => this.kingdom.hasCapital()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.kingdom.capital)
    });
    this.banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_clan,
      check = (Func<bool>) (() => this.kingdom.getKingClan() != null),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.kingdom.getKingClan())
    });
    this.banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_alliance,
      check = (Func<bool>) (() => this.kingdom.hasAlliance()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.kingdom.getAlliance())
    });
    this.banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_language,
      check = (Func<bool>) (() => this.kingdom.hasLanguage()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.kingdom.getLanguage())
    });
    this.banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_culture,
      check = (Func<bool>) (() => this.kingdom.hasCulture()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.kingdom.getCulture())
    });
    this.banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_religion,
      check = (Func<bool>) (() => this.kingdom.hasReligion()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.kingdom.getReligion())
    });
    this.banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_subspecies,
      check = (Func<bool>) (() => this.kingdom.getMainSubspecies() != null),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.kingdom.getMainSubspecies())
    });
    this.enableClickAnimation();
  }

  protected override IEnumerator showContent()
  {
    KingdomMetaBanners kingdomMetaBanners = this;
    kingdomMetaBanners.banners.Sort((Comparison<MetaBannerElement>) ((x, y) => ((Component) x.banner).transform.GetSiblingIndex().CompareTo(((Component) y.banner).transform.GetSiblingIndex())));
    yield return (object) new WaitForSecondsRealtime(0.025f);
    foreach (MetaBannerElement banner in kingdomMetaBanners.banners)
    {
      if (banner.check())
      {
        kingdomMetaBanners.track_objects.Add(banner.nano());
        // ISSUE: explicit non-virtual call
        __nonvirtual (kingdomMetaBanners.metaBannerShow(banner));
      }
    }
  }

  protected override void clear()
  {
    base.clear();
    foreach (MetaBannerElement banner in this.banners)
      this.metaBannerHide(banner);
    this.visible_banners = 0;
  }

  public void metaBannerShow(MetaBannerElement pAsset)
  {
    ((Component) pAsset.banner).gameObject.SetActive(true);
    pAsset.banner.load(pAsset.nano());
    ++this.visible_banners;
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
