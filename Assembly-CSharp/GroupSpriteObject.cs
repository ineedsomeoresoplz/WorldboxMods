// Decompiled with JetBrains decompiler
// Type: GroupSpriteObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using FMOD.Studio;
using UnityEngine;

#nullable disable
public class GroupSpriteObject : MonoBehaviour
{
  internal EventInstance fmod_instance;
  internal Transform m_transform;
  internal SpriteRenderer sprite_renderer;
  private bool _has_sprite_renderer;
  private Vector2 _last_pos_v2 = new Vector2(-1f, -1f);
  private Vector2 _last_scale_v2 = new Vector2(-1f, -1f);
  private Vector3 _last_pos_v3 = new Vector3(-1f, -1f, -1f);
  private Vector3 _last_scale_v3 = new Vector3(-1f, -1f, -1f);
  private Vector3 _last_angles_v3 = new Vector3(-1f, -1f, -1f);
  private Color _last_color;
  private bool _last_flip_x;
  private int _last_sprite_hash_code = -1;
  private int _last_sprite_material = -1;
  public int last_id;

  public bool has_sprite_renderer => this._has_sprite_renderer;

  private void Awake() => this.create();

  protected void create()
  {
    this.m_transform = ((Component) this).gameObject.transform;
    this.sprite_renderer = ((Component) this).gameObject.GetComponent<SpriteRenderer>();
    if (!Object.op_Inequality((Object) this.sprite_renderer, (Object) null))
      return;
    this._has_sprite_renderer = true;
  }

  public void checkRotation(Vector3 pPos, BaseSimObject pSimObject, float pZ)
  {
    pPos.z = pZ;
    Vector3 pivot = new Vector3();
    ref Vector3 local = ref pSimObject.current_rotation;
    if ((double) local.y != 0.0 || (double) local.z != 0.0)
    {
      ((Vector3) ref pivot).Set(pSimObject.cur_transform_position.x, pSimObject.cur_transform_position.y, 0.0f);
      pPos = Toolbox.RotatePointAroundPivot(ref pPos, ref pivot, ref local);
      pPos.z = pZ;
    }
    this.setPosOnly(ref pPos);
    this.setLocalEulerAngles(pSimObject.current_rotation);
  }

  public void setPosOnly(Vector2 pPosition)
  {
    if ((double) this._last_pos_v3.x == (double) pPosition.x && (double) this._last_pos_v3.y == (double) pPosition.y)
      return;
    this._last_pos_v2 = pPosition;
    this._last_pos_v3 = Vector2.op_Implicit(pPosition);
    this.m_transform.localPosition = this._last_pos_v3;
  }

  public void setPosOnly(ref Vector2 pPosition)
  {
    if ((double) this._last_pos_v3.x == (double) pPosition.x && (double) this._last_pos_v3.y == (double) pPosition.y && (double) this._last_pos_v3.z == 0.0)
      return;
    this._last_pos_v2 = pPosition;
    this._last_pos_v3 = Vector2.op_Implicit(pPosition);
    this.m_transform.localPosition = this._last_pos_v3;
  }

  public void setPosOnly(ref Vector3 pPosition)
  {
    if ((double) this._last_pos_v3.x == (double) pPosition.x && (double) this._last_pos_v3.y == (double) pPosition.y && (double) this._last_pos_v3.z == (double) pPosition.z)
      return;
    this._last_pos_v2 = Vector2.op_Implicit(pPosition);
    this._last_pos_v3 = pPosition;
    this.m_transform.localPosition = this._last_pos_v3;
  }

  public void setRotation(ref Vector3 pVec)
  {
    if ((double) this._last_angles_v3.y == (double) pVec.y && (double) this._last_angles_v3.z == (double) pVec.z)
      return;
    this._last_angles_v3 = pVec;
    this.m_transform.eulerAngles = pVec;
  }

  public void setLocalEulerAngles(Vector3 pVec)
  {
    if ((double) this._last_angles_v3.y == (double) pVec.y && (double) this._last_angles_v3.z == (double) pVec.z)
      return;
    this._last_angles_v3 = pVec;
    this.m_transform.localEulerAngles = pVec;
  }

  public void setSprite(Sprite pSprite)
  {
    int hashCode = pSprite.GetHashCode();
    if (this._last_sprite_hash_code == hashCode)
      return;
    this.sprite_renderer.sprite = pSprite;
    this._last_sprite_hash_code = hashCode;
  }

  public void setFlipX(bool pFlipX)
  {
    if (this._last_flip_x == pFlipX)
      return;
    this._last_flip_x = pFlipX;
    this.sprite_renderer.flipX = pFlipX;
  }

  public void setSharedMat(Material pMaterial)
  {
    int hashCode = pMaterial.GetHashCode();
    if (this._last_sprite_material == hashCode)
      return;
    ((Renderer) this.sprite_renderer).sharedMaterial = pMaterial;
    this._last_sprite_material = hashCode;
  }

