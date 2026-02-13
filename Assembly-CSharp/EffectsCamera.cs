// Decompiled with JetBrains decompiler
// Type: EffectsCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EffectsCamera : MonoBehaviour
{
  private Camera _mainCamera;
  private Camera _effectsCamera;
  internal RenderTexture renderTexture;

  private void Awake() => this._effectsCamera = ((Component) this).GetComponent<Camera>();

  private void Start() => this._mainCamera = World.world.camera;

  private void LateUpdate()
  {
    this._effectsCamera.orthographicSize = this._mainCamera.orthographicSize;
    int num1 = Screen.width / 3;
    int num2 = Screen.height / 3;
    if (!Object.op_Equality((Object) this.renderTexture, (Object) null) && ((Texture) this.renderTexture).width == num1 && ((Texture) this.renderTexture).height == num2)
      return;
    this.renderTexture = new RenderTexture(num1, num2, 0);
    ((Texture) this.renderTexture).filterMode = (FilterMode) 0;
    ((Texture) this.renderTexture).wrapMode = (TextureWrapMode) 1;
    this._effectsCamera.targetTexture = this.renderTexture;
  }
}
