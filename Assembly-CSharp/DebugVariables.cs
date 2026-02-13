// Decompiled with JetBrains decompiler
// Type: DebugVariables
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DebugVariables : MonoBehaviour
{
  public static DebugVariables instance;
  [Range(1f, 1000f)]
  public float multiplier = 1f;
  [Range(1f, 1E+07f)]
  public float bonus = 1f;
  public float time;
  [Range(0.0f, 1000f)]
  public float gravity = 9.8f;
  [Range(0.0f, 10f)]
  public float unit_force_multiplier = 1f;
  [Range(0.0f, 10f)]
  public float test_mass = 2f;
  public bool layout_city_test;
  public bool layout_lines_horizontal;
  public bool layout_lines_vertical;
  public bool layout_cross;
  public bool layout_diagonal;
  public bool layout_lattice_small;
  public bool layout_lattice_medium;
  public bool layout_lattice_big;
  public bool layout_clusters_small;
  public bool layout_clusters_medium;
  public bool layout_clusters_big;
  public bool layout_ring;
  public bool layout_diamond;
  public bool layout_diamond_cluster;
  public bool layout_honeycomb;
  public bool layout_brick_vertical;
  public bool layout_brick_horizontal;
  public bool layout_madman_labyrinth;
  public bool layout_map_ring;
}
