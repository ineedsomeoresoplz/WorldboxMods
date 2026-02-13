// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehaviourActionActor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace ai.behaviours;

public class BehaviourActionActor : BehaviourActionBase<Actor>
{
  public bool null_check_city;
  public bool null_check_kingdom;
  public bool null_check_tile_target;
  public bool null_check_building_target;
  public bool null_check_actor_target;
  public bool check_building_target_non_usable;
  public bool land_if_hovering;
  internal bool special_prevent_can_be_attacked;
  internal string force_animation_id = string.Empty;
  internal bool force_animation;
  internal bool socialize;
  protected static List<Actor> temp_actors = new List<Actor>();
  protected static List<WorldTile> possible_moves = new List<WorldTile>();
  public bool calibrate_target_position;
  public float check_actor_target_position_distance;

  public BehResult forceTask(Actor pActor, string pTask, bool pClean = true, bool pForceAction = false)
  {
    pActor.setTask(pTask, pClean, pForceAction: pForceAction);
    return BehResult.Skip;
  }

  public BehResult forceTaskImmediate(Actor pActor, string pTask, bool pClean = true, bool pForceAction = false)
  {
    pActor.setTask(pTask, pClean, pForceAction: pForceAction);
    return BehResult.ImmediateRun;
  }

  public override bool errorsFound(Actor pObject)
  {
    if (pObject.current_tile.region == null || pObject.current_tile.region.island == null || this.null_check_city && (pObject.city == null || !pObject.city.isAlive()) || this.null_check_actor_target && (pObject.beh_actor_target == null || !pObject.beh_actor_target.isAlive()) || this.null_check_tile_target && pObject.beh_tile_target == null)
      return true;
    if (this.check_building_target_non_usable)
    {
      if (pObject.beh_building_target == null || !pObject.beh_building_target.isUsable())
        return true;
    }
    else if (this.null_check_building_target && (pObject.beh_building_target == null || !pObject.beh_building_target.isAlive()))
      return true;
    return base.errorsFound(pObject);
  }

  public static void clear()
  {
    BehaviourActionActor.temp_actors.Clear();
    BehaviourActionActor.possible_moves.Clear();
  }
}
