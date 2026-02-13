// Decompiled with JetBrains decompiler
// Type: TesterBehCullUnits
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class TesterBehCullUnits : BehaviourActionTester
{
  private string _actor_asset_id;

  public TesterBehCullUnits(string pActorAssetId) => this._actor_asset_id = pActorAssetId;

  public override BehResult execute(AutoTesterBot pObject)
  {
    foreach (Actor unit in AssetManager.actor_library.get(this._actor_asset_id).units)
    {
      if (!unit.isRekt() && !Randy.randomChance(0.1f))
        unit.getHit(10000f, false, AttackType.Divine, (BaseSimObject) null, true, false, true);
    }
    return base.execute(pObject);
  }
}
