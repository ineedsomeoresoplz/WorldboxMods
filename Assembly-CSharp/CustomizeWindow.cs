// Decompiled with JetBrains decompiler
// Type: CustomizeWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CustomizeWindow : MonoBehaviour
{
  public ColorElement color_element_prefab;
  public MetaType meta_type;
  private bool _created;

  private void OnEnable()
  {
    if (this._created)
      return;
    this._created = true;
    AssetManager.meta_customization_library.getAsset(this.meta_type).customize_component(((Component) this).gameObject);
  }
}
