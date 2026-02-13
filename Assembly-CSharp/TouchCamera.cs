// Decompiled with JetBrains decompiler
// Type: TouchCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TouchCamera : MonoBehaviour
{
  private Vector2?[] oldTouchPositions = new Vector2?[2];
  private Vector2 oldTouchVector;
  private float oldTouchDistance;
  private Camera _camera;
  private const int orthographicSizeMin = 10;
  internal float orthographicSizeMax = 130f;

  private void Awake() => this._camera = Camera.main;

  private void Update()
  {
    switch (Input.touchCount)
    {
      case 0:
        this.oldTouchPositions[0] = new Vector2?();
        this.oldTouchPositions[1] = new Vector2?();
        break;
      case 1:
        if (!this.oldTouchPositions[0].HasValue || this.oldTouchPositions[1].HasValue)
        {
          Vector2?[] oldTouchPositions = this.oldTouchPositions;
          Touch touch = Input.GetTouch(0);
          Vector2? nullable = new Vector2?(((Touch) ref touch).position);
          oldTouchPositions[0] = nullable;
          this.oldTouchPositions[1] = new Vector2?();
          break;
        }
        Touch touch1 = Input.GetTouch(0);
        Vector2 position1 = ((Touch) ref touch1).position;
        Transform transform1 = ((Component) this).transform;
        Vector3 position2 = transform1.position;
        Transform transform2 = ((Component) this).transform;
        Vector2? oldTouchPosition1 = this.oldTouchPositions[0];
        Vector2 vector2_1 = position1;
        Vector2? nullable1 = oldTouchPosition1.HasValue ? new Vector2?(Vector2.op_Subtraction(oldTouchPosition1.GetValueOrDefault(), vector2_1)) : new Vector2?();
        float orthographicSize1 = this._camera.orthographicSize;
        Vector2? nullable2 = nullable1.HasValue ? new Vector2?(Vector2.op_Multiply(nullable1.GetValueOrDefault(), orthographicSize1)) : new Vector2?();
        float pixelHeight = (float) this._camera.pixelHeight;
        Vector3 vector3_1 = Vector2.op_Implicit((nullable2.HasValue ? new Vector2?(Vector2.op_Multiply(Vector2.op_Division(nullable2.GetValueOrDefault(), pixelHeight), 2f)) : new Vector2?()).Value);
        Vector3 vector3_2 = transform2.TransformDirection(vector3_1);
        transform1.position = Vector3.op_Addition(position2, vector3_2);
        this.oldTouchPositions[0] = new Vector2?(position1);
        break;
      default:
        if (!this.oldTouchPositions[1].HasValue)
        {
          Vector2?[] oldTouchPositions1 = this.oldTouchPositions;
          Touch touch2 = Input.GetTouch(0);
          Vector2? nullable3 = new Vector2?(((Touch) ref touch2).position);
          oldTouchPositions1[0] = nullable3;
          Vector2?[] oldTouchPositions2 = this.oldTouchPositions;
          touch2 = Input.GetTouch(1);
          Vector2? nullable4 = new Vector2?(((Touch) ref touch2).position);
          oldTouchPositions2[1] = nullable4;
          Vector2? oldTouchPosition2 = this.oldTouchPositions[0];
          Vector2? oldTouchPosition3 = this.oldTouchPositions[1];
          this.oldTouchVector = (oldTouchPosition2.HasValue & oldTouchPosition3.HasValue ? new Vector2?(Vector2.op_Subtraction(oldTouchPosition2.GetValueOrDefault(), oldTouchPosition3.GetValueOrDefault())) : new Vector2?()).Value;
          this.oldTouchDistance = ((Vector2) ref this.oldTouchVector).magnitude;
          break;
        }
        Vector2 vector2_2;
        // ISSUE: explicit constructor call
        ((Vector2) ref vector2_2).\u002Ector((float) this._camera.pixelWidth, (float) this._camera.pixelHeight);
        Vector2[] vector2Array1 = new Vector2[2];
        Touch touch3 = Input.GetTouch(0);
        vector2Array1[0] = ((Touch) ref touch3).position;
        Touch touch4 = Input.GetTouch(1);
        vector2Array1[1] = ((Touch) ref touch4).position;
        Vector2[] vector2Array2 = vector2Array1;
        Vector2 vector2_3 = Vector2.op_Subtraction(vector2Array2[0], vector2Array2[1]);
        float magnitude = ((Vector2) ref vector2_3).magnitude;
        Transform transform3 = ((Component) this).transform;
        Vector3 position3 = transform3.position;
        Transform transform4 = ((Component) this).transform;
        Vector2? oldTouchPosition4 = this.oldTouchPositions[0];
        Vector2? nullable5 = this.oldTouchPositions[1];
        Vector2? nullable6 = oldTouchPosition4.HasValue & nullable5.HasValue ? new Vector2?(Vector2.op_Addition(oldTouchPosition4.GetValueOrDefault(), nullable5.GetValueOrDefault())) : new Vector2?();
        Vector2 vector2_4 = vector2_2;
        Vector2? nullable7;
        if (!nullable6.HasValue)
        {
          nullable5 = new Vector2?();
          nullable7 = nullable5;
        }
        else
          nullable7 = new Vector2?(Vector2.op_Subtraction(nullable6.GetValueOrDefault(), vector2_4));
        Vector2? nullable8 = nullable7;
        float orthographicSize2 = this._camera.orthographicSize;
        Vector2? nullable9;
        if (!nullable8.HasValue)
        {
          nullable6 = new Vector2?();
          nullable9 = nullable6;
        }
        else
          nullable9 = new Vector2?(Vector2.op_Multiply(nullable8.GetValueOrDefault(), orthographicSize2));
        Vector2? nullable10 = nullable9;
        float y = vector2_2.y;
        Vector3 vector3_3 = Vector2.op_Implicit((nullable10.HasValue ? new Vector2?(Vector2.op_Division(nullable10.GetValueOrDefault(), y)) : new Vector2?()).Value);
        Vector3 vector3_4 = transform4.TransformDirection(vector3_3);
        transform3.position = Vector3.op_Addition(position3, vector3_4);
        this._camera.orthographicSize = Mathf.Clamp(this._camera.orthographicSize * (this.oldTouchDistance / magnitude), 10f, this.orthographicSizeMax);
        Transform transform5 = ((Component) this).transform;
        transform5.position = Vector3.op_Subtraction(transform5.position, ((Component) this).transform.TransformDirection(Vector2.op_Implicit(Vector2.op_Division(Vector2.op_Multiply(Vector2.op_Subtraction(Vector2.op_Addition(vector2Array2[0], vector2Array2[1]), vector2_2), this._camera.orthographicSize), vector2_2.y))));
        this.oldTouchPositions[0] = new Vector2?(vector2Array2[0]);
        this.oldTouchPositions[1] = new Vector2?(vector2Array2[1]);
        this.oldTouchVector = vector2_3;
        this.oldTouchDistance = magnitude;
        break;
    }
  }
}
