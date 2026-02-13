// Decompiled with JetBrains decompiler
// Type: Meteorite
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Meteorite : BaseEffect
{
  private SpriteRenderer _shadow_renderer;
  public Vector3 rotationSpeed = new Vector3(0.0f, 0.0f, 50f);
  private float _falling_speed = 200f;
  public GameObject mainSprite;
  public GameObject shadowSprite;
  private int _radius;
  private float _shadow_alpha;
  private float _timer_smoke = 0.01f;
  public string terraform_asset = "meteorite";
  private Actor _owner;

  public override void Awake()
  {
    base.Awake();
    this._shadow_renderer = this.shadowSprite.GetComponent<SpriteRenderer>();
  }

  internal override void create() => base.create();

  public void spawnOn(WorldTile pTile, string pTerraformId, Actor pActor)
  {
    this.terraform_asset = pTerraformId;
    this.tile = pTile;
    this._radius = 20;
    ((Component) this).transform.position = new Vector3(pTile.posV3.x, pTile.posV3.y);
    this.current_position.x = Randy.randomFloat(-200f, 200f);
    this.current_position.y = Randy.randomFloat(200f, 250f);
    this.updateMainSpritePos();
    this.setShadowAlpha(0.0f);
  }

  private void updateMainSpritePos()
  {
    Vector3 vector3 = new Vector3();
    vector3.x = this.current_position.x;
    vector3.y = this.current_position.y;
    float y = this.current_position.y;
    vector3.z = y;
    this.mainSprite.transform.localPosition = vector3;
  }

  protected void smoothMovement(Vector2 end, float pElapsed)
  {
    Vector2 vector2 = Vector2.op_Subtraction(this.current_position, end);
    if ((double) ((Vector2) ref vector2).sqrMagnitude > 1.4012984643248171E-45)
    {
      this.current_position = Vector2.MoveTowards(this.current_position, end, this._falling_speed * pElapsed);
      this.updateMainSpritePos();
      this.shadowSprite.transform.localPosition = Vector2.op_Implicit(new Vector2(this.current_position.x, this.shadowSprite.transform.localPosition.y));
    }
    else
      this.explode();
  }

  public override void update(float pElapsed)
  {
    this.smoothMovement(Vector2.zero, pElapsed);
    this._shadow_alpha += World.world.elapsed * 0.2f;
    this.setShadowAlpha(this._shadow_alpha);
    this.mainSprite.transform.Rotate(Vector3.op_Multiply(this.rotationSpeed, World.world.elapsed));
    this.shadowSprite.transform.Rotate(Vector3.op_Multiply(this.rotationSpeed, World.world.elapsed));
    if ((double) this._timer_smoke > 0.0)
    {
      this._timer_smoke -= World.world.elapsed;
    }
    else
    {
      EffectsLibrary.spawnAt("fx_fire_smoke", this.mainSprite.transform.position, 1f);
      this._timer_smoke = 0.05f;
    }
  }

  protected void setShadowAlpha(float pVal)
  {
    this._shadow_alpha = pVal;
    if ((double) this._shadow_alpha < 0.0)
      this._shadow_alpha = 0.0f;
    Color color = this._shadow_renderer.color;
    color.a = this._shadow_alpha;
    this._shadow_renderer.color = color;
  }

  private void explode()
  {
    ++World.world.game_stats.data.meteoritesLaunched;
    MapAction.damageWorld(this.tile, this._radius, AssetManager.terraform.get(this.terraform_asset), (BaseSimObject) this._owner);
    EffectsLibrary.spawnExplosionWave(this.tile.posV3, (float) this._radius);
    Vector3 pPos;
    ref Vector3 local = ref pPos;
    Vector2Int pos = this.tile.pos;
    double x = (double) ((Vector2Int) ref pos).x;
    pos = this.tile.pos;
    double num = (double) (((Vector2Int) ref pos).y - 2);
    // ISSUE: explicit constructor call
    ((Vector3) ref local).\u002Ector((float) x, (float) num);
    float pScale = Randy.randomFloat(0.8f, 0.9f);
    EffectsLibrary.spawnAt("fx_explosion_meteorite", pPos, pScale);
    this.addRandomMineral(this.tile);
    this.addRandomMineral(this.tile.zone.getRandomTile());
    this.addRandomMineral(this.tile.zone.getRandomTile());
    this.addRandomMineral(this.tile.zone.getRandomTile());
    this.addRandomMineral(this.tile.zone.getRandomTile());
    this.controller.killObject((BaseEffect) this);
  }

  private void addRandomMineral(WorldTile pTile)
  {
    if (pTile == null)
      return;
    World.world.buildings.addBuilding("mineral_adamantine", pTile, true);
  }

  public static void spawnMeteoriteDisaster(WorldTile pTile, Actor pActor = null)
  {
    EffectsLibrary.spawn("fx_meteorite", pTile, "meteorite_disaster", pActor: pActor);
  }

  public static void spawnMeteorite(WorldTile pTile, Actor pActor = null)
  {
    EffectsLibrary.spawn("fx_meteorite", pTile, "meteorite", pActor: pActor);
  }
}
