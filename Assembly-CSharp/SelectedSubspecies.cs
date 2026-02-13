// Decompiled with JetBrains decompiler
// Type: SelectedSubspecies
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SelectedSubspecies : SelectedMeta<Subspecies, SubspeciesData>
{
  [SerializeField]
  private CitiesKingdomsContainersController _banners_cities_kingdoms;
  private SubspeciesSelectedContainerBirthTraits _container_traits_birth;

  protected override MetaType meta_type => MetaType.Subspecies;

  protected override string getPowerTabAssetID() => "selected_subspecies";

  protected override void Awake()
  {
    base.Awake();
    this._container_traits_birth = ((Component) this).GetComponentInChildren<SubspeciesSelectedContainerBirthTraits>();
  }

  protected override void updateTraits()
  {
    base.updateTraits();
    if (Object.op_Equality((Object) this._container_traits_birth, (Object) null))
      return;
    this._container_traits_birth.update((NanoObject) this.nano_object);
  }

  protected override void showStatsGeneral(Subspecies pSubspecies)
  {
    base.showStatsGeneral(pSubspecies);
    this.setIconValue("i_kingdoms", (float) pSubspecies.countMainKingdoms());
    this.setIconValue("i_villages", (float) pSubspecies.countMainCities());
  }

  public void openBirthTraitsTab()
  {
    ScrollWindow.showWindow(this.window_id);
    ScrollWindow.getCurrentWindow().tabs.showTab("BirthTraitsEditor");
  }

  public void openGeneticsTab()
  {
    ScrollWindow.showWindow(this.window_id);
    ScrollWindow.getCurrentWindow().tabs.showTab("Genetics");
  }

  protected override void updateElementsOnChange(Subspecies pNano)
  {
    base.updateElementsOnChange(pNano);
    this._banners_cities_kingdoms.update((NanoObject) pNano);
  }

  protected override void checkAchievements(Subspecies pNano)
  {
    AchievementLibrary.checkSubspeciesAchievements(pNano);
  }
}
