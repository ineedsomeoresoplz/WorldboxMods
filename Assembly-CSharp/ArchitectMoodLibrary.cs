// Decompiled with JetBrains decompiler
// Type: ArchitectMoodLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ArchitectMoodLibrary : AssetLibrary<ArchitectMood>
{
  public const string DEFAULT_MOOD = "serene";

  public override void init()
  {
    base.init();
    ArchitectMood pAsset1 = new ArchitectMood();
    pAsset1.id = "trickster";
    pAsset1.color_main = "#1cf713";
    pAsset1.color_text = "#1cf713";
    this.add(pAsset1);
    ArchitectMood pAsset2 = new ArchitectMood();
    pAsset2.id = "benevolent";
    pAsset2.color_main = "#ffe90b";
    pAsset2.color_text = "#ffe90b";
    this.add(pAsset2);
    ArchitectMood pAsset3 = new ArchitectMood();
    pAsset3.id = "malevolent";
    pAsset3.color_main = "#a00cfc";
    pAsset3.color_text = "#a00cfc";
    this.add(pAsset3);
    ArchitectMood pAsset4 = new ArchitectMood();
    pAsset4.id = "serene";
    pAsset4.color_main = "#68FFFF";
    pAsset4.color_text = "#68FFFF";
    this.add(pAsset4);
    ArchitectMood pAsset5 = new ArchitectMood();
    pAsset5.id = "chaotic";
    pAsset5.color_main = "#ff0e0e";
    pAsset5.color_text = "#ff0e0e";
    this.add(pAsset5);
    ArchitectMood pAsset6 = new ArchitectMood();
    pAsset6.id = "orderly";
    pAsset6.color_main = "#ff870e";
    pAsset6.color_text = "#ff870e";
    this.add(pAsset6);
    ArchitectMood pAsset7 = new ArchitectMood();
    pAsset7.id = "mysterious";
    pAsset7.color_main = "#f01fb4";
    pAsset7.color_text = "#f01fb4";
    this.add(pAsset7);
    ArchitectMood pAsset8 = new ArchitectMood();
    pAsset8.id = "apathetic";
    pAsset8.color_main = "#000000";
    pAsset8.color_text = "#AAAAAA";
    this.add(pAsset8);
    ArchitectMood pAsset9 = new ArchitectMood();
    pAsset9.id = "ethereal";
    pAsset9.color_main = "#73a18e";
    pAsset9.color_text = "#73a18e";
    this.add(pAsset9);
  }

  public override void editorDiagnosticLocales()
  {
    foreach (ArchitectMood pAsset in this.list)
    {
      string localeId = pAsset.getLocaleID();
      this.checkLocale((Asset) pAsset, localeId);
    }
    base.editorDiagnosticLocales();
  }

  public override void post_init()
  {
    base.post_init();
    foreach (ArchitectMood architectMood in this.list)
      architectMood.path_icon = "ui/Icons/architect_moods/architect_mood_" + architectMood.id;
  }

  public override void editorDiagnostic()
  {
    foreach (ArchitectMood architectMood in this.list)
    {
      if (Object.op_Equality((Object) SpriteTextureLoader.getSprite(architectMood.path_icon), (Object) null))
        BaseAssetLibrary.logAssetError("Missing icon file", architectMood.path_icon);
    }
    base.editorDiagnostic();
  }
}
