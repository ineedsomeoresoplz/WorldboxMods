// Decompiled with JetBrains decompiler
// Type: MapGenSettingsAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class MapGenSettingsAsset : Asset, ILocalizedAsset
{
  public bool is_switch;
  public int min_value;
  public int max_value;
  public MapGenSettingsDelegateBool allowed_check;
  public MapGenSettingsDelegate increase;
  public MapGenSettingsDelegate decrease;
  public MapGenSettingsDelegateSwitch action_switch;
  public MapGenSettingsDelegateGet action_get;
  public MapGenSettingsDelegateSet action_set;

  public string getLocaleID() => this.id;
}
