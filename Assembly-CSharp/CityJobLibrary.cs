// Decompiled with JetBrains decompiler
// Type: CityJobLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class CityJobLibrary : AssetLibrary<JobCityAsset>
{
  public override void init()
  {
    base.init();
    JobCityAsset pAsset = new JobCityAsset();
    pAsset.id = "city";
    this.add(pAsset);
    this.t.addTask("check_army");
    this.t.addTask("wait1");
    this.t.addTask("do_checks");
    this.t.addTask("wait1");
    this.t.addTask("border_shrink");
    this.t.addTask("wait1");
    this.t.addTask("produce_boat");
    this.t.addTask("wait1");
    this.t.addTask("supply_kingdom_cities");
    this.t.addTask("wait1");
    this.t.addTask("produce_resources");
    this.t.addTask("wait1");
    this.t.addTask("check_farms");
  }
}
