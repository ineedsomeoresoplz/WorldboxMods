// Decompiled with JetBrains decompiler
// Type: HistoryHudItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class HistoryHudItem : MonoBehaviour
{
  private bool _creating = true;
  private float _remove_timer = 8f;
  private CanvasGroup _canvas_group;
  private Button _button;
  private WorldLogMessage _message;
  public Text textField;
  public Image icon;
  private RectTransform _rect_transform;
  public Image background;
  private bool _removing;
  private HistoryHud _history_hud;
  private float _time_limit;
  internal float target_bottom;

  private void Start()
  {
    this._history_hud = ((Component) this).GetComponentInParent<HistoryHud>();
    this._canvas_group = ((Component) this).GetComponent<CanvasGroup>();
    this._canvas_group.alpha = 0.0f;
    this._button = ((Component) this).GetComponent<Button>();
    this._rect_transform = ((Component) this).GetComponent<RectTransform>();
    // ISSUE: method pointer
    ((UnityEvent) this._button.onClick).AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Eb__13_0)));
  }

  private void OnEnable()
  {
    this._creating = true;
    this._remove_timer = 8f;
    this._removing = false;
    ((Component) this).GetComponent<CanvasGroup>().alpha = 0.0f;
  }

  public bool isRemoving() => this._removing;

  public void setMessage(WorldLogMessage pMessage)
  {
    this.textField.text = pMessage.getFormatedText(this.textField);
    ((Component) this.textField).GetComponent<LocalizedText>().checkTextFont();
    ((Component) this.textField).GetComponent<LocalizedText>().checkSpecialLanguages();
    if (pMessage.getAsset().path_icon != "")
      this.icon.sprite = SpriteTextureLoader.getSprite(pMessage.getAsset().path_icon);
    else
      ((Component) this.icon).gameObject.SetActive(false);
    this._message = pMessage;
  }

  public void moveTo(float newBottom)
  {
    this._time_limit = 0.0f;
    this.target_bottom = newBottom;
  }

  public void moveToAndDestroy(float newBottom)
  {
    this._time_limit = 0.0f;
    this.target_bottom = newBottom;
    this._remove_timer = 0.5f;
    this._removing = true;
  }

  private void Update()
  {
    ((Graphic) this.background).raycastTarget = this._history_hud.raycastOn;
    this._rect_transform.sizeDelta = new Vector2(this._rect_transform.sizeDelta.x, 10f);
    if (this._creating)
    {
      if ((double) this._canvas_group.alpha < 1.0)
        this._canvas_group.alpha += (float) ((double) Time.deltaTime * (double) Config.time_scale_asset.multiplier * 2.0);
      else
        this._creating = false;
    }
    else
    {
      if (Config.paused || ScrollWindow.isWindowActive() || RewardedAds.isShowing())
        return;
      if ((double) this._time_limit <= 2.0)
      {
        this._time_limit += Time.deltaTime;
        this._rect_transform.SetTop(-Mathf.Lerp(this._rect_transform.offsetMax.y, -this.target_bottom, this._time_limit / 2f));
      }
      if (this._removing && (double) this._rect_transform.offsetMax.y > 10.0)
      {
        this._history_hud.makeInactive(this);
      }
      else
      {
        this._remove_timer -= Time.deltaTime;
        if ((double) this._remove_timer > 0.0)
          return;
        this._canvas_group.alpha -= Time.deltaTime * 2f;
        if ((double) this._canvas_group.alpha > 0.0)
          return;
        this._history_hud.makeInactive(this);
      }
    }
  }

  private void OnDisable() => this._message.clear();
}
