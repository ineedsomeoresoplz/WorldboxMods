// Decompiled with JetBrains decompiler
// Type: VersionTextUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class VersionTextUpdater : MonoBehaviour
{
  public bool addText = true;
  public Text text;
  private bool errored;
  private bool modded;

  private void Start()
  {
    if (this.addText)
    {
      this.text.text = $"version: {Application.version}-{Config.versionCodeText}";
      if (string.IsNullOrEmpty(Config.gitCodeText))
        return;
      Text text = this.text;
      text.text = $"{text.text}@{Config.gitCodeText}";
    }
    else
    {
      this.text.text = $"{Application.platform.ToString().ToLower().Replace("player", "")} {Application.version}-{Config.versionCodeText}";
      if (!string.IsNullOrEmpty(Config.gitCodeText))
      {
        Text text = this.text;
        text.text = $"{text.text}@{Config.gitCodeText}";
      }
      try
      {
        if (string.IsNullOrEmpty(RequestHelper.salt) || !(RequestHelper.salt != "err"))
          return;
        Text text = this.text;
        text.text = $"{text.text} ({RequestHelper.salt.Substring(0, 2)})";
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex.ToString());
      }
    }
  }

  private void Update()
  {
    if (this.errored)
      return;
    if (!this.modded && Config.MODDED)
    {
      ((Graphic) this.text).color = Color.yellow;
      this.modded = true;
    }
    if (LogHandler.errorNum <= 0 && !WorldBoxConsole.Console.hasErrors())
      return;
    if (this.modded)
      ((Graphic) this.text).color = Color.cyan;
    else
      ((Graphic) this.text).color = Color.red;
    this.errored = true;
  }
}
