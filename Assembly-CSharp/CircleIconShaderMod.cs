// Decompiled with JetBrains decompiler
// Type: CircleIconShaderMod
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CircleIconShaderMod : MonoBehaviour
{
  public Material prefab_radial_fill;
  private Material _instance_material;
  public SpriteRenderer sprite_renderer_with_mat;

  private void Awake()
  {
    this._instance_material = new Material(this.prefab_radial_fill);
    ((Renderer) this.sprite_renderer_with_mat).material = this._instance_material;
  }

  public void setShaderVal(float pVal)
  {
    if (Object.op_Equality((Object) this.sprite_renderer_with_mat, (Object) null))
      return;
    this._instance_material.SetFloat("_FillAmount", Mathf.PingPong(pVal, 1f));
  }
}
