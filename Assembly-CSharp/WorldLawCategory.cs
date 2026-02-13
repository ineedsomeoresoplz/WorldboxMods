// Decompiled with JetBrains decompiler
// Type: WorldLawCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using LayoutGroupExt;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WorldLawCategory : MonoBehaviour
{
  [SerializeField]
  private Text _title;
  [SerializeField]
  private Text _selected_counter;
  public GridLayoutGroupExtended grid;
  private WorldLawGroupAsset _asset;
  private HashSet<WorldLawElement> _laws_list = new HashSet<WorldLawElement>();

  public void init(WorldLawGroupAsset pGroupAsset)
  {
    this._asset = pGroupAsset;
    ((Graphic) this._title).color = this._asset.getColor();
    ((Component) this._title).GetComponent<LocalizedText>().setKeyAndUpdate(this._asset.getLocaleID());
  }

  public void addElement(WorldLawElement pElement)
  {
    this._laws_list.Add(pElement);
    pElement.setSelectionColor(ColorStyleLibrary.m.getSelectorColor());
  }

  public void updateCounter()
  {
    int num = 0;
    foreach (WorldLawElement laws in this._laws_list)
    {
      if (laws.isLawEnabled())
        ++num;
    }
    this._selected_counter.text = $"{num} / {this._laws_list.Count}";
  }

  public void updateButtons()
  {
    foreach (WorldLawElement laws in this._laws_list)
      laws.updateStatus();
  }
}
