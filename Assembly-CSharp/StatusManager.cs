// Decompiled with JetBrains decompiler
// Type: StatusManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class StatusManager : CoreSystemManager<Status, StatusData>
{
  public StatusManager() => this.type_id = "statuses";

  public Status newStatus(BaseSimObject pSimObject, StatusAsset pAsset, float pOverrideTimer)
  {
    Status status = this.newObject();
    status.start(pSimObject, pAsset);
    if ((double) pOverrideTimer > 0.0)
      status.setDuration(pOverrideTimer);
    return status;
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    this.updateStatuses(pElapsed);
    this.checkDead();
  }

  public override void removeObject(Status pObject)
  {
    base.removeObject(pObject);
    pObject.sim_object.removeFinishedStatusEffect(pObject);
  }

  private void updateStatuses(float pElapsed)
  {
    float curWorldTime = (float) World.world.getCurWorldTime();
    bool flag = World.world.isPaused();
    List<Status> list = this.list;
    for (int index = 0; index < list.Count; ++index)
    {
      Status status = list[index];
      if (!status.is_finished)
      {
        if (!flag)
          status.update(pElapsed, curWorldTime);
        if (!flag || status.asset.is_animated_in_pause)
          status.updateAnimationFrame(pElapsed);
      }
    }
  }

  private void checkDead()
  {
    for (int index = this.list.Count - 1; index >= 0; --index)
    {
      Status pObject = this.list[index];
      if (pObject.is_finished)
        this.removeObject(pObject);
    }
  }
}
