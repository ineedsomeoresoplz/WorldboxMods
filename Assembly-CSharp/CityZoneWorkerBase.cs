// Decompiled with JetBrains decompiler
// Type: CityZoneWorkerBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public abstract class CityZoneWorkerBase
{
  protected bool debug;
  protected Queue<ZoneConnection> _wave = new Queue<ZoneConnection>();
  protected Queue<ZoneConnection> _next_wave = new Queue<ZoneConnection>();
  protected HashSet<ZoneConnection> _zones_checked = new HashSet<ZoneConnection>();

  protected void prepareWave()
  {
    this._wave.Clear();
    this._next_wave.Clear();
  }

  internal virtual void clearAll()
  {
    this._zones_checked.Clear();
    this._wave.Clear();
    this._next_wave.Clear();
    this.checkZoneDebug();
  }

  protected virtual void queueConnection(
    ZoneConnection pConnection,
    Queue<ZoneConnection> pWave,
    bool pSetChecked = false)
  {
    pWave.Enqueue(pConnection);
    if (!pSetChecked)
      return;
    this._zones_checked.Add(pConnection);
  }

  protected void checkZoneDebug()
  {
    if (!this.debug)
      return;
    World.world.zone_calculator.clearDebug();
  }
}
