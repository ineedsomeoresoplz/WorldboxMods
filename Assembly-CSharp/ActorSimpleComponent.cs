// Decompiled with JetBrains decompiler
// Type: ActorSimpleComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public abstract class ActorSimpleComponent : IDisposable
{
  internal Actor actor;

  internal virtual void create(Actor pActor) => this.actor = pActor;

  public virtual void update(float pElapsed)
  {
  }

  public virtual void Dispose() => this.actor = (Actor) null;
}
