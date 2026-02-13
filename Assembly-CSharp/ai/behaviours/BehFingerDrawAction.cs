// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFingerDrawAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehFingerDrawAction : BehFinger
{
  public bool check_has_target_tiles = true;
  public bool check_current_tile_in_target_tiles = true;
  public bool check_target_tile_in_target_tiles = true;

  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    if (!this.check_target_tile_in_target_tiles)
      return;
    this.null_check_tile_target = true;
  }

  public override bool errorsFound(Actor pActor)
  {
    if (base.errorsFound(pActor))
      return true;
    this.finger = pActor.children_special[0] as GodFinger;
    if (this.check_has_target_tiles && this.finger.target_tiles.Count == 0)
      return true;
    if (this.check_current_tile_in_target_tiles)
    {
      pActor.findCurrentTile(false);
      if (!this.finger.target_tiles.Contains(pActor.current_tile))
      {
        bool flag = false;
        if (pActor.beh_tile_target != null && (double) Toolbox.DistTile(pActor.current_tile, pActor.beh_tile_target) < 6.0)
        {
          flag = true;
        }
        else
        {
          foreach (WorldTile worldTile in pActor.current_tile.neighboursAll)
          {
            if (this.finger.target_tiles.Contains(worldTile))
            {
              flag = true;
              break;
            }
          }
        }
        if (!flag)
          return true;
      }
    }
    return this.check_target_tile_in_target_tiles && !this.finger.target_tiles.Contains(pActor.beh_tile_target);
  }
}
