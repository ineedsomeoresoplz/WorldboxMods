// Decompiled with JetBrains decompiler
// Type: GenericTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class GenericTest
{
  private List<object> list = new List<object>();

  public T get<T>(int pI) where T : class => this.list[pI] as T;

  public void Add(object pObject) => this.list.Add(pObject);
}
