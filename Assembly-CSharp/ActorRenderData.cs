// Decompiled with JetBrains decompiler
// Type: ActorRenderData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ActorRenderData
{
  public Vector3[] positions;
  public Vector3[] scales;
  public Vector3[] rotations;
  public Color[] colors;
  public bool[] has_normal_render;
  public Sprite[] main_sprites;
  public Sprite[] main_sprite_colored;
  public Material[] materials;
  public bool[] flip_x_states;
  public bool[] shadows;
  public Vector3[] shadow_position;
  public Vector3[] shadow_scales;
  public Sprite[] shadow_sprites;
  public bool[] has_item;
  public Vector3[] item_scale;
  public Vector3[] item_pos;
  public Sprite[] item_sprites;

  public ActorRenderData(int pCapacity) => this.checkSize(pCapacity);

  public void checkSize(int pTargetSize)
  {
    if (this.positions != null && this.positions.Length >= pTargetSize)
      return;
    this.positions = Toolbox.checkArraySize<Vector3>(this.positions, pTargetSize);
    this.scales = Toolbox.checkArraySize<Vector3>(this.scales, pTargetSize);
    this.rotations = Toolbox.checkArraySize<Vector3>(this.rotations, pTargetSize);
    this.colors = Toolbox.checkArraySize<Color>(this.colors, pTargetSize);
    this.has_normal_render = Toolbox.checkArraySize<bool>(this.has_normal_render, pTargetSize);
    this.main_sprites = Toolbox.checkArraySize<Sprite>(this.main_sprites, pTargetSize);
    this.main_sprite_colored = Toolbox.checkArraySize<Sprite>(this.main_sprite_colored, pTargetSize);
    this.materials = Toolbox.checkArraySize<Material>(this.materials, pTargetSize);
    this.flip_x_states = Toolbox.checkArraySize<bool>(this.flip_x_states, pTargetSize);
    this.shadows = Toolbox.checkArraySize<bool>(this.shadows, pTargetSize);
    this.shadow_sprites = Toolbox.checkArraySize<Sprite>(this.shadow_sprites, pTargetSize);
    this.shadow_position = Toolbox.checkArraySize<Vector3>(this.shadow_position, pTargetSize);
    this.shadow_scales = Toolbox.checkArraySize<Vector3>(this.shadow_scales, pTargetSize);
    this.has_item = Toolbox.checkArraySize<bool>(this.has_item, pTargetSize);
    this.item_scale = Toolbox.checkArraySize<Vector3>(this.item_scale, pTargetSize);
    this.item_pos = Toolbox.checkArraySize<Vector3>(this.item_pos, pTargetSize);
    this.item_sprites = Toolbox.checkArraySize<Sprite>(this.item_sprites, pTargetSize);
  }
}
