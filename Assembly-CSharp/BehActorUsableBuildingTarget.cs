// Decompiled with JetBrains decompiler
// Type: BehActorUsableBuildingTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class BehActorUsableBuildingTarget : BehActorBuildingTarget
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.check_building_target_non_usable = true;
  }
}
