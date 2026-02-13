// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehDragonSetAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehDragonSetAnimation : BehDragon
{
  private DragonState state;
  private bool looped;
  private bool forceRestart;

  public BehDragonSetAnimation(DragonState pState, bool pLooped = true, bool pForceRestart = true)
  {
    this.state = pState;
    this.looped = pLooped;
    this.forceRestart = pForceRestart;
  }

  public override BehResult execute(Actor pActor)
  {
    if (pActor.flipAnimationActive())
      return BehResult.RepeatStep;
    SpriteAnimation spriteAnimation = this.dragon.spriteAnimation;
    this.dragon.setFrames(this.state, this.forceRestart);
    int num = this.looped ? 1 : 0;
    spriteAnimation.looped = num != 0;
    return BehResult.Continue;
  }
}
