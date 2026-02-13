// Decompiled with JetBrains decompiler
// Type: SoundController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SoundController : MonoBehaviour
{
  public List<AudioClip> clips;
  public bool soundEnabled = true;
  public bool ambientSound;
  internal int curCopies;
  public int copies;
  public float randomizePitch;
  internal float timeout;
  public float timeoutInterval;
  public float originPitch = 1f;
  internal AudioSource s;
  private MoveCamera _camera;
  private float originVolume = 1f;
  private Vector3 sfxPos;
  private Vector3 sfxPosCamera;

  private void Awake()
  {
    this.s = ((Component) this).GetComponent<AudioSource>();
    this._camera = ((Component) Camera.main).GetComponent<MoveCamera>();
    this.originVolume = this.s.volume;
    if (!this.ambientSound)
      return;
    this.s.spatialBlend = 1f;
    this.s.dopplerLevel = 0.0f;
    this.s.rolloffMode = (AudioRolloffMode) 0;
  }

  internal void play(float pX = 0.0f, float pY = 0.0f)
  {
    float num = 1f;
    if (this.ambientSound)
    {
      this.sfxPos = new Vector3(pX, pY, 0.0f);
      this.sfxPosCamera = this._camera.main_camera.WorldToViewportPoint(this.sfxPos);
      if ((double) this.sfxPosCamera.x > 0.0 && (double) this.sfxPosCamera.x < 1.0 && (double) this.sfxPosCamera.y > 0.0)
      {
        double y = (double) this.sfxPosCamera.y;
      }
      if ((double) pX != 0.0 && (double) pY != 0.0)
        num = Mathf.Clamp01((float) (1.0 - (double) this._camera.main_camera.orthographicSize / (double) this._camera.orthographic_size_max * 0.699999988079071));
    }
    if (this.clips != null && this.clips.Count > 0)
      this.s.clip = Randy.getRandom<AudioClip>(this.clips);
    ((Component) this).gameObject.SetActive(true);
    this.s.volume = this.originVolume * num;
    this.s.pitch = this.originPitch + Randy.randomFloat(-this.randomizePitch, this.randomizePitch);
    ((Component) this.s).transform.position = new Vector3(pX, pY);
    this.s.Play();
  }

  internal void update(float pElapsed)
  {
    if ((double) this.timeout > 0.0)
      this.timeout -= pElapsed;
    if (!this.ambientSound)
      return;
    this.sfxPosCamera = this._camera.main_camera.WorldToViewportPoint(this.sfxPos);
    float num = 1f;
    if ((double) this.sfxPosCamera.x <= 0.0 || (double) this.sfxPosCamera.x >= 1.0 || (double) this.sfxPosCamera.y <= 0.0 || (double) this.sfxPosCamera.y >= 1.0)
      num = 0.0f;
    this.s.volume = this.originVolume * Mathf.Clamp01((float) (1.0 - (double) this._camera.main_camera.orthographicSize / (double) this._camera.orthographic_size_max * 0.699999988079071));
  }
}
