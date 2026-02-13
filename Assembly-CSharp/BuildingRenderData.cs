// Decompiled with JetBrains decompiler
// Type: BuildingRenderData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BuildingRenderData
{
  public Vector3[] positions;
  public Vector3[] scales;
  public Vector3[] rotations;
  public Sprite[] colored_sprites;
  public Sprite[] main_sprites;
  public Material[] materials;
  public bool[] flip_x_states;
  public Color[] colors;
  public bool[] shadows;
  public Sprite[] shadow_sprites;

  public BuildingRenderData(int pCapacity) => this.checkSize(pCapacity);

  public void checkSize(int pTargetSize)
  {
    if (this.positions != null && this.positions.Length >= pTargetSize)
      return;
    this.positions = Toolbox.checkArraySize<Vector3>(this.positions, pTargetSize);
    this.scales = Toolbox.checkArraySize<Vector3>(this.scales, pTargetSize);
    this.rotations = Toolbox.checkArraySize<Vector3>(this.rotations, pTargetSize);
    this.colored_sprites = Toolbox.checkArraySize<Sprite>(this.colored_sprites, pTargetSize);
    this.main_sprites = Toolbox.checkArraySize<Sprite>(this.main_sprites, pTargetSize);
    this.materials = Toolbox.checkArraySize<Material>(this.materials, pTargetSize);
    this.flip_x_states = Toolbox.checkArraySize<bool>(this.flip_x_states, pTargetSize);
    this.colors = Toolbox.checkArraySize<Color>(this.colors, pTargetSize);
    this.shadows = Toolbox.checkArraySize<bool>(this.shadows, pTargetSize);
    this.shadow_sprites = Toolbox.checkArraySize<Sprite>(this.shadow_sprites, pTargetSize);
  }
}
