// Decompiled with JetBrains decompiler
// Type: LavaRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
public class LavaRenderer : MonoBehaviour
{
  public Camera curCamera;
  public Camera targetCamera;
  private RenderTexture renderTexture;

  private void Start()
  {
    this.renderTexture = new RenderTexture(Screen.width, Screen.height, 8, (RenderTextureFormat) 0);
    ((Texture) this.renderTexture).dimension = (TextureDimension) 2;
    this.renderTexture.antiAliasing = 1;
    ((Texture) this.renderTexture).anisoLevel = 0;
    ((Texture) this.renderTexture).filterMode = (FilterMode) 0;
    this.renderTexture.Create();
    this.curCamera.targetTexture = this.renderTexture;
  }

  private void OnPreRender() => this.targetCamera.targetTexture = this.renderTexture;

  private void OnPostRender()
  {
    this.targetCamera.targetTexture = (RenderTexture) null;
    Graphics.DrawTexture(new Rect(0.0f, 0.0f, (float) (Screen.width / 2), (float) (Screen.height / 2)), (Texture) this.renderTexture, (Material) null);
  }
}
