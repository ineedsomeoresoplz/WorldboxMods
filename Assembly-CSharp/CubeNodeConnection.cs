// Decompiled with JetBrains decompiler
// Type: CubeNodeConnection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CubeNodeConnection : MonoBehaviour
{
  [SerializeField]
  private Sprite _connection_inner;
  [SerializeField]
  private Sprite _connection_outer;
  public Image image;
  internal CubeNode node_1;
  internal CubeNode node_2;
  internal bool inner_cube;
  internal float mod_light = 1f;

  public void update()
  {
    this.mod_light -= Time.deltaTime * 2f;
    this.mod_light = Mathf.Max(0.0f, this.mod_light);
  }

  public void setConnection(bool pInner)
  {
    this.inner_cube = pInner;
    if (pInner)
      this.image.sprite = this._connection_inner;
    else
      this.image.sprite = this._connection_outer;
  }

  public void clear()
  {
    this.node_1 = (CubeNode) null;
    this.node_2 = (CubeNode) null;
    this.inner_cube = false;
  }
}
