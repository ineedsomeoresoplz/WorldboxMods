// Decompiled with JetBrains decompiler
// Type: BaseDebugAnimationElement`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class BaseDebugAnimationElement<TAsset> : MonoBehaviour where TAsset : Asset
{
  protected TAsset asset;
  public Button play_pause_button;
  public Image play_pause_icon;
  public Sprite sprite_play;
  public Sprite sprite_pause;
  public Button frame_number_button;
  public Text frame_number_text;
  protected bool is_playing;

  protected virtual void Start()
  {
    // ISSUE: method pointer
    ((UnityEvent) this.play_pause_button.onClick).AddListener(new UnityAction((object) this, __methodptr(clickToggleState)));
    Button.ButtonClickedEvent onClick = this.frame_number_button.onClick;
    BaseDebugAnimationElement<TAsset> animationElement = this;
    // ISSUE: virtual method pointer
    UnityAction unityAction = new UnityAction((object) animationElement, __vmethodptr(animationElement, clickNextFrame));
    ((UnityEvent) onClick).AddListener(unityAction);
  }

  public virtual void update() => throw new NotImplementedException();

  public virtual void setData(TAsset pAsset)
  {
    this.asset = pAsset;
    this.clear();
    this.is_playing = true;
  }

  protected virtual void clear() => throw new NotImplementedException();

  public virtual void stopAnimations()
  {
    this.is_playing = false;
    this.checkButtons();
  }

  public virtual void startAnimations()
  {
    this.is_playing = true;
    this.checkButtons();
  }

  private void clickToggleState()
  {
    this.is_playing = !this.is_playing;
    if (this.is_playing)
      this.startAnimations();
    else
      this.stopAnimations();
  }

  private void checkButtons()
  {
    if (this.is_playing)
    {
      this.play_pause_icon.sprite = this.sprite_pause;
      ((Selectable) this.frame_number_button).interactable = false;
    }
    else
    {
      this.play_pause_icon.sprite = this.sprite_play;
      ((Selectable) this.frame_number_button).interactable = true;
    }
  }

  protected virtual void clickNextFrame() => throw new NotImplementedException();
}
