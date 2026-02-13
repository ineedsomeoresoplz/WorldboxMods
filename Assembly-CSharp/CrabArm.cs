// Decompiled with JetBrains decompiler
// Type: CrabArm
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CrabArm : MonoBehaviour
{
  internal Crabzilla crabzilla;
  public SpriteRenderer laser;
  public Transform laserPoint;
  public GameObject joint;
  public List<Sprite> laserSprites;
  public bool mirrored;
  private const float LASER_INTERVAL = 0.07f;
  private float _laser_timer = 0.07f;
  private int _laser_frame_index;

  private void Start() => ((Renderer) this.laser).enabled = false;

  internal void update(float pElapsed)
  {
    Vector3 screenPoint1 = World.world.camera.WorldToScreenPoint(this.crabzilla.armTarget.transform.position);
    screenPoint1.z = 5.23f;
    Vector3 screenPoint2 = World.world.camera.WorldToScreenPoint(this.joint.transform.position);
    screenPoint1.x -= screenPoint2.x;
    screenPoint1.y -= screenPoint2.y;
    float num = (float) ((double) Mathf.Atan2(screenPoint1.y, screenPoint1.x) * 57.295780181884766 + 90.0);
    if (this.mirrored)
      num += 180f;
    this.joint.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, num));
    this.updateLaser(pElapsed);
    if (!this.crabzilla.isBeamEnabled())
      return;
    float x = ((Component) this.laserPoint).transform.position.x;
    float y = ((Component) this.laserPoint).transform.position.y;
    MusicBox.inst.playDrawingSound("event:/SFX/UNIQUE/Crabzilla/CrabzillaLazer", x, y);
    World.world.stack_effects.light_blobs.Add(new LightBlobData()
    {
      position = new Vector2(((Component) this.laser).transform.position.x, ((Component) this.laser).transform.position.y),
      radius = 1.5f
    });
    if (this._laser_frame_index <= 6 || this._laser_frame_index >= 10)
      return;
    this.damageWorld();
  }

  private void damageWorld()
  {
    WorldTile tile = World.world.GetTile((int) ((Component) this.laserPoint).transform.position.x, (int) ((Component) this.laserPoint).transform.position.y);
    if (tile == null)
      return;
    MapAction.damageWorld(tile, 4, AssetManager.terraform.get("crab_laser"));
  }

  private void updateLaser(float pTime)
  {
    this._laser_timer -= pTime;
    if (this.crabzilla.isBeamEnabled())
    {
      if ((double) this._laser_timer <= 0.0)
      {
        ++this._laser_frame_index;
        if (this._laser_frame_index >= 10)
          this._laser_frame_index = 6;
      }
    }
    else if (this._laser_frame_index != 0)
    {
      ++this._laser_frame_index;
      if (this._laser_frame_index > 13)
        this._laser_frame_index = 0;
    }
    if ((double) this._laser_timer <= 0.0)
      this._laser_timer = 0.07f;
    if (((Object) this.laser.sprite).name != ((Object) this.laserSprites[this._laser_frame_index]).name)
      this.laser.sprite = this.laserSprites[this._laser_frame_index];
    ((Renderer) this.laser).enabled = this._laser_frame_index != 0 || this.crabzilla.isBeamEnabled();
  }
}
