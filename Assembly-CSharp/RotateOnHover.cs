// Decompiled with JetBrains decompiler
// Type: RotateOnHover
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class RotateOnHover : MonoBehaviour
{
  private const float TILT_ANGLE = 60f;
  private const float TILT_SPEED = 15f;
  public List<Shadow> shadows;
  private bool _is_hovering;
  private Vector3 _original_rotation;
  private Vector2 _original_pivot;
  private List<Vector2> _original_shadows;
  private RectTransform _rect_transform;

  private void Awake()
  {
    this._rect_transform = ((Component) this).GetComponent<RectTransform>();
    this._original_rotation = ((Transform) this._rect_transform).eulerAngles;
    this._original_pivot = this._rect_transform.pivot;
    this._original_shadows = new List<Vector2>();
    foreach (Shadow shadow in this.shadows)
      this._original_shadows.Add(shadow.effectDistance);
  }

  private void Start()
  {
    Button componentInParent = ((Component) this).GetComponentInParent<Button>();
    if (Object.op_Equality((Object) componentInParent, (Object) null))
      return;
    // ISSUE: method pointer
    componentInParent.OnHover(new UnityAction((object) this, __methodptr(\u003CStart\u003Eb__9_0)));
    // ISSUE: method pointer
    componentInParent.OnHoverOut(new UnityAction((object) this, __methodptr(\u003CStart\u003Eb__9_1)));
  }

  private void OnEnable()
  {
    this._is_hovering = false;
    ((Transform) this._rect_transform).eulerAngles = this._original_rotation;
    this._rect_transform.pivot = this._original_pivot;
    for (int index = 0; index < this.shadows.Count; ++index)
      this.shadows[index].effectDistance = this._original_shadows[index];
  }

  private void OnDisable()
  {
    this._is_hovering = false;
    ((Transform) this._rect_transform).eulerAngles = Vector3.zero;
  }

  private void Update()
  {
    if (!this._is_hovering)
    {
      float num = (float) (15.0 * (double) Time.deltaTime * 0.5);
      this._rect_transform.pivot = Vector2.Lerp(this._rect_transform.pivot, this._original_pivot, num);
      for (int index = 0; index < this.shadows.Count; ++index)
        this.shadows[index].effectDistance = Vector2.Lerp(this.shadows[index].effectDistance, this._original_shadows[index], num);
      ((Transform) this._rect_transform).rotation = Quaternion.Lerp(((Transform) this._rect_transform).rotation, Quaternion.Euler(this._original_rotation), num);
    }
    else
    {
      Vector2 vector2_1 = Vector2.op_Implicit(((Transform) this._rect_transform).InverseTransformPoint(Input.mousePosition));
      double x = (double) vector2_1.x;
      Rect rect1 = this._rect_transform.rect;
      double width = (double) ((Rect) ref rect1).width;
      float num1 = Mathf.Clamp((float) (x / width), -0.5f, 0.5f);
      double y = (double) vector2_1.y;
      Rect rect2 = this._rect_transform.rect;
      double height = (double) ((Rect) ref rect2).height;
      float num2 = Mathf.Clamp((float) (y / height), -0.5f, 0.5f);
      Vector3 vector3;
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3).\u002Ector(this._original_rotation.x - num2 * 60f, this._original_rotation.y + num1 * 60f, this._original_rotation.z);
      Vector2 vector2_2;
      // ISSUE: explicit constructor call
      ((Vector2) ref vector2_2).\u002Ector(num1 * 4f, num2 * 4f);
      foreach (Shadow shadow in this.shadows)
        shadow.effectDistance = vector2_2;
      this._rect_transform.pivot = new Vector2(this._original_pivot.x - num1 * 0.1f, this._original_pivot.y - num2 * 0.1f);
      ((Transform) this._rect_transform).rotation = Quaternion.Lerp(((Transform) this._rect_transform).rotation, Quaternion.Euler(vector3), 15f * Time.deltaTime);
    }
  }
}
