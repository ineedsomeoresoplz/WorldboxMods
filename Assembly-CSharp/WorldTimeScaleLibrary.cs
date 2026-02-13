// Decompiled with JetBrains decompiler
// Type: WorldTimeScaleLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class WorldTimeScaleLibrary : AssetLibrary<WorldTimeScaleAsset>
{
  public override void init()
  {
    WorldTimeScaleAsset pAsset1 = new WorldTimeScaleAsset();
    pAsset1.id = "slow_mo";
    pAsset1.locale_key = "speed_slow_mo";
    pAsset1.multiplier = 0.5f;
    pAsset1.ticks = 1;
    pAsset1.conway_ticks = 1;
    pAsset1.path_icon = "ui/Icons/iconClockX0.5";
    this.add(pAsset1);
    WorldTimeScaleAsset pAsset2 = new WorldTimeScaleAsset();
    pAsset2.id = "x1";
    pAsset2.locale_key = "speed_x1";
    pAsset2.multiplier = 1f;
    pAsset2.ticks = 1;
    pAsset2.conway_ticks = 1;
    pAsset2.path_icon = "ui/Icons/iconClockX1";
    this.add(pAsset2);
    WorldTimeScaleAsset pAsset3 = new WorldTimeScaleAsset();
    pAsset3.id = "x2";
    pAsset3.locale_key = "speed_x2";
    pAsset3.multiplier = 2f;
    pAsset3.ticks = 1;
    pAsset3.conway_ticks = 2;
    pAsset3.path_icon = "ui/Icons/iconClockX2";
    this.add(pAsset3);
    WorldTimeScaleAsset pAsset4 = new WorldTimeScaleAsset();
    pAsset4.id = "x3";
    pAsset4.locale_key = "speed_x3";
    pAsset4.multiplier = 3f;
    pAsset4.ticks = 1;
    pAsset4.conway_ticks = 3;
    pAsset4.path_icon = "ui/Icons/iconClockX3";
    this.add(pAsset4);
    WorldTimeScaleAsset pAsset5 = new WorldTimeScaleAsset();
    pAsset5.id = "x4";
    pAsset5.locale_key = "speed_x4";
    pAsset5.multiplier = 4f;
    pAsset5.ticks = 1;
    pAsset5.conway_ticks = 4;
    pAsset5.path_icon = "ui/Icons/iconClockX4";
    this.add(pAsset5);
    WorldTimeScaleAsset pAsset6 = new WorldTimeScaleAsset();
    pAsset6.id = "x5";
    pAsset6.locale_key = "speed_x5";
    pAsset6.multiplier = 5f;
    pAsset6.ticks = 1;
    pAsset6.conway_ticks = 5;
    pAsset6.path_icon = "ui/Icons/iconClockX5";
    this.add(pAsset6);
    WorldTimeScaleAsset pAsset7 = new WorldTimeScaleAsset();
    pAsset7.id = "x10";
    pAsset7.locale_key = "speed_x10";
    pAsset7.multiplier = 10f;
    pAsset7.ticks = 1;
    pAsset7.conway_ticks = 10;
    pAsset7.path_icon = "ui/Icons/iconClockX5";
    this.add(pAsset7);
    WorldTimeScaleAsset pAsset8 = new WorldTimeScaleAsset();
    pAsset8.id = "x15";
    pAsset8.locale_key = "speed_x15";
    pAsset8.multiplier = 15f;
    pAsset8.ticks = 1;
    pAsset8.conway_ticks = 15;
    pAsset8.path_icon = "ui/Icons/iconClockX5";
    this.add(pAsset8);
    WorldTimeScaleAsset pAsset9 = new WorldTimeScaleAsset();
    pAsset9.id = "x20";
    pAsset9.locale_key = "speed_x20";
    pAsset9.multiplier = 20f;
    pAsset9.ticks = 1;
    pAsset9.conway_ticks = 20;
    pAsset9.path_icon = "ui/Icons/iconClockX5";
    this.add(pAsset9);
    WorldTimeScaleAsset pAsset10 = new WorldTimeScaleAsset();
    pAsset10.id = "x40";
    pAsset10.locale_key = "speed_x40";
    pAsset10.multiplier = 20f;
    pAsset10.sonic = true;
    pAsset10.ticks = 2;
    pAsset10.conway_ticks = 40;
    pAsset10.path_icon = "ui/Icons/iconClockXSonic";
    this.add(pAsset10);
  }

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (WorldTimeScaleAsset pAsset in this.list)
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
  }
}
