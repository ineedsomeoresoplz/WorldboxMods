// Decompiled with JetBrains decompiler
// Type: AntimatterBombEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class AntimatterBombEffect : BaseEffect
{
  private bool used;

  private void Update()
  {
    World.world.startShake(0.03f, pIntensity: 0.3f);
    if (this.sprite_animation.currentFrameIndex < 6 || this.used)
      return;
    this.used = true;
    World.world.applyForceOnTile(this.tile, pForceAmount: 0.0f, pDamage: 1000);
    World.world.loopWithBrush(this.tile, Brush.get(11), new PowerActionWithID(this.tileAntimatter));
  }

  public bool tileAntimatter(WorldTile pTile, string pPowerID)
  {
    TileType pType = TileLibrary.pit_deep_ocean;
    bool pSkipTerraform = false;
    if (!MapAction.checkTileDamageGaiaCovenant(pTile, true))
    {
      pType = (TileType) null;
      pSkipTerraform = true;
    }
    MapAction.terraformMain(pTile, pType, TerraformLibrary.destroy_no_flash, pSkipTerraform);
    return true;
  }

  internal override void spawnOnTile(WorldTile pTile)
  {
    this.tile = pTile;
    this.used = false;
    this.prepare(pTile);
    this.resetAnim();
  }
}
