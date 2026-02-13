// Decompiled with JetBrains decompiler
// Type: NeuronsOverview
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class NeuronsOverview : 
  UnitElement,
  IInitializePotentialDragHandler,
  IEventSystemHandler,
  IBeginDragHandler,
  IDragHandler,
  IEndDragHandler
{
  private const float DRAGGING_SMOOTHING_TIME = 0.1f;
  private const float ROTATION_BOUNDS = 0.7f;
  private const float ROTATION_BOUNDS_MARGIN = 1.05f;
  private const float DRAG_SPEED = 0.46f;
  private const float DRAG_ROTATE_SPEED = 0.005f;
  private const float MIN_NEURON_CURSOR_DISTANCE = 40f;
  private const float RADIUS_NEURONS = 70f;
  private const float NEURON_SCALE_MIN = 0.8f;
  private const float NEURON_SCALE_MAX = 1.5f;
  private const float BASE_AXON_DISTANCE = 250f;
  private const float DISTANCE_SCALING_FACTOR = 1.5f;
  [SerializeField]
  private NeuronElement _prefab_neuron;
  [SerializeField]
  private NerveImpulseElement _prefab_nerve_impulse;
  [SerializeField]
  private AxonElement _prefab_axon;
  [SerializeField]
  private RectTransform _parent_axons;
  [SerializeField]
  private RectTransform _parent_nerve_impulses;
  [SerializeField]
  private RectTransform _parent_neurons;
  [SerializeField]
  private GameObject _mind_main;
  [SerializeField]
  private UnitTextManager _text_phrases;
  private ObjectPoolGenericMono<NeuronElement> _pool_neurons;
  private ObjectPoolGenericMono<NerveImpulseElement> _pool_impulses;
  private ObjectPoolGenericMono<AxonElement> _pool_axons;
  private List<NeuronElement> _neurons = new List<NeuronElement>();
  private NeuronElement _last_activated_neuron;
  private List<NerveImpulseElement> _active_impulses = new List<NerveImpulseElement>();
  private Color _color_neuron_disabled_front = Toolbox.makeColor("#111111");
  private Color _color_neuron_disabled_back = Toolbox.makeColor("#111111", 0.3f);
  private Color _color_neuron_back = Toolbox.makeColor("#A9A9A9", 0.3f);
  private Color _color_neuron_front = Toolbox.makeColor("#DDDDDD");
  private Color _color_axon_default = Toolbox.makeColor("#FFFFFF", 0.1f);
  private Color _color_axon_default_center = Toolbox.makeColor("#FF6666", 0.1f);
  private Color _color_light_axon = Toolbox.makeColor("#3AFFFF", 0.54f);
  private Color _neuron_highlighted = Toolbox.makeColor("#FFFFFF");
  private float _offset_target_x = -0.015f;
  private float _offset_target_y = 0.07f;
  private bool _is_dragging;
  private Vector2 _last_mouse_delta;
  private float _offset_x;
  private float _offset_y;
  private int _decision_counter;
  private DecisionAsset[] _decision_assets;
  public static NeuronsOverview instance;
  private NeuronElement _active_neuron;
  private NeuronElement _latest_touched_neuron;
  private bool _all_state = true;

  private void Start() => NeuronsOverview.instance = this;

  protected override void Awake()
  {
    base.Awake();
    this._pool_neurons = new ObjectPoolGenericMono<NeuronElement>(this._prefab_neuron, (Transform) this._parent_neurons);
    this._pool_impulses = new ObjectPoolGenericMono<NerveImpulseElement>(this._prefab_nerve_impulse, (Transform) this._parent_nerve_impulses);
    this._pool_axons = new ObjectPoolGenericMono<AxonElement>(this._prefab_axon, (Transform) this._parent_axons);
  }

  public void OnInitializePotentialDrag(PointerEventData eventData)
  {
    eventData.useDragThreshold = false;
    this._last_mouse_delta = Vector2.zero;
  }

  public void OnBeginDrag(PointerEventData eventData)
  {
    this._offset_x = this._offset_target_x = 0.0f;
    this._offset_y = this._offset_target_y = 0.0f;
    this.clearHighlight();
    Tooltip.hideTooltipNow();
  }

  private void highlightAllAxons(float pLight)
  {
    foreach (AxonElement axonElement in (IEnumerable<AxonElement>) this._pool_axons.getListTotal())
    {
      if ((double) axonElement.mod_light <= (double) pLight)
        axonElement.mod_light = pLight;
    }
  }

  public void OnDrag(PointerEventData eventData)
  {
    this._is_dragging = true;
    Vector2 delta = eventData.delta;
    if ((double) ((Vector2) ref delta).magnitude > (double) ((Vector2) ref this._last_mouse_delta).magnitude)
      this.highlightAllAxons(0.35f);
    this._last_mouse_delta = delta;
    this._offset_x = (float) (-(double) delta.y * 0.46000000834465027);
    this._offset_y = delta.x * 0.46f;
    this.updateNeuronsVisual();
  }

  public void OnEndDrag(PointerEventData eventData)
  {
    this._is_dragging = false;
    Vector2 delta = eventData.delta;
    this._offset_target_x += (float) (-(double) delta.y * 0.004999999888241291);
    this._offset_target_y += delta.x * 0.005f;
    if ((double) Mathf.Abs(this._offset_target_x) > 0.699999988079071 || (double) Mathf.Abs(this._offset_target_y) > 0.699999988079071)
    {
      if ((double) Mathf.Abs(this._offset_target_x) > (double) Mathf.Abs(this._offset_target_y))
        this._offset_target_y = (float) ((double) this._offset_target_y / (double) Mathf.Abs(this._offset_target_x) * 0.699999988079071);
      else
        this._offset_target_x = (float) ((double) this._offset_target_x / (double) Mathf.Abs(this._offset_target_y) * 0.699999988079071);
    }
    this._offset_target_x = Mathf.Clamp(this._offset_target_x, -0.7f, 0.7f);
    this._offset_target_y = Mathf.Clamp(this._offset_target_y, -0.7f, 0.7f);
    this.highlightAllAxons(1f);
    this.fireImpulsesEverywhere();
  }

  private void fireImpulsesEverywhere()
  {
    for (int index = 0; index < this._neurons.Count; ++index)
      this.fireImpulseWaveFromHere(this._neurons[index], 2);
  }

  private void highlightNeuron(NeuronElement pHighlighted = null)
  {
    foreach (NeuronElement neuron in this._neurons)
    {
      if (!Object.op_Equality((Object) neuron, (Object) pHighlighted) && neuron.highlighted)
      {
        neuron.highlighted = false;
        Tooltip.hideTooltipNow();
      }
    }
    pHighlighted?.setHighlighted();
  }

  private NeuronElement getClosestNeuronToCursor()
  {
    NeuronElement closestNeuronToCursor = (NeuronElement) null;
    float num1 = float.MaxValue;
    Vector2 vector2_1 = Vector2.op_Implicit(Input.mousePosition);
    foreach (NeuronElement neuron in this._neurons)
    {
      Vector2 vector2_2 = Vector2.op_Implicit(((Component) neuron).transform.position);
      float num2 = Vector2.Distance(vector2_1, vector2_2);
      if ((double) num2 <= 40.0)
      {
        if (Object.op_Equality((Object) neuron, (Object) this._active_neuron))
          return neuron;
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          closestNeuronToCursor = neuron;
        }
      }
    }
    return closestNeuronToCursor;
  }

  private void prepareAxons()
  {
    float num = (float) (250.0 / (double) Mathf.Sqrt((float) this._neurons.Count) * 1.5);
    for (int index1 = 0; index1 < this._neurons.Count - 1; ++index1)
    {
      NeuronElement neuron1 = this._neurons[index1];
      if (!neuron1.isCenter())
      {
        for (int index2 = index1 + 1; index2 < this._neurons.Count; ++index2)
        {
          NeuronElement neuron2 = this._neurons[index2];
          if (!neuron2.isCenter() && (double) Vector3.Distance(((Component) neuron1).transform.localPosition, ((Component) neuron2).transform.localPosition) <= (double) num)
            this.makeAxon(neuron1, neuron2);
        }
      }
    }
  }

  private AxonElement makeAxon(NeuronElement pNeuron1, NeuronElement pNeuron2)
  {
    AxonElement next = this._pool_axons.getNext();
    next.neuron_1 = pNeuron1;
    next.neuron_2 = pNeuron2;
    pNeuron1.addConnection(pNeuron2, next);
    pNeuron2.addConnection(pNeuron1, next);
    return next;
  }

  private void checkActorDecisions()
  {
    DecisionHelper.runSimulationForMindTab(this.actor);
    this._decision_counter = DecisionHelper.decision_system.getCounter();
    this._decision_assets = DecisionHelper.decision_system.getActions();
  }

  private void updateNeuronsVisual()
  {
    Quaternion quaternion = Quaternion.Euler(this._offset_x, this._offset_y, 0.0f);
    foreach (NeuronElement neuron in this._neurons)
    {
      neuron.updateColorsAndTooltip();
      Vector3 vector3 = Quaternion.op_Multiply(quaternion, ((Component) neuron).transform.localPosition);
      ((Component) neuron).transform.localPosition = vector3;
      this.calculateNeuronDepth(neuron, 70f);
      this.updateNeuronColorAndScale(neuron);
    }
    this.sortNeuronsByDepth();
  }

  private void updateNeuronImpulseAutoSpawn()
  {
    foreach (NeuronElement neuron in this._neurons)
      neuron.updateSpawnTimer();
  }

  private void sortNeuronsByDepth()
  {
    foreach (Component neuron in this._neurons)
      neuron.transform.SetAsLastSibling();
    this._neurons.Sort((Comparison<NeuronElement>) ((a, b) => a.render_depth.CompareTo(b.render_depth)));
  }

  private void calculateNeuronDepth(NeuronElement pNeuronElement, float pRadius)
  {
    float z = ((Component) pNeuronElement).transform.localPosition.z;
    float num = Mathf.InverseLerp(-pRadius, pRadius, z);
    pNeuronElement.render_depth = num;
  }

  private void updateNeuronColorAndScale(NeuronElement pElement)
  {
    if (!pElement.isDecisionEnabled())
    {
      Color pColor = Color.Lerp(this._color_neuron_disabled_back, this._color_neuron_disabled_front, pElement.render_depth);
      pElement.setColor(pColor);
    }
    else if (pElement.highlighted)
    {
      Color pColor = Color.Lerp(this._color_neuron_back, this._neuron_highlighted, pElement.render_depth);
      pElement.setColor(pColor);
    }
    else
    {
      Color pColor = Color.Lerp(this._color_neuron_back, this._color_neuron_front, pElement.render_depth);
      pElement.setColor(pColor);
    }
    float num = Mathf.Lerp(0.8f, 1.5f, pElement.render_depth) * (pElement.scale_mod_spawn * pElement.bonus_scale);
    ((Component) pElement).transform.localScale = new Vector3(num, num, num);
  }

  private void updateAxonPositions()
  {
    foreach (AxonElement axonElement in (IEnumerable<AxonElement>) this._pool_axons.getListTotal())
    {
      axonElement.update();
      float num1 = 1f;
      NeuronElement neuron1 = axonElement.neuron_1;
      NeuronElement neuron2 = axonElement.neuron_2;
      if (neuron1.highlighted || neuron2.highlighted)
        num1 = 6f;
      Color color1 = this._color_axon_default;
      if (axonElement.axon_center)
      {
        color1 = this._color_axon_default_center;
        num1 = 7f;
      }
      if ((double) axonElement.mod_light > 0.0)
      {
        Color color2 = Color.Lerp(color1, this._color_light_axon, axonElement.mod_light);
        ((Graphic) axonElement.image).color = color2;
      }
      else
        ((Graphic) axonElement.image).color = color1;
      Vector2 vector2_1 = Vector2.op_Implicit(((Component) neuron1).transform.localPosition);
      Vector2 vector2_2 = Vector2.op_Implicit(((Component) neuron2).transform.localPosition);
      Vector2 vector2_3 = Vector2.op_Division(Vector2.op_Addition(vector2_1, vector2_2), 2f);
      ((Component) axonElement).transform.localPosition = Vector2.op_Implicit(vector2_3);
      float num2 = Vector3.Distance(Vector2.op_Implicit(vector2_1), Vector2.op_Implicit(vector2_2));
      ((Component) axonElement).transform.localScale = new Vector3(num2, num1, 1f);
      Vector3 vector3 = Vector2.op_Implicit(Vector2.op_Subtraction(vector2_2, vector2_1));
      float num3 = Mathf.Atan2(vector3.y, vector3.x) * 57.29578f;
      ((Component) axonElement).transform.rotation = Quaternion.Euler(0.0f, 0.0f, num3);
    }
  }

  private void smoothOffsets()
  {
    this._offset_x = Mathf.Lerp(this._offset_x, this._offset_target_x, 0.1f);
    this._offset_y = Mathf.Lerp(this._offset_y, this._offset_target_y, 0.1f);
  }

  internal void fireImpulseWaveFromHere(NeuronElement pNeuron, int pWaves = 4)
  {
    if (!pNeuron.isDecisionEnabled())
      return;
    foreach (NeuronElement connectedNeuron in pNeuron.connected_neurons)
    {
      if (connectedNeuron.isDecisionEnabled())
        this.fireImpulse(pNeuron, connectedNeuron, pWaves);
    }
  }

  private void fireImpulseFrom(
    NeuronElement pPresynapticNeuron,
    int pWave,
    NeuronElement pIgnoreNeuron = null)
  {
    if (pPresynapticNeuron.connected_neurons.Count == 0)
      return;
    NeuronElement random;
    if (Object.op_Equality((Object) pIgnoreNeuron, (Object) null))
    {
      random = pPresynapticNeuron.connected_neurons.GetRandom<NeuronElement>();
    }
    else
    {
      using (ListPool<NeuronElement> list = new ListPool<NeuronElement>())
      {
        foreach (NeuronElement connectedNeuron in pPresynapticNeuron.connected_neurons)
        {
          if (!Object.op_Equality((Object) pIgnoreNeuron, (Object) connectedNeuron))
            list.Add(connectedNeuron);
        }
        if (list.Count == 0)
          return;
        random = list.GetRandom<NeuronElement>();
      }
    }
    if (Object.op_Equality((Object) random, (Object) null))
      return;
    this.fireImpulse(pPresynapticNeuron, random, pWave);
  }

  internal void fireImpulse(
    NeuronElement pPresynapticNeuron,
    NeuronElement pPostsynapticNeuron,
    int pWave)
  {
    NerveImpulseElement next = this._pool_impulses.getNext();
    next.energize(pPresynapticNeuron, pPostsynapticNeuron, pWave);
    pPresynapticNeuron.spawnImpulseFromHere();
    this._active_impulses.Add(next);
  }

  private void updateImpulses()
  {
    for (int index = this._active_impulses.Count - 1; index >= 0; --index)
    {
      NerveImpulseElement activeImpulse = this._active_impulses[index];
      ImpulseReachResult impulseReachResult = activeImpulse.moveTowardsNextNeuron();
      NeuronElement postsynapticNeuron = activeImpulse.postsynaptic_neuron;
      NeuronElement presynapticNeuron = activeImpulse.presynaptic_neuron;
      switch (impulseReachResult)
      {
        case ImpulseReachResult.Done:
          postsynapticNeuron?.receiveImpulse();
          this._active_impulses.RemoveAt(index);
          this._pool_impulses.release(activeImpulse);
          break;
        case ImpulseReachResult.Split:
          postsynapticNeuron?.receiveImpulse();
          this.fireImpulseFrom(postsynapticNeuron, activeImpulse.wave, presynapticNeuron);
          break;
      }
    }
  }

  private void Update()
  {
    if (!this._is_dragging)
    {
      this.smoothOffsets();
      if (InputHelpers.mouseSupported)
      {
        this._active_neuron = this.getHighlightedNeuron();
        this.highlightNeuron(this._active_neuron);
      }
      if ((InputHelpers.mouseSupported || Object.op_Equality((Object) this._latest_touched_neuron, (Object) null) ? 1 : (!Tooltip.isShowingFor((object) this._latest_touched_neuron) ? 1 : 0)) != 0)
        this.updateNeuronsVisual();
    }
    this.updateNeuronImpulseAutoSpawn();
    this.updateAxonPositions();
    this.updateImpulseSpawn();
    this.updateImpulses();
  }

  private NeuronElement getHighlightedNeuron()
  {
    if (this._is_dragging)
      return (NeuronElement) null;
    if ((double) this._offset_x > 1.0499999523162842 || (double) this._offset_x < -1.0499999523162842)
      return (NeuronElement) null;
    return (double) this._offset_y > 1.0499999523162842 || (double) this._offset_y < -1.0499999523162842 ? (NeuronElement) null : this.getClosestNeuronToCursor();
  }

  internal void startNewWhat() => this._text_phrases.startNewWhat();

  private void updateImpulseSpawn()
  {
    foreach (NeuronElement neuron in this._neurons)
    {
      if (neuron.hasDecisionSet() && neuron.readyToSpawnImpulse())
        this.fireImpulseFrom(neuron, 1);
    }
  }

  private void initStartPositions()
  {
    for (int pNeuronIndex = 0; pNeuronIndex < this._decision_counter; ++pNeuronIndex)
    {
      DecisionAsset decisionAsset = this._decision_assets[pNeuronIndex];
      NeuronElement next = this._pool_neurons.getNext();
      next.setupDecisionAndActor(decisionAsset, this.actor);
      Vector3 positionOnSphere = this.getPositionOnSphere(pNeuronIndex, this._decision_counter);
      ((Component) next).transform.localPosition = positionOnSphere;
      this._neurons.Add(next);
    }
    this.updateNeuronsVisual();
  }

  private void loadLastDecisionForCenter()
  {
    NeuronElement pNeuron1 = (NeuronElement) null;
    string decisionForMindOverview = this.actor.getLastDecisionForMindOverview();
    if (!string.IsNullOrEmpty(decisionForMindOverview))
    {
      foreach (NeuronElement neuron in this._neurons)
      {
        if (neuron.decision.id == decisionForMindOverview)
        {
          pNeuron1 = neuron;
          break;
        }
      }
    }
    this._last_activated_neuron = this._pool_neurons.getNext();
    ((Component) this._last_activated_neuron).transform.localPosition = Vector3.zero;
    this._last_activated_neuron.image.sprite = SpriteTextureLoader.getSprite("ui/icons/iconBrain");
    this._last_activated_neuron.bonus_scale = 1.5f;
    this._last_activated_neuron.setCenter(true);
    this._last_activated_neuron.actor = this.actor;
    this._neurons.Add(this._last_activated_neuron);
    if (!Object.op_Inequality((Object) pNeuron1, (Object) null))
      return;
    this.makeAxon(pNeuron1, this._last_activated_neuron).axon_center = true;
  }

  private Vector3 getPositionOnSphere(int pNeuronIndex, int pTotalNeurons)
  {
    float num1 = Mathf.Acos((float) (1.0 - (double) (2 * (pNeuronIndex + 1)) / (double) pTotalNeurons));
    float num2 = (float) (3.1415927410125732 * (1.0 + (double) Mathf.Sqrt(5f))) * (float) pNeuronIndex;
    double num3 = 70.0 * (double) Mathf.Cos(num2) * (double) Mathf.Sin(num1);
    float num4 = 70f * Mathf.Sin(num2) * Mathf.Sin(num1);
    float num5 = 70f * Mathf.Cos(num1);
    double num6 = (double) num4;
    double num7 = (double) num5;
    return new Vector3((float) num3, (float) num6, (float) num7);
  }

  private void clearMind()
  {
    foreach (NeuronElement neuron in this._neurons)
      neuron.clear();
    this._active_impulses.Clear();
    this._pool_axons.clear();
    this._pool_neurons.clear();
    this._pool_impulses.clear();
    this._neurons.Clear();
    foreach (AxonElement axonElement in (IEnumerable<AxonElement>) this._pool_axons.getListTotal())
      axonElement.clear();
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    ShortcutExtensions.DOKill((Component) this._mind_main.transform, false);
    this._mind_main.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(this._mind_main.transform, 1f, 0.6f), (Ease) 27);
    this.checkActorDecisions();
    this.clearMind();
    this.initStartPositions();
    this.loadLastDecisionForCenter();
    this.prepareAxons();
    this._is_dragging = false;
  }

  internal bool isDragging() => this._is_dragging;

  internal void clearHighlight()
  {
    if (Object.op_Equality((Object) this._active_neuron, (Object) null))
      return;
    this._active_neuron.highlighted = false;
    this._active_neuron = (NeuronElement) null;
  }

  public static void debugTool(DebugTool pTool) => NeuronsOverview.instance?.debug(pTool);

  public void debug(DebugTool pTool)
  {
    pTool.setText("offset_target_x:", (object) NeuronsOverview.getFloat(this._offset_target_x));
    pTool.setText("offset_target_y:", (object) NeuronsOverview.getFloat(this._offset_target_y));
    pTool.setSeparator();
    pTool.setText("offset_x:", (object) NeuronsOverview.getFloat(this._offset_x));
    pTool.setText("offset_y:", (object) NeuronsOverview.getFloat(this._offset_y));
    pTool.setSeparator();
    Quaternion quaternion = Quaternion.Euler(this._offset_x, this._offset_y, 0.0f);
    pTool.setText("combined_rotation.x:", (object) NeuronsOverview.getFloat(quaternion.x));
    pTool.setText("combined_rotation.y:", (object) NeuronsOverview.getFloat(quaternion.y));
    pTool.setText("combined_rotation.z:", (object) NeuronsOverview.getFloat(quaternion.z));
    pTool.setText("combined_rotation.w:", (object) NeuronsOverview.getFloat(quaternion.w));
    pTool.setSeparator();
    pTool.setText("is_dragging:", (object) this._is_dragging);
    pTool.setText("last_mouse_delta:", (object) this._last_mouse_delta);
    pTool.setSeparator();
    pTool.setText("decisions:", (object) this._decision_counter);
    pTool.setSeparator();
    pTool.setText("neuron selected:", (object) this._active_neuron);
  }

  public static string getFloat(float pFloat)
  {
    if ((double) pFloat < 1.0 / 1000.0 && (double) pFloat > -1.0 / 1000.0)
      return pFloat.ToString("F6", (IFormatProvider) CultureInfo.InvariantCulture);
    return (double) pFloat > 0.0 ? $"<color=#75D53A>{pFloat.ToString("F6", (IFormatProvider) CultureInfo.InvariantCulture)}</color>" : $"<color=#DB2920>{pFloat.ToString("F6", (IFormatProvider) CultureInfo.InvariantCulture)}</color>";
  }

  public void setLatestTouched(NeuronElement pNeuron) => this._latest_touched_neuron = pNeuron;

  public void switchAllNeurons()
  {
    this._all_state = !this.isAnyEnabled() || !this.isAllEnabled() && !this._all_state;
    foreach (NeuronElement neuron in this._neurons)
    {
      if (neuron.hasDecisionSet())
        this.actor.setDecisionState(neuron.decision.decision_index, this._all_state);
    }
    this.fireImpulsesEverywhere();
  }

  public bool getAllState() => this._all_state;

  private bool isAnyEnabled()
  {
    foreach (NeuronElement neuron in this._neurons)
    {
      if (neuron.hasDecisionSet() && this.actor.isDecisionEnabled(neuron.decision.decision_index))
        return true;
    }
    return false;
  }

  private bool isAllEnabled()
  {
    foreach (NeuronElement neuron in this._neurons)
    {
      if (neuron.hasDecisionSet() && !this.actor.isDecisionEnabled(neuron.decision.decision_index))
        return false;
    }
    return true;
  }
}
