// Decompiled with JetBrains decompiler
// Type: NameplateText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class NameplateText : MonoBehaviour
{
  private NameplateManager _manager;
  [SerializeField]
  private Image _icon_species;
  [SerializeField]
  private Image _icon_special;
  [SerializeField]
  private Image _icon_favorite;
  [SerializeField]
  private Image _background_image;
  [SerializeField]
  private KingdomBanner _banner_kingdoms;
  [SerializeField]
  private CultureBanner _banner_culture;
  [SerializeField]
  private LanguageBanner _banner_language;
  [SerializeField]
  private AllianceBanner _banner_alliance;
  [SerializeField]
  private ReligionBanner _banner_religion;
  [SerializeField]
  private SubspeciesBanner _banner_subspecies;
  [SerializeField]
  private FamilyBanner _banner_family;
  [SerializeField]
  private ClanBanner _banner_clan;
  [SerializeField]
  private ArmyBanner _banner_army;
  [SerializeField]
  private CityBanner _banner_city;
  [SerializeField]
  private RectTransform _container_capture;
  public HorizontalLayoutGroup layout_group;
  private NameplateAsset _asset;
  private bool _show_icon_species;
  private bool _show_icon_special;
  private bool _show_icon_favorite;
  private bool _show_banner_army;
  private bool _show_banner_kingdom;
  private bool _show_banner_culture;
  private bool _show_banner_alliance;
  private bool _show_banner_clan;
  private bool _show_banner_religion;
  private bool _show_banner_family;
  private bool _show_banner_subspecies;
  private bool _show_banner_language;
  private bool _show_banner_city;
  private bool _show_capture_counter;
  private CanvasGroup _canvas_group;
  [SerializeField]
  private RectTransform _rect_background;
  [SerializeField]
  private Text _text_name;
  [SerializeField]
  private Text _conquer_text;
  private RectTransform _rect;
  private RectTransform _text_rect;
  private bool _showing;
  internal bool priority_capital;
  internal int priority_population;
  internal bool favorited;
  internal Rect map_text_rect_click;
  internal Rect map_text_rect_overlap;
  public NanoObject nano_object;
  private float _last_scale;
  private string _old_text;
  private float _text_width;
  private bool _active_check_dirty;
  private NameplateRenderingType _last_mode;
  private Vector2 _last_position;

  public Vector2 getLastScreenPosition() => this._last_position;

  private void Awake()
  {
    this._rect = ((Component) this).GetComponent<RectTransform>();
    this._text_rect = ((Component) this._text_name).GetComponent<RectTransform>();
    this._canvas_group = ((Component) this).GetComponent<CanvasGroup>();
  }

  public void prepare(
    NameplateAsset pAsset,
    NanoObject pMeta,
    float pGlobalScale,
    NameplateRenderingType pNameplateMode,
    bool pNanoObjectSet,
    NanoObject pSelectedNanoObject)
  {
    if (pNanoObjectSet)
      pNameplateMode = pSelectedNanoObject != pMeta ? NameplateRenderingType.BannerOnly : NameplateRenderingType.Full;
    if (pNameplateMode != this._last_mode)
    {
      this.clearCaches();
      this._active_check_dirty = true;
      this._last_mode = pNameplateMode;
      switch (this._last_mode)
      {
        case NameplateRenderingType.Full:
          ((Component) this._background_image).transform.localScale = new Vector3(1f, 1f, 1f);
          ((Behaviour) this._background_image).enabled = true;
          break;
        case NameplateRenderingType.BannerOnly:
          ((Component) this._background_image).transform.localScale = new Vector3(pAsset.banner_only_mode_scale, pAsset.banner_only_mode_scale, 1f);
          ((Behaviour) this._background_image).enabled = false;
          break;
      }
    }
    this.updateScale(pMeta, pGlobalScale, pNanoObjectSet, pSelectedNanoObject);
    this.resetElements();
    this.setShowing(true);
    this.setAssetAndMeta(pAsset, pMeta);
    if (((IFavoriteable) pMeta).isFavorite())
      this.showFavoriteIcon();
    else
      this._show_icon_favorite = false;
    this.checkSetActive((Component) this._icon_favorite, this._show_icon_favorite);
  }

  private void updateScale(
    NanoObject pMeta,
    float pGlobalScale,
    bool pNanoObjectSet,
    NanoObject pSelectedNanoObject)
  {
    float num = pGlobalScale;
    if (pNanoObjectSet)
      num = pSelectedNanoObject != pMeta ? pGlobalScale * 0.8f : pGlobalScale * 1.2f;
    if ((double) this._last_scale == (double) num)
      return;
    this._last_scale = num;
    ((Component) this).transform.localScale = new Vector3(num, num, 1f);
  }

  public void forceScale(Vector3 pScale)
  {
    this._last_scale = pScale.x;
    ((Component) this).transform.localScale = pScale;
  }

  public void newNameplate(NameplateManager pManager, string pName)
  {
    this.clearFull();
    this._manager = pManager;
    ((Object) this).name = pName;
  }

  public bool isShowing() => this._showing;

  public void setShowing(bool pVal) => this._showing = pVal;

  public void checkActive()
  {
    if (this._showing)
    {
      if (!((Component) this).gameObject.activeSelf)
        ((Component) this).gameObject.SetActive(true);
    }
    else if (((Component) this).gameObject.activeSelf)
      ((Component) this).gameObject.SetActive(false);
    if (!this._showing || !this._active_check_dirty)
      return;
    this._active_check_dirty = false;
    this.checkActiveElements();
  }

  private void checkActiveElements()
  {
    this.checkSetActive((Component) this._container_capture, this._show_capture_counter);
    this.checkSetActive((Component) this._icon_species, this._show_icon_species);
    this.checkSetActive((Component) this._icon_special, this._show_icon_special);
    this.checkSetActive((Component) this._banner_alliance, this._show_banner_alliance);
    this.checkSetActive((Component) this._banner_clan, this._show_banner_clan);
    this.checkSetActive((Component) this._banner_culture, this._show_banner_culture);
    this.checkSetActive((Component) this._banner_kingdoms, this._show_banner_kingdom);
    this.checkSetActive((Component) this._banner_religion, this._show_banner_religion);
    this.checkSetActive((Component) this._banner_family, this._show_banner_family);
    this.checkSetActive((Component) this._banner_language, this._show_banner_language);
    this.checkSetActive((Component) this._banner_subspecies, this._show_banner_subspecies);
    this.checkSetActive((Component) this._banner_army, this._show_banner_army);
    this.checkSetActive((Component) this._banner_city, this._show_banner_city);
  }

  private void checkSetActive(Component pComponent, bool pShouldBeActive)
  {
    this.checkSetActive(pComponent.gameObject, pShouldBeActive);
  }

  private void checkSetActive(GameObject pObject, bool pShouldBeActive)
  {
    if (pShouldBeActive)
    {
      if (pObject.activeSelf)
        return;
      pObject.SetActive(true);
    }
    else
    {
      if (!pObject.activeSelf)
        return;
      pObject.SetActive(false);
    }
  }

  public void clearFull()
  {
    this.nano_object = (NanoObject) null;
    this.clearCaches();
    this.setShowing(false);
    this.resetElements();
  }

  public void clearCaches()
  {
    this._asset = (NameplateAsset) null;
    this._last_position = Globals.POINT_IN_VOID_2;
    this._last_scale = -1f;
    this._old_text = "!";
    this._last_mode = NameplateRenderingType.Clear;
  }

  private void resetElements()
  {
    this.priority_capital = false;
    this.priority_population = 0;
    this.favorited = false;
    this._show_capture_counter = false;
    this._show_icon_favorite = false;
    this._show_icon_species = false;
    this._show_icon_special = false;
    this._show_banner_alliance = false;
    this._show_banner_clan = false;
    this._show_banner_culture = false;
    this._show_banner_army = false;
    this._show_banner_kingdom = false;
    this._show_banner_religion = false;
    this._show_banner_subspecies = false;
    this._show_banner_language = false;
    this._show_banner_family = false;
    this._show_banner_city = false;
  }

  private void setupMeta(MetaObjectData pData, ColorAsset pColorAsset)
  {
    this.favorited = pData.favorite;
    ((Graphic) this._text_name).color = pColorAsset.getColorText();
    this.updateAlpha(pData);
  }

  private void updateAlpha(MetaObjectData pData)
  {
    float num = !this.checkShouldDrawObject(pData) ? 0.5f : 1f;
    if ((double) this._canvas_group.alpha == (double) num)
      return;
    this._canvas_group.alpha = num;
  }

  internal void showTextKingdom(Kingdom pMetaObject, Vector2 pPosition)
  {
    this.setupMeta((MetaObjectData) pMetaObject.data, pMetaObject.getColor());
    int populationPeople = pMetaObject.getPopulationPeople();
    string pNewText = this.getStringForNameplate(pMetaObject.name, populationPeople);
    if (this.is_full)
    {
      if (DebugConfig.isOn(DebugOption.ShowWarriorsCityText))
        pNewText = $"{pNewText} | {pMetaObject.countTotalWarriors().ToString()}/{pMetaObject.countWarriorsMax().ToString()}";
      if (DebugConfig.isOn(DebugOption.ShowCityWeaponsText))
        pNewText = $"{pNewText} | w{pMetaObject.countWeapons().ToString()}";
    }
    this.setText(pNewText, Vector2.op_Implicit(pPosition));
    this.setPriority(populationPeople);
    this.showSpecies(pMetaObject.getSpriteIcon());
    this._show_banner_kingdom = true;
    this._banner_kingdoms.load((NanoObject) pMetaObject);
    if (!this.is_full)
      return;
    Clan kingClan = pMetaObject.getKingClan();
    if (kingClan == null)
      return;
    this._show_banner_clan = true;
    this._banner_clan.load((NanoObject) kingClan);
  }

  internal void showTextReligion(Religion pMetaObject, Vector3 pPosition)
  {
    this.setupMeta((MetaObjectData) pMetaObject.data, pMetaObject.getColor());
    int count = pMetaObject.units.Count;
    this.setText(this.getStringForNameplate(pMetaObject.name, count), pPosition);
    this.setPriority(count);
    this._show_banner_religion = true;
    this._banner_religion.load((NanoObject) pMetaObject);
    this.showSpecies(pMetaObject.getActorAsset().getSpriteIcon());
  }

  internal void showTextSubspecies(Subspecies pMetaObject, Vector3 pPosition)
  {
    this.setupMeta((MetaObjectData) pMetaObject.data, pMetaObject.getColor());
    int count = pMetaObject.units.Count;
    this.setText(this.getStringForNameplate(pMetaObject.name, count), pPosition);
    this.setPriority(count);
    this._show_banner_subspecies = true;
    this._banner_subspecies.load((NanoObject) pMetaObject);
    this.showSpecies(pMetaObject.getSpriteIcon());
  }

  internal void showTextFamily(Family pMetaObject, Vector3 pPosition)
  {
    this.setupMeta((MetaObjectData) pMetaObject.data, pMetaObject.getColor());
    int count = pMetaObject.units.Count;
    this.setText(this.getStringForNameplate(pMetaObject.name, count), pPosition);
    this.setPriority(count);
    this._show_banner_family = true;
    this._banner_family.load((NanoObject) pMetaObject);
  }

  internal void showTextArmy(Army pMetaObject, Vector3 pPosition)
  {
    this.setupMeta((MetaObjectData) pMetaObject.data, pMetaObject.getColor());
    int count = pMetaObject.units.Count;
    this.setText(this.getStringForNameplateLine(pMetaObject.name, count), pPosition);
    this.setPriority(count);
    this._show_banner_army = true;
    this._banner_army.load((NanoObject) pMetaObject);
    if (pMetaObject.hasCaptain())
      this.showSpecies(pMetaObject.getCaptain().getActorAsset().getSpriteIcon());
    else
      this.showSpecies(pMetaObject.getActorAsset().getSpriteIcon());
  }

  internal void showTextCulture(Culture pMetaObject, Vector3 pPosition)
  {
    this.setupMeta((MetaObjectData) pMetaObject.data, pMetaObject.getColor());
    int count = pMetaObject.units.Count;
    this.setText(this.getStringForNameplate(pMetaObject.name, count), pPosition);
    this.setPriority(count);
    this._show_banner_culture = true;
    this._banner_culture.load((NanoObject) pMetaObject);
    this.showSpecies(pMetaObject.getActorAsset().getSpriteIcon());
  }

  internal void showTextLanguage(Language pMetaObject, Vector3 pPosition)
  {
    this.setupMeta((MetaObjectData) pMetaObject.data, pMetaObject.getColor());
    int count = pMetaObject.units.Count;
    this.setText(this.getStringForNameplate(pMetaObject.name, count), pPosition);
    this.setPriority(count);
    this._show_banner_language = true;
    this._banner_language.load((NanoObject) pMetaObject);
    this.showSpecies(pMetaObject.getActorAsset().getSpriteIcon());
  }

  internal void showTextAlliance(Alliance pMetaObject, City pCity)
  {
    this.setupMeta((MetaObjectData) pMetaObject.data, pMetaObject.getColor());
    int num = pMetaObject.countPopulation();
    this._show_icon_species = false;
    string pNewText = this.getStringForNameplate(pMetaObject.name, num);
    if (this.is_full && DebugConfig.isOn(DebugOption.ShowWarriorsCityText))
      pNewText = $"{pNewText} | {pMetaObject.countWarriors().ToString()}";
    this.setText(pNewText, Vector2.op_Implicit(pCity.city_center));
    this.setPriority(num);
    this._show_banner_alliance = true;
    this._banner_alliance.load((NanoObject) pMetaObject);
  }

  internal void showTextClanFluid(Clan pMetaObject, Vector3 pPosition)
  {
    this.setupMeta((MetaObjectData) pMetaObject.data, pMetaObject.getColor());
    int count = pMetaObject.units.Count;
    this.setText(this.getStringForNameplate(pMetaObject.name, count), pPosition);
    this.setPriority(count);
    this._show_banner_clan = true;
    this._banner_clan.load((NanoObject) pMetaObject);
    this.showSpecies(pMetaObject.getActorAsset().getSpriteIcon());
  }

  internal void showTextClanCity(Clan pMetaObject, City pCity)
  {
    this.setupMeta((MetaObjectData) pMetaObject.data, pMetaObject.getColor());
    int count = pMetaObject.units.Count;
    this.setText(this.getStringForNameplate(pMetaObject.name, count), Vector2.op_Implicit(pCity.city_center));
    this.setPriority(count);
    this.showSpecies(pMetaObject.getActorAsset().getSpriteIcon());
    this._show_banner_clan = true;
    this._banner_clan.load((NanoObject) pMetaObject);
  }

  internal void showTextCity(City pMetaObject, Vector2 pPosition)
  {
    this.setupMeta((MetaObjectData) pMetaObject.data, pMetaObject.kingdom.getColor());
    if (pMetaObject.isCapitalCity())
      this.setNameplateSprite("ui/nameplates/nameplate_city_capital");
    else
      this.setNameplateSprite("ui/nameplates/nameplate_city");
    int populationPeople = pMetaObject.getPopulationPeople();
    string pNewText = this.getStringForNameplate(pMetaObject.name, populationPeople);
    if (this.is_full)
    {
      if (DebugConfig.isOn(DebugOption.ShowWarriorsCityText))
      {
        pNewText = $"{pNewText} | {pMetaObject.countWarriors().ToString()}/{pMetaObject.getMaxWarriors().ToString()}";
        if (Config.isEditor)
        {
          string str = $"  :  {((int) ((double) pMetaObject.getArmyMaxMultiplier() * 100.0)).ToString()}%";
          pNewText += str;
        }
      }
      if (DebugConfig.isOn(DebugOption.ShowCityWeaponsText))
        pNewText = $"{pNewText} | w{pMetaObject.countWeapons().ToString()}";
      if (DebugConfig.isOn(DebugOption.ShowFoodCityText))
        pNewText = $"{pNewText} | F{pMetaObject.getTotalFood().ToString()}";
    }
    this.setText(pNewText, Vector2.op_Implicit(pPosition));
    if (pMetaObject.getMainSubspecies() != null)
      this.showSpecies(pMetaObject.getMainSubspecies().getActorAsset().getSpriteIcon());
    if (pMetaObject.last_visual_capture_ticks != 0)
    {
      this._show_capture_counter = true;
      this._active_check_dirty = true;
      if (pMetaObject.being_captured_by != null && pMetaObject.being_captured_by.isAlive())
        ((Graphic) this._conquer_text).color = pMetaObject.being_captured_by.getColor().getColorText();
      this._conquer_text.text = pMetaObject.last_visual_capture_ticks.ToString() + "%";
    }
    else
    {
      this._show_capture_counter = false;
      this._active_check_dirty = true;
    }
    if (this._show_capture_counter)
    {
      Vector2 vector2;
      if (this.is_full)
      {
        // ISSUE: explicit constructor call
        ((Vector2) ref vector2).\u002Ector(0.0f, -1f);
      }
      else
      {
        // ISSUE: explicit constructor call
        ((Vector2) ref vector2).\u002Ector(3f, -25f);
      }
      this._container_capture.anchoredPosition = vector2;
    }
    this._show_banner_city = true;
    this._banner_city.load((NanoObject) pMetaObject);
    this.priority_capital = pMetaObject.isCapitalCity();
    this.setPriority(populationPeople);
  }

  private void showSpecies(string pPath) => this.showSpecies(SpriteTextureLoader.getSprite(pPath));

  private void showSpecies(Sprite pSprite)
  {
    if (this.is_mini)
      return;
    this._show_icon_species = true;
    this._icon_species.sprite = pSprite;
  }

  public void showFavoriteIcon() => this._show_icon_favorite = true;

  private void showSpecial(string pPath)
  {
    this._show_icon_special = true;
    this._icon_special.sprite = SpriteTextureLoader.getSprite(pPath);
  }

  private void setText(string pNewText, Vector3 pPos, int pAdditionalWidth = 10)
  {
    if (this.is_mini)
      pAdditionalWidth = 0;
    this.updatePositionAndRect(pPos);
    if (this._old_text == pNewText)
      return;
    this._old_text = pNewText;
    this._text_name.text = pNewText;
    this._text_width = this._text_name.preferredWidth + (float) pAdditionalWidth;
    this._text_rect.sizeDelta = new Vector2(this._text_width, this._rect_background.sizeDelta.y);
    this._last_position = Globals.POINT_IN_VOID_2;
  }

  private void updatePositionAndRect(Vector3 pPos)
  {
    Vector2 vector2 = this.transformPosition(pPos);
    if (!Vector2.op_Inequality(vector2, this._last_position))
      return;
    this._last_position = vector2;
    ((Component) this).transform.position = Vector2.op_Implicit(vector2);
    this.recalcScaledOverlapRect(this._rect_background, ref this.map_text_rect_click);
    float num1 = (float) ((double) ((Rect) ref this.map_text_rect_click).width * 0.0 * 0.5);
    float num2 = (float) ((double) ((Rect) ref this.map_text_rect_click).height * 0.0 * 0.5);
    ((Rect) ref this.map_text_rect_overlap).x = ((Rect) ref this.map_text_rect_click).x + num1;
    ((Rect) ref this.map_text_rect_overlap).y = ((Rect) ref this.map_text_rect_click).y + num2;
    ((Rect) ref this.map_text_rect_overlap).width = ((Rect) ref this.map_text_rect_click).width - num1 * 2f;
    ((Rect) ref this.map_text_rect_overlap).height = ((Rect) ref this.map_text_rect_click).height - num2 * 2f;
  }

  private string getStringForNameplate(string pName, int pCount)
  {
    return this.is_mini ? string.Empty : $"{pName} {pCount.ToString()}";
  }

  private string getStringForNameplateLine(string pName, int pCount)
  {
    return this.is_mini ? string.Empty : $"{pName} - {pCount.ToString()}";
  }

  public bool overlapsWithOtherPlate(NameplateText pText)
  {
    return ((Rect) ref this.map_text_rect_overlap).Overlaps(pText.map_text_rect_overlap);
  }

  private void recalcScaledOverlapRect(RectTransform pRectBackground, ref Rect pMapTextRect)
  {
    this.recalcScaledOverlapRectSimple(pRectBackground, ref pMapTextRect);
  }

  private void recalcScaledOverlapRectCorners(RectTransform pRectBackground, ref Rect pMapTextRect)
  {
    Vector3[] vector3Array = new Vector3[4];
    pRectBackground.GetWorldCorners(vector3Array);
    float num1 = Mathf.Min(new float[4]
    {
      vector3Array[0].x,
      vector3Array[1].x,
      vector3Array[2].x,
      vector3Array[3].x
    });
    float num2 = Mathf.Max(new float[4]
    {
      vector3Array[0].x,
      vector3Array[1].x,
      vector3Array[2].x,
      vector3Array[3].x
    });
    float num3 = Mathf.Min(new float[4]
    {
      vector3Array[0].y,
      vector3Array[1].y,
      vector3Array[2].y,
      vector3Array[3].y
    });
    float num4 = Mathf.Max(new float[4]
    {
      vector3Array[0].y,
      vector3Array[1].y,
      vector3Array[2].y,
      vector3Array[3].y
    });
    ((Rect) ref pMapTextRect).x = num1;
    ((Rect) ref pMapTextRect).y = num3;
    ((Rect) ref pMapTextRect).width = num2 - num1;
    ((Rect) ref pMapTextRect).height = num4 - num3;
  }

  private void recalcScaledOverlapRectSimple(RectTransform pRectBackground, ref Rect pMapTextRect)
  {
    float cachedCanvasScale = this._manager.cached_canvas_scale;
    Vector2 sizeDelta = pRectBackground.sizeDelta;
    if (this.is_mini)
      ((Vector2) ref sizeDelta).Set(60f, 60f);
    float num1 = sizeDelta.x * 0.55f * cachedCanvasScale;
    float num2 = sizeDelta.y * 0.55f * cachedCanvasScale;
    Vector3 position = ((Transform) pRectBackground).position;
    ((Rect) ref pMapTextRect).x = position.x - num1 * 0.5f;
    ((Rect) ref pMapTextRect).y = position.y - num2 * 0.5f;
    ((Rect) ref pMapTextRect).width = num1;
    ((Rect) ref pMapTextRect).height = num2;
  }

  private Vector2 transformPosition(Vector3 pVec)
  {
    return Vector2.op_Implicit(World.world.camera.WorldToScreenPoint(pVec));
  }

  private bool checkShouldDrawObject(MetaObjectData pData)
  {
    return !this._manager.cached_favorites_only || pData.favorite;
  }

  public void setAssetAndMeta(NameplateAsset pAsset, NanoObject pNano)
  {
    this.nano_object = pNano;
    if (this._asset == pAsset)
      return;
    this._active_check_dirty = true;
    this._asset = pAsset;
    this.setNameplateSprite(pAsset.path_sprite);
    RectOffset padding = ((LayoutGroup) this.layout_group).padding;
    if (this.is_mini)
    {
      padding.left = 0;
      padding.right = 0;
    }
    else
    {
      padding.left = pAsset.padding_left;
      padding.right = pAsset.padding_right;
    }
    padding.top = pAsset.padding_top;
  }

  public void setNameplateSprite(string pPath)
  {
    this._background_image.sprite = SpriteTextureLoader.getSprite(pPath);
  }

  private void setPriority(int pValue) => this.priority_population = pValue;

  private bool is_full => this._last_mode == NameplateRenderingType.Full;

  public bool is_mini => this._last_mode == NameplateRenderingType.BannerOnly;
}
