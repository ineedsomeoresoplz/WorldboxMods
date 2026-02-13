// Decompiled with JetBrains decompiler
// Type: BaseAugmentationsEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using LayoutGroupExt;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class BaseAugmentationsEditor : MonoBehaviour
{
  public Transform augmentation_groups_parent;
  public Text text_counter_augmentations;
  public LocalizedText window_title_text;
  public Image power_icon;
  public Transform powers_icons;
  public GridLayoutGroupExtended selected_editor_augmentations_grid;
  public RainSwitcherButton rain_state_switcher;
  protected List<string> augmentations_list_link;
  protected readonly HashSet<string> augmentations_hashset = new HashSet<string>();
  public bool rain_editor;
  public RainState rain_editor_state;
  private bool _groups_initialized;
  private bool _created;
  private StatsWindow _stats_window;
  protected ToggleRainStateAction rain_state_toggle_action;

  private void Awake()
  {
    this.create();
    this._stats_window = ((Component) this).GetComponentInParent<StatsWindow>();
    // ISSUE: method pointer
    ((UnityEvent) this.rain_state_switcher?.getButton().onClick).AddListener(new UnityAction((object) this, __methodptr(\u003CAwake\u003Eb__15_0)));
  }

  protected virtual void OnEnable()
  {
    this.reloadButtons();
    this.checkEnabledGroups();
    if (this.rain_editor)
      return;
    this._stats_window.updateStats();
  }

  protected virtual void create()
  {
    if (this._created)
      return;
    this._created = true;
  }

  protected virtual void onEnableRain() => throw new NotImplementedException();

  public virtual void reloadButtons()
  {
    if (!((Component) this).gameObject.activeInHierarchy)
      return;
    this.loadAugmentationGroups();
  }

  protected virtual void showActiveButtons() => throw new NotImplementedException();

  private void loadAugmentationGroups()
  {
    if (this._groups_initialized)
      return;
    this._groups_initialized = true;
    this.groupsBuilder();
  }

  protected virtual void checkEnabledGroups() => throw new NotImplementedException();

  protected virtual void groupsBuilder() => throw new NotImplementedException();

  protected virtual void startSignal()
  {
  }

  protected virtual void onNanoWasModified() => throw new NotImplementedException();

  protected virtual void toggleRainState(ref RainState pState)
  {
    if (pState == RainState.Add)
    {
      pState = RainState.Remove;
      this.rain_state_switcher.toggleState(true);
    }
    else
    {
      pState = RainState.Add;
      this.rain_state_switcher.toggleState(false);
    }
    this.rain_editor_state = pState;
    this.reloadButtons();
  }
}
