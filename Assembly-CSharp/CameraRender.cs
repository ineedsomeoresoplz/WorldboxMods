// Decompiled with JetBrains decompiler
// Type: CameraRender
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CameraRender : MonoBehaviour
{
  public Material PostProcessMaterial;
  public Camera BackgroundCamera;
  public Camera MainCamera;
  private RenderTexture mainRenderTexture;

  private void Start()
  {
    this.mainRenderTexture = new RenderTexture(Screen.width, Screen.height, 16 /*0x10*/, (RenderTextureFormat) 0);
    this.mainRenderTexture.Create();
    this.BackgroundCamera.targetTexture = this.mainRenderTexture;
    this.MainCamera.targetTexture = this.mainRenderTexture;
  }

  private void Update()
  {
  }

  private void OnPostRender()
  {
    Graphics.Blit((Texture) this.mainRenderTexture, this.PostProcessMaterial);
  }
}
