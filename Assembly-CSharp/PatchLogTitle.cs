// Decompiled with JetBrains decompiler
// Type: PatchLogTitle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PatchLogTitle : MonoBehaviour
{
  [SerializeField]
  private Image _background;
  [SerializeField]
  private Sprite _bg_folded;
  [SerializeField]
  private Sprite _bg_unfolded;
  public Image icon_left;
  public Image icon_right;
  public Text title;

  public void setUnfolded() => this._background.sprite = this._bg_unfolded;

  public void setFolded() => this._background.sprite = this._bg_folded;
}
