// Decompiled with JetBrains decompiler
// Type: Toolbox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityPools;

#nullable disable
public static class Toolbox
{
  public static readonly ActorDirection[] directions = new ActorDirection[4]
  {
    ActorDirection.Up,
    ActorDirection.Right,
    ActorDirection.Down,
    ActorDirection.Left
  };
  public static readonly ActorDirection[] directions_all = new ActorDirection[8]
  {
    ActorDirection.Up,
    ActorDirection.UpRight,
    ActorDirection.UpLeft,
    ActorDirection.Right,
    ActorDirection.DownRight,
    ActorDirection.DownLeft,
    ActorDirection.Down,
    ActorDirection.Left
  };
  public static readonly Dictionary<ActorDirection, ActorDirection[]> directions_turns = new Dictionary<ActorDirection, ActorDirection[]>()
  {
    {
      ActorDirection.Up,
      new ActorDirection[2]
      {
        ActorDirection.Right,
        ActorDirection.Left
      }
    },
    {
      ActorDirection.Right,
      new ActorDirection[2]
      {
        ActorDirection.Down,
        ActorDirection.Up
      }
    },
    {
      ActorDirection.Down,
      new ActorDirection[2]
      {
        ActorDirection.Left,
        ActorDirection.Right
      }
    },
    {
      ActorDirection.Left,
      new ActorDirection[2]
      {
        ActorDirection.Up,
        ActorDirection.Down
      }
    }
  };
  public static readonly Dictionary<ActorDirection, ActorDirection[]> directions_all_turns = new Dictionary<ActorDirection, ActorDirection[]>()
  {
    {
      ActorDirection.Up,
      new ActorDirection[4]
      {
        ActorDirection.Right,
        ActorDirection.UpRight,
        ActorDirection.UpLeft,
        ActorDirection.Left
      }
    },
    {
      ActorDirection.UpRight,
      new ActorDirection[4]
      {
        ActorDirection.DownRight,
        ActorDirection.Right,
        ActorDirection.Up,
        ActorDirection.UpLeft
      }
    },
    {
      ActorDirection.Right,
      new ActorDirection[4]
      {
        ActorDirection.Down,
        ActorDirection.DownRight,
        ActorDirection.UpRight,
        ActorDirection.Up
      }
    },
    {
      ActorDirection.DownRight,
      new ActorDirection[4]
      {
        ActorDirection.DownLeft,
        ActorDirection.Down,
        ActorDirection.Right,
        ActorDirection.UpRight
      }
    },
    {
      ActorDirection.Down,
      new ActorDirection[4]
      {
        ActorDirection.Left,
        ActorDirection.DownLeft,
        ActorDirection.DownRight,
        ActorDirection.Right
      }
    },
    {
      ActorDirection.DownLeft,
      new ActorDirection[4]
      {
        ActorDirection.UpLeft,
        ActorDirection.Left,
        ActorDirection.Down,
        ActorDirection.DownRight
      }
    },
    {
      ActorDirection.Left,
      new ActorDirection[4]
      {
        ActorDirection.Up,
        ActorDirection.UpLeft,
        ActorDirection.DownLeft,
        ActorDirection.Down
      }
    },
    {
      ActorDirection.UpLeft,
      new ActorDirection[4]
      {
        ActorDirection.UpRight,
        ActorDirection.Up,
        ActorDirection.Left,
        ActorDirection.DownLeft
      }
    }
  };
  public static readonly Color32 EVERYTHING_MAGIC_COLOR32 = Color32.op_Implicit(Toolbox.makeColor("#DF7FFF"));
  public static readonly Color32 color_grey_dark = Color32.op_Implicit(Toolbox.makeColor("#5D5D5D"));
  public static readonly Color32 color_grey = Color32.op_Implicit(Toolbox.makeColor("#AAAAAA"));
  public static readonly Color32 color_transparent_grey = Color32.op_Implicit(Toolbox.makeColor("#666666", 0.5f));
  public static readonly Color32 color_debug_bar_blue = Color32.op_Implicit(Toolbox.makeColor("#0092FF", 0.5f));
  public static readonly Color32 color_debug_bar_red = Color32.op_Implicit(Toolbox.makeColor("#FF6262", 0.5f));
  public static readonly Color32 color_phenotype_green_0 = Color32.op_Implicit(Toolbox.makeColor("#B8FF96"));
  public static readonly Color32 color_phenotype_green_1 = Color32.op_Implicit(Toolbox.makeColor("#00FF00"));
  public static readonly Color32 color_phenotype_green_2 = Color32.op_Implicit(Toolbox.makeColor("#00AF00"));
  public static readonly Color32 color_phenotype_green_3 = Color32.op_Implicit(Toolbox.makeColor("#4A831F"));
  public static readonly Color32 color_map_icon_green = Color32.op_Implicit(Toolbox.makeColor("#00FF00"));
  public static readonly Color32 color_magenta_0 = Color32.op_Implicit(Toolbox.makeColor("#FF00FF"));
  public static readonly Color32 color_magenta_1 = Color32.op_Implicit(Toolbox.makeColor("#DE00DE"));
  public static readonly Color32 color_magenta_2 = Color32.op_Implicit(Toolbox.makeColor("#A700A7"));
  public static readonly Color32 color_magenta_3 = Color32.op_Implicit(Toolbox.makeColor("#7F007F"));
  public static readonly Color32 color_magenta_4 = Color32.op_Implicit(Toolbox.makeColor("#580058"));
  public static readonly Color32 color_teal_0 = Color32.op_Implicit(Toolbox.makeColor("#00EFEF"));
  public static readonly Color32 color_teal_1 = Color32.op_Implicit(Toolbox.makeColor("#00DBDB"));
  public static readonly Color32 color_teal_2 = Color32.op_Implicit(Toolbox.makeColor("#00BCBC"));
  public static readonly Color32 color_teal_3 = Color32.op_Implicit(Toolbox.makeColor("#009E9E"));
  public static readonly Color32 color_teal_4 = Color32.op_Implicit(Toolbox.makeColor("#007777"));
  public static readonly Color32 color_ocean = Color32.op_Implicit(Toolbox.makeColor("#3370CC"));
  public static readonly Color32 color_night = Color32.op_Implicit(Toolbox.makeColor("#05003F"));
  public static readonly Color32 color_light = Color32.op_Implicit(Toolbox.makeColor("#FFD800"));
  public static readonly Color32 color_light_100 = Color32.op_Implicit(Toolbox.makeColor("#FFFFFF"));
  public static readonly Color32 color_light_10 = Color32.op_Implicit(Toolbox.makeColor("#FFFFFF", 0.3f));
  public static readonly Color32 color_light_replace = Color32.op_Implicit(Toolbox.makeColor("#000000"));
  public static Color color_augmentation_selected = Color.white;
  public static Color color_augmentation_unselected = new Color(0.7f, 0.7f, 0.7f, 1f);
  public static readonly Color32 color_clear = Color32.op_Implicit(Color.clear);
  public static Color color_white = Color.white;
  public static Color color_gray = Color.gray;
  public static Color color_black = Color.black;
  public static Color32 color_black_32 = Color32.op_Implicit(Color.black);
  public static readonly Color32 color_white_32 = Color32.op_Implicit(Color.white);
  public static Color color_red = Color.red;
  public static Color color_yellow = Color.yellow;
  public static Color color_blue = Color.blue;
  public static Color color_green = Color.green;
  public static Color color_purple = new Color(0.5f, 0.0f, 0.5f);
  public static Color color_cyan = Color.cyan;
  public static Color color_cursed = new Color(1f, 0.0f, 0.8352941f);
  public static Color color_abandoned_building = new Color(0.8f, 0.8f, 0.8f);
  public const string color_positive = "#43FF43";
  public const string color_negative = "#FB2C21";
  public const string color_positive_light = "#95DD5D";
  public const string color_negative_light = "#FF8686";
  public static readonly Color color_positive_RGBA = Toolbox.makeColor("#43FF43");
  public static readonly Color color_negative_RGBA = Toolbox.makeColor("#FB2C21");
  public const string color_report_positive = "#ADADAD";
  public const string color_report_negative = "#919191";
  public const string color_hex_white = "#FFFFFF";
  public const string color_hex_black = "#000000";
  public const string color_hex_neutral = "#F3961F";
  public const string color_hex_brighter = "#FFBC66";
  public const string color_tooltip_hotkey = "#95DD5D";
  public static readonly Color32 clear = Color32.op_Implicit(Color.clear);
  public static readonly Color32 edge_alpha = Color32.op_Implicit(Toolbox.makeColor("#000000", 0.1f));
  public static readonly Color color_white_transparent = Toolbox.makeColor("#FFFFFF", 0.0f);
  public static readonly Color color_text_default = Toolbox.makeColor("#FF9B1C");
  public static readonly Color color_text_default_bright = Toolbox.makeColor("#FFBC66");
  public static readonly Color color_log_good = Toolbox.makeColor("#95DD5D");
  public static readonly Color color_log_warning = Toolbox.makeColor("#FF8686");
  public static readonly Color color_log_neutral = Toolbox.makeColor("#F3961F");
  public static readonly Color32 color_fire = Color32.op_Implicit(Toolbox.makeColor("#FF6930"));
  public const string color_hex_ocean = "#3370CC";
  public const string color_hex_blue = "#4CCFFF";
  public const string color_hex_red = "#FF637D";
  public const string color_hex_green = "#43FF43";
  public const string color_hex_purple = "#E060CD";
  public const string color_hex_yellow = "#FFFF51";
  public const string color_hex_heal = "#23F3FF";
  public const string color_hex_plague = "#CE4A9B";
  public const string color_hex_mush_spores = "#8CFF99";
  public const string color_hex_infected = "#35CC6E";
  public const string color_hex_poisoned = "#D85BC5";
  public static Color color_heal = Toolbox.makeColor("#23F3FF");
  public static Color color_plague = Toolbox.makeColor("#CE4A9B");
  public static Color color_mushSpores = Toolbox.makeColor("#8CFF99");
  public static Color color_infected = Toolbox.makeColor("#35CC6E");
  public static Color color_poisoned = Toolbox.makeColor("#D85BC5");
  public static readonly Color[] colors_fire = new Color[10]
  {
    Toolbox.makeColor("#D95032"),
    Toolbox.makeColor("#F27F3D"),
    Toolbox.makeColor("#F2A444"),
    Toolbox.makeColor("#F2C36B"),
    Toolbox.makeColor("#F2CA50"),
    Toolbox.makeColor("#E35632"),
    Toolbox.makeColor("#EEB543"),
    Color.red,
    Color.yellow,
    Color.white
  };
  public static readonly Color[] colors_wheat = new Color[5]
  {
    Toolbox.makeColor("#20B22B"),
    Toolbox.makeColor("#2A8E31"),
    Toolbox.makeColor("#20B22B"),
    Toolbox.makeColor("#74A926"),
    Toolbox.makeColor("#FFEB93")
  };
  internal static readonly List<WorldTile> temp_list_tiles = new List<WorldTile>();
  private static readonly MapChunk[] _temp_array_chunks = new MapChunk[9];
  private static readonly TileZone[] _temp_array_zones = new TileZone[9];

