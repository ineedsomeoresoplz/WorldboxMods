// Decompiled with JetBrains decompiler
// Type: AuthButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AuthButton : MonoBehaviour
{
  private static string windowId;
  private static List<string> worldnetNoSub = new List<string>()
  {
    "worldnet_main"
  };

  private void Awake() => Object.Destroy((Object) ((Component) this).gameObject);

  public void showWorldNetOwnWorldsWindow()
  {
  }

  public void showWorldNetWorldsListWindow()
  {
  }

  public void showWorldNetMainWindow()
  {
  }

  public void showWorldNetUploadWindow()
  {
  }

  public void showBrowseByTagWindow()
  {
  }

  public void wbbConfirm()
  {
  }

  public void uploadWorldButton()
  {
  }

  public void checkAuthAndOpenWindow()
  {
  }
}
