// Decompiled with JetBrains decompiler
// Type: WorkshopMaps
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using RSG;
using Steamworks;
using Steamworks.Data;
using Steamworks.Ugc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

#nullable disable
public static class WorkshopMaps
{
  internal static WorkshopUploadProgress uploadProgressTracker = new WorkshopUploadProgress();
  internal static float uploadProgress = 0.0f;
  public static PublishedFileId uploaded_file_id;
  internal static List<Item> foundMaps = new List<Item>();

  public static bool workshopAvailable()
  {
    return SteamSDK.steamInitialized != null && SteamSDK.steamInitialized.CurState == 2;
  }

  internal static Promise uploadMap()
  {
    Promise promise = new Promise();
    WorkshopMaps.uploadProgress = 0.0f;
    WorkshopMapData workshop = WorkshopMapData.currentMapToWorkshop();
    SaveManager.currentWorkshopMapData = workshop;
    MapMetaData metaDataMap1 = workshop.meta_data_map;
    if (SaveManager.currentWorkshopMapData == null)
    {
      promise.Reject(new Exception("Missing world data"));
      return promise;
    }
    if (!MapSizeLibrary.isSizeValid(metaDataMap1.width))
    {
      promise.Reject(new Exception("Not a valid world size!"));
      return promise;
    }
    if (metaDataMap1.width != metaDataMap1.height)
    {
      promise.Reject(new Exception("Not a square world!"));
      return promise;
    }
    MapMetaData metaDataMap2 = workshop.meta_data_map;
    string name = metaDataMap2.mapStats.name;
    string description = metaDataMap2.mapStats.description;
    if (string.IsNullOrWhiteSpace(name))
    {
      promise.Reject(new Exception("Give your world a name!"));
      return promise;
    }
    if (string.IsNullOrWhiteSpace(description))
    {
      promise.Reject(new Exception("Give your world a description!"));
      return promise;
    }
    string mainPath = workshop.main_path;
    string previewImagePath = workshop.preview_image_path;
    Editor newCommunityFile = Editor.NewCommunityFile;
    Editor editor1 = ((Editor) ref newCommunityFile).WithTag("World");
    if (!string.IsNullOrWhiteSpace(name))
      editor1 = ((Editor) ref editor1).WithTitle(name);
    if (!string.IsNullOrWhiteSpace(description))
      editor1 = ((Editor) ref editor1).WithDescription(description);
    if (!string.IsNullOrWhiteSpace(previewImagePath))
      editor1 = ((Editor) ref editor1).WithPreviewFile(previewImagePath);
    if (!string.IsNullOrWhiteSpace(mainPath))
      editor1 = ((Editor) ref editor1).WithContent(mainPath);
    Editor editor2 = ((Editor) ref editor1).WithFriendsOnlyVisibility();
    WorkshopMaps.uploadProgressTracker = new WorkshopUploadProgress();
    ((Editor) ref editor2).SubmitAsync((IProgress<float>) WorkshopMaps.uploadProgressTracker).ContinueWith((Action<Task<PublishResult>>) (taskResult =>
    {
      if (taskResult.Status == TaskStatus.RanToCompletion)
      {
        PublishResult result = taskResult.Result;
        if (!((PublishResult) ref result).Success)
          Debug.LogError((object) "Error when uploading Workshop world");
        if (result.NeedsWorkshopAgreement)
        {
          Debug.Log((object) "w: Needs Workshop Agreement");
          WorkshopUploadingWorldWindow.needsWorkshopAgreement = true;
          WorkshopOpenSteamWorkshop.fileID = result.FileId.ToString();
        }
        if (result.Result != 1)
        {
          Debug.LogError((object) result.Result);
          promise.Reject(new Exception("Something went wrong: " + result.Result.ToString()));
        }
        else
        {
          WorkshopMaps.uploaded_file_id = result.FileId;
          ++World.world.game_stats.data.workshopUploads;
          WorkshopAchievements.checkAchievements();
          promise.Resolve();
        }
      }
      else
        promise.Reject(taskResult.Exception.GetBaseException());
    }), TaskScheduler.FromCurrentSynchronizationContext());
    return promise;
  }

  internal static async Task<List<Item>> listWorkshopMaps(bool pOrder = false, bool pByFriends = false)
  {
    Query itemsReadyToUse = Query.ItemsReadyToUse;
    Query query = ((Query) ref itemsReadyToUse).WhereUserSubscribed(new SteamId());
    Query q = ((Query) ref query).WithTag("World");
    if (pByFriends)
      q = ((Query) ref q).CreatedByFriends();
    q = !pOrder ? ((Query) ref q).SortByCreationDateAsc() : ((Query) ref q).SortByCreationDate();
    WorkshopMaps.foundMaps.Clear();
    int num = 1;
    int totalFound = 0;
    int tPage = 1;
    while (num > totalFound)
    {
      ResultPage? pageAsync = await ((Query) ref q).GetPageAsync(tPage++);
      if (pageAsync.HasValue)
      {
        num = pageAsync.Value.TotalCount;
        totalFound += pageAsync.Value.ResultCount;
        Debug.Log((object) $"w: This page has {pageAsync.Value.ResultCount} results");
        ResultPage resultPage = pageAsync.Value;
        foreach (Item entry in ((ResultPage) ref resultPage).Entries)
        {
          Debug.Log((object) ("w: Entry: " + ((Item) ref entry).Title));
          if (((Item) ref entry).IsInstalled && !((Item) ref entry).IsDownloadPending && !((Item) ref entry).IsDownloading)
          {
            if (!WorkshopMaps.filesPresent(entry))
              Debug.Log((object) "w: Incomplete files for Workshop Item, skipped");
            else
              WorkshopMaps.foundMaps.Add(entry);
          }
        }
        Debug.Log((object) pageAsync.Value.ResultCount);
        Debug.Log((object) pageAsync.Value.TotalCount);
      }
      else
        break;
    }
    List<Item> foundMaps = WorkshopMaps.foundMaps;
    q = new Query();
    return foundMaps;
  }

  internal static bool filesPresent(Item pEntry)
  {
    if (!Directory.Exists(((Item) ref pEntry).Directory))
      return false;
    string[] files = Directory.GetFiles(((Item) ref pEntry).Directory);
    Debug.Log((object) $"w: {((Item) ref pEntry).Directory} with {files.Length.ToString()} Files");
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = false;
    bool flag4 = false;
    foreach (string str in files)
    {
      if (str.Contains("map.wbox"))
        flag1 = true;
      else if (str.Contains("map.meta"))
        flag4 = true;
      else if (str.Contains("preview.png"))
        flag2 = true;
      else if (str.Contains("preview_small.png"))
        flag3 = true;
    }
    if (!flag1)
      Debug.Log((object) "w: Missing Map");
    if (!flag2)
      Debug.Log((object) "w: Missing Preview");
    if (!flag3)
      Debug.Log((object) "w: Missing PreviewSmall");
    if (!flag4)
      Debug.Log((object) "w: Missing Meta");
    return flag4 && flag1 && flag2 && flag3;
  }
}
