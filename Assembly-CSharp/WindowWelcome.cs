// Decompiled with JetBrains decompiler
// Type: WindowWelcome
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WindowWelcome : MonoBehaviour
{
  [SerializeField]
  private Transform _content;
  private LocalizedText top_text;
  private LocalizedText text_tip;

  public void Awake()
  {
    this.text_tip = !Config.isMobile ? ((Component) ((Component) this).transform.FindRecursive("Text PC")).GetComponent<LocalizedText>() : ((Component) ((Component) this).transform.FindRecursive("Text Mobile")).GetComponent<LocalizedText>();
    this.top_text = ((Component) ((Component) this).transform.FindRecursive("Text Main")).GetComponent<LocalizedText>();
  }

  public void nextTip() => this.text_tip.setKeyAndUpdate(LoadingScreen.getTipID());

  private void Update()
  {
    if (Config.MODDED || Config.experimental_mode)
    {
      this.top_text.setKeyAndUpdate("mods_warning");
      ((Graphic) ((Component) this.top_text).GetComponent<Text>()).color = Toolbox.color_red;
    }
    if (Time.frameCount % 30 != 0)
      return;
    this.tldrCheck();
  }

  private void tldrCheck()
  {
    if (((Component) this._content).GetComponentsInChildren<UiCreature>().Length > 0)
      return;
    AchievementLibrary.tldr.check();
  }
}
