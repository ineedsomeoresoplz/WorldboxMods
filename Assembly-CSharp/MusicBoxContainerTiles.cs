// Decompiled with JetBrains decompiler
// Type: MusicBoxContainerTiles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MusicBoxContainerTiles
{
  public int amount;
  public float percent;
  public bool enabled;
  public Vector2 cur_pan;
  private Vector2 _last_pan;
  private float _chunks;
  public MusicAsset asset;

  public void clear()
  {
    this.amount = 0;
    ((Vector2) ref this._last_pan).Set(-1f, -1f);
    this._chunks = 0.0f;
  }

  public void count(int pAmount, float pWhereFromX, float pWhereFromY)
  {
    this.amount += pAmount;
    ++this._chunks;
    this._last_pan.x += pWhereFromX;
    this._last_pan.y += pWhereFromY;
  }

  public void calculatePan()
  {
    this._last_pan.x /= this._chunks + 1f;
    this._last_pan.y /= this._chunks + 1f;
    if ((double) this._chunks == 0.0)
      ((Vector2) ref this.cur_pan).Set(-1f, -1f);
    else if ((double) this.cur_pan.x == -1.0 && (double) this.cur_pan.y == -1.0)
      ((Vector2) ref this.cur_pan).Set(this._last_pan.x, this._last_pan.y);
    else
      this.cur_pan = Vector2.MoveTowards(this.cur_pan, this._last_pan, 5f);
  }
}
