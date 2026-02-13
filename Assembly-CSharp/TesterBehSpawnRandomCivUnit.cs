// Decompiled with JetBrains decompiler
// Type: TesterBehSpawnRandomCivUnit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class TesterBehSpawnRandomCivUnit : TesterBehSpawnRandomUnit
{
  public TesterBehSpawnRandomCivUnit(int pAmount = 1, string pLocation = "random")
    : base(pAmount, pLocation)
  {
    this.filter_delegate = (ActorAssetFilter) (pActorAsset => !pActorAsset.isTemplateAsset() && pActorAsset.has_ai_system && !pActorAsset.is_boat && !pActorAsset.unit_other && !pActorAsset.special && !pActorAsset.id.Contains("zombie") && pActorAsset.civ);
  }
}
