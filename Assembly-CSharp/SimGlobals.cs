// Decompiled with JetBrains decompiler
// Type: SimGlobals
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class SimGlobals : AssetLibrary<SimGlobalAsset>
{
  public static SimGlobalAsset m;

  public override void init()
  {
    base.init();
    SimGlobalAsset pAsset = new SimGlobalAsset();
    pAsset.id = "main";
    SimGlobals.m = this.add(pAsset);
  }
}
