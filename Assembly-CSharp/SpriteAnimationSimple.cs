// Decompiled with JetBrains decompiler
// Type: SpriteAnimationSimple
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SpriteAnimationSimple : MonoBehaviour
{
  [SerializeField]
  public float _time_between_frames = 0.1f;
  private Image _renderer;
  [SerializeField]
  public Sprite[] _frames;
  private EffectParticlesCursorDelegate _action_finish;
  private int _frame_index_current;
  private float _next_frame_time;

  private void Awake() => this._renderer = ((Component) this).GetComponent<Image>();

  public void setActionFinish(EffectParticlesCursorDelegate pAction)
  {
    this._action_finish = pAction;
  }

  public void resetAnim()
  {
    this._frame_index_current = 0;
    this._next_frame_time = this._time_between_frames;
    this.updateFrame();
  }

  internal virtual void update(float pElapsed)
  {
    if ((double) this._next_frame_time > 0.0)
    {
      this._next_frame_time -= pElapsed;
      if ((double) this._next_frame_time > 0.0)
        return;
    }
    this._next_frame_time = this._time_between_frames;
    ++this._frame_index_current;
    if (this._frame_index_current >= this._frames.Length)
    {
      if (this._action_finish == null)
        return;
      this._action_finish((MonoBehaviour) this);
    }
    else
      this.updateFrame();
  }

  private void updateFrame() => this._renderer.sprite = this._frames[this._frame_index_current];

  public void setFrames(Sprite[] pFrames) => this._frames = pFrames;
}
