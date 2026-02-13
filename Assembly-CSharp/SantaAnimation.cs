// Decompiled with JetBrains decompiler
// Type: SantaAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;

#nullable disable
public class SantaAnimation : BaseMapObject
{
  public float shakeX = 2f;
  public float shakeY = 0.3f;
  private Tween shakeTween;
  private Vector3 tStr;
  private Santa santa;
  private SpriteRenderer spriteRenderer;

  internal override void create()
  {
    base.create();
    this.tStr = new Vector3(this.shakeX, this.shakeY);
    this.shakeTween = (Tween) ShortcutExtensions.DOShakePosition(((Component) this).transform, 0.5f, this.tStr, 10, 90f, false, false, (ShakeRandomnessMode) 0);
    this.santa = ((Component) ((Component) this).transform.parent).GetComponent<Santa>();
    this.spriteRenderer = ((Component) this).GetComponent<SpriteRenderer>();
  }

  private void Update()
  {
    if (this.santa.alive)
      ((Renderer) this.spriteRenderer).sharedMaterial = this.santa.current_material;
    else
      ((Renderer) this.spriteRenderer).sharedMaterial = LibraryMaterials.instance.mat_world_object;
    if (World.world.isPaused() || this.shakeTween.active)
      return;
    this.shakeTween = (Tween) ShortcutExtensions.DOShakePosition(((Component) this).transform, 0.5f, this.tStr, 10, 90f, false, false, (ShakeRandomnessMode) 0);
  }
}
