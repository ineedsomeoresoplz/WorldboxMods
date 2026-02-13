// Decompiled with JetBrains decompiler
// Type: TooltipItemsRow`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class TooltipItemsRow<TComponent> : MonoBehaviour where TComponent : MonoBehaviour
{
  public Transform items_parent;
  public TComponent item;
  protected Tooltip tooltip;
  protected TooltipData tooltip_data;
  protected ObjectPoolGenericMono<TComponent> items_pool;

  public void init(Tooltip pTooltip, TooltipData pData)
  {
    this.tooltip = pTooltip;
    this.tooltip_data = pData;
    if (this.items_pool == null)
      this.items_pool = new ObjectPoolGenericMono<TComponent>(this.item, this.items_parent);
    this.loadItems();
  }

  protected virtual void loadItems() => throw new NotImplementedException();
}
