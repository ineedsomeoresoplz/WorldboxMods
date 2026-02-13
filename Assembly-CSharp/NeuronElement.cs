// Decompiled with JetBrains decompiler
// Type: NeuronElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class NeuronElement : 
  MonoBehaviour,
  IBeginDragHandler,
  IEventSystemHandler,
  IDragHandler,
  IEndDragHandler,
  IInitializePotentialDragHandler
{
  [SerializeField]
  public Image image;
  private const float SCALE_HIGHLIGHTED = 1.6f;
  private const float SCALE_SPAWN_IMPULSE = 1.5f;
  private const float SCALE_RECEIVE_IMPULSE_INCREASE = 0.1f;
  private const float SCALE_NORMAL = 1f;
  internal float render_depth;
  internal bool highlighted;
  internal List<NeuronElement> connected_neurons = new List<NeuronElement>();
  internal float scale_mod_spawn = 1f;
  internal float bonus_scale = 1f;
  internal DecisionAsset decision;
  internal Actor actor;
  private float _spawn_interval;
  private float _spawn_timer;
  private List<AxonElement> _axons = new List<AxonElement>();
  private NeuronsOverview _neurons_overview;
  private TooltipData _tooltip_data;
  private bool _center;

  private void Start()
  {
    this._neurons_overview = ((Component) this).gameObject.GetComponentInParent<NeuronsOverview>();
    this.initClick();
    this.initTooltip();
  }

  protected void initClick()
  {
    Button button;
    if (!((Component) this).TryGetComponent<Button>(ref button))
      return;
    // ISSUE: method pointer
    ((UnityEvent) button.onClick).AddListener(new UnityAction((object) this, __methodptr(setPressed)));
  }

  protected void initTooltip()
  {
    TipButton tipButton;
    if (!((Component) this).TryGetComponent<TipButton>(ref tipButton))
      return;
    Object.Destroy((Object) tipButton);
  }

  private void showTooltip()
  {
    if (this.isCenter())
    {
      Tooltip.show((object) this, "tip", new TooltipData()
      {
        tip_name = "toggle_all_neurons",
        tip_description = "toggle_all_neurons_description"
      });
    }
    else
    {
      if (!this.hasDecisionSet())
        return;
      this._tooltip_data = new TooltipData()
      {
        neuron = this,
        tooltip_scale = Mathf.Lerp(0.4f, 1f, this.render_depth)
      };
      Tooltip.show((object) this, "neuron", this._tooltip_data);
    }
  }

  public void setupDecisionAndActor(DecisionAsset pAsset, Actor pActor)
  {
    this.decision = pAsset;
    this.actor = pActor;
    this.image.sprite = this.decision.getSprite();
    this._spawn_interval = (float) this.decision.cooldown;
    this._spawn_timer = Randy.randomFloat(0.0f, this._spawn_interval);
  }

  public void updateColorsAndTooltip()
  {
    if (!this.highlighted)
    {
      this.scale_mod_spawn -= Time.deltaTime * 2f;
      this.scale_mod_spawn = Mathf.Max(1f, this.scale_mod_spawn);
    }
    else
    {
      if (!this.hasDecisionSet() || !Tooltip.isShowingFor((object) this))
        return;
      this._tooltip_data.tooltip_scale = Mathf.Lerp(0.4f, 1f, this.render_depth);
    }
  }

  public void updateSpawnTimer()
  {
    if ((double) this._spawn_timer <= 0.0 || !this.isDecisionEnabled())
      return;
    this._spawn_timer -= Time.deltaTime;
  }

  public void spawnImpulseFromHere()
  {
    this.scale_mod_spawn = Math.Max(1.5f, this.scale_mod_spawn);
    this._spawn_timer = this._spawn_interval;
    foreach (AxonElement axon in this._axons)
      axon.mod_light = 1f;
  }

  public bool isDecisionEnabled()
  {
    return !this.hasDecisionSet() || this.actor.isDecisionEnabled(this.decision.decision_index);
  }

  public void setHighlighted()
  {
    if (this.highlighted)
      return;
    this.highlighted = true;
    this.scale_mod_spawn = 1.6f;
    this.showTooltip();
  }

  public void setPressed()
  {
    if (this._neurons_overview.isDragging() || !this.hasDecisionSet() && !this.isCenter())
      return;
    this._neurons_overview.setLatestTouched(this);
    if (!InputHelpers.mouseSupported)
    {
      if (!Tooltip.isShowingFor((object) this))
      {
        this.showTooltip();
        return;
      }
    }
    else
      this.showTooltip();
    this.scale_mod_spawn = 1.6f;
    if (this.isCenter())
      this.centerBrainClick();
    else
      this.actor.switchDecisionState(this.decision.decision_index);
    this.actor.makeConfused(pColorEffect: true);
    this._neurons_overview.fireImpulseWaveFromHere(this);
    this._neurons_overview.startNewWhat();
    this.actor.updateStats();
    ((Component) this).GetComponentInParent<StatsWindow>().updateStats();
    AchievementLibrary.mindless_husk.check((object) this.actor);
  }

  private void centerBrainClick()
  {
    this._neurons_overview.switchAllNeurons();
    if (this._neurons_overview.getAllState())
      ((Graphic) this.image).color = Color32.op_Implicit(Toolbox.color_clear);
    else
      ((Graphic) this.image).color = Toolbox.color_white;
  }

  public void receiveImpulse()
  {
    this.scale_mod_spawn += 0.1f;
    this.scale_mod_spawn = Mathf.Min(1.5f, this.scale_mod_spawn);
  }

  public bool readyToSpawnImpulse()
  {
    return this.isDecisionEnabled() && (double) this._spawn_timer <= 0.0;
  }

  public int getSimulatedTimer() => (int) this._spawn_timer;

  public void clear()
  {
    this.decision = (DecisionAsset) null;
    this.actor = (Actor) null;
    this.connected_neurons.Clear();
    this._axons.Clear();
    this._center = false;
  }

  public void setColor(Color pColor) => ((Graphic) this.image).color = pColor;

  public bool hasDecisionSet() => this.decision != null;

  public void addConnection(NeuronElement pConnection, AxonElement pAxon)
  {
    this.connected_neurons.Add(pConnection);
    this._axons.Add(pAxon);
  }

  public void setCenter(bool pState) => this._center = pState;

  public bool isCenter() => this._center;

  public void OnInitializePotentialDrag(PointerEventData pEventData)
  {
    ((Component) this._neurons_overview)?.SendMessage(nameof (OnInitializePotentialDrag), (object) pEventData);
  }

  public void OnBeginDrag(PointerEventData pEventData)
  {
    ((Component) this._neurons_overview)?.SendMessage(nameof (OnBeginDrag), (object) pEventData);
  }

  public void OnDrag(PointerEventData pEventData)
  {
    ((Component) this._neurons_overview)?.SendMessage(nameof (OnDrag), (object) pEventData);
  }

  public void OnEndDrag(PointerEventData pEventData)
  {
    ((Component) this._neurons_overview)?.SendMessage(nameof (OnEndDrag), (object) pEventData);
  }
}
