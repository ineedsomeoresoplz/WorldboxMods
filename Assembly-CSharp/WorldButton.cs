// Decompiled with JetBrains decompiler
// Type: WorldButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class WorldButton : MonoBehaviour
{
  public static WorldButton active_buttons;
  public WorldButton mainButtonObject;
  public WorldButton[] lesser_buttons;
  private Vector3 initial_pos;

  private void Start()
  {
    this.initial_pos = ((Component) this).transform.localPosition;
    if (!Object.op_Inequality((Object) this.mainButtonObject, (Object) null))
      return;
    this.hide();
  }

  public void onClickMain()
  {
    if (Object.op_Inequality((Object) WorldButton.active_buttons, (Object) null) && Object.op_Inequality((Object) WorldButton.active_buttons, (Object) this))
    {
      WorldButton.active_buttons.hideChildren();
      WorldButton.active_buttons = (WorldButton) null;
    }
    if (!((Component) this.lesser_buttons[0]).gameObject.activeSelf)
    {
      foreach (WorldButton lesserButton in this.lesser_buttons)
        lesserButton.activate();
      WorldButton.active_buttons = this;
    }
    else
      this.hideChildren();
  }

  public void hideChildren()
  {
    foreach (WorldButton lesserButton in this.lesser_buttons)
      lesserButton.hide();
  }

  public void hide()
  {
    ((Component) this).gameObject.SetActive(false);
    ((Component) this).transform.localPosition = ((Component) this.mainButtonObject).transform.position;
  }

  public void activate()
  {
    ((Component) this).gameObject.SetActive(true);
    ((Component) this).transform.localPosition = this.initial_pos;
  }
}
