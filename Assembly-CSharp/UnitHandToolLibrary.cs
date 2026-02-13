// Decompiled with JetBrains decompiler
// Type: UnitHandToolLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class UnitHandToolLibrary : AssetLibrary<UnitHandToolAsset>
{
  public override void init()
  {
    base.init();
    UnitHandToolAsset pAsset1 = new UnitHandToolAsset();
    pAsset1.id = "flag";
    pAsset1.animated = true;
    pAsset1.colored = true;
    this.add(pAsset1);
    UnitHandToolAsset pAsset2 = new UnitHandToolAsset();
    pAsset2.id = "axe";
    this.add(pAsset2);
    UnitHandToolAsset pAsset3 = new UnitHandToolAsset();
    pAsset3.id = "basket";
    this.add(pAsset3);
    UnitHandToolAsset pAsset4 = new UnitHandToolAsset();
    pAsset4.id = "book";
    this.add(pAsset4);
    UnitHandToolAsset pAsset5 = new UnitHandToolAsset();
    pAsset5.id = "bucket";
    this.add(pAsset5);
    UnitHandToolAsset pAsset6 = new UnitHandToolAsset();
    pAsset6.id = "coffee_cup";
    pAsset6.animated = true;
    this.add(pAsset6);
    UnitHandToolAsset pAsset7 = new UnitHandToolAsset();
    pAsset7.id = "hammer";
    this.add(pAsset7);
    UnitHandToolAsset pAsset8 = new UnitHandToolAsset();
    pAsset8.id = "hoe";
    this.add(pAsset8);
    UnitHandToolAsset pAsset9 = new UnitHandToolAsset();
    pAsset9.id = "notebook";
    this.add(pAsset9);
    UnitHandToolAsset pAsset10 = new UnitHandToolAsset();
    pAsset10.id = "pickaxe";
    this.add(pAsset10);
  }

  public override void post_init()
  {
    base.post_init();
    foreach (UnitHandToolAsset unitHandToolAsset in this.list)
    {
      if (string.IsNullOrEmpty(unitHandToolAsset.path_gameplay_sprite))
        unitHandToolAsset.path_gameplay_sprite = "items/tools/tool_" + unitHandToolAsset.id;
    }
  }

  public void loadSprites()
  {
    foreach (UnitHandToolAsset unitHandToolAsset in this.list)
      unitHandToolAsset.gameplay_sprites = SpriteTextureLoader.getSpriteList(unitHandToolAsset.path_gameplay_sprite);
  }
}
