// Decompiled with JetBrains decompiler
// Type: CloudAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class CloudAsset : Asset
{
  public bool normal_cloud;
  [NonSerialized]
  public Color color;
  public string color_hex = "#FFFFFF";
  public float max_alpha = 0.8f;
  public CloudAction cloud_action_1;
  public CloudAction cloud_action_2;
  public float interval_action_1 = 0.05f;
  public float interval_action_2 = 0.05f;
  public float speed_min = 1f;
  public float speed_max = 6f;
  public string drop_id = string.Empty;
  public string[] path_sprites;
  [NonSerialized]
  internal Sprite[] cached_sprites;
  public bool draw_light_area;
  public float draw_light_area_offset_x;
  public float draw_light_area_offset_y;
  public float draw_light_size = 4f;
  public bool considered_disaster;
}
