// Decompiled with JetBrains decompiler
// Type: AllianceListElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class AllianceListElement : WindowListElementBase<Alliance, AllianceData>
{
  public Text text_name;
  public CountUpOnClick age;
  public CountUpOnClick population;
  public CountUpOnClick warriors;
  public CountUpOnClick villages;
  public CountUpOnClick kingdoms;
  public Text level;
  public KingdomBanner prefabMiniKingdomBanner;
  public GameObject grid;
  private ObjectPoolGenericMono<KingdomBanner> pool_mini_banners;

  internal override void show(Alliance pAlliance)
  {
    base.show(pAlliance);
    this.text_name.text = this.meta_object.name;
    ((Graphic) this.text_name).color = this.meta_object.getColor().getColorText();
    this.age.setValue(this.meta_object.getAge());
    this.population.setValue(this.meta_object.countPopulation());
    this.warriors.setValue(this.meta_object.countWarriors());
    this.villages.setValue(this.meta_object.countCities());
    this.kingdoms.setValue(this.meta_object.countKingdoms());
    this.showKingdomBanners(this.meta_object.kingdoms_list);
  }

  public void showKingdomBanners(List<Kingdom> pList)
  {
    if (this.pool_mini_banners == null)
      this.pool_mini_banners = new ObjectPoolGenericMono<KingdomBanner>(this.prefabMiniKingdomBanner, this.grid.transform);
    this.pool_mini_banners.clear();
    foreach (Kingdom p in pList)
    {
      KingdomBanner next = this.pool_mini_banners.getNext();
      next.load((NanoObject) p);
      ((Behaviour) ((Component) next).GetComponentInChildren<RotateOnHover>()).enabled = false;
    }
  }

  protected override void tooltipAction()
  {
    Tooltip.show((object) this, "alliance", new TooltipData()
    {
      alliance = this.meta_object
    });
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    this.pool_mini_banners?.clear();
  }
}
