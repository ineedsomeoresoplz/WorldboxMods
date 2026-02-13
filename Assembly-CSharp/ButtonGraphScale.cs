// Decompiled with JetBrains decompiler
// Type: ButtonGraphScale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class ButtonGraphScale : MonoBehaviour
{
  public Sprite sprite_on;
  public Sprite sprite_off;
  public GraphTimeScale button_scale;
  private GraphTimeScaleContainer _main_container;
  private GraphController _graph_controller;
  private Image _image;

  private void Awake()
  {
    // ISSUE: method pointer
    ((UnityEvent) ((Component) this).GetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(setScale)));
    this._image = ((Component) this).GetComponent<Image>();
    this._main_container = ((Component) this).GetComponentInParent<GraphTimeScaleContainer>();
    this._graph_controller = ((Component) ((Component) this).transform.parent.parent).GetComponentInChildren<GraphController>();
    this.checkSpriteStatus();
  }

  private void Update() => this.checkSpriteStatus();

  private void checkSpriteStatus()
  {
    if (this._main_container.current_scale == this.button_scale)
      this._image.sprite = this.sprite_on;
    else
      this._image.sprite = this.sprite_off;
  }

  public void setScale()
  {
    this._main_container.setTimeScale(this.button_scale);
    this._graph_controller.forceUpdateGraph();
  }
}
