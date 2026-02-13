// Decompiled with JetBrains decompiler
// Type: UnitSpriteConstructorAtlas
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UnitSpriteConstructorAtlas
{
  public UnitTextureAtlasID id;
  private bool _big_atlas;
  public Texture2D texture;
  public Color32[] pixels;
  public List<Texture2D> textures = new List<Texture2D>();
  public int last_x;
  public int last_y;
  private int _biggest_height;
  public bool dirty;

  public UnitSpriteConstructorAtlas(UnitTextureAtlasID pID, bool pBigAtlas)
  {
    this.id = pID;
    this._big_atlas = pBigAtlas;
  }

  public void setBigAtlas(bool pBigAtlas) => this._big_atlas = pBigAtlas;

  public bool isBigSpriteSheetAtlas() => this._big_atlas;

  public void newTexture(int pWidth, int pHeight, string tName)
  {
    if (!this._big_atlas)
    {
      pWidth += 2;
      pHeight += 10;
    }
    this.texture = new Texture2D(pWidth, pHeight);
    this.textures.Add(this.texture);
    ((Texture) this.texture).filterMode = (FilterMode) 0;
    ((Texture) this.texture).wrapMode = (TextureWrapMode) 1;
    ((Object) this.texture).name = tName;
    this.pixels = this.texture.GetPixels32();
    Color32 color32 = Color32.op_Implicit(Color.clear);
    for (int index = 0; index < this.pixels.Length; ++index)
      this.pixels[index] = color32;
    this.dirty = true;
    this.last_x = 0;
    this.last_y = 0;
    this._biggest_height = 0;
  }

  public void checkDirty()
  {
    if (!this.dirty)
      return;
    this.dirty = false;
    this.texture.SetPixels32(this.pixels);
    this.texture.Apply();
  }

  public string debug() => $"{this.textures.Count.ToString()} | {this.last_y.ToString()}";

  public void checkBounds(int pWidth, int pHeight)
  {
    if (!this._big_atlas)
    {
      this.newTexture(pWidth, pHeight, this.id.ToString() + "_small_atlas");
      this.last_x = 1;
      this.last_y = 1;
    }
    else
    {
      bool flag = false;
      if (this.textures.Count == 0)
        flag = true;
      if (pHeight > this._biggest_height)
        this._biggest_height = pHeight;
      int textureSize = DynamicSpritesConfig.texture_size;
      if (this.last_x + pWidth + 1 > textureSize)
      {
        this.last_x = 0;
        this.last_y += this._biggest_height + 1;
        if (this.last_y + this._biggest_height >= textureSize || this.last_y >= textureSize)
          flag = true;
        else
          this._biggest_height = pHeight;
      }
      else if (this.last_y + pHeight >= textureSize)
        flag = true;
      if (!flag)
        return;
      this.checkDirty();
      this.newTexture(textureSize, textureSize, this.id.ToString() + "_big_atlas");
      this._biggest_height = pHeight;
    }
  }

  public void clear()
  {
    foreach (Texture2D texture in this.textures)
    {
      if (Object.op_Inequality((Object) texture, (Object) null))
        Object.Destroy((Object) texture);
    }
    this.textures.Clear();
    this._biggest_height = 0;
    this.last_x = 0;
    this.last_y = 0;
  }
}
