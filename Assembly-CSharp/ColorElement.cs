// Decompiled with JetBrains decompiler
// Type: ColorElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class ColorElement : MonoBehaviour
{
  public Button button;
  public Image selection;
  public Image outer;
  public Image inner;
  public int index;
  public MetaCustomizationAsset asset;

  public void setColor(Color pOuter, Color pInner)
  {
    ((Graphic) this.outer).color = pOuter;
    ((Graphic) this.inner).color = pInner;
  }

  public void setSelected(bool pSelected) => ((Behaviour) this.selection).enabled = pSelected;

  public void setAction(UnityAction pAction)
  {
    ((UnityEvent) this.button.onClick).AddListener(pAction);
  }

  public void showTooltip()
  {
    Tooltip.show((object) ((Component) this).gameObject, "color_counter", new TooltipData()
    {
      tip_name = this.asset.color_locale,
      custom_data_int = new CustomDataContainer<int>()
      {
        ["color_count"] = this.asset.color_count(),
        ["color_current"] = this.index + 1
      }
    });
  }
}
