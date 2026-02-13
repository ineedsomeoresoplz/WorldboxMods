// Decompiled with JetBrains decompiler
// Type: WorldLawsCursedStars
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WorldLawsCursedStars : MonoBehaviour
{
  private const int STARS_COUNT = 88;
  private const float ATTRACTION_SPEED_MIN = 0.05f;
  private const float ATTRACTION_SPEED_MAX = 0.3f;
  private const float ROTATION_SPEED_MIN = 0.05f;
  private const float ROTATION_SPEED_MAX = 0.5f;
  private const float RADIUS_MULTIPLIER = 1.25f;
  private const float MOUSE_AVOIDANCE_RADIUS = 40f;
  private const float MOUSE_AVOIDANCE_POWER = 15f;
  private const float ALPHA_FIX = 0.1f;
  private const float ALPHA_START = 8.4f;
  private static readonly Color OUTER_COLOR = Toolbox.makeColor("#FFAA00");
  private static readonly Color CENTER_COLOR = Toolbox.makeColor("#8B00FF");
  private float _attraction_speed;
  private float _rotation_speed;
  [SerializeField]
  private RectTransform _stars_parent;
  [SerializeField]
  private WorldLawsCursedStar _star_prefab;
  private float _angle;
  private Vector3 _center;
  private readonly List<WorldLawsCursedStar> _stars = new List<WorldLawsCursedStar>();
  private readonly List<float> _offset_indexes = new List<float>();

  private void Awake()
  {
    this._center = ((Transform) this._stars_parent).localPosition;
    for (int index = 0; index < 88; ++index)
    {
      this._stars.Add(Object.Instantiate<WorldLawsCursedStar>(this._star_prefab, (Transform) this._stars_parent));
      this._offset_indexes.Add((float) index);
    }
    this.updateStarsPositions();
  }

  private void OnEnable()
  {
    float curseProgressRatio = CursedSacrifice.getCurseProgressRatio();
    this._rotation_speed = Mathf.Lerp(0.05f, 0.5f, curseProgressRatio);
    this._attraction_speed = Mathf.Lerp(0.05f, 0.3f, curseProgressRatio);
  }

  private void Update() => this.updateStarsPositions();

  private void updateStarsPositions()
  {
    if (this._stars.Count == 0)
      return;
    float curseProgressRatio = CursedSacrifice.getCurseProgressRatio();
    this._angle += this._rotation_speed * Time.deltaTime;
    for (int index = 0; index < this._stars.Count; ++index)
    {
      WorldLawsCursedStar star = this._stars[index];
      Transform transform = ((Component) star).transform;
      if ((double) this._offset_indexes[index] <= 0.0)
      {
        this._offset_indexes[index] += 87f;
        star.toggleEgg(CursedSacrifice.isLatestWasEgg());
        star.toggleFilled(Randy.randomChance(curseProgressRatio));
      }
      else
        this._offset_indexes[index] -= this._attraction_speed;
      if (star.isFilled())
        star.setStarsTransparency(1f);
      else
        star.setStarsTransparency(0.0f);
      float offsetIndex = this._offset_indexes[index];
      float num1 = (float) index + this._angle;
      Vector3 center = this._center;
      center.x += (float) ((double) Mathf.Cos(num1) * (double) offsetIndex * 1.25);
      center.y += (float) ((double) Mathf.Sin(num1) * (double) offsetIndex * 1.25);
      transform.localPosition = center;
      this.mouseAvoidance(transform, offsetIndex);
      float num2 = Mathf.Lerp(0.5f, 1f, this.normalizedDistanceFromCenter(offsetIndex));
      transform.localScale = new Vector3(num2, num2);
      this.colorize(star, offsetIndex);
    }
  }

  private void mouseAvoidance(Transform pTransform, float pIndex)
  {
    Vector2 vector2;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(this._stars_parent, Vector2.op_Implicit(Input.mousePosition), (Camera) null, ref vector2);
    float num1 = 15f * (float) (1.0 - (double) Mathf.Min(Vector2.Distance(Vector2.op_Implicit(pTransform.localPosition), vector2), 40f) / 40.0);
    Vector3 vector3 = Vector3.op_Subtraction(pTransform.localPosition, Vector2.op_Implicit(vector2));
    Vector3 normalized = ((Vector3) ref vector3).normalized;
    float num2 = Mathf.Max(this.normalizedDistanceFromCenter(pIndex), 0.2f);
    Transform transform = pTransform;
    transform.localPosition = Vector3.op_Addition(transform.localPosition, Vector3.op_Multiply(new Vector3(num1 * normalized.x, num1 * normalized.y), num2));
  }

  private void colorize(WorldLawsCursedStar pStar, float pIndex)
  {
    float num = this.normalizedDistanceFromCenter(pIndex);
    Color pColor = Toolbox.blendColor(WorldLawsCursedStars.OUTER_COLOR, WorldLawsCursedStars.CENTER_COLOR, num * 1.35f);
    float pValue = (float) (8.3999996185302734 - ((double) pIndex + 1.0) / 8.8000001907348633);
    pStar.setColorMultiplyAlphaBoth(pColor, pValue);
  }

  private float normalizedDistanceFromCenter(float pIndex)
  {
    return (float) (((double) pIndex + 1.0) / 88.0);
  }
}
