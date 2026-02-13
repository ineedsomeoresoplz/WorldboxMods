// Decompiled with JetBrains decompiler
// Type: QualityChanger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class QualityChanger : MonoBehaviour
{
  private const float TIMER_SPEED_MULTIPLIER = 3.5f;
  private const float BUILDING_HIDE_VALUE = 0.3f;
  public const float BUILDING_SHADER_VALUE_BOUND = 4f;
  public const float BUILDING_SHADER_VALUE_BOUND_2 = 3f;
  private const float BOUND_RATE_SHADOWS_UNITS = 0.7f;
  private const float BOUND_RATE_SHADOWS_BUILDINGS = 0.85f;
  private const float BOUND_STRATOSPHERE_ORTHOGRAPHIC = 400f;
  private bool _low_resolution;
  private float _transition_animation_factor = 1f;
  private float _tween_buildings;
  private Color _color_alpha = new Color(1f, 1f, 1f);
  private bool _render_buildings;
  private float _current_zoom_orthographic;
  private float _main_zoom;

  internal void reset()
  {
    this.setLowRes(true);
    this._transition_animation_factor = 1f;
    this._main_zoom = !Config.isMobile ? 240f : 160f;
    World.world.world_layer.setRendererEnabled(true);
  }

  internal void update()
  {
    LibraryMaterials.instance.updateMat();
    this._render_buildings = (!this.isLowRes() || (double) this._tween_buildings >= 0.30000001192092896) && !this.isFullLowRes() && (double) this._tween_buildings != 0.0;
    if ((double) this._color_alpha.a != (double) this._transition_animation_factor || (double) this._transition_animation_factor != 0.0 && (double) this._transition_animation_factor != 1.0)
    {
      this._tween_buildings = 1f - iTween.easeInCirc(0.0f, 1f, this._transition_animation_factor);
      LibraryMaterials.instance.updateZoomoutValue(this._tween_buildings);
    }
    if (this.isLowRes() && (double) this._transition_animation_factor < 1.0)
    {
      this._transition_animation_factor += Time.deltaTime * 3.5f;
      if ((double) this._transition_animation_factor > 1.0)
      {
        this._transition_animation_factor = 1f;
        this._tween_buildings = 0.0f;
      }
    }
    else if (!this.isLowRes() && (double) this._transition_animation_factor > 0.0)
    {
      this._transition_animation_factor -= Time.deltaTime * 3.5f;
      if ((double) this._transition_animation_factor < 0.0)
      {
        this._transition_animation_factor = 0.0f;
        this._tween_buildings = 1f;
      }
    }
    if (!PlayerConfig.optionBoolEnabled("minimap_transition_animation"))
    {
      if (this.isLowRes())
      {
        this._transition_animation_factor = 1f;
        this._tween_buildings = 0.0f;
      }
      else
      {
        this._transition_animation_factor = 0.0f;
        this._tween_buildings = 1f;
      }
      this.setZoomOrthographic(MoveCamera.instance.main_camera.orthographicSize);
    }
    this._color_alpha.a = this._transition_animation_factor;
    Color colorAlpha = this._color_alpha;
    if (this.isLowRes())
      colorAlpha.a = Mathf.Max(0.9f, colorAlpha.a);
    World.world.world_layer.sprRnd.color = colorAlpha;
    World.world.world_layer_edges.sprRnd.color = this._color_alpha;
    World.world.unit_layer.sprRnd.color = this._color_alpha;
    World.world.tilemap.checkEnableForWaterRunups(this.isLowRes());
    if (this.isLowRes())
    {
      World.world.world_layer.setRendererEnabled(true);
      if (((Component) World.world.tilemap).gameObject.activeSelf)
      {
        Color color = World.world._world_layer_switch_effect.color;
        color.a = 0.1f;
        World.world._world_layer_switch_effect.color = color;
      }
      World.world.tilemap.enableTiles(false);
    }
    else
    {
      World.world.tilemap.enableTiles(true);
      if ((double) this._transition_animation_factor != 0.0)
        return;
      World.world.world_layer.setRendererEnabled(false);
    }
  }

  public bool isFullLowRes()
  {
    return this.isLowRes() && (double) this._transition_animation_factor == 1.0;
  }

  public bool shouldRenderUnitShadows()
  {
    return TrailerMonolith.enable_trailer_stuff || Config.shadows_active && !this.isLowRes() && this.isZoomLevelWithinUnitShadows();
  }

  public bool isZoomLevelWithinUnitShadows()
  {
    return (double) this._current_zoom_orthographic < (double) this.getZoomBoundUnitShadows();
  }

  public float getZoomRateBoundLow() => this.getCameraMultiplier(this._main_zoom);

  public float getZoomRateShadows() => this.getCameraMultiplier(this._main_zoom * 0.85f);

  public bool shouldRenderBuildingShadows()
  {
    if (TrailerMonolith.enable_trailer_stuff)
      return true;
    return Config.shadows_active && (double) World.world.camera.orthographicSize <= (double) this.getZoomRateShadows() && this._render_buildings;
  }

  public float getZoomBoundUnitShadows() => this.getCameraMultiplier(this._main_zoom * 0.7f);

  private float getCameraMultiplier(float pVal)
  {
    if (!DebugConfig.isOn(DebugOption.UseCameraAspect))
      return pVal;
    int pixelWidth = World.world.camera.pixelWidth;
    int pixelHeight = World.world.camera.pixelHeight;
    float aspect = World.world.camera.aspect;
    float num = (float) pixelHeight;
    if (World.world.camera.pixelWidth > pixelHeight)
      num = (float) pixelWidth;
    if ((double) aspect > 2.0)
      num /= aspect * 0.5f;
    pVal *= (float) ((double) pixelHeight / (double) num * 0.5);
    return pVal;
  }

  internal void setZoomOrthographic(float pZoom)
  {
    this._current_zoom_orthographic = pZoom;
    bool pValue = (double) this._current_zoom_orthographic > (double) this.getZoomRateBoundLow();
    if (pValue == this.isLowRes())
      return;
    this.setLowRes(pValue);
    if (this.isLowRes())
      MusicBox.playSound("event:/SFX/MAP/BitScaleWhooshIn");
    else
      MusicBox.playSound("event:/SFX/MAP/BitScaleWhooshOut");
    World.world.zone_calculator.clearCurrentDrawnZones();
    World.world.resetRedrawTimer();
  }

  public float getZoomRatioLow()
  {
    return Mathf.Clamp((this._current_zoom_orthographic - 10f) / (this.getZoomRateBoundLow() - 10f), 0.0f, 1f);
  }

  public float getZoomRatioHigh()
  {
    return (double) this.getZoomRatioLow() < 1.0 ? 0.0f : Mathf.Clamp((this._current_zoom_orthographic - this.getZoomRateBoundLow()) / (400f - this.getZoomRateBoundLow()), 0.0f, 1f);
  }

  public float getZoomRatioFull()
  {
    return Mathf.Clamp((this._current_zoom_orthographic - 10f) / 400f, 0.0f, 1f);
  }

  private void setLowRes(bool pValue) => this._low_resolution = pValue;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool shouldRenderBuildings() => this._render_buildings;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isLowRes() => this._low_resolution;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public float getTweenBuildingsValue() => this._tween_buildings;

  public void debug(DebugTool pTool)
  {
    pTool.setText("Zoom_Low:", (object) this.getZoomRatioLow());
    pTool.setText("Zoom_High:", (object) this.getZoomRatioHigh());
    pTool.setText("Zoom_Full:", (object) this.getZoomRatioFull());
    pTool.setSeparator();
    pTool.setText("lowRes", (object) this.isLowRes());
    pTool.setText("_timer_animation", (object) this._transition_animation_factor);
    pTool.setText("getTweenBuildinsValue", (object) this.getTweenBuildingsValue());
    pTool.setText("isBuildingRendered", (object) this.shouldRenderBuildings());
    pTool.setText("_current_zoom", (object) this._current_zoom_orthographic);
    pTool.setText("_main_zoom", (object) this._main_zoom);
    pTool.setText("camera_zoom_max", (object) World.world.move_camera.orthographic_size_max);
    pTool.setText("camera_ortho", (object) World.world.move_camera.main_camera.orthographicSize);
    pTool.setText("camera_zoom_min", (object) 10f);
    pTool.setText("BOUND_STRATOSPHERE_ORTHOGRAPHIC", (object) 400f);
    pTool.setText("isFullLowRes()", (object) this.isFullLowRes());
    pTool.setText("renderShadowsUnits()", (object) this.shouldRenderUnitShadows());
  }
}
