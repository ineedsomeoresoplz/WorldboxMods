// Decompiled with JetBrains decompiler
// Type: PatchLogElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PatchLogElement : MonoBehaviour
{
  public PatchLogTitle title;
  public Text date;
  public Text date_ago;
  public Image background;
  public GameObject texts;
  public bool _folded;

  public void fold()
  {
    this._folded = true;
    this.title.setFolded();
    this.texts.gameObject.SetActive(false);
  }

  public void unfold()
  {
    this._folded = false;
    this.title.setUnfolded();
    this.texts.gameObject.SetActive(true);
  }

  public void toggleState()
  {
    if (this._folded)
      this.unfold();
    else
      this.fold();
  }
}
