// Decompiled with JetBrains decompiler
// Type: AvatarsCombineDataContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class AvatarsCombineDataContainer
{
  private Dictionary<string, AvatarsCombineDataElement> _dict = new Dictionary<string, AvatarsCombineDataElement>();
  private List<AvatarsCombineDataElement> _list = new List<AvatarsCombineDataElement>();

  public void add(string pId, int pAmount)
  {
    AvatarsCombineDataElement combineDataElement = new AvatarsCombineDataElement(this._dict.Count + 1, pAmount);
    this._dict.Add(pId, combineDataElement);
    this._list.Add(combineDataElement);
  }

  public int getListIndex(int pIndex, string pId)
  {
    AvatarsCombineDataElement combineDataElement = this._dict[pId];
    int num1 = combineDataElement.order_index - 1;
    int num2 = 1;
    for (int index = num1 + 1; index < this._list.Count; ++index)
      num2 *= this._list[index].total_amount;
    return pIndex / num2 % combineDataElement.total_amount;
  }

  public void clear()
  {
    this._dict.Clear();
    this._list.Clear();
  }

  public int totalCombinations()
  {
    int num = 1;
    for (int index = 0; index < this._list.Count; ++index)
      num *= this._list[index].total_amount;
    return num;
  }
}
