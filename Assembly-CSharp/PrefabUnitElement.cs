// Decompiled with JetBrains decompiler
// Type: PrefabUnitElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class PrefabUnitElement : WindowListElementBaseActor, IPointerMoveHandler, IEventSystemHandler
{
  private Actor _actor;
  public Text unitName;
  public UiUnitAvatarElement avatarElement;
  public StatBar health_bar;
  public CountUpOnClick text_damage;
  public CountUpOnClick text_level;
  public CountUpOnClick text_kills;
  public CountUpOnClick text_age;
  public Image icon_sex;
  [SerializeField]
  private Image _icon_species;
  [SerializeField]
  private GameObject _icon_favorite;

  private void Awake() => this.initTooltip();

  internal void show(Actor pActor)
  {
    this._actor = pActor;
    this.unitName.text = pActor.coloredName;
    this.avatarElement.show(pActor);
    this.health_bar.setBar((float) pActor.getHealth(), (float) pActor.getMaxHealth(), "");
    this.text_level.setValue(pActor.level);
    this.text_kills.setValue(pActor.data.kills);
    this.text_age.setValue(pActor.getAge());
    if (pActor.asset.inspect_sex)
    {
      ((Component) this.icon_sex).gameObject.SetActive(true);
      this.icon_sex.sprite = !pActor.isSexMale() ? SpriteTextureLoader.getSprite("ui/icons/IconFemale") : SpriteTextureLoader.getSprite("ui/icons/IconMale");
    }
    else
      ((Component) this.icon_sex).gameObject.SetActive(false);
    this._icon_species.sprite = this._actor.asset.getSpriteIcon();
    this.toggleFavorited(this._actor.isFavorite());
  }

  public void clickLocate() => WorldLog.locationFollow(this._actor);

  public void clickInspect()
  {
    if (!InputHelpers.mouseSupported && !Tooltip.isShowingFor((object) this))
      this.tooltipAction();
    else
      ActionLibrary.openUnitWindow(this._actor);
  }

  public Actor getActor() => this._actor;

  public void toggleFavorited(bool pState) => this._icon_favorite.SetActive(pState);

  private void OnDisable() => this._actor = (Actor) null;

  public void OnPointerMove(PointerEventData pData)
  {
    if (!InputHelpers.mouseSupported || Tooltip.anyActive())
      return;
    this.tooltipAction();
  }

  private void tooltipAction() => this._actor.showTooltip((object) this);

  private void initTooltip()
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: method pointer
    ((Component) this).GetComponent<Button>().OnHoverOut(PrefabUnitElement.\u003C\u003Ec.\u003C\u003E9__20_0 ?? (PrefabUnitElement.\u003C\u003Ec.\u003C\u003E9__20_0 = new UnityAction((object) PrefabUnitElement.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CinitTooltip\u003Eb__20_0))));
  }
}
