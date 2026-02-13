// Decompiled with JetBrains decompiler
// Type: MusicBoxContainerUnits
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class MusicBoxContainerUnits
{
  public MusicAsset asset;
  public int units;
  public bool enabled;

  public void clear()
  {
    this.units = 0;
    this.enabled = false;
  }
}
