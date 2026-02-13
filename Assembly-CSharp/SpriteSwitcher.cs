// Decompiled with JetBrains decompiler
// Type: SpriteSwitcher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SpriteSwitcher : MonoBehaviour
{
  public Sprite primary_sprite;
  public Sprite secondary_sprite;
  private Image _image;
  private Image _icon;
  private bool? _last_state;
  private static List<SpriteSwitcher> _all_buttons = new List<SpriteSwitcher>();

  private void Awake()
  {
    this._image = ((Component) this).GetComponent<Image>();
    this._icon = ((Component) ((Component) this).transform.Find("Icon")).GetComponent<Image>();
  }

  private void OnEnable()
  {
    SpriteSwitcher._all_buttons.Add(this);
    this.checkState();
  }

  private void OnDisable() => SpriteSwitcher._all_buttons.Remove(this);

  public static void checkAllStates()
  {
    foreach (SpriteSwitcher allButton in SpriteSwitcher._all_buttons)
      allButton.checkState();
  }

  private void checkState()
  {
    if (!Config.game_loaded || !((Component) this).gameObject.activeInHierarchy)
      return;
    bool flag1 = this.hasAny();
    bool? lastState = this._last_state;
    bool flag2 = flag1;
    if (lastState.GetValueOrDefault() == flag2 & lastState.HasValue)
      return;
    this._last_state = new bool?(flag1);
    if (flag1)
      this.setPrimary();
    else
      this.setSecondary();
  }

  protected virtual bool hasAny() => throw new NotImplementedException();

  private void setPrimary()
  {
    this._image.sprite = this.primary_sprite;
    Color color = ((Graphic) this._icon).color;
    color.a = 1f;
    ((Graphic) this._icon).color = color;
  }

  private void setSecondary()
  {
    this._image.sprite = this.secondary_sprite;
    Color color = ((Graphic) this._icon).color;
    color.a = 0.9f;
    ((Graphic) this._icon).color = color;
  }
}
