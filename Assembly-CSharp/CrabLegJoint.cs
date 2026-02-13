// Decompiled with JetBrains decompiler
// Type: CrabLegJoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CrabLegJoint : MonoBehaviour
{
  [Header("Joints")]
  public Transform Joint0;
  public Transform Joint1;
  public Transform Hand;
  [Header("Target")]
  public Transform Target;
  private float length0;
  private float length1;
  public float targetDistance;
  public bool mirrored;
  internal Crabzilla crabzilla;
  public float angleMax;
  public float angleMin;
  public float defaultAngle;
  private float atan;
  private float jointAngle0;
  private float jointAngle1;
  public float angle0;
  public float angle1;
  public float groundAngleMin = 50f;
  public float groundAngleMax = 140f;
  internal Transform bodyPoint;
  public float actual_z_pos;

  internal void create()
  {
    this.actual_z_pos = ((Component) this).transform.localPosition.z;
    this.length0 = Vector2.Distance(Vector2.op_Implicit(this.Joint0.position), Vector2.op_Implicit(this.Joint1.position));
    this.length1 = Vector2.Distance(Vector2.op_Implicit(this.Joint1.position), Vector2.op_Implicit(this.Hand.position));
    int num = this.mirrored ? 1 : 0;
    this.targetDistance = Vector2.Distance(Vector2.op_Implicit(this.Joint0.position), Vector2.op_Implicit(this.Target.position));
    Vector2 vector2 = Vector2.op_Implicit(Vector3.op_Subtraction(this.Target.position, this.Joint0.position));
    Quaternion rotation = ((Component) this.crabzilla).transform.rotation;
    this.atan = (float) (-(double) ((Quaternion) ref rotation).eulerAngles.z + (double) Mathf.Atan2(vector2.y, vector2.x) * 57.295780181884766);
    this.defaultAngle = Mathf.Acos((float) (((double) this.targetDistance * (double) this.targetDistance + (double) this.length0 * (double) this.length0 - (double) this.length1 * (double) this.length1) / (2.0 * (double) this.targetDistance * (double) this.length0))) * 57.29578f;
    this.angleMin = this.defaultAngle + 20f;
    this.angleMax = this.defaultAngle + 20f;
    this.bodyPoint = new GameObject("leg_point_" + ((Object) ((Component) this).transform).name)
    {
      transform = {
        position = new Vector3(((Component) this).transform.position.x, ((Component) this).transform.position.y, 0.0f),
        parent = ((Component) this.crabzilla.mainBody).transform
      }
    }.transform;
  }

  public bool isAngleOk(float pMinAngle, float pMaxAngle)
  {
    this.angleMin = this.defaultAngle + pMinAngle;
    this.angleMax = this.defaultAngle + pMaxAngle;
    int num1 = Toolbox.inBounds(this.angle0, this.angleMin, this.angleMax) ? 1 : 0;
    Vector2 vector2 = Vector2.op_Implicit(Vector3.op_Subtraction(((Component) this.Joint1).transform.position, ((Component) this.Hand).transform.position));
    int num2 = Toolbox.inBounds(Mathf.Atan2(vector2.y, vector2.x) * 57.29578f, this.groundAngleMin, this.groundAngleMax) ? 1 : 0;
    return (num1 & num2) != 0;
  }

  internal void LateUpdate()
  {
    Vector3 position = this.bodyPoint.position;
    position.z = 0.0f;
    ((Component) this).transform.position = position;
    ((Component) this).transform.localPosition = new Vector3(((Component) this).transform.localPosition.x, ((Component) this).transform.localPosition.y, this.actual_z_pos);
    this.targetDistance = Vector2.Distance(Vector2.op_Implicit(this.Joint0.position), Vector2.op_Implicit(this.Target.position));
    Vector2 vector2 = Vector2.op_Implicit(Vector3.op_Subtraction(this.Target.position, this.Joint0.position));
    Quaternion rotation = ((Component) this.crabzilla).transform.rotation;
    this.atan = (float) (-(double) ((Quaternion) ref rotation).eulerAngles.z + (double) Mathf.Atan2(vector2.y, vector2.x) * 57.295780181884766);
    if ((double) this.length0 + (double) this.length1 < (double) this.targetDistance)
    {
      this.jointAngle0 = this.atan;
      this.jointAngle1 = 0.0f;
    }
    else
    {
      this.angle0 = Mathf.Acos((float) (((double) this.targetDistance * (double) this.targetDistance + (double) this.length0 * (double) this.length0 - (double) this.length1 * (double) this.length1) / (2.0 * (double) this.targetDistance * (double) this.length0))) * 57.29578f;
      this.angle1 = Mathf.Acos((float) (((double) this.length1 * (double) this.length1 + (double) this.length0 * (double) this.length0 - (double) this.targetDistance * (double) this.targetDistance) / (2.0 * (double) this.length1 * (double) this.length0))) * 57.29578f;
      if (this.mirrored)
      {
        this.jointAngle0 = this.atan + this.angle0;
        this.jointAngle1 = 180f + this.angle1;
      }
      else
      {
        this.jointAngle0 = this.atan - this.angle0;
        this.jointAngle1 = 180f - this.angle1;
      }
    }
    if (float.IsNaN(this.jointAngle0))
      return;
    Vector3 localEulerAngles1 = ((Component) this.Joint0).transform.localEulerAngles;
    localEulerAngles1.z = this.jointAngle0;
    ((Component) this.Joint0).transform.localEulerAngles = localEulerAngles1;
    Vector3 localEulerAngles2 = ((Component) this.Joint1).transform.localEulerAngles;
    localEulerAngles2.z = this.jointAngle1;
    ((Component) this.Joint1).transform.localEulerAngles = localEulerAngles2;
  }
}
