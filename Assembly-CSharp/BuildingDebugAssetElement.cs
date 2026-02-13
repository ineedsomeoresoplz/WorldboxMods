// Decompiled with JetBrains decompiler
// Type: BuildingDebugAssetElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class BuildingDebugAssetElement : BaseDebugAssetElement<BuildingAsset>
{
  public BuildingDebugAnimationElement spawn;
  public BuildingDebugAnimationElement main;
  public BuildingDebugAnimationElement disabled;
  public BuildingDebugAnimationElement ruin;
  public BuildingDebugAnimationElement special;
  public Image construction;
  public Image mini;

  public override void setData(BuildingAsset pAsset)
  {
    this.asset = pAsset;
    this.title.text = this.asset.id;
    this.initAnimations();
    this.initStats();
  }

  protected override void initAnimations()
  {
    BuildingSprites buildingSprites = this.asset.building_sprites;
    this.spawn.setData(this.asset);
    this.main.setData(this.asset);
    this.disabled.setData(this.asset);
    this.ruin.setData(this.asset);
    this.special.setData(this.asset);
    List<DebugAnimatedVariation> pVariations1 = new List<DebugAnimatedVariation>();
    List<DebugAnimatedVariation> pVariations2 = new List<DebugAnimatedVariation>();
    List<DebugAnimatedVariation> pVariations3 = new List<DebugAnimatedVariation>();
    List<DebugAnimatedVariation> pVariations4 = new List<DebugAnimatedVariation>();
    List<DebugAnimatedVariation> pVariations5 = new List<DebugAnimatedVariation>();
    foreach (BuildingAnimationData buildingAnimationData in this.asset.building_sprites.animation_data)
    {
      pVariations1.Add(new DebugAnimatedVariation(this.getBuildingColoredSprites(buildingAnimationData.spawn), buildingAnimationData.animated));
      pVariations2.Add(new DebugAnimatedVariation(this.getBuildingColoredSprites(buildingAnimationData.main), buildingAnimationData.animated));
      pVariations3.Add(new DebugAnimatedVariation(this.getBuildingColoredSprites(buildingAnimationData.main_disabled), buildingAnimationData.animated));
      pVariations4.Add(new DebugAnimatedVariation(this.getBuildingColoredSprites(buildingAnimationData.ruins), buildingAnimationData.animated));
      pVariations5.Add(new DebugAnimatedVariation(this.getBuildingColoredSprites(buildingAnimationData.special), buildingAnimationData.animated));
    }
    this.spawn.setFrames(pVariations1, this.asset.has_sprites_spawn);
    this.main.setFrames(pVariations2, this.asset.has_sprites_main);
    this.disabled.setFrames(pVariations3, this.asset.has_sprites_main_disabled);
    this.ruin.setFrames(pVariations4, this.asset.has_sprites_ruin);
    this.special.setFrames(pVariations5, this.asset.has_sprites_special);
    if (Object.op_Inequality((Object) buildingSprites.construction, (Object) null))
      this.construction.sprite = buildingSprites.construction;
    else if (this.asset.has_sprite_construction)
      this.construction.sprite = this.no_animation;
    else
      ((Graphic) this.construction).color = Color.clear;
    this.mini.sprite = this.loadMini();
  }

  private Sprite loadMini()
  {
    string str = this.asset.sprite_path;
    if (string.IsNullOrEmpty(str))
      str = this.asset.main_path + this.asset.id;
    Sprite sprite = SpriteTextureLoader.getSprite(str + "/mini_0");
    if (Object.op_Equality((Object) sprite, (Object) null))
    {
      Debug.LogError((object) ("Not found mini sprite for building: " + this.asset.id));
      return sprite;
    }
    KingdomAsset kingdomAsset = AssetManager.kingdoms.get("mad");
    if (!this.asset.has_kingdom_color)
      return sprite;
    ColorAsset debugColorAsset = kingdomAsset.debug_color_asset;
    Texture2D texture2D = new Texture2D(((Texture) sprite.texture).width, ((Texture) sprite.texture).height);
    ((Texture) texture2D).filterMode = ((Texture) sprite.texture).filterMode;
    for (int index1 = 0; index1 < ((Texture) texture2D).width; ++index1)
    {
      for (int index2 = 0; index2 < ((Texture) texture2D).height; ++index2)
      {
        Color color = Color32.op_Implicit(this.getColor(sprite.texture.GetPixel(index1, index2), debugColorAsset));
        texture2D.SetPixel(index1, index2, color);
      }
    }
    texture2D.Apply();
    return Sprite.Create(texture2D, new Rect(Vector2.zero, new Vector2((float) ((Texture) texture2D).width, (float) ((Texture) texture2D).height)), new Vector2(0.5f, 0.5f), 1f);
  }

  private Color32 getColor(Color pOrigColor, ColorAsset pKingdomColor)
  {
    if (Toolbox.areColorsEqual(Color32.op_Implicit(pOrigColor), Toolbox.color_magenta_0))
      pOrigColor = Color32.op_Implicit(pKingdomColor.k_color_0);
    else if (Toolbox.areColorsEqual(Color32.op_Implicit(pOrigColor), Toolbox.color_magenta_1))
      pOrigColor = Color32.op_Implicit(pKingdomColor.k_color_1);
    else if (Toolbox.areColorsEqual(Color32.op_Implicit(pOrigColor), Toolbox.color_magenta_2))
      pOrigColor = Color32.op_Implicit(pKingdomColor.k_color_2);
    else if (Toolbox.areColorsEqual(Color32.op_Implicit(pOrigColor), Toolbox.color_magenta_3))
      pOrigColor = Color32.op_Implicit(pKingdomColor.k_color_3);
    else if (Toolbox.areColorsEqual(Color32.op_Implicit(pOrigColor), Toolbox.color_magenta_4))
      pOrigColor = Color32.op_Implicit(pKingdomColor.k_color_4);
    return Color32.op_Implicit(pOrigColor);
  }

  public override void update()
  {
    if (!((Component) this).gameObject.activeSelf)
      return;
    this.spawn.update();
    this.main.update();
    this.disabled.update();
    this.ruin.update();
    this.special.update();
  }

  public override void stopAnimations()
  {
    this.spawn.stopAnimations();
    this.main.stopAnimations();
    this.disabled.stopAnimations();
    this.ruin.stopAnimations();
    this.special.stopAnimations();
  }

  public override void startAnimations()
  {
    this.spawn.startAnimations();
    this.main.startAnimations();
    this.disabled.startAnimations();
    this.ruin.startAnimations();
    this.special.startAnimations();
  }

  private Sprite[] getBuildingColoredSprites(Sprite[] pSprites)
  {
    if (pSprites == null)
      return new Sprite[0];
    Sprite[] buildingColoredSprites = new Sprite[pSprites.Length];
    for (int index = 0; index < pSprites.Length; ++index)
      buildingColoredSprites[index] = this.getBuildingColoredSprite(pSprites[index]);
    return buildingColoredSprites;
  }

  private Sprite getBuildingColoredSprite(Sprite pMainSprite)
  {
    ColorAsset pColor = (ColorAsset) null;
    if (this.asset.has_kingdom_color)
      pColor = AssetManager.kingdoms.get("mad").debug_color_asset;
    return DynamicSprites.getRecoloredBuilding(pMainSprite, pColor, this.asset.atlas_asset);
  }

  protected override void initStats()
  {
    base.initStats();
    this.showStat("health", (object) this.asset.base_stats["health"]);
    this.showStat("damage", (object) this.asset.base_stats["damage"]);
    this.showStat("targets", (object) this.asset.base_stats["targets"]);
    this.showStat("area_of_effect", (object) this.asset.base_stats["area_of_effect"]);
  }

  protected override void showAssetWindow()
  {
    base.showAssetWindow();
    ScrollWindow.showWindow("building_asset");
  }
}