  public static void fromStringListToHashset(List<string> pList, HashSet<string> pHashset)
  {
    foreach (string p in pList)
      pHashset.Add(p);
  }

  public static string coloredText(string pText, string pColor, bool pLocalize = false)
  {
    if (pLocalize)
      pText = LocalizedTextManager.getText(pText);
    return $"<color={pColor}>{pText}</color>";
  }

  public static string coloredGreyPart(object pPart, string pMainColor, bool pUnit = false)
  {
    string empty = string.Empty;
    return !pUnit ? empty + Toolbox.coloredString(" [", ColorStyleLibrary.m.color_dead_text) + Toolbox.coloredString(pPart.ToString(), pMainColor) + Toolbox.coloredString("]", ColorStyleLibrary.m.color_dead_text) : empty + Toolbox.coloredString(" (", ColorStyleLibrary.m.color_dead_text) + Toolbox.coloredString(pPart.ToString(), pMainColor) + Toolbox.coloredString(")", ColorStyleLibrary.m.color_dead_text);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool areColorsEqual(Color32 pC1, Color32 pC2)
  {
    return (int) pC1.r == (int) pC2.r && (int) pC1.g == (int) pC2.g && (int) pC1.b == (int) pC2.b;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool inBounds(float pVal, float pMin, float pMax)
  {
    return (double) pVal > (double) pMin && (double) pVal < (double) pMax;
  }

  public static string firstLetterToUpper(string str)
  {
    if (str == null)
      return (string) null;
    return str.Length > 1 ? char.ToUpper(str[0]).ToString() + str.Substring(1) : str.ToUpper();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int loopIndex(int pIndex, int pLength)
  {
    return pLength < 1 ? 0 : (pIndex % pLength + pLength) % pLength;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 RotatePointAroundPivot(
    ref Vector3 point,
    ref Vector3 pivot,
    ref Vector3 angles)
  {
    return Vector3.op_Addition(Quaternion.op_Multiply(Quaternion.Euler(angles), Vector3.op_Subtraction(point, pivot)), pivot);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 RotatePointAroundPivot2(
    ref Vector3 point,
    ref Vector3 pivot,
    ref Vector3 angles)
  {
    Vector3 vector3_1 = Vector3.op_Subtraction(point, pivot);
    Vector3 vector3_2 = Quaternion.op_Multiply(Quaternion.Euler(angles), vector3_1);
    point = Vector3.op_Addition(vector3_2, pivot);
    return point;
  }

  public static Vector2 rotateVector(Vector2 pVector, float degrees)
  {
    double num1 = (double) degrees * (Math.PI / 180.0);
    float num2 = Mathf.Sin((float) num1);
    float num3 = Mathf.Cos((float) num1);
    float x = pVector.x;
    float y = pVector.y;
    return new Vector2((float) ((double) num3 * (double) x - (double) num2 * (double) y), (float) ((double) num2 * (double) x + (double) num3 * (double) y));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 cubeBezier3(
    ref Vector3 p0,
    ref Vector3 p1,
    ref Vector3 p2,
    ref Vector3 p3,
    float t)
  {
    float num1 = 1f - t;
    float num2 = num1 * num1 * num1;
    float num3 = (float) ((double) num1 * (double) num1 * (double) t * 3.0);
    float num4 = (float) ((double) num1 * (double) t * (double) t * 3.0);
    float num5 = t * t * t;
    return new Vector3((float) ((double) num2 * (double) p0.x + (double) num3 * (double) p1.x + (double) num4 * (double) p2.x + (double) num5 * (double) p3.x), (float) ((double) num2 * (double) p0.y + (double) num3 * (double) p1.y + (double) num4 * (double) p2.y + (double) num5 * (double) p3.y), (float) ((double) num2 * (double) p0.z + (double) num3 * (double) p1.z + (double) num4 * (double) p2.z + (double) num5 * (double) p3.z));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector2 cubeBezier2(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
  {
    float num1 = 1f - t;
    float num2 = num1 * num1 * num1;
    float num3 = (float) ((double) num1 * (double) num1 * (double) t * 3.0);
    float num4 = (float) ((double) num1 * (double) t * (double) t * 3.0);
    float num5 = t * t * t;
    return new Vector2((float) ((double) num2 * (double) p0.x + (double) num3 * (double) p1.x + (double) num4 * (double) p2.x + (double) num5 * (double) p3.x), (float) ((double) num2 * (double) p0.y + (double) num3 * (double) p1.y + (double) num4 * (double) p2.y + (double) num5 * (double) p3.y));
  }

  public static Vector2 cubeBezierN(float pTick, params Vector3[] pPoints)
  {
    if (pPoints.Length > 2)
    {
      Vector3[] vector3Array = new Vector3[pPoints.Length - 1];
      for (int index = 0; index < pPoints.Length - 1; ++index)
        vector3Array[index] = Vector2.op_Implicit(Vector2.Lerp(Vector2.op_Implicit(pPoints[index]), Vector2.op_Implicit(pPoints[index + 1]), pTick));
      return Toolbox.cubeBezierN(pTick, vector3Array);
    }
    return pPoints.Length == 2 ? Vector2.Lerp(Vector2.op_Implicit(pPoints[0]), Vector2.op_Implicit(pPoints[1]), pTick) : Vector2.op_Implicit(pPoints[0]);
  }

  public static string encode(string pString)
  {
    string str = "WorldboxIsAwesome";
    pString = Encryption.EncryptString(pString, str + "555");
    return pString;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float easeInOutQuart(float x)
  {
    return (double) x < 0.5 ? 8f * x * x * x * x : (float) (1.0 - Math.Pow(-2.0 * (double) x + 2.0, 4.0) / 2.0);
  }

  public static string decode(string pString)
  {
    string str = "WorldboxIsAwesome";
    pString = Encryption.DecryptString(pString, str + "555");
    return pString;
  }

  public static string decodeMobile(string pString)
  {
    string uniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
    string str = "WorldboxIsAwesome";
    pString = Encryption.DecryptString(pString, $"{str}555{uniqueIdentifier}");
    return pString;
  }

  public static string generateID_old() => Toolbox.shortGUID(Guid.NewGuid());

  public static string shortGUID(Guid guid)
  {
    return Convert.ToBase64String(guid.ToByteArray()).Replace('+', '-').Replace('/', '_').Substring(0, 8);
  }

  public static Vector3 getNewPoint(
    float pX1,
    float pY1,
    float pX2,
    float pY2,
    float pDist,
    bool pConvertNegative = true)
  {
    Vector3 newPoint = new Vector3();
    float num1 = Toolbox.Dist(pX1, pY1, pX2, pY2) - pDist;
    if ((double) num1 == 0.0)
    {
      ((Vector3) ref newPoint).Set(pX2, pY2, 0.0f);
      return newPoint;
    }
    float num2 = pDist / num1;
    if (pConvertNegative && (double) num2 < 0.0)
      num2 = -num2;
    float num3 = (float) (((double) pX1 + (double) num2 * (double) pX2) / (1.0 + (double) num2));
    float num4 = (float) (((double) pY1 + (double) num2 * (double) pY2) / (1.0 + (double) num2));
    newPoint.x = num3;
    newPoint.y = num4;
    return newPoint;
  }

  public static Vector2 getNewPointVec2(
    Vector2 pVec1,
    Vector2 pVec2,
    float pDist,
    bool pConvertNegative = true)
  {
    return Toolbox.getNewPointVec2(pVec1.x, pVec1.y, pVec2.x, pVec2.y, pDist, pConvertNegative);
  }

  public static Vector2 getNewPointVec2(
    float pX1,
    float pY1,
    float pX2,
    float pY2,
    float pDist,
    bool pConvertNegative = true)
  {
    float num1 = Toolbox.Dist(pX1, pY1, pX2, pY2) - pDist;
    if ((double) num1 == 0.0)
      return new Vector2(pX2, pY2);
    float num2 = pDist / num1;
    if (pConvertNegative && (double) num2 < 0.0)
      num2 = -num2;
    return new Vector2((float) (((double) pX1 + (double) num2 * (double) pX2) / (1.0 + (double) num2)), (float) (((double) pY1 + (double) num2 * (double) pY2) / (1.0 + (double) num2)));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float DistVec3(Vector3 pT1, Vector3 pT2)
  {
    return Mathf.Sqrt((float) (((double) pT1.x - (double) pT2.x) * ((double) pT1.x - (double) pT2.x) + ((double) pT1.y - (double) pT2.y) * ((double) pT1.y - (double) pT2.y)));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float DistVec2(Vector2Int pT1, Vector2Int pT2)
  {
    return Mathf.Sqrt((float) ((((Vector2Int) ref pT1).x - ((Vector2Int) ref pT2).x) * (((Vector2Int) ref pT1).x - ((Vector2Int) ref pT2).x) + (((Vector2Int) ref pT1).y - ((Vector2Int) ref pT2).y) * (((Vector2Int) ref pT1).y - ((Vector2Int) ref pT2).y)));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float DistVec2Float(Vector2 pT1, Vector2 pT2)
  {
    return Mathf.Sqrt((float) (((double) pT1.x - (double) pT2.x) * ((double) pT1.x - (double) pT2.x) + ((double) pT1.y - (double) pT2.y) * ((double) pT1.y - (double) pT2.y)));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float DistTile(WorldTile pT1, WorldTile pT2)
  {
    return Mathf.Sqrt((float) ((pT1.x - pT2.x) * (pT1.x - pT2.x) + (pT1.y - pT2.y) * (pT1.y - pT2.y)));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float Dist(float x1, float y1, float x2, float y2)
  {
    return Mathf.Sqrt((float) (((double) x1 - (double) x2) * ((double) x1 - (double) x2) + ((double) y1 - (double) y2) * ((double) y1 - (double) y2)));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float Dist(int x1, int y1, int x2, int y2)
  {
    return Mathf.Sqrt((float) ((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float SquaredDist(float x1, float y1, float x2, float y2)
  {
    return (float) (((double) x1 - (double) x2) * ((double) x1 - (double) x2) + ((double) y1 - (double) y2) * ((double) y1 - (double) y2));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int SquaredDist(int x1, int y1, int x2, int y2)
  {
    return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int SquaredDistTile(WorldTile pT1, WorldTile pT2)
  {
    return (pT1.x - pT2.x) * (pT1.x - pT2.x) + (pT1.y - pT2.y) * (pT1.y - pT2.y);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float SquaredDistVec2Float(Vector2 pT1, Vector2 pT2)
  {
    return (float) (((double) pT1.x - (double) pT2.x) * ((double) pT1.x - (double) pT2.x) + ((double) pT1.y - (double) pT2.y) * ((double) pT1.y - (double) pT2.y));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int SquaredDistVec2(Vector2Int pT1, Vector2Int pT2)
  {
    return (((Vector2Int) ref pT1).x - ((Vector2Int) ref pT2).x) * (((Vector2Int) ref pT1).x - ((Vector2Int) ref pT2).x) + (((Vector2Int) ref pT1).y - ((Vector2Int) ref pT2).y) * (((Vector2Int) ref pT1).y - ((Vector2Int) ref pT2).y);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float SquaredDistVec3(Vector3 pT1, Vector3 pT2)
  {
    return (float) (((double) pT1.x - (double) pT2.x) * ((double) pT1.x - (double) pT2.x) + ((double) pT1.y - (double) pT2.y) * ((double) pT1.y - (double) pT2.y));
  }

  public static Color makeColor(string pHex)
  {
    Color color;
    ColorUtility.TryParseHtmlString(pHex, ref color);
    return color;
  }

  public static Color makeColor(string pHex, float pAlpha)
  {
    Color color;
    ColorUtility.TryParseHtmlString(pHex, ref color);
    color.a = pAlpha;
    return color;
  }

  public static string colorToHex(Color32 pColor, bool pAlpha = true)
  {
    return pAlpha ? "#" + ColorUtility.ToHtmlStringRGBA(Color32.op_Implicit(pColor)) : "#" + ColorUtility.ToHtmlStringRGB(Color32.op_Implicit(pColor));
  }

  public static string coloredString(string pText, string pColor)
  {
    if (string.IsNullOrEmpty(pColor))
      return pText;
    return $"<color={pColor}>{pText}</color>";
  }

  public static string colorBetween(
    double pValue,
    double pMin,
    double pMax,
    string pMinColor = "#FB2C21",
    string pMaxColor = "#43FF43")
  {
    float num = 100f;
    if (pMax - pMin != 0.0)
      num = (float) (pValue - pMin) / (float) (pMax - pMin);
    return Toolbox.colorToHex(Color32.op_Implicit(Color.Lerp(Toolbox.makeColor(pMinColor), Toolbox.makeColor(pMaxColor), num)));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float getAngle(float pX1, float pY1, float pX2, float pY2)
  {
    float x = pX2 - pX1;
    return (float) Math.Atan2((double) pY2 - (double) pY1, (double) x);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Quaternion getEulerAngle(float pX1, float pY1, float pX2, float pY2)
  {
    return Quaternion.Euler(new Vector3(0.0f, 0.0f, Toolbox.getAngleDegrees(pX1, pY1, pX2, pY2)));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Quaternion getEulerAngle(Vector2 pVec1, Vector2 pVec2)
  {
    return Quaternion.Euler(new Vector3(0.0f, 0.0f, Toolbox.getAngleDegrees(pVec1, pVec2)));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float getAngleDegrees(Vector2 pVec1, Vector2 pVec2)
  {
    return Toolbox.getAngle(pVec1.x, pVec1.y, pVec2.x, pVec2.y) * 57.29578f;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float getAngleDegrees(float pX1, float pY1, float pX2, float pY2)
  {
    return Toolbox.getAngle(pX1, pY1, pX2, pY2) * 57.29578f;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Color makeDarkerColor(Color pColor, float pMod = 0.4f)
  {
    return new Color(pColor.r * pMod, pColor.g * pMod, pColor.b * pMod, pColor.a);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static string makeDarkerColor(string pHexColor, float pMod = 0.4f)
  {
    return Toolbox.colorToHex(Color32.op_Implicit(Toolbox.makeDarkerColor(Toolbox.makeColor(pHexColor), pMod)));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Color blendColor(Color pFrom, Color pTo, float amount)
  {
    double num1 = (double) pFrom.r * (double) amount + (double) pTo.r * (1.0 - (double) amount);
    float num2 = (float) ((double) pFrom.g * (double) amount + (double) pTo.g * (1.0 - (double) amount));
    float num3 = (float) ((double) pFrom.b * (double) amount + (double) pTo.b * (1.0 - (double) amount));
    double num4 = (double) num2;
    double num5 = (double) num3;
    return new Color((float) num1, (float) num4, (float) num5);
  }

  public static WorldTile getClosestTile(WorldTile[] pArray, WorldTile pTarget)
  {
    WorldTile closestTile = (WorldTile) null;
    WorldTile[] worldTileArray = pArray;
    int length = worldTileArray.Length;
    int num1 = int.MaxValue;
    for (int index = 0; index < length; ++index)
    {
      WorldTile worldTile = worldTileArray[index];
      int num2 = Toolbox.SquaredDist(pTarget.x, pTarget.y, worldTile.x, worldTile.y);
      if (num2 < num1)
      {
        num1 = num2;
        closestTile = worldTile;
      }
    }
    return closestTile;
  }

  public static WorldTile getClosestTile(List<WorldTile> pArray, WorldTile pTarget)
  {
    WorldTile closestTile = (WorldTile) null;
    List<WorldTile> worldTileList = pArray;
    int count = worldTileList.Count;
    int num1 = int.MaxValue;
    for (int index = 0; index < count; ++index)
    {
      WorldTile worldTile = worldTileList[index];
      int num2 = Toolbox.SquaredDist(pTarget.x, pTarget.y, worldTile.x, worldTile.y);
      if (num2 < num1)
      {
        num1 = num2;
        closestTile = worldTile;
      }
    }
    return closestTile;
  }

  public static WorldTile getClosestTile(ListPool<WorldTile> pArray, WorldTile pTarget)
  {
    WorldTile closestTile = (WorldTile) null;
    ListPool<WorldTile> listPool = pArray;
    int count = listPool.Count;
    int num1 = int.MaxValue;
    for (int index = 0; index < count; ++index)
    {
      WorldTile worldTile = listPool[index];
      int num2 = Toolbox.SquaredDist(pTarget.x, pTarget.y, worldTile.x, worldTile.y);
      if (num2 < num1)
      {
        num1 = num2;
        closestTile = worldTile;
      }
    }
    return closestTile;
  }

  public static void sortRegionsByDistance(WorldTile pTile, List<MapRegion> pRegions)
  {
    pRegions.Sort((Comparison<MapRegion>) ((x, y) => Toolbox.SquaredDistTile(pTile, x.tiles[0]).CompareTo(Toolbox.SquaredDistTile(pTile, y.tiles[0]))));
  }

  public static void sortTilesByDistance(WorldTile pTile, ListPool<WorldTile> pTiles)
  {
    pTiles.Sort((Comparison<WorldTile>) ((x, y) => Toolbox.SquaredDistTile(pTile, x).CompareTo(Toolbox.SquaredDistTile(pTile, y))));
  }

  public static float maxTileDistance(WorldTile pTile, ListPool<WorldTile> pTiles)
  {
    float num1 = 0.0f;
    for (int index = 0; index < pTiles.Count; ++index)
    {
      WorldTile pTile1 = pTiles[index];
      float num2 = Toolbox.DistTile(pTile, pTile1);
      if ((double) num2 > (double) num1)
        num1 = num2;
    }
    return num1;
  }

  public static MapRegion getClosestRegion(List<MapRegion> pArray, WorldTile pTarget)
  {
    MapRegion closestRegion = (MapRegion) null;
    int num1 = int.MaxValue;
    for (int index = 0; index < pArray.Count; ++index)
    {
      MapRegion p = pArray[index];
      Vector2Int pos = pTarget.pos;
      int x1 = ((Vector2Int) ref pos).x;
      pos = pTarget.pos;
      int y1 = ((Vector2Int) ref pos).y;
      pos = p.tiles[0].pos;
      int x2 = ((Vector2Int) ref pos).x;
      pos = p.tiles[0].pos;
      int y2 = ((Vector2Int) ref pos).y;
      int num2 = Toolbox.SquaredDist(x1, y1, x2, y2);
      if (num2 < num1)
      {
        num1 = num2;
        closestRegion = p;
      }
    }
    return closestRegion;
  }

  public static Vector2Int getRandomVectorWithinDistance(int pX, int pY, int pRange)
  {
    Vector2 pPos1;
    // ISSUE: explicit constructor call
    ((Vector2) ref pPos1).\u002Ector((float) (pX - pRange), (float) (pY - pRange));
    Vector2 pPos2;
    // ISSUE: explicit constructor call
    ((Vector2) ref pPos2).\u002Ector((float) (pX + pRange), (float) (pY + pRange));
    Toolbox.clampToMap(ref pPos1);
    Toolbox.clampToMap(ref pPos2);
    Vector2 vector2 = new Vector2();
    vector2.x = Randy.randomFloat(pPos1.x, pPos2.x);
    vector2.y = Randy.randomFloat(pPos1.y, pPos2.y);
    return new Vector2Int((int) vector2.x, (int) vector2.y);
  }

  public static WorldTile getRandomTileWithinDistance(WorldTile pWorldTile, int pRange)
  {
    Vector2Int pos = pWorldTile.pos;
    int x = ((Vector2Int) ref pos).x;
    pos = pWorldTile.pos;
    int y = ((Vector2Int) ref pos).y;
    int pRange1 = pRange;
    Vector2Int vectorWithinDistance = Toolbox.getRandomVectorWithinDistance(x, y, pRange1);
    return World.world.GetTileSimple(((Vector2Int) ref vectorWithinDistance).x, ((Vector2Int) ref vectorWithinDistance).y);
  }

  public static WorldTile getRandomTileWithinDistance(
    WorldTile pWorldTile,
    int pRange,
    ListPool<WorldTile> pTiles)
  {
    foreach (WorldTile pT2 in pTiles.LoopRandom<WorldTile>())
    {
      if ((double) Toolbox.DistTile(pWorldTile, pT2) <= (double) pRange)
        return pT2;
    }
    return Toolbox.getRandomTileWithinDistance(pWorldTile, pRange);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Actor getClosestActor(HashSet<Actor> pCollection, WorldTile pTile)
  {
    Actor closestActor = (Actor) null;
    int num1 = int.MaxValue;
    Vector2Int pos1 = pTile.pos;
    foreach (Actor p in pCollection)
    {
      if (!p.isRekt())
      {
        Vector2Int pos2 = p.current_tile.pos;
        int num2 = Toolbox.SquaredDist(((Vector2Int) ref pos1).x, ((Vector2Int) ref pos1).y, ((Vector2Int) ref pos2).x, ((Vector2Int) ref pos2).y);
        if (num2 < num1)
        {
          num1 = num2;
          closestActor = p;
        }
      }
    }
    return closestActor;
  }

  public static Actor getClosestActor(List<Actor> pCollection, WorldTile pTile)
  {
    Actor closestActor = (Actor) null;
    int num1 = int.MaxValue;
    Vector2Int pos1 = pTile.pos;
    int count = pCollection.Count;
    for (int index = 0; index < count; ++index)
    {
      Actor p = pCollection[index];
      Vector2Int pos2 = p.current_tile.pos;
      int num2 = Toolbox.SquaredDist(((Vector2Int) ref pos1).x, ((Vector2Int) ref pos1).y, ((Vector2Int) ref pos2).x, ((Vector2Int) ref pos2).y);
      if (num2 < num1)
      {
        num1 = num2;
        closestActor = p;
      }
    }
    return closestActor;
  }

  public static Actor getClosestActor(ListPool<Actor> pCollection, WorldTile pTile)
  {
    Actor closestActor = (Actor) null;
    int num1 = int.MaxValue;
    Vector2Int pos1 = pTile.pos;
    int count = pCollection.Count;
    for (int index = 0; index < count; ++index)
    {
      Actor p = pCollection[index];
      Vector2Int pos2 = p.current_tile.pos;
      int num2 = Toolbox.SquaredDist(((Vector2Int) ref pos1).x, ((Vector2Int) ref pos1).y, ((Vector2Int) ref pos2).x, ((Vector2Int) ref pos2).y);
      if (num2 < num1)
      {
        num1 = num2;
        closestActor = p;
      }
    }
    return closestActor;
  }

  public static Building getClosestBuilding(List<Building> pCollection, WorldTile pTile)
  {
    Building closestBuilding = (Building) null;
    int num1 = int.MaxValue;
    Vector2Int pos1 = pTile.pos;
    int count = pCollection.Count;
    for (int index = 0; index < count; ++index)
    {
      Building p = pCollection[index];
      Vector2Int pos2 = p.current_tile.pos;
      int num2 = Toolbox.SquaredDist(((Vector2Int) ref pos1).x, ((Vector2Int) ref pos1).y, ((Vector2Int) ref pos2).x, ((Vector2Int) ref pos2).y);
      if (num2 < num1)
      {
        num1 = num2;
        closestBuilding = p;
      }
    }
    return closestBuilding;
  }

  public static async Task<byte[]> ReadAllBytes(string filePath)
  {
    byte[] result;
    using (FileStream stream = File.Open(filePath, FileMode.Open))
    {
      result = new byte[stream.Length];
      int num = await stream.ReadAsync(result, 0, (int) stream.Length);
    }
    byte[] numArray = result;
    result = (byte[]) null;
    return numArray;
  }

  public static Sprite LoadSprite(string path)
  {
    if (string.IsNullOrEmpty(path))
      return (Sprite) null;
    if (!File.Exists(path))
      return (Sprite) null;
    byte[] numArray = File.ReadAllBytes(path);
    Texture2D texture2D = new Texture2D(1, 1);
    ((Texture) texture2D).anisoLevel = 0;
    ImageConversion.LoadImage(texture2D, numArray);
    return Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float) ((Texture) texture2D).width, (float) ((Texture) texture2D).height), new Vector2(0.5f, 0.5f));
  }

  public static Sprite LoadResizedSprite(string path, int width, int height)
  {
    Sprite sprite1 = Toolbox.LoadSprite(path);
    if (Object.op_Equality((Object) sprite1, (Object) null))
      return (Sprite) null;
    Sprite sprite2 = Sprite.Create(Toolbox.ScaleTexture(sprite1.texture, width, height), new Rect(0.0f, 0.0f, (float) width, (float) height), new Vector2(0.0f, 0.0f));
    Object.DestroyImmediate((Object) sprite1.texture);
    Object.DestroyImmediate((Object) sprite1);
    return sprite2;
  }

  public static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
  {
    Texture2D texture2D = new Texture2D(targetWidth, targetHeight, source.format, true);
    Color[] pixels = texture2D.GetPixels(0);
    float num1 = (float) (1.0 / (double) ((Texture) source).width * ((double) ((Texture) source).width / (double) targetWidth));
    float num2 = (float) (1.0 / (double) ((Texture) source).height * ((double) ((Texture) source).height / (double) targetHeight));
    for (int index = 0; index < pixels.Length; ++index)
      pixels[index] = source.GetPixelBilinear(num1 * ((float) index % (float) targetWidth), num2 * Mathf.Floor((float) (index / targetWidth)));
    texture2D.SetPixels(pixels, 0);
    texture2D.Apply();
    return texture2D;
  }

  public static string formatTimer(float pTime)
  {
    int num1 = (int) ((double) pTime / 60.0);
    int num2 = (int) ((double) pTime - (double) (num1 * 60));
    string str = num1 >= 10 ? num1.ToString() + ":" : $"0{num1.ToString()}:";
    return num2 >= 10 ? str + num2.ToString() : $"{str}0{num2.ToString()}";
  }

  public static string formatTime(float pTime)
  {
    string str1 = "";
    TimeSpan timeSpan = TimeSpan.FromSeconds((double) pTime);
    int num1 = timeSpan.Days / 7;
    int days = timeSpan.Days;
    int totalHours = (int) timeSpan.TotalHours;
    if (num1 > 0)
    {
      str1 = $"{str1}{num1.ToString()}w ";
      days -= num1 * 7;
      totalHours -= num1 * 7 * 24;
    }
    if (days > 1)
    {
      str1 = $"{str1}{days.ToString()}d ";
      totalHours -= days * 24;
    }
    string str2 = str1 + totalHours.ToString();
    int num2;
    string str3;
    if (timeSpan.Minutes < 10)
    {
      string str4 = str2;
      num2 = timeSpan.Minutes;
      string str5 = num2.ToString();
      str3 = $"{str4}:0{str5}";
    }
    else
    {
      string str6 = str2;
      num2 = timeSpan.Minutes;
      string str7 = num2.ToString();
      str3 = $"{str6}:{str7}";
    }
    string str8;
    if (timeSpan.Seconds < 10)
    {
      string str9 = str3;
      num2 = timeSpan.Seconds;
      string str10 = num2.ToString();
      str8 = $"{str9}:0{str10}";
    }
    else
    {
      string str11 = str3;
      num2 = timeSpan.Seconds;
      string str12 = num2.ToString();
      str8 = $"{str11}:{str12}";
    }
    return str8;
  }

  public static string formatNumber(long pNumber)
  {
    long num = Math.Abs(pNumber);
    if (num >= 10000000000L)
      return ((double) pNumber / 1000000000.0).ToString("N0") + "b";
    if (num >= 1000000000L)
      return ((double) pNumber / 1000000000.0).ToText() + "b";
    if (num >= 10000000L)
      return ((double) pNumber / 1000000.0).ToString("N0") + "m";
    if (num >= 1000000L)
      return ((double) pNumber / 1000000.0).ToText() + "m";
    if (num >= 10000L)
      return ((float) pNumber / 1000f).ToString("N0") + "k";
    return num >= 1000L ? ((float) pNumber / 1000f).ToText() + "k" : pNumber.ToText();
  }

  public static string formatNumber(long pNumber, int pMaxSize)
  {
    return pNumber.ToText().Length <= pMaxSize ? pNumber.ToText() : Toolbox.formatNumber(pNumber);
  }

  internal static void clearAll()
  {
    Toolbox.temp_list_tiles.Clear();
    Toolbox._temp_array_chunks.Clear<MapChunk>();
    Toolbox._temp_array_zones.Clear<TileZone>();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static MapChunk getRandomChunkFromTile(WorldTile pTile)
  {
    (MapChunk[], int) allChunksFromTile = Toolbox.getAllChunksFromTile(pTile);
    return allChunksFromTile.Item1.GetRandom<MapChunk>(allChunksFromTile.Item2);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static WorldTile getRandomTileAround(WorldTile pTile)
  {
    return Toolbox.getRandomChunkFromTile(pTile).tiles.GetRandom<WorldTile>();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static (MapChunk[], int) getAllChunksFromTile(WorldTile pTile)
  {
    return Toolbox.getAllChunksFromChunk(pTile.chunk);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static (MapChunk[], int) getAllChunksFromChunk(MapChunk pChunk)
  {
    MapChunk[] tempArrayChunks = Toolbox._temp_array_chunks;
    tempArrayChunks[0] = pChunk;
    int length = pChunk.neighbours_all.Length;
    Array.Copy((Array) pChunk.neighbours_all, 0, (Array) Toolbox._temp_array_chunks, 1, length);
    return (tempArrayChunks, length + 1);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static (TileZone[], int) getAllZonesFromTile(WorldTile pTile)
  {
    return Toolbox.getAllZonesFromZone(pTile.zone);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static (TileZone[], int) getAllZonesFromZone(TileZone pZone)
  {
    TileZone[] tempArrayZones = Toolbox._temp_array_zones;
    tempArrayZones[0] = pZone;
    int length = pZone.neighbours_all.Length;
    Array.Copy((Array) pZone.neighbours_all, 0, (Array) Toolbox._temp_array_zones, 1, length);
    return (tempArrayZones, length + 1);
  }

  internal static bool hasDifferentSpeciesInChunkAround(WorldTile pTile, string pSpecies)
  {
    foreach (BaseSimObject baseSimObject in Finder.getUnitsFromChunk(pTile, 1))
    {
      if (!baseSimObject.a.isSameSpecies(pSpecies))
        return true;
    }
    return false;
  }

  internal static int countUnitsInChunk(WorldTile pTile)
  {
    int num = 0;
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 0))
      ++num;
    return num;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static bool inMapBorder(ref Vector2 pPoint)
  {
    return (double) pPoint.x < (double) MapBox.width && (double) pPoint.y < (double) MapBox.height && (double) pPoint.x >= 0.0 && (double) pPoint.y >= 0.0;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static bool inMapBorder(ref Vector3 pPoint)
  {
    return (double) pPoint.x < (double) MapBox.width && (double) pPoint.y < (double) MapBox.height && (double) pPoint.x >= 0.0 && (double) pPoint.y >= 0.0;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static void clampToMap(ref Vector2 pPos)
  {
    pPos.x = Mathf.Clamp(pPos.x, 0.0f, (float) (MapBox.width - 1));
    pPos.y = Mathf.Clamp(pPos.y, 0.0f, (float) (MapBox.height - 1));
  }

  internal static IEnumerable<Building> getBuildingsTypeFromChunk(
    MapChunk pChunk,
    string pType,
    bool pOnlyNonTargeted,
    bool pOnlyWithResources)
  {
    foreach (Building building in Finder.getBuildingsFromChunk(pChunk.tiles[0], 0, pRandom: true))
    {
      if ((!pOnlyWithResources || building.hasResourcesToCollect()) && building.isUsable() && (!pOnlyNonTargeted || !building.current_tile.isTargeted()) && building.asset.type == pType)
        yield return building;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Quaternion LookAt2D(Vector2 forward)
  {
    return Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(forward.y, forward.x) * 57.29578f);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static string LowerCaseFirst(string pString)
  {
    return pString.Length == 0 ? "" : char.ToLower(pString[0]).ToString() + (pString.Length > 1 ? pString.Substring(1) : string.Empty);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T[] resizeArray<T>(T[] pArray, int aPos)
  {
    Array.Resize<T>(ref pArray, aPos);
    return pArray;
  }

  public static string getRoundedTimestamp()
  {
    DateTime utcNow = DateTime.UtcNow;
    DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    int num;
    string str1;
    if (utcNow.Month >= 10)
    {
      num = utcNow.Month;
      str1 = num.ToString();
    }
    else
    {
      num = utcNow.Month;
      str1 = "0" + num.ToString();
    }
    string str2 = str1;
    string str3;
    if (utcNow.Day >= 10)
    {
      num = utcNow.Day;
      str3 = num.ToString();
    }
    else
    {
      num = utcNow.Day;
      str3 = "0" + num.ToString();
    }
    string str4 = str3;
    num = utcNow.Year;
    return num.ToString() + str2 + str4;
  }

  public static ListPool<string> getDirectories(string pPath)
  {
    ListPool<string> directories = new ListPool<string>();
    foreach (string directory in Directory.GetDirectories(pPath))
    {
      if (!directory.Contains(".meta"))
        directories.Add(directory);
    }
    return directories;
  }

  public static ListPool<string> getFiles(string pPath)
  {
    ListPool<string> files = new ListPool<string>();
    foreach (string file in Directory.GetFiles(pPath))
    {
      if (!file.Contains(".meta"))
        files.Add(file);
    }
    return files;
  }

  public static string cacheBuster()
  {
    return $"{DateTime.UtcNow.RoundMinutes().ToFileTime().ToString()}_{Config.versionCodeText}";
  }

  public static DateTime RoundMinutes(this DateTime value) => value.RoundMinutes(30);

  public static DateTime RoundMinutes(this DateTime value, int roundMinutes)
  {
    DateTime dateTime = new DateTime(value.Ticks);
    int minute = value.Minute;
    int hour = value.Hour;
    int num1 = roundMinutes;
    int num2 = minute % num1;
    return num2 > roundMinutes / 2 ? dateTime.AddMinutes((double) (roundMinutes - num2)) : dateTime.AddMinutes((double) -num2);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static WorldTile getTileAt(float pX, float pY)
  {
    return World.world.GetTileSimple(Mathf.Clamp(Mathf.FloorToInt(pX), 0, MapBox.width - 1), Mathf.Clamp(Mathf.FloorToInt(pY), 0, MapBox.height - 1));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static WorldTile getNearestTileToCursor()
  {
    Vector2 vector2 = Vector2.op_Implicit(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    return Toolbox.getTileAt(vector2.x, vector2.y);
  }

  public static bool isBlockAt(float pX, float pY)
  {
    WorldTile tileAt = Toolbox.getTileAt(pX, pY);
    return tileAt != null && tileAt.Type.block;
  }

  public static List<string> splitStringIntoList(params string[] pTypes)
  {
    List<string> stringList = new List<string>();
    for (int index1 = 0; index1 < pTypes.Length; ++index1)
    {
      string pType = pTypes[index1];
      if (pType.Contains("#"))
      {
        string[] strArray = pType.Split('#', StringSplitOptions.None);
        string str = strArray[0];
        if (strArray.Length > 2)
        {
          Debug.LogError((object) ("WRONG FORMAT - splitStringIntoList" + pType));
          Debug.LogError((object) "RETURN EMPTY STRING");
          return new List<string>();
        }
        int num = int.Parse(strArray[1]);
        for (int index2 = 0; index2 < num; ++index2)
          stringList.Add(str);
      }
      else
        stringList.Add(pType);
    }
    return stringList;
  }

  public static string[] splitStringIntoArray(params string[] pTypes)
  {
    using (ListPool<string> list = new ListPool<string>(pTypes.Length * 2))
    {
      for (int index1 = 0; index1 < pTypes.Length; ++index1)
      {
        string pType = pTypes[index1];
        if (pType.Contains('#'))
        {
          string[] strArray = pType.Split('#', StringSplitOptions.None);
          string str = strArray[0];
          if (strArray.Length > 2)
          {
            Debug.LogError((object) ("WRONG FORMAT - splitStringIntoList" + pType));
            Debug.LogError((object) "RETURN EMPTY STRING");
            return new string[0];
          }
          int num = int.Parse(strArray[1]);
          for (int index2 = 0; index2 < num; ++index2)
            list.Add(str);
        }
        else
          list.Add(pType);
      }
      return list.ToArray<string>();
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool isFirstLatin(string pString)
  {
    char ch = pString[0];
    return ch >= 'A' && ch <= 'Z';
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector2 Parabola(Vector2 pStart, Vector2 pEnd, float pHeight, float pTime)
  {
    pTime = Mathf.Clamp(pTime, 0.0f, 1f);
    Vector2 vector2 = Vector2.Lerp(pStart, pEnd, pTime);
    return new Vector2(vector2.x, Toolbox.parabolaHelper(pTime, pHeight) + vector2.y);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector2 ParabolaDrag(Vector2 pStart, Vector2 pEnd, float pHeight, float pTime)
  {
    pTime = Mathf.Clamp(pTime, 0.0f, 1f);
    double num1 = (double) Mathf.Lerp(pStart.x, pEnd.x, iTween.easeOutQuad(0.0f, 1f, pTime));
    float num2 = Mathf.Lerp(pStart.y, pEnd.y, iTween.easeInQuad(0.0f, 1f, pTime));
    double num3 = (double) Toolbox.parabolaHelper(pTime, pHeight) + (double) num2;
    return new Vector2((float) num1, (float) num3);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static float parabolaHelper(float pTime, float pHeight)
  {
    return (float) (-4.0 * (double) pHeight * (double) pTime * (double) pTime + 4.0 * (double) pHeight * (double) pTime);
  }

  public static bool WriteSafely(string pWhat, string pDataPath, ref string pStringData)
  {
    return Toolbox.WriteSafely(pWhat, pDataPath, ref pStringData, (byte[]) null);
  }

  public static bool WriteSafely(string pWhat, string pDataPath, byte[] pByteData)
  {
    string pStringData = (string) null;
    return Toolbox.WriteSafely(pWhat, pDataPath, ref pStringData, pByteData);
  }

  private static bool WriteSafely(
    string pWhat,
    string pDataPath,
    ref string pStringData,
    byte[] pByteData)
  {
    bool flag = false;
    try
    {
      if (!string.IsNullOrEmpty(pStringData))
        File.WriteAllText(pDataPath + ".tmp", pStringData);
      if (pByteData != null)
        File.WriteAllBytes(pDataPath + ".tmp", pByteData);
    }
    catch (IOException ex)
    {
      if (Toolbox.IsDiskFull(ex))
      {
        WorldTip.showNow($"Error saving {pWhat} : Disk full!", false, "top");
      }
      else
      {
        Debug.Log((object) $"Could not save {pWhat} due to hard drive / IO Error : ");
        Debug.Log((object) ex);
        WorldTip.showNow($"Error saving {pWhat} due to IOError! Check console for details", false, "top");
      }
      flag = true;
    }
    catch (Exception ex)
    {
      Debug.Log((object) $"Could not save {pWhat} due to error : ");
      Debug.Log((object) ex);
      WorldTip.showNow($"Error saving {pWhat}! Check console for errors", false, "top");
      flag = true;
    }
    if (flag)
    {
      if (File.Exists(pDataPath + ".tmp"))
        File.Delete(pDataPath + ".tmp");
      return false;
    }
    if (File.Exists(pDataPath))
      File.Delete(pDataPath);
    File.Move(pDataPath + ".tmp", pDataPath);
    return true;
  }

  public static bool MoveSafely(string pOldPath, string pNewPath)
  {
    if (string.IsNullOrEmpty(pOldPath) || string.IsNullOrEmpty(pNewPath))
      return false;
    if (File.Exists(pNewPath))
      File.Delete(pNewPath);
    File.Move(pOldPath, pNewPath);
    return true;
  }

  public static bool IsDiskFull(IOException ex)
  {
    return (ex.HResult & (int) ushort.MaxValue) == 39 || (ex.HResult & (int) ushort.MaxValue) == 112 /*0x70*/;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static string textureID(string pStringData, string pID)
  {
    return Encryption.EncryptString(pStringData, pID);
  }

  public static int getClosestAngle(int pAngle, AnimationDataBoat pData)
  {
    int closestAngle = int.MinValue;
    float num1 = 0.0f;
    foreach (int key in pData.dict.Keys)
    {
      float num2 = (float) Mathf.Abs(key - pAngle);
      if ((double) num2 < (double) num1 || closestAngle == int.MinValue)
      {
        num1 = num2;
        closestAngle = key;
      }
    }
    return closestAngle;
  }

  public static bool isInTriangle(Vector2 pPoint, Vector2 p0, Vector2 p1, Vector2 p2)
  {
    float num1 = (float) (0.5 * (-(double) p1.y * (double) p2.x + (double) p0.y * (-(double) p1.x + (double) p2.x) + (double) p0.x * ((double) p1.y - (double) p2.y) + (double) p1.x * (double) p2.y));
    int num2 = (double) num1 < 0.0 ? -1 : 1;
    float num3 = (float) ((double) p0.y * (double) p2.x - (double) p0.x * (double) p2.y + ((double) p2.y - (double) p0.y) * (double) pPoint.x + ((double) p0.x - (double) p2.x) * (double) pPoint.y) * (float) num2;
    float num4 = (float) ((double) p0.x * (double) p1.y - (double) p0.y * (double) p1.x + ((double) p0.y - (double) p1.y) * (double) pPoint.x + ((double) p1.x - (double) p0.x) * (double) pPoint.y) * (float) num2;
    return (double) num3 > 0.0 && (double) num4 > 0.0 && (double) num3 + (double) num4 < 2.0 * (double) num1 * (double) num2;
  }

  public static List<string> getListForSave<T>(IReadOnlyCollection<T> pList) where T : Asset
  {
    List<string> listForSave = new List<string>(pList.Count);
    foreach (T p in (IEnumerable<T>) pList)
      listForSave.Add(p.id);
    return listForSave;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T[] checkArraySize<T>(T[] pArray, int pTargetSize)
  {
    if (pArray == null || pTargetSize > pArray.Length)
      pArray = new T[Toolbox.nextPowerOfTwo(pTargetSize)];
    return pArray;
  }

  private static int nextPowerOfTwo(int pN)
  {
    --pN;
    pN |= pN >> 1;
    pN |= pN >> 2;
    pN |= pN >> 4;
    pN |= pN >> 8;
    pN |= pN >> 16 /*0x10*/;
    ++pN;
    return pN;
  }

  public static string fillLeft(string pString, int pSize = 1, char pFill = ' ')
  {
    string str = Toolbox.removeRichTextTags(pString);
    if (pString != str)
      pSize += pString.Length - str.Length;
    if (pString.Length >= pSize)
      return pString;
    Span<char> span = stackalloc char[pSize];
    MemoryExtensions.AsSpan(pString).CopyTo(span.Slice(pSize - pString.Length));
    for (int index = 0; index < pSize - pString.Length; ++index)
      span[index] = pFill;
    return new string(Span<char>.op_Implicit(span));
  }

  public static void fillRight(ref string pString, int pSize = 1, char pFill = ' ')
  {
    int length1 = pString.Length;
    if (Toolbox.removeRichTextTags(ref pString))
      pSize += length1 - pString.Length;
    if (pString.Length >= pSize)
      return;
    Span<char> span = stackalloc char[pSize];
    MemoryExtensions.AsSpan(pString).CopyTo(span.Slice(0, pString.Length));
    for (int length2 = pString.Length; length2 < pSize; ++length2)
      span[length2] = pFill;
    pString = new string(Span<char>.op_Implicit(span));
  }

  public static string fillRight(string pString, int pSize = 1, char pFill = ' ')
  {
    string str = Toolbox.removeRichTextTags(pString);
    if (pString != str)
      pSize += pString.Length - str.Length;
    if (pString.Length >= pSize)
      return pString;
    Span<char> span = stackalloc char[pSize];
    MemoryExtensions.AsSpan(pString).CopyTo(span.Slice(0, pString.Length));
    for (int length = pString.Length; length < pSize; ++length)
      span[length] = pFill;
    return new string(Span<char>.op_Implicit(span));
  }

  public static string printRows(ListPool<string[]> pRows, string pAlign = "right", bool pSkipFormatting = false)
  {
    int length1 = 0;
    int count = pRows.Count;
    for (int index = 0; index < count; ++index)
    {
      string[] pRow = pRows[index];
      if (pRow.Length > length1)
        length1 = pRow.Length;
    }
    int[] numArray = new int[length1];
    for (int index1 = 0; index1 < count; ++index1)
    {
      string[] pRow = pRows[index1];
      for (int index2 = 0; index2 < pRow.Length; ++index2)
      {
        int length2 = Toolbox.removeRichTextTags(pRow[index2]).Length;
        if (length2 > numArray[index2])
          numArray[index2] = length2;
      }
    }
    using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
    {
      for (int index3 = 0; index3 <= count; ++index3)
      {
        if (index3 == 0 || index3 == count || pRows[index3].Length == 0)
        {
          stringBuilderPool.Append("|");
          for (int index4 = 0; index4 < length1; ++index4)
          {
            if (numArray[index4] != 0)
            {
              stringBuilderPool.Append(Toolbox.fillRight("", numArray[index4] + 2, '='));
              stringBuilderPool.Append("|");
            }
          }
          stringBuilderPool.Append("\n");
          if (index3 == count)
            break;
        }
        string[] pRow = pRows[index3];
        if (pRow.Length != 0)
        {
          stringBuilderPool.Append("|");
          for (int index5 = 0; index5 < length1; ++index5)
          {
            if (numArray[index5] != 0)
            {
              string pString = "";
              if (index5 < pRow.Length)
                pString = pRow[index5];
              stringBuilderPool.Append(" ");
              if (index5 == 0)
              {
                if (!pSkipFormatting)
                  stringBuilderPool.Append("<b>");
                stringBuilderPool.Append(Toolbox.fillRight(pString, numArray[index5]));
                if (!pSkipFormatting)
                  stringBuilderPool.Append("</b>");
              }
              else if (pAlign == "right")
                stringBuilderPool.Append(Toolbox.fillLeft(pString, numArray[index5]));
              else
                stringBuilderPool.Append(Toolbox.fillRight(pString, numArray[index5]));
              stringBuilderPool.Append(" ");
              stringBuilderPool.Append("|");
            }
          }
          stringBuilderPool.Append("\n");
        }
      }
      return stringBuilderPool.ToString();
    }
  }

  public static string printColumns(params ListPool<string>[] pLists)
  {
    int num = 0;
    int length1 = pLists.Length;
    int[] numArray = new int[length1];
    for (int index1 = 0; index1 < length1; ++index1)
    {
      ListPool<string> pList = pLists[index1];
      if (pList.Count > num)
        num = pList.Count;
      for (int index2 = 0; index2 < pList.Count; ++index2)
      {
        int length2 = Toolbox.removeRichTextTags(pList[index2]).Length;
        if (length2 > numArray[index1])
          numArray[index1] = length2;
      }
    }
    using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
    {
      for (int index3 = 0; index3 < num; ++index3)
      {
        if (index3 == 0 || index3 == 1)
        {
          stringBuilderPool.Append("|");
          for (int index4 = 0; index4 < length1; ++index4)
          {
            if (numArray[index4] != 0)
            {
              stringBuilderPool.Append(Toolbox.fillRight("", numArray[index4] + 2, '='));
              stringBuilderPool.Append("|");
            }
          }
          stringBuilderPool.Append("\n");
        }
        stringBuilderPool.Append("|");
        for (int index5 = 0; index5 < length1; ++index5)
        {
          if (numArray[index5] != 0)
          {
            string pString = "";
            if (index3 < pLists[index5].Count)
              pString = pLists[index5][index3];
            stringBuilderPool.Append(" ");
            if (index3 == 0)
              stringBuilderPool.Append("<b>");
            stringBuilderPool.Append(Toolbox.fillRight(pString, numArray[index5] + 1));
            if (index3 == 0)
              stringBuilderPool.Append("</b>");
            stringBuilderPool.Append("|");
          }
        }
        stringBuilderPool.Append("\n");
        if (index3 == num - 1)
        {
          stringBuilderPool.Append("|");
          for (int index6 = 0; index6 < length1; ++index6)
          {
            if (numArray[index6] != 0)
            {
              stringBuilderPool.Append(Toolbox.fillRight("", numArray[index6] + 2, '='));
              stringBuilderPool.Append("|");
            }
          }
          stringBuilderPool.Append("\n");
        }
      }
      return stringBuilderPool.ToString();
    }
  }

  public static string getRepeatedString(char pChar, int pCount)
  {
    Span<char> span = stackalloc char[pCount];
    for (int index = 0; index < pCount; ++index)
      span[index] = pChar;
    return new string(Span<char>.op_Implicit(span));
  }

  public static bool removeRichTextTags(ref string pInput)
  {
    bool flag = false;
    while (true)
    {
      int startIndex = pInput.IndexOf('<');
      if (startIndex != -1)
      {
        int num = pInput.IndexOf('>', startIndex);
        if (num != -1)
        {
          pInput = pInput.Remove(startIndex, num - startIndex + 1);
          flag = true;
        }
        else
          goto label_4;
      }
      else
        break;
    }
    return flag;
label_4:
    return flag;
  }

  public static string removeRichTextTags(string pInput)
  {
    while (true)
    {
      int startIndex = pInput.IndexOf('<');
      if (startIndex != -1)
      {
        int num = pInput.IndexOf('>', startIndex);
        if (num != -1)
          pInput = pInput.Remove(startIndex, num - startIndex + 1);
        else
          goto label_3;
      }
      else
        break;
    }
    return pInput;
label_3:
    return pInput;
  }

  public static bool areListsEqual<T>(IList<T> pList1, IList<T> pList2)
  {
    HashSet<T> objSet = UnsafeCollectionPool<HashSet<T>, T>.Get();
    objSet.UnionWith((IEnumerable<T>) pList1);
    bool flag = objSet.SetEquals((IEnumerable<T>) pList2);
    UnsafeCollectionPool<HashSet<T>, T>.Release(objSet);
    return flag;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static TA[] a<TA>(params TA[] pArgs) => pArgs;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static List<TL> l<TL>(params TL[] pArgs) => List.Of<TL>(pArgs);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static HashSet<TH> h<TH>(params TH[] pArgs) => new HashSet<TH>((IEnumerable<TH>) pArgs);
}
