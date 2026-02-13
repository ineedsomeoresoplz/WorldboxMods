// Decompiled with JetBrains decompiler
// Type: ContainerItemColor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class ContainerItemColor
{
  public string color_id;
  public Color color;
  private Material material;
  private string path_material;

  public ContainerItemColor(string pID, string pMaterialPath)
  {
    this.color = Toolbox.makeColor(pID);
    this.color_id = pID;
    this.path_material = pMaterialPath;
  }

  public Material getMaterial()
  {
    if (string.IsNullOrEmpty(this.path_material))
      return (Material) null;
    this.material = Resources.Load<Material>(this.path_material);
    return this.material;
  }
}
