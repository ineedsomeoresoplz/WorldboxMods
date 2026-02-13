// Decompiled with JetBrains decompiler
// Type: EpPathFinding.cs.PartialGridWPool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace EpPathFinding.cs;

public class PartialGridWPool : BaseGrid
{
  private NodePool m_nodePool;

  public override int width
  {
    get => this.m_gridRect.maxX - this.m_gridRect.minX + 1;
    protected set
    {
    }
  }

  public override int height
  {
    get => this.m_gridRect.maxY - this.m_gridRect.minY + 1;
    protected set
    {
    }
  }

  public PartialGridWPool(NodePool iNodePool, GridRect iGridRect = null)
  {
    if (iGridRect == (GridRect) null)
      this.m_gridRect = new GridRect();
    else
      this.m_gridRect = iGridRect;
    this.m_nodePool = iNodePool;
  }

  public PartialGridWPool(PartialGridWPool b)
    : base((BaseGrid) b)
  {
    this.m_nodePool = b.m_nodePool;
  }

  public void SetGridRect(GridRect iGridRect) => this.m_gridRect = iGridRect;

  public bool IsInside(int iX, int iY)
  {
    return iX >= this.m_gridRect.minX && iX <= this.m_gridRect.maxX && iY >= this.m_gridRect.minY && iY <= this.m_gridRect.maxY;
  }

  public override Node GetNodeAt(int iX, int iY) => this.GetNodeAt(new GridPos(iX, iY));

  public override bool IsWalkableAt(int iX, int iY) => this.IsWalkableAt(new GridPos(iX, iY));

  public override bool SetWalkableAt(int iX, int iY, bool iWalkable, int pCost = 1)
  {
    if (!this.IsInside(iX, iY))
      return false;
    this.m_nodePool.SetNode(new GridPos(iX, iY), new bool?(iWalkable));
    return true;
  }

  public bool IsInside(GridPos iPos) => this.IsInside(iPos.x, iPos.y);

  public override Node GetNodeAt(GridPos iPos)
  {
    return !this.IsInside(iPos) ? (Node) null : this.m_nodePool.GetNode(iPos);
  }

  public override bool IsWalkableAt(GridPos iPos)
  {
    return this.IsInside(iPos) && this.m_nodePool.Nodes.ContainsKey(iPos);
  }

  public override bool SetWalkableAt(GridPos iPos, bool iWalkable)
  {
    return this.SetWalkableAt(iPos.x, iPos.y, iWalkable, 1);
  }

  public override void Reset()
  {
    if (this.m_nodePool.Nodes.Count > (this.m_gridRect.maxX - this.m_gridRect.minX) * (this.m_gridRect.maxY - this.m_gridRect.minY))
    {
      GridPos iPos = new GridPos(0, 0);
      for (int minX = this.m_gridRect.minX; minX <= this.m_gridRect.maxX; ++minX)
      {
        iPos.x = minX;
        for (int minY = this.m_gridRect.minY; minY <= this.m_gridRect.maxY; ++minY)
        {
          iPos.y = minY;
          Node node = this.m_nodePool.GetNode(iPos);
          if (node != (Node) null)
            node.Reset();
        }
      }
    }
    else
    {
      foreach (KeyValuePair<GridPos, Node> node in this.m_nodePool.Nodes)
        node.Value.Reset();
    }
  }

  public override BaseGrid Clone()
  {
    return (BaseGrid) new PartialGridWPool(this.m_nodePool, this.m_gridRect);
  }
}
