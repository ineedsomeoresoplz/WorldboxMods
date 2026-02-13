// Decompiled with JetBrains decompiler
// Type: WorkshopOpenSteamWorkshop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class WorkshopOpenSteamWorkshop : MonoBehaviour
{
  public static string fileID;

  public void playWorkShopMap()
  {
    Application.OpenURL($"steam://url/SteamWorkshopPage/{(ValueType) 1206560U}");
  }

  public void openWorkShopAgreement()
  {
    Application.OpenURL("steam://url/CommunityFilePage/" + WorkshopOpenSteamWorkshop.fileID);
    ((Component) this).gameObject.SetActive(false);
  }
}
