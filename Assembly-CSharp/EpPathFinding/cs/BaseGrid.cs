// Decompiled with JetBrains decompiler
// Type: EpPathFinding.cs.BaseGrid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EpPathFinding.cs;

public abstract class BaseGrid : IDisposable
{
  public readonly List<Node> closedList = new List<Node>();
  public int closed_list_count;
  public const int CLOSED_LIST_MINIMUM_ELEMENTS = 10;
  protected GridRect m_gridRect;

  public BaseGrid() => this.m_gridRect = new GridRect();

  public BaseGrid(BaseGrid b)
  {
    this.m_gridRect = new GridRect(b.m_gridRect);
    this.width = b.width;
    this.height = b.height;
  }

  public GridRect gridRect => this.m_gridRect;

  public abstract int width { get; protected set; }

  public abstract int height { get; protected set; }

  public abstract Node GetNodeAt(int iX, int iY);

  public abstract bool IsWalkableAt(int iX, int iY);

  public abstract bool SetWalkableAt(int iX, int iY, bool iWalkable, int pCost = 1);

  public abstract Node GetNodeAt(GridPos iPos);

  public abstract bool IsWalkableAt(GridPos iPos);

  public abstract bool SetWalkableAt(GridPos iPos, bool iWalkable);

  public List<Node> GetNeighbors(Node iNode, DiagonalMovement diagonalMovement)
  {
    int x = iNode.x;
    int y = iNode.y;
    List<Node> neighbors = new List<Node>();
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = false;
    bool flag4 = false;
    bool flag5 = false;
    bool flag6 = false;
    bool flag7 = false;
    bool flag8 = false;
    GridPos iPos = new GridPos();
    if (this.IsWalkableAt(iPos.Set(x, y - 1)))
    {
      neighbors.Add(this.GetNodeAt(iPos));
      flag1 = true;
    }
    if (this.IsWalkableAt(iPos.Set(x + 1, y)))
    {
      neighbors.Add(this.GetNodeAt(iPos));
      flag3 = true;
    }
    if (this.IsWalkableAt(iPos.Set(x, y + 1)))
    {
      neighbors.Add(this.GetNodeAt(iPos));
      flag5 = true;
    }
    if (this.IsWalkableAt(iPos.Set(x - 1, y)))
    {
      neighbors.Add(this.GetNodeAt(iPos));
      flag7 = true;
    }
    switch (diagonalMovement)
    {
      case DiagonalMovement.Always:
        flag2 = true;
        flag4 = true;
        flag6 = true;
        flag8 = true;
        break;
      case DiagonalMovement.IfAtLeastOneWalkable:
        flag2 = flag7 | flag1;
        flag4 = flag1 | flag3;
        flag6 = flag3 | flag5;
        flag8 = flag5 | flag7;
        break;
      case DiagonalMovement.OnlyWhenNoObstacles:
        flag2 = flag7 & flag1;
        flag4 = flag1 & flag3;
        flag6 = flag3 & flag5;
        flag8 = flag5 & flag7;
        break;
    }
    if (flag2 && this.IsWalkableAt(iPos.Set(x - 1, y - 1)))
      neighbors.Add(this.GetNodeAt(iPos));
    if (flag4 && this.IsWalkableAt(iPos.Set(x + 1, y - 1)))
      neighbors.Add(this.GetNodeAt(iPos));
    if (flag6 && this.IsWalkableAt(iPos.Set(x + 1, y + 1)))
      neighbors.Add(this.GetNodeAt(iPos));
    if (flag8 && this.IsWalkableAt(iPos.Set(x - 1, y + 1)))
      neighbors.Add(this.GetNodeAt(iPos));
    return neighbors;
  }

  public void addToClosed(Node pNode)
  {
    this.closedList.Add(pNode);
    ++this.closed_list_count;
  }

  public abstract void Reset();

  public abstract BaseGrid Clone();

  public virtual void Dispose()
  {
    foreach (Node closed in this.closedList)
      closed.Dispose();
    this.closedList.Clear();
    this.closed_list_count = 0;
    this.m_gridRect = (GridRect) null;
  }
}
