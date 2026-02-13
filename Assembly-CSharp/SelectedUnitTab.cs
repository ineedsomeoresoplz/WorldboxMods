// Decompiled with JetBrains decompiler
// Type: SelectedUnitTab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SelectedUnitTab : SelectedNano<Actor>
{
  [SerializeField]
  private Image _task_icon_left;
  [SerializeField]
  private Image _task_icon_right;
  [SerializeField]
  private Text _task_title;
  [SerializeField]
  private Sprite _no_task_icon;
  [SerializeField]
  private StatBar _bar_health;
  [SerializeField]
  private StatBar _bar_mana;
  [SerializeField]
  private StatBar _bar_stamina;
  [SerializeField]
  private StatBar _bar_hunger;
  [SerializeField]
  private StatBar _bar_happiness;
  [SerializeField]
  private HappinessBarIcon _bar_happiness_icon;
  [SerializeField]
  private UiUnitAvatarElement _avatar_element;
  [SerializeField]
  private UnitTextManager _avatar_text;
  [SerializeField]
  private SwitchStateButton _button_traits_editor;
  [SerializeField]
  private SwitchStateButton _button_equipment_editor;
  [SerializeField]
  private SwitchStateButton _button_mind;
  [SerializeField]
  private SwitchStateButton _button_genealogy;
  [SerializeField]
  private SwitchStateButton _button_plots_tab;
  [SerializeField]
  private Image _button_equipment_editor_icon;
  [SerializeField]
  private Sprite _button_equipment_editor_sprite_normal;
  [SerializeField]
  private Sprite _button_equipment_editor_sprite_firearm;
  [SerializeField]
  private ActorSelectedContainerStatus _container_status;
  [SerializeField]
  private ActorSelectedContainerEquipment _container_equipment;
  [SerializeField]
  private DragSnapElement _drag_snap_avatar;
  private bool _requested_buttons_update;

  private void Start()
  {
    SelectedUnit.subscribeClearEvent(new SelectedUnitClearEvent(((SelectedNano<Actor>) this).clearLastObject));
  }

  private void OnEnable()
  {
    if (Object.op_Equality((Object) this._drag_snap_avatar, (Object) null))
      return;
    if (InputHelpers.mouseSupported)
      ((Behaviour) this._drag_snap_avatar).enabled = true;
    else
      ((Behaviour) this._drag_snap_avatar).enabled = false;
  }

  private void LateUpdate()
  {
    if (!this._requested_buttons_update)
      return;
    this._requested_buttons_update = false;
    PowerTabController.instance.tab_selected_unit.findNeighbours(true);
  }

  protected override void updateElementsOnChange(Actor pActor)
  {
    base.updateElementsOnChange(pActor);
    pActor.asset.unlock(true);
    this.updateButtons(pActor);
    this.showStatsGeneral(pActor);
    this.updateStatuses(pActor);
    this.updateEquipment(pActor);
  }

  protected override void updateElementsAlways(Actor pActor)
  {
    this.showTask(pActor);
    this.showStatBars(pActor);
    this.checkAvatar(pActor);
    this.setIconValue("i_age", (float) pActor.getAge());
    base.updateElementsAlways(pActor);
  }

  protected override void checkAchievements(Actor pActor)
  {
    AchievementLibrary.checkUnitAchievements(pActor);
  }

  private void updateButtons(Actor pActor)
  {
    ((Component) this._button_equipment_editor).gameObject.SetActive(pActor.asset.can_edit_equipment);
    ((Component) this._button_mind).gameObject.SetActive(pActor.asset.inspect_mind);
    ((Component) this._button_traits_editor).gameObject.SetActive(pActor.asset.can_edit_traits);
    ((Component) this._button_genealogy).gameObject.SetActive(pActor.asset.inspect_genealogy);
    ((Component) this._button_plots_tab).gameObject.SetActive(pActor.isKing() || pActor.isCityLeader() || pActor.hasClan());
    this._button_equipment_editor_icon.sprite = pActor.isWeaponFirearm() ? this._button_equipment_editor_sprite_firearm : this._button_equipment_editor_sprite_normal;
    this._requested_buttons_update = true;
  }

  private void updateStatuses(Actor pActor) => this._container_status.update((NanoObject) pActor);

  private void updateEquipment(Actor pActor) => this._container_equipment.update(pActor);

  protected override void showStatsGeneral(Actor pActor)
  {
    this.setIconValue("i_renown", (float) pActor.renown);
    this.setIconValue("i_level", (float) pActor.level);
    this.setIconValue("i_experience", (float) pActor.data.experience, new float?((float) pActor.getExpToLevelup()));
    this.setIconValue("i_money", (float) pActor.money);
    this.setIconValue("i_loot", (float) pActor.loot);
    this.setIconValue("i_kills", (float) pActor.data.kills);
    this.setIconValue("i_children", (float) pActor.current_children_count);
    int stat = (int) pActor.stats["damage"];
    this.setIconValue("i_damage", (float) (int) ((double) stat * (double) pActor.stats["damage_range"]), new float?((float) stat), pSeparator: '-');
    this.name_field.text = pActor.getName();
    ((Graphic) this.name_field).color = pActor.kingdom.getColor().getColorText();
    if (pActor.asset.is_boat)
      this.icon_right.sprite = pActor.asset.getSpriteIcon();
    else if (pActor.isSexMale())
      this.icon_right.sprite = SpriteTextureLoader.getSprite("ui/icons/IconMale");
    else
      this.icon_right.sprite = SpriteTextureLoader.getSprite("ui/icons/IconFemale");
    this.icon_left.sprite = !pActor.asset.is_boat || !pActor.hasCity() ? pActor.asset.getSpriteIcon() : pActor.city.getSpriteIcon();
  }

  private void checkAvatar(Actor pActor)
  {
    if (this.isNanoChanged(pActor) || this._avatar_element.avatarLoader.actorStateChanged())
      this._avatar_element.show(pActor);
    else
      this._avatar_element.updateTileSprite();
  }

  private void showStatAvatar(Actor pActor) => this._avatar_element.show(pActor);

  private void showTask(Actor pActor)
  {
    BehaviourTaskActor task = pActor.ai.task;
    string taskText = pActor.getTaskText();
    Sprite sprite = task != null ? task.getSprite() : this._no_task_icon;
    this._task_icon_left.sprite = sprite;
    this._task_icon_right.sprite = sprite;
    this._task_title.text = taskText;
  }

  private void showStatBars(Actor pActor)
  {
    float health = (float) pActor.getHealth();
    float maxHealth = (float) pActor.getMaxHealth();
    this._bar_health.setBar(health, maxHealth, "/" + ((int) maxHealth).ToText(4), false, pSpeed: 0.25f);
    bool pShow1 = pActor.hasEmotions();
    this._bar_happiness_icon.load(pActor);
    this.checkShowProgressBar(this._bar_happiness, "%", (float) pActor.getHappinessPercent(), 100f, pShow1);
    bool pShow2 = pActor.needsFood();
    this.checkShowProgressBar(this._bar_hunger, "%", (float) ((double) pActor.getNutrition() / (double) pActor.getMaxNutrition() * 100.0), 100f, pShow2);
    bool pShow3 = !pActor.asset.force_hide_stamina;
    int maxStamina = pActor.getMaxStamina();
    float pCurrentValue1 = (float) Mathf.Clamp(pActor.getStamina(), 0, maxStamina);
    this.checkShowProgressBar(this._bar_stamina, "/" + maxStamina.ToText(), pCurrentValue1, (float) maxStamina, pShow3);
    bool pShow4 = !pActor.asset.force_hide_mana;
    int maxMana = pActor.getMaxMana();
    float pCurrentValue2 = (float) Mathf.Clamp(pActor.getMana(), 0, maxMana);
    this.checkShowProgressBar(this._bar_mana, "/" + maxMana.ToText(), pCurrentValue2, (float) maxMana, pShow4);
  }

  protected override string getPowerTabAssetID() => "selected_unit";

  private void checkShowProgressBar(
    StatBar pBar,
    string pEnding,
    float pCurrentValue,
    float pMax,
    bool pShow)
  {
    ((Component) pBar).gameObject.SetActive(pShow);
    if (!pShow)
      return;
    pBar.setBar(pCurrentValue, pMax, pEnding, false, pSpeed: 0.25f);
  }

  public void avatarTouchScream()
  {
    if (SelectedUnit.isSet())
    {
      this.nano_object.pokeFromAvatarUI();
      World.world.locatePosition(Vector2.op_Implicit(this.nano_object.current_position));
    }
    this._avatar_text.spawnAvatarText();
    ToolbarButtons.instance.shake();
  }
}
