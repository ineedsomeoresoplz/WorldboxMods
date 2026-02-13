// Decompiled with JetBrains decompiler
// Type: ActorAssetWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ActorAssetWindow : BaseDebugAssetWindow<ActorAsset, ActorDebugAssetElement>
{
  public void clickRandomKingdomColor()
  {
    AssetsDebugManager.setRandomKingdomColor(this.asset.kingdom_id_wild);
    this.asset_debug_element.setData(this.asset);
  }

  public void clickRandomSkinColor()
  {
    AssetsDebugManager.setRandomSkinColor(this.asset);
    this.asset_debug_element.setData(this.asset);
  }

  public void clickChangeSex()
  {
    AssetsDebugManager.changeSex();
    this.asset_debug_element.setData(this.asset);
  }

  protected override void initSprites()
  {
    base.initSprites();
    string pPath = this.asset.texture_asset.texture_path_base;
    if (new List<string>()
    {
      "dragon",
      "zombie_dragon",
      "worm"
    }.Contains(this.asset.id))
      pPath = "actors_special/t_" + this.asset.id;
    if (this.asset.is_boat)
      pPath = "actors/boats/" + this.asset.id;
    switch (this.asset.id)
    {
      case "UFO":
        pPath = "actors/special/t_ufo";
        break;
      case "crabzilla":
        pPath = "actors/special/crab";
        break;
      case "god_finger":
        pPath = "actors/species/other/god_finger";
        break;
    }
    foreach (Sprite sprite in SpriteTextureLoader.getSpriteList(pPath))
    {
      SpriteElement spriteElement = Object.Instantiate<SpriteElement>(this.sprite_element_prefab, this.sprite_elements_parent);
      spriteElement.image.sprite = sprite;
      spriteElement.text_name.text = ((Object) sprite).name;
    }
  }
}
