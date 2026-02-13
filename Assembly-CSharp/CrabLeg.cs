// Decompiled with JetBrains decompiler
// Type: CrabLeg
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CrabLeg : MonoBehaviour
{
  public CrabLegLimbPoint limbPoint;
  internal Crabzilla crabzilla;
  private Vector3 _current_position;
  private Vector3 _target_position;
  private Vector3 _random_pos = Vector3.zero;
  public CrabLegJoint legJoint;
  private Vector3 _target_pos;

  internal void create()
  {
    this._target_position = ((Component) this.limbPoint).transform.position;
    this._target_position.z = 0.0f;
    this._current_position = this._target_position;
    ((Component) this).transform.position = new Vector3(this._target_position.x, this._target_position.y, 0.0f);
    ((Renderer) ((Component) this).GetComponent<SpriteRenderer>()).enabled = false;
  }

  internal void update(float pElapsed)
  {
    this._current_position = Vector3.MoveTowards(this._current_position, this._target_position, (float) (1.5 + (double) Toolbox.DistVec3(this._current_position, this._target_position) / 5.0));
    ((Component) this).transform.position = new Vector3(this._current_position.x, this._current_position.y, 0.0f);
    this._target_pos = Vector3.op_Addition(((Component) this.limbPoint).transform.position, this._random_pos);
    if (this.legJoint.isAngleOk(-20f, 30f))
      return;
    this.moveLeg();
  }

  public void moveLeg()
  {
    this._target_pos = Vector3.op_Addition(((Component) this.limbPoint).transform.position, this._random_pos);
    this._target_pos.z = 0.0f;
    this._target_position = this._target_pos;
    this._random_pos.x = Randy.randomFloat(-1f, 1f);
    this._random_pos.y = Randy.randomFloat(-1f, 1f);
    Vector2 vector2 = ControllableUnit.getMovementVector();
    if (!ControllableUnit.isMovementActionActive())
      vector2 = Vector2.zero;
    if ((double) vector2.x != 0.0)
    {
      if ((double) vector2.x > 0.0)
        this._random_pos.x += 2f;
      else
        this._random_pos.x -= 2f;
    }
    if ((double) vector2.y != 0.0)
    {
      if ((double) vector2.y > 0.0)
        this._random_pos.y += 2f;
      else
        this._random_pos.y -= 2f;
    }
    this.crabzilla.legMoved();
    WorldTile tile = World.world.GetTile((int) this._target_pos.x, (int) this._target_pos.y);
    if (tile == null)
      return;
    MapAction.damageWorld(tile, 3, AssetManager.terraform.get("crab_step"));
    MusicBox.playSound("event:/SFX/UNIQUE/Crabzilla/CrabzillaFootsteps", tile);
  }
}
