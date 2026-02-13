// Decompiled with JetBrains decompiler
// Type: CitizenJobLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class CitizenJobLibrary : AssetLibrary<CitizenJobAsset>
{
  public List<CitizenJobAsset> list_priority_normal;
  public List<CitizenJobAsset> list_priority_high;
  public List<CitizenJobAsset> list_priority_high_food;
  public static CitizenJobAsset builder;
  public static CitizenJobAsset gatherer_bushes;
  public static CitizenJobAsset gatherer_herbs;
  public static CitizenJobAsset gatherer_honey;
  public static CitizenJobAsset farmer;
  public static CitizenJobAsset hunter;
  public static CitizenJobAsset woodcutter;
  public static CitizenJobAsset miner;
  public static CitizenJobAsset miner_deposit;
  public static CitizenJobAsset road_builder;
  public static CitizenJobAsset cleaner;
  public static CitizenJobAsset manure_cleaner;
  public static CitizenJobAsset attacker;

  public override void init()
  {
    base.init();
    CitizenJobAsset pAsset1 = new CitizenJobAsset();
    pAsset1.id = "builder";
    pAsset1.priority = 9;
    pAsset1.debug_option = DebugOption.CitizenJobBuilder;
    pAsset1.path_icon = "ui/Icons/citizen_jobs/iconCitizenJobBuilder";
    CitizenJobLibrary.builder = this.add(pAsset1);
    CitizenJobAsset pAsset2 = new CitizenJobAsset();
    pAsset2.id = "gatherer_bushes";
    pAsset2.priority_no_food = 10;
    pAsset2.debug_option = DebugOption.CitizenJobGathererBushes;
    pAsset2.path_icon = "ui/Icons/citizen_jobs/iconCitizenJobGathererBushes";
    CitizenJobLibrary.gatherer_bushes = this.add(pAsset2);
    CitizenJobAsset pAsset3 = new CitizenJobAsset();
    pAsset3.id = "gatherer_herbs";
    pAsset3.priority_no_food = 10;
    pAsset3.debug_option = DebugOption.CitizenJobGathererHerbs;
    pAsset3.path_icon = "ui/Icons/citizen_jobs/iconCitizenJobGathererHerbs";
    CitizenJobLibrary.gatherer_herbs = this.add(pAsset3);
    CitizenJobAsset pAsset4 = new CitizenJobAsset();
    pAsset4.id = "gatherer_honey";
    pAsset4.priority_no_food = 10;
    pAsset4.debug_option = DebugOption.CitizenJobGathererHoney;
    pAsset4.path_icon = "ui/Icons/citizen_jobs/iconCitizenJobGathererHoney";
    CitizenJobLibrary.gatherer_honey = this.add(pAsset4);
    CitizenJobAsset pAsset5 = new CitizenJobAsset();
    pAsset5.id = "farmer";
    pAsset5.ok_for_king = false;
    pAsset5.ok_for_leader = false;
    pAsset5.debug_option = DebugOption.CitizenJobFarmer;
    pAsset5.path_icon = "ui/Icons/citizen_jobs/iconCitizenJobFarmer";
    CitizenJobLibrary.farmer = this.add(pAsset5);
    CitizenJobAsset pAsset6 = new CitizenJobAsset();
    pAsset6.id = "hunter";
    pAsset6.debug_option = DebugOption.CitizenJobHunter;
    pAsset6.path_icon = "ui/Icons/citizen_jobs/iconCitizenJobHunter";
    CitizenJobLibrary.hunter = this.add(pAsset6);
    CitizenJobAsset pAsset7 = new CitizenJobAsset();
    pAsset7.id = "woodcutter";
    pAsset7.debug_option = DebugOption.CitizenJobWoodcutter;
    pAsset7.path_icon = "ui/Icons/citizen_jobs/iconCitizenJobWoodcutter";
    CitizenJobLibrary.woodcutter = this.add(pAsset7);
    CitizenJobAsset pAsset8 = new CitizenJobAsset();
    pAsset8.id = "miner";
    pAsset8.ok_for_king = false;
    pAsset8.ok_for_leader = false;
    pAsset8.debug_option = DebugOption.CitizenJobMiner;
    pAsset8.path_icon = "ui/Icons/citizen_jobs/iconCitizenJobMiner";
    CitizenJobLibrary.miner = this.add(pAsset8);
    CitizenJobAsset pAsset9 = new CitizenJobAsset();
    pAsset9.id = "miner_deposit";
    pAsset9.ok_for_king = false;
    pAsset9.ok_for_leader = false;
    pAsset9.debug_option = DebugOption.CitizenJobMinerDeposit;
    pAsset9.path_icon = "ui/Icons/citizen_jobs/iconCitizenJobMinerDeposit";
    CitizenJobLibrary.miner_deposit = this.add(pAsset9);
    CitizenJobAsset pAsset10 = new CitizenJobAsset();
    pAsset10.id = "road_builder";
    pAsset10.debug_option = DebugOption.CitizenJobRoadBuilder;
    pAsset10.path_icon = "ui/Icons/citizen_jobs/iconCitizenJobRoadBuilder";
    CitizenJobLibrary.road_builder = this.add(pAsset10);
    CitizenJobAsset pAsset11 = new CitizenJobAsset();
    pAsset11.id = "cleaner";
    pAsset11.debug_option = DebugOption.CitizenJobCleaner;
    pAsset11.path_icon = "ui/Icons/citizen_jobs/iconCitizenJobCleaner";
    CitizenJobLibrary.cleaner = this.add(pAsset11);
    CitizenJobAsset pAsset12 = new CitizenJobAsset();
    pAsset12.id = "manure_cleaner";
    pAsset12.debug_option = DebugOption.CitizenJobManureCleaner;
    pAsset12.path_icon = "ui/Icons/citizen_jobs/iconCitizenJobManureCleaner";
    CitizenJobLibrary.manure_cleaner = this.add(pAsset12);
    CitizenJobAsset pAsset13 = new CitizenJobAsset();
    pAsset13.id = "attacker";
    pAsset13.debug_option = DebugOption.CitizenJobAttacker;
    pAsset13.path_icon = "ui/Icons/citizen_jobs/iconCitizenJobAttacker";
    pAsset13.common_job = false;
    CitizenJobLibrary.attacker = this.add(pAsset13);
  }

  public override void post_init()
  {
    base.post_init();
    foreach (CitizenJobAsset citizenJobAsset in this.list)
    {
      if (citizenJobAsset.common_job)
        citizenJobAsset.unit_job_default = citizenJobAsset.id;
    }
  }

  public override void linkAssets()
  {
    base.linkAssets();
    this.list_priority_normal = new List<CitizenJobAsset>();
    this.list_priority_high = new List<CitizenJobAsset>();
    this.list_priority_high_food = new List<CitizenJobAsset>();
    foreach (CitizenJobAsset citizenJobAsset in this.list)
    {
      if (citizenJobAsset.common_job)
      {
        if (citizenJobAsset.priority_no_food > 0)
          this.list_priority_high_food.Add(citizenJobAsset);
        if (citizenJobAsset.priority > 0)
          this.list_priority_high.Add(citizenJobAsset);
        else
          this.list_priority_normal.Add(citizenJobAsset);
      }
    }
  }
}
