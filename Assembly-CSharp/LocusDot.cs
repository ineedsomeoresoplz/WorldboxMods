// Decompiled with JetBrains decompiler
// Type: LocusDot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class LocusDot : MonoBehaviour
{
  [SerializeField]
  private Image _status;

  internal Image status => this._status;

  public void colorDot(Color pColor) => ((Graphic) this._status).color = pColor;

  public void colorDot(char pGeneticCode) => this.colorDot(NucleobaseHelper.getColor(pGeneticCode));
}
