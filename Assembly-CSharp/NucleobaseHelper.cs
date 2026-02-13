// Decompiled with JetBrains decompiler
// Type: NucleobaseHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class NucleobaseHelper
{
  private const char SHORT_ADENINE = 'A';
  private const char SHORT_CYTOSINE = 'C';
  private const char SHORT_GUANINE = 'G';
  private const char SHORT_THYMINE = 'T';
  private const string NOT_CONNECTED_TRANSPARENCY = "88";
  private const string COLOR_HEX_ADENINE = "#70FF70";
  private const string COLOR_HEX_CYTOSINE = "#3A8FFF";
  private const string COLOR_HEX_GUANINE = "#FFDF42";
  private const string COLOR_HEX_THYMINE = "#FF3A3A";
  private const string COLOR_HEX_ADENINE_DARK = "#70FF7088";
  private const string COLOR_HEX_CYTOSINE_DARK = "#3A8FFF88";
  private const string COLOR_HEX_GUANINE_DARK = "#FFDF4288";
  private const string COLOR_HEX_THYMINE_DARK = "#FF3A3A88";
  private static readonly Color color_adenine = Toolbox.makeColor("#70FF70");
  private static readonly Color color_cytosine = Toolbox.makeColor("#3A8FFF");
  private static readonly Color color_guanine = Toolbox.makeColor("#FFDF42");
  private static readonly Color color_thymine = Toolbox.makeColor("#FF3A3A");
  private static readonly Color color_adenine_dark = Toolbox.makeColor("#70FF7088");
  private static readonly Color color_cytosine_dark = Toolbox.makeColor("#3A8FFF88");
  private static readonly Color color_guanine_dark = Toolbox.makeColor("#FFDF4288");
  private static readonly Color color_thymine_dark = Toolbox.makeColor("#FF3A3A88");
  public static readonly Color color_bad = Color.black;
  public const string COLOR_HEX_BAD = "#B159FF";

  public static Color getColor(char pChar, bool pDark = false)
  {
    Color color;
    if (pDark)
    {
      switch (pChar)
      {
        case 'A':
          color = NucleobaseHelper.color_adenine_dark;
          break;
        case 'C':
          color = NucleobaseHelper.color_cytosine_dark;
          break;
        case 'G':
          color = NucleobaseHelper.color_guanine_dark;
          break;
        case 'T':
          color = NucleobaseHelper.color_thymine_dark;
          break;
        default:
          color = Color.black;
          break;
      }
    }
    else
    {
      switch (pChar)
      {
        case 'A':
          color = NucleobaseHelper.color_adenine;
          break;
        case 'C':
          color = NucleobaseHelper.color_cytosine;
          break;
        case 'G':
          color = NucleobaseHelper.color_guanine;
          break;
        case 'T':
          color = NucleobaseHelper.color_thymine;
          break;
        default:
          color = Color.black;
          break;
      }
    }
    return color;
  }

  public static string getColorHex(char pChar, bool pDark = false)
  {
    string colorHex = string.Empty;
    if (pDark)
    {
      switch (pChar)
      {
        case 'A':
          colorHex = "#70FF7088";
          break;
        case 'C':
          colorHex = "#3A8FFF88";
          break;
        case 'G':
          colorHex = "#FFDF4288";
          break;
        case 'T':
          colorHex = "#FF3A3A88";
          break;
      }
    }
    else
    {
      switch (pChar)
      {
        case 'A':
          colorHex = "#70FF70";
          break;
        case 'C':
          colorHex = "#3A8FFF";
          break;
        case 'G':
          colorHex = "#FFDF42";
          break;
        case 'T':
          colorHex = "#FF3A3A";
          break;
      }
    }
    return colorHex;
  }

  private static string getNucleobaseFullID(char pChar)
  {
    string nucleobaseFullId = string.Empty;
    switch (pChar)
    {
      case 'A':
        nucleobaseFullId = "nucleo_adenine";
        break;
      case 'C':
        nucleobaseFullId = "nucleo_cytosine";
        break;
      case 'G':
        nucleobaseFullId = "nucleo_guanine";
        break;
      case 'T':
        nucleobaseFullId = "nucleo_thymine";
        break;
    }
    return nucleobaseFullId;
  }

  public static string getColoredNucleobaseFull(char pChar)
  {
    return $"<color={NucleobaseHelper.getColorHex(pChar)}>{NucleobaseHelper.getFullNucleobaseName(pChar)}</color>";
  }

  public static string getFullNucleobaseName(char pChar)
  {
    return LocalizedTextManager.getText(NucleobaseHelper.getNucleobaseFullID(pChar));
  }

  public static string getColoredSequence(string pGeneticCode)
  {
    string empty = string.Empty;
    for (int index = 0; index < pGeneticCode.Length; ++index)
    {
      char pChar = pGeneticCode[index];
      string colorHex = NucleobaseHelper.getColorHex(pChar);
      empty += $"<color={colorHex}>{pChar}</color>";
    }
    return empty;
  }
}
