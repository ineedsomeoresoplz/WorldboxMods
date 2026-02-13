// Decompiled with JetBrains decompiler
// Type: CityZoneHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class CityZoneHelper
{
  public CityZoneGrowth city_growth;
  public CityZoneAbandon city_abandon;
  public CityPlaceFinder city_place_finder;

  public CityZoneHelper()
  {
    this.city_growth = new CityZoneGrowth();
    this.city_abandon = new CityZoneAbandon();
    this.city_place_finder = new CityPlaceFinder();
  }

  public void update(float pElapsed) => this.city_abandon.checkCities();

  public void clear()
  {
    this.city_abandon.clearAll();
    this.city_growth.clearAll();
    this.city_place_finder.clearAll();
  }
}
