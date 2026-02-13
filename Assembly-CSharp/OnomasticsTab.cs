// Decompiled with JetBrains decompiler
// Type: OnomasticsTab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class OnomasticsTab : OnomasticsNameGenerator
{
  private const int MAX_CARDS = 30;
  private const float BUTTON_SCALE = 1f;
  private const float WORD_SCALE = 1f;
  private const float EFFECT_SCALE = 0.8f;
  private const float WORD_BOX_SIZE_X = 2f;
  private const float WORD_BOX_SIZE_Y = 2f;
  private const float EFFECT_BOX_SIZE_X = 10f;
  private const float EFFECT_BOX_SIZE_Y = 3f;
  public Transform parent_name_variation_1;
  public DragOrderContainer name_variation_1_drag_container;
  public Transform parent_asset_groups;
  public Transform parent_asset_specials;
  public Transform parent_asset_editor_group;
  public Image icon_last_selected_group;
  public Text text_counter;
  public OnomasticsAssetButton prefab_button;
  public OnomasticsAssetButton prefab_button_template;
  private ObjectPoolGenericMono<OnomasticsAssetButton> _pool_buttons;
  protected NameInput _editor_input;
  private bool _created;
  private OnomasticsData _onomastics_data;
  private MetaType _name_set_type = MetaType.Unit;
  private OnomasticsAsset _selected_editor_group;
  private OnomasticsAssetButton _selected_editor_group_button;
  public Image selected_icon_effect;
  public Image selected_icon_effect_2;
  private ObjectPoolGenericMono<Image> _pool_boxed_effects;
  public Image boxed_effect_prefab;
  public Transform boxed_effects_transform;
  private ObjectPoolGenericMono<Image> _pool_word_effects;
  public Image word_effect_prefab;
  public Transform word_effects_transform;
  [SerializeField]
  private TabTogglesGroup _tab_groups;
  private static readonly string[] consonant_combinations = new string[30]
  {
    "b",
    "c",
    "d",
    "f",
    "g",
    "h",
    "j",
    "k",
    "l",
    "m",
    "n",
    "p",
    "q",
    "r",
    "s",
    "t",
    "v",
    "w",
    "x",
    "y",
    "z",
    "st",
    "bl",
    "tr",
    "pr",
    "cl",
    "kr",
    "fr",
    "gr",
    "pl"
  };
  private static readonly string[] vowel_combinations = new string[30]
  {
    "a",
    "e",
    "i",
    "o",
    "u",
    "ai",
    "ei",
    "oi",
    "au",
    "ou",
    "ie",
    "ee",
    "oa",
    "ea",
    "io",
    "ia",
    "ui",
    "ue",
    "oo",
    "ae",
    "ya",
    "yo",
    "ye",
    "wa",
    "we",
    "wi",
    "wo",
    "ua",
    "eu",
    "iu"
  };

  private void OnEnable()
  {
    this.create();
    this.showCategoryGroups();
  }

  private void showCategoryGroups()
  {
    ((Component) this._tab_groups).gameObject.SetActive(true);
    this._tab_groups.clearButtons();
    this._tab_groups.tryAddButton("ui/Icons/actor_traits/iconAttractive", "tab_onomastics_unit", new TabToggleAction(this.loadNameSet), (TabToggleAction) (() => this._name_set_type = MetaType.Unit));
    this._tab_groups.tryAddButton("ui/Icons/iconFamilyList", "tab_onomastics_family", new TabToggleAction(this.loadNameSet), (TabToggleAction) (() => this._name_set_type = MetaType.Family));
    this._tab_groups.tryAddButton("ui/Icons/iconClanList", "tab_onomastics_clan", new TabToggleAction(this.loadNameSet), (TabToggleAction) (() => this._name_set_type = MetaType.Clan));
    this._tab_groups.tryAddButton("ui/Icons/iconCityList", "tab_onomastics_city", new TabToggleAction(this.loadNameSet), (TabToggleAction) (() => this._name_set_type = MetaType.City));
    this._tab_groups.tryAddButton("ui/Icons/iconKingdomList", "tab_onomastics_kingdom", new TabToggleAction(this.loadNameSet), (TabToggleAction) (() => this._name_set_type = MetaType.Kingdom));
    this._tab_groups.enableFirst();
  }

  private void openFirstGroup()
  {
    using (ListPool<OnomasticsAssetButton> activeButtons = this.getActiveButtons(this.parent_asset_groups))
    {
      for (int index = 0; index < activeButtons.Count; ++index)
      {
        OnomasticsAssetButton pButton = activeButtons[index];
        if (pButton.onomastics_asset.id == "group_1")
        {
          this.openGroup(pButton);
          break;
        }
      }
    }
  }

  private void Update()
  {
    if (this._selected_editor_group != null)
    {
      ((Component) this.selected_icon_effect).transform.position = ((Component) this._selected_editor_group_button).transform.position;
      Vector3 position = ((Component) this._selected_editor_group_button).transform.position;
      position.y += 30f;
      ((Component) this.selected_icon_effect_2).transform.position = position;
    }
    this.updateNameGeneration(this._onomastics_data);
    this.text_counter.text = $"{this._pool_buttons.countActive().ToString()}/{30}";
  }

  private void LateUpdate() => this.checkButtonsAndEffects();

  private void checkButtonsAndEffects()
  {
    this._pool_boxed_effects.clear();
    this._pool_word_effects.clear();
    OnomasticsAssetButton pButton1 = (OnomasticsAssetButton) null;
    using (ListPool<OnomasticsAssetButton> activeButtons = this.getActiveButtons(this.parent_name_variation_1))
    {
      for (int index = 0; index < activeButtons.Count; ++index)
      {
        OnomasticsAssetButton onomasticsAssetButton = activeButtons[index];
        OnomasticsAssetButton pButton2 = (OnomasticsAssetButton) null;
        if (onomasticsAssetButton.onomastics_asset.is_word_divider)
          pButton1 = (OnomasticsAssetButton) null;
        else if (Object.op_Equality((Object) pButton1, (Object) null))
          pButton1 = onomasticsAssetButton;
        if (index + 1 < activeButtons.Count)
          pButton2 = activeButtons[index + 1];
        if (onomasticsAssetButton.onomastics_asset.affects_left_word)
          this.showWordBox(pButton1, onomasticsAssetButton);
        if (!onomasticsAssetButton.onomastics_asset.is_immune && Object.op_Inequality((Object) pButton2, (Object) null) && pButton2.onomastics_asset.affects_left && (!pButton2.onomastics_asset.affects_left_group_only || onomasticsAssetButton.onomastics_asset.isGroupType()))
          this.showEffectBox(onomasticsAssetButton, pButton2);
      }
    }
  }

  private ListPool<OnomasticsAssetButton> getActiveButtons(Transform pTransform)
  {
    ListPool<OnomasticsAssetButton> activeButtons = new ListPool<OnomasticsAssetButton>(pTransform.childCount);
    for (int index = 0; index < pTransform.childCount; ++index)
    {
      OnomasticsAssetButton component = ((Component) pTransform.GetChild(index)).GetComponent<OnomasticsAssetButton>();
      if (!Object.op_Equality((Object) component, (Object) null) && ((Component) component).gameObject.activeSelf)
        activeButtons.Add(component);
    }
    return activeButtons;
  }

  private void showEffectBox(OnomasticsAssetButton pButton1, OnomasticsAssetButton pButton2)
  {
    Image next = this._pool_boxed_effects.getNext();
    RectTransform component = ((Component) next).GetComponent<RectTransform>();
    Vector3[] vector3Array1 = new Vector3[4];
    Vector3[] vector3Array2 = new Vector3[4];
    pButton1.getRect().GetWorldCorners(vector3Array1);
    pButton2.getRect().GetWorldCorners(vector3Array2);
    float num1 = Mathf.Min(vector3Array1[0].x, vector3Array2[0].x);
    float num2 = Mathf.Max(vector3Array1[2].x, vector3Array2[2].x);
    float num3 = Mathf.Min(vector3Array1[0].y, vector3Array2[0].y);
    float num4 = Mathf.Max(vector3Array1[2].y, vector3Array2[2].y);
    Vector3 vector3_1 = ((Component) next).transform.parent.InverseTransformPoint(new Vector3(num1, num3, 0.0f));
    Vector3 vector3_2 = ((Component) next).transform.parent.InverseTransformPoint(new Vector3(num2, num4, 0.0f));
    float num5 = 10f;
    float num6 = 3f;
    component.anchoredPosition = new Vector2((float) (((double) vector3_1.x + (double) vector3_2.x) / 2.0), (float) (((double) vector3_1.y + (double) vector3_2.y) / 2.0));
    component.sizeDelta = new Vector2(vector3_2.x - vector3_1.x + num5, vector3_2.y - vector3_1.y + num6);
    ((Transform) component).localScale = new Vector3(0.8f, 0.8f, 0.8f);
  }

  private void showWordBox(OnomasticsAssetButton pButton1, OnomasticsAssetButton pButton2)
  {
    Image next = this._pool_word_effects.getNext();
    RectTransform component = ((Component) next).GetComponent<RectTransform>();
    Vector3[] vector3Array1 = new Vector3[4];
    Vector3[] vector3Array2 = new Vector3[4];
    pButton1.getRect().GetWorldCorners(vector3Array1);
    pButton2.getRect().GetWorldCorners(vector3Array2);
    float num1 = Mathf.Min(vector3Array1[0].x, vector3Array2[0].x);
    float num2 = Mathf.Max(vector3Array1[2].x, vector3Array2[2].x);
    float num3 = Mathf.Min(vector3Array1[0].y, vector3Array2[0].y);
    float num4 = Mathf.Max(vector3Array1[2].y, vector3Array2[2].y);
    Vector3 vector3_1 = ((Component) next).transform.parent.InverseTransformPoint(new Vector3(num1, num3, 0.0f));
    Vector3 vector3_2 = ((Component) next).transform.parent.InverseTransformPoint(new Vector3(num2, num4, 0.0f));
    float num5 = 2f;
    float num6 = 2f;
    component.anchoredPosition = new Vector2((float) (((double) vector3_1.x + (double) vector3_2.x) / 2.0), (float) (((double) vector3_1.y + (double) vector3_2.y) / 2.0));
    component.sizeDelta = new Vector2(vector3_2.x - vector3_1.x + num5, vector3_2.y - vector3_1.y + num6);
    ((Transform) component).localScale = new Vector3(1f, 1f, 1f);
  }

  private void loadOnomasicsData()
  {
    this._onomastics_data = SelectedMetas.selected_culture.getOnomasticData(this._name_set_type);
    this.loadInitialButtons();
  }

  private void loadInitialButtons()
  {
    this._pool_buttons.clear();
    List<string> fullTemplateData = this._onomastics_data.getFullTemplateData();
    for (int index = 0; index < fullTemplateData.Count; ++index)
      this.loadTemplateButton(fullTemplateData[index]);
  }

  public OnomasticsData getOnomasticsData() => this._onomastics_data;

  protected void OnDisable()
  {
    if (Object.op_Equality((Object) this._editor_input, (Object) null))
      return;
    this._editor_input.inputField.DeactivateInputField();
  }

  protected virtual void initNameInput()
  {
    if (Object.op_Equality((Object) this._editor_input, (Object) null))
      return;
    // ISSUE: method pointer
    this._editor_input.addListener(new UnityAction<string>((object) this, __methodptr(applyInputName)));
  }

  private void applyInputName(string pString)
  {
    pString = pString.Replace("\n", " ");
    pString = pString.Replace("\r", " ");
    while (pString.Contains("  "))
      pString = pString.Replace("  ", " ");
    pString = pString.Trim();
    if (!this._onomastics_data.setGroup(this._selected_editor_group.id, pString))
      return;
    this.resetNameGenerationTextBox();
  }

  private void resetNameGenerationTextBox()
  {
    this.clickRegenerate();
    using (ListPool<OnomasticsAssetButton> activeButtons = this.getActiveButtons(this.parent_name_variation_1))
    {
      using (ListPool<string> pTemplateData = new ListPool<string>(activeButtons.Count))
      {
        for (int index = 0; index < activeButtons.Count; ++index)
        {
          OnomasticsAssetButton onomasticsAssetButton = activeButtons[index];
          pTemplateData.Add(onomasticsAssetButton.onomastics_asset.id);
        }
        this._onomastics_data.setTemplateData((IReadOnlyList<string>) pTemplateData);
        if (!Object.op_Inequality((Object) this.name_variation_1_drag_container.rect_transform, (Object) null))
          return;
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.name_variation_1_drag_container.rect_transform);
      }
    }
  }

  private void create()
  {
    if (this._created)
      return;
    this.name_variation_1_drag_container.on_order_changed += new Action(this.resetNameGenerationTextBox);
    this._pool_boxed_effects = new ObjectPoolGenericMono<Image>(this.boxed_effect_prefab, this.boxed_effects_transform);
    this._pool_word_effects = new ObjectPoolGenericMono<Image>(this.word_effect_prefab, this.word_effects_transform);
    this._editor_input = ((Component) ((Component) this).transform.FindRecursive("Group Editor Element"))?.GetComponent<NameInput>();
    this.initNameInput();
    this._created = true;
    this._pool_buttons = new ObjectPoolGenericMono<OnomasticsAssetButton>(this.prefab_button_template, this.parent_name_variation_1);
    foreach (OnomasticsAsset pAsset in AssetManager.onomastics_library.list)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      OnomasticsTab.\u003C\u003Ec__DisplayClass49_0 cDisplayClass490 = new OnomasticsTab.\u003C\u003Ec__DisplayClass49_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass490.\u003C\u003E4__this = this;
      Transform transform = !pAsset.isGroupType() ? this.parent_asset_specials : this.parent_asset_groups;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass490.tB = Object.Instantiate<OnomasticsAssetButton>(this.prefab_button, transform);
      // ISSUE: reference to a compiler-generated field
      this.setupButton(cDisplayClass490.tB, pAsset);
      if (pAsset.isGroupType())
      {
        // ISSUE: reference to a compiler-generated field
        ((Component) cDisplayClass490.tB).GetComponent<TipButton>().showOnClick = false;
        // ISSUE: reference to a compiler-generated field
        ((Behaviour) ((Component) cDisplayClass490.tB).GetComponent<DraggableLayoutElement>()).enabled = false;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      ((UnityEvent) cDisplayClass490.tB.button.onClick).AddListener(new UnityAction((object) cDisplayClass490, __methodptr(\u003Ccreate\u003Eb__0)));
    }
  }

  private void clickAssetButton(OnomasticsAssetButton pButton)
  {
    if (!InputHelpers.mouseSupported)
    {
      if (!Tooltip.isShowingFor((object) pButton))
      {
        pButton.showTooltip();
        return;
      }
      Tooltip.hideTooltip();
    }
    if (this._selected_editor_group != pButton.onomastics_asset && pButton.isGroupType())
      this.openGroup(pButton);
    else if (pButton.isGroupType() && this._onomastics_data.isGroupEmpty(pButton.onomastics_asset.id))
      this.punch(((Component) this.parent_asset_editor_group).transform);
    else if (this._pool_buttons.countActive() >= 30)
    {
      this.punch(this.parent_name_variation_1.parent);
    }
    else
    {
      this.punch(((Component) pButton).transform);
      this.loadTemplateButton(pButton.onomastics_asset.id, true);
      this.resetNameGenerationTextBox();
    }
  }

  private void punch(
    Transform pTransformTarget,
    float pDefaultScale = 1f,
    float pPower = 0.1f,
    float pDuration = 0.3f)
  {
    ShortcutExtensions.DOKill((Component) pTransformTarget, true);
    pTransformTarget.localScale = new Vector3(pDefaultScale, pDefaultScale, pDefaultScale);
    ShortcutExtensions.DOPunchScale(pTransformTarget, new Vector3(pPower, pPower, pPower), pDuration, 10, 1f);
  }

  private void setupButton(OnomasticsAssetButton pButton, OnomasticsAsset pAsset)
  {
    pButton.setupButton(pAsset, new GetCurrentOnomasticsData(this.getOnomasticsData));
  }

  private void setupButton(OnomasticsAssetButton pButton, string pAssetID)
  {
    OnomasticsAsset pAsset = AssetManager.onomastics_library.get(pAssetID);
    this.setupButton(pButton, pAsset);
  }

  private void loadTemplateButton(string pID, bool pPunch = false)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    OnomasticsTab.\u003C\u003Ec__DisplayClass54_0 cDisplayClass540 = new OnomasticsTab.\u003C\u003Ec__DisplayClass54_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass540.\u003C\u003E4__this = this;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass540.tNewButton = this._pool_buttons.getNext();
    // ISSUE: reference to a compiler-generated field
    this.setupButton(cDisplayClass540.tNewButton, pID);
    // ISSUE: reference to a compiler-generated field
    ((Component) cDisplayClass540.tNewButton).transform.SetAsLastSibling();
    // ISSUE: reference to a compiler-generated field
    // ISSUE: method pointer
    ((UnityEvent) cDisplayClass540.tNewButton.button.onClick).AddListener(new UnityAction((object) cDisplayClass540, __methodptr(\u003CloadTemplateButton\u003Eb__0)));
    // ISSUE: reference to a compiler-generated field
    this.punch(((Component) cDisplayClass540.tNewButton).transform);
  }

  private void clickToRemoveButton(OnomasticsAssetButton pButton)
  {
    this._pool_buttons.release(pButton);
    this.resetNameGenerationTextBox();
    Tooltip.blockTooltips(0.01f);
  }

  public void openGroup(OnomasticsAssetButton pButton)
  {
    this._selected_editor_group = pButton.onomastics_asset;
    this._selected_editor_group_button = pButton;
    ((Component) this.parent_asset_editor_group).gameObject.SetActive(true);
    this._editor_input.setText(this._onomastics_data.getGroupString(this._selected_editor_group.id));
    this.icon_last_selected_group.sprite = pButton.onomastics_asset.getSprite();
  }

  public void loadFromTemplate(bool pReset = false)
  {
    this._onomastics_data = SelectedMetas.selected_culture.getOnomasticData(this._name_set_type, pReset);
    this.loadInitialButtons();
    this.openFirstGroup();
    this.resetNameGenerationTextBox();
  }

  public void loadNameSet() => this.loadFromTemplate();

  public void resetTemplate() => this.loadFromTemplate(true);

  public void clickRegenerateNames() => this.resetNameGenerationTextBox();

  public void randomEverything()
  {
    this._onomastics_data.clearTemplateData();
    int num = Randy.randomInt(3, 5);
    bool flag = Randy.randomBool();
    for (int index = 1; index <= num; ++index)
    {
      string pString = !flag ? this.getRandomConsonants() : this.getRandomVowels();
      flag = !flag;
      this._onomastics_data.setGroup("group_" + index.ToString(), pString);
    }
    this.fillRandomCards();
    this.loadInitialButtons();
    this.openFirstGroup();
    this.resetNameGenerationTextBox();
  }

  public void randomCards()
  {
    this.fillRandomCards();
    this.loadInitialButtons();
    this.openFirstGroup();
    this.resetNameGenerationTextBox();
  }

  public void fillRandomCards()
  {
    this._onomastics_data.clearTemplateData();
    using (ListPool<string> list = new ListPool<string>())
    {
      using (new ListPool<string>())
      {
        foreach (KeyValuePair<string, OnomasticsDataGroup> group in this._onomastics_data.groups)
        {
          if (!group.Value.isEmpty())
            list.Add(group.Key);
        }
        int num = Randy.randomInt(2, 6);
        for (int index = 0; index < num; ++index)
          this._onomastics_data.addToTemplateData(list.GetRandom<string>());
        for (int index = 0; index < num / 2; ++index)
          this._onomastics_data.addToTemplateData(AssetManager.onomastics_library.list_special.GetRandom<OnomasticsAsset>().id);
        this._onomastics_data.shuffleAllCards();
      }
    }
  }

  private string getRandomVowels()
  {
    string randomVowels = string.Empty;
    int num = Randy.randomInt(2, 4);
    for (int index = 0; index < num; ++index)
      randomVowels = $"{randomVowels}{OnomasticsTab.vowel_combinations[Randy.randomInt(0, OnomasticsTab.vowel_combinations.Length)]} ";
    return randomVowels;
  }

  private string getRandomConsonants()
  {
    string randomConsonants = string.Empty;
    int num = Randy.randomInt(2, 4);
    for (int index = 0; index < num; ++index)
      randomConsonants = $"{randomConsonants}{OnomasticsTab.consonant_combinations[Randy.randomInt(0, OnomasticsTab.consonant_combinations.Length)]} ";
    return randomConsonants;
  }

  public void saveToLibrary()
  {
    string currentTemplate = OnomasticsDropdown.current_template;
    int currentTemplateIndex = OnomasticsDropdown.current_template_index;
    Debug.Log((object) $"Saving to library: {currentTemplate} {currentTemplateIndex.ToString()}");
    string shortTemplate = this._onomastics_data.getShortTemplate();
    NameGeneratorAsset nameGeneratorAsset = AssetManager.name_generator.get(currentTemplate);
    if (currentTemplateIndex >= nameGeneratorAsset.onomastics_templates.Count)
      nameGeneratorAsset.onomastics_templates.Add(shortTemplate);
    else
      nameGeneratorAsset.onomastics_templates[currentTemplateIndex] = shortTemplate;
    AssetManager.name_generator.exportAssets();
  }

  public void saveToClipboard()
  {
    GUIUtility.systemCopyBuffer = $"`{this._onomastics_data.getShortTemplate()}`";
    WorldTip.showNow("onomastics_exported", pPosition: "top");
  }

  public void saveNamesToClipboard()
  {
    string str1 = "" + "## Template: \n\n" + this._onomastics_data.getShortTemplate() + "\n\n";
    string str2;
    if (this._onomastics_data.isGendered())
    {
      string str3 = "";
      string str4 = "";
      for (int index = 0; index < 25; ++index)
      {
        string name1 = this._onomastics_data.generateName(ActorSex.Male);
        str3 = $"{str3}- {name1}\n";
        string name2 = this._onomastics_data.generateName(ActorSex.Female);
        str4 = $"{str4}- {name2}\n";
      }
      str2 = $"{$"{str1}## Male names: \n\n{str3}" + "\n"}## Female names: \n\n{str4}";
    }
    else
    {
      str2 = str1 + "## Generated names: \n\n";
      for (int index = 0; index < 50; ++index)
      {
        string name = this._onomastics_data.generateName();
        str2 = $"{str2}- {name}\n";
      }
    }
    GUIUtility.systemCopyBuffer = str2;
  }

  public void loadFromClipboard() => this.loadTemplate(GUIUtility.systemCopyBuffer);

  public void loadTemplate(string pTemplate = null)
  {
    string shortTemplate = this._onomastics_data.getShortTemplate();
    string str;
    if (pTemplate == null)
      str = (string) null;
    else
      str = pTemplate.Trim('\n', '\r', ' ', '"', '`');
    if (str == null)
      str = "";
    pTemplate = str;
    try
    {
      if (!this._onomastics_data.templateIsValid(pTemplate))
        throw new ArgumentException("Invalid template format: (OT) " + pTemplate);
      this._onomastics_data.loadFromShortTemplate(pTemplate);
    }
    catch (ArgumentException ex)
    {
      WorldTip.showNow("onomastics_import_error_invalid", pPosition: "top", pColor: "#FF637D");
      Debug.LogWarning((object) ex.Message);
      return;
    }
    catch (Exception ex)
    {
      WorldTip.showNow("onomastics_import_error_logs", pPosition: "top", pColor: "#FF637D");
      Debug.LogWarning((object) ex.Message);
      return;
    }
    Debug.Log((object) ("old: " + shortTemplate.Trim('\n', '\r', ' ', '"', '`')));
    Debug.Log((object) ("new: " + pTemplate));
    WorldTip.showNow(pTemplate, false, "top");
    this.loadInitialButtons();
    this.openFirstGroup();
    this.resetNameGenerationTextBox();
  }

  public static string debugTemplateReport(string pTemplateName)
  {
    OnomasticsData originalData = OnomasticsCache.getOriginalData(AssetManager.name_generator.get(pTemplateName).onomastics_templates.GetRandom<string>());
    string str1 = "" + "## Template: \n\n" + originalData.getShortTemplate() + "\n\n";
    string str2;
    if (originalData.isGendered())
    {
      string str3 = "";
      string str4 = "";
      for (int index = 0; index < 25; ++index)
      {
        string name1 = originalData.generateName(ActorSex.Male);
        if (index > 0)
          str3 += ", ";
        str3 += name1;
        string name2 = originalData.generateName(ActorSex.Female);
        if (index > 0)
          str4 += ", ";
        str4 += name2;
      }
      str2 = $"{$"{str1}## Male names: \n\n{str3}" + "\n"}## Female names: \n\n{str4}";
    }
    else
    {
      str2 = str1 + "## Generated names: \n\n";
      for (int index = 0; index < 50; ++index)
      {
        string name = originalData.generateName();
        if (index > 0)
          str2 += ", ";
        str2 += name;
      }
    }
    return str2 + "\n\n";
  }
}
