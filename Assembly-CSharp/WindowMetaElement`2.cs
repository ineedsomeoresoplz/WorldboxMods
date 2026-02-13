// Decompiled with JetBrains decompiler
// Type: WindowMetaElement`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class WindowMetaElement<TMetaObject, TData> : WindowMetaElementBase
  where TMetaObject : CoreSystemObject<TData>
  where TData : BaseSystemData
{
  protected TMetaObject meta_object;
  protected WindowMetaGeneric<TMetaObject, TData> window;

  protected override void Awake()
  {
    this.window = ((Component) this).GetComponentInParent<WindowMetaGeneric<TMetaObject, TData>>();
    base.Awake();
  }

  protected override void OnEnable()
  {
    this.meta_object = this.window.getMetaObject();
    base.OnEnable();
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    this.meta_object = default (TMetaObject);
  }

  public override bool checkRefreshWindow()
  {
    return this.meta_object.isRekt() || base.checkRefreshWindow();
  }
}
