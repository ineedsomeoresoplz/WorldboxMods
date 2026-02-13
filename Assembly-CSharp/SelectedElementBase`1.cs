// Decompiled with JetBrains decompiler
// Type: SelectedElementBase`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SelectedElementBase<TComponent> : MonoBehaviour where TComponent : Component
{
  [SerializeField]
  protected Transform _grid;
  protected ObjectPoolGenericMono<TComponent> _pool;

  protected void clear() => this._pool?.clear();

  protected virtual void refresh(NanoObject pNano)
  {
  }

  private void OnDisable() => this.clear();
}
