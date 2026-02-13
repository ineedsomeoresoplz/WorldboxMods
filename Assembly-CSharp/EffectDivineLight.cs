// Decompiled with JetBrains decompiler
// Type: EffectDivineLight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EffectDivineLight : BaseAnimatedObject
{
  public SpriteAnimation raySpawn;
  public SpriteAnimation rayIdle;
  public SpriteAnimation baseSpawn;
  public SpriteAnimation baseIdle;
  public bool isOn;
  private DivineLightState state;

  public override void Awake()
  {
    base.Awake();
    this.setState(DivineLightState.SpawnFirstStage);
  }

  private void setState(DivineLightState pState)
  {
    this.state = pState;
    switch (this.state)
    {
      case DivineLightState.SpawnFirstStage:
        ((Component) this.raySpawn).gameObject.SetActive(true);
        ((Component) this.rayIdle).gameObject.SetActive(false);
        ((Component) this.baseSpawn).gameObject.SetActive(false);
        ((Component) this.baseIdle).gameObject.SetActive(false);
        break;
      case DivineLightState.SpawnSecondStage:
        ((Component) this.raySpawn).gameObject.SetActive(false);
        ((Component) this.rayIdle).gameObject.SetActive(true);
        ((Component) this.baseSpawn).gameObject.SetActive(true);
        ((Component) this.baseIdle).gameObject.SetActive(false);
        break;
      case DivineLightState.Idle:
        ((Component) this.raySpawn).gameObject.SetActive(false);
        ((Component) this.rayIdle).gameObject.SetActive(true);
        ((Component) this.baseSpawn).gameObject.SetActive(false);
        ((Component) this.baseIdle).gameObject.SetActive(true);
        break;
      case DivineLightState.Hide:
        ((Component) this.raySpawn).gameObject.SetActive(true);
        ((Component) this.rayIdle).gameObject.SetActive(false);
        ((Component) this.baseSpawn).gameObject.SetActive(true);
        ((Component) this.baseIdle).gameObject.SetActive(false);
        break;
    }
  }

  private void stopEffet()
  {
  }

  private void useEffect()
  {
  }

  private void Update()
  {
    if (this.isOn)
    {
      this.raySpawn.playType = AnimPlayType.Forward;
      this.baseSpawn.playType = AnimPlayType.Forward;
      if (this.raySpawn.isLastFrame())
      {
        ((Component) this.raySpawn).gameObject.SetActive(false);
        ((Component) this.rayIdle).gameObject.SetActive(true);
      }
      else
      {
        ((Component) this.raySpawn).gameObject.SetActive(true);
        ((Component) this.rayIdle).gameObject.SetActive(false);
      }
      if (this.baseSpawn.isLastFrame())
      {
        ((Component) this.baseSpawn).gameObject.SetActive(false);
        ((Component) this.baseIdle).gameObject.SetActive(true);
      }
      else
      {
        ((Component) this.baseSpawn).gameObject.SetActive(true);
        ((Component) this.baseIdle).gameObject.SetActive(false);
      }
    }
    else
    {
      this.raySpawn.playType = AnimPlayType.Backward;
      this.baseSpawn.playType = AnimPlayType.Backward;
      ((Component) this.rayIdle).gameObject.SetActive(false);
      ((Component) this.baseIdle).gameObject.SetActive(false);
      if (this.raySpawn.isFirstFrame())
        ((Component) this.raySpawn).gameObject.SetActive(false);
      else
        ((Component) this.raySpawn).gameObject.SetActive(true);
      if (this.baseSpawn.isFirstFrame())
        ((Component) this.baseSpawn).gameObject.SetActive(false);
      else
        ((Component) this.baseSpawn).gameObject.SetActive(true);
    }
    if (((Component) this.baseSpawn).gameObject.activeSelf)
      this.baseSpawn.update(World.world.delta_time);
    if (((Component) this.baseIdle).gameObject.activeSelf)
      this.baseIdle.update(World.world.delta_time);
    if (((Component) this.raySpawn).gameObject.activeSelf)
      this.raySpawn.update(World.world.delta_time);
    if (((Component) this.rayIdle).gameObject.activeSelf)
      this.rayIdle.update(World.world.delta_time);
    this.isOn = false;
  }

  public void playOn(WorldTile pTile)
  {
    ((Component) this).gameObject.transform.localPosition = pTile.posV3;
    this.isOn = true;
  }
}
