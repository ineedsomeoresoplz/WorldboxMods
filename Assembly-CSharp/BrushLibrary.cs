// Decompiled with JetBrains decompiler
// Type: BrushLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BrushLibrary : AssetLibrary<BrushData>
{
  [NonSerialized]
  private static readonly List<string> _available_brushes = new List<string>();

  public override void init()
  {
    base.init();
    BrushData pAsset1 = new BrushData();
    pAsset1.id = "circ_0";
    pAsset1.size = 0;
    pAsset1.group = BrushGroup.Circles;
    pAsset1.fast_spawn = true;
    pAsset1.continuous = true;
    pAsset1.localized_key = "brush_circ";
    pAsset1.generate_action = (BrushGenerateAction) (pAsset => pAsset.pos = new BrushPixelData[1]
    {
      new BrushPixelData(0, 0, 0)
    });
    this.add(pAsset1);
    this.t.show_in_brush_window = true;
    this.clone("circ_1", "circ_0");
    this.t.size = 1;
    this.t.fast_spawn = false;
    this.t.show_in_brush_window = true;
    this.t.generate_action = (BrushGenerateAction) (pAsset =>
    {
      int size = pAsset.size;
      int num1 = 0;
      int num2 = size;
      int num3 = 1 - size;
      HashSet<BrushPixelData> pHashSet = new HashSet<BrushPixelData>();
      for (; num1 <= num2; ++num1)
      {
        for (int index = -num1; index <= num1; ++index)
        {
          int pDist = (int) Toolbox.Dist(0, 0, index, num2);
          pHashSet.Add(new BrushPixelData(index, num2, pDist));
          pHashSet.Add(new BrushPixelData(index, -num2, pDist));
        }
        for (int index = -num2; index <= num2; ++index)
        {
          int pDist = (int) Toolbox.Dist(0, 0, num1, index);
          pHashSet.Add(new BrushPixelData(index, num1, pDist));
          pHashSet.Add(new BrushPixelData(index, -num1, pDist));
        }
        if (num3 < 0)
        {
          num3 += 2 * num1 + 3;
        }
        else
        {
          num3 += 2 * (num1 - num2) + 5;
          --num2;
        }
      }
      pAsset.pos = pHashSet.ToArray<BrushPixelData>();
    });
    this.clone("circ_2", "circ_1");
    this.t.size = 2;
    this.t.show_in_brush_window = true;
    this.clone("circ_3", "circ_1");
    this.t.size = 3;
    this.t.show_in_brush_window = true;
    this.clone("circ_4", "circ_1");
    this.t.size = 4;
    this.t.continuous = false;
    this.t.show_in_brush_window = true;
    this.clone("circ_5", "circ_4");
    this.t.drops = 2;
    this.t.size = 5;
    this.t.show_in_brush_window = true;
    this.clone("circ_6", "circ_5");
    this.t.size = 6;
    this.t.show_in_brush_window = true;
    this.clone("circ_7", "circ_5");
    this.t.size = 7;
    this.t.show_in_brush_window = true;
    this.clone("circ_8", "circ_5");
    this.t.size = 8;
    this.clone("circ_9", "circ_5");
    this.t.size = 9;
    this.clone("circ_10", "circ_5");
    this.t.size = 10;
    this.t.show_in_brush_window = true;
    this.t.ui_scale = new Vector2(0.9f, 0.9f);
    this.clone("circ_11", "circ_5");
    this.t.size = 11;
    this.clone("circ_12", "circ_5");
    this.t.size = 12;
    this.clone("circ_15", "circ_5");
    this.t.size = 15;
    this.t.show_in_brush_window = true;
    this.t.ui_scale = new Vector2(0.9f, 0.9f);
    this.clone("circ_20", "circ_5");
    this.t.size = 20;
    this.clone("circ_30", "circ_5");
    this.t.size = 30;
    this.clone("circ_70", "circ_5");
    this.t.drops = 3;
    this.t.size = 70;
    BrushData pAsset2 = new BrushData();
    pAsset2.id = "sqr_0";
    pAsset2.size = 0;
    pAsset2.group = BrushGroup.Squares;
    pAsset2.continuous = true;
    pAsset2.fast_spawn = true;
    pAsset2.localized_key = "brush_sqr";
    pAsset2.generate_action = (BrushGenerateAction) (pAsset => pAsset.pos = new BrushPixelData[1]
    {
      new BrushPixelData(0, 0, 0)
    });
    this.add(pAsset2);
    BrushData pAsset3 = new BrushData();
    pAsset3.id = "sqr_1";
    pAsset3.size = 1;
    pAsset3.group = BrushGroup.Squares;
    pAsset3.continuous = true;
    pAsset3.fast_spawn = false;
    pAsset3.localized_key = "brush_sqr";
    this.add(pAsset3);
    this.t.show_in_brush_window = true;
    this.t.generate_action = (BrushGenerateAction) (pAsset =>
    {
      int size = pAsset.size;
      Vector2Int vector2Int;
      // ISSUE: explicit constructor call
      ((Vector2Int) ref vector2Int).\u002Ector(size / 2, size / 2);
      using (ListPool<BrushPixelData> list = new ListPool<BrushPixelData>())
      {
        for (int index1 = -size; index1 <= size; ++index1)
        {
          for (int index2 = -size; index2 <= size; ++index2)
          {
            int pDist = (int) Toolbox.Dist(index1, index2, ((Vector2Int) ref vector2Int).x, ((Vector2Int) ref vector2Int).y);
            list.Add(new BrushPixelData(index1, index2, pDist));
          }
        }
        pAsset.pos = list.ToArray<BrushPixelData>();
      }
    });
    this.clone("sqr_2", "sqr_1");
    this.t.size = 2;
    this.t.continuous = false;
    this.t.show_in_brush_window = true;
    this.clone("sqr_3", "sqr_1");
    this.t.size = 3;
    this.t.continuous = false;
    this.clone("sqr_4", "sqr_1");
    this.t.size = 4;
    this.t.continuous = false;
    this.t.show_in_brush_window = true;
    this.clone("sqr_5", "sqr_1");
    this.t.size = 5;
    this.t.continuous = false;
    this.clone("sqr_10", "sqr_1");
    this.t.size = 10;
    this.t.drops = 2;
    this.t.continuous = false;
    this.t.show_in_brush_window = true;
    this.t.ui_scale = new Vector2(0.8f, 0.8f);
    this.clone("sqr_15", "sqr_10");
    this.t.size = 15;
    this.t.continuous = false;
    this.t.show_in_brush_window = true;
    this.t.ui_scale = new Vector2(0.8f, 0.8f);
    BrushData pAsset4 = new BrushData();
    pAsset4.id = "diamond_1";
    pAsset4.continuous = true;
    pAsset4.group = BrushGroup.Diamonds;
    pAsset4.localized_key = "brush_diamond";
    pAsset4.size = 1;
    pAsset4.fast_spawn = false;
    this.add(pAsset4);
    this.t.show_in_brush_window = true;
    this.t.generate_action = (BrushGenerateAction) (pAsset =>
    {
      Texture2D texture2D = Resources.Load<Texture2D>("ui/Icons/brushes/" + ("brush_" + pAsset.id));
      int width = ((Texture) texture2D).width;
      int height = ((Texture) texture2D).height;
      int num4 = width / 2;
      Vector2Int vector2Int;
      // ISSUE: explicit constructor call
      ((Vector2Int) ref vector2Int).\u002Ector(width / 2, height / 2);
      using (ListPool<BrushPixelData> list = new ListPool<BrushPixelData>())
      {
        for (int index3 = 0; index3 < width; ++index3)
        {
          for (int index4 = 0; index4 < height; ++index4)
          {
            if (!Color.op_Inequality(texture2D.GetPixel(index3, index4), Color.white))
            {
              int num5 = ((Vector2Int) ref vector2Int).x - index3;
              int num6 = ((Vector2Int) ref vector2Int).y - index4;
              int pDist = (int) Toolbox.Dist(num5, num6, ((Vector2Int) ref vector2Int).x, ((Vector2Int) ref vector2Int).y);
              list.Add(new BrushPixelData(num5, num6, pDist));
            }
          }
        }
        pAsset.pos = list.ToArray<BrushPixelData>();
      }
    });
    this.clone("diamond_2", "diamond_1");
    this.t.size = 2;
    this.t.show_in_brush_window = true;
    this.t.ui_scale = new Vector2(0.8f, 0.8f);
    this.clone("diamond_4", "diamond_1");
    this.t.size = 4;
    this.t.continuous = false;
    this.t.show_in_brush_window = true;
    this.t.ui_scale = new Vector2(0.7f, 0.7f);
    this.clone("diamond_5", "diamond_1");
    this.t.drops = 2;
    this.t.size = 5;
    this.t.continuous = false;
    this.t.show_in_brush_window = true;
    this.t.ui_scale = new Vector2(0.7f, 0.7f);
    this.clone("diamond_7", "diamond_5");
    this.t.size = 7;
    this.t.continuous = false;
    this.t.show_in_brush_window = true;
    this.t.ui_scale = new Vector2(0.9f, 0.9f);
    BrushData pAsset5 = new BrushData();
    pAsset5.id = "special_1";
    pAsset5.continuous = true;
    pAsset5.group = BrushGroup.Special;
    pAsset5.localized_key = "brush_special";
    pAsset5.size = 1;
    pAsset5.fast_spawn = false;
    this.add(pAsset5);
    this.t.generate_action = this.get("diamond_1").generate_action;
    this.t.show_in_brush_window = true;
    this.t.ui_size = new Vector2(14f, 14f);
    this.clone("special_2", "special_1");
    this.t.size = 2;
    this.t.show_in_brush_window = true;
    this.t.ui_size = new Vector2(17.93f, 17.93f);
    this.clone("special_3", "special_1");
    this.t.size = 3;
    this.t.continuous = false;
    this.t.show_in_brush_window = true;
    this.t.ui_size = new Vector2(21.4f, 21.4f);
    this.clone("special_4", "special_1");
    this.t.drops = 2;
    this.t.size = 4;
    this.t.continuous = false;
    this.t.show_in_brush_window = true;
    this.t.ui_size = new Vector2(23.55f, 23.55f);
    this.clone("special_5", "special_4");
    this.t.drops = 2;
    this.t.size = 5;
    this.t.continuous = false;
    this.t.show_in_brush_window = true;
    this.t.ui_size = new Vector2(28.28f, 28.28f);
  }

  public override void post_init()
  {
    base.post_init();
    foreach (BrushData pAsset in this.list)
    {
      if (pAsset.show_in_brush_window)
        BrushLibrary._available_brushes.Add(pAsset.id);
      BrushPixelData[] pos = pAsset.pos;
      if ((pos != null ? (pos.Length != 0 ? 1 : 0) : 0) == 0)
      {
        pAsset.generate_action(pAsset);
        int num1 = int.MaxValue;
        int num2 = int.MinValue;
        int num3 = int.MaxValue;
        int num4 = int.MinValue;
        for (int index = 0; index < pAsset.pos.Length; ++index)
        {
          BrushPixelData po = pAsset.pos[index];
          if (po.x < num1)
            num1 = po.x;
          if (po.x > num2)
            num2 = po.x;
          if (po.y < num3)
            num3 = po.y;
          if (po.y > num4)
            num4 = po.y;
        }
        pAsset.width = Mathf.Abs(num2 - num1) + 1;
        pAsset.height = Mathf.Abs(num4 - num3) + 1;
        pAsset.sqr_size = pAsset.width * pAsset.height;
      }
    }
  }

  public override void linkAssets()
  {
    base.linkAssets();
    foreach (BrushData pAsset in this.list)
      BrushLibrary.shuffleBrush(pAsset);
  }

  public static void shuffleBrush(BrushData pAsset)
  {
    pAsset.pos.Shuffle<BrushPixelData>();
    int index1 = 0;
    for (int index2 = 0; index2 < pAsset.pos.Length; ++index2)
    {
      BrushPixelData po = pAsset.pos[index2];
      if (po.x == 0 && po.y == 0)
      {
        index1 = index2;
        break;
      }
    }
    BrushPixelData po1 = pAsset.pos[0];
    BrushPixelData po2 = pAsset.pos[index1];
    pAsset.pos[0] = po2;
    pAsset.pos[index1] = po1;
  }

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (BrushData pAsset in this.list)
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
  }

  public override BrushData clone(string pNew, string pFrom)
  {
    BrushData brushData = base.clone(pNew, pFrom);
    brushData.show_in_brush_window = false;
    return brushData;
  }

  internal static void nextBrush()
  {
    Config.current_brush = BrushLibrary.getPrevious(Config.current_brush);
  }

  internal static void previousBrush()
  {
    Config.current_brush = BrushLibrary.getNext(Config.current_brush);
  }

  private static string getNext(string pBrushName)
  {
    bool flag = false;
    for (int index = 0; index < BrushLibrary._available_brushes.Count; ++index)
    {
      string availableBrush = BrushLibrary._available_brushes[index];
      if (availableBrush == pBrushName)
        flag = true;
      else if (flag)
        return availableBrush;
    }
    return pBrushName;
  }

  private static string getPrevious(string pBrushName)
  {
    bool flag = false;
    for (int index = BrushLibrary._available_brushes.Count - 1; index >= 0; --index)
    {
      string availableBrush = BrushLibrary._available_brushes[index];
      if (availableBrush == pBrushName)
        flag = true;
      else if (flag)
        return availableBrush;
    }
    return pBrushName;
  }
}
