// Decompiled with JetBrains decompiler
// Type: ArmySelectedMetaBanners
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class ArmySelectedMetaBanners : ArmyMetaBanners, ISelectedTabBanners<Army>
{
  public void update(Army pArmy)
  {
    this.meta_object = pArmy;
    this.clear();
    foreach (MetaBannerElement banner in this.banners)
    {
      if (banner.check())
        this.metaBannerShow(banner);
    }
  }

  protected override void OnEnable()
  {
  }

  public int countVisibleBanners() => this.visible_banners;
}
