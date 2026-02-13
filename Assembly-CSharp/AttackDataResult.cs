// Decompiled with JetBrains decompiler
// Type: AttackDataResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public struct AttackDataResult(ApplyAttackState pState, long pDeflectedByWhoId = -1)
{
  public long deflected_by_who_id = pDeflectedByWhoId;
  public ApplyAttackState state = pState;

  public static AttackDataResult Continue => new AttackDataResult(ApplyAttackState.Continue);

  public static AttackDataResult Miss => new AttackDataResult(ApplyAttackState.Miss);

  public static AttackDataResult Hit => new AttackDataResult(ApplyAttackState.Hit);

  public static AttackDataResult Block => new AttackDataResult(ApplyAttackState.Block);
}
