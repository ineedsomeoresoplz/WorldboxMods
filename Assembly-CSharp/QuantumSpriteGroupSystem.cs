// Decompiled with JetBrains decompiler
// Type: QuantumSpriteGroupSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class QuantumSpriteGroupSystem : SpriteGroupSystem<QuantumSprite>
{
  private string _path;
  private QuantumSpriteAsset _asset;
  private QuantumSpriteCacheData _cache_data;

  public void create(QuantumSpriteAsset pAsset)
  {
    this._path = pAsset.id_prefab;
    this._asset = pAsset;
    this.create();
    ((Object) ((Component) this).transform).name = pAsset.id;
  }

  public QuantumSpriteCacheData getCacheData(int pSize)
  {
    if (this._cache_data == null)
      this._cache_data = new QuantumSpriteCacheData(pSize);
    else
      this._cache_data.checkSize(pSize);
    return this._cache_data;
  }

  public override void deactivate(QuantumSprite pObject)
  {
  }

  public override void checkActiveAction(QuantumSprite pObject)
  {
  }

  protected override QuantumSprite createNew()
  {
    QuantumSprite pMark = base.createNew();
    if (this._asset.create_object != null)
      this._asset.create_object(this._asset, pMark);
    return pMark;
  }

  public override void create()
  {
    base.create();
    this.prefab = Resources.Load<QuantumSprite>("civ/" + this._path);
    if (!Object.op_Equality((Object) this.prefab, (Object) null))
      return;
    Debug.LogError((object) ("Missing Prefab " + this._path));
  }
}
