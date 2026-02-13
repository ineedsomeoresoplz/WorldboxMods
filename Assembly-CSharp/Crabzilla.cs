// Decompiled with JetBrains decompiler
// Type: Crabzilla
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Crabzilla : BaseActorComponent
{
  internal const float HIGH_HP_THRESHOLD = 0.7f;
  internal const float MED_HP_THRESHOLD = 0.35f;
  private CrabLeg[] list_legs;
  private CrabLegJoint[] list_joints;
  private CrabLimbGroup[] list_limbs;
  private int active_limb = -1;
  public CrabBody mainBody;
  internal const float angle0_min = -20f;
  internal const float angle0_max = 30f;
  public GameObject armTarget;
  public GameObject mouthSprite;
  private SpriteAnimation mouthSpriteAnim;
  private bool _beam_enabled;
  private Vector3 bodyRotationTarget;
  private Vector3 bodyRotation;
  private float moveRotationLimit = 5f;
  private Vector3 bodyPosTarget;
  private Vector3 bodyPos;
  private float bodyPosTimeout;
  public CrabArm arm1;
  public CrabArm arm2;
  public float z_pos = 10f;

  internal override void create(Actor pActor)
  {
    base.create(pActor);
    ((Component) this).transform.position = Vector2.op_Implicit(this.actor.current_position);
    this.bodyPos = new Vector3(0.0f, 27.8f, 0.0f);
    this.bodyPosTarget = new Vector3(0.0f, 27.8f, 0.0f);
    this.mouthSpriteAnim = this.mouthSprite.GetComponent<SpriteAnimation>();
    this.createLimbs();
    ControllableUnit.setControllableCreatureCrabzilla(this.actor);
    if (Config.isMobile)
      WorldTip.showNow("crabzilla_controls_mobile", pPosition: "top", pTime: 8f);
    else
      WorldTip.showNow("crabzilla_controls_pc", pPosition: "top", pTime: 8f);
    if (Config.joyControls)
      UltimateJoystick.ResetJoysticks();
    Vector3 position = ((Component) this).transform.position;
    position.z = this.z_pos;
    ((Component) this).transform.position = position;
    this.actor.current_position = Vector2.op_Implicit(((Component) this).transform.position);
  }

  public bool isBeamEnabled() => this._beam_enabled;

  internal void legMoved()
  {
    if ((double) this.bodyPosTimeout > 0.0)
      return;
    this.bodyPosTarget.y = 27.8f + Randy.randomFloat(-3f, 3f);
  }

  public override void update(float pElapsed)
  {
    if ((double) this.bodyPosTimeout > 0.0)
      this.bodyPosTimeout -= pElapsed;
    this.arm1.update(pElapsed);
    this.arm2.update(pElapsed);
    foreach (CrabLeg listLeg in this.list_legs)
      listLeg.update(pElapsed);
    if (this.isAnyLimbFlickering())
      this.list_limbs[this.active_limb].update(pElapsed);
    this._beam_enabled = ControllableUnit.isAttackPressedLeft();
    this.mouthSprite.SetActive(this.isBeamEnabled());
    if (this.mouthSprite.gameObject.activeSelf)
    {
      this.mouthSpriteAnim.update(pElapsed);
      MusicBox.inst.playDrawingSound("event:/SFX/UNIQUE/Crabzilla/CrabzillaVoice", this.actor.current_position.x, this.actor.current_position.y);
    }
    Vector2 objA = ControllableUnit.getMovementVector();
    if (!ControllableUnit.isMovementActionActive())
      objA = Vector2.zero;
    this.bodyRotationTarget.z = (double) objA.x <= 0.0 ? ((double) objA.x >= 0.0 ? 0.0f : -this.moveRotationLimit) : this.moveRotationLimit;
    float num = World.world.elapsed * 60f;
    this.bodyRotation = Vector3.MoveTowards(this.bodyRotation, this.bodyRotationTarget, 0.7f * num);
    if ((double) objA.y > 0.0 && (double) this.bodyRotation.z > (double) this.moveRotationLimit)
      this.bodyRotation.z = this.moveRotationLimit;
    else if ((double) objA.y < 0.0 && (double) this.bodyRotation.z < -(double) this.moveRotationLimit)
      this.bodyRotation.z = -this.moveRotationLimit;
    this.bodyPos.z = 0.0f;
    this.bodyPosTarget.z = 0.0f;
    ((Component) this.mainBody).transform.localRotation = Quaternion.Euler(this.bodyRotation);
    this.bodyPos = Vector2.op_Implicit(Vector2.MoveTowards(Vector2.op_Implicit(this.bodyPos), Vector2.op_Implicit(this.bodyPosTarget), 0.7f * num));
    ((Component) this.mainBody).transform.localPosition = this.bodyPos;
    Vector3 position = ((Component) this).transform.position;
    if (!object.Equals((object) objA, (object) Vector2.zero))
    {
      Vector2 vector2_1 = Vector2.op_Implicit(((Component) this).transform.position);
      Vector2 vector2_2 = Vector2.MoveTowards(vector2_1, Vector2.op_Addition(vector2_1, Vector2.op_Multiply(Vector2.op_Multiply(objA, 0.2f), num)), 1f * num);
      // ISSUE: explicit constructor call
      ((Vector3) ref position).\u002Ector(vector2_2.x, vector2_2.y);
      if ((double) position.x < 0.0)
        position.x = 0.0f;
      if ((double) position.y < 0.0)
        position.y = 0.0f;
      if ((double) position.x > (double) MapBox.width)
        position.x = (float) MapBox.width;
      if ((double) position.y > (double) MapBox.height)
        position.y = (float) MapBox.height;
      position.z = this.z_pos;
    }
    position.x += this.actor.shake_offset.x;
    position.y += this.actor.shake_offset.y;
    ((Component) this).transform.position = position;
    this.actor.current_position = Vector2.op_Implicit(((Component) this).transform.position);
    this.actor.dirty_current_tile = true;
    this.updateArms();
  }

  private void updateArms()
  {
    if (Config.joyControls)
    {
      Vector2 vector2 = Vector2.op_Implicit(this.armTarget.transform.position);
      float axisVerticalRight = ControllableUnit.getJoyAxisVerticalRight();
      float axisHorizontalRight = ControllableUnit.getJoyAxisHorizontalRight();
      Vector2 objA;
      // ISSUE: explicit constructor call
      ((Vector2) ref objA).\u002Ector(axisHorizontalRight, axisVerticalRight);
      if (!object.Equals((object) objA, (object) Vector2.zero))
      {
        vector2 = Vector2.MoveTowards(vector2, Vector2.op_Addition(vector2, Vector2.op_Multiply(objA, 2f)), 1f);
        if ((double) Toolbox.DistVec3(Vector2.op_Implicit(vector2), ((Component) this).transform.position) > 35.0)
          vector2 = Vector2.MoveTowards(Vector2.op_Implicit(((Component) this).transform.position), vector2, 35f);
      }
      this.armTarget.transform.position = Vector2.op_Implicit(vector2);
    }
    else
      this.armTarget.transform.position = Vector2.op_Implicit(World.world.getMousePos());
  }

  private void createLimbs()
  {
    this.list_joints = ((Component) this).GetComponentsInChildren<CrabLegJoint>(false);
    foreach (CrabLegJoint listJoint in this.list_joints)
      listJoint.crabzilla = this;
    this.list_legs = ((Component) this).GetComponentsInChildren<CrabLeg>(false);
    foreach (CrabLeg listLeg in this.list_legs)
      listLeg.crabzilla = this;
    this.arm1.crabzilla = this;
    this.arm2.crabzilla = this;
    this.list_limbs = new CrabLimbGroup[Enum.GetNames(typeof (CrabLimb)).Length];
    for (int pCrabLimb = 0; pCrabLimb < this.list_limbs.Length; ++pCrabLimb)
      this.list_limbs[pCrabLimb] = new CrabLimbGroup((CrabLimb) pCrabLimb, this.actor);
    this.list_limbs.Shuffle<CrabLimbGroup>();
    foreach (CrabLeg listLeg in this.list_legs)
    {
      listLeg.create();
      listLeg.update(World.world.delta_time);
    }
    foreach (CrabLegJoint listJoint in this.list_joints)
    {
      listJoint.create();
      listJoint.LateUpdate();
    }
    this.update(World.world.delta_time);
    foreach (CrabLeg listLeg in this.list_legs)
      listLeg.moveLeg();
  }

  internal static bool getHit(BaseSimObject pSelf, BaseSimObject pAttackedBy = null, WorldTile pTile = null)
  {
    Actor a = pSelf.a;
    Crabzilla actorComponent = a.getActorComponent<Crabzilla>();
    if ((double) a.getHealthRatio() > 0.44999998807907104)
      return true;
    actorComponent.ShowLimbDamage();
    return true;
  }

  public void ShowLimbDamage()
  {
    if (this.isAnyLimbFlickering())
      return;
    ++this.active_limb;
    if (this.active_limb >= this.list_limbs.Length)
    {
      this.active_limb = 0;
      this.list_limbs.Shuffle<CrabLimbGroup>();
    }
    this.actor.startShake(0.05f);
    this.list_limbs[this.active_limb].showDamage();
  }

  private bool isAnyLimbFlickering()
  {
    return this.active_limb != -1 && this.list_limbs[this.active_limb].IsFlickering();
  }
}
