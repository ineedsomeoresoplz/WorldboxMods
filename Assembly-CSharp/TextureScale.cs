// Decompiled with JetBrains decompiler
// Type: TextureScale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TextureScale
{
  private static Color[] texColors;
  private static Color[] newColors;
  private static int w;
  private static float ratioX;
  private static float ratioY;
  private static int w2;
  private static int finishCount;

  public static void Point(Texture2D tex, int newWidth, int newHeight)
  {
    TextureScale.ThreadedScale(tex, newWidth, newHeight, false);
  }

  public static void Bilinear(Texture2D tex, int newWidth, int newHeight)
  {
    TextureScale.ThreadedScale(tex, newWidth, newHeight, true);
  }

  private static void ThreadedScale(Texture2D tex, int newWidth, int newHeight, bool useBilinear)
  {
    TextureScale.texColors = tex.GetPixels();
    TextureScale.newColors = new Color[newWidth * newHeight];
    if (useBilinear)
    {
      TextureScale.ratioX = (float) (1.0 / ((double) newWidth / (double) (((Texture) tex).width - 1)));
      TextureScale.ratioY = (float) (1.0 / ((double) newHeight / (double) (((Texture) tex).height - 1)));
    }
    else
    {
      TextureScale.ratioX = (float) ((Texture) tex).width / (float) newWidth;
      TextureScale.ratioY = (float) ((Texture) tex).height / (float) newHeight;
    }
    TextureScale.w = ((Texture) tex).width;
    TextureScale.w2 = newWidth;
    int num1 = Mathf.Min(SystemInfo.processorCount, newHeight);
    int num2 = newHeight / num1;
    TextureScale.finishCount = 0;
    TextureScale.ThreadData threadData = new TextureScale.ThreadData(0, newHeight);
    if (useBilinear)
      TextureScale.BilinearScale((object) threadData);
    else
      TextureScale.PointScale((object) threadData);
    tex.Reinitialize(newWidth, newHeight);
    tex.SetPixels(TextureScale.newColors);
    tex.Apply();
    TextureScale.texColors = (Color[]) null;
    TextureScale.newColors = (Color[]) null;
  }

  public static void BilinearScale(object obj)
  {
    TextureScale.ThreadData threadData = (TextureScale.ThreadData) obj;
    for (int start = threadData.start; start < threadData.end; ++start)
    {
      int num1 = (int) Mathf.Floor((float) start * TextureScale.ratioY);
      int num2 = num1 * TextureScale.w;
      int num3 = (num1 + 1) * TextureScale.w;
      int num4 = start * TextureScale.w2;
      for (int index = 0; index < TextureScale.w2; ++index)
      {
        int num5 = (int) Mathf.Floor((float) index * TextureScale.ratioX);
        float num6 = (float) index * TextureScale.ratioX - (float) num5;
        TextureScale.newColors[num4 + index] = TextureScale.ColorLerpUnclamped(TextureScale.ColorLerpUnclamped(TextureScale.texColors[num2 + num5], TextureScale.texColors[num2 + num5 + 1], num6), TextureScale.ColorLerpUnclamped(TextureScale.texColors[num3 + num5], TextureScale.texColors[num3 + num5 + 1], num6), (float) start * TextureScale.ratioY - (float) num1);
      }
    }
    ++TextureScale.finishCount;
  }

  public static void PointScale(object obj)
  {
    TextureScale.ThreadData threadData = (TextureScale.ThreadData) obj;
    for (int start = threadData.start; start < threadData.end; ++start)
    {
      int num1 = (int) ((double) TextureScale.ratioY * (double) start) * TextureScale.w;
      int num2 = start * TextureScale.w2;
      for (int index = 0; index < TextureScale.w2; ++index)
        TextureScale.newColors[num2 + index] = TextureScale.texColors[(int) ((double) num1 + (double) TextureScale.ratioX * (double) index)];
    }
    ++TextureScale.finishCount;
  }

  private static Color ColorLerpUnclamped(Color c1, Color c2, float value)
  {
    return new Color(c1.r + (c2.r - c1.r) * value, c1.g + (c2.g - c1.g) * value, c1.b + (c2.b - c1.b) * value, c1.a + (c2.a - c1.a) * value);
  }

  public class ThreadData
  {
    public int start;
    public int end;

    public ThreadData(int s, int e)
    {
      this.start = s;
      this.end = e;
    }
  }
}
