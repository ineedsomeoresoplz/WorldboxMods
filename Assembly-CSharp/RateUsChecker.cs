// Decompiled with JetBrains decompiler
// Type: RateUsChecker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RateUsChecker : MonoBehaviour
{
  public GameObject rateUs;
  public GameObject updateAvailable;

  private void OnEnable()
  {
    if (!Config.game_loaded || !Object.op_Inequality((Object) this.rateUs, (Object) null) || !Object.op_Inequality((Object) this.rateUs.gameObject, (Object) null))
      return;
    this.rateUs.gameObject.SetActive(false);
  }

  private void Update()
  {
    if (VersionCheck.isOutdated())
    {
      if (Object.op_Inequality((Object) this.rateUs, (Object) null) && Object.op_Inequality((Object) this.rateUs.gameObject, (Object) null))
        this.rateUs.gameObject.SetActive(false);
      if (!Object.op_Inequality((Object) this.updateAvailable, (Object) null) || !Object.op_Inequality((Object) this.updateAvailable.gameObject, (Object) null))
        return;
      this.updateAvailable.gameObject.SetActive(true);
    }
    else
    {
      if (!Object.op_Inequality((Object) this.updateAvailable, (Object) null) || !Object.op_Inequality((Object) this.updateAvailable.gameObject, (Object) null))
        return;
      this.updateAvailable.gameObject.SetActive(false);
    }
  }
}
