// Decompiled with JetBrains decompiler
// Type: LibraryMaterials
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LibraryMaterials : MonoBehaviour
{
  public static LibraryMaterials instance;
  public const string mat_id_world_object = "mat_world_object";
  public const string mat_id_world_object_lit_always = "mat_world_object_lit";
  public Dictionary<string, Material> dict = new Dictionary<string, Material>();
  public Material mat_damaged;
  public Material mat_highlighted;
  public Material mat_world_object;
  public Material mat_world_object_lit;
  public Material mat_buildings;
  public Material mat_socialize;
  public Material mat_minis;
  public Material mat_tree;
  public Material mat_tree_celestial;
  public Material mat_jelly;
  public Material mat_buildings_light;
  public Material mat_lava_glow;
  public Material mat_overlapped_shadows;
  private float _shadow_alpha_target = 0.403921574f;
  private float _shadow_alpha = 0.403921574f;
  private Color _shadows_color;
  private List<Material> _night_affected_colors = new List<Material>();
  private float _time;

  private void Awake()
  {
    LibraryMaterials.instance = this;
    this.mat_damaged = this.loadMaterial("materials/damaged", true);
    this.mat_highlighted = this.loadMaterial("materials/highlighted", true);
    this.mat_buildings = this.loadMaterial("materials/building", true);
    this.mat_tree = this.loadMaterial("materials/tree", true);
    this.mat_socialize = this.loadMaterial("materials/socialize");
    this.mat_minis = this.loadMaterial("materials/minis");
    this.mat_tree_celestial = this.loadMaterial("materials/tree_celestial", true);
    this.mat_jelly = this.loadMaterial("materials/jelly", true);
    this.mat_overlapped_shadows = this.loadMaterial("materials/OverlappedShadows", true);
    this.mat_buildings_light = this.loadMaterial("materials/MatBuildingsLight");
    this.mat_world_object = this.loadMaterial("materials/mat_world_object");
    this.mat_world_object_lit = this.loadMaterial("materials/mat_world_object_lit");
    this.mat_lava_glow = this.loadMaterial("materials/lava_glow", true);
    this._night_affected_colors.Add(this.mat_buildings);
    this._night_affected_colors.Add(this.mat_tree);
    this._night_affected_colors.Add(this.mat_jelly);
    this._night_affected_colors.Add(this.mat_world_object);
    this._shadows_color = this.mat_overlapped_shadows.GetColor("_Color");
    AssetManager.status.linkMaterials();
    Shader.SetGlobalFloat("GlobalTime", 1f);
  }

  private Material loadMaterial(string pPath, bool pCopy = false)
  {
    Material material = Resources.Load<Material>(pPath);
    if (pCopy)
      material = Object.Instantiate<Material>(material);
    this.dict.Add(((Object) material).name.Replace("(Clone)", ""), material);
    return material;
  }

  internal void updateMat()
  {
    if (!World.world.isPaused())
      this._time += World.world.elapsed;
    this.updateNight();
    Shader.SetGlobalFloat("GlobalTime", this._time);
  }

  private void updateNight()
  {
    float nightMod = World.world.era_manager.getNightMod();
    Color color1 = Toolbox.blendColor(Color32.op_Implicit(Toolbox.color_night), Toolbox.color_white, nightMod);
    foreach (Material nightAffectedColor in this._night_affected_colors)
      nightAffectedColor.color = color1;
    Color color2 = Toolbox.blendColor(Color32.op_Implicit(Toolbox.color_night), Color32.op_Implicit(Toolbox.color_ocean), nightMod);
    if ((double) nightMod > 0.0)
    {
      color2.r -= 0.007843138f;
      color2.b -= 0.0196078438f;
    }
    World.world.camera.backgroundColor = color2;
  }

  public void updateZoomoutValue(float pValue)
  {
    float num = (float) (4.0 - (double) pValue * 3.0);
    if (!DebugConfig.isOn(DebugOption.ScaleEffectEnabled))
      num = 1f;
    this._shadow_alpha = num * this._shadow_alpha_target;
    this._shadows_color.a = this._shadow_alpha;
    this.mat_overlapped_shadows.SetColor("_Color", this._shadows_color);
  }
}
