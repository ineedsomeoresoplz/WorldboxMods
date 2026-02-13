// Decompiled with JetBrains decompiler
// Type: BaseBuildingComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public class BaseBuildingComponent : IDisposable
{
  internal Building building;

  internal virtual void create(Building pBuilding) => this.building = pBuilding;

  public virtual void update(float pElapsed)
  {
  }

  public virtual void Dispose() => this.building = (Building) null;
}
