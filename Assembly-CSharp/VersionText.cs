// Decompiled with JetBrains decompiler
// Type: VersionText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class VersionText : MonoBehaviour
{
  internal Text text;

  private void Awake() => this.text = ((Component) this).GetComponent<Text>();

  private void OnEnable()
  {
    if (!Config.game_loaded)
      return;
    ((Component) this.text).GetComponent<LocalizedText>().updateText();
  }

  private void Update()
  {
    if (Object.op_Equality((Object) this.text, (Object) null))
      return;
    this.text.text = this.text.text.Replace("$old_version$", this.oldText(Config.gv));
    this.text.text = this.text.text.Replace("$new_version$", this.newText(VersionCheck.onlineVersion));
  }

  private string oldText(string pText) => $"<color=#FF0000>{pText}</color>";

  private string newText(string pText) => $"<color=#00FF00>{pText}</color>";
}
