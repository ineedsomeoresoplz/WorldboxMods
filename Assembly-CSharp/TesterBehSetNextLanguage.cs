// Decompiled with JetBrains decompiler
// Type: TesterBehSetNextLanguage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System;
using System.Collections.Generic;

#nullable disable
public class TesterBehSetNextLanguage : BehaviourActionTester
{
  private List<string> languages = new List<string>();
  private int currentLanguage;

  public TesterBehSetNextLanguage()
  {
    this.languages = LocalizedTextManager.getAllLanguagesWithChanges();
  }

  public override BehResult execute(AutoTesterBot pObject)
  {
    if (this.currentLanguage >= this.languages.Count)
      this.currentLanguage = 0;
    string language = this.languages[this.currentLanguage++];
    Console.WriteLine($"[{Date.TimeNow()}] Changed language to : {language} {this.currentLanguage.ToString()}/{this.languages.Count.ToString()}");
    LocalizedTextManager.instance.setLanguage(language);
    return BehResult.Continue;
  }
}
