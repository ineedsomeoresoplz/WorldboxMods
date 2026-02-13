// Decompiled with JetBrains decompiler
// Type: EpPathFinding.cs.NodePool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace EpPathFinding.cs;

public class NodePool
{
  protected Dictionary<GridPos, Node> m_nodes;

  public NodePool() => this.m_nodes = new Dictionary<GridPos, Node>();

  public Dictionary<GridPos, Node> Nodes => this.m_nodes;

  public Node GetNode(int iX, int iY) => this.GetNode(new GridPos(iX, iY));

  public Node GetNode(GridPos iPos)
  {
    Node node = (Node) null;
    this.m_nodes.TryGetValue(iPos, out node);
    return node;
  }

  public Node SetNode(int iX, int iY, bool? iWalkable = null)
  {
    return this.SetNode(new GridPos(iX, iY), iWalkable);
  }

  public Node SetNode(GridPos iPos, bool? iWalkable = null)
  {
    if (iWalkable.HasValue)
    {
      if (iWalkable.Value)
      {
        Node node1 = (Node) null;
        if (this.m_nodes.TryGetValue(iPos, out node1))
          return node1;
        Node node2 = new Node(iPos.x, iPos.y, iWalkable);
        this.m_nodes.Add(iPos, node2);
        return node2;
      }
      this.removeNode(iPos);
      return (Node) null;
    }
    Node node = new Node(iPos.x, iPos.y, new bool?(true));
    this.m_nodes.Add(iPos, node);
    return node;
  }

  protected void removeNode(int iX, int iY) => this.removeNode(new GridPos(iX, iY));

  protected void removeNode(GridPos iPos)
  {
    if (!this.m_nodes.ContainsKey(iPos))
      return;
    this.m_nodes.Remove(iPos);
  }
}
