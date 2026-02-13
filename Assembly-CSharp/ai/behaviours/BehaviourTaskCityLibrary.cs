// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehaviourTaskCityLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehaviourTaskCityLibrary : AssetLibrary<BehaviourTaskCity>
{
  public override void init()
  {
    base.init();
    BehaviourTaskCity behaviourTaskCity1 = new BehaviourTaskCity();
    behaviourTaskCity1.id = "nothing";
    BehaviourTaskCity pAsset1 = behaviourTaskCity1;
    this.t = behaviourTaskCity1;
    this.add(pAsset1);
    BehaviourTaskCity behaviourTaskCity2 = new BehaviourTaskCity();
    behaviourTaskCity2.id = "wait1";
    BehaviourTaskCity pAsset2 = behaviourTaskCity2;
    this.t = behaviourTaskCity2;
    this.add(pAsset2);
    this.t.addBeh((BehaviourActionCity) new CityBehRandomWait());
    BehaviourTaskCity behaviourTaskCity3 = new BehaviourTaskCity();
    behaviourTaskCity3.id = "wait5";
    BehaviourTaskCity pAsset3 = behaviourTaskCity3;
    this.t = behaviourTaskCity3;
    this.add(pAsset3);
    this.t.addBeh((BehaviourActionCity) new CityBehRandomWait(5f, 5f));
    BehaviourTaskCity behaviourTaskCity4 = new BehaviourTaskCity();
    behaviourTaskCity4.id = "random_wait_test";
    BehaviourTaskCity pAsset4 = behaviourTaskCity4;
    this.t = behaviourTaskCity4;
    this.add(pAsset4);
    this.t.addBeh((BehaviourActionCity) new CityBehRandomWait(5f, 10f));
    this.t.addBeh((BehaviourActionCity) new CityBehRandomWait(5f, 10f));
    this.t.addBeh((BehaviourActionCity) new CityBehRandomWait(5f, 10f));
    BehaviourTaskCity behaviourTaskCity5 = new BehaviourTaskCity();
    behaviourTaskCity5.id = "do_checks";
    BehaviourTaskCity pAsset5 = behaviourTaskCity5;
    this.t = behaviourTaskCity5;
    this.add(pAsset5);
    this.t.addBeh((BehaviourActionCity) new CityBehCheckLeader());
    this.t.addBeh((BehaviourActionCity) new CityBehRandomWait(0.1f));
    this.t.addBeh((BehaviourActionCity) new CityBehCheckAttackZone());
    this.t.addBeh((BehaviourActionCity) new CityBehRandomWait(0.1f));
    this.t.addBeh((BehaviourActionCity) new CityBehCheckCitizenTasks());
    this.t.addBeh((BehaviourActionCity) new CityBehRandomWait(0.1f));
    BehaviourTaskCity behaviourTaskCity6 = new BehaviourTaskCity();
    behaviourTaskCity6.id = "do_initial_load_check";
    BehaviourTaskCity pAsset6 = behaviourTaskCity6;
    this.t = behaviourTaskCity6;
    this.add(pAsset6);
    this.t.addBeh((BehaviourActionCity) new CityBehCheckCitizenTasks());
    this.t.addBeh((BehaviourActionCity) new CityBehCheckLoyalty());
    BehaviourTaskCity behaviourTaskCity7 = new BehaviourTaskCity();
    behaviourTaskCity7.id = "check_farms";
    BehaviourTaskCity pAsset7 = behaviourTaskCity7;
    this.t = behaviourTaskCity7;
    this.add(pAsset7);
    this.t.addBeh((BehaviourActionCity) new CityBehCheckFarms());
    BehaviourTaskCity behaviourTaskCity8 = new BehaviourTaskCity();
    behaviourTaskCity8.id = "check_loyalty";
    behaviourTaskCity8.single_interval = 2f;
    BehaviourTaskCity pAsset8 = behaviourTaskCity8;
    this.t = behaviourTaskCity8;
    this.add(pAsset8);
    this.t.addBeh((BehaviourActionCity) new CityBehCheckLoyalty());
    BehaviourTaskCity behaviourTaskCity9 = new BehaviourTaskCity();
    behaviourTaskCity9.id = "check_destruction";
    behaviourTaskCity9.single_interval = 2f;
    BehaviourTaskCity pAsset9 = behaviourTaskCity9;
    this.t = behaviourTaskCity9;
    this.add(pAsset9);
    this.t.addBeh((BehaviourActionCity) new CityBehCheckDestruction());
    BehaviourTaskCity behaviourTaskCity10 = new BehaviourTaskCity();
    behaviourTaskCity10.id = "produce_boat";
    BehaviourTaskCity pAsset10 = behaviourTaskCity10;
    this.t = behaviourTaskCity10;
    this.add(pAsset10);
    this.t.addBeh((BehaviourActionCity) new CityBehProduceBoat());
    BehaviourTaskCity behaviourTaskCity11 = new BehaviourTaskCity();
    behaviourTaskCity11.id = "border_shrink";
    BehaviourTaskCity pAsset11 = behaviourTaskCity11;
    this.t = behaviourTaskCity11;
    this.add(pAsset11);
    this.t.addBeh((BehaviourActionCity) new CityBehBorderShrink());
    BehaviourTaskCity behaviourTaskCity12 = new BehaviourTaskCity();
    behaviourTaskCity12.id = "build";
    behaviourTaskCity12.single_interval = 0.0f;
    BehaviourTaskCity pAsset12 = behaviourTaskCity12;
    this.t = behaviourTaskCity12;
    this.add(pAsset12);
    this.t.addBeh((BehaviourActionCity) new CityBehBuild());
    BehaviourTaskCity behaviourTaskCity13 = new BehaviourTaskCity();
    behaviourTaskCity13.id = "supply_kingdom_cities";
    BehaviourTaskCity pAsset13 = behaviourTaskCity13;
    this.t = behaviourTaskCity13;
    this.add(pAsset13);
    this.t.addBeh((BehaviourActionCity) new CityBehSupplyKingdomCities());
    BehaviourTaskCity behaviourTaskCity14 = new BehaviourTaskCity();
    behaviourTaskCity14.id = "produce_resources";
    BehaviourTaskCity pAsset14 = behaviourTaskCity14;
    this.t = behaviourTaskCity14;
    this.add(pAsset14);
    this.t.addBeh((BehaviourActionCity) new CityBehProduceResources());
    BehaviourTaskCity behaviourTaskCity15 = new BehaviourTaskCity();
    behaviourTaskCity15.id = "check_army";
    BehaviourTaskCity pAsset15 = behaviourTaskCity15;
    this.t = behaviourTaskCity15;
    this.add(pAsset15);
    this.t.addBeh((BehaviourActionCity) new CityBehCheckArmy());
  }

  public override void editorDiagnosticLocales()
  {
  }
}
