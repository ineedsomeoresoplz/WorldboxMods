// Decompiled with JetBrains decompiler
// Type: ComponentListSapient`4
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ComponentListSapient<TListElement, TMetaObject, TData, TComponent> : 
  ComponentListBase<TListElement, TMetaObject, TData, TComponent>,
  ISapientListComponent
  where TListElement : WindowListElementBase<TMetaObject, TData>
  where TMetaObject : CoreSystemObject<TData>
  where TData : BaseSystemData
  where TComponent : ComponentListBase<TListElement, TMetaObject, TData, TComponent>
{
  [SerializeField]
  private Text _sapient_counter;
  [SerializeField]
  private Text _non_sapient_counter;
  private SapientListFilter _filter;

  protected override void show()
  {
    if (!Config.game_loaded)
      return;
    base.show();
    if (Object.op_Inequality((Object) this._sapient_counter, (Object) null))
      this._sapient_counter.text = this.latest_counted.ToString();
    if (!Object.op_Inequality((Object) this._non_sapient_counter, (Object) null))
      return;
    this._non_sapient_counter.text = this.latest_counted.ToString();
  }

  protected override IEnumerable<TMetaObject> getFiltered(IEnumerable<TMetaObject> pList)
  {
    switch (this._filter)
    {
      case SapientListFilter.Default:
        foreach (TMetaObject metaObject in base.getFiltered(pList))
          yield return metaObject;
        break;
      case SapientListFilter.Sapient:
        foreach (TMetaObject p in pList)
        {
          ISapient sapient = (ISapient) (object) p;
          if (sapient.isSapient())
            yield return (TMetaObject) sapient;
        }
        break;
      case SapientListFilter.NonSapient:
        foreach (TMetaObject p in pList)
        {
          ISapient sapient = (ISapient) (object) p;
          if (!sapient.isSapient())
            yield return (TMetaObject) sapient;
        }
        break;
    }
  }

  public void setShowSapientOnly() => this._filter = SapientListFilter.Sapient;

  public void setShowNonSapientOnly() => this._filter = SapientListFilter.NonSapient;

  public override void setDefault() => this._filter = SapientListFilter.Default;

  public void setSapientCounter(Text pCounter) => this._sapient_counter = pCounter;

  public void setNonSapientCounter(Text pCounter) => this._non_sapient_counter = pCounter;
}
