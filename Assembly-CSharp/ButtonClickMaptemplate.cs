// Decompiled with JetBrains decompiler
// Type: ButtonClickMaptemplate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class ButtonClickMaptemplate : MonoBehaviour
{
  private Button _button;
  private MapGenTemplate _template;

  private void Awake()
  {
    string name = ((Object) ((Component) this).transform).name;
    this._button = ((Component) this).GetComponent<Button>();
    // ISSUE: method pointer
    ((UnityEvent) this._button.onClick).AddListener(new UnityAction((object) this, __methodptr(click)));
    if (Input.mousePresent)
    {
      // ISSUE: method pointer
      this._button.OnHover(new UnityAction((object) this, __methodptr(\u003CAwake\u003Eb__2_0)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      this._button.OnHoverOut(ButtonClickMaptemplate.\u003C\u003Ec.\u003C\u003E9__2_1 ?? (ButtonClickMaptemplate.\u003C\u003Ec.\u003C\u003E9__2_1 = new UnityAction((object) ButtonClickMaptemplate.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CAwake\u003Eb__2_1))));
    }
    this._template = AssetManager.map_gen_templates.get(name);
    ((Component) ((Component) this).transform.Find("preview_icon")).GetComponent<Image>().sprite = SpriteTextureLoader.getSprite(this._template.path_icon);
  }

  private void showTooltip()
  {
    Tooltip.show((object) ((Component) this._button).gameObject, "normal", new TooltipData()
    {
      tip_name = this._template.getLocaleID(),
      tip_description = this._template.getDescriptionID()
    });
  }

  public void click()
  {
    if (!InputHelpers.mouseSupported)
    {
      if (!Tooltip.isShowingFor((object) ((Component) this._button).gameObject))
      {
        this.showTooltip();
        return;
      }
      Tooltip.hideTooltipNow();
    }
    Config.current_map_template = this._template.id;
    ScrollWindow.showWindow("new_world_templates_2");
  }
}
