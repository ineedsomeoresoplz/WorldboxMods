// Decompiled with JetBrains decompiler
// Type: FieldInfoListItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class FieldInfoListItem
{
  public string field_name;
  public string field_value;
  public Dictionary<string, string> collection_data;

  public FieldInfoListItem(string pName, string pValue, Dictionary<string, string> pCollectionData = null)
  {
    this.field_name = pName;
    this.field_value = pValue;
    this.collection_data = pCollectionData;
  }
}
