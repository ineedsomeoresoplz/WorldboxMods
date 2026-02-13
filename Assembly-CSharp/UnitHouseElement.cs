// Decompiled with JetBrains decompiler
// Type: UnitHouseElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UnitHouseElement : UnitElement
{
  [SerializeField]
  private GameObject _title;
  [SerializeField]
  private GameObject _house_container;
  [SerializeField]
  private Image _house_image;

  protected override IEnumerator showContent()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    UnitHouseElement unitHouseElement = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (!unitHouseElement.actor.hasHomeBuilding())
      return false;
    Building homeBuilding = unitHouseElement.actor.getHomeBuilding();
    unitHouseElement.track_objects.Add((NanoObject) homeBuilding);
    unitHouseElement._title.SetActive(true);
    unitHouseElement._house_container.gameObject.SetActive(true);
    unitHouseElement.showSprite(unitHouseElement.actor.kingdom, unitHouseElement._house_image, homeBuilding);
    unitHouseElement.setIconValue("i_house_health", (float) homeBuilding.getHealth(), new float?((float) homeBuilding.getMaxHealth()));
    unitHouseElement.setIconValue("i_house_people", (float) homeBuilding.countResidents(), new float?((float) homeBuilding.asset.housing_slots));
    return false;
  }

  private void setIconValue(
    string pName,
    float pMainVal,
    float? pMax = null,
    string pColor = "",
    bool pFloat = false,
    string pEnding = "",
    char pSeparator = '/')
  {
    Transform recursive = ((Component) this).transform.FindRecursive(pName);
    if (Object.op_Equality((Object) recursive, (Object) null))
      return;
    StatsIcon component = ((Component) recursive).GetComponent<StatsIcon>();
    ((Component) component).gameObject.SetActive(true);
    component.setValue(pMainVal, pMax, pColor, pFloat, pEnding, pSeparator);
  }

  private void showSprite(Kingdom pKingdom, Image pImage, Building pBuilding)
  {
    BuildingAsset asset = pBuilding.asset;
    Sprite recoloredBuilding = DynamicSprites.getRecoloredBuilding(asset.building_sprites.animation_data[pBuilding.animData_index].main.GetRandom<Sprite>(), pKingdom.getColor(), asset.atlas_asset);
    pImage.sprite = recoloredBuilding;
    ((Graphic) pImage).SetNativeSize();
    float num = Mathf.Min(28f / ((Graphic) pImage).rectTransform.sizeDelta.x, 28f / ((Graphic) pImage).rectTransform.sizeDelta.y);
    ((Graphic) pImage).rectTransform.sizeDelta = new Vector2(((Graphic) pImage).rectTransform.sizeDelta.x * num, ((Graphic) pImage).rectTransform.sizeDelta.y * num);
  }

  protected override void clear()
  {
    this._title.SetActive(false);
    this._house_container.SetActive(false);
    base.clear();
  }

  public override bool checkRefreshWindow()
  {
    return this._house_container.activeSelf && !this.actor.hasHomeBuilding() || base.checkRefreshWindow();
  }
}
