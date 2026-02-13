// Decompiled with JetBrains decompiler
// Type: SentenceAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class SentenceAsset : Asset
{
  private List<string[]> _templates = new List<string[]>();

  public void addTemplate(params string[] pTemplates) => this._templates.Add(pTemplates);

  public string[] getRandomTemplate()
  {
    return this._templates.Count == 0 ? (string[]) null : this._templates.GetRandom<string[]>();
  }
}
