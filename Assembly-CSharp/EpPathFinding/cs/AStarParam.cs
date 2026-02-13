// Decompiled with JetBrains decompiler
// Type: EpPathFinding.cs.AStarParam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace EpPathFinding.cs;

public class AStarParam : ParamBase
{
  internal float weight;
  internal int max_open_list = -1;
  internal bool roads;
  internal bool use_global_path_lock;
  internal bool boat;
  internal bool limit;
  internal bool swamp;
  internal bool ocean;
  internal bool lava;
  internal bool fire;
  internal bool block;
  internal bool ground;
  internal bool end_to_start_path;

  public void resetParam()
  {
    this.swamp = false;
    this.roads = false;
    this.ocean = false;
    this.lava = false;
    this.ground = false;
    this.use_global_path_lock = false;
    this.boat = false;
    this.limit = false;
    this.fire = false;
    this.end_to_start_path = false;
  }

  internal override void _reset(GridPos iStartPos, GridPos iEndPos, BaseGrid iSearchGrid = null)
  {
  }
}
