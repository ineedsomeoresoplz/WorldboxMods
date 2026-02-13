// Decompiled with JetBrains decompiler
// Type: CommunicationLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class CommunicationLibrary : AssetLibrary<CommunicationAsset>
{
  public static CommunicationAsset normal;

  public override void init()
  {
    base.init();
    CommunicationAsset pAsset1 = new CommunicationAsset();
    pAsset1.id = "normal";
    pAsset1.icon_path = "speech/speech_bubble";
    pAsset1.show_topic = true;
    CommunicationLibrary.normal = this.add(pAsset1);
    CommunicationAsset pAsset2 = new CommunicationAsset();
    pAsset2.id = "singing";
    pAsset2.icon_path = "speech/speech_02";
    this.add(pAsset2);
    CommunicationAsset pAsset3 = new CommunicationAsset();
    pAsset3.id = "exclamation";
    pAsset3.icon_path = "speech/speech_03";
    this.add(pAsset3);
    CommunicationAsset pAsset4 = new CommunicationAsset();
    pAsset4.id = "questioning";
    pAsset4.icon_path = "speech/speech_04";
    this.add(pAsset4);
    CommunicationAsset pAsset5 = new CommunicationAsset();
    pAsset5.id = "offensive";
    pAsset5.icon_path = "speech/speech_05";
    this.add(pAsset5);
    CommunicationAsset pAsset6 = new CommunicationAsset();
    pAsset6.id = "defensive";
    pAsset6.icon_path = "speech/speech_06";
    this.add(pAsset6);
    CommunicationAsset pAsset7 = new CommunicationAsset();
    pAsset7.id = "mortality";
    pAsset7.icon_path = "speech/speech_07";
    this.add(pAsset7);
    CommunicationAsset pAsset8 = new CommunicationAsset();
    pAsset8.id = "romantic";
    pAsset8.icon_path = "speech/speech_08";
    this.add(pAsset8);
    CommunicationAsset pAsset9 = new CommunicationAsset();
    pAsset9.id = "pleasant";
    pAsset9.icon_path = "speech/speech_09";
    this.add(pAsset9);
    CommunicationAsset pAsset10 = new CommunicationAsset();
    pAsset10.id = "unpleasant";
    pAsset10.icon_path = "speech/speech_10";
    this.add(pAsset10);
  }
}
