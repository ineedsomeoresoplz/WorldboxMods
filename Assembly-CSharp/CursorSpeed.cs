// Decompiled with JetBrains decompiler
// Type: CursorSpeed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CursorSpeed
{
  private Vector2 _lastFramePos;
  private Vector2 _curFramePos;
  private float difference;
  public float speed;
  public float fmod_speed;

  public void update()
  {
    if (Input.GetMouseButton(0))
    {
      ((Vector2) ref this._lastFramePos).Set(this._curFramePos.x, this._curFramePos.y);
      Vector3 mousePosition = Input.mousePosition;
      ((Vector2) ref this._curFramePos).Set(mousePosition.x, mousePosition.y);
      this.difference = Toolbox.DistVec2Float(this._curFramePos, this._lastFramePos) / 2f;
      if ((double) this.difference > (double) this.speed)
        this.speed = this.difference;
    }
    this.speed = (float) ((double) this.speed * 0.949999988079071 - 1.0);
    if ((double) this.speed < 0.0)
      this.speed = 0.0f;
    this.fmod_speed = (float) (int) this.speed;
    if ((double) this.fmod_speed <= 100.0)
      return;
    this.fmod_speed = 100f;
  }

  public void debug(DebugTool pTool)
  {
    pTool.setText("difference", (object) this.difference);
    pTool.setText("speed", (object) this.speed);
    pTool.setText("fmod_speed", (object) this.fmod_speed);
  }
}
