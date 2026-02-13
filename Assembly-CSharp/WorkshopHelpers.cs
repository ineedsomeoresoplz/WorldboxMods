// Decompiled with JetBrains decompiler
// Type: WorkshopHelpers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Steamworks.Ugc;
using UnityEngine;

#nullable disable
public class WorkshopHelpers : MonoBehaviour
{
  public const string color_own_map = "#3DDEFF";
  public const string color_other_map = "#FF9B1C";

  public void openCurrentMapInWorkshop()
  {
    Application.OpenURL("steam://url/CommunityFilePage/" + ((Item) ref SaveManager.currentWorkshopMapData.workshop_item).Id.ToString());
  }

  public void openUploadWorld()
  {
    SaveManager.clearCurrentSelectedWorld();
    ScrollWindow.showWindow("steam_workshop_upload_world");
  }

  public void openBrowseWorlds()
  {
    SaveManager.clearCurrentSelectedWorld();
    ScrollWindow.showWindow("steam_workshop_browse");
  }
}
