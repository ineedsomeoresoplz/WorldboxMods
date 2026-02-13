// Decompiled with JetBrains decompiler
// Type: TooltipTraitsRow`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class TooltipTraitsRow<TTrait> : TooltipItemsRow<Image> where TTrait : BaseTrait<TTrait>
{
  protected override void loadItems()
  {
    this.items_pool.clear();
    IReadOnlyCollection<TTrait> traitsHashset = this.traits_hashset;
    if (traitsHashset == null || traitsHashset.Count == 0)
    {
      ((Component) this).gameObject.SetActive(false);
    }
    else
    {
      ((Component) this).gameObject.SetActive(true);
      foreach (TTrait trait in (IEnumerable<TTrait>) this.traits_hashset)
        this.items_pool.getNext().sprite = trait.getSprite();
    }
  }

  protected virtual IReadOnlyCollection<TTrait> traits_hashset
  {
    get => throw new NotImplementedException();
  }
}
