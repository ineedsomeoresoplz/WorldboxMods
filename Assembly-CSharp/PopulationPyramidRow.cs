// Decompiled with JetBrains decompiler
// Type: PopulationPyramidRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class PopulationPyramidRow : MonoBehaviour
{
  [SerializeField]
  private Image _left_icon;
  [SerializeField]
  private Image _right_icon;
  [SerializeField]
  private PopulationPyramidItem _left_item;
  [SerializeField]
  private PopulationPyramidItem _right_item;
  [SerializeField]
  private Text _text;
  private int _age_group_min;
  private int _age_group_max;

  private void Start()
  {
    // ISSUE: method pointer
    ((UnityEvent) ((Component) this).gameObject.AddOrGetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(animateBars)));
    this.setupTooltip();
  }

  private void setupTooltip()
  {
    TipButton tipButton;
    if (!((Component) this).TryGetComponent<TipButton>(ref tipButton))
      return;
    tipButton.setHoverAction(new TooltipAction(this.showTooltip), false);
  }

  private void showTooltip()
  {
    Tooltip.show((object) ((Component) this).gameObject, "gender_data", new TooltipData()
    {
      custom_data_string = new CustomDataContainer<string>()
      {
        ["age_range"] = $"{this._age_group_min.ToString()} - {this._age_group_max.ToString()}"
      },
      custom_data_int = new CustomDataContainer<int>()
      {
        ["males"] = this._left_item.getCount(),
        ["females"] = this._right_item.getCount()
      }
    });
  }

  private void animateBars()
  {
    this._left_item.animateBar();
    this._right_item.animateBar();
  }

  internal void setAgeGroup(int pAgeGroup, int pAgeGroupMax)
  {
    this._age_group_min = pAgeGroup;
    this._age_group_max = pAgeGroupMax;
    this._text.text = pAgeGroup.ToString();
    float pOpacity = Mathf.Clamp((float) (0.75 + (double) pAgeGroup / 400.0), 0.75f, 1f);
    this._left_item.setOpacity(pOpacity);
    this._right_item.setOpacity(pOpacity);
  }

  internal void setColorTextBasedOnAmount(int pAmount)
  {
    if (pAmount == 0)
      ((Graphic) this._text).color = new Color(1f, 1f, 1f, 0.3f);
    else
      ((Graphic) this._text).color = new Color(1f, 1f, 1f, 1f);
  }

  internal void setMaleCount(int pCount, int pMax)
  {
    this._left_item.setCount(pCount, pMax);
    if (pCount == 0)
      ((Graphic) this._left_icon).color = new Color(1f, 1f, 1f, 0.3f);
    else
      ((Graphic) this._left_icon).color = new Color(1f, 1f, 1f, 1f);
  }

  internal void setFemaleCount(int pCount, int pMax)
  {
    this._right_item.setCount(pCount, pMax);
    if (pCount == 0)
      ((Graphic) this._right_icon).color = new Color(1f, 1f, 1f, 0.3f);
    else
      ((Graphic) this._right_icon).color = new Color(1f, 1f, 1f, 1f);
  }
}
