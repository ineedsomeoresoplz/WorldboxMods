// Decompiled with JetBrains decompiler
// Type: IconOutline
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class IconOutline : MonoBehaviour
{
  private static Dictionary<string, Sprite> _cached_textures = new Dictionary<string, Sprite>();
  private Image _image;
  public Image parent_image;

  private void Awake() => this.checkInit();

  private void checkInit()
  {
    if (Object.op_Inequality((Object) this._image, (Object) null))
      return;
    this._image = ((Component) this).GetComponent<Image>();
    ((Component) this).gameObject.AddComponent<FadeInOutAnimation>();
  }

  public void show(ContainerItemColor pContainer)
  {
    this.checkInit();
    ((Component) this).gameObject.SetActive(true);
    Color color = pContainer.color;
    color.a = 1f;
    ((Graphic) this._image).color = color;
    int hashCode = this.parent_image.sprite.texture.GetHashCode();
    string str1 = hashCode.ToString();
    hashCode = color.GetHashCode();
    string str2 = hashCode.ToString();
    string key = $"{str1}_{str2}";
    Sprite sprite;
    if (IconOutline._cached_textures.ContainsKey(key))
    {
      sprite = IconOutline._cached_textures[key];
    }
    else
    {
      sprite = this.generateSprite();
      IconOutline._cached_textures.Add(key, sprite);
    }
    this._image.sprite = sprite;
  }

  private Sprite generateSprite()
  {
    Texture2D pTexture = new Texture2D(((Texture) this.parent_image.sprite.texture).width, ((Texture) this.parent_image.sprite.texture).height);
    Color color;
    // ISSUE: explicit constructor call
    ((Color) ref color).\u002Ector(1f, 1f, 1f, 0.0f);
    for (int index1 = 0; index1 < ((Texture) pTexture).width; ++index1)
    {
      for (int index2 = 0; index2 < ((Texture) pTexture).height; ++index2)
        pTexture.SetPixel(index1, index2, color);
    }
    this.makePixels(-1, -1, pTexture);
    this.makePixels(1, 1, pTexture);
    this.makePixels(1, -1, pTexture);
    this.makePixels(-1, 1, pTexture);
    this.makePixels(1, 0, pTexture);
    this.makePixels(-1, 0, pTexture);
    this.makePixels(0, 1, pTexture);
    this.makePixels(0, -1, pTexture);
    pTexture.Apply();
    ((Texture) pTexture).filterMode = (FilterMode) 0;
    ((Object) pTexture).name = nameof (IconOutline);
    Rect rect;
    // ISSUE: explicit constructor call
    ((Rect) ref rect).\u002Ector(0.0f, 0.0f, (float) ((Texture) pTexture).width, (float) ((Texture) pTexture).height);
    Vector2 vector2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2).\u002Ector(0.5f, 0.5f);
    return Sprite.Create(pTexture, rect, vector2, 1f);
  }

  private void makePixels(int pOffsetX, int pOffsetY, Texture2D pTexture)
  {
    for (int index1 = 0; index1 < ((Texture) pTexture).width; ++index1)
    {
      for (int index2 = 0; index2 < ((Texture) pTexture).height; ++index2)
      {
        if ((double) this.parent_image.sprite.texture.GetPixel(index1, index2).a != 0.0)
        {
          int num1 = index1 + pOffsetX;
          int num2 = index2 + pOffsetY;
          if (num1 >= 0 && num1 <= ((Texture) pTexture).width && num2 >= 0 && num2 <= ((Texture) pTexture).height)
          {
            Color pixel = pTexture.GetPixel(num1, num2);
            pixel.a += 0.3f;
            pTexture.SetPixel(num1, num2, pixel);
          }
        }
      }
    }
  }
}
