// Decompiled with JetBrains decompiler
// Type: WorkshopMapData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Steamworks.Ugc;
using UnityEngine;

#nullable disable
public class WorkshopMapData
{
  public string main_path;
  public string preview_image_path;
  public Sprite sprite_small_preview;
  public MapMetaData meta_data_map;
  public WorkshopMapMetaData meta_data_workshop;
  public Item workshop_item;

  public static WorkshopMapData currentMapToWorkshop()
  {
    WorkshopMapData workshop = new WorkshopMapData();
    string workshopPath = SaveManager.generateWorkshopPath();
    workshop.meta_data_map = SaveManager.saveWorldToDirectory(workshopPath).getMeta();
    workshop.preview_image_path = workshopPath + "preview.png";
    workshop.main_path = workshopPath;
    return workshop;
  }
}
