// Decompiled with JetBrains decompiler
// Type: DebugMusicBoxData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using FMOD.Studio;

#nullable disable
public class DebugMusicBoxData
{
  public const float INTERVAL = 3f;
  public float timer = 3f;
  public string path;
  public float x;
  public float y;
  public EventInstance instance;

  public bool isPlaying()
  {
    PLAYBACK_STATE playbackState;
    ((EventInstance) ref this.instance).getPlaybackState(ref playbackState);
    return playbackState == 0;
  }
}
