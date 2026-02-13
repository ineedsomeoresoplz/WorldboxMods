// Decompiled with JetBrains decompiler
// Type: WorldAgesParticles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class WorldAgesParticles : MonoBehaviour
{
  public static bool effects_enabled = true;
  private ParticleSystem _system_rain;
  private Material _mat_rain;
  private ParticleSystem _system_snow;
  private Material _mat_snow;
  private ParticleSystem _system_magic;
  private Material _mat_magic;
  private ParticleSystem _system_ash;
  private Material _mat_ash;
  private ParticleSystem _system_sun_blobs;
  private Material _mat_sun_blobs;
  private ParticleSystem _system_sun_rays;
  private Material _mat_sun_ray;
  private Camera _camera;

  private void Awake()
  {
    this.setSystem("Rain", out this._system_rain, out this._mat_rain);
    this.setSystem("Snow", out this._system_snow, out this._mat_snow);
    this.setSystem("Magic", out this._system_magic, out this._mat_magic);
    this.setSystem("Ash", out this._system_ash, out this._mat_ash);
    this.setSystem("Sun Blobs", out this._system_sun_blobs, out this._mat_sun_blobs);
    this.setSystem("Sun Rays", out this._system_sun_rays, out this._mat_sun_ray);
  }

  private void setSystem(string pID, out ParticleSystem pSystem, out Material pMat)
  {
    pSystem = ((Component) ((Component) this).transform.Find(pID)).GetComponent<ParticleSystem>();
    pMat = ((Component) pSystem).GetComponent<Renderer>().material;
    pSystem.Stop(false, (ParticleSystemStopBehavior) 0);
    Color color = pMat.color;
    color.a = 0.0f;
    pMat.color = color;
  }

  private void Update()
  {
    if (Object.op_Equality((Object) World.world, (Object) null) || World.world_era == null)
      return;
    this._camera = World.world.camera;
    this.updateParticles(this._system_rain, this._mat_rain, World.world_era.particles_rain);
    this.updateParticles(this._system_snow, this._mat_snow, World.world_era.particles_snow);
    this.updateParticles(this._system_magic, this._mat_magic, World.world_era.particles_magic);
    this.updateParticles(this._system_ash, this._mat_ash, World.world_era.particles_ash);
    this.updateParticles(this._system_sun_blobs, this._mat_sun_blobs, World.world_era.particles_sun);
    this.updateParticles(this._system_sun_rays, this._mat_sun_ray, World.world_era.particles_sun);
  }

  private void updateParticles(ParticleSystem pSystem, Material pMaterial, bool pEnabled)
  {
    if (!WorldAgesParticles.effects_enabled)
      pEnabled = false;
    Color color = pMaterial.color;
    bool flag = MapBox.isRenderGameplay() & pEnabled;
    if ((double) color.a != 0.0 && !flag && !pSystem.isPlaying)
      return;
    int width = MapBox.width;
    int height = MapBox.height;
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector((float) (width / 2), (float) (height / 2));
    ((Component) pSystem).transform.localPosition = vector3;
    ParticleSystem.ShapeModule shape = pSystem.shape;
    ((ParticleSystem.ShapeModule) ref shape).scale = new Vector3((float) width * 1.5f, (float) height * 1.5f, 1f);
    if (!flag)
    {
      if ((double) color.a > 0.0)
        color.a -= World.world.delta_time * 0.1f;
    }
    else if ((double) color.a < 1.0)
    {
      color.a += World.world.delta_time * 0.1f;
      if ((double) color.a > 1.0)
        color.a = 1f;
    }
    if ((double) color.a <= 0.0)
    {
      color.a = 0.0f;
      pSystem.Stop(false, (ParticleSystemStopBehavior) 0);
    }
    else if (!pSystem.isPlaying)
      pSystem.Play();
    pMaterial.color = color;
  }
}
