// Decompiled with JetBrains decompiler
// Type: GlowParticles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class GlowParticles : MonoBehaviour
{
  private float cooldown;
  public ParticleSystem particles;

  private void Awake() => this.particles = ((Component) this).GetComponent<ParticleSystem>();

  private void Update()
  {
    if ((double) this.cooldown <= 0.0)
      return;
    this.cooldown -= Time.deltaTime;
  }

  public void spawn(float pX, float pY, bool pRemoveCooldown = false)
  {
    if (!((Behaviour) this).enabled || this.particles.particleCount > 50 || !MapBox.isRenderGameplay())
      return;
    if (pRemoveCooldown)
      this.cooldown = 0.0f;
    if ((double) this.cooldown > 0.0)
      return;
    this.cooldown = 0.2f + Randy.randomFloat(0.0f, 0.3f);
    ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
    ((ParticleSystem.EmitParams) ref emitParams).position = new Vector3(pX, pY);
    this.particles.Emit(emitParams, 1);
  }

  public void spawn(Vector3 pPos) => this.spawn(pPos.x, pPos.y);

  public void clear() => this.particles.Clear();
}
