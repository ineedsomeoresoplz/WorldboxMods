// Decompiled with JetBrains decompiler
// Type: MetaObjectCounter`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class MetaObjectCounter<TObject, TData>
  where TObject : MetaObject<TData>
  where TData : MetaObjectData
{
  public TObject meta_object;
  public int amount;

  public MetaObjectCounter(TObject pMetaObject) => this.meta_object = pMetaObject;
}
