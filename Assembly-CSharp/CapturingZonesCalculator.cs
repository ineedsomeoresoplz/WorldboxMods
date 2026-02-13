// Decompiled with JetBrains decompiler
// Type: CapturingZonesCalculator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class CapturingZonesCalculator
{
  private static int _zoneTicks = 0;
  private static Queue<TileZone> _currentWave = new Queue<TileZone>();
  private static Queue<TileZone> _nextWave = new Queue<TileZone>();
  private static HashSet<TileZone> _waveChecked = new HashSet<TileZone>();

  public static void getListToDraw(City pCity, int pTicks, ListPool<TileZone> pResults)
  {
    pResults.Clear();
    TileZone tileZone = pCity.getTile()?.zone ?? pCity.zones[0];
    Queue<TileZone> tileZoneQueue1 = CapturingZonesCalculator._currentWave;
    tileZoneQueue1.Enqueue(tileZone);
    CapturingZonesCalculator._zoneTicks = pTicks;
    while (tileZoneQueue1.Count > 0 && CapturingZonesCalculator._zoneTicks != 0)
    {
      TileZone pTargetZone = tileZoneQueue1.Dequeue();
      CapturingZonesCalculator.check(pTargetZone, pCity);
      pResults.Add(pTargetZone);
      if (tileZoneQueue1.Count == 0)
      {
        Queue<TileZone> tileZoneQueue2 = tileZoneQueue1;
        tileZoneQueue1 = CapturingZonesCalculator._nextWave;
        CapturingZonesCalculator._nextWave = tileZoneQueue2;
      }
    }
    CapturingZonesCalculator._nextWave.Clear();
    CapturingZonesCalculator._waveChecked.Clear();
    tileZoneQueue1.Clear();
  }

  private static void check(TileZone pTargetZone, City pCity)
  {
    --CapturingZonesCalculator._zoneTicks;
    CapturingZonesCalculator._waveChecked.Add(pTargetZone);
    foreach (TileZone neighbour in pTargetZone.neighbours)
    {
      if (neighbour.city == pCity && !CapturingZonesCalculator._waveChecked.Contains(neighbour))
      {
        CapturingZonesCalculator._waveChecked.Add(neighbour);
        CapturingZonesCalculator._nextWave.Enqueue(neighbour);
      }
    }
  }
}
