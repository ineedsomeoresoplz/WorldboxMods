// Decompiled with JetBrains decompiler
// Type: AugmentationsEditor`7
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class AugmentationsEditor<TAugmentation, TAugmentationButton, TAugmentationEditorButton, TAugmentationGroupAsset, TAugmentationGroup, TAugmentationWindow, TEditorInterface> : 
  BaseAugmentationsEditor
  where TAugmentation : BaseAugmentationAsset
  where TAugmentationButton : AugmentationButton<TAugmentation>
  where TAugmentationEditorButton : AugmentationEditorButton<TAugmentationButton, TAugmentation>
  where TAugmentationGroupAsset : BaseCategoryAsset
  where TAugmentationGroup : AugmentationCategory<TAugmentation, TAugmentationButton, TAugmentationEditorButton>
  where TAugmentationWindow : IAugmentationsWindow<TEditorInterface>
  where TEditorInterface : IAugmentationsEditor
{
  private const float FOCUS_SCROLL_OFFSET_TOP = -5f;
  private const float FOCUS_SCROLL_OFFSET_BOTTOM = 1f;
  public const float FOCUS_SCROLL_DURATION = 0.3f;
  [SerializeField]
  protected Image art;
  public TAugmentationButton prefab_augmentation;
  public TAugmentationEditorButton prefab_editor_augmentation;
  public TAugmentationGroup prefab_augmentation_group;
  protected readonly Dictionary<string, TAugmentationGroup> dict_groups = new Dictionary<string, TAugmentationGroup>();
  protected readonly List<TAugmentationEditorButton> all_augmentation_buttons = new List<TAugmentationEditorButton>();
  protected TAugmentationWindow augmentation_window;
  protected ObjectPoolGenericMono<TAugmentationButton> selected_editor_buttons;
  [SerializeField]
  private WindowMetaTab _editor_tab;

  protected virtual List<TAugmentationGroupAsset> augmentation_groups_list
  {
    get => throw new NotImplementedException();
  }

  protected virtual List<TAugmentation> all_augmentations_list
  {
    get => throw new NotImplementedException();
  }

  protected virtual TAugmentation edited_marker_augmentation => default (TAugmentation);

  protected override void create()
  {
    base.create();
    this.augmentation_window = ((Component) this).GetComponentInParent<TAugmentationWindow>();
    if (!this.rain_editor)
      return;
    this.selected_editor_buttons = new ObjectPoolGenericMono<TAugmentationButton>(this.prefab_augmentation, ((Component) this.selected_editor_augmentations_grid).transform);
  }

  protected override void OnEnable()
  {
    if (this.rain_editor)
      this.onEnableRain();
    base.OnEnable();
  }

  protected virtual ListPool<TAugmentation> getOrderedAugmentationsList()
  {
    ListPool<TAugmentation> augmentationsList1 = new ListPool<TAugmentation>((ICollection<TAugmentation>) this.all_augmentations_list);
    augmentationsList1.Sort((Comparison<TAugmentation>) ((pT1, pT2) =>
    {
      int augmentationsList2 = pT2.priority.CompareTo(pT1.priority);
      if (augmentationsList2 == 0)
        augmentationsList2 = StringComparer.Ordinal.Compare(pT1.id, pT2.id);
      return augmentationsList2;
    }));
    return augmentationsList1;
  }

  public override void reloadButtons()
  {
    base.reloadButtons();
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    foreach (TAugmentationEditorButton augmentationButton in this.all_augmentation_buttons)
    {
      int num4 = this.isAugmentationAvailable(augmentationButton.augmentation_button) ? 1 : 0;
      TAugmentation elementAsset = augmentationButton.augmentation_button.getElementAsset();
      ++num3;
      if (num4 != 0)
        ++num2;
      ((Component) augmentationButton.selected_icon).gameObject.SetActive(false);
      if (num4 != 0)
        ((Graphic) augmentationButton.augmentation_button.image).color = Toolbox.color_augmentation_unselected;
      int num5 = elementAsset.can_be_given ? 1 : 0;
      bool pSelected = false;
      if (num5 == 0)
      {
        bool flag = !this.rain_editor && this.hasAugmentation(augmentationButton.augmentation_button);
        ((Component) augmentationButton.selected_icon).gameObject.SetActive(flag);
        ((Graphic) augmentationButton.selected_icon).color = Toolbox.color_log_warning;
        if (flag)
        {
          ++num1;
          pSelected = true;
        }
      }
      else if (this.rain_editor && this.augmentations_hashset.Contains(augmentationButton.augmentation_button.getElementId()))
      {
        Color color = this.rain_editor_state != RainState.Add ? ColorStyleLibrary.m.getSelectorRemoveColor() : ColorStyleLibrary.m.getSelectorColor();
        ((Component) augmentationButton.selected_icon).gameObject.SetActive(true);
        ((Graphic) augmentationButton.selected_icon).color = color;
        pSelected = true;
      }
      else if (!this.rain_editor && this.hasAugmentation(augmentationButton.augmentation_button))
      {
        ((Component) augmentationButton.selected_icon).gameObject.SetActive(true);
        ((Graphic) augmentationButton.selected_icon).color = ColorStyleLibrary.m.getSelectorColor();
        pSelected = true;
        ++num1;
      }
      augmentationButton.augmentation_button.updateIconColor(pSelected);
    }
    foreach (TAugmentationGroup augmentationGroup in this.dict_groups.Values)
    {
      if (augmentationGroup.asset.show_counter)
        augmentationGroup.updateCounter();
      else
        augmentationGroup.hideCounter();
    }
    if (this.rain_editor)
      this.text_counter_augmentations.text = $"{num2.ToString()}/{num3.ToString()}";
    else
      this.text_counter_augmentations.text = $"{num1.ToString()}/{num3.ToString()}";
    this.startSignal();
  }

  protected override void groupsBuilder()
  {
    // ISSUE: unable to decompile the method.
  }

  protected override void checkEnabledGroups()
  {
    foreach (TAugmentationGroup augmentationGroup in this.dict_groups.Values)
      ((Component) (object) augmentationGroup).gameObject.SetActive(augmentationGroup.countActiveButtons() > 0);
  }

  protected void editorButtonClick(TAugmentationEditorButton pButton)
  {
    if (!InputHelpers.mouseSupported && !Tooltip.isShowingFor((object) pButton.augmentation_button))
      return;
    if (!Config.hasPremium)
    {
      ScrollWindow.showWindow("premium_menu");
    }
    else
    {
      if (!pButton.augmentation_button.getElementAsset().can_be_given)
        return;
      if (this.rain_editor)
        this.rainAugmentationClick(pButton);
      else
        this.metaAugmentationClick(pButton);
      this.reloadButtons();
    }
  }

  protected virtual void metaAugmentationClick(TAugmentationEditorButton pButton)
  {
    this.showActiveButtons();
    this.refreshAugmentationWindow();
  }

  protected virtual void rainAugmentationClick(TAugmentationEditorButton pButton)
  {
    this.saveRainValues();
    this.loadEditorSelectedAugmentations();
  }

  protected virtual void validateRainData()
  {
    this.augmentations_list_link.RemoveAll((Predicate<string>) (tId =>
    {
      TAugmentation augmentation = this.all_augmentations_list.Find((Predicate<TAugmentation>) (tAugmentation => tAugmentation.id == tId));
      return (object) augmentation == null || !augmentation.isAvailable();
    }));
  }

  protected virtual void refreshAugmentationWindow()
  {
    this.augmentation_window.updateStats();
    this.augmentation_window.reloadBanner();
  }

  protected void saveRainValues()
  {
    this.augmentations_list_link.Clear();
    foreach (string str in this.augmentations_hashset)
      this.augmentations_list_link.Add(str);
    PlayerConfig.saveData();
  }

  protected virtual void loadEditorSelectedAugmentations()
  {
    this.selected_editor_buttons.clear();
    foreach (string str in this.augmentations_hashset)
    {
      if (this.isAugmentationExists(str))
        this.loadEditorSelectedButton(this.selected_editor_buttons.getNext(), str);
    }
  }

  public void scrollToGroupStarter(GameObject pButton) => this.scrollToGroupStarter(pButton, false);

  public virtual void scrollToGroupStarter(GameObject pButton, bool pIgnoreTooltipCheck)
  {
    if (!pIgnoreTooltipCheck && !InputHelpers.mouseSupported && !Tooltip.isShowingFor((object) pButton.GetComponent<TAugmentationButton>()))
      return;
    bool pWithDelay = false;
    if (!((Component) this).gameObject.activeInHierarchy)
    {
      if (!Object.op_Inequality((Object) this._editor_tab, (Object) null))
        return;
      this._editor_tab.container.showTab(this._editor_tab);
      pWithDelay = true;
    }
    this.StartCoroutine(this.scrollToGroupStarterRoutine(pButton, pWithDelay));
  }

  private IEnumerator scrollToGroupStarterRoutine(GameObject pButton, bool pWithDelay)
  {
    if (pWithDelay)
      yield return (object) new WaitForSeconds(Config.getScrollToGroupDelay());
    this.scrollToGroup(pButton);
  }

  private void scrollToGroup(GameObject pButton, float pDuration = 0.3f)
  {
    TAugmentationGroup augmentationGroup1 = default (TAugmentationGroup);
    foreach (TAugmentationGroup augmentationGroup2 in this.dict_groups.Values)
    {
      TAugmentationButton component = pButton.GetComponent<TAugmentationButton>();
      if (augmentationGroup2.hasAugmentation(component.getElementAsset()))
      {
        augmentationGroup1 = augmentationGroup2;
        break;
      }
    }
    if (Object.op_Equality((Object) (object) augmentationGroup1, (Object) null))
      return;
    RectTransform transform1 = ((Component) pButton.GetComponentInParent<HeaderContainer>()).transform as RectTransform;
    RectTransform component1 = ((Component) ((Component) this).transform.parent).GetComponent<RectTransform>();
    RectTransform component2 = ((Component) ((Transform) component1).parent).GetComponent<RectTransform>();
    RectTransform transform2 = ((Component) this).transform as RectTransform;
    RectTransform component3 = ((Component) (object) augmentationGroup1).GetComponent<RectTransform>();
    Rect rect = component2.rect;
    float height1 = ((Rect) ref rect).height;
    rect = transform1.rect;
    float height2 = ((Rect) ref rect).height;
    rect = component1.rect;
    float height3 = ((Rect) ref rect).height;
    rect = transform2.rect;
    float height4 = ((Rect) ref rect).height;
    rect = component3.rect;
    float height5 = ((Rect) ref rect).height;
    float num1 = Mathf.Abs(transform2.anchoredPosition.y) - height4 * (1f - transform2.pivot.y) - height2;
    float num2 = Mathf.Abs(component3.anchoredPosition.y) - height5 * (1f - component3.pivot.y) + num1;
    float num3 = num2 + height5;
    bool flag1 = (double) num2 < (double) ((Transform) component1).localPosition.y;
    bool flag2 = (double) num3 > (double) ((Transform) component1).localPosition.y + (double) height1 - (double) height2;
    if ((flag1 ? 0 : (!flag2 ? 1 : 0)) != 0)
      return;
    float num4 = Mathf.Clamp(!flag1 ? num3 - height1 + height2 + 1f : num2 - -5f, 0.0f, height3 - height1);
    ShortcutExtensions.DOLocalMoveY((Transform) component1, num4, pDuration, false);
  }

  protected virtual bool isAugmentationExists(string pId) => throw new NotImplementedException();

  protected virtual void loadEditorSelectedButton(
    TAugmentationButton pButton,
    string pAugmentationId)
  {
    pButton.removeClickAction(new AugmentationButtonClickAction(this.scrollToGroupStarter));
    pButton.addClickAction(new AugmentationButtonClickAction(this.scrollToGroupStarter));
  }

  protected virtual void createButton(TAugmentation pElement, TAugmentationGroup pGroup)
  {
    throw new NotImplementedException();
  }

  protected virtual bool hasAugmentation(TAugmentationButton pButton)
  {
    throw new NotImplementedException();
  }

  protected virtual bool addAugmentation(TAugmentationButton pButton)
  {
    throw new NotImplementedException();
  }

  protected virtual bool removeAugmentation(TAugmentationButton pButton)
  {
    throw new NotImplementedException();
  }

  public WindowMetaTab getEditorTab() => this._editor_tab;

  protected bool isAugmentationAvailable(TAugmentationButton pButton)
  {
    return pButton.getElementAsset().isAvailable();
  }
}
