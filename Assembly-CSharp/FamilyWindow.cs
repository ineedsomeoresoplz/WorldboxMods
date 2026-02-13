// Decompiled with JetBrains decompiler
// Type: FamilyWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
public class FamilyWindow : WindowMetaGeneric<Family, FamilyData>
{
  public Text title_family;

  public override MetaType meta_type => MetaType.Family;

  protected override Family meta_object => SelectedMetas.selected_family;

  protected override void showTopPartInformation()
  {
    base.showTopPartInformation();
    Family metaObject = this.meta_object;
    if (metaObject == null)
      return;
    this.title_family.text = LocalizedTextManager.getText(metaObject.getActorAsset().getCollectiveTermID());
  }

  internal override void showStatsRows()
  {
    Family metaObject = this.meta_object;
    this.tryShowPastNames();
    this.showStatRow("founded", (object) metaObject.getFoundedDate(), MetaType.None, -1L, "iconAge", (string) null, (TooltipDataGetter) null);
    this.tryToShowActor("founder", metaObject.data.main_founder_id_1, metaObject.data.founder_actor_name_1, pIconPath: "actor_traits/iconStupid");
    if (metaObject.data.main_founder_id_2 != -1L)
      this.tryToShowActor("founder", metaObject.data.main_founder_id_2, metaObject.data.founder_actor_name_2, pIconPath: "actor_traits/iconStupid");
    this.tryToShowMetaKingdom("origin", metaObject.data.founder_kingdom_id, metaObject.data.founder_kingdom_name);
    this.tryToShowMetaCity("birthplace", metaObject.data.founder_city_id, metaObject.data.founder_city_name);
    this.tryToShowMetaSubspecies("founder_subspecies", metaObject.data.subspecies_id, metaObject.data.subspecies_name);
    foreach (Family originFamily in metaObject.getOriginFamilies())
      this.tryToShowMetaFamily("origin_family", pObject: originFamily);
    this.tryToShowMetaSpecies("founder_species", metaObject.data.species_id);
  }
}
