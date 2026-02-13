// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehDragonLandAttack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehDragonLandAttack : BehDragon
{
  public override BehResult execute(Actor pActor)
  {
    SpriteAnimation spriteAnimation = this.dragon.spriteAnimation;
    if (spriteAnimation.currentFrameIndex == 4)
      pActor.data.set("shouldAttack", true);
    if (spriteAnimation.currentFrameIndex == 5)
    {
      bool pResult1;
      pActor.data.get("shouldAttack", out pResult1);
      if (pResult1)
      {
        pActor.data.removeBool("shouldAttack");
        foreach (WorldTile landAttackTile in this.dragon.landAttackTiles(pActor.current_tile))
        {
          if (landAttackTile != null && (landAttackTile.hasUnits() || !Randy.randomBool()))
            this.dragon.attackTile(landAttackTile);
        }
        int pResult2;
        pActor.data.get("landAttacks", out pResult2);
        pActor.data.set("landAttacks", ++pResult2);
      }
    }
    return spriteAnimation.currentFrameIndex < spriteAnimation.frames.Length - 1 ? BehResult.RepeatStep : BehResult.Continue;
  }
}
