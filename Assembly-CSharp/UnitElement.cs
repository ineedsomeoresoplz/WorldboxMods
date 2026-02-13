// Decompiled with JetBrains decompiler
// Type: UnitElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public abstract class UnitElement : WindowMetaElementBase, IRefreshElement
{
  protected Actor actor;
  protected UnitWindow unit_window;

  protected override void Awake()
  {
    this.checkSetWindow();
    base.Awake();
  }

  protected virtual void checkSetWindow()
  {
    this.unit_window = ((Component) this).GetComponentInParent<UnitWindow>();
  }

  protected override void OnEnable()
  {
    this.checkSetActor();
    base.OnEnable();
  }

  protected virtual void checkSetActor() => this.setActor(this.unit_window.actor);

  protected virtual void setActor(Actor pActor) => this.actor = pActor;

  public override bool checkRefreshWindow()
  {
    return this.actor.isRekt() || !this.actor.hasHealth() || base.checkRefreshWindow();
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    this.actor = (Actor) null;
  }
}
