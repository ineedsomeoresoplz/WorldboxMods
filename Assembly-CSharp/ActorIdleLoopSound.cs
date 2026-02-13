// Decompiled with JetBrains decompiler
// Type: ActorIdleLoopSound
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using FMOD.Studio;

#nullable disable
public class ActorIdleLoopSound
{
  internal EventInstance fmod_instance;
  private Actor _actor;

  public ActorIdleLoopSound(ActorAsset pAsset, Actor pActor)
  {
  }

  public void stop() => this.stopLoopCallback(this._actor);

  internal void stopLoopCallback(Actor pActor)
  {
    if (!((EventInstance) ref this.fmod_instance).isValid())
      return;
    ((EventInstance) ref this.fmod_instance).stop((STOP_MODE) 0);
    ((EventInstance) ref this.fmod_instance).release();
  }
}
