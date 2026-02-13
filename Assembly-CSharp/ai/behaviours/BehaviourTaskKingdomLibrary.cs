// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehaviourTaskKingdomLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehaviourTaskKingdomLibrary : AssetLibrary<BehaviourTaskKingdom>
{
  public override void init()
  {
    base.init();
    BehaviourTaskKingdom behaviourTaskKingdom1 = new BehaviourTaskKingdom();
    behaviourTaskKingdom1.id = "nothing";
    BehaviourTaskKingdom pAsset1 = behaviourTaskKingdom1;
    this.t = behaviourTaskKingdom1;
    this.add(pAsset1);
    BehaviourTaskKingdom behaviourTaskKingdom2 = new BehaviourTaskKingdom();
    behaviourTaskKingdom2.id = "wait1";
    BehaviourTaskKingdom pAsset2 = behaviourTaskKingdom2;
    this.t = behaviourTaskKingdom2;
    this.add(pAsset2);
    this.t.addBeh((BehaviourActionKingdom) new KingdomBehRandomWait());
    BehaviourTaskKingdom behaviourTaskKingdom3 = new BehaviourTaskKingdom();
    behaviourTaskKingdom3.id = "wait_random";
    BehaviourTaskKingdom pAsset3 = behaviourTaskKingdom3;
    this.t = behaviourTaskKingdom3;
    this.add(pAsset3);
    this.t.addBeh((BehaviourActionKingdom) new KingdomBehRandomWait(pMax: 5f));
    BehaviourTaskKingdom behaviourTaskKingdom4 = new BehaviourTaskKingdom();
    behaviourTaskKingdom4.id = "do_checks";
    BehaviourTaskKingdom pAsset4 = behaviourTaskKingdom4;
    this.t = behaviourTaskKingdom4;
    this.add(pAsset4);
    this.t.addBeh((BehaviourActionKingdom) new KingdomBehCheckCapital());
    this.t.addBeh((BehaviourActionKingdom) new KingdomBehCheckKing());
    this.t.addBeh((BehaviourActionKingdom) new KingdomBehRandomWait());
  }

  public override void editorDiagnosticLocales()
  {
  }
}
