// Decompiled with JetBrains decompiler
// Type: QuantumSpriteCacheData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class QuantumSpriteCacheData
{
  public Sprite[] sprites;
  public Vector3[] scales;
  public Vector3[] shadow_scales;
  public Vector3[] positions;
  public Material[] materials;
  public Vector3[] rotations;
  public bool[] flip_x_states;
  public Color[] colors;
  public int[] indexes;
  public int[] indexes_2;

  public QuantumSpriteCacheData(int pCapacity) => this.checkSize(pCapacity);

  public void checkSize(int pTargetSize)
  {
    if (this.positions != null && this.positions.Length >= pTargetSize)
      return;
    this.positions = Toolbox.checkArraySize<Vector3>(this.positions, pTargetSize);
    this.scales = Toolbox.checkArraySize<Vector3>(this.scales, pTargetSize);
    this.shadow_scales = Toolbox.checkArraySize<Vector3>(this.shadow_scales, pTargetSize);
    this.rotations = Toolbox.checkArraySize<Vector3>(this.rotations, pTargetSize);
    this.sprites = Toolbox.checkArraySize<Sprite>(this.sprites, pTargetSize);
    this.materials = Toolbox.checkArraySize<Material>(this.materials, pTargetSize);
    this.flip_x_states = Toolbox.checkArraySize<bool>(this.flip_x_states, pTargetSize);
    this.colors = Toolbox.checkArraySize<Color>(this.colors, pTargetSize);
    this.indexes = Toolbox.checkArraySize<int>(this.indexes, pTargetSize);
    this.indexes_2 = Toolbox.checkArraySize<int>(this.indexes_2, pTargetSize);
  }
}
