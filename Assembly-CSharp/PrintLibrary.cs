// Decompiled with JetBrains decompiler
// Type: PrintLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PrintLibrary : MonoBehaviour
{
  private readonly Color _color_0 = Toolbox.makeColor("#FFFFFF");
  private readonly Color _color_1 = Toolbox.makeColor("#CCCCCC");
  private readonly Color _color_2 = Toolbox.makeColor("#7F7F7F");
  private readonly Color _color_3 = Toolbox.makeColor("#000000");
  public List<PrintTemplate> list;
  private readonly Dictionary<string, PrintTemplate> _dict = new Dictionary<string, PrintTemplate>();
  private readonly List<PrintTemplate> _list_quakes = new List<PrintTemplate>();
  private static PrintLibrary _instance;

  private void Awake()
  {
    PrintLibrary._instance = this;
    for (int index = 0; index < this.list.Count; ++index)
    {
      PrintTemplate printTemplate = this.list[index];
      this.calcSteps(printTemplate);
      this._dict.Add(printTemplate.name, printTemplate);
      if (printTemplate.name.Contains("quake"))
      {
        this._list_quakes.Add(printTemplate);
        this.addRotatedQuake(printTemplate, 90);
        this.addRotatedQuake(printTemplate, 180);
        this.addRotatedQuake(printTemplate, 360);
        this.addRotatedQuake(printTemplate, -360);
        this.addRotatedQuake(printTemplate, -90);
        this.addRotatedQuake(printTemplate, -180);
        this.addRotatedQuake(printTemplate, -270);
      }
    }
  }

  private void addRotatedQuake(PrintTemplate pOrigin, int pRotation)
  {
    PrintTemplate pPrint = new PrintTemplate();
    pPrint.name = $"{pOrigin.name}_{pRotation.ToString()}";
    Texture2D originTexture = Object.Instantiate<Texture2D>(pOrigin.graphics);
    pPrint.graphics = TextureRotator.Rotate(originTexture, pRotation, new Color32((byte) 0, (byte) 0, (byte) 0, (byte) 0));
    this.calcSteps(pPrint);
    this._list_quakes.Add(pPrint);
  }

  private void calcSteps(PrintTemplate pPrint)
  {
    List<PrintStep> printStepList = new List<PrintStep>();
    int width = ((Texture) pPrint.graphics).width;
    int height = ((Texture) pPrint.graphics).height;
    for (int index1 = 1; index1 < width - 1; ++index1)
    {
      for (int index2 = 1; index2 < height - 1; ++index2)
      {
        Color pixel = pPrint.graphics.GetPixel(index1, index2);
        if (!Color.op_Equality(pixel, this._color_0))
        {
          PrintStep printStep = new PrintStep()
          {
            x = index1 - 1 - width / 2,
            y = index2 - 1 - height / 2,
            action = 1
          };
          printStepList.Add(printStep);
          if (Color.op_Equality(pixel, this._color_2))
            printStepList.Add(printStep);
          else if (Color.op_Equality(pixel, this._color_3))
          {
            printStepList.Add(printStep);
            printStepList.Add(printStep);
          }
        }
      }
    }
    pPrint.steps = printStepList.ToArray();
    pPrint.steps_per_tick = (int) ((double) pPrint.steps.Length * 0.004999999888241291 + 1.0);
  }

  public static PrintTemplate getTemplate(string pTemplateID)
  {
    return PrintLibrary._instance._dict[pTemplateID];
  }

  public static List<PrintTemplate> getQuakes() => PrintLibrary._instance._list_quakes;
}
