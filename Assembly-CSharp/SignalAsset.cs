// Decompiled with JetBrains decompiler
// Type: SignalAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class SignalAsset : Asset
{
  public SignalAction action;
  public bool has_action;
  public AchievementCheck action_achievement;
  public bool has_action_achievement;
  public SignalBanCheckAction ban_check_action;
  public bool has_ban_check_action;
  private bool _banned;

  public bool isBanned() => this._banned;

  public bool ban() => this._banned = true;

  public bool unban() => this._banned = false;
}
