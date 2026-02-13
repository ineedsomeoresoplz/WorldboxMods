// Decompiled with JetBrains decompiler
// Type: ActorSelectedMetaBanners
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class ActorSelectedMetaBanners : UnitMetaBanners, ISelectedTabBanners<Actor>
{
  public void update(Actor pActor)
  {
    this.setActor(pActor);
    this.clear();
    foreach (MetaBannerElement banner in this._banners)
    {
      if (banner.check())
        this.metaBannerShow(banner);
    }
  }

  protected override void checkSetActor()
  {
  }

  protected override void OnEnable()
  {
  }

  protected override void checkSetWindow()
  {
  }

  public int countVisibleBanners() => this.visible_banners;
}
