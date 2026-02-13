// Decompiled with JetBrains decompiler
// Type: BattleContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class BattleContainer
{
  public float timer = 1f;
  public float timeout = 1f;
  public float timer_animation = 0.05f;
  public int frame;
  private int _deaths_civs;
  private int _deaths_mobs;
  public WorldTile tile;

  public int getDeathsTotal() => this._deaths_civs + this._deaths_mobs;

  public void increaseDeaths(Actor pActor)
  {
    if (pActor.isSapient())
      ++this._deaths_civs;
    else
      ++this._deaths_mobs;
  }

  public bool isRendered() => this._deaths_civs > 3 || this._deaths_mobs > 6;
}
