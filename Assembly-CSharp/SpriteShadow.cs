// Decompiled with JetBrains decompiler
// Type: SpriteShadow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SpriteShadow : MonoBehaviour
{
  public Vector2 offset = new Vector2(-3f, 3f);
  internal int z_height;
  private SpriteRenderer sprRndCaster;
  private SpriteRenderer sprRndShadow;
  private Transform transCaster;
  private Transform transShadow;
  public Color shadowColor;
  private BaseMapObject baseMapObject;

  private void Start()
  {
    this.baseMapObject = ((Component) this).GetComponent<BaseMapObject>();
    this.transCaster = ((Component) this).transform;
    this.transShadow = new GameObject().transform;
    this.transShadow.parent = this.transCaster;
    ((Object) ((Component) this.transShadow).gameObject).name = "Shadow";
    this.transShadow.localRotation = Quaternion.identity;
    this.transShadow.localScale = new Vector3(1f, 0.5f);
    this.sprRndCaster = ((Component) this).GetComponent<SpriteRenderer>();
    this.sprRndShadow = ((Component) this.transShadow).gameObject.AddComponent<SpriteRenderer>();
    ((Renderer) this.sprRndShadow).sharedMaterial = LibraryMaterials.instance.mat_world_object;
    this.sprRndShadow.color = this.shadowColor;
    ((Renderer) this.sprRndShadow).sortingLayerName = ((Renderer) this.sprRndCaster).sortingLayerName;
    ((Renderer) this.sprRndShadow).sortingOrder = ((Renderer) this.sprRndCaster).sortingOrder - 1;
  }

  private void LateUpdate()
  {
    this.transShadow.position = Vector2.op_Implicit(new Vector2(this.transCaster.position.x + this.offset.x, this.transCaster.position.y + this.offset.y));
    Color shadowColor = this.shadowColor;
    shadowColor.a = this.sprRndCaster.color.a * 0.5f;
    this.sprRndShadow.color = shadowColor;
    this.sprRndShadow.sprite = this.sprRndCaster.sprite;
    this.sprRndShadow.flipX = this.sprRndCaster.flipX;
  }
}
