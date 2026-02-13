// Decompiled with JetBrains decompiler
// Type: BehRestoreStats
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehRestoreStats : BehaviourActionActor
{
  private readonly float _health;
  private readonly float _mana;

  public BehRestoreStats(float pHealth = 0.0f, float pMana = 0.0f)
  {
    this._health = pHealth;
    this._mana = pMana;
  }

  public override BehResult execute(Actor pActor)
  {
    if ((double) this._health != 0.0)
      pActor.restoreHealthPercent(this._health);
    if ((double) this._mana != 0.0)
      pActor.restoreManaPercent(this._mana);
    return BehResult.Continue;
  }
}
