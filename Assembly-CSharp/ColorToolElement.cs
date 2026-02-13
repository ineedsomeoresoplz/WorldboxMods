// Decompiled with JetBrains decompiler
// Type: ColorToolElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ColorToolElement : MonoBehaviour
{
  [Header("Edit Colors")]
  public Color colorMain;
  public Color colorMain2;
  public Color colorBanner;
  public Color colorText;
  [Header("Edit Asset Name / Id")]
  public string id;
  public bool favorite;
  [Header("Other Stuff")]
  [Space(30f)]
  public Image background;
  public Image icon;
  public Text text;
  public Image sprite_favorite;
  public Image borderInside;
  public Image borderOutside;
  [HideInInspector]
  public ColorAsset color_asset;
  public Image test_house;
  public Image test_face;
  public Sprite house_default_sprite;
  public Sprite face_default_sprite;
  public int debug_index;

  public void createKingdom(ColorAsset pColor) => this.color_asset = pColor;

  public void createCulture(ColorAsset pColor)
  {
    this.color_asset = pColor;
    this.setColorsForObjects(pColor);
    this.saveColors(pColor);
  }

  public void createClans(ColorAsset pColor)
  {
    this.color_asset = pColor;
    string random1 = AssetManager.clan_banners_library.main.backgrounds.GetRandom<string>();
    string random2 = AssetManager.clan_banners_library.main.icons.GetRandom<string>();
    this.background.sprite = SpriteTextureLoader.getSprite(random1);
    this.icon.sprite = SpriteTextureLoader.getSprite(random2);
    this.setColorsForObjects(pColor);
    this.saveColors(pColor);
  }

  private void setColorsForObjects(ColorAsset pColorAsset)
  {
    ((Graphic) this.borderInside).color = Color32.op_Implicit(pColorAsset.getColorBorderInsideAlpha32());
    ((Graphic) this.borderOutside).color = pColorAsset.getColorMainSecond();
    ((Graphic) this.background).color = pColorAsset.getColorMainSecond();
    ((Graphic) this.icon).color = pColorAsset.getColorBanner();
    ((Graphic) this.text).color = pColorAsset.getColorText();
    this.favorite = pColorAsset.favorite;
    this.id = pColorAsset.id;
    this.text.text = $"{pColorAsset.id} |  {pColorAsset.index_id.ToString()}";
    this.debug_index = pColorAsset.index_id;
    if (Object.op_Inequality((Object) this.test_house, (Object) null) && Object.op_Inequality((Object) this.house_default_sprite, (Object) null))
      this.test_house.sprite = DynamicSpriteCreator.createNewSpriteForDebug(this.house_default_sprite, pColorAsset);
    if (Object.op_Inequality((Object) this.test_face, (Object) null) && Object.op_Inequality((Object) this.face_default_sprite, (Object) null))
      this.test_face.sprite = DynamicSpriteCreator.createNewSpriteForDebug(this.face_default_sprite, pColorAsset);
    if (!Object.op_Inequality((Object) this.sprite_favorite, (Object) null))
      return;
    ((Component) this.sprite_favorite).gameObject.SetActive(this.favorite);
  }

  private void OnValidate()
  {
    if (this.color_asset == null)
      return;
    this.color_asset.color_main = Toolbox.colorToHex(Color32.op_Implicit(this.colorMain), false);
    this.color_asset.color_main_2 = Toolbox.colorToHex(Color32.op_Implicit(this.colorMain2), false);
    this.color_asset.color_banner = Toolbox.colorToHex(Color32.op_Implicit(this.colorBanner), false);
    this.color_asset.color_text = Toolbox.colorToHex(Color32.op_Implicit(this.colorText), false);
    this.color_asset.id = this.id;
    this.color_asset.favorite = this.favorite;
    this.color_asset.setEditorColors(this.colorMain, this.colorMain2, this.colorBanner, this.colorText);
    this.setColorsForObjects(this.color_asset);
  }

  private void saveColors(ColorAsset pColor)
  {
    this.colorMain = pColor.getColorMain();
    this.colorMain2 = pColor.getColorMainSecond();
    this.colorBanner = pColor.getColorBanner();
    this.colorText = pColor.getColorText();
  }
}
