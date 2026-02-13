// Decompiled with JetBrains decompiler
// Type: ThunderFlash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ThunderFlash : BaseEffect
{
  private float _last_alpha = 1f;
  private int _blinks;
  private int _cur_blinks = 3;
  private float _timer_blink = 0.1f;

  internal void spawnFlash()
  {
    this.prepare(Vector2.op_Implicit(Vector3.zero), 0.3f);
    this.updatePos();
    this._blinks = Randy.randomInt(6, 10);
    this._cur_blinks = this._blinks;
    this.startBlink();
  }

  private void updatePos()
  {
    float width = (float) ((Texture) this.sprite_renderer.sprite.texture).width;
    float height = (float) ((Texture) this.sprite_renderer.sprite.texture).height;
    Vector3 position = ((Component) World.world.camera).transform.position;
    double num1 = (double) (World.world.camera.orthographicSize * 2f);
    float num2 = (float) (num1 / (double) Screen.height * (double) Screen.width / (double) width * 1.0);
    float num3 = (float) (num1 / (double) height * 1.0);
    float num4 = 4f;
    float num5 = 4f;
    ((Component) this).transform.localPosition = new Vector3(position.x, position.y + (float) ((double) num3 * (double) height / 2.0));
    ((Component) this).transform.localScale = new Vector3(num2 * num4, num3 * num5);
  }

  private void setColor(float pAlpha = 1f)
  {
    this._last_alpha = pAlpha;
    Color color;
    // ISSUE: explicit constructor call
    ((Color) ref color).\u002Ector(1f, 1f, 1f, pAlpha);
    this.sprite_renderer.color = color;
  }

  private void startBlink()
  {
    this._timer_blink = Randy.randomFloat(0.0f, 0.1f);
    this.setColor(0.4f);
  }

  public override void update(float pElapsed)
  {
    pElapsed = Time.deltaTime;
    base.update(pElapsed);
    this.updatePos();
    if ((double) this._last_alpha > 0.0)
    {
      this._last_alpha -= pElapsed * 2f;
      if ((double) this._last_alpha < 0.0)
        this._last_alpha = 0.0f;
    }
    this.setColor(this._last_alpha);
    if ((double) this._timer_blink > 0.0)
    {
      this._timer_blink -= pElapsed;
      if ((double) this._timer_blink > 0.0)
        return;
      --this._cur_blinks;
      if (this._cur_blinks != 0)
      {
        this.startBlink();
        return;
      }
    }
    if ((double) this._last_alpha > 0.0)
      return;
    this.kill();
  }
}
