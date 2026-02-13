// Decompiled with JetBrains decompiler
// Type: EpPathFinding.cs.StaticGrid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Runtime.CompilerServices;

#nullable disable
namespace EpPathFinding.cs;

public class StaticGrid : BaseGrid
{
  public Node[][] m_nodes;

  public override int width { get; protected set; }

  public override int height { get; protected set; }

  public StaticGrid(int iWidth, int iHeight, bool[][] iMatrix = null)
  {
    this.width = iWidth;
    this.height = iHeight;
    this.m_gridRect.minX = 0;
    this.m_gridRect.minY = 0;
    this.m_gridRect.maxX = iWidth - 1;
    this.m_gridRect.maxY = iHeight - 1;
    this.m_nodes = this.buildNodes(iWidth, iHeight, iMatrix);
  }

  public StaticGrid(StaticGrid b)
    : base((BaseGrid) b)
  {
    bool[][] iMatrix = new bool[b.width][];
    for (int iX = 0; iX < b.width; ++iX)
    {
      iMatrix[iX] = new bool[b.height];
      for (int iY = 0; iY < b.height; ++iY)
        iMatrix[iX][iY] = b.IsWalkableAt(iX, iY);
    }
    this.m_nodes = this.buildNodes(b.width, b.height, iMatrix);
  }

  private Node[][] buildNodes(int iWidth, int iHeight, bool[][] iMatrix)
  {
    Node[][] nodeArray = new Node[iWidth][];
    for (int iX = 0; iX < iWidth; ++iX)
    {
      nodeArray[iX] = new Node[iHeight];
      for (int iY = 0; iY < iHeight; ++iY)
        nodeArray[iX][iY] = new Node(iX, iY);
    }
    return nodeArray;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public override Node GetNodeAt(int iX, int iY) => this.m_nodes[iX][iY];

  public override bool IsWalkableAt(int iX, int iY) => this.isInside(iX, iY);

  protected bool isInside(int iX, int iY)
  {
    return iX >= 0 && iX < this.width && iY >= 0 && iY < this.height;
  }

  public override bool SetWalkableAt(int iX, int iY, bool iWalkable, int pCost = 1) => true;

  public void SetTileNode(int iX, int iY, WorldTile pTile) => this.m_nodes[iX][iY].tile = pTile;

  protected bool isInside(GridPos iPos) => this.isInside(iPos.x, iPos.y);

  public override Node GetNodeAt(GridPos iPos) => this.GetNodeAt(iPos.x, iPos.y);

  public override bool IsWalkableAt(GridPos iPos) => this.IsWalkableAt(iPos.x, iPos.y);

  public override bool SetWalkableAt(GridPos iPos, bool iWalkable)
  {
    return this.SetWalkableAt(iPos.x, iPos.y, iWalkable, 1);
  }

  public override void Reset() => this.Reset((bool[][]) null);

  public void Reset(bool[][] iMatrix)
  {
    foreach (Node closed in this.closedList)
      closed.Reset();
    this.closedList.Clear();
    this.closed_list_count = 0;
  }

  public override BaseGrid Clone()
  {
    int width = this.width;
    int height = this.height;
    Node[][] nodes = this.m_nodes;
    StaticGrid staticGrid = new StaticGrid(width, height);
    Node[][] nodeArray = new Node[width][];
    for (int iX = 0; iX < width; ++iX)
    {
      nodeArray[iX] = new Node[height];
      for (int iY = 0; iY < height; ++iY)
        nodeArray[iX][iY] = new Node(iX, iY, new bool?(false));
    }
    staticGrid.m_nodes = nodeArray;
    return (BaseGrid) staticGrid;
  }

  public override void Dispose()
  {
    Node[][] nodes = this.m_nodes;
    for (int index1 = 0; index1 < this.width; ++index1)
    {
      for (int index2 = 0; index2 < this.height; ++index2)
        nodes[index1][index2].Dispose();
    }
    this.m_nodes = (Node[][]) null;
    base.Dispose();
  }
}
