// Decompiled with JetBrains decompiler
// Type: TabToggle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class TabToggle : MonoBehaviour
{
  public Image icon;
  public Image background;
  private TabToggleState _state;
  public TabToggleAction action;
  public TabToggleAction post_action;
  public TabToggleClearAction select_action;
  protected static Sprite _tab_toggle_on;
  protected static Sprite _tab_toggle_off;

  private void Awake()
  {
    if (Object.op_Equality((Object) TabToggle._tab_toggle_on, (Object) null))
    {
      TabToggle._tab_toggle_on = SpriteTextureLoader.getSprite("ui/tab_button_sort_selected");
      TabToggle._tab_toggle_off = SpriteTextureLoader.getSprite("ui/tab_button_sort");
    }
    this.unselect();
  }

  private void Start()
  {
    // ISSUE: method pointer
    ((UnityEvent) ((Component) this).GetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(click)));
  }

  public TabToggleState getState() => this._state;

  private void setState(TabToggleState pState) => this._state = pState;

  public void click()
  {
    if (this._state == TabToggleState.Selected)
      return;
    TabToggleClearAction selectAction = this.select_action;
    if (selectAction != null)
      selectAction(this);
    this.select();
    TabToggleAction action = this.action;
    if (action != null)
      action();
    TabToggleAction postAction = this.post_action;
    if (postAction == null)
      return;
    postAction();
  }

  public void select()
  {
    this.setState(TabToggleState.Selected);
    this.background.sprite = TabToggle._tab_toggle_on;
    ((Graphic) this.icon).color = Color.white;
  }

  public void unselect()
  {
    this.setState(TabToggleState.None);
    this.background.sprite = TabToggle._tab_toggle_off;
    Color white = Color.white;
    white.a = 0.5f;
    ((Graphic) this.icon).color = white;
  }
}
