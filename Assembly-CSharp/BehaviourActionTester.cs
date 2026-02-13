// Decompiled with JetBrains decompiler
// Type: BehaviourActionTester
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class BehaviourActionTester : BehaviourActionBase<AutoTesterBot>
{
  public bool null_check_tile_target;

  public override bool errorsFound(AutoTesterBot pObject)
  {
    return this.null_check_tile_target && pObject.beh_tile_target == null || base.errorsFound(pObject);
  }
}
