// Decompiled with JetBrains decompiler
// Type: CityBuildingIcons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CityBuildingIcons : CityElement
{
  public Image top;
  public Image topLeft;
  public Image topRight;
  public Image left;
  public Image right;
  public Image bottom;
  public Image bottomLeft;
  public Image bottomRight;
  public Image centerBonfire;
  private Image[] _list_house_images;
  private List<SpriteAnimation> _sprite_animations = new List<SpriteAnimation>();

  protected override void Awake()
  {
    this._list_house_images = new Image[8]
    {
      this.top,
      this.topRight,
      this.right,
      this.bottomRight,
      this.bottom,
      this.bottomLeft,
      this.left,
      this.topLeft
    };
    base.Awake();
  }

  private void Update()
  {
    foreach (SpriteAnimation spriteAnimation in this._sprite_animations)
      spriteAnimation.update(Time.deltaTime);
  }

  protected override void clear()
  {
    foreach (Component listHouseImage in this._list_house_images)
      listHouseImage.gameObject.SetActive(false);
    this._sprite_animations.Clear();
  }

  protected override IEnumerator showContent()
  {
    ((Graphic) this.centerBonfire).SetNativeSize();
    this._sprite_animations.Add(((Component) this.centerBonfire).GetComponent<SpriteAnimation>());
    bool flag;
    using (ListPool<string> pPossibleIDs = this.getCityBuildingTypes())
    {
      if (pPossibleIDs.Count == 0)
      {
        flag = false;
      }
      else
      {
        Image[] imageArray = this._list_house_images;
        for (int index = 0; index < imageArray.Length; ++index)
        {
          Image tImage = imageArray[index];
          string tID = pPossibleIDs.GetRandom<string>();
          yield return (object) new WaitForEndOfFrame();
          this.showSprite(tImage, tID);
          tID = (string) null;
          tImage = (Image) null;
        }
        imageArray = (Image[]) null;
        flag = false;
      }
    }
    return flag;
  }

  private void showSprite(Image pImage, string pAssetID)
  {
    BuildingAsset buildingAsset = AssetManager.buildings.get(pAssetID);
    Sprite recoloredBuilding = DynamicSprites.getRecoloredBuilding(buildingAsset.building_sprites.animation_data.GetRandom<BuildingAnimationData>().main.GetRandom<Sprite>(), this.city.kingdom?.getColor(), buildingAsset.atlas_asset);
    pImage.sprite = recoloredBuilding;
    ((Graphic) pImage).SetNativeSize();
    ((Component) pImage).gameObject.SetActive(true);
  }

  private ListPool<string> getCityBuildingTypes()
  {
    ListPool<string> cityBuildingTypes = new ListPool<string>(this.city.buildings.Count);
    foreach (Building building in this.city.buildings)
    {
      if (building.asset.hasHousingSlots() && building.asset.id.Contains("house"))
        cityBuildingTypes.Add(building.asset.id);
    }
    return cityBuildingTypes;
  }
}
