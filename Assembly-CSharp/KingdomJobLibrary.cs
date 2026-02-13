// Decompiled with JetBrains decompiler
// Type: KingdomJobLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class KingdomJobLibrary : AssetLibrary<KingdomJob>
{
  public override void init()
  {
    base.init();
    KingdomJob pAsset = new KingdomJob();
    pAsset.id = "kingdom";
    this.add(pAsset);
    this.t.addTask("do_checks");
    this.t.addTask("wait1");
    this.t.addTask("wait1");
  }
}
