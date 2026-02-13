// Decompiled with JetBrains decompiler
// Type: MusicBoxIdle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using FMOD.Studio;
using System.Collections.Generic;

#nullable disable
public class MusicBoxIdle
{
  private List<BaseSimObject> _toRemove = new List<BaseSimObject>();
  public Dictionary<BaseSimObject, EventInstance> currentAttachedSounds = new Dictionary<BaseSimObject, EventInstance>();
  private float _timer;

  public void update(float pElapsed)
  {
    if ((double) this._timer > 2.0)
    {
      this._timer -= pElapsed;
    }
    else
    {
      this._timer = 2f;
      this._toRemove.Clear();
      if (World.world.quality_changer.isLowRes())
        this.clearAllSounds();
      this.checkDeadSounds();
      if (World.world.quality_changer.isLowRes())
        return;
      this.updateBuildings();
    }
  }

  public virtual void checkDeadSounds()
  {
    foreach (BaseSimObject key in this.currentAttachedSounds.Keys)
    {
      bool flag = false;
      if (!key.isAlive())
        flag = true;
      if (flag)
        this._toRemove.Add(key);
    }
    foreach (BaseSimObject pObj in this._toRemove)
      this.removeSound(pObj);
  }

  private void updateBuildings()
  {
  }

  private void removeSound(BaseSimObject pObj)
  {
    EventInstance eventInstance;
    this.currentAttachedSounds.TryGetValue(pObj, out eventInstance);
    if (!((EventInstance) ref eventInstance).isValid())
      return;
    ((EventInstance) ref eventInstance).stop((STOP_MODE) 0);
    ((EventInstance) ref eventInstance).release();
    this.currentAttachedSounds.Remove(pObj);
  }

  private void playAttachedSound(BaseSimObject pObject, string pSound)
  {
    if (!MusicBox.sounds_on)
      return;
    EventInstance eventInstance;
    this.currentAttachedSounds.TryGetValue(pObject, out eventInstance);
    if (((EventInstance) ref eventInstance).isValid())
      return;
    this.currentAttachedSounds.Add(pObject, eventInstance);
  }

  private bool isPlaying(BaseSimObject pObject)
  {
    EventInstance eventInstance;
    this.currentAttachedSounds.TryGetValue(pObject, out eventInstance);
    return ((EventInstance) ref eventInstance).isValid();
  }

  public void clearAllSounds()
  {
    foreach (EventInstance eventInstance in this.currentAttachedSounds.Values)
    {
      ((EventInstance) ref eventInstance).stop((STOP_MODE) 0);
      ((EventInstance) ref eventInstance).release();
    }
    this.currentAttachedSounds.Clear();
  }

  public int CountCurrentSounds() => this.currentAttachedSounds.Count;
}
