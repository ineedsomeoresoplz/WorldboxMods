// Decompiled with JetBrains decompiler
// Type: WorkshopAchievements
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Steamworks;
using Steamworks.Ugc;
using System;
using System.Threading.Tasks;
using UnityEngine;

#nullable disable
internal class WorkshopAchievements
{
  internal static void checkAchievements()
  {
    SteamSDK.steamInitialized.Then((Action) (() => WorkshopAchievements.countUsersWorkshopMaps())).Catch((Action<Exception>) (err =>
    {
      Debug.Log((object) "Error happened while getting users maps");
      Debug.Log((object) err);
    }));
  }

  internal static async Task countUsersWorkshopMaps()
  {
    Query itemsReadyToUse = Query.ItemsReadyToUse;
    Query query = ((Query) ref itemsReadyToUse).WhereUserPublished(new SteamId());
    Query tQuery = ((Query) ref query).WithTag("World");
    int tTotalVotes = 0;
    int tTotalCount = 1;
    int tTotalFound = 0;
    int tPage = 1;
    while (tTotalCount > tTotalFound)
    {
      ResultPage? pageAsync = await ((Query) ref tQuery).GetPageAsync(tPage++);
      if (pageAsync.HasValue)
      {
        tTotalCount = pageAsync.Value.TotalCount;
        tTotalFound += pageAsync.Value.ResultCount;
        ResultPage resultPage = pageAsync.Value;
        foreach (Item entry in ((ResultPage) ref resultPage).Entries)
          tTotalVotes += (int) ((Item) ref entry).VotesUp;
      }
    }
    if ((long) tTotalCount > World.world.game_stats.data.workshopUploads)
      World.world.game_stats.data.workshopUploads = (long) tTotalCount;
    AchievementLibrary.checkSteamMapUploads();
    tQuery = new Query();
  }
}
