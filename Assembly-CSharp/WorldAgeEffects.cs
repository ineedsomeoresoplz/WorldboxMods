// Decompiled with JetBrains decompiler
// Type: WorldAgeEffects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WorldAgeEffects : MonoBehaviour
{
  internal Dictionary<string, SpriteRenderer> dict_effects = new Dictionary<string, SpriteRenderer>();
  public static WorldAgeEffects instance;
  public bool override_night;
  [Range(0.0f, 1f)]
  public float night_value_top;
  [Range(0.0f, 1f)]
  public float night_value_mat;

  public void Awake()
  {
    WorldAgeEffects.instance = this;
    for (int index = 0; index < ((Component) this).transform.childCount; ++index)
    {
      SpriteRenderer component = ((Component) ((Component) this).transform.GetChild(index)).GetComponent<SpriteRenderer>();
      Color color = component.color;
      color.a = 0.0f;
      component.color = color;
      this.dict_effects.Add(((Object) component).name, component);
    }
  }

  public void update(float pElapsed)
  {
    this.fitTheCamera();
    this.updateEffects(pElapsed);
  }

  private void updateEffects(float pElapsed)
  {
    this.updateLayer(World.world_era.overlay_chaos, "chaos", pElapsed);
    this.updateLayer(World.world_era.overlay_moon, "moon", pElapsed);
    this.updateLayer(World.world_era.overlay_magic, "magic", pElapsed);
    this.updateLayer(World.world_era.overlay_sun, "sun", pElapsed);
    this.updateLayer(World.world_era.overlay_rain_darkness, "rain_darkness", pElapsed);
    this.updateLayer(World.world_era.overlay_winter, "winter", pElapsed);
    this.updateLayer(World.world_era.overlay_ash, "ash", pElapsed);
    this.updateLayer(World.world_era.overlay_night, "night", pElapsed);
    this.updateLayer(World.world_era.overlay_rain, "rain", pElapsed);
  }

  private void updateLayer(bool pEnabled, string pID, float pElapsed)
  {
    SpriteRenderer spriteRenderer = (SpriteRenderer) null;
    if (!this.dict_effects.TryGetValue(pID, out spriteRenderer))
    {
      Debug.LogError((object) ("NO ERA EFFECT " + pID));
    }
    else
    {
      Color color = spriteRenderer.color;
      if (this.override_night)
      {
        color.a = this.night_value_top;
        spriteRenderer.color = color;
      }
      else
      {
        if (pEnabled)
        {
          float intValue = (float) PlayerConfig.getIntValue("age_overlay_effect");
          float num = World.world_era.era_effect_overlay_alpha * (intValue / 100f);
          ((Renderer) spriteRenderer).enabled = (double) intValue > 0.0;
          if ((double) color.a < (double) num)
          {
            color.a += pElapsed * 0.2f;
            if ((double) color.a > (double) num)
              color.a = num;
            spriteRenderer.color = color;
          }
          if ((double) color.a > (double) num)
          {
            color.a -= pElapsed * 0.7f;
            if ((double) color.a < (double) num)
              color.a = num;
            spriteRenderer.color = color;
          }
        }
        if (pEnabled || !((Renderer) spriteRenderer).enabled || (double) color.a <= 0.0)
          return;
        color.a -= pElapsed * 0.2f;
        if ((double) color.a <= 0.0)
          ((Renderer) spriteRenderer).enabled = false;
        spriteRenderer.color = color;
      }
    }
  }

  private void fitTheCamera()
  {
    ((Component) this).transform.localScale = new Vector3(1f, 1f, 1f);
    float num = World.world.camera.orthographicSize * 2f;
    ((Component) this).transform.localScale = new Vector3(num / (float) Screen.height * (float) Screen.width, num);
  }
}
