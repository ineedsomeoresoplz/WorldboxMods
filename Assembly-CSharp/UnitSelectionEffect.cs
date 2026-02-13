// Decompiled with JetBrains decompiler
// Type: UnitSelectionEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class UnitSelectionEffect : BaseAnimatedObject
{
  public static Actor last_actor;
  private bool _is_visible = true;

  internal override void create()
  {
    base.create();
    ((Component) this).transform.parent = ((Component) World.world).transform;
    ((Object) ((Component) this).transform).name = "unit_selector_effect";
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    this.setVisible(this.visibleCheck());
  }

  public bool visibleCheck()
  {
    if (!MapBox.isRenderGameplay())
      return false;
    if (World.world.isAnyPowerSelected())
    {
      GodPower godPower = World.world.selected_buttons.selectedButton.godPower;
      if (!godPower.allow_unit_selection && !godPower.show_close_actor)
        return false;
    }
    if (World.world.isBusyWithUI())
      return false;
    Actor pActor = World.world.getActorNearCursor();
    if (ControllableUnit.isControllingUnit())
      pActor = (Actor) null;
    UnitSelectionEffect.setLastActor(pActor);
    return pActor != null;
  }

  public void setVisible(bool pVisible)
  {
    if (pVisible != this._is_visible)
    {
      ((Component) this).gameObject.SetActive(pVisible);
      if (!pVisible)
      {
        ((Component) this).transform.position = Globals.POINT_IN_VOID;
        ((Component) this).transform.localScale = Vector3.one;
        UnitSelectionEffect.setLastActor((Actor) null);
      }
    }
    if (pVisible)
    {
      ((Component) this).transform.position = Vector2.op_Implicit(UnitSelectionEffect.last_actor.current_position);
      ((Component) this).transform.localScale = UnitSelectionEffect.last_actor.current_scale;
    }
    this._is_visible = pVisible;
  }

  public static void setLastActor(Actor pActor) => UnitSelectionEffect.last_actor = pActor;
}
