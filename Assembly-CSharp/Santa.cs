// Decompiled with JetBrains decompiler
// Type: Santa
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Santa : BaseEffect
{
  private float _timer_bomb = 1f;
  private float _timer_smoke;
  internal bool alive = true;
  internal Material current_material;
  private float current_height;

  public void spawnOn(WorldTile pTile)
  {
    this.alive = true;
    this.current_height = Randy.randomFloat(30f, 50f);
    ((Vector2) ref this.current_position).Set((float) pTile.x, (float) pTile.y - this.current_height);
    this.current_material = LibraryMaterials.instance.mat_world_object;
    this._timer_bomb = 2f + Randy.randomFloat(0.0f, 2f);
  }

  private void updateSanta(float pElapsed)
  {
    if ((double) this.current_position.x > (double) (MapBox.width * 2))
      this.kill();
    else if (this.alive)
    {
      if (World.world.isPaused())
        return;
      this.updateSantaMovement();
      this.updateBombDropTimer(pElapsed);
    }
    else
    {
      this.updateSantaDeadFall();
      if ((double) this.current_height != 0.0)
        return;
      this.fallDeathEvent();
    }
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    this.updateSanta(pElapsed);
    this.updatePosition();
  }

  public void updatePosition()
  {
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(this.current_position.x, this.current_position.y + this.current_height, this.current_height);
    ((Component) this).transform.localPosition = vector3;
  }

  private void updateBombDropTimer(float pElapsed)
  {
    if ((double) this._timer_bomb > 0.0)
    {
      this._timer_bomb -= pElapsed;
    }
    else
    {
      this._timer_bomb = 2f + Randy.randomFloat(0.0f, 2f);
      this.dropSantaBomb();
    }
  }

  private void fallDeathEvent()
  {
    this.kill();
    EffectsLibrary.spawnAt("fx_land_explosion_old", ((Component) this).transform.localPosition, 0.6f);
    WorldTile tile = World.world.GetTile((int) this.current_position.x, (int) this.current_position.y);
    if (tile == null)
      return;
    MapAction.damageWorld(tile, 5, AssetManager.terraform.get("grenade"));
  }

  private void updateSantaDeadFall()
  {
    if ((double) this._timer_smoke > 0.0)
    {
      this._timer_smoke -= World.world.elapsed;
    }
    else
    {
      this._timer_smoke = 0.1f;
      EffectsLibrary.spawnAt("fx_fire_smoke", ((Component) this).transform.position, 0.6f);
    }
    this.current_position = Vector2.op_Addition(this.current_position, Vector2.op_Multiply(new Vector2(4f, Randy.randomFloat(-1f, 1f)), World.world.elapsed));
    this.current_height -= 20f * World.world.elapsed;
    if ((double) this.current_height >= 0.0)
      return;
    this.current_height = 0.0f;
  }

  private void updateSantaMovement()
  {
    this.current_position = Vector2.op_Addition(this.current_position, Vector2.op_Multiply(new Vector2(5f, Randy.randomFloat(-1f, 1f)), World.world.elapsed));
  }

  private void dropSantaBomb()
  {
    WorldTile tile = World.world.GetTile((int) this.current_position.x, (int) this.current_position.y);
    if (tile == null)
      return;
    World.world.drop_manager.spawn(tile, "santa_bomb", this.current_height).soundOn = true;
    if (!Randy.randomBool())
      return;
    MusicBox.playSound("event:/SFX/OTHER/RoboSanta/RoboSantaVoice", this.current_position.x, this.current_position.y - this.current_height);
  }
}
