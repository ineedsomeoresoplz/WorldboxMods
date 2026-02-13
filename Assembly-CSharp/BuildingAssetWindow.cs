// Decompiled with JetBrains decompiler
// Type: BuildingAssetWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BuildingAssetWindow : BaseDebugAssetWindow<BuildingAsset, BuildingDebugAssetElement>
{
  public void clickRandomKingdomColor()
  {
    AssetsDebugManager.setRandomKingdomColor(this.asset.civ_kingdom);
    this.asset_debug_element.setData(this.asset);
  }

  protected override void initSprites()
  {
    base.initSprites();
    string pPath = this.asset.sprite_path;
    if (string.IsNullOrEmpty(pPath))
      pPath = this.asset.main_path + this.asset.id;
    foreach (Sprite sprite in SpriteTextureLoader.getSpriteList(pPath))
    {
      SpriteElement spriteElement = Object.Instantiate<SpriteElement>(this.sprite_element_prefab, this.sprite_elements_parent);
      spriteElement.image.sprite = sprite;
      spriteElement.text_name.text = ((Object) sprite).name;
    }
  }

  public static void reloadSprites()
  {
    BaseDebugAssetWindow<BuildingAsset, BuildingDebugAssetElement>.current_element.setData(BaseDebugAssetWindow<BuildingAsset, BuildingDebugAssetElement>.current_element.asset);
  }
}
