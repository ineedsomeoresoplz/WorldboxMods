// Decompiled with JetBrains decompiler
// Type: AvatarEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class AvatarEffect : MonoBehaviour
{
  public Image image;
  private RectTransform _rect_transform;
  private Vector3 _initial_position;
  private StatusAsset _asset;
  private Actor _actor;
  private UnitAvatarLoader _avatar;
  private bool _animated;
  private float _time_between_frames;
  private float _elapsed;
  private int _current_frame;

  public void load(StatusAsset pAsset, Actor pActor, UnitAvatarLoader pAvatar)
  {
    this._asset = pAsset;
    this._actor = pActor;
    this._avatar = pAvatar;
    this._animated = pAsset.animated;
    this._rect_transform = ((Component) this).GetComponent<RectTransform>();
    int pIndex;
    if (!pAsset.animated)
    {
      pIndex = !pAsset.random_frame ? 0 : Randy.randomInt(0, pAsset.get_sprites_count((BaseSimObject) pActor, pAsset));
    }
    else
    {
      this._time_between_frames = pAsset.animation_speed + Randy.randomFloat(0.0f, pAsset.animation_speed_random);
      pIndex = 0;
    }
    ((Component) this.image).transform.localEulerAngles = this.getSpriteRotation(this._current_frame);
    this.image.sprite = this.getSprite(pIndex);
  }

  public void update(float pElapsed)
  {
    if (!this._animated)
      return;
    this._elapsed += pElapsed;
    if ((double) this._elapsed < (double) this._time_between_frames)
      return;
    this._elapsed = 0.0f;
    this._current_frame = Toolbox.loopIndex(this._current_frame + 1, this._asset.get_sprites_count((BaseSimObject) this._actor, this._asset));
    Sprite sprite = this.getSprite(this._current_frame);
    ((Component) this.image).transform.localPosition = Vector3.op_Addition(this._initial_position, this.getSpritePosition(this._current_frame));
    ((Component) this.image).transform.localEulerAngles = this.getSpriteRotation(this._current_frame);
    this.image.sprite = sprite;
  }

  private Sprite getSprite(int pIndex)
  {
    return !this._asset.has_override_sprite ? this._asset.sprite_list[pIndex] : this._asset.get_override_sprite_ui(this, pIndex);
  }

  private Vector3 getSpritePosition(int pIndex)
  {
    return !this._asset.has_override_sprite ? new Vector3() : this._asset.get_override_sprite_position_ui(this, pIndex);
  }

  private Vector3 getSpriteRotation(int pIndex)
  {
    return new Vector3()
    {
      z = !this._asset.has_override_sprite_rotation_z ? this._asset.rotation_z : this._asset.get_override_sprite_rotation_z_ui(this, pIndex)
    };
  }

  public void setInitialPosition(Vector2 pPosition)
  {
    this._initial_position = Vector2.op_Implicit(pPosition);
  }

  public RectTransform getRectTransform() => this._rect_transform;

  public UnitAvatarLoader getAvatar() => this._avatar;

  public StatusAsset getAsset() => this._asset;
}
