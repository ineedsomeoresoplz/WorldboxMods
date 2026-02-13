// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFingerSetFlying
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehFingerSetFlying : BehFinger
{
  private bool _flying;
  private float _height_target = -1f;

  public BehFingerSetFlying(bool pFlying, float pHeightTarget = -1f)
  {
    this._flying = pFlying;
    if ((double) pHeightTarget <= -1.0)
      return;
    this._height_target = pHeightTarget;
  }

  public override BehResult execute(Actor pActor)
  {
    pActor.setFlying(this._flying);
    if (this._flying)
    {
      if ((double) this._height_target > -1.0)
        this.finger.flying_target = this._height_target;
      else
        this.finger.flying_target = Randy.randomFloat(5f, 13f);
    }
    else
      this.finger.flying_target = 0.3f;
    return BehResult.Continue;
  }
}
