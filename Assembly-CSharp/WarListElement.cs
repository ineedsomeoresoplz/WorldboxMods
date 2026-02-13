// Decompiled with JetBrains decompiler
// Type: WarListElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WarListElement : WindowListElementBase<War, WarData>
{
  public Text text_name;
  public LocalizedText war_type;
  public CountUpOnClick age;
  public CountUpOnClick duration;
  public CountUpOnClick kingdoms;
  public CountUpOnClick cities;
  public CountUpOnClick renown;
  public CountUpOnClick dead;
  public KingdomBanner prefabMiniKingdomBanner;
  public GameObject gridAttackers;
  public GameObject gridDefenders;
  protected ObjectPoolGenericMono<KingdomBanner> pool_mini_banners_attackers;
  protected ObjectPoolGenericMono<KingdomBanner> pool_mini_banners_defenders;
  public Image total_war_icon;

  internal override void show(War pWar)
  {
    base.show(pWar);
    this.text_name.text = pWar.data.name;
    ((Graphic) this.text_name).color = pWar.getColor().getColorText();
    this.war_type.setKeyAndUpdate(pWar.getAsset().localized_war_name);
    this.kingdoms.setValue(pWar.countKingdoms());
    this.cities.setValue(pWar.countCities());
    this.renown.setValue(pWar.getRenown());
    this.dead.setValue((int) pWar.getTotalDeaths());
    this.age.setValue(pWar.getAge());
    this.duration.setValue(pWar.getDuration());
    ((Component) this.total_war_icon).gameObject.SetActive(false);
    this.clearBanners();
    WarWinner winner = pWar.data.winner;
    this.showKingdomBanners(pWar.getAttackers(), this.pool_mini_banners_attackers, pWinner: winner == WarWinner.Attackers, pLoser: winner == WarWinner.Defenders);
    this.showKingdomBanners(pWar.getDiedAttackers(), this.pool_mini_banners_attackers);
    this.showKingdomBanners(pWar.getPastAttackers(), this.pool_mini_banners_attackers, true);
    this.showKingdomBanners(pWar.getDefenders(), this.pool_mini_banners_defenders, pWinner: winner == WarWinner.Defenders, pLoser: winner == WarWinner.Attackers);
    this.showKingdomBanners(pWar.getDiedDefenders(), this.pool_mini_banners_defenders);
    this.showKingdomBanners(pWar.getPastDefenders(), this.pool_mini_banners_defenders, true);
    bool flag1 = pWar.countAttackersPopulation() > pWar.countDefendersPopulation();
    bool flag2 = pWar.getDeadDefenders() > pWar.getDeadAttackers();
    bool flag3 = pWar.countAttackersWarriors() > pWar.countDefendersWarriors();
    pWar.countAttackersCities();
    pWar.countDefendersCities();
    this.setIconValue("i_attackers_population", pWar.countAttackersPopulation(), flag1 ? "#43FF43" : "#FB2C21");
    this.setIconValue("i_attackers_army", pWar.countAttackersWarriors(), flag3 ? "#43FF43" : "#FB2C21");
    this.setIconValue("i_attackers_dead", pWar.getDeadAttackers(), flag2 ? "#43FF43" : "#FB2C21");
    this.setIconValue("i_defenders_population", pWar.countDefendersPopulation(), flag1 ? "#FB2C21" : "#43FF43");
    this.setIconValue("i_defenders_army", pWar.countDefendersWarriors(), flag3 ? "#FB2C21" : "#43FF43");
    this.setIconValue("i_defenders_dead", pWar.getDeadDefenders(), flag2 ? "#FB2C21" : "#43FF43");
  }

  private void checkCreation()
  {
    if (this.pool_mini_banners_attackers != null)
      return;
    this.pool_mini_banners_attackers = new ObjectPoolGenericMono<KingdomBanner>(this.prefabMiniKingdomBanner, this.gridAttackers.transform);
    this.pool_mini_banners_defenders = new ObjectPoolGenericMono<KingdomBanner>(this.prefabMiniKingdomBanner, this.gridDefenders.transform);
  }

  public void clearBanners()
  {
    this.checkCreation();
    this.pool_mini_banners_attackers.clear();
    this.pool_mini_banners_defenders.clear();
  }

  public void showKingdomBanners(
    IEnumerable<Kingdom> pList,
    ObjectPoolGenericMono<KingdomBanner> pPool,
    bool pLeft = false,
    bool pWinner = false,
    bool pLoser = false)
  {
    this.checkCreation();
    int num = 6 - pPool.countActive();
    if (num <= 0)
      return;
    foreach (Kingdom p in pList)
    {
      if (p != null && p.isAlive())
      {
        KingdomBanner next = pPool.getNext();
        next.load((NanoObject) p);
        if (pLeft)
          next.hasLeftWar();
        if (pWinner)
          next.hasWon();
        if (pLoser)
          next.hasLost();
        ((Behaviour) ((Component) next).GetComponentInChildren<RotateOnHover>()).enabled = true;
        if (num-- <= 0)
          break;
      }
    }
  }

  public void setIconValue(string pName, int pMainVal, string pColor)
  {
    Transform recursive = ((Component) this).transform.FindRecursive(pName);
    if (Object.op_Equality((Object) recursive, (Object) null))
    {
      Debug.LogError((object) ("No icon with this name! " + pName));
    }
    else
    {
      Transform transform = recursive.Find("Container/Text");
      if (Object.op_Equality((Object) transform, (Object) null))
      {
        Debug.LogError((object) (pName + " doesn't have Container/Text"));
      }
      else
      {
        ((Component) transform).gameObject.SetActive(true);
        Text component1 = ((Component) transform).GetComponent<Text>();
        CountUpOnClick component2 = ((Component) recursive).GetComponent<CountUpOnClick>();
        Color color = Toolbox.makeColor(pColor);
        ((Graphic) component1).color = color;
        component2.setValue(pMainVal);
      }
    }
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    this.pool_mini_banners_attackers?.clear();
    this.pool_mini_banners_defenders?.clear();
  }

  protected override void tooltipAction()
  {
    Tooltip.show((object) this, "war", new TooltipData()
    {
      war = this.meta_object
    });
  }
}
