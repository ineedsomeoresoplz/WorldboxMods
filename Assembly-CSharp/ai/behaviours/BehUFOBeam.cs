// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehUFOBeam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ai.behaviours;

public class BehUFOBeam : BehaviourActionActor
{
  private bool enabled;

  public BehUFOBeam(bool pEnabled = false) => this.enabled = pEnabled;

  public override BehResult execute(Actor pActor)
  {
    UFO actorComponent = pActor.getActorComponent<UFO>();
    if (!this.enabled)
    {
      actorComponent.hideBeam();
      return BehResult.Continue;
    }
    if (actorComponent.beamAnim.isOn)
    {
      if (actorComponent.beamAnim.currentFrameIndex == 4)
      {
        for (int index1 = 0; index1 < 8; ++index1)
        {
          for (int index2 = 0; index2 < 8; ++index2)
          {
            MapBox world = BehaviourActionBase<Actor>.world;
            Vector2Int pos = pActor.current_tile.pos;
            int pX = ((Vector2Int) ref pos).x + index2 - 4;
            pos = pActor.current_tile.pos;
            int pY = ((Vector2Int) ref pos).y + index1 - 4;
            WorldTile tile = world.GetTile(pX, pY);
            if (tile != null)
            {
              pos = pActor.current_tile.pos;
              int x1 = ((Vector2Int) ref pos).x;
              pos = pActor.current_tile.pos;
              int y1 = ((Vector2Int) ref pos).y;
              pos = tile.pos;
              int x2 = ((Vector2Int) ref pos).x;
              pos = tile.pos;
              int y2 = ((Vector2Int) ref pos).y;
              if ((double) Toolbox.Dist(x1, y1, x2, y2) <= 4.0)
                MapAction.damageWorld(tile, 0, AssetManager.terraform.get("ufo_attack"), (BaseSimObject) pActor);
            }
          }
        }
      }
      if (actorComponent.beamAnim.currentFrameIndex != actorComponent.beamAnim.frames.Length - 1)
        return BehResult.RepeatStep;
      actorComponent.hideBeam();
      return BehResult.Continue;
    }
    actorComponent.startBeam();
    return BehResult.RepeatStep;
  }
}
