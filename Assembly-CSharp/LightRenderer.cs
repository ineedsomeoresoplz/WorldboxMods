// Decompiled with JetBrains decompiler
// Type: LightRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class LightRenderer : MonoBehaviour
{
  public static LightRenderer instance;
  public Camera camera;
  public EffectsCamera effectsCamera;
  private RawImage _rawImage;

  private void Awake()
  {
    LightRenderer.instance = this;
    this._rawImage = ((Component) this).GetComponent<RawImage>();
  }

  public void update(float pElapsed)
  {
    this._rawImage.texture = (Texture) this.effectsCamera.renderTexture;
    Color lightColor = World.world_era.light_color;
    lightColor.a = World.world.era_manager.getNightMod() * 0.6f;
    ((Graphic) this._rawImage).color = lightColor;
  }
}
