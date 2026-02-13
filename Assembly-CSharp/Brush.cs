// Decompiled with JetBrains decompiler
// Type: Brush
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public class Brush
{
  public static string getRandom() => AssetManager.brush_library.list.GetRandom<BrushData>().id;

  public static string getRandom(int pMinSize, int pMaxSize = 50, Predicate<BrushData> pMatch = null)
  {
    foreach (BrushData brushData in AssetManager.brush_library.list.LoopRandom<BrushData>())
    {
      if ((pMatch == null || pMatch(brushData)) && brushData.sqr_size >= pMinSize && brushData.sqr_size <= pMaxSize)
        return brushData.id;
    }
    return "circ_1";
  }

  public static BrushData get(int pSize, string pID = "circ_")
  {
    string str = pID + pSize.ToString();
    BrushData brushData1 = AssetManager.brush_library.get(str);
    if (brushData1 != null)
      return brushData1;
    BrushData brushData2 = AssetManager.brush_library.clone(str, pID + "1");
    brushData2.size = pSize;
    AssetManager.brush_library.post_init();
    return brushData2;
  }

  public static BrushData get(string pID) => AssetManager.brush_library.get(pID);
}