  public void setColor(ref Color pColor)
  {
    if ((double) this._last_color.r == (double) pColor.r && (double) this._last_color.g == (double) pColor.g && (double) this._last_color.b == (double) pColor.b && (double) this._last_color.a == (double) pColor.a)
      return;
    this.sprite_renderer.color = pColor;
    this._last_color = pColor;
  }

  public void setScale(float pScale)
  {
    if ((double) this._last_scale_v3.y == (double) pScale)
      return;
    this._last_scale_v2 = new Vector2(pScale, pScale);
    this._last_scale_v3 = Vector2.op_Implicit(this._last_scale_v2);
    this.m_transform.localScale = this._last_scale_v3;
  }

  public void setScale(float pScaleX, float pScaleY)
  {
    if ((double) this._last_scale_v2.y == (double) pScaleY && (double) this._last_scale_v2.x == (double) pScaleX)
      return;
    this._last_scale_v2 = new Vector2(pScaleX, pScaleY);
    this._last_scale_v3 = Vector2.op_Implicit(this._last_scale_v2);
    this.m_transform.localScale = this._last_scale_v3;
  }

  public void setScale(ref Vector3 pScaleVec)
  {
    if (!Vector3.op_Inequality(this._last_scale_v3, pScaleVec))
      return;
    this._last_scale_v3 = pScaleVec;
    this.m_transform.localScale = pScaleVec;
  }

  public void set(ref Vector2 pPosition, ref Vector3 pScale)
  {
    if ((double) this._last_pos_v2.x != (double) pPosition.x || (double) this._last_pos_v2.y != (double) pPosition.y)
    {
      this._last_pos_v2 = pPosition;
      this._last_pos_v3 = Vector2.op_Implicit(pPosition);
      this.m_transform.localPosition = this._last_pos_v3;
    }
    if ((double) this._last_scale_v2.y == (double) pScale.y && (double) this._last_scale_v2.x == (double) pScale.x)
      return;
    this._last_scale_v2 = Vector2.op_Implicit(pScale);
    this._last_scale_v3 = Vector2.op_Implicit(this._last_scale_v2);
    this.m_transform.localScale = pScale;
  }

  public void set(ref Vector2 pPosition, float pScale)
  {
    if ((double) this._last_pos_v3.x != (double) pPosition.x || (double) this._last_pos_v3.y != (double) pPosition.y)
    {
      this._last_pos_v2 = pPosition;
      this._last_pos_v3 = Vector2.op_Implicit(pPosition);
      this.m_transform.localPosition = this._last_pos_v3;
    }
    if ((double) this._last_scale_v2.x == (double) pScale)
      return;
    this.setScale(pScale);
  }

  public void set(ref Vector3 pPosition, float pScale)
  {
    if ((double) this._last_pos_v3.x != (double) pPosition.x || (double) this._last_pos_v3.y != (double) pPosition.y || (double) this._last_pos_v3.z != (double) pPosition.z)
    {
      this._last_pos_v2 = Vector2.op_Implicit(pPosition);
      this._last_pos_v3 = pPosition;
      this.m_transform.localPosition = this._last_pos_v3;
    }
    if ((double) this._last_scale_v2.x == (double) pScale)
      return;
    this.setScale(pScale);
  }

  public void set(ref Vector3 pPosition, ref Vector2 pScale)
  {
    if ((double) this._last_pos_v3.x != (double) pPosition.x || (double) this._last_pos_v3.y != (double) pPosition.y || (double) this._last_pos_v3.z != (double) pPosition.z)
    {
      this._last_pos_v2 = Vector2.op_Implicit(pPosition);
      this._last_pos_v3 = pPosition;
      this.m_transform.localPosition = this._last_pos_v3;
    }
    if ((double) this._last_scale_v2.y == (double) pScale.y && (double) this._last_scale_v2.x == (double) pScale.x)
      return;
    this._last_scale_v2 = pScale;
    this._last_scale_v3 = Vector2.op_Implicit(this._last_scale_v2);
    this.m_transform.localScale = Vector2.op_Implicit(pScale);
  }

  public void set(ref Vector3 pPosition, ref Vector3 pScale)
  {
    if ((double) this._last_pos_v3.x != (double) pPosition.x || (double) this._last_pos_v3.y != (double) pPosition.y)
    {
      this._last_pos_v2 = Vector2.op_Implicit(pPosition);
      this._last_pos_v3 = pPosition;
      this.m_transform.localPosition = this._last_pos_v3;
    }
    if ((double) this._last_scale_v2.y == (double) pScale.y && (double) this._last_scale_v2.x == (double) pScale.x)
      return;
    this._last_scale_v2 = Vector2.op_Implicit(pScale);
    this._last_scale_v3 = Vector2.op_Implicit(this._last_scale_v2);
    this.m_transform.localScale = pScale;
  }
}
