// Decompiled with JetBrains decompiler
// Type: BuildingMapIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BuildingMapIcon
{
  private BuildingColorPixel[][] _tex;
  private BuildingColorPixel _clear_color_pixel = new BuildingColorPixel(Toolbox.clear, Toolbox.clear, Toolbox.clear);
  private int _width;
  private int _height;

  public BuildingMapIcon(Sprite sprite)
  {
    this._width = ((Texture) sprite.texture).width;
    this._height = ((Texture) sprite.texture).height;
    this._tex = new BuildingColorPixel[this._height][];
    for (int index1 = 0; index1 < this._height; ++index1)
    {
      BuildingColorPixel[] buildingColorPixelArray = new BuildingColorPixel[this._width];
      for (int index2 = 0; index2 < this._width; ++index2)
      {
        Color32 pColor = Color32.op_Implicit(sprite.texture.GetPixel(index2, index1));
        if (pColor.a == (byte) 0)
        {
          buildingColorPixelArray[index2] = this._clear_color_pixel;
        }
        else
        {
          Color color1 = Toolbox.makeDarkerColor(Color32.op_Implicit(pColor), 0.9f);
          Color color2 = Toolbox.makeDarkerColor(Color32.op_Implicit(pColor), 0.6f);
          buildingColorPixelArray[index2] = new BuildingColorPixel(pColor, Color32.op_Implicit(color1), Color32.op_Implicit(color2));
        }
      }
      this._tex[index1] = buildingColorPixelArray;
    }
  }

  internal Color32 getColor(int pX, int pY, Building pBuilding)
  {
    if (pX >= this._width || pY >= this._height)
      return Toolbox.clear;
    BuildingColorPixel buildingColorPixel = this._tex[pY][pX];
    Color32 pC1 = buildingColorPixel.color;
    bool flag = false;
    ColorAsset color = pBuilding.kingdom.getColor();
    if (color != null)
    {
      if (Toolbox.areColorsEqual(pC1, Toolbox.color_magenta_0))
      {
        pC1 = color.k_color_0;
        flag = true;
      }
      else if (Toolbox.areColorsEqual(pC1, Toolbox.color_magenta_1))
      {
        pC1 = color.k_color_1;
        flag = true;
      }
      else if (Toolbox.areColorsEqual(pC1, Toolbox.color_magenta_2))
      {
        pC1 = color.k_color_2;
        flag = true;
      }
      else if (Toolbox.areColorsEqual(pC1, Toolbox.color_magenta_3))
      {
        pC1 = color.k_color_3;
        flag = true;
      }
      else if (Toolbox.areColorsEqual(pC1, Toolbox.color_magenta_4))
      {
        pC1 = color.k_color_4;
        flag = true;
      }
    }
    if (pBuilding.asset.has_get_map_icon_color && Toolbox.areColorsEqual(pC1, Toolbox.color_map_icon_green))
    {
      pC1 = pBuilding.asset.get_map_icon_color(pBuilding);
      flag = true;
    }
    if (!flag)
    {
      if (pBuilding.isAbandoned())
        pC1 = buildingColorPixel.color_abandoned;
      else if (pBuilding.isRuin())
        pC1 = buildingColorPixel.color_ruin;
    }
    return pC1;
  }
}
