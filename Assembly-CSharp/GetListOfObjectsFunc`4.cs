// Decompiled with JetBrains decompiler
// Type: GetListOfObjectsFunc`4
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public delegate IEnumerable<TMetaObject> GetListOfObjectsFunc<TListElement, TMetaObject, TData, TComponent>(
  TComponent pComponent)
  where TListElement : WindowListElementBase<TMetaObject, TData>
  where TMetaObject : CoreSystemObject<TData>
  where TData : BaseSystemData
  where TComponent : ComponentListBase<TListElement, TMetaObject, TData, TComponent>;
