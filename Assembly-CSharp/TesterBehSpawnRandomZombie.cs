// Decompiled with JetBrains decompiler
// Type: TesterBehSpawnRandomZombie
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class TesterBehSpawnRandomZombie : TesterBehSpawnRandomUnit
{
  public TesterBehSpawnRandomZombie(int pAmount = 1)
    : base(pAmount)
  {
    this.filter_delegate = (ActorAssetFilter) (pActorAsset => !pActorAsset.isTemplateAsset() && pActorAsset.id.Contains("zombie"));
  }
}
