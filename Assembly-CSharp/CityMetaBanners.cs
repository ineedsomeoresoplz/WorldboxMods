// Decompiled with JetBrains decompiler
// Type: CityMetaBanners
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CityMetaBanners : CityElement, IBaseMetaBanners
{
  [SerializeField]
  private KingdomBanner _banner_kingdom;
  [SerializeField]
  private ClanBanner _banner_clan;
  [SerializeField]
  private AllianceBanner _banner_alliance;
  [SerializeField]
  private LanguageBanner _banner_language;
  [SerializeField]
  private CultureBanner _banner_culture;
  [SerializeField]
  private ReligionBanner _banner_religion;
  [SerializeField]
  private SubspeciesBanner _banner_subspecies;
  [SerializeField]
  private ArmyBanner _banner_army;
  protected List<MetaBannerElement> banners = new List<MetaBannerElement>();
  private const float DELAY = 0.025f;
  protected int visible_banners;

  protected override void Awake()
  {
    base.Awake();
    this.banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_kingdom,
      check = (Func<bool>) (() => !this.city.kingdom.isRekt() && !this.city.kingdom.isNeutral()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.city.kingdom)
    });
    this.banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_clan,
      check = (Func<bool>) (() => this.city.hasLeader() && this.city.leader.hasClan()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.city.leader.clan)
    });
    this.banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_alliance,
      check = (Func<bool>) (() => this.city.kingdom.hasAlliance()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.city.kingdom.getAlliance())
    });
    this.banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_language,
      check = (Func<bool>) (() => this.city.hasLanguage()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.city.getLanguage())
    });
    this.banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_culture,
      check = (Func<bool>) (() => this.city.hasCulture()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.city.getCulture())
    });
    this.banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_religion,
      check = (Func<bool>) (() => this.city.hasReligion()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.city.getReligion())
    });
    this.banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_subspecies,
      check = (Func<bool>) (() => !this.city.getMainSubspecies().isRekt()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.city.getMainSubspecies())
    });
    this.banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_army,
      check = (Func<bool>) (() => this.city.hasArmy()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.city.getArmy())
    });
    this.enableClickAnimation();
  }

  protected override IEnumerator showContent()
  {
    CityMetaBanners cityMetaBanners = this;
    cityMetaBanners.banners.Sort((Comparison<MetaBannerElement>) ((x, y) => ((Component) x.banner).transform.GetSiblingIndex().CompareTo(((Component) y.banner).transform.GetSiblingIndex())));
    yield return (object) new WaitForSecondsRealtime(0.025f);
    if (!cityMetaBanners.city.kingdom.isNeutral())
    {
      foreach (MetaBannerElement banner in cityMetaBanners.banners)
      {
        if (banner.check())
        {
          cityMetaBanners.track_objects.Add(banner.nano());
          // ISSUE: explicit non-virtual call
          __nonvirtual (cityMetaBanners.metaBannerShow(banner));
        }
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
