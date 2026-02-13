// Decompiled with JetBrains decompiler
// Type: Boulder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Boulder : BaseEffect
{
  private const float SPEED = 2.5f;
  private const int BOUNCES_AMOUNT = 3;
  private const float SINGLE_BOUNCE_TIMER = 2f;
  private const float BASE_HEIGHT_MULTIPLIER = 10f;
  private const float BASE_LENGTH_MULTIPLIER = 40f;
  private const float INITIAL_ANGLE_RANGE = 200f;
  private const float CHARGE_VECTOR_MULTIPLIER = 0.777f;
  private const float Z_SORTING_FIX = 5f;
  private const int NO_TOUCH_ID = -2;
  private float angle;
  private float angleRotation;
  private float impactEffect;
  public GameObject mainSprite;
  public GameObject shadowSprite;
  private SpriteRenderer shadowRenderer;
  private Transform mainTransform;
  private Transform shadowTransform;
  private Vector2 _previous_bounce_position;
  private List<Vector2> _bounce_positions = new List<Vector2>();
  private int _bounces_left;
  private float _force_timer;
  private static bool _charge_started;
  private static Vector2 _initial_charge_position;
  private static Touch _latest_touch;
  private static int _latest_touch_id = -2;

  public override void Awake()
  {
    base.Awake();
    this.sprite_renderer = this.mainSprite.GetComponent<SpriteRenderer>();
    this.shadowRenderer = this.shadowSprite.GetComponent<SpriteRenderer>();
    this.mainTransform = this.mainSprite.transform;
    this.shadowTransform = this.shadowSprite.transform;
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    this.updateForce(pElapsed);
    if ((double) this.impactEffect > 0.0)
      this.impactEffect -= pElapsed;
    if ((double) this.position_height == 0.0)
      return;
    this.angle += this.angleRotation * pElapsed;
    this.mainTransform.localEulerAngles = new Vector3(0.0f, 0.0f, this.angle);
  }

  private void updateForce(float pElapsed)
  {
    this._force_timer -= pElapsed * 2.5f;
    if ((double) this._force_timer <= 0.0)
    {
      this._force_timer = 2f;
      this.actionLanded();
    }
    else
    {
      float heightPosition = this.getHeightPosition();
      Vector2 vector2 = this.calcCurrentPos();
      this.setCurrentPosition(vector2.x, vector2.y, heightPosition);
      this.updateCurrentPosition();
    }
  }

  private void updateShadow()
  {
    double num1 = ((double) this.position_height / -5.0 + 10.0) / 10.0;
    this.setShadowAlpha(Mathf.Clamp((float) num1, 0.15f, 1f) * 0.3f);
    float num2 = Mathf.Clamp((float) num1, 0.25f, 0.9f) * 0.3f;
    Vector3 localScale = this.shadowTransform.localScale;
    ((Vector3) ref localScale).Set(num2, num2, 1f);
    this.shadowTransform.localScale = localScale;
  }

  private void setShadowAlpha(float pVal)
  {
    float num = pVal;
    if ((double) num < 0.0)
      num = 0.0f;
    Color color = this.shadowRenderer.color;
    color.a = num;
    this.shadowRenderer.color = color;
  }

  private void spawnEffect(string pEffectID)
  {
    if ((double) this.impactEffect > 0.0)
      return;
    this.impactEffect = 0.8f;
    Vector3 pPos = Vector2.op_Implicit(this.current_position);
    pPos.y -= 2f;
    EffectsLibrary.spawnAt(pEffectID, pPos, this.mainTransform.localScale.x);
  }

  internal void actionLanded()
  {
    this._previous_bounce_position = this.current_position;
    --this._bounces_left;
    this.current_tile = World.world.GetTile((int) ((Component) this).transform.localPosition.x, (int) ((Component) this).transform.localPosition.y);
    bool flag = true;
    if (this.current_tile != null && this.current_tile.Type.lava)
      flag = false;
    if (this._bounces_left < 1)
      flag = false;
    if (flag)
      this.sequencedBounce();
    else
      this.explosion();
  }

  private void sequencedBounce()
  {
    Vector3 pVec = Vector2.op_Implicit(this.current_position);
    pVec.y -= 2f;
    EffectsLibrary.spawnExplosionWave(pVec, (float) this._bounces_left * 0.14f, 6f);
    World.world.startShake(pIntensity: 1f);
    if (!Toolbox.inMapBorder(ref this.current_position))
    {
      this.spawnEffect("fx_boulder_impact_water");
    }
    else
    {
      if (this.current_tile == null)
        return;
      if (this.current_tile.Type.ocean)
        this.spawnEffect("fx_boulder_impact_water");
      else
        this.spawnEffect("fx_boulder_impact");
      World.world.loopWithBrush(this.current_tile, Brush.get(5), new PowerActionWithID(Boulder.tileDrawBoulder));
      World.world.applyForceOnTile(this.current_tile, 5, 0.5f, false);
      World.world.conway_layer.checkKillRange(this.current_tile.pos, 5);
    }
  }

  private void explosion()
  {
    if (this.current_tile == null || this.current_tile.Type.ocean)
      this.spawnEffect("fx_boulder_impact_water");
    else
      this.spawnEffect("fx_boulder_impact");
    this.impactEffect = 0.0f;
    if (Toolbox.inMapBorder(ref this.current_position))
      MapAction.damageWorld(this.current_tile, 10, AssetManager.terraform.get("bomb"));
    this.spawnEffect("fx_explosion_small");
    this.controller.killObject((BaseEffect) this);
  }

  public static bool tileDrawBoulder(WorldTile pTile, string pPowerID)
  {
    pTile.doUnits((Action<Actor>) (pActor =>
    {
      AchievementLibrary.ball_to_ball.checkBySignal((object) pActor);
      pActor.getHitFullHealth(AttackType.Gravity);
    }));
    if (pTile.Type.ocean && Randy.randomChance(0.3f))
      World.world.drop_manager.spawnParabolicDrop(pTile, "rain", pMinHeight: 1f, pMaxHeight: 30f, pMinRadius: 0.7f, pMaxRadius: 22f);
    if (pTile.Type.lava && Randy.randomChance(0.3f))
      World.world.drop_manager.spawnParabolicDrop(pTile, "lava", pMinHeight: 1f, pMaxHeight: 30f, pMinRadius: 0.7f, pMaxRadius: 22f);
    MapAction.decreaseTile(pTile, true, "destroy");
    return true;
  }

  public void spawnOn(Vector2 pPosition)
  {
    this._bounce_positions.Clear();
    this._force_timer = !Boulder.isRandomLaunch(pPosition) ? 2f : 1f;
    this._bounces_left = 3;
    this.angle = 0.0f;
    this.angleRotation = Randy.randomFloat(-200f, 200f);
    this.impactEffect = 0.0f;
    Vector2 vector2;
    if (Boulder.isRandomLaunch(pPosition))
    {
      vector2.x = Randy.randomFloat(-40f, 40f);
      vector2.y = Randy.randomFloat(-40f, 40f);
      vector2 = Vector2.ClampMagnitude(vector2, 40f);
    }
    else
      vector2 = Vector2.op_Multiply(Boulder.chargeVector(pPosition), 0.777f);
    this._previous_bounce_position = pPosition;
    this._previous_bounce_position.y -= this.getHeightPosition();
    this._previous_bounce_position = Vector2.op_Subtraction(this._previous_bounce_position, Vector2.op_Multiply(vector2, this.getBounceProgress()));
    for (int index = 0; index < 3; ++index)
    {
      int num = index + 1;
      this._bounce_positions.Add(new Vector2()
      {
        x = this._previous_bounce_position.x + vector2.x * (float) num,
        y = this._previous_bounce_position.y + vector2.y * (float) num
      });
    }
    this.updateCurrentPosition();
    Boulder.endCharging();
  }

  private void setCurrentPosition(float pX, float pY, float pHeight)
  {
    this.current_position.x = pX;
    this.current_position.y = pY;
    this.position_height = pHeight;
  }

  private void updateCurrentPosition()
  {
    Vector3 localPosition1 = ((Component) this).transform.localPosition;
    localPosition1.x = this.current_position.x;
    localPosition1.y = this.current_position.y;
    localPosition1.z = this.position_height + 5f;
    ((Component) this).transform.localPosition = localPosition1;
    Vector3 localPosition2 = this.mainTransform.localPosition;
    localPosition2.y = this.position_height;
    this.mainTransform.localPosition = localPosition2;
    this.updateShadow();
  }

  private float getBounceProgress() => (float) (1.0 - (double) this._force_timer / 2.0);

  private float getBounceProgressMirrored()
  {
    return 1f - Mathf.Abs((float) ((double) this.getBounceProgress() * 2.0 - 1.0));
  }

  private float getHeightProgress()
  {
    return iTween.easeOutQuad(0.0f, 1f, this.getBounceProgressMirrored());
  }

  private float getHeightPosition()
  {
    return (float) ((double) this._bounces_left * (double) this.getHeightProgress() * 10.0);
  }

  private int getCurrentBounceIndex() => 3 - this._bounces_left;

  private Vector2 getNextBouncePos() => this._bounce_positions[this.getCurrentBounceIndex()];

  private Vector2 calcCurrentPos()
  {
    return Vector2.Lerp(this._previous_bounce_position, this.getNextBouncePos(), this.getBounceProgress());
  }

  public static void chargeBoulder(Vector2 pPosition, Touch pTouch = default (Touch))
  {
    Boulder._latest_touch = pTouch;
    if (ScrollWindow.isWindowActive())
      Boulder.endCharging();
    else if (HotkeyLibrary.many_mod.isHolding() || !InputHelpers.mouseSupported && DebugConfig.isOn(DebugOption.FastSpawn))
    {
      if (Boulder._charge_started)
        Boulder.endCharging();
      Boulder.releaseManyBoulders(pPosition);
    }
    else if (Boulder.isInteractionJustStarted())
    {
      Boulder.startCharging(pPosition);
    }
    else
    {
      if (!Boulder.isInteractionJustEnded())
        return;
      Boulder.releaseBoulder();
    }
  }

  private static void startCharging(Vector2 pPosition)
  {
    Boulder._charge_started = true;
    Boulder._initial_charge_position = pPosition;
    Boulder._latest_touch_id = ((Touch) ref Boulder._latest_touch).fingerId;
  }

  private static void endCharging()
  {
    Boulder._charge_started = false;
    Boulder._latest_touch_id = -2;
  }

  public static void checkRelease()
  {
    if (!Boulder._charge_started)
      return;
    if (!Boulder.isBoulderPowerSelected())
    {
      Boulder.endCharging();
    }
    else
    {
      Boulder.spawnParticles();
      if (!Boulder.isInteractionJustEnded())
        return;
      Boulder.releaseBoulder();
    }
  }

  private static void releaseManyBoulders(Vector2 pPosition)
  {
    Boulder._initial_charge_position = pPosition;
    Boulder.releaseBoulder();
  }

  private static void releaseBoulder()
  {
    EffectsLibrary.spawnAt("fx_boulder", Boulder.getPointerPosition(), 1f);
  }

  private static void spawnParticles()
  {
    Vector2 zero = Vector2.zero;
    if (!Boulder.getPointerPositionPure(ref zero) || Boulder.isRandomLaunch(zero))
      return;
    EffectsLibrary.spawnAt("fx_boulder_charge", zero, 1f);
  }

  private static bool isRandomLaunch(Vector2 pPosition)
  {
    Vector2 vector2 = Boulder.chargeVector(pPosition);
    return (double) ((Vector2) ref vector2).magnitude < 1.5;
  }

  private static bool isBoulderPowerSelected()
  {
    return PowerButtonSelector.instance.selectedButton?.godPower?.id == "bowling_ball";
  }

  private static Vector2 chargeVector(Vector2 pPosition)
  {
    return Vector2.op_Subtraction(Boulder._initial_charge_position, pPosition);
  }

  public static Vector2 chargeVector() => Boulder.chargeVector(Boulder.getPointerPosition());

  private static bool isInteractionJustStarted()
  {
    if (Boulder._charge_started)
      return false;
    if (InputHelpers.mouseSupported)
    {
      if (Input.GetMouseButtonDown(0))
        return true;
    }
    else if (((Touch) ref Boulder._latest_touch).fingerId != Boulder._latest_touch_id)
      return true;
    return false;
  }

  private static bool isInteractionJustEnded()
  {
    if (InputHelpers.mouseSupported)
    {
      if (Input.GetMouseButtonUp(0))
        return true;
    }
    else if (Input.touchCount == 0 || ((Touch) ref Boulder._latest_touch).phase == 3)
      return true;
    return false;
  }

  private static Vector2 getPointerPosition()
  {
    return InputHelpers.mouseSupported ? World.world.getMousePos() : Vector2.op_Implicit(World.world.camera.ScreenToWorldPoint(Vector2.op_Implicit(((Touch) ref Boulder._latest_touch).position)));
  }

  private static bool getPointerPositionPure(ref Vector2 pPosition)
  {
    if (InputHelpers.mouseSupported)
    {
      pPosition = World.world.getMousePos();
      return true;
    }
    Touch pTouch;
    if (!World.world.player_control.getTouchPos(out pTouch))
      return false;
    pPosition = Vector2.op_Implicit(World.world.camera.ScreenToWorldPoint(Vector2.op_Implicit(((Touch) ref pTouch).position)));
    return true;
  }
}
