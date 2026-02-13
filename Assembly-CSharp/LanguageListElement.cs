// Decompiled with JetBrains decompiler
// Type: LanguageListElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
public class LanguageListElement : WindowListElementBase<Language, LanguageData>
{
  public Text text_name;
  public CountUpOnClick text_age;
  public CountUpOnClick text_population;
  public CountUpOnClick text_books;
  public CountUpOnClick text_villages;
  public CountUpOnClick text_kingdom;

  internal override void show(Language pLanguage)
  {
    base.show(pLanguage);
    this.text_name.text = pLanguage.name;
    ((Graphic) this.text_name).color = pLanguage.getColor().getColorText();
    this.text_age.setValue(pLanguage.getAge());
    this.text_population.setValue(pLanguage.countUnits());
    this.text_villages.setValue(pLanguage.countCities());
    this.text_kingdom.setValue(pLanguage.countKingdoms());
    this.text_books.setValue(pLanguage.books.count());
  }

  protected override void tooltipAction()
  {
    Tooltip.show((object) this, "language", new TooltipData()
    {
      language = this.meta_object
    });
  }

  protected override ActorAsset getActorAsset() => this.meta_object.getActorAsset();
}
