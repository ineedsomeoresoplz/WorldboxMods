// Decompiled with JetBrains decompiler
// Type: DynamicColorPixelTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public static class DynamicColorPixelTool
{
  private static bool _draw_phenotype;
  private static Color32 _phenotype_color;
  public static Color32 phenotype_shade_0;
  public static Color32 phenotype_shade_1;
  public static Color32 phenotype_shade_2;
  public static Color32 phenotype_shade_3;
  private static readonly Color32 _zombie_blood_color = Color32.op_Implicit(Toolbox.makeColor("#CE566E"));

  public static Color32 checkSpecialColors(
    Color32 pColor,
    ColorAsset pKingdomColor,
    bool pCheckForLightColors = false)
  {
    if (Config.EVERYTHING_MAGIC_COLOR)
      return Toolbox.EVERYTHING_MAGIC_COLOR32;
    if (pCheckForLightColors && Toolbox.areColorsEqual(pColor, Toolbox.color_light))
    {
      pColor = Toolbox.color_light_replace;
      return pColor;
    }
    if (pKingdomColor != null)
    {
      if (Toolbox.areColorsEqual(pColor, Toolbox.color_magenta_0))
        pColor = pKingdomColor.k_color_0;
      else if (Toolbox.areColorsEqual(pColor, Toolbox.color_magenta_1))
        pColor = pKingdomColor.k_color_1;
      else if (Toolbox.areColorsEqual(pColor, Toolbox.color_magenta_2))
        pColor = pKingdomColor.k_color_2;
      else if (Toolbox.areColorsEqual(pColor, Toolbox.color_magenta_3))
        pColor = pKingdomColor.k_color_3;
      else if (Toolbox.areColorsEqual(pColor, Toolbox.color_magenta_4))
        pColor = pKingdomColor.k_color_4;
      else if (Toolbox.areColorsEqual(pColor, Toolbox.color_teal_0))
        pColor = pKingdomColor.k2_color_0;
      else if (Toolbox.areColorsEqual(pColor, Toolbox.color_teal_1))
        pColor = pKingdomColor.k2_color_1;
      else if (Toolbox.areColorsEqual(pColor, Toolbox.color_teal_2))
        pColor = pKingdomColor.k2_color_2;
      else if (Toolbox.areColorsEqual(pColor, Toolbox.color_teal_3))
        pColor = pKingdomColor.k2_color_3;
      else if (Toolbox.areColorsEqual(pColor, Toolbox.color_teal_4))
        pColor = pKingdomColor.k2_color_4;
    }
    if (DynamicColorPixelTool._draw_phenotype)
    {
      if (Toolbox.areColorsEqual(pColor, Toolbox.color_phenotype_green_0))
        pColor = DynamicColorPixelTool.phenotype_shade_0;
      else if (Toolbox.areColorsEqual(pColor, Toolbox.color_phenotype_green_1))
        pColor = DynamicColorPixelTool.phenotype_shade_1;
      else if (Toolbox.areColorsEqual(pColor, Toolbox.color_phenotype_green_2))
        pColor = DynamicColorPixelTool.phenotype_shade_2;
      else if (Toolbox.areColorsEqual(pColor, Toolbox.color_phenotype_green_3))
        pColor = DynamicColorPixelTool.phenotype_shade_3;
    }
    return pColor;
  }

  public static Color32 checkZombieColors(ActorAsset pAsset, Color32 pColor, int pID, bool pHead = false)
  {
    Color32 pTargetBlendColor = Color32.op_Implicit(Toolbox.makeColor(pAsset.zombie_color_hex));
    return DynamicColorPixelTool.addNoiseAndBlood(DynamicColorPixelTool.multiplyBlend(pColor, pTargetBlendColor), pID);
  }

  private static Color32 addNoiseAndBlood(Color32 pTargetColor, int pID)
  {
    Random random = new Random(pID);
    if (random.NextDouble() < 0.5)
      return DynamicColorPixelTool.multiplyBlend(pTargetColor, DynamicColorPixelTool._zombie_blood_color, 0.2f);
    int num = random.Next(0, 20);
    return new Color32((byte) Mathf.Clamp((int) pTargetColor.r + num, 0, (int) byte.MaxValue), (byte) Mathf.Clamp((int) pTargetColor.g + num, 0, (int) byte.MaxValue), (byte) Mathf.Clamp((int) pTargetColor.b + num, 0, (int) byte.MaxValue), pTargetColor.a);
  }

  private static Color32 multiplyBlend(
    Color32 pBaseColor,
    Color32 pTargetBlendColor,
    float pIntensity = 1f)
  {
    double num1 = (double) pBaseColor.r / (double) byte.MaxValue;
    float num2 = (float) pBaseColor.g / (float) byte.MaxValue;
    float num3 = (float) pBaseColor.b / (float) byte.MaxValue;
    float num4 = Mathf.Lerp(1f, (float) pTargetBlendColor.r / (float) byte.MaxValue, pIntensity);
    float num5 = Mathf.Lerp(1f, (float) pTargetBlendColor.g / (float) byte.MaxValue, pIntensity);
    float num6 = Mathf.Lerp(1f, (float) pTargetBlendColor.b / (float) byte.MaxValue, pIntensity);
    double num7 = (double) num4;
    return new Color32((byte) ((double) Mathf.Clamp01((float) (num1 * num7)) * (double) byte.MaxValue), (byte) ((double) Mathf.Clamp01(num2 * num5) * (double) byte.MaxValue), (byte) ((double) Mathf.Clamp01(num3 * num6) * (double) byte.MaxValue), pBaseColor.a);
  }

  private static Color32 overlayBlend(Color32 pBaseColor, Color32 pTargetBlendColor)
  {
    float num1 = (float) pBaseColor.r / (float) byte.MaxValue;
    float num2 = (float) pBaseColor.g / (float) byte.MaxValue;
    float num3 = (float) pBaseColor.b / (float) byte.MaxValue;
    float num4 = (float) pTargetBlendColor.r / (float) byte.MaxValue;
    float num5 = (float) pTargetBlendColor.g / (float) byte.MaxValue;
    float num6 = (float) pTargetBlendColor.b / (float) byte.MaxValue;
    return new Color32((byte) (((double) num1 < 0.5 ? 2.0 * (double) num1 * (double) num4 : 1.0 - 2.0 * (1.0 - (double) num1) * (1.0 - (double) num4)) * (double) byte.MaxValue), (byte) ((double) ((double) num2 < 0.5 ? 2f * num2 * num5 : (float) (1.0 - 2.0 * (1.0 - (double) num2) * (1.0 - (double) num5))) * (double) byte.MaxValue), (byte) ((double) ((double) num3 < 0.5 ? 2f * num3 * num6 : (float) (1.0 - 2.0 * (1.0 - (double) num3) * (1.0 - (double) num6))) * (double) byte.MaxValue), pBaseColor.a);
  }

  public static void loadPhenotype(int pPhenotypeIndex, int pPhenotypeShadeIndex)
  {
    DynamicColorPixelTool.loadPhenotype(AssetManager.phenotype_library.getAssetByPhenotypeIndex(pPhenotypeIndex), pPhenotypeShadeIndex);
  }

  public static void loadPhenotype(PhenotypeAsset pPhenotypeAsset, int pPhenotypeShadeIndex)
  {
    DynamicColorPixelTool._phenotype_color = pPhenotypeAsset.colors[pPhenotypeShadeIndex];
    DynamicColorPixelTool._draw_phenotype = true;
    DynamicColorPixelTool.phenotype_shade_0 = Color32.op_Implicit(Toolbox.makeDarkerColor(Color32.op_Implicit(DynamicColorPixelTool._phenotype_color), 1f));
    DynamicColorPixelTool.phenotype_shade_1 = Color32.op_Implicit(Toolbox.makeDarkerColor(Color32.op_Implicit(DynamicColorPixelTool._phenotype_color), 0.9f));
    DynamicColorPixelTool.phenotype_shade_2 = Color32.op_Implicit(Toolbox.makeDarkerColor(Color32.op_Implicit(DynamicColorPixelTool._phenotype_color), 0.8f));
    DynamicColorPixelTool.phenotype_shade_3 = Color32.op_Implicit(Toolbox.makeDarkerColor(Color32.op_Implicit(DynamicColorPixelTool._phenotype_color), 0.7f));
  }

  public static void loadSkinColorsPreview(PhenotypeAsset pPhenotype, int pSkinColor)
  {
    DynamicColorPixelTool._draw_phenotype = true;
    DynamicColorPixelTool.phenotype_shade_0 = pPhenotype.colors[0];
    DynamicColorPixelTool.phenotype_shade_1 = pPhenotype.colors[1];
    DynamicColorPixelTool.phenotype_shade_2 = pPhenotype.colors[2];
    DynamicColorPixelTool.phenotype_shade_3 = pPhenotype.colors[3];
  }

  public static void resetSkinColors() => DynamicColorPixelTool._draw_phenotype = false;

  public static void setPlaceholderSkinColor(Color32 pColor)
  {
    DynamicColorPixelTool._phenotype_color = pColor;
  }
}
