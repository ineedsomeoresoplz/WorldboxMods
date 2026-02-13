// Decompiled with JetBrains decompiler
// Type: TesterJobLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class TesterJobLibrary : AssetLibrary<JobTesterAsset>
{
  private int _last_job;

  public override void init()
  {
    base.init();
    JobTesterAsset pAsset1 = new JobTesterAsset();
    pAsset1.id = "test_spawn_units";
    this.add(pAsset1);
    for (int index = 0; index < 50; ++index)
    {
      this.t.addTask("spawn_random_unit");
      this.t.addTask("wait_1");
    }
    this.t.addTask("super_damage_units");
    this.t.addTask("end_test");
    JobTesterAsset pAsset2 = new JobTesterAsset();
    pAsset2.id = "test_spawn_buildings";
    this.add(pAsset2);
    for (int index = 0; index < 50; ++index)
    {
      this.t.addTask("spawn_random_building");
      this.t.addTask("wait_1");
    }
    this.t.addTask("end_test");
    JobTesterAsset pAsset3 = new JobTesterAsset();
    pAsset3.id = "destroy_sim_objects";
    this.add(pAsset3);
    this.t.addTask("destroy_sim_objects");
    this.t.addTask("end_test");
    JobTesterAsset pAsset4 = new JobTesterAsset();
    pAsset4.id = "test_random_power";
    this.add(pAsset4);
    for (int index = 0; index < 15; ++index)
    {
      this.t.addTask("spawn_random_power");
      this.t.addTask("wait_1");
    }
    this.t.addTask("end_test");
    JobTesterAsset pAsset5 = new JobTesterAsset();
    pAsset5.id = "test_world_change";
    this.add(pAsset5);
    this.t.addTask("fill_world_water");
    this.t.addTask("wait_1");
    this.t.addTask("fill_world_randomly");
    this.t.addTask("wait_1");
    this.t.addTask("fill_world_soil");
    this.t.addTask("wait_1");
    this.t.addTask("fill_world_randomly");
    this.t.addTask("wait_1");
    this.t.addTask("end_test");
    JobTesterAsset pAsset6 = new JobTesterAsset();
    pAsset6.id = "test_world_pit";
    this.add(pAsset6);
    this.t.addTask("fill_world_pit");
    this.t.addTask("wait_10");
    this.t.addTask("end_test");
    JobTesterAsset pAsset7 = new JobTesterAsset();
    pAsset7.id = "test_world_lava";
    this.add(pAsset7);
    for (int index = 0; index < 10; ++index)
      this.t.addTask("fill_world_lava");
    this.t.addTask("wait_5");
    this.t.addTask("end_test");
    JobTesterAsset pAsset8 = new JobTesterAsset();
    pAsset8.id = "test_windows";
    this.add(pAsset8);
    this.t.addTask("end_test");
    this.t.addTask("open_random_window");
    this.t.addTask("open_random_window");
    this.t.addTask("open_random_window");
    this.t.addTask("open_random_window");
    this.t.addTask("open_random_window");
    this.t.addTask("open_random_window");
    this.t.addTask("open_random_window");
    this.t.addTask("open_random_window");
    this.t.addTask("open_random_window");
    this.t.addTask("open_random_window");
    this.t.addTask("open_random_window");
    this.t.addTask("open_random_window");
    this.t.addTask("open_random_window");
    this.t.addTask("open_random_window");
    this.t.addTask("close_all_windows");
    this.t.addTask("end_test");
    JobTesterAsset pAsset9 = new JobTesterAsset();
    pAsset9.id = "test_gene_editor";
    pAsset9.manual_test = true;
    this.add(pAsset9);
    this.t.addTask("unpause");
    this.t.addTask("setup_laws");
    this.t.addTask("wait_1");
    this.t.addTask("pick_random_meta_objects");
    this.t.addTask("pause");
    this.t.addTask("open_gene_editor");
    this.t.addTask("end_test");
    JobTesterAsset pAsset10 = new JobTesterAsset();
    pAsset10.id = "test_chart_windows";
    pAsset10.manual_test = true;
    this.add(pAsset10);
    this.t.addTask("unpause");
    this.t.addTask("setup_laws");
    for (int index = 0; index < 6; ++index)
      this.t.addTask("try_new_city");
    this.t.addTask("wait_for_cities");
    this.t.addTask("wait_years_10");
    for (int index = 0; index < 3; ++index)
    {
      this.t.addTask("wait_years_5");
      this.t.addTask("pick_random_meta_objects");
      this.t.addTask("open_city_chart_window");
      this.t.addTask("pick_random_meta_objects");
      this.t.addTask("open_city_chart_window");
      this.t.addTask("wait_years_5");
      this.t.addTask("pick_random_meta_objects");
      this.t.addTask("open_kingdom_chart_window");
      this.t.addTask("pick_random_meta_objects");
      this.t.addTask("open_kingdom_chart_window");
    }
    this.t.addTask("pick_random_meta_objects_graph");
    this.t.addTask("show_compare_window");
    this.t.addTask("pause");
    JobTesterAsset pAsset11 = new JobTesterAsset();
    pAsset11.id = "test_meta_windows";
    pAsset11.manual_test = true;
    this.add(pAsset11);
    this.t.addTask("unpause");
    this.t.addTask("setup_laws");
    for (int index = 0; index < 6; ++index)
      this.t.addTask("try_new_city");
    this.t.addTask("wait_for_cities");
    this.t.addTask("wait_5");
    for (int index1 = 0; index1 < 50; ++index1)
    {
      for (int index2 = 0; index2 < 5; ++index2)
        this.t.addTask("pick_random_meta_objects");
      this.t.addTask("open_random_meta_window");
      for (int index3 = 0; index3 < 20; ++index3)
      {
        this.t.addTask("random_window_tab");
        this.t.addTask("random_meta_switch");
      }
      this.t.addTask("close_all_windows");
    }
    this.t.addTask("pick_random_meta_objects_graph");
    this.t.addTask("show_compare_window");
    this.t.addTask("pause");
    JobTesterAsset pAsset12 = new JobTesterAsset();
    pAsset12.id = "test_unit_selection";
    pAsset12.manual_test = true;
    this.add(pAsset12);
    this.t.addTask("setup_laws");
    for (int index4 = 0; index4 < 10; ++index4)
    {
      this.t.addTask("spawn_random_unit_5");
      for (int index5 = 0; index5 < 15; ++index5)
        this.t.addTask("pick_random_meta_objects");
      this.t.addTask("wait_1");
    }
    this.t.addTask("cull_mobs");
    this.t.addTask("cull_units");
    this.t.addTask("cull_godfinger");
    this.t.addTask("fix_water");
    this.t.addTask("close_all_windows");
    for (int index6 = 0; index6 < 10; ++index6)
    {
      this.t.addTask("spawn_random_unit_5");
      for (int index7 = 0; index7 < 15; ++index7)
        this.t.addTask("pick_random_meta_objects");
      this.t.addTask("wait_1");
    }
    this.t.addTask("randomize_subspecies_traits");
    this.t.addTask("randomize_subspecies_genes");
    this.t.addTask("randomize_actor_traits");
    this.t.addTask("randomize_actor_status");
    this.t.addTask("restart_test");
    JobTesterAsset pAsset13 = new JobTesterAsset();
    pAsset13.id = "test_multi_chart";
    pAsset13.manual_test = true;
    this.add(pAsset13);
    this.t.addTask("unpause");
    this.t.addTask("setup_laws");
    for (int index = 0; index < 6; ++index)
      this.t.addTask("try_new_city");
    this.t.addTask("wait_for_cities");
    for (int index = 0; index < 10; ++index)
    {
      this.t.addTask("pick_random_meta_objects_graph");
      this.t.addTask("show_compare_window");
      this.t.addTask("wait_years_5");
    }
    this.t.addTask("pause");
    JobTesterAsset pAsset14 = new JobTesterAsset();
    pAsset14.id = "auto_play";
    pAsset14.manual_test = true;
    this.add(pAsset14);
    this.t.addTask("unpause");
    this.t.addTask("setup_laws");
    this.t.addTask("fix_water");
    for (int index = 0; index < 6; ++index)
      this.t.addTask("try_new_city");
    this.t.addTask("wait_for_cities");
    this.t.addTask("random_bombs_and_kingdoms");
    this.t.addTask("cull_godfinger");
    this.t.addTask("restart_test");
    JobTesterAsset pAsset15 = new JobTesterAsset();
    pAsset15.id = "test_windows_tooltips";
    pAsset15.manual_test = true;
    this.add(pAsset15);
    this.t.addTask("unpause");
    this.t.addTask("setup_laws");
    this.t.addTask("fix_water");
    for (int index = 0; index < 6; ++index)
      this.t.addTask("try_new_city");
    this.t.addTask("wait_for_cities");
    for (int index = 0; index < 5; ++index)
      this.t.addTask("spawn_random_unit_5");
    this.t.addTask("cull_mobs");
    this.t.addTask("cull_units");
    this.t.addTask("cull_godfinger");
    this.t.addTask("fix_water");
    for (int index = 0; index < 5; ++index)
      this.t.addTask("spawn_random_unit_5");
    this.t.addTask("randomize_subspecies_traits");
    this.t.addTask("randomize_subspecies_genes");
    this.t.addTask("randomize_actor_traits");
    this.t.addTask("randomize_actor_status");
    this.t.addTask("reset_tweens");
    this.t.addTask("close_all_windows");
    this.t.addTask("prepare_window_testdata");
    this.t.addTask("open_next_window");
    this.t.addTask("test_window_and_tooltips");
    this.t.addTask("clear_window_testdata");
    this.t.addTask("close_all_windows");
    this.t.addTask("restart_test");
  }

  public override void linkAssets() => base.linkAssets();

  public string getNextJob()
  {
    if (this.list.Count == 0)
      return string.Empty;
    if (this._last_job > this.list.Count - 1)
    {
      this._last_job = 0;
      this.list.Shuffle<JobTesterAsset>();
    }
    JobTesterAsset jobTesterAsset = this.list[this._last_job++];
    return jobTesterAsset.manual_test ? this.getNextJob() : jobTesterAsset.id;
  }
}
