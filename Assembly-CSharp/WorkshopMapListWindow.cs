// Decompiled with JetBrains decompiler
// Type: WorkshopMapListWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Steamworks.Ugc;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#nullable disable
public class WorkshopMapListWindow : MonoBehaviour
{
  public WorkshopMapElement elementPrefab;
  private Dictionary<string, Sprite> cached_sprites = new Dictionary<string, Sprite>();
  private List<WorkshopMapElement> elements = new List<WorkshopMapElement>();
  public Transform transformContent;
  private float _timer;
  private bool _no_items;
  private Queue<Item> _showQueue = new Queue<Item>();

  private void OnEnable()
  {
    if (!Config.game_loaded)
      return;
    this._timer = 0.3f;
    foreach (Component element in this.elements)
      Object.Destroy((Object) element.gameObject);
    this.elements.Clear();
    SteamSDK.steamInitialized.Then((Action) (() => this.prepareList())).Catch((Action<Exception>) (err =>
    {
      Debug.LogError((object) err);
      ErrorWindow.errorMessage = "Error happened while connecting to Steam Workshop:\n" + err.Message.ToString();
      ScrollWindow.get("error_with_reason").clickShow();
    }));
  }

  private void OnDisable() => this._showQueue.Clear();

  private void Update()
  {
    if ((double) this._timer > 0.0)
    {
      this._timer -= Time.deltaTime;
    }
    else
    {
      this._timer = 0.015f;
      this.showNextItemFromQueue();
    }
    if (!this._no_items)
      return;
    this._no_items = false;
    ScrollWindow.showWindow("steam_workshop_empty");
  }

  private async void prepareList()
  {
    List<Item> objList = await WorkshopMaps.listWorkshopMaps();
    if (objList.Count > 0)
    {
      foreach (Item obj in objList)
        this._showQueue.Enqueue(obj);
      AchievementLibrary.checkSteamMapDownloads(objList.Count);
    }
    else
      this._no_items = true;
  }

  private void showNextItemFromQueue()
  {
    if (this._showQueue.Count == 0)
      return;
    this.renderMapElement(this._showQueue.Dequeue());
  }

  private WorkshopMapData loadMapDataFromStorage(Item pSteamworksItem)
  {
    string smallPreviewPath = SaveManager.generatePngSmallPreviewPath(((Item) ref pSteamworksItem).Directory);
    WorkshopMapData workshopMapData = new WorkshopMapData();
    workshopMapData.main_path = ((Item) ref pSteamworksItem).Directory;
    workshopMapData.workshop_item = pSteamworksItem;
    if (!string.IsNullOrEmpty(smallPreviewPath) && File.Exists(smallPreviewPath))
    {
      if (this.cached_sprites.ContainsKey(smallPreviewPath))
      {
        workshopMapData.sprite_small_preview = this.cached_sprites[smallPreviewPath];
      }
      else
      {
        try
        {
          byte[] numArray = File.ReadAllBytes(smallPreviewPath);
          Texture2D texture2D = new Texture2D(32 /*0x20*/, 32 /*0x20*/);
          ((Texture) texture2D).anisoLevel = 0;
          ((Texture) texture2D).filterMode = (FilterMode) 0;
          if (ImageConversion.LoadImage(texture2D, numArray))
          {
            workshopMapData.sprite_small_preview = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, 32f, 32f), new Vector2(0.5f, 0.5f));
            this.cached_sprites.Add(smallPreviewPath, workshopMapData.sprite_small_preview);
          }
        }
        catch (Exception ex)
        {
        }
      }
    }
    MapMetaData metaFor = SaveManager.getMetaFor(((Item) ref pSteamworksItem).Directory);
    bool flag = false;
    if (!string.IsNullOrWhiteSpace(((Item) ref pSteamworksItem).Title) && metaFor.mapStats.name != ((Item) ref pSteamworksItem).Title)
    {
      metaFor.mapStats.name = ((Item) ref pSteamworksItem).Title;
      flag = true;
    }
    if (metaFor.mapStats.description != ((Item) ref pSteamworksItem).Description)
    {
      metaFor.mapStats.description = ((Item) ref pSteamworksItem).Description;
      flag = true;
    }
    if (flag)
      SaveManager.saveMetaIn(((Item) ref pSteamworksItem).Directory, metaFor);
    workshopMapData.meta_data_map = metaFor;
    return workshopMapData;
  }

  private void renderMapElement(Item pSteamworksItem)
  {
    WorkshopMapElement workshopMapElement = Object.Instantiate<WorkshopMapElement>(this.elementPrefab, this.transformContent);
    this.elements.Add(workshopMapElement);
    WorkshopMapData pData = this.loadMapDataFromStorage(pSteamworksItem);
    workshopMapElement.load(pData);
  }
}
