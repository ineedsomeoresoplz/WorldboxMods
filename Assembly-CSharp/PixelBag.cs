// Decompiled with JetBrains decompiler
// Type: PixelBag
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PixelBag
{
  public readonly int texture_rect_width;
  public readonly int texture_rect_height;
  public readonly Pixel[] arr_pixels_normal;
  public readonly Pixel[] arr_pixels_light;
  public readonly Pixel[] arr_pixels_k1_0;
  public readonly Pixel[] arr_pixels_k1_1;
  public readonly Pixel[] arr_pixels_k1_2;
  public readonly Pixel[] arr_pixels_k1_3;
  public readonly Pixel[] arr_pixels_k1_4;
  public readonly Pixel[] arr_pixels_k2_0;
  public readonly Pixel[] arr_pixels_k2_1;
  public readonly Pixel[] arr_pixels_k2_2;
  public readonly Pixel[] arr_pixels_k2_3;
  public readonly Pixel[] arr_pixels_k2_4;
  public readonly Pixel[] arr_pixels_phenotype_shade_0;
  public readonly Pixel[] arr_pixels_phenotype_shade_1;
  public readonly Pixel[] arr_pixels_phenotype_shade_2;
  public readonly Pixel[] arr_pixels_phenotype_shade_3;
  private ListPool<Pixel> _pixels_normal;
  private ListPool<Pixel> _pixels_light;
  private ListPool<Pixel> _pixels_k1_0;
  private ListPool<Pixel> _pixels_k1_1;
  private ListPool<Pixel> _pixels_k1_2;
  private ListPool<Pixel> _pixels_k1_3;
  private ListPool<Pixel> _pixels_k1_4;
  private ListPool<Pixel> _pixels_k2_0;
  private ListPool<Pixel> _pixels_k2_1;
  private ListPool<Pixel> _pixels_k2_2;
  private ListPool<Pixel> _pixels_k2_3;
  private ListPool<Pixel> _pixels_k2_4;
  private ListPool<Pixel> _pixels_phenotype_shade_0;
  private ListPool<Pixel> _pixels_phenotype_shade_1;
  private ListPool<Pixel> _pixels_phenotype_shade_2;
  private ListPool<Pixel> _pixels_phenotype_shade_3;

  public PixelBag(Sprite pSpriteSource, bool pCheckPhenotypes, bool pCheckLights)
  {
    Texture2D texture = pSpriteSource.texture;
    Rect rect = pSpriteSource.rect;
    int width = ((Texture) texture).width;
    this.texture_rect_width = (int) ((Rect) ref rect).width;
    this.texture_rect_height = (int) ((Rect) ref rect).height;
    int x = (int) ((Rect) ref rect).x;
    int y = (int) ((Rect) ref rect).y;
    Color32[] pixels32 = texture.GetPixels32();
    for (int pX = 0; pX < this.texture_rect_width; ++pX)
    {
      for (int pY = 0; pY < this.texture_rect_height; ++pY)
      {
        int index = pX + x + (pY + y) * width;
        Color32 pColor = pixels32[index];
        if (pColor.a != (byte) 0)
          this.checkAndSavePixel(pColor, pX, pY, pCheckPhenotypes, pCheckLights);
      }
    }
    ListPool<Pixel> pixelsNormal = this._pixels_normal;
    this.arr_pixels_normal = pixelsNormal != null ? pixelsNormal.ToArray<Pixel>() : (Pixel[]) null;
    ListPool<Pixel> pixelsLight = this._pixels_light;
    this.arr_pixels_light = pixelsLight != null ? pixelsLight.ToArray<Pixel>() : (Pixel[]) null;
    ListPool<Pixel> pixelsK10 = this._pixels_k1_0;
    this.arr_pixels_k1_0 = pixelsK10 != null ? pixelsK10.ToArray<Pixel>() : (Pixel[]) null;
    ListPool<Pixel> pixelsK11 = this._pixels_k1_1;
    this.arr_pixels_k1_1 = pixelsK11 != null ? pixelsK11.ToArray<Pixel>() : (Pixel[]) null;
    ListPool<Pixel> pixelsK12 = this._pixels_k1_2;
    this.arr_pixels_k1_2 = pixelsK12 != null ? pixelsK12.ToArray<Pixel>() : (Pixel[]) null;
    ListPool<Pixel> pixelsK13 = this._pixels_k1_3;
    this.arr_pixels_k1_3 = pixelsK13 != null ? pixelsK13.ToArray<Pixel>() : (Pixel[]) null;
    ListPool<Pixel> pixelsK14 = this._pixels_k1_4;
    this.arr_pixels_k1_4 = pixelsK14 != null ? pixelsK14.ToArray<Pixel>() : (Pixel[]) null;
    ListPool<Pixel> pixelsK20 = this._pixels_k2_0;
    this.arr_pixels_k2_0 = pixelsK20 != null ? pixelsK20.ToArray<Pixel>() : (Pixel[]) null;
    ListPool<Pixel> pixelsK21 = this._pixels_k2_1;
    this.arr_pixels_k2_1 = pixelsK21 != null ? pixelsK21.ToArray<Pixel>() : (Pixel[]) null;
    ListPool<Pixel> pixelsK22 = this._pixels_k2_2;
    this.arr_pixels_k2_2 = pixelsK22 != null ? pixelsK22.ToArray<Pixel>() : (Pixel[]) null;
    ListPool<Pixel> pixelsK23 = this._pixels_k2_3;
    this.arr_pixels_k2_3 = pixelsK23 != null ? pixelsK23.ToArray<Pixel>() : (Pixel[]) null;
    ListPool<Pixel> pixelsK24 = this._pixels_k2_4;
    this.arr_pixels_k2_4 = pixelsK24 != null ? pixelsK24.ToArray<Pixel>() : (Pixel[]) null;
    ListPool<Pixel> pixelsPhenotypeShade0 = this._pixels_phenotype_shade_0;
    this.arr_pixels_phenotype_shade_0 = pixelsPhenotypeShade0 != null ? pixelsPhenotypeShade0.ToArray<Pixel>() : (Pixel[]) null;
    ListPool<Pixel> pixelsPhenotypeShade1 = this._pixels_phenotype_shade_1;
    this.arr_pixels_phenotype_shade_1 = pixelsPhenotypeShade1 != null ? pixelsPhenotypeShade1.ToArray<Pixel>() : (Pixel[]) null;
    ListPool<Pixel> pixelsPhenotypeShade2 = this._pixels_phenotype_shade_2;
    this.arr_pixels_phenotype_shade_2 = pixelsPhenotypeShade2 != null ? pixelsPhenotypeShade2.ToArray<Pixel>() : (Pixel[]) null;
    ListPool<Pixel> pixelsPhenotypeShade3 = this._pixels_phenotype_shade_3;
    this.arr_pixels_phenotype_shade_3 = pixelsPhenotypeShade3 != null ? pixelsPhenotypeShade3.ToArray<Pixel>() : (Pixel[]) null;
    this.clearLists();
  }

  private void clearLists()
  {
    this._pixels_normal?.Dispose();
    this._pixels_light?.Dispose();
    this._pixels_k1_0?.Dispose();
    this._pixels_k1_1?.Dispose();
    this._pixels_k1_2?.Dispose();
    this._pixels_k1_3?.Dispose();
    this._pixels_k1_4?.Dispose();
    this._pixels_k2_0?.Dispose();
    this._pixels_k2_1?.Dispose();
    this._pixels_k2_2?.Dispose();
    this._pixels_k2_3?.Dispose();
    this._pixels_k2_4?.Dispose();
    this._pixels_phenotype_shade_0?.Dispose();
    this._pixels_phenotype_shade_1?.Dispose();
    this._pixels_phenotype_shade_2?.Dispose();
    this._pixels_phenotype_shade_3?.Dispose();
    this._pixels_normal = (ListPool<Pixel>) null;
    this._pixels_light = (ListPool<Pixel>) null;
    this._pixels_k1_0 = (ListPool<Pixel>) null;
    this._pixels_k1_1 = (ListPool<Pixel>) null;
    this._pixels_k1_2 = (ListPool<Pixel>) null;
    this._pixels_k1_3 = (ListPool<Pixel>) null;
    this._pixels_k1_4 = (ListPool<Pixel>) null;
    this._pixels_k2_0 = (ListPool<Pixel>) null;
    this._pixels_k2_1 = (ListPool<Pixel>) null;
    this._pixels_k2_2 = (ListPool<Pixel>) null;
    this._pixels_k2_3 = (ListPool<Pixel>) null;
    this._pixels_k2_4 = (ListPool<Pixel>) null;
    this._pixels_phenotype_shade_0 = (ListPool<Pixel>) null;
    this._pixels_phenotype_shade_1 = (ListPool<Pixel>) null;
    this._pixels_phenotype_shade_2 = (ListPool<Pixel>) null;
    this._pixels_phenotype_shade_3 = (ListPool<Pixel>) null;
  }

  private void checkAndSavePixel(
    Color32 pColor,
    int pX,
    int pY,
    bool pCheckPhenotypes,
    bool pCheckLights)
  {
    Pixel pixel = new Pixel(pX, pY, pColor);
    if (Toolbox.areColorsEqual(pColor, Toolbox.color_magenta_0))
    {
      if (this._pixels_k1_0 == null)
        this._pixels_k1_0 = new ListPool<Pixel>();
      this._pixels_k1_0.Add(pixel);
    }
    else if (Toolbox.areColorsEqual(pColor, Toolbox.color_magenta_1))
    {
      if (this._pixels_k1_1 == null)
        this._pixels_k1_1 = new ListPool<Pixel>();
      this._pixels_k1_1.Add(pixel);
    }
    else if (Toolbox.areColorsEqual(pColor, Toolbox.color_magenta_2))
    {
      if (this._pixels_k1_2 == null)
        this._pixels_k1_2 = new ListPool<Pixel>();
      this._pixels_k1_2.Add(pixel);
    }
    else if (Toolbox.areColorsEqual(pColor, Toolbox.color_magenta_3))
    {
      if (this._pixels_k1_3 == null)
        this._pixels_k1_3 = new ListPool<Pixel>();
      this._pixels_k1_3.Add(pixel);
    }
    else if (Toolbox.areColorsEqual(pColor, Toolbox.color_magenta_4))
    {
      if (this._pixels_k1_4 == null)
        this._pixels_k1_4 = new ListPool<Pixel>();
      this._pixels_k1_4.Add(pixel);
    }
    else if (Toolbox.areColorsEqual(pColor, Toolbox.color_teal_0))
    {
      if (this._pixels_k2_0 == null)
        this._pixels_k2_0 = new ListPool<Pixel>();
      this._pixels_k2_0.Add(pixel);
    }
    else if (Toolbox.areColorsEqual(pColor, Toolbox.color_teal_1))
    {
      if (this._pixels_k2_1 == null)
        this._pixels_k2_1 = new ListPool<Pixel>();
      this._pixels_k2_1.Add(pixel);
    }
    else if (Toolbox.areColorsEqual(pColor, Toolbox.color_teal_2))
    {
      if (this._pixels_k2_2 == null)
        this._pixels_k2_2 = new ListPool<Pixel>();
      this._pixels_k2_2.Add(pixel);
    }
    else if (Toolbox.areColorsEqual(pColor, Toolbox.color_teal_3))
    {
      if (this._pixels_k2_3 == null)
        this._pixels_k2_3 = new ListPool<Pixel>();
      this._pixels_k2_3.Add(pixel);
    }
    else if (Toolbox.areColorsEqual(pColor, Toolbox.color_teal_4))
    {
      if (this._pixels_k2_4 == null)
        this._pixels_k2_4 = new ListPool<Pixel>();
      this._pixels_k2_4.Add(pixel);
    }
    else
    {
      if (pCheckPhenotypes)
      {
        if (Toolbox.areColorsEqual(pColor, Toolbox.color_phenotype_green_0))
        {
          if (this._pixels_phenotype_shade_0 == null)
            this._pixels_phenotype_shade_0 = new ListPool<Pixel>();
          this._pixels_phenotype_shade_0.Add(pixel);
          return;
        }
        if (Toolbox.areColorsEqual(pColor, Toolbox.color_phenotype_green_1))
        {
          if (this._pixels_phenotype_shade_1 == null)
            this._pixels_phenotype_shade_1 = new ListPool<Pixel>();
          this._pixels_phenotype_shade_1.Add(pixel);
          return;
        }
        if (Toolbox.areColorsEqual(pColor, Toolbox.color_phenotype_green_2))
        {
          if (this._pixels_phenotype_shade_2 == null)
            this._pixels_phenotype_shade_2 = new ListPool<Pixel>();
          this._pixels_phenotype_shade_2.Add(pixel);
          return;
        }
        if (Toolbox.areColorsEqual(pColor, Toolbox.color_phenotype_green_3))
        {
          if (this._pixels_phenotype_shade_3 == null)
            this._pixels_phenotype_shade_3 = new ListPool<Pixel>();
          this._pixels_phenotype_shade_3.Add(pixel);
          return;
        }
      }
      if (pCheckLights && Toolbox.areColorsEqual(pColor, Toolbox.color_light))
      {
        if (this._pixels_light == null)
          this._pixels_light = new ListPool<Pixel>();
        this._pixels_light.Add(pixel);
      }
      else
      {
        if (this._pixels_normal == null)
          this._pixels_normal = new ListPool<Pixel>();
        this._pixels_normal.Add(pixel);
      }
    }
  }
}
