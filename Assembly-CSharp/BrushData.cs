// Decompiled with JetBrains decompiler
// Type: BrushData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[Serializable]
public class BrushData : Asset, ILocalizedAsset
{
  [DefaultValue(1)]
  public int size = 1;
  [DefaultValue(1)]
  public int drops = 1;
  public BrushGroup group;
  public bool show_in_brush_window;
  public int width;
  public int height;
  public int sqr_size;
  public bool auto_size;
  public bool continuous;
  public bool fast_spawn;
  public string localized_key;
  public BrushPixelData[] pos;
  public BrushGenerateAction generate_action;
  public Vector2 ui_scale = new Vector2(1f, 1f);
  public Vector2 ui_size = new Vector2(28f, 28f);
  [NonSerialized]
  private Sprite _sprite;

  public void setupImage(Image pSprite)
  {
    pSprite.sprite = this.getSprite();
    Vector2 uiScale = this.ui_scale;
    Vector2 uiSize = this.ui_size;
    if (this.height < 28)
    {
      // ISSUE: explicit constructor call
      ((Vector2) ref uiSize).\u002Ector((float) this.width, (float) this.height);
    }
    ((Graphic) pSprite).rectTransform.sizeDelta = new Vector2(uiSize.x, uiSize.y);
    ((Component) pSprite).transform.localScale = new Vector3(uiScale.x, uiScale.y, 1f);
  }

  public Sprite getSprite()
  {
    if (Object.op_Inequality((Object) this._sprite, (Object) null))
      return this._sprite;
    Texture2D texture2D1 = new Texture2D(this.width, this.height, (TextureFormat) 4, false);
    ((Texture) texture2D1).filterMode = (FilterMode) 0;
    ((Texture) texture2D1).wrapMode = (TextureWrapMode) 1;
    Texture2D texture2D2 = texture2D1;
    Color[] colorArray = new Color[this.width * this.height];
    for (int index = 0; index < colorArray.Length; ++index)
      colorArray[index] = Color.clear;
    texture2D2.SetPixels(colorArray);
    Color white = Color.white;
    int num1 = 0;
    int num2 = 0;
    foreach (BrushPixelData po in this.pos)
    {
      if (po.x < num1)
        num1 = po.x;
      if (po.y < num2)
        num2 = po.y;
    }
    foreach (BrushPixelData po in this.pos)
      texture2D2.SetPixel(po.x - num1, po.y - num2, white);
    texture2D2.Apply(false, true);
    Rect rect;
    // ISSUE: explicit constructor call
    ((Rect) ref rect).\u002Ector(0.0f, 0.0f, (float) ((Texture) texture2D2).width, (float) ((Texture) texture2D2).height);
    Vector2 vector2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2).\u002Ector(0.0f, 0.0f);
    this._sprite = Sprite.Create(texture2D2, rect, vector2, 1f);
    ((Object) this._sprite).name = this.id;
    return this._sprite;
  }

  public string getLocaleID() => this.localized_key;
}
