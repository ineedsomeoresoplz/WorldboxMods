// Decompiled with JetBrains decompiler
// Type: CityTasksData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class CityTasksData
{
  public int trees;
  public int minerals;
  public int bushes;
  public int plants;
  public int hives;
  public int farm_fields;
  public int farms_total;
  public int wheats;
  public int ruins;
  public int poops;
  public int roads;
  public int fire;

  public void clear()
  {
    this.trees = 0;
    this.minerals = 0;
    this.bushes = 0;
    this.plants = 0;
    this.hives = 0;
    this.ruins = 0;
    this.poops = 0;
    this.farm_fields = 0;
    this.roads = 0;
    this.wheats = 0;
    this.fire = 0;
    this.farms_total = 0;
  }
}
