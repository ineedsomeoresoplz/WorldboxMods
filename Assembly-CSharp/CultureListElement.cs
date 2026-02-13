// Decompiled with JetBrains decompiler
// Type: CultureListElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CultureListElement : WindowListElementBase<Culture, CultureData>
{
  public Text name;
  public CountUpOnClick textFollowers;
  public CountUpOnClick textCities;
  public CountUpOnClick textRenown;
  public CountUpOnClick textAge;
  public CountUpOnClick textBooks;

  internal override void show(Culture pCulture)
  {
    base.show(pCulture);
    this.name.text = pCulture.data.name;
    ((Graphic) this.name).color = pCulture.getColor().getColorText();
    this.textAge.setValue(pCulture.getAge());
    this.textFollowers.setValue(pCulture.countUnits());
    this.textRenown.setValue(pCulture.getRenown());
    this.textCities.setValue(pCulture.countCities());
    this.textBooks.setValue(pCulture.books.count());
  }

  protected override void OnDisable()
  {
    ShortcutExtensions.DOKill((Component) this.textFollowers, false);
    ShortcutExtensions.DOKill((Component) this.textCities, false);
    ShortcutExtensions.DOKill((Component) this.textRenown, false);
    ShortcutExtensions.DOKill((Component) this.textAge, false);
    ShortcutExtensions.DOKill((Component) this.textBooks, false);
    base.OnDisable();
  }

  protected override void tooltipAction()
  {
    Tooltip.show((object) this, "culture", new TooltipData()
    {
      culture = this.meta_object
    });
  }

  protected override ActorAsset getActorAsset() => this.meta_object.getActorAsset();
}
