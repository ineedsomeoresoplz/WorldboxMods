// Decompiled with JetBrains decompiler
// Type: AxonElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class AxonElement : MonoBehaviour
{
  public Image image;
  internal NeuronElement neuron_1;
  internal NeuronElement neuron_2;
  internal float mod_light = 1f;
  public bool axon_center;

  public void update()
  {
    this.mod_light -= Time.deltaTime * 2f;
    this.mod_light = Mathf.Max(0.0f, this.mod_light);
  }

  public void clear() => this.axon_center = false;
}
