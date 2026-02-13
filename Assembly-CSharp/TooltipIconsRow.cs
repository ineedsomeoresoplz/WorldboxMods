// Decompiled with JetBrains decompiler
// Type: TooltipIconsRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class TooltipIconsRow : TooltipItemsRow<Image>
{
  private List<(Sprite, Color)> _icons = new List<(Sprite, Color)>();

  protected override void loadItems()
  {
    this.items_pool.clear();
    if (this._icons.Count == 0)
    {
      ((Component) this).gameObject.SetActive(false);
    }
    else
    {
      ((Component) this).gameObject.SetActive(true);
      foreach ((Sprite, Color) icon in this._icons)
      {
        Image next = this.items_pool.getNext();
        next.sprite = icon.Item1;
        ((Graphic) next).color = icon.Item2;
      }
      this.clearIcons();
    }
  }

  public void addIcon(Sprite pIcon, string pColor = "#FFFFFF")
  {
    Color color = Toolbox.makeColor(pColor);
    this._icons.Add((pIcon, color));
  }

  private void clearIcons() => this._icons.Clear();
}
