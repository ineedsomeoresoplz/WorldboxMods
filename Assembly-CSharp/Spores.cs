// Decompiled with JetBrains decompiler
// Type: Spores
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Spores : BaseEffect
{
  private const float WALL_CHECKER_MOD_DISTANCE = 0.5f;
  private float _speed_x;
  private float _speed_y;
  private float _life_time;
  private long _actor_parent_id;

  public void setActorParent(Actor pActor)
  {
    this._actor_parent_id = pActor.getID();
    PhenotypeAsset randomPhenotypeAsset = pActor.subspecies.getRandomPhenotypeAsset();
    if (randomPhenotypeAsset != null)
      this.sprite_animation.phenotype = randomPhenotypeAsset;
    else
      this.sprite_animation.phenotype = PhenotypeLibrary.default_green;
    this.sprite_animation.forceUpdateFrame();
    this.current_position = Vector2.op_Implicit(pActor.current_tile.posV3);
    this.prepare(pActor.current_position, pActor.actor_scale);
    float pMaxExclusive1 = pActor.subspecies.base_stats["speed"] / 2f;
    float baseStat = pActor.subspecies.base_stats["lifespan"];
    float pMaxExclusive2 = Mathf.Clamp(Randy.randomFloat(0.0f, pMaxExclusive1), 0.0f, 10f);
    this._speed_x = Randy.randomFloat(-pMaxExclusive2, pMaxExclusive2);
    this._speed_y = Randy.randomFloat(-pMaxExclusive2, pMaxExclusive2);
    this._life_time = Mathf.Clamp(Randy.randomFloat(1f, baseStat), 1f, 120f);
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    if (World.world.isPaused())
      return;
    this.updateMovement(pElapsed);
    this.updatePosition();
    this.updateLifetime(pElapsed);
    if ((double) this._life_time > 0.0)
      return;
    this.kill();
  }

  private void updateLifetime(float pElapsed) => this._life_time -= pElapsed;

  public override void kill()
  {
    base.kill();
    Actor pActor = World.world.units.get(this._actor_parent_id);
    if (pActor == null)
      return;
    BabyMaker.spawnBabyFromSpore(pActor, Vector2.op_Implicit(this.current_position));
  }

  private void updatePosition()
  {
    ((Component) this).transform.localPosition = new Vector3(this.current_position.x, this.current_position.y, 0.0f);
  }

  private void updateMovement(float pElapsed)
  {
    float num1 = this._speed_x * pElapsed;
    float num2 = this._speed_y * pElapsed;
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(this.current_position.x + this._speed_x * 0.5f, this.current_position.y + this._speed_y * 0.5f, 0.0f);
    if (this.isBlockedByTile(Vector2.op_Implicit(vector3)))
    {
      this._life_time = 0.0f;
    }
    else
    {
      this.current_position.x += num1;
      this.current_position.y += num2;
      vector3.x = this.current_position.x;
      vector3.y = this.current_position.y;
      ((Component) this).transform.localPosition = vector3;
    }
  }

  private bool isBlockedByTile(Vector2 pPos)
  {
    WorldTile tile = World.world.GetTile((int) pPos.x, (int) pPos.y);
    return tile != null && tile.Type.block;
  }
}
