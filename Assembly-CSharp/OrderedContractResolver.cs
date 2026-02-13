// Decompiled with JetBrains decompiler
// Type: OrderedContractResolver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
public class OrderedContractResolver : DefaultContractResolver
{
  protected virtual IList<JsonProperty> CreateProperties(
    Type pObjectType,
    MemberSerialization pMemberSerialization)
  {
    List<JsonProperty> properties = new List<JsonProperty>((IEnumerable<JsonProperty>) base.CreateProperties(pObjectType, pMemberSerialization));
    properties.Sort(new Comparison<JsonProperty>(this.orderedPropertySorter));
    return (IList<JsonProperty>) properties;
  }

  private int orderedPropertySorter(JsonProperty p1, JsonProperty p2)
  {
    int? order1 = p1.Order;
    int? order2 = p2.Order;
    if (!(order1.GetValueOrDefault() == order2.GetValueOrDefault() & order1.HasValue == order2.HasValue))
    {
      int? order3 = p1.Order;
      int num1 = order3 ?? int.MaxValue;
      order3 = p2.Order;
      int num2 = order3 ?? int.MaxValue;
      return num1.CompareTo(num2);
    }
    bool flag1 = this.isDelegate(p1.PropertyType);
    bool flag2 = this.isDelegate(p2.PropertyType);
    if (flag1 != flag2)
      return flag1.CompareTo(flag2);
    bool flag3 = this.isCollection(p1.PropertyType);
    bool flag4 = this.isCollection(p2.PropertyType);
    return flag3 != flag4 ? flag3.CompareTo(flag4) : p1.PropertyName.CompareTo(p2.PropertyName);
  }

  private int getBaseTypesCount(Type pType)
  {
    int baseTypesCount = 0;
    for (; pType != (Type) null; pType = pType.BaseType)
      ++baseTypesCount;
    return baseTypesCount;
  }

  private bool isDelegate(Type pType)
  {
    return pType == typeof (Delegate) || pType.IsSubclassOf(typeof (Delegate));
  }

  private bool isCollection(Type pType) => typeof (ICollection).IsAssignableFrom(pType);
}
