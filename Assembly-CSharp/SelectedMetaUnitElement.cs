// Decompiled with JetBrains decompiler
// Type: SelectedMetaUnitElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SelectedMetaUnitElement : MonoBehaviour
{
  [SerializeField]
  private UiUnitAvatarElement _avatar;
  [SerializeField]
  private Text _title;
  [SerializeField]
  private UnitTextManager _unit_texts;
  [SerializeField]
  private Image _icon_sex;
  [SerializeField]
  private Image _icon_species;
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
  private Dictionary<string, StatsIcon> _stats_icons = new Dictionary<string, StatsIcon>();
  private Actor _actor;

  private void Awake()
  {
    foreach (StatsIcon componentsInChild in ((Component) this).GetComponentsInChildren<StatsIcon>(true))
    {
      if (!this._stats_icons.TryAdd(((Object) componentsInChild).name, componentsInChild))
        Debug.LogError((object) ("Duplicate icon name! " + ((Object) componentsInChild).name));
    }
  }

  public void show(Actor pActor, string pLocaleKey)
  {
    this._actor = pActor;
    this._avatar.show(this._actor);
    this._title.text = !string.IsNullOrEmpty(pLocaleKey) ? LocalizedTextManager.getText(pLocaleKey).Replace("$unit$", this._actor.getName()) : this._actor.getName();
    ((Graphic) this._title).color = this._actor.kingdom.getColor().getColorText();
    this._icon_sex.sprite = !this._actor.isSexMale() ? SpriteTextureLoader.getSprite("ui/icons/IconFemale") : SpriteTextureLoader.getSprite("ui/icons/IconMale");
    this._icon_species.sprite = this._actor.getActorAsset().getSprite();
  }

  public void updateBarAndTask(Actor pActor)
  {
    float health = (float) pActor.getHealth();
    float maxHealth = (float) pActor.getMaxHealth();
    this._bar_health.setBar(health, maxHealth, "/" + ((int) maxHealth).ToText(4), false, pSpeed: 0.25f);
    BehaviourTaskActor task = pActor.ai.task;
    string taskText = pActor.getTaskText();
    Sprite sprite = task != null ? task.getSprite() : this._no_task_icon;
    this._task_icon_left.sprite = sprite;
    this._task_icon_right.sprite = sprite;
    this._task_title.text = taskText;
  }

  public void showStats(Actor pActor)
  {
    int stat = (int) pActor.stats["damage"];
    int pMainVal = (int) ((double) stat * (double) pActor.stats["damage_range"]);
    this.setIconValue("i_age", (float) pActor.data.getAge());
    this.setIconValue("i_damage", (float) pMainVal, new float?((float) stat), pSeparator: '-');
    this.setIconValue("i_armor", pActor.stats["armor"], pEnding: "%");
    this.setIconValue("i_kills", (float) pActor.data.kills);
    this.setIconValue("i_renown", (float) pActor.data.renown);
    this.setIconValue("i_level", (float) pActor.data.level);
    this.setIconValue("i_experience", (float) pActor.data.experience, new float?((float) pActor.getExpToLevelup()));
    this.setIconValue("i_money", (float) pActor.money);
    this.setIconValue("i_loot", (float) pActor.loot);
  }

  public void setIconValue(
    string pName,
    float pMainVal,
    float? pMax = null,
    string pColor = "",
    bool pFloat = false,
    string pEnding = "",
    char pSeparator = '/')
  {
    StatsIcon iconViaId = this.getIconViaId(pName);
    if (Object.op_Equality((Object) iconViaId, (Object) null) || iconViaId.areValuesTooClose(pMainVal))
      return;
    iconViaId.setValue(pMainVal, pMax, pColor, pFloat, pEnding, pSeparator);
  }

  public StatsIcon getIconViaId(string pName)
  {
    StatsIcon iconViaId;
    this._stats_icons.TryGetValue(pName, out iconViaId);
    if (Object.op_Equality((Object) iconViaId, (Object) null))
      return (StatsIcon) null;
    ((Component) iconViaId).gameObject.SetActive(true);
    return iconViaId;
  }

  public void spawnAvatarText() => this._unit_texts.spawnAvatarText(this._actor);

  public UiUnitAvatarElement getAvatar() => this._avatar;
}
