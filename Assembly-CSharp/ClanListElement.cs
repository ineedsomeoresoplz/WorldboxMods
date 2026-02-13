// Decompiled with JetBrains decompiler
// Type: ClanListElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ClanListElement : WindowListElementBase<Clan, ClanData>
{
  public Text text_name;
  public CountUpOnClick members;
  public CountUpOnClick dead;
  public CountUpOnClick age;
  public CountUpOnClick renown;
  public UiUnitAvatarElement avatarLoader;

  internal override void show(Clan pClan)
  {
    base.show(pClan);
    Actor chief = pClan.getChief();
    if (chief.isRekt())
    {
      ((Component) this.avatarLoader).gameObject.SetActive(false);
    }
    else
    {
      ((Component) this.avatarLoader).gameObject.SetActive(true);
      this.avatarLoader.show(chief);
    }
    this.text_name.text = pClan.name;
    ((Graphic) this.text_name).color = pClan.getColor().getColorText();
    this.members.setValue(pClan.countUnits());
    this.renown.setValue(pClan.getRenown());
    this.age.setValue(pClan.getAge());
    this.dead.setValue((int) pClan.getTotalDeaths());
  }

  protected override void tooltipAction()
  {
    Tooltip.show((object) this, "clan", new TooltipData()
    {
      clan = this.meta_object
    });
  }

  protected override ActorAsset getActorAsset() => this.meta_object.getActorAsset();
}
