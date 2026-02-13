// Decompiled with JetBrains decompiler
// Type: UiUnitAvatarElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UiUnitAvatarElement : MonoBehaviour, IBanner, IBaseMono, IRefreshElement
{
  public Image unit_type_bg;
  [SerializeField]
  private Sprite _type_king;
  [SerializeField]
  private Sprite _type_leader;
  [SerializeField]
  private Sprite _type_captain;
  public UnitAvatarLoader avatarLoader;
  public KingdomBanner kingdomBanner;
  public ClanBanner clanBanner;
  public bool show_banner_kingdom = true;
  public bool show_banner_clan = true;
  [SerializeField]
  private Image _tile_graphics_1;
  [SerializeField]
  private Image _tile_graphics_2;
  [SerializeField]
  private Sprite _tile_inside_boat;
  private Actor _actor;

  public MetaCustomizationAsset meta_asset
  {
    get => AssetManager.meta_customization_library.getAsset(MetaType.Unit);
  }

  public MetaTypeAsset meta_type_asset => AssetManager.meta_type_library.getAsset(MetaType.Unit);

  private void Start() => this.setupTooltip();

  public void setupTooltip()
  {
    TipButton tipButton;
    if (!((Component) this).TryGetComponent<TipButton>(ref tipButton))
      return;
    tipButton.hoverAction = (TooltipAction) (() =>
    {
      if (!InputHelpers.mouseSupported)
        return;
      this.tooltipActionActor();
    });
  }

  public void showTooltip() => this.tooltipActionActor();

  public void tooltipActionActor()
  {
    if (this._actor.isRekt())
      return;
    this._actor.showTooltip((object) this);
  }

  public void load(NanoObject pActor) => this.show((Actor) pActor);

  public void show(Actor pActor)
  {
    if (pActor == null)
    {
      ((Component) this).gameObject.SetActive(false);
    }
    else
    {
      ((Component) this).gameObject.SetActive(true);
      this._actor = pActor;
      this.checkSpecialBg(pActor);
      this.avatarLoader.load(pActor);
      if (this.show_banner_kingdom)
      {
        if (pActor.isAlive() && pActor.isKingdomCiv())
        {
          ((Component) this.kingdomBanner).gameObject.SetActive(true);
          this.kingdomBanner.load((NanoObject) pActor.kingdom);
        }
        else
          ((Component) this.kingdomBanner).gameObject.SetActive(false);
      }
      if (this.show_banner_clan)
      {
        if (pActor.isAlive() && pActor.hasClan())
        {
          ((Component) this.clanBanner).gameObject.SetActive(true);
          this.clanBanner.load((NanoObject) pActor.clan);
        }
        else
          ((Component) this.clanBanner).gameObject.SetActive(false);
      }
      this.updateTileSprite();
      ((Object) ((Component) this).gameObject).name = "UnitAvatar_" + pActor.data.id.ToString();
    }
  }

  public void updateTileSprite()
  {
    if (this._actor.isRekt() || this._actor.current_tile == null)
    {
      ((Component) this._tile_graphics_1).gameObject.SetActive(false);
      ((Component) this._tile_graphics_2).gameObject.SetActive(false);
    }
    else
    {
      ((Component) this._tile_graphics_1).gameObject.SetActive(true);
      ((Component) this._tile_graphics_2).gameObject.SetActive(true);
      if (this._actor.is_inside_boat)
      {
        this._tile_graphics_1.sprite = this._tile_inside_boat;
        this._tile_graphics_2.sprite = this._tile_inside_boat;
      }
      else
      {
        this._tile_graphics_1.sprite = this._actor.current_tile.Type.sprites.main.sprite;
        this._tile_graphics_2.sprite = this._actor.current_tile.Type.sprites.main.sprite;
      }
    }
  }

  private void checkSpecialBg(Actor pActor)
  {
    ((Component) this.unit_type_bg).gameObject.SetActive(false);
    if (!pActor.isAlive())
      return;
    if (pActor.hasKingdom() && pActor.isKing())
    {
      this.unit_type_bg.sprite = this._type_king;
      ((Component) this.unit_type_bg).gameObject.SetActive(true);
    }
    else if (pActor.hasCity() && pActor.isCityLeader())
    {
      this.unit_type_bg.sprite = this._type_leader;
      ((Component) this.unit_type_bg).gameObject.SetActive(true);
    }
    else
    {
      if (!pActor.is_army_captain)
        return;
      this.unit_type_bg.sprite = this._type_captain;
      ((Component) this.unit_type_bg).gameObject.SetActive(true);
    }
  }

  public void showThisActor()
  {
    if (this._actor.isRekt())
      return;
    if (!InputHelpers.mouseSupported && !Tooltip.isShowingFor((object) this))
      this.tooltipActionActor();
    else
      ActionLibrary.openUnitWindow(this._actor);
  }

  public Actor getActor() => this._actor;

  private void OnDisable() => this._actor = (Actor) null;

  public bool isSameActor(Actor pActor) => this._actor == pActor;

  public MetaCustomizationAsset GetMetaAsset()
  {
    return AssetManager.meta_customization_library.get("unit");
  }

  public string getName() => this._actor.getName();

  public NanoObject GetNanoObject() => (NanoObject) this._actor;

  Transform IBaseMono.get_transform() => ((Component) this).transform;

  GameObject IBaseMono.get_gameObject() => ((Component) this).gameObject;

  T IBaseMono.GetComponent<T>() => ((Component) this).GetComponent<T>();
}
