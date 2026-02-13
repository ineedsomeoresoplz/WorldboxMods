// Decompiled with JetBrains decompiler
// Type: TextureRotator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class TextureRotator
{
  public static Texture2D Rotate(Texture2D originTexture, int angle, Color32 pDefaultColor)
  {
    Texture2D texture2D = new Texture2D(((Texture) originTexture).width, ((Texture) originTexture).height);
    ((Object) texture2D).name = "rotated_" + ((Object) originTexture).name;
    Color32[] pixels32_1 = texture2D.GetPixels32();
    Color32[] pixels32_2 = originTexture.GetPixels32();
    int width = ((Texture) originTexture).width;
    int height = ((Texture) originTexture).height;
    int num1 = 0;
    int num2 = 0;
    double phi = Math.PI / 180.0 * (double) angle;
    Texture2D originTexture1 = originTexture;
    Color32 pDefaultColor1 = pDefaultColor;
    Color32[] color32Array = TextureRotator.rotateSquare(pixels32_2, phi, originTexture1, pDefaultColor1);
    for (int index1 = 0; index1 < height; ++index1)
    {
      for (int index2 = 0; index2 < width; ++index2)
        pixels32_1[((Texture) texture2D).width / 2 - width / 2 + num1 + index2 + ((Texture) texture2D).width * (((Texture) texture2D).height / 2 - height / 2 + index1 + num2)] = color32Array[index2 + index1 * width];
    }
    texture2D.SetPixels32(pixels32_1);
    texture2D.Apply();
    return texture2D;
  }

  private static Color32[] rotateSquare(
    Color32[] arr,
    double phi,
    Texture2D originTexture,
    Color32 pDefaultColor)
  {
    double num1 = Math.Sin(phi);
    double num2 = Math.Cos(phi);
    Color32[] pixels32 = originTexture.GetPixels32();
    int width = ((Texture) originTexture).width;
    int height = ((Texture) originTexture).height;
    int num3 = width / 2;
    int num4 = height / 2;
    for (int index1 = 0; index1 < height; ++index1)
    {
      for (int index2 = 0; index2 < width; ++index2)
      {
        pixels32[index1 * width + index2] = pDefaultColor;
        int num5 = (int) (num2 * (double) (index2 - num3) + num1 * (double) (index1 - num4) + (double) num3);
        int num6 = (int) (-num1 * (double) (index2 - num3) + num2 * (double) (index1 - num4) + (double) num4);
        if (num5 > -1 && num5 < width && num6 > -1 && num6 < height)
          pixels32[index1 * width + index2] = arr[num6 * width + num5];
      }
    }
    return pixels32;
  }
}
