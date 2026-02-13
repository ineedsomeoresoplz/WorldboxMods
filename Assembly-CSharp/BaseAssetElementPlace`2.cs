// Decompiled with JetBrains decompiler
// Type: BaseAssetElementPlace`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class BaseAssetElementPlace<TAsset, TAssetElement> : MonoBehaviour
  where TAsset : Asset
  where TAssetElement : BaseDebugAssetElement<TAsset>
{
  public GameObject game_object_cache;
  public RectTransform rect_transform;
  public LayoutElement layout_element;
  public bool has_element;
  public TAssetElement element;
  public GameObject element_game_object_cache;
  public bool allowed_for_search = true;

  public void clear()
  {
    if (!this.has_element)
      return;
    LayoutElement layoutElement = this.layout_element;
    Rect rect = this.element.rect_transform.rect;
    double height = (double) ((Rect) ref rect).height;
    layoutElement.minHeight = (float) height;
    Object.Destroy((Object) this.element_game_object_cache);
    this.element_game_object_cache = (GameObject) null;
    this.element = default (TAssetElement);
    this.has_element = false;
  }

  public void setData(TAsset pAsset, TAssetElement pPrefab)
  {
    if (this.has_element)
      this.clear();
    this.layout_element.minHeight = -1f;
    TAssetElement assetElement = Object.Instantiate<TAssetElement>(pPrefab, (Transform) this.rect_transform);
    assetElement.setData(pAsset);
    ((Transform) assetElement.rect_transform).localScale = Vector3.one;
    this.element = assetElement;
    this.element_game_object_cache = ((Component) (object) assetElement).gameObject;
    this.has_element = true;
  }
}
