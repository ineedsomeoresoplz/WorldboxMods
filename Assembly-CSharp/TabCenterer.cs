// Decompiled with JetBrains decompiler
// Type: TabCenterer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TabCenterer : MonoBehaviour
{
  [SerializeField]
  private PowersTab _powers_tab;
  public const bool ENABLE_CENTERING = false;

  private void Update() => this.centerTab();

  public void centerTab()
  {
  }
}
