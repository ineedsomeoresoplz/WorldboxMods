// Decompiled with JetBrains decompiler
// Type: BrushSelectButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class BrushSelectButton : MonoBehaviour
{
  public Image icon;
  private BrushData _brush_asset;

  private void Start()
  {
    // ISSUE: method pointer
    ((UnityEvent) ((Component) this).GetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Eb__2_0)));
  }

  public void setup(BrushData pBrushData)
  {
    this._brush_asset = pBrushData;
    ((Object) ((Component) this).gameObject).name = this._brush_asset.id;
    this._brush_asset.setupImage(this.icon);
    ((Component) this).GetComponent<TipButton>().textOnClick = this._brush_asset.getLocaleID();
  }
}
