// Decompiled with JetBrains decompiler
// Type: NerveImpulseElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class NerveImpulseElement : MonoBehaviour
{
  public Image image;
  private const float SPEED_MIN = 1f;
  private const float SPEED_MAX = 3f;
  private const float SCALE_MIN = 0.6f;
  private const float SCALE_MAX = 1f;
  private float _speed_current;
  private float _move_timer;
  public NeuronElement presynaptic_neuron;
  public NeuronElement postsynaptic_neuron;
  private Color _color_back = Toolbox.makeColor("#26A8A8", 0.5f);
  private Color _color_front = Toolbox.makeColor("#3AFFFF", 0.7f);
  public int wave;

  public void energize(
    NeuronElement pPresynapticNeuron,
    NeuronElement pPostsynapticNeuron,
    int pWave)
  {
    ((Component) this).transform.localPosition = ((Component) pPresynapticNeuron).transform.localPosition;
    this.presynaptic_neuron = pPresynapticNeuron;
    this.postsynaptic_neuron = pPostsynapticNeuron;
    this._move_timer = 0.0f;
    this._speed_current = Randy.randomFloat(1f, 3f);
    this.wave = pWave;
  }

  public ImpulseReachResult moveTowardsNextNeuron()
  {
    if (Object.op_Equality((Object) this.postsynaptic_neuron, (Object) null))
      return ImpulseReachResult.Done;
    this._move_timer += this._speed_current * Time.deltaTime;
    this._move_timer = Mathf.Clamp01(this._move_timer);
    ((Component) this).transform.localPosition = Vector3.Lerp(((Component) this.presynaptic_neuron).transform.localPosition, ((Component) this.postsynaptic_neuron).transform.localPosition, this._move_timer);
    this.updateImpulseColor();
    if ((double) this._move_timer < 1.0)
      return ImpulseReachResult.Move;
    this.presynaptic_neuron = this.postsynaptic_neuron;
    this.postsynaptic_neuron = this.GetNextTargetNeuron();
    this._move_timer = 0.0f;
    --this.wave;
    return this.wave > 0 ? ImpulseReachResult.Split : ImpulseReachResult.Done;
  }

  private NeuronElement GetNextTargetNeuron()
  {
    return this.presynaptic_neuron.connected_neurons.Count == 0 ? (NeuronElement) null : this.presynaptic_neuron.connected_neurons.GetRandom<NeuronElement>();
  }

  private void updateImpulseColor()
  {
    float num1 = Mathf.Lerp(this.presynaptic_neuron.render_depth, this.postsynaptic_neuron.render_depth, this._move_timer);
    Color color = Color.Lerp(this._color_back, this._color_front, num1);
    if (Color.op_Inequality(((Graphic) this.image).color, color))
      ((Graphic) this.image).color = color;
    float num2 = Mathf.Lerp(0.6f, 1f, num1);
    if ((double) ((Component) this).transform.localScale.x == (double) num2)
      return;
    ((Component) this).transform.localScale = new Vector3(num2, num2, num2);
  }
}
