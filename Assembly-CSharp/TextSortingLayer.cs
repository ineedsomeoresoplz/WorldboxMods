// Decompiled with JetBrains decompiler
// Type: TextSortingLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TextSortingLayer : MonoBehaviour
{
  private MeshRenderer meshRenderer;

  private void Start()
  {
    this.meshRenderer = ((Component) this).gameObject.GetComponent<MeshRenderer>();
    ((Renderer) this.meshRenderer).sortingLayerID = SortingLayer.NameToID("MapOverlay");
    ((Renderer) this.meshRenderer).sortingOrder = 200;
  }
}
