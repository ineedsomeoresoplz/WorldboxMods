// Decompiled with JetBrains decompiler
// Type: HeatRayEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class HeatRayEffect : BaseAnimatedObject
{
  public SpriteAnimation ray;
  public SpriteAnimation heat;
  private bool active;
  private int ticksActive;
  private bool touchedGround;
  private float rayScaleY;
  private float rayWidth = 1f;

  public override void Awake()
  {
    base.Awake();
    ((Component) this.ray).transform.localScale = new Vector3(1f, 0.0f, 1f);
  }

  private void Update()
  {
    this.update(World.world.elapsed);
    this.ray.update(World.world.elapsed);
    this.heat.update(World.world.elapsed);
    if (this.ticksActive > 0)
      --this.ticksActive;
    else
      this.active = false;
  }

  internal bool isReady() => this.touchedGround;

  public Vector2 getPosForLight()
  {
    return Vector2.op_Implicit(((Component) this.heat).transform.position);
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    Vector3 position = ((Component) this.ray).transform.position;
    position.z = ((Component) this).transform.position.y;
    ((Component) this.ray).transform.position = position;
    ((Component) this.heat).transform.position = ((Component) this.heat).transform.position;
    if (this.active)
    {
      if ((double) this.rayScaleY < 2000.0)
      {
        this.rayScaleY += pElapsed * 7000f;
        if ((double) this.rayScaleY >= 2000.0)
        {
          this.rayScaleY = 2000f;
          this.touchedGround = true;
        }
        ((Component) this.ray).transform.localScale = new Vector3(this.rayWidth, this.rayScaleY, 1f);
      }
    }
    else
    {
      this.touchedGround = false;
      if ((double) this.rayScaleY > 0.0)
      {
        this.rayScaleY -= pElapsed * 4000f;
        if ((double) this.rayScaleY < 0.0)
        {
          this.rayScaleY = 0.0f;
          ((Component) this).gameObject.SetActive(false);
        }
        ((Component) this.ray).transform.localScale = new Vector3(this.rayWidth, this.rayScaleY, 1f);
      }
    }
    ((Component) this.heat).gameObject.SetActive(this.touchedGround);
  }

  internal void play(Vector2 pPos, int pSize)
  {
    this.rayWidth = pSize < 10 ? 0.4f : 1f;
    ((Component) this).transform.localPosition = new Vector3(pPos.x, pPos.y);
    this.active = true;
    this.ticksActive = 4;
    ((Component) this).gameObject.SetActive(true);
  }
}
