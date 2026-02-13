// Decompiled with JetBrains decompiler
// Type: EpPathFinding.cs.DynamicGridWPool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace EpPathFinding.cs;

public class DynamicGridWPool : BaseGrid
{
  private bool m_notSet;
  private NodePool m_nodePool;

  public override int width
  {
    get
    {
      if (this.m_notSet)
        this.setBoundingBox();
      return this.m_gridRect.maxX - this.m_gridRect.minX + 1;
    }
    protected set
    {
    }
  }

  public override int height
  {
    get
    {
      if (this.m_notSet)
        this.setBoundingBox();
      return this.m_gridRect.maxY - this.m_gridRect.minY + 1;
    }
    protected set
    {
    }
  }

  public DynamicGridWPool(NodePool iNodePool)
  {
    this.m_gridRect = new GridRect();
    this.m_gridRect.minX = 0;
    this.m_gridRect.minY = 0;
    this.m_gridRect.maxX = 0;
    this.m_gridRect.maxY = 0;
    this.m_notSet = true;
    this.m_nodePool = iNodePool;
  }

  public DynamicGridWPool(DynamicGridWPool b)
    : base((BaseGrid) b)
  {
    this.m_notSet = b.m_notSet;
    this.m_nodePool = b.m_nodePool;
  }

  public override Node GetNodeAt(int iX, int iY) => this.GetNodeAt(new GridPos(iX, iY));

  public override bool IsWalkableAt(int iX, int iY) => this.IsWalkableAt(new GridPos(iX, iY));

  private void setBoundingBox()
  {
    this.m_notSet = true;
    foreach (KeyValuePair<GridPos, Node> node in this.m_nodePool.Nodes)
    {
      if (node.Key.x < this.m_gridRect.minX || this.m_notSet)
        this.m_gridRect.minX = node.Key.x;
      if (node.Key.x > this.m_gridRect.maxX || this.m_notSet)
        this.m_gridRect.maxX = node.Key.x;
      if (node.Key.y < this.m_gridRect.minY || this.m_notSet)
        this.m_gridRect.minY = node.Key.y;
      if (node.Key.y > this.m_gridRect.maxY || this.m_notSet)
        this.m_gridRect.maxY = node.Key.y;
      this.m_notSet = false;
    }
    this.m_notSet = false;
  }

  public override bool SetWalkableAt(int iX, int iY, bool iWalkable, int pCost = 1)
  {
    this.m_nodePool.SetNode(new GridPos(iX, iY), new bool?(iWalkable));
    if (iWalkable)
    {
      if (iX < this.m_gridRect.minX || this.m_notSet)
        this.m_gridRect.minX = iX;
      if (iX > this.m_gridRect.maxX || this.m_notSet)
        this.m_gridRect.maxX = iX;
      if (iY < this.m_gridRect.minY || this.m_notSet)
        this.m_gridRect.minY = iY;
      if (iY > this.m_gridRect.maxY || this.m_notSet)
        this.m_gridRect.maxY = iY;
    }
    else if (iX == this.m_gridRect.minX || iX == this.m_gridRect.maxX || iY == this.m_gridRect.minY || iY == this.m_gridRect.maxY)
      this.m_notSet = true;
    return true;
  }

  public override Node GetNodeAt(GridPos iPos) => this.m_nodePool.GetNode(iPos);

  public override bool IsWalkableAt(GridPos iPos) => this.m_nodePool.Nodes.ContainsKey(iPos);

  public override bool SetWalkableAt(GridPos iPos, bool iWalkable)
  {
    return this.SetWalkableAt(iPos.x, iPos.y, iWalkable, 1);
  }

  public override void Reset()
  {
    foreach (KeyValuePair<GridPos, Node> node in this.m_nodePool.Nodes)
      node.Value.Reset();
  }

  public override BaseGrid Clone() => (BaseGrid) new DynamicGridWPool(this.m_nodePool);
}
