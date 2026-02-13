// Decompiled with JetBrains decompiler
// Type: TabbedWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TabbedWindow : MonoBehaviour
{
  protected ScrollWindow scroll_window;

  protected WindowMetaTabButtonsContainer tabs => this.scroll_window.tabs;

  protected void Awake()
  {
    this.scroll_window = ((Component) ((Component) this).transform).GetComponentInParent<ScrollWindow>();
    this.create();
  }

  protected virtual void create() => this.tabs.init();

  internal virtual bool checkCancelWindow() => false;

  public void showTab(WindowMetaTab pTab) => this.tabs.showTab(pTab);
}
