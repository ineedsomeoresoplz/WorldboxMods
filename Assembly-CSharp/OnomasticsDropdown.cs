// Decompiled with JetBrains decompiler
// Type: OnomasticsDropdown
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class OnomasticsDropdown : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
  [SerializeField]
  private OnomasticsTab _onomastics_tab;
  private Button _button;
  private Dropdown _dropdown;
  private List<string> _options;
  internal static string current_template;
  internal static int current_template_index;

  private void Start()
  {
    this._dropdown = ((Component) this).GetComponent<Dropdown>();
    this.createDropdownOptions();
    // ISSUE: method pointer
    ((UnityEvent<int>) this._dropdown.onValueChanged).AddListener(new UnityAction<int>((object) this, __methodptr(dropdownValueChanged)));
  }

  private void createDropdownOptions()
  {
    this._dropdown.ClearOptions();
    this._options = new List<string>();
    this._options.Add("");
    foreach (NameGeneratorAsset nameGeneratorAsset in AssetManager.name_generator.list)
    {
      if (nameGeneratorAsset.onomastics_templates.Count < 1)
        this._options.Add($"<color=red>{nameGeneratorAsset.id}</color>");
      else if (nameGeneratorAsset.onomastics_templates.Count < 2)
      {
        this._options.Add(nameGeneratorAsset.id);
      }
      else
      {
        for (int index = 0; index < nameGeneratorAsset.onomastics_templates.Count; ++index)
          this._options.Add($"{nameGeneratorAsset.id}#{index.ToString()}");
      }
    }
    this._options.Sort((Comparison<string>) ((a, b) => Toolbox.removeRichTextTags(a).CompareTo(Toolbox.removeRichTextTags(b))));
    this._dropdown.AddOptions(this._options);
  }

  private void dropdownValueChanged(int pOption)
  {
    if (pOption < 0 || pOption >= this._dropdown.options.Count)
      return;
    string text = this._dropdown.options[pOption].text;
    if (string.IsNullOrEmpty(text))
      return;
    string pID = Toolbox.removeRichTextTags(text);
    int result = 0;
    if (pID.Contains('#'))
    {
      string[] strArray = pID.Split('#', StringSplitOptions.None);
      pID = strArray[0];
      if (!int.TryParse(strArray[1], out result))
        return;
    }
    NameGeneratorAsset nameGeneratorAsset = AssetManager.name_generator.get(pID);
    if (nameGeneratorAsset == null)
      return;
    OnomasticsDropdown.current_template = pID;
    OnomasticsDropdown.current_template_index = result;
    string pTemplate = (string) null;
    if (result >= 0 && result < nameGeneratorAsset.onomastics_templates.Count)
      pTemplate = nameGeneratorAsset.onomastics_templates[result];
    this._onomastics_tab.loadTemplate(pTemplate);
  }

  public void OnPointerClick(PointerEventData pEventData)
  {
    if (Object.op_Equality((Object) ((BaseEventData) pEventData).selectedObject, (Object) null) || Object.op_Inequality((Object) ((BaseEventData) pEventData).selectedObject.GetComponentInChildren<Scrollbar>(), (Object) null) || !((UIBehaviour) this._dropdown).IsActive() || !((Selectable) this._dropdown).IsInteractable())
      return;
    Scrollbar verticalScrollbar = ((Component) this).gameObject.GetComponentInChildren<ScrollRect>()?.verticalScrollbar;
    if (this._options.Count <= 1 || !Object.op_Inequality((Object) verticalScrollbar, (Object) null))
      return;
    if (verticalScrollbar.direction == 3)
      verticalScrollbar.value = Mathf.Max(1f / 1000f, (float) this._dropdown.value / (float) (this._options.Count - 1));
    else
      verticalScrollbar.value = Mathf.Max(1f / 1000f, (float) (1.0 - (double) this._dropdown.value / (double) (this._options.Count - 1)));
  }
}
