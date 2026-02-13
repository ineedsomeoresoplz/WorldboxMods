// Decompiled with JetBrains decompiler
// Type: UiWorldAgeInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UiWorldAgeInfo : MonoBehaviour
{
  [SerializeField]
  private Sprite _random_age_sprite;
  [SerializeField]
  private Image _icon_age_left;
  [SerializeField]
  private Image _icon_age_right;
  [SerializeField]
  private Image _icon_age_next_left;
  [SerializeField]
  private Image _icon_age_next_right;
  [SerializeField]
  private Image _icon_clock_left;
  [SerializeField]
  private Image _icon_clock_right;
  [SerializeField]
  private Text _text_age_title;
  [SerializeField]
  private Text _text_year;
  [SerializeField]
  private Text _text_month;
  [SerializeField]
  private StatBar _bar_age;
  [SerializeField]
  private StatBar _bar_year;
  [SerializeField]
  private Sprite _sprite_play;
  [SerializeField]
  private Sprite _sprite_pause;
  private LocalizedText _text_age_title_localized;
  private LocalizedText _text_year_localized;
  private LocalizedText _text_month_localized;
  private LocalizedText _bar_age_localized;

  private void Awake()
  {
    this._text_age_title_localized = ((Component) this._text_age_title).GetComponent<LocalizedText>();
    this._text_year_localized = ((Component) this._text_year).GetComponent<LocalizedText>();
    this._text_month_localized = ((Component) this._text_month).GetComponent<LocalizedText>();
    this._bar_age_localized = ((Component) this._bar_age.textField).GetComponent<LocalizedText>();
  }

  private void Update()
  {
    if (!Config.game_loaded || Object.op_Equality((Object) World.world, (Object) null) || World.world_era == null)
      return;
    WorldAgeManager eraManager = World.world.era_manager;
    Sprite sprite = eraManager.isPaused() ? this._sprite_pause : this._sprite_play;
    this._icon_clock_left.sprite = sprite;
    this._icon_clock_right.sprite = sprite;
    this._icon_age_left.sprite = World.world_era.getSprite();
    this._icon_age_right.sprite = World.world_era.getSprite();
    ((Graphic) this._text_age_title).color = World.world_era.title_color;
    this._text_age_title_localized.setKeyAndUpdate(World.world_era.getLocaleID());
    MapStats mapStats = World.world.map_stats;
    this._text_year_localized.setKeyAndUpdate("year_era");
    this._text_month_localized.setKeyAndUpdate(AssetManager.months.getMonth(Date.getCurrentMonth()).getLocaleID());
    this._bar_year.setBar((float) ((double) Date.getCurrentMonth() + (double) Date.getMonthTime() / 5.0 - 1.0), 12f, "", false, true, false);
    this._bar_age.setBar(mapStats.current_age_progress, 1f, "", false, true, false);
    ((Component) this._bar_age).gameObject.SetActive(true);
    if (eraManager.isPaused())
      this._bar_age_localized.setKeyAndUpdate("ages_paused");
    else
      this._bar_age_localized.setKeyAndUpdate("next_age_in_moons");
    WorldAgeAsset nextAge = World.world.era_manager.getNextAge();
    if (nextAge == null)
    {
      this._icon_age_next_left.sprite = this._random_age_sprite;
      this._icon_age_next_right.sprite = this._random_age_sprite;
    }
    else
    {
      this._icon_age_next_left.sprite = nextAge.getSprite();
      this._icon_age_next_right.sprite = nextAge.getSprite();
    }
  }
}
