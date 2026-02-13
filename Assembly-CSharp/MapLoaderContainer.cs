// Decompiled with JetBrains decompiler
// Type: MapLoaderContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
internal class MapLoaderContainer
{
  public MapLoaderAction action;
  public string id;
  public bool debug_log = true;
  public float new_timer_value = 1f / 1000f;

  public MapLoaderContainer(
    MapLoaderAction pAction,
    string pID,
    bool pDebugLog = true,
    float pNewWaitTimerValue = 0.001f)
  {
    this.action = pAction;
    this.id = pID;
    this.debug_log = pDebugLog;
    this.new_timer_value = pNewWaitTimerValue;
  }
}
