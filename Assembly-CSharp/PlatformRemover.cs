// Decompiled with JetBrains decompiler
// Type: PlatformRemover
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PlatformRemover : MonoBehaviour
{
  public bool removeOnIOS;
  public bool removeOnAndroid;
  public bool removeOnPC;
  public bool removeOnEditor;
  public bool removeOnGlobalVersion;
  public bool removeOnChineseVersion;
  public bool removeOnNonPremiumVersion;

  private void Awake()
  {
    if (this.removeOnGlobalVersion)
      Object.Destroy((Object) ((Component) this).gameObject);
    else if (this.removeOnEditor && Config.isEditor)
      Object.Destroy((Object) ((Component) this).gameObject);
    else if (this.removeOnPC && Config.isComputer)
      Object.Destroy((Object) ((Component) this).gameObject);
    else if (this.removeOnAndroid && Config.isAndroid)
    {
      Object.Destroy((Object) ((Component) this).gameObject);
    }
    else
    {
      if (!this.removeOnIOS || !Config.isIos)
        return;
      Object.Destroy((Object) ((Component) this).gameObject);
    }
  }
}
