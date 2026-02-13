// Decompiled with JetBrains decompiler
// Type: RegionLink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public class RegionLink : IEquatable<RegionLink>
{
  public int id;
  public readonly HashSetMapRegion regions = new HashSetMapRegion();

  public void reset()
  {
    this.regions.Clear();
    this.id = -1;
  }

  public override int GetHashCode() => this.id;

  public override bool Equals(object obj) => this.Equals(obj as RegionLink);

  public bool Equals(RegionLink pObject) => this.id == pObject.id;
}
