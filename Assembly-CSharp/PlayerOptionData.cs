// Decompiled with JetBrains decompiler
// Type: PlayerOptionData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.ComponentModel;

#nullable disable
[Serializable]
public class PlayerOptionData
{
  public string name = "OPTION";
  [DefaultValue(true)]
  public bool boolVal = true;
  [DefaultValue("")]
  public string stringVal = string.Empty;
  [DefaultValue(0)]
  public int intVal;
  [NonSerialized]
  public PlayerOptionAction on_switch;

  public PlayerOptionData(string pName) => this.name = pName;
}
