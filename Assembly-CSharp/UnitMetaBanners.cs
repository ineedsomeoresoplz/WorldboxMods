// Decompiled with JetBrains decompiler
// Type: UnitMetaBanners
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UnitMetaBanners : UnitElement, IBaseMetaBanners
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
  private KingdomBanner _banner_kingdom;
  [SerializeField]
  private SubspeciesBanner _banner_subspecies;
  [SerializeField]
  private FamilyBanner _banner_family;
  [SerializeField]
  private PlotBanner _banner_plot;
  [SerializeField]
  private ArmyBanner _banner_army;
  protected List<MetaBannerElement> _banners = new List<MetaBannerElement>();
  private const float DELAY = 0.025f;
  private int _visible_banners;

  public int visible_banners => this._visible_banners;

  protected override void Awake()
  {
    base.Awake();
    this._banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_kingdom,
      check = (Func<bool>) (() => this.actor.isKingdomCiv()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.actor.kingdom)
    });
    this._banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_clan,
      check = (Func<bool>) (() => this.actor.hasClan()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.actor.clan)
    });
    this._banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_alliance,
      check = (Func<bool>) (() => this.actor.kingdom.hasAlliance()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.actor.kingdom.getAlliance())
    });
    this._banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_language,
      check = (Func<bool>) (() => this.actor.hasLanguage()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.actor.language)
    });
    this._banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_culture,
      check = (Func<bool>) (() => this.actor.hasCulture()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.actor.culture)
    });
    this._banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_religion,
      check = (Func<bool>) (() => this.actor.hasReligion()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.actor.religion)
    });
    this._banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_subspecies,
      check = (Func<bool>) (() => this.actor.hasSubspecies()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.actor.subspecies)
    });
    this._banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_family,
      check = (Func<bool>) (() => this.actor.hasFamily()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.actor.family)
    });
    this._banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_plot,
      check = (Func<bool>) (() => this.actor.hasPlot()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.actor.plot)
    });
    this._banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_city,
      check = (Func<bool>) (() => this.actor.hasCity()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.actor.getCity())
    });
    this._banners.Add(new MetaBannerElement()
    {
      banner = (BannerBase) this._banner_army,
      check = (Func<bool>) (() => this.actor.hasArmy()),
      nano = (MetaSelectedGetter) (() => (NanoObject) this.actor.army)
    });
    this.enableClickAnimation();
  }

  protected override IEnumerator showContent()
  {
    UnitMetaBanners unitMetaBanners = this;
    unitMetaBanners._banners.Sort((Comparison<MetaBannerElement>) ((x, y) => ((Component) x.banner).transform.GetSiblingIndex().CompareTo(((Component) y.banner).transform.GetSiblingIndex())));
    yield return (object) new WaitForSecondsRealtime(0.025f);
    foreach (MetaBannerElement banner in unitMetaBanners._banners)
    {
      if (banner.check())
      {
        unitMetaBanners.track_objects.Add(banner.nano());
        // ISSUE: explicit non-virtual call
        __nonvirtual (unitMetaBanners.metaBannerShow(banner));
      }
    }
  }

  protected override void clear()
  {
    base.clear();
    this._visible_banners = 0;
    foreach (MetaBannerElement banner in this._banners)
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
    return (IReadOnlyCollection<MetaBannerElement>) this._banners;
  }
}
