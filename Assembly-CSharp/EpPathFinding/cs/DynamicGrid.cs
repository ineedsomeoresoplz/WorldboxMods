// Decompiled with JetBrains decompiler
// Type: EpPathFinding.cs.DynamicGrid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace EpPathFinding.cs;

public class DynamicGrid : BaseGrid
{
  protected Dictionary<GridPos, Node> m_nodes;
  private bool m_notSet;

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

  public DynamicGrid(List<GridPos> iWalkableGridList = null)
  {
    this.m_gridRect = new GridRect();
    this.m_gridRect.minX = 0;
    this.m_gridRect.minY = 0;
    this.m_gridRect.maxX = 0;
    this.m_gridRect.maxY = 0;
    this.m_notSet = true;
    this.buildNodes(iWalkableGridList);
  }

  public DynamicGrid(DynamicGrid b)
    : base((BaseGrid) b)
  {
    this.m_notSet = b.m_notSet;
    this.m_nodes = new Dictionary<GridPos, Node>((IDictionary<GridPos, Node>) b.m_nodes);
  }

  protected void buildNodes(List<GridPos> iWalkableGridList)
  {
    this.m_nodes = new Dictionary<GridPos, Node>();
    if (iWalkableGridList == null)
      return;
    foreach (GridPos iWalkableGrid in iWalkableGridList)
      this.SetWalkableAt(iWalkableGrid.x, iWalkableGrid.y, true, 1);
  }

  public override Node GetNodeAt(int iX, int iY) => this.GetNodeAt(new GridPos(iX, iY));

  public override bool IsWalkableAt(int iX, int iY) => this.IsWalkableAt(new GridPos(iX, iY));

  private void setBoundingBox()
  {
    this.m_notSet = true;
    foreach (KeyValuePair<GridPos, Node> node in this.m_nodes)
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
    GridPos key = new GridPos(iX, iY);
    if (iWalkable)
    {
      if (this.m_nodes.ContainsKey(key))
        return true;
      if (iX < this.m_gridRect.minX || this.m_notSet)
        this.m_gridRect.minX = iX;
      if (iX > this.m_gridRect.maxX || this.m_notSet)
        this.m_gridRect.maxX = iX;
      if (iY < this.m_gridRect.minY || this.m_notSet)
        this.m_gridRect.minY = iY;
      if (iY > this.m_gridRect.maxY || this.m_notSet)
        this.m_gridRect.maxY = iY;
      this.m_nodes.Add(new GridPos(key.x, key.y), new Node(key.x, key.y, new bool?(iWalkable)));
    }
    else if (this.m_nodes.ContainsKey(key))
    {
      this.m_nodes.Remove(key);
      if (iX == this.m_gridRect.minX || iX == this.m_gridRect.maxX || iY == this.m_gridRect.minY || iY == this.m_gridRect.maxY)
        this.m_notSet = true;
    }
    return true;
  }

  public override Node GetNodeAt(GridPos iPos)
  {
    return this.m_nodes.ContainsKey(iPos) ? this.m_nodes[iPos] : (Node) null;
  }

  public override bool IsWalkableAt(GridPos iPos) => this.m_nodes.ContainsKey(iPos);

  public override bool SetWalkableAt(GridPos iPos, bool iWalkable)
  {
    return this.SetWalkableAt(iPos.x, iPos.y, iWalkable, 1);
  }

  public override void Reset() => this.Reset((List<GridPos>) null);

  public void Reset(List<GridPos> iWalkableGridList)
  {
    foreach (KeyValuePair<GridPos, Node> node in this.m_nodes)
      node.Value.Reset();
    if (iWalkableGridList == null)
      return;
    foreach (KeyValuePair<GridPos, Node> node in this.m_nodes)
    {
      if (iWalkableGridList.Contains(node.Key))
        this.SetWalkableAt(node.Key, true);
      else
        this.SetWalkableAt(node.Key, false);
    }
  }

  public override BaseGrid Clone()
  {
    DynamicGrid dynamicGrid = new DynamicGrid();
    foreach (KeyValuePair<GridPos, Node> node in this.m_nodes)
      dynamicGrid.SetWalkableAt(node.Key.x, node.Key.y, true, 1);
    return (BaseGrid) dynamicGrid;
  }
}
