// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehDragonSlide
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehDragonSlide : BehDragon
{
  public override BehResult execute(Actor pActor)
  {
    SpriteAnimation spriteAnimation = this.dragon.spriteAnimation;
    if (spriteAnimation.currentFrameIndex == 7)
    {
      foreach (WorldTile pTile in this.dragon.attackRange(pActor.flip))
      {
        if (pTile != null && (pTile.hasUnits() || !Randy.randomBool()))
          this.dragon.attackTile(pTile);
      }
    }
    return spriteAnimation.currentFrameIndex < spriteAnimation.frames.Length - 1 ? BehResult.RepeatStep : BehResult.Continue;
  }
}
