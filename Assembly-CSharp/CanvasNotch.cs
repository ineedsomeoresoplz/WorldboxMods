// Decompiled with JetBrains decompiler
// Type: CanvasNotch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (RectTransform))]
public class CanvasNotch : MonoBehaviour
{
  private bool screenChangeVarsInitialized;
  private bool ranFirstTime;
  private ScreenOrientation lastOrientation = (ScreenOrientation) 5;
  private Vector2 lastResolution = Vector2.zero;
  private Rect lastSafeArea = Rect.zero;
  private Rect lastCanvasRect = Rect.zero;
  private RectTransform safeAreaTransform;
  private Canvas _canvas;

  private void Awake()
  {
    this._canvas = ((Component) ((Component) this).gameObject.transform).GetComponentInParent<Canvas>();
    this.safeAreaTransform = ((Component) this).GetComponent<RectTransform>();
    if (this.screenChangeVarsInitialized)
      return;
    this.lastOrientation = Screen.orientation;
    this.lastResolution.x = (float) Screen.width;
    this.lastResolution.y = (float) Screen.height;
    this.lastSafeArea = Screen.safeArea;
    this.screenChangeVarsInitialized = true;
  }

  private void Start() => this.ApplySafeArea();

  private void Update()
  {
    if (Application.isMobilePlatform && Screen.orientation != this.lastOrientation)
      this.OrientationChanged();
    if (Rect.op_Inequality(Screen.safeArea, this.lastSafeArea))
      this.SafeAreaChanged();
    if (Object.op_Inequality((Object) this._canvas, (Object) null) && Rect.op_Inequality(this._canvas.pixelRect, this.lastCanvasRect))
      this.CanvasChanged();
    if ((double) Screen.width != (double) this.lastResolution.x || (double) Screen.height != (double) this.lastResolution.y)
      this.ResolutionChanged();
    if (this.ranFirstTime)
      return;
    this.ApplySafeArea();
  }

  private void ApplySafeArea()
  {
    if (Object.op_Equality((Object) this._canvas, (Object) null) || Object.op_Equality((Object) this.safeAreaTransform, (Object) null))
      return;
    this.ranFirstTime = true;
    Rect safeArea = Screen.safeArea;
    Rect rect;
    // ISSUE: explicit constructor call
    ((Rect) ref rect).\u002Ector(0.0f, 0.0f, (float) Screen.width, (float) Screen.height);
    Vector2 vector2_1 = Vector2.op_Subtraction(((Rect) ref safeArea).min, ((Rect) ref rect).min);
    Vector2 vector2_2 = Vector2.op_Subtraction(((Rect) ref safeArea).max, ((Rect) ref rect).max);
    ref Rect local1 = ref safeArea;
    ((Rect) ref local1).min = Vector2.op_Subtraction(((Rect) ref local1).min, vector2_2);
    ref Rect local2 = ref safeArea;
    ((Rect) ref local2).max = Vector2.op_Subtraction(((Rect) ref local2).max, vector2_1);
    Vector2 position = ((Rect) ref safeArea).position;
    Vector2 vector2_3 = Vector2.op_Addition(((Rect) ref safeArea).position, ((Rect) ref safeArea).size);
    ref float local3 = ref position.x;
    double num1 = (double) local3;
    Rect pixelRect1 = this._canvas.pixelRect;
    double width1 = (double) ((Rect) ref pixelRect1).width;
    local3 = (float) (num1 / width1);
    ref float local4 = ref position.y;
    double num2 = (double) local4;
    Rect pixelRect2 = this._canvas.pixelRect;
    double height1 = (double) ((Rect) ref pixelRect2).height;
    local4 = (float) (num2 / height1);
    ref float local5 = ref vector2_3.x;
    double num3 = (double) local5;
    Rect pixelRect3 = this._canvas.pixelRect;
    double width2 = (double) ((Rect) ref pixelRect3).width;
    local5 = (float) (num3 / width2);
    ref float local6 = ref vector2_3.y;
    double num4 = (double) local6;
    Rect pixelRect4 = this._canvas.pixelRect;
    double height2 = (double) ((Rect) ref pixelRect4).height;
    local6 = (float) (num4 / height2);
    this.safeAreaTransform.anchorMin = position;
    this.safeAreaTransform.anchorMax = vector2_3;
  }

  private void OrientationChanged()
  {
    this.lastOrientation = Screen.orientation;
    this.lastResolution.x = (float) Screen.width;
    this.lastResolution.y = (float) Screen.height;
    this.ApplySafeArea();
  }

  private void ResolutionChanged()
  {
    this.lastResolution.x = (float) Screen.width;
    this.lastResolution.y = (float) Screen.height;
    this.ApplySafeArea();
  }

  private void SafeAreaChanged()
  {
    this.lastSafeArea = Screen.safeArea;
    this.ApplySafeArea();
  }

  private void CanvasChanged()
  {
    this.lastCanvasRect = this._canvas.pixelRect;
    this.ApplySafeArea();
  }

  private void debugConsole()
  {
    Dictionary<string, Rect> dictionary = new Dictionary<string, Rect>();
    Debug.Log((object) ("amount of cutouts: " + Screen.cutouts.Length.ToString()));
    dictionary["screen"] = new Rect(0.0f, 0.0f, (float) Screen.width, (float) Screen.height);
    dictionary["safearea"] = Screen.safeArea;
    foreach (string key in dictionary.Keys)
    {
      string[] strArray = new string[10];
      strArray[0] = "[o] ";
      strArray[1] = key;
      strArray[2] = ": x:";
      Rect rect1 = dictionary[key];
      float num = ((Rect) ref rect1).x;
      strArray[3] = num.ToString();
      strArray[4] = ", y:";
      Rect rect2 = dictionary[key];
      num = ((Rect) ref rect2).y;
      strArray[5] = num.ToString();
      strArray[6] = ", w:";
      Rect rect3 = dictionary[key];
      num = ((Rect) ref rect3).width;
      strArray[7] = num.ToString();
      strArray[8] = ", h:";
      Rect rect4 = dictionary[key];
      num = ((Rect) ref rect4).height;
      strArray[9] = num.ToString();
      Debug.Log((object) string.Concat(strArray));
    }
    if (Object.op_Equality((Object) this._canvas, (Object) null))
    {
      Debug.Log((object) "canvas not ready");
    }
    else
    {
      foreach (string key in dictionary.Keys)
      {
        string[] strArray = new string[10];
        strArray[0] = "[c] ";
        strArray[1] = key;
        strArray[2] = ": x:";
        Rect rect5 = dictionary[key];
        float num = ((Rect) ref rect5).x / this._canvas.scaleFactor;
        strArray[3] = num.ToString();
        strArray[4] = ", y:";
        Rect rect6 = dictionary[key];
        num = ((Rect) ref rect6).y / this._canvas.scaleFactor;
        strArray[5] = num.ToString();
        strArray[6] = ", w:";
        Rect rect7 = dictionary[key];
        num = ((Rect) ref rect7).width / this._canvas.scaleFactor;
        strArray[7] = num.ToString();
        strArray[8] = ", h:";
        Rect rect8 = dictionary[key];
        num = ((Rect) ref rect8).height / this._canvas.scaleFactor;
        strArray[9] = num.ToString();
        Debug.Log((object) string.Concat(strArray));
      }
    }
  }
}
