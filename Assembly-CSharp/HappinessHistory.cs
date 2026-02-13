// Decompiled with JetBrains decompiler
// Type: HappinessHistory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public struct HappinessHistory
{
  public int index;
  public double timestamp;
  public int bonus;

  public HappinessAsset asset => AssetManager.happiness_library.list[this.index];

  public string getAgoString() => Date.getAgoString(this.timestamp);

  public double elapsedSince() => (double) World.world.getWorldTimeElapsedSince(this.timestamp);
}
