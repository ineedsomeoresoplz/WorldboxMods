// Decompiled with JetBrains decompiler
// Type: EpPathFinding.cs.ParamBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace EpPathFinding.cs;

public abstract class ParamBase
{
  internal BaseGrid m_searchGrid;
  internal Node m_startNode;
  internal Node m_endNode;
  internal HeuristicMode m_heuristicMode;
  public DiagonalMovement DiagonalMovement;

  public ParamBase(
    BaseGrid iGrid,
    GridPos iStartPos,
    GridPos iEndPos,
    DiagonalMovement iDiagonalMovement,
    HeuristicMode iMode)
    : this(iGrid, iDiagonalMovement, iMode)
  {
    this.m_startNode = this.m_searchGrid.GetNodeAt(iStartPos.x, iStartPos.y);
    this.m_endNode = this.m_searchGrid.GetNodeAt(iEndPos.x, iEndPos.y);
    if (this.m_startNode == (Node) null)
      this.m_startNode = new Node(iStartPos.x, iStartPos.y, new bool?(true));
    if (!(this.m_endNode == (Node) null))
      return;
    this.m_endNode = new Node(iEndPos.x, iEndPos.y, new bool?(true));
  }

  public void setGrid(BaseGrid iGrid, GridPos iStartPos, GridPos iEndPos)
  {
    this.m_searchGrid = iGrid;
    this.m_startNode = this.m_searchGrid.GetNodeAt(iStartPos.x, iStartPos.y);
    this.m_endNode = this.m_searchGrid.GetNodeAt(iEndPos.x, iEndPos.y);
    if (this.m_startNode == (Node) null)
      this.m_startNode = new Node(iStartPos.x, iStartPos.y, new bool?(true));
    if (!(this.m_endNode == (Node) null))
      return;
    this.m_endNode = new Node(iEndPos.x, iEndPos.y, new bool?(true));
  }

  public ParamBase(BaseGrid iGrid, DiagonalMovement iDiagonalMovement, HeuristicMode iMode)
  {
    this.SetHeuristic(iMode);
    this.m_searchGrid = iGrid;
    this.DiagonalMovement = iDiagonalMovement;
    this.m_startNode = (Node) null;
    this.m_endNode = (Node) null;
  }

  public ParamBase()
  {
  }

  public ParamBase(ParamBase param)
  {
    this.m_searchGrid = param.m_searchGrid;
    this.DiagonalMovement = param.DiagonalMovement;
    this.m_startNode = param.m_startNode;
    this.m_endNode = param.m_endNode;
  }

  internal abstract void _reset(GridPos iStartPos, GridPos iEndPos, BaseGrid iSearchGrid = null);

  public void Reset(GridPos iStartPos, GridPos iEndPos, BaseGrid iSearchGrid = null)
  {
    this._reset(iStartPos, iEndPos, iSearchGrid);
    this.m_startNode = (Node) null;
    this.m_endNode = (Node) null;
    if (iSearchGrid != null)
      this.m_searchGrid = iSearchGrid;
    this.m_searchGrid.Reset();
    this.m_startNode = this.m_searchGrid.GetNodeAt(iStartPos.x, iStartPos.y);
    this.m_endNode = this.m_searchGrid.GetNodeAt(iEndPos.x, iEndPos.y);
    if (this.m_startNode == (Node) null)
      this.m_startNode = new Node(iStartPos.x, iStartPos.y, new bool?(true));
    if (!(this.m_endNode == (Node) null))
      return;
    this.m_endNode = new Node(iEndPos.x, iEndPos.y, new bool?(true));
  }

  public BaseGrid SearchGrid => this.m_searchGrid;

  public Node StartNode => this.m_startNode;

  public Node EndNode => this.m_endNode;

  public void SetHeuristic(HeuristicMode iMode) => this.m_heuristicMode = iMode;
}
