// Decompiled with JetBrains decompiler
// Type: DeadKingdom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class DeadKingdom : Kingdom
{
  public override void loadData(KingdomData pData)
  {
    this.setData(pData);
    this.data.load();
    ActorAsset actorAsset = this.getActorAsset();
    this.asset = AssetManager.kingdoms.get(actorAsset.kingdom_id_civilization);
  }

  public override int getAge()
  {
    int year = Date.getYear(this.data.created_time);
    return Date.getYear(this.data.died_time) - year;
  }

  public override string getMotto() => this.data.motto;
}
