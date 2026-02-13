// Decompiled with JetBrains decompiler
// Type: MapLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MapLayer : BaseMapObject
{
  public bool autoDisable;
  public bool autoDisableCheckPixels;
  public int textureID;
  protected float timer;
  protected Color colorValues;
  protected int colors_amount = 1;
  internal SpriteRenderer sprRnd;
  internal Texture2D texture;
  internal Color32[] pixels;
  internal HashSetWorldTile pixels_to_update;
  protected List<Color32> colors;
  internal HashSetWorldTile hashsetTiles;
  private int textureWidth;
  private int textureHeight;
  public bool rewriteSortingLayer = true;

  internal override void create()
  {
    base.create();
    this.pixels_to_update = new HashSetWorldTile();
    this.sprRnd = ((Component) this).gameObject.GetComponent<SpriteRenderer>();
    if (this.rewriteSortingLayer)
      ((Renderer) this.sprRnd).sortingLayerName = ((Renderer) ((Component) World.world).GetComponent<SpriteRenderer>()).sortingLayerName;
    this.colors = new List<Color32>();
    this.createColors();
  }

  protected virtual void checkAutoDisable()
  {
    if (!this.autoDisable)
      return;
    if (this.autoDisableCheckPixels)
    {
      if (this.pixels_to_update.Count > 0)
      {
        if (((Renderer) this.sprRnd).enabled)
          return;
        ((Renderer) this.sprRnd).enabled = true;
      }
      else
      {
        if (!((Renderer) this.sprRnd).enabled)
          return;
        ((Renderer) this.sprRnd).enabled = false;
      }
    }
    else if (this.hashsetTiles.Count > 0)
    {
      if (((Renderer) this.sprRnd).enabled)
        return;
      ((Renderer) this.sprRnd).enabled = true;
    }
    else
    {
      if (!((Renderer) this.sprRnd).enabled)
        return;
      ((Renderer) this.sprRnd).enabled = false;
    }
  }

  internal void createTextureNew()
  {
    if (!Object.op_Equality((Object) this.texture, (Object) null) && MapBox.width == this.textureWidth && MapBox.height == ((Texture) this.texture).height)
      return;
    if (Object.op_Inequality((Object) this.sprRnd.sprite, (Object) null) && this.textureWidth != 0)
      Texture2DStorage.addToStorage(this.sprRnd.sprite, this.textureWidth, this.textureHeight);
    this.textureWidth = MapBox.width;
    this.textureHeight = MapBox.height;
    this.sprRnd.sprite = Texture2DStorage.getSprite(this.textureWidth, this.textureHeight);
    this.texture = this.sprRnd.sprite.texture;
    this.textureID = this.texture.GetHashCode();
    int length = ((Texture) this.texture).height * ((Texture) this.texture).width;
    Color32 color32 = Color32.op_Implicit(Color.clear);
    this.pixels = new Color32[length];
    for (int index = 0; index < length; ++index)
      this.pixels[index] = color32;
    this.updatePixels();
  }

  public bool contains(WorldTile pTile) => this.pixels_to_update.Contains(pTile);

  internal virtual void clear()
  {
    if (this.pixels == null)
      return;
    this.pixels_to_update.Clear();
    Color32 color32 = Color32.op_Implicit(Color.clear);
    for (int index = 0; index < this.pixels.Length; ++index)
      this.pixels[index] = color32;
    this.updatePixels();
  }

  public void setRendererEnabled(bool pBool) => ((Renderer) this.sprRnd).enabled = pBool;

  protected void createColors()
  {
    for (int index = 0; index < this.colors_amount; ++index)
      this.colors.Add(Color32.op_Implicit(new Color(this.colorValues.r, this.colorValues.g, this.colorValues.b, (index <= 0 ? 0.0f : 1f / (float) this.colors_amount * (float) index) * this.colorValues.a)));
  }

  public override void update(float pElapsed) => this.checkAutoDisable();

  public virtual void draw(float pElapsed)
  {
    if (!((Renderer) this.sprRnd).enabled)
      return;
    this.UpdateDirty(pElapsed);
  }

  internal void updatePixels()
  {
    this.texture.SetPixels32(this.pixels);
    this.texture.Apply();
  }

  protected virtual void UpdateDirty(float pElapsed)
  {
  }
}
