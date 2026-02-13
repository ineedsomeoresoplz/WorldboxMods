// Decompiled with JetBrains decompiler
// Type: RateUsRemover
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RateUsRemover : MonoBehaviour
{
  public void clickedRateUs()
  {
    PlayerConfig.instance.data.lastRateID = 12;
    ((Component) this).gameObject.SetActive(false);
    PlayerConfig.saveData();
  }
}
