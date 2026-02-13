// Decompiled with JetBrains decompiler
// Type: BrushSizesButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BrushSizesButton : MonoBehaviour
{
  private PowerButton _power_button;
  private string _latest_used = string.Empty;

  private void Awake() => this._power_button = ((Component) this).GetComponent<PowerButton>();

  private void Update()
  {
    if (!(Config.current_brush != this._latest_used))
      return;
    this._latest_used = Config.current_brush;
    BrushData brushData = Brush.get(Config.current_brush);
    if (brushData == null)
      Debug.LogError((object) (Config.current_brush + " is not a valid brush"));
    else
      brushData.setupImage(this._power_button.icon);
  }
}
