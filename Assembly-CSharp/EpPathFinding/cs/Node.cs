// Decompiled with JetBrains decompiler
// Type: EpPathFinding.cs.Node
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
namespace EpPathFinding.cs;

public class Node : IComparable<Node>, IDisposable
{
  public WorldTile tile;
  public readonly int x;
  public readonly int y;
  public float heuristicStartToEndLen;
  public float startToCurNodeLen;
  public float? heuristicCurNodeToEndLen;
  public bool isOpened;
  public bool isClosed;
  public Node parent;

  public Node(int iX, int iY, bool? iWalkable = null)
  {
    this.x = iX;
    this.y = iY;
    this.heuristicStartToEndLen = 0.0f;
    this.startToCurNodeLen = 0.0f;
    this.heuristicCurNodeToEndLen = new float?();
    this.isOpened = false;
    this.isClosed = false;
    this.parent = (Node) null;
  }

  public Node(Node b)
  {
    this.x = b.x;
    this.y = b.y;
    this.heuristicStartToEndLen = b.heuristicStartToEndLen;
    this.startToCurNodeLen = b.startToCurNodeLen;
    this.heuristicCurNodeToEndLen = b.heuristicCurNodeToEndLen;
    this.isOpened = b.isOpened;
    this.isClosed = b.isClosed;
    this.parent = b.parent;
  }

  public void Reset(bool? iWalkable = null)
  {
    this.heuristicStartToEndLen = 0.0f;
    this.startToCurNodeLen = 0.0f;
    this.heuristicCurNodeToEndLen = new float?();
    this.isOpened = false;
    this.isClosed = false;
    this.parent = (Node) null;
  }

  public int CompareTo(Node iObj)
  {
    float num = this.heuristicStartToEndLen - iObj.heuristicStartToEndLen;
    if ((double) num > 0.0)
      return 1;
    return (double) num == 0.0 ? 0 : -1;
  }

  public override int GetHashCode() => this.x ^ this.y;

  public override bool Equals(object obj)
  {
    if (obj == null)
      return false;
    Node node = obj as Node;
    return (object) node != null && this.x == node.x && this.y == node.y;
  }

  public bool Equals(Node p) => (object) p != null && this.x == p.x && this.y == p.y;

  public static bool operator ==(Node a, Node b)
  {
    if ((object) a == (object) b)
      return true;
    return (object) a != null && (object) b != null && a.x == b.x && a.y == b.y;
  }

  public static bool operator !=(Node a, Node b) => !(a == b);

  public void Dispose()
  {
    this.tile = (WorldTile) null;
    this.parent = (Node) null;
  }
}
