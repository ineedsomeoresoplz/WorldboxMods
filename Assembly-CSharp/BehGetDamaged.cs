// Decompiled with JetBrains decompiler
// Type: BehGetDamaged
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehGetDamaged : BehaviourActionActor
{
  private int _damage;
  private AttackType _attackType;

  public BehGetDamaged(int pDamage, AttackType pAttackType)
  {
    this._damage = pDamage;
    this._attackType = pAttackType;
  }

  public override BehResult execute(Actor pActor)
  {
    pActor.getHit((float) this._damage, pAttackType: this._attackType);
    return pActor.hasHealth() ? BehResult.Continue : BehResult.Stop;
  }
}
