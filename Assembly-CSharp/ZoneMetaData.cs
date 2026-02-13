// Decompiled with JetBrains decompiler
// Type: ZoneMetaData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public struct ZoneMetaData
{
  public double timestamp;
  public double timestamp_new;
  public IMetaObject meta_object;
  public int previous_priority_amount;
  public TileZone zone;

  public float getDiffTime() => this.getDiffTime(World.world.getCurWorldTime());

  public float getDiffTime(double pWorldTime) => (float) (pWorldTime - this.timestamp);
}
