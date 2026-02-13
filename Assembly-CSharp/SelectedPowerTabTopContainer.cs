// Decompiled with JetBrains decompiler
// Type: SelectedPowerTabTopContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SelectedPowerTabTopContainer : MonoBehaviour
{
  [SerializeField]
  private Image _icon_unit;
  public bool goUp;
  public bool goDown;
  private bool _going_down;
  private bool _going_up;
  private RectTransform _rect;
  private float _timer;

  private void Awake() => this._rect = ((Component) this).GetComponent<RectTransform>();

  private void Update()
  {
    if (SelectedUnit.isSet())
    {
      Actor unit = SelectedUnit.unit;
      if (!unit.isRekt())
        this._icon_unit.sprite = unit.asset.getSpriteIcon();
    }
    else if (SelectedObjects.isNanoObjectSet())
    {
      NanoObject selectedNanoObject = SelectedObjects.getSelectedNanoObject();
      if (!selectedNanoObject.isRekt())
      {
        MetaTypeAsset metaTypeAsset = selectedNanoObject.getMetaTypeAsset();
        if (metaTypeAsset.set_icon_for_cancel_button)
          this._icon_unit.sprite = metaTypeAsset.getIconSprite();
      }
    }
    if (this.goDown != this._going_down)
    {
      this._going_down = this.goDown;
      this._timer = 0.0f;
      if (this.goDown)
        this._timer = 0.95f;
    }
    if (this.goUp != this._going_up)
    {
      this._going_up = this.goUp;
      this._timer = -1f;
    }
    if ((double) this._timer >= 1.0)
      return;
    this._timer += Time.deltaTime / 2f;
    this._timer = Mathf.Clamp(this._timer, 0.0f, 1f);
  }

  public void cancelSelection()
  {
    SelectedUnit.clear();
    SelectedObjects.unselectNanoObject();
    PowersTab.unselect();
  }
}
