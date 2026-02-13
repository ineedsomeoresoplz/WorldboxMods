// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFingerCheckCanDraw
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehFingerCheckCanDraw : BehFingerDrawAction
{
  protected override void setupErrorChecks()
  {
    this.check_has_target_tiles = true;
    this.check_current_tile_in_target_tiles = true;
    this.check_target_tile_in_target_tiles = false;
    base.setupErrorChecks();
  }
}
