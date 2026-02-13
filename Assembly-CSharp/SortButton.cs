// Decompiled with JetBrains decompiler
// Type: SortButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class SortButton : MonoBehaviour
{
  public Image arrow_sprite;
  public Image icon;
  public Image background;
  private SortButtonState _state;
  public SortButtonAction action;
  public SortButtonAction post_action;
  public SortButtonClearAction select_action;
  protected static Sprite _tab_button_on;
  protected static Sprite _tab_button_off;

  private void Awake()
  {
    if (Object.op_Equality((Object) SortButton._tab_button_on, (Object) null))
    {
      SortButton._tab_button_on = SpriteTextureLoader.getSprite("ui/tab_button_sort_selected");
      SortButton._tab_button_off = SpriteTextureLoader.getSprite("ui/tab_button_sort");
    }
    ((Component) this.arrow_sprite).gameObject.SetActive(false);
    this.setState(SortButtonState.None);
    this.background.sprite = SortButton._tab_button_off;
  }

  private void Start()
  {
    // ISSUE: method pointer
    ((UnityEvent) ((Component) this).GetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(click)));
  }

  public SortButtonState getState() => this._state;

  private void setState(SortButtonState pState) => this._state = pState;

  internal void turnOff()
  {
    this.setState(SortButtonState.None);
    ((Component) this.arrow_sprite).gameObject.SetActive(false);
    this.background.sprite = SortButton._tab_button_off;
    Color white = Color.white;
    white.a = 0.5f;
    ((Graphic) this.icon).color = white;
    ((Component) ((Component) this).transform.parent).GetComponent<RectTransform>().sizeDelta = new Vector2(27f, 37f);
  }

  public void click()
  {
    SortButtonClearAction selectAction = this.select_action;
    if (selectAction != null)
      selectAction(this);
    switch (this._state)
    {
      case SortButtonState.None:
        this.setSortUP();
        break;
      case SortButtonState.Up:
        this.setSortDOWN();
        break;
      case SortButtonState.Down:
        this.setSortUP();
        break;
    }
    SortButtonAction action = this.action;
    if (action != null)
      action();
    SortButtonAction postAction = this.post_action;
    if (postAction == null)
      return;
    postAction();
  }

  public void callAction()
  {
  }

  public void setSortUP()
  {
    this.setState(SortButtonState.Up);
    ((Component) this.arrow_sprite).gameObject.SetActive(true);
    this.arrow_sprite.sprite = SpriteTextureLoader.getSprite("ui/Icons/iconArrowUP");
    this.background.sprite = SortButton._tab_button_on;
    ((Graphic) this.icon).color = Color.white;
    ((Component) ((Component) this).transform.parent).GetComponent<RectTransform>().sizeDelta = new Vector2(33f, 37f);
  }

  public void setSortDOWN()
  {
    this.setState(SortButtonState.Down);
    ((Component) this.arrow_sprite).gameObject.SetActive(true);
    this.arrow_sprite.sprite = SpriteTextureLoader.getSprite("ui/Icons/iconArrowDOWN");
    this.background.sprite = SortButton._tab_button_on;
    ((Graphic) this.icon).color = Color.white;
    ((Component) ((Component) this).transform.parent).GetComponent<RectTransform>().sizeDelta = new Vector2(33f, 37f);
  }
}
