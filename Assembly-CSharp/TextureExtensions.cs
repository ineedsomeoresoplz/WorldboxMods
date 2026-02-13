// Decompiled with JetBrains decompiler
// Type: TextureExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class TextureExtensions
{
  public static Texture2D getAsReadable(this Texture2D pSourceTexture)
  {
    RenderTexture active = RenderTexture.active;
    RenderTexture temporary = RenderTexture.GetTemporary(((Texture) pSourceTexture).width, ((Texture) pSourceTexture).height, 0, (RenderTextureFormat) 7, ((Texture) pSourceTexture).isDataSRGB ? (RenderTextureReadWrite) 2 : (RenderTextureReadWrite) 1);
    Graphics.Blit((Texture) pSourceTexture, temporary);
    RenderTexture.active = temporary;
    Texture2D asReadable = new Texture2D(((Texture) pSourceTexture).width, ((Texture) pSourceTexture).height);
    asReadable.ReadPixels(new Rect(0.0f, 0.0f, (float) ((Texture) temporary).width, (float) ((Texture) temporary).height), 0, 0);
    asReadable.Apply();
    RenderTexture.active = active;
    RenderTexture.ReleaseTemporary(temporary);
    return asReadable;
  }
}
