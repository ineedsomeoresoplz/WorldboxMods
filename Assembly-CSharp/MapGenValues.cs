// Decompiled with JetBrains decompiler
// Type: MapGenValues
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class MapGenValues
{
  public int perlin_scale_stage_1 = 5;
  public int perlin_scale_stage_2 = 5;
  public int perlin_scale_stage_3 = 5;
  public bool main_perlin_noise_stage;
  public bool perlin_noise_stage_2;
  public bool perlin_noise_stage_3;
  public bool square_edges;
  public bool gradient_round_edges;
  public bool add_center_gradient_land;
  public bool center_gradient_mountains;
  public bool add_center_lake;
  public bool ring_effect;
  public bool add_vegetation = true;
  public bool add_resources = true;
  public bool add_mountain_edges;
  public bool random_biomes = true;
  public int random_shapes_amount;
  public int cubicle_size;
  public bool remove_mountains;
  public bool forbidden_knowledge_start;
  public bool low_ground;
  public bool high_ground;
}
