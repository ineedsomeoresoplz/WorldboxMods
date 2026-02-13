// Decompiled with JetBrains decompiler
// Type: PixelDetector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public static class PixelDetector
{
  public static bool GetSpritePixelColorUnderMousePointer(
    MonoBehaviour mono,
    out Vector2Int pVector)
  {
    pVector = new Vector2Int(-1, -1);
    Vector2 vector2 = Vector2.op_Implicit(Camera.main.ScreenToViewportPoint(Vector2.op_Implicit(Vector2.op_Implicit(Input.mousePosition))));
    if ((double) vector2.x > 0.0 && (double) vector2.x < 1.0 && (double) vector2.y > 0.0)
    {
      if ((double) vector2.y < 1.0)
      {
        Ray ray;
        try
        {
          ray = Camera.main.ViewportPointToRay(Vector2.op_Implicit(vector2));
        }
        catch (Exception ex)
        {
          return false;
        }
        return PixelDetector.IntersectsSprite(mono, ray, out pVector);
      }
    }
    return false;
  }

  private static bool IntersectsSprite(MonoBehaviour mono, Ray ray, out Vector2Int pVector)
  {
    SpriteRenderer component = ((Component) mono).GetComponent<SpriteRenderer>();
    pVector = new Vector2Int(-1, -1);
    if (Object.op_Equality((Object) component, (Object) null))
      return false;
    Sprite sprite = component.sprite;
    if (Object.op_Equality((Object) sprite, (Object) null))
      return false;
    Texture2D texture = sprite.texture;
    if (Object.op_Equality((Object) texture, (Object) null))
      return false;
    if (sprite.packed && sprite.packingMode == null)
    {
      Debug.LogError((object) "SpritePackingMode.Tight atlas packing is not supported!");
      return false;
    }
    Plane plane;
    // ISSUE: explicit constructor call
    ((Plane) ref plane).\u002Ector(((Component) mono).transform.forward, ((Component) mono).transform.position);
    float num1;
    if (!((Plane) ref plane).Raycast(ray, ref num1))
      return false;
    Matrix4x4 worldToLocalMatrix = ((Renderer) component).worldToLocalMatrix;
    Vector3 vector3 = ((Matrix4x4) ref worldToLocalMatrix).MultiplyPoint3x4(Vector3.op_Addition(((Ray) ref ray).origin, Vector3.op_Multiply(((Ray) ref ray).direction, num1)));
    Rect textureRect = sprite.textureRect;
    float pixelsPerUnit = sprite.pixelsPerUnit;
    float num2 = (float) ((Texture) texture).width * 0.0f;
    float num3 = (float) ((Texture) texture).height * 0.0f;
    int num4 = (int) ((double) vector3.x * (double) pixelsPerUnit + (double) num2);
    int num5 = (int) ((double) vector3.y * (double) pixelsPerUnit + (double) num3);
    if (num4 < 0 || (double) num4 < (double) ((Rect) ref textureRect).x || num4 >= Mathf.FloorToInt(((Rect) ref textureRect).xMax) || num5 < 0 || (double) num5 < (double) ((Rect) ref textureRect).y || num5 >= Mathf.FloorToInt(((Rect) ref textureRect).yMax))
      return false;
    pVector = new Vector2Int(num4, num5);
    return true;
  }
}
