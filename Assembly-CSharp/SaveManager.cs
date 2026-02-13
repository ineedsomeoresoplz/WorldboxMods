// Decompiled with JetBrains decompiler
// Type: SaveManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Assets.SimpleZip;
using db;
using Ionic.Zlib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

#nullable disable
public class SaveManager : MonoBehaviour
{
  public static int currentSlot = 0;
  public static string currentSavePath = string.Empty;
  public static WorkshopMapData currentWorkshopMapData;
  private const string saveslot_base = "saves";
  private const string workshop_base = "workshop_upload";
  private const string autosaves_base = "autosaves";
  internal const string name_main_data_old = "map.json";
  internal const string name_main_data = "map.wbox";
  internal const string name_main_data_non_zip = "map.wbax";
  internal const string name_meta_data = "map.meta";
  internal const string name_stats_data = "map_stats.s3db";
  public const string name_png_preview_main = "preview.png";
  public const string name_png_preview_small = "preview_small.png";
  private SavedMap data;
  private static string persistentDataPath;
  private string _tile_id_main;
  private string _tile_id_top;
  private static bool _loading_animation_active;

  private static JsonSerializerSettings _settings => JsonHelper.read_settings;

  private static JsonSerializer _reader => JsonHelper.reader;

  private void Start() => SaveManager.persistentDataPath = Application.persistentDataPath;

  public static void clearCurrentSelectedWorld()
  {
    SaveManager.currentWorkshopMapData = (WorkshopMapData) null;
    SaveManager.currentSavePath = string.Empty;
    SaveManager.currentSlot = 0;
  }

  public void clickSaveSlot()
  {
    try
    {
      this.saveToCurrentPath();
      ScrollWindow.hideAllEvent();
    }
    catch (Exception ex)
    {
      Debug.Log((object) "Error during saving");
      Debug.LogError((object) ex);
      ScrollWindow.showWindow("error_happened");
    }
  }

  public SavedMap saveToCurrentPath()
  {
    return SaveManager.saveWorldToDirectory(SaveManager.currentSavePath);
  }

  public static SavedMap saveWorldToDirectory(string pFolder, bool pCompress = true, bool pCheckFolder = true)
  {
    pFolder = SaveManager.folderPath(pFolder);
    if (pCheckFolder)
    {
      if (!Directory.Exists(pFolder))
        Directory.CreateDirectory(pFolder);
    }
    else
      Directory.CreateDirectory(pFolder);
    SaveManager.saveImagePreview(pFolder);
    return SaveManager.saveMapData(pFolder, pCompress);
  }

  public static SavedMap currentWorldToSavedMap()
  {
    World.world.items.diagnostic();
    SavedMap savedMap = new SavedMap();
    savedMap.create();
    return savedMap;
  }

  public static void deleteSavePath(string pPath)
  {
    if (!Directory.Exists(pPath))
      return;
    Directory.Delete(pPath, true);
  }

  public static void deleteCurrentSave()
  {
    SaveManager.deleteSavePath(SaveManager.currentSavePath);
    FavoriteWorld.checkFavoriteWorld();
    ScrollWindow.hideAllEvent();
  }

  public static SavedMap saveMapData(string pFolder, bool pCompress = true)
  {
    SavedMap savedMap = SaveManager.currentWorldToSavedMap();
    pFolder = SaveManager.folderPath(pFolder);
    bool flag = false;
    string pNewPath = "";
    string str1 = "";
    string str2 = "Map";
    SaveManager.saveMetaData(savedMap.getMeta(), pFolder);
    SaveManager.saveStatsIn(pFolder);
    try
    {
      if (pCompress)
      {
        pNewPath = pFolder + "map.wbox";
        str1 = pNewPath + ".tmp";
        savedMap.toZip(str1);
      }
      else
      {
        pNewPath = pFolder + "map.wbax";
        str1 = pNewPath + ".tmp";
        savedMap.toJson(str1);
      }
    }
    catch (IOException ex)
    {
      if (Toolbox.IsDiskFull(ex))
      {
        WorldTip.showNow($"Error saving {str2} : Disk full!", false, "top");
      }
      else
      {
        Debug.Log((object) $"Could not save {str2} due to hard drive / IO Error : ");
        Debug.Log((object) ex);
        WorldTip.showNow($"Error saving {str2} due to IOError! Check console for details", false, "top");
      }
      flag = true;
    }
    catch (Exception ex)
    {
      Debug.Log((object) $"Could not save {str2} due to error : ");
      Debug.Log((object) ex);
      WorldTip.showNow($"Error saving {str2}! Check console for errors", false, "top");
      flag = true;
    }
    if (flag)
    {
      if (File.Exists(str1))
        File.Delete(str1);
    }
    else
      Toolbox.MoveSafely(str1, pNewPath);
    return savedMap;
  }

  public static void saveMetaData(MapMetaData pMetaData, string pPath)
  {
    pMetaData.prepareForSave();
    SaveManager.saveMetaIn(pPath, pMetaData);
  }

  public static MapMetaData getCurrentMeta() => SaveManager.getMetaFor(SaveManager.currentSavePath);

  public static MapMetaData getMetaFor(int pSlot)
  {
    return SaveManager.getMetaFor(SaveManager.getSlotSavePath(pSlot));
  }

  public static MapMetaData getMetaFor(string pFolder)
  {
    pFolder = SaveManager.folderPath(pFolder);
    if (!SaveManager.doesSaveExist(pFolder))
      return (MapMetaData) null;
    string metaPath = SaveManager.generateMetaPath(pFolder);
    if (!File.Exists(metaPath))
    {
      SavedMap mapFromPath = SaveManager.getMapFromPath(pFolder);
      if (mapFromPath != null)
      {
        mapFromPath.check();
        SaveManager.saveMetaData(mapFromPath.getMeta(), pFolder);
        Config.scheduleGC(nameof (getMetaFor));
      }
    }
    if (!File.Exists(metaPath))
      return (MapMetaData) null;
    try
    {
      using (FileStream fileStream = new FileStream(metaPath, FileMode.Open, FileAccess.Read))
      {
        using (StreamReader streamReader = new StreamReader((Stream) fileStream))
        {
          using (JsonReader jsonReader = (JsonReader) new JsonTextReader((TextReader) streamReader))
            return SaveManager._reader.Deserialize<MapMetaData>(jsonReader) ?? throw new Exception("Meta data was null");
        }
      }
    }
    catch (Exception ex1)
    {
      Debug.LogWarning((object) ex1);
      try
      {
        File.Delete(metaPath);
      }
      catch (Exception ex2)
      {
        Debug.Log((object) ex2);
      }
      return (MapMetaData) null;
    }
  }

  public static bool loadStatsFrom(string pFolder)
  {
    return SaveManager.doesStatsDBExist(pFolder) && DBManager.loadDBFrom(SaveManager.generateStatsPath(pFolder));
  }

  public static bool doesStatsDBExist(string pFolder)
  {
    pFolder = SaveManager.folderPath(pFolder);
    return SaveManager.doesSaveExist(pFolder) && File.Exists(SaveManager.generateStatsPath(pFolder));
  }

  public static string getMapCreationTime(string pFolder)
  {
    string savePath = SaveManager.getSavePath(pFolder);
    if (!File.Exists(savePath))
      return "??";
    DateTime dateTime = DateTime.UtcNow;
    dateTime = dateTime.AddDays(7.0);
    DateTime lastWriteTime = File.GetLastWriteTime(savePath);
    if (lastWriteTime.Year < 2017)
      return "GREG";
    if (lastWriteTime > dateTime)
      return "DREDD";
    CultureInfo culture = LocalizedTextManager.getCulture();
    string shortDatePattern = culture.DateTimeFormat.ShortDatePattern;
    string shortTimePattern = culture.DateTimeFormat.ShortTimePattern;
    return lastWriteTime.ToString($"{shortDatePattern} {shortTimePattern}", (IFormatProvider) culture);
  }

  public static void saveMetaIn(string pFolder, MapMetaData pMetaData)
  {
    string metaPath = SaveManager.generateMetaPath(pFolder);
    string json = pMetaData.toJson();
    Toolbox.WriteSafely("Map Meta", metaPath, ref json);
  }

  public static void saveStatsIn(string pFolder)
  {
    DBManager.saveToPath(SaveManager.generateStatsPath(pFolder));
  }

  public static bool doesSaveExist(string pFolder)
  {
    return Directory.Exists(pFolder) && (File.Exists(pFolder + "map.wbox") || File.Exists(pFolder + "map.wbax") || File.Exists(pFolder + "map.json"));
  }

  public static string getSavePath(string pFolder)
  {
    string path1 = SaveManager.folderPath(pFolder);
    if (!Directory.Exists(path1))
      return (string) null;
    string path2 = path1 + "map.wbox";
    if (File.Exists(path2))
      return path2;
    string path3 = path1 + "map.wbax";
    if (File.Exists(path3))
      return path3;
    string path4 = path1 + "map.json";
    return File.Exists(path4) ? path4 : (string) null;
  }

  public static bool slotExists(int pSlot)
  {
    return File.Exists(SaveManager.getSlotPathWbox(pSlot)) || File.Exists(SaveManager.getOldSlotPath(pSlot));
  }

  public static bool slotMetaExists(int pSlot = -1)
  {
    return File.Exists(SaveManager.getMetaSlotPath(pSlot));
  }

  public static void copyDataToClipboard()
  {
  }

  private static void saveImagePreview(string pFolder)
  {
    Texture2D texture = PreviewHelper.convertMapToTexture();
    Texture2D tex = new Texture2D(((Texture) texture).width, ((Texture) texture).height);
    Graphics.CopyTexture((Texture) texture, (Texture) tex);
    byte[] png1 = ImageConversion.EncodeToPNG(texture);
    Toolbox.WriteSafely("PNG Preview", SaveManager.generatePngPreviewPath(pFolder), png1);
    Object.Destroy((Object) texture);
    TextureScale.Point(tex, 32 /*0x20*/, 32 /*0x20*/);
    byte[] png2 = ImageConversion.EncodeToPNG(tex);
    Toolbox.WriteSafely("PNG Small Preview", SaveManager.generatePngSmallPreviewPath(pFolder), png2);
    Object.Destroy((Object) tex);
  }

  public static int getCurrentSlot() => SaveManager.currentSlot;

  public static string getSlotSavePath(int pSlot)
  {
    return SaveManager.generateMainPath("saves") + SaveManager.folderPath("save" + pSlot.ToString());
  }

  public static string generateMainPath(string pFolder)
  {
    return SaveManager.folderPath(SaveManager.persistentDataPath) + SaveManager.folderPath(pFolder);
  }

  public static string generateAutosavesPath(string pFolder = "")
  {
    return SaveManager.generateMainPath("autosaves") + SaveManager.folderPath(pFolder);
  }

  public static string generateWorkshopPath(string pFolder = "")
  {
    return SaveManager.generateMainPath("workshop_upload") + SaveManager.folderPath(pFolder);
  }

  public static string generatePngPreviewPath(string pFolder)
  {
    return SaveManager.folderPath(pFolder) + "preview.png";
  }

  public static string generatePngSmallPreviewPath(string pFolder)
  {
    return SaveManager.folderPath(pFolder) + "preview_small.png";
  }

  public static string generateMetaPath(string pFolder)
  {
    return SaveManager.folderPath(pFolder) + "map.meta";
  }

  public static string generateStatsPath(string pFolder)
  {
    return SaveManager.folderPath(pFolder) + "map_stats.s3db";
  }

  public static string getSlotPathWbox(int pSlot)
  {
    return SaveManager.getSlotSavePath(pSlot) + "map.wbox";
  }

  public static string getMetaSlotPath(int pSlot)
  {
    return SaveManager.getSlotSavePath(pSlot) + "map.meta";
  }

  public static string getOldSlotPath(int pSlot) => SaveManager.getSlotSavePath(pSlot) + "map.json";

  public static string getPngSlotPath(int pSlot = -1)
  {
    return SaveManager.getSlotSavePath(pSlot) + "preview.png";
  }

  public static void setCurrentSlot(int pSlotID)
  {
    SaveManager.currentSlot = pSlotID;
    SaveManager.currentSavePath = SaveManager.generateMainPath("saves") + SaveManager.folderPath("save" + pSlotID.ToString());
  }

  public static void setCurrentPath(string pPath)
  {
    SaveManager.currentSavePath = SaveManager.folderPath(pPath);
  }

  public static void setCurrentPathAndId(string pPath, int pSlotID)
  {
    SaveManager.currentSlot = pSlotID;
    SaveManager.currentSavePath = SaveManager.folderPath(pPath);
  }

  public static string getCurrentPreviewPath() => SaveManager.currentSavePath + "preview.png";

  public static bool currentSlotExists() => SaveManager.doesSaveExist(SaveManager.currentSavePath);

  public static bool currentPreviewExists()
  {
    return File.Exists(SaveManager.currentSavePath + "preview.png");
  }

  public static bool currentMetaExists() => File.Exists(SaveManager.currentSavePath + "map.meta");

  internal void loadWorld()
  {
    SaveManager._loading_animation_active = false;
    this.prepareLoading();
    if (SaveManager.currentWorkshopMapData != null)
    {
      this.loadWorld(SaveManager.currentWorkshopMapData.main_path, true);
      SaveManager.currentWorkshopMapData = (WorkshopMapData) null;
    }
    else
      this.loadWorld(SaveManager.currentSavePath);
  }

  internal void loadWorld(string pPath, bool pLoadWorkshop = false)
  {
    SmoothLoader.prepare();
    try
    {
      SavedMap mapFromPath = SaveManager.getMapFromPath(pPath, pLoadWorkshop);
      if (mapFromPath == null)
        throw new Exception("Save file not found - has it been deleted?");
      Debug.Log((object) "World Loaded");
      mapFromPath.check();
      Debug.Log((object) "World Laws Loaded");
      Config.scheduleGC("load world");
      this.loadData(mapFromPath, pPath);
    }
    catch (Exception ex1)
    {
      Debug.Log((object) ("Error during loading of slot " + pPath));
      try
      {
        MapMetaData metaFor = SaveManager.getMetaFor(pPath);
        if (metaFor != null)
          Debug.Log((object) JsonUtility.ToJson((object) metaFor));
        else
          Debug.Log((object) "No meta data");
      }
      catch (Exception ex2)
      {
        Debug.Log((object) "Failed to load meta data");
        Debug.Log((object) ex2);
      }
      Debug.LogError((object) ex1);
      ScrollWindow.showWindow("error_happened");
      MapBox.instance.startTheGame(true);
    }
  }

  public static void loadMapFromResources(string pPath)
  {
    SmoothLoader.prepare();
    SavedMap pData = JsonConvert.DeserializeObject<SavedMap>(Zip.Decompress((Resources.Load(pPath) as TextAsset).bytes), SaveManager._settings);
    pData.check();
    World.world.save_manager.loadData(pData);
  }

  public static void loadMapFromBytes(byte[] pMapData)
  {
    SmoothLoader.prepare();
    SavedMap pData = JsonConvert.DeserializeObject<SavedMap>(Zip.Decompress(pMapData), SaveManager._settings);
    pData.check();
    World.world.save_manager.loadData(pData);
  }

  public static SavedMap getMapFromPath(string pMainPath, bool pLoadWorkshop = false)
  {
    if (pLoadWorkshop)
      return JsonConvert.DeserializeObject<SavedMap>(Zip.Decompress(File.ReadAllBytes(SaveManager.folderPath(SaveManager.currentWorkshopMapData.main_path) + "map.wbox")), SaveManager._settings);
    pMainPath = SaveManager.folderPath(pMainPath);
    if (!Directory.Exists(pMainPath))
    {
      Debug.Log((object) ("Directory does not exist : " + pMainPath));
      return (SavedMap) null;
    }
    string str1 = pMainPath + "map.wbox";
    if (File.Exists(str1))
    {
      SaveManager.fileInfo(str1);
      using (FileStream fileStream = new FileStream(str1, FileMode.Open, FileAccess.Read))
      {
        using (ZlibStream zlibStream = new ZlibStream((Stream) fileStream, (CompressionMode) 1))
        {
          using (StreamReader streamReader = new StreamReader((Stream) zlibStream))
          {
            using (JsonReader jsonReader = (JsonReader) new JsonTextReader((TextReader) streamReader))
              return SaveManager._reader.Deserialize<SavedMap>(jsonReader);
          }
        }
      }
    }
    Debug.Log((object) ("Does not exist : " + str1));
    string str2 = pMainPath + "map.wbax";
    if (File.Exists(str2))
    {
      SaveManager.fileInfo(str2);
      using (FileStream fileStream = new FileStream(str2, FileMode.Open, FileAccess.Read))
      {
        using (StreamReader streamReader = new StreamReader((Stream) fileStream))
        {
          using (JsonReader jsonReader = (JsonReader) new JsonTextReader((TextReader) streamReader))
            return SaveManager._reader.Deserialize<SavedMap>(jsonReader);
        }
      }
    }
    Debug.Log((object) ("Does not exist : " + str2));
    string str3 = pMainPath + "map.json";
    if (File.Exists(str3))
    {
      SaveManager.fileInfo(str3);
      return JsonConvert.DeserializeObject<SavedMap>(Zip.Decompress(File.ReadAllText(str3)), SaveManager._settings);
    }
    Debug.Log((object) ("Does not exist : " + str3));
    return (SavedMap) null;
  }

  private static void fileInfo(string full_path)
  {
    try
    {
      FileInfo fileInfo = new FileInfo(full_path);
      Debug.Log((object) ("Loading          : " + fileInfo.Name));
      Debug.Log((object) ("Size             : " + fileInfo.Length.ToString()));
      Debug.Log((object) ("Creation time    : " + fileInfo.CreationTime.ToString()));
      Debug.Log((object) ("Last access time : " + fileInfo.LastAccessTime.ToString()));
      Debug.Log((object) ("Last write time  : " + fileInfo.LastWriteTime.ToString()));
      Debug.Log((object) ("Folder           : " + fileInfo.Directory?.Name));
    }
    catch (Exception ex)
    {
      Debug.Log((object) "Error when getting file info for");
      Debug.Log((object) full_path);
      Debug.Log((object) ex);
    }
  }

  public void startLoadSlot()
  {
    SaveManager._loading_animation_active = true;
    this.prepareLoading();
    World.world.transition_screen.startTransition(new LoadingScreen.TransitionAction(this.loadWorld));
  }

  private void prepareLoading()
  {
    ScrollWindow.hideAllEvent();
    World.world.nameplate_manager.clearAll();
  }

  private void loadTiles(string pString)
  {
    string[] strArray1 = pString.Split(',', StringSplitOptions.None);
    int zoneAmountX = Config.ZONE_AMOUNT_X;
    int zoneAmountY = Config.ZONE_AMOUNT_Y;
    if (this.data.saveVersion < 7)
    {
      string[] strArray2 = new string[World.world.tiles_list.Length];
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      for (int index1 = 0; index1 < strArray1.Length; ++index1)
      {
        if (num2 >= 50 * zoneAmountX)
        {
          num2 = 0;
          ++num3;
          num1 += 14 * zoneAmountX;
        }
        int index2 = index1 + num1;
        if (index2 <= strArray2.Length)
        {
          strArray2[index2] = strArray1[index1];
          ++num2;
        }
      }
      strArray1 = strArray2;
    }
    for (int index = 0; index < World.world.tiles_list.Length; ++index)
    {
      WorldTile tiles = World.world.tiles_list[index];
      if (tiles.data.tile_id >= strArray1.Length || strArray1[tiles.data.tile_id] == null)
      {
        tiles.setTileType("deep_ocean");
      }
      else
      {
        string[] strArray3 = strArray1[tiles.data.tile_id].Split(':', StringSplitOptions.None);
        string str = strArray3.Length == 2 ? strArray3[1] : (string) null;
        this._tile_id_main = strArray3[0];
        this._tile_id_top = string.Empty;
        this.convertOldTilesToNewOnes();
        if (string.IsNullOrEmpty(this._tile_id_top))
          tiles.setTileType(this._tile_id_main);
        else
          tiles.setTileTypes(this._tile_id_main, AssetManager.top_tiles.get(this._tile_id_top));
        tiles.Height = tiles.Type.height_min;
        if (str != null)
        {
          if (str.Contains("fire"))
            tiles.setFireData(true);
          if (str.Contains("conv0"))
          {
            tiles.data.conwayType = ConwayType.Eater;
            World.world.conway_layer.add(tiles, "conway");
          }
          if (str.Contains("conv1"))
          {
            tiles.data.conwayType = ConwayType.Creator;
            World.world.conway_layer.add(tiles, "conway_inverse");
          }
        }
      }
    }
  }

  private void convertPermafrost()
  {
    switch (this._tile_id_top)
    {
      case "snow_low":
        this._tile_id_top = "permafrost_low";
        break;
      case "snow_high":
        this._tile_id_top = "permafrost_high";
        break;
      case "snow_sand":
      case "ice":
      case "snow_hills":
      case "snow_block":
        this._tile_id_top = string.Empty;
        break;
    }
  }

  private void convertOldTilesToNewOnes()
  {
    if (this._tile_id_main.Contains("road"))
    {
      this._tile_id_main = "soil_low";
      this._tile_id_top = "road";
    }
    switch (this._tile_id_main)
    {
      case "close_ocean":
        break;
      case "deep_ocean":
        break;
      case "field":
        this._tile_id_main = "soil_low";
        this._tile_id_top = "field";
        break;
      case "forest":
      case "forest_flowers":
        this._tile_id_main = "soil_high";
        this._tile_id_top = "grass_high";
        break;
      case "forest_soil":
        this._tile_id_main = "soil_high";
        break;
      case "forest_soil_frozen":
        this._tile_id_main = "soil_high";
        this._tile_id_top = "permafrost_high";
        break;
      case "fuse":
        this._tile_id_main = "soil_low";
        this._tile_id_top = "fuse";
        break;
      case "grass":
      case "grass_flowers":
        this._tile_id_main = "soil_low";
        this._tile_id_top = "grass_low";
        break;
      case "hills":
        break;
      case "hills_frozen":
        this._tile_id_main = "mountains";
        this._tile_id_top = "snow_block";
        break;
      case "lava0":
        break;
      case "lava1":
        break;
      case "lava2":
        break;
      case "lava3":
        break;
      case "mountains":
        break;
      case "mountains_frozen":
        this._tile_id_main = "mountains";
        this._tile_id_top = "snow_block";
        break;
      case "sand":
        this._tile_id_main = "sand";
        break;
      case "sand_creep":
        this._tile_id_main = "sand";
        this._tile_id_top = "tumor_low";
        break;
      case "sand_frozen":
        this._tile_id_main = "sand";
        this._tile_id_top = "snow_sand";
        break;
      case "shallow_waters":
        break;
      case "shallow_waters_frozen":
        this._tile_id_main = "shallow_waters";
        this._tile_id_top = "ice";
        break;
      case "soil":
        this._tile_id_main = "soil_low";
        break;
      case "soil_creep":
        this._tile_id_main = "soil_low";
        this._tile_id_top = "tumor_low";
        break;
      case "soil_frozen":
        this._tile_id_main = "soil_low";
        this._tile_id_top = "permafrost_low";
        break;
      default:
        this._tile_id_main = "soil_low";
        break;
    }
  }

  private void loadTileArray(SavedMap pData)
  {
    if (pData.tileAmounts.Length == 0)
      return;
    int num1 = pData.width * pData.height * 64 /*0x40*/ * 64 /*0x40*/;
    int num2 = 0;
    for (int index1 = 0; index1 < pData.tileArray.Length; ++index1)
    {
      for (int index2 = 0; index2 < pData.tileArray[index1].Length; ++index2)
      {
        this._tile_id_top = string.Empty;
        this._tile_id_main = pData.tileMap[pData.tileArray[index1][index2]] ?? "deep_ocean";
        if (this._tile_id_main.Contains(":"))
        {
          string[] strArray = this._tile_id_main.Split(':', StringSplitOptions.None);
          this._tile_id_main = strArray[0];
          this._tile_id_top = strArray[1];
        }
        if (pData.saveVersion < 9)
          this.convertOldTilesToNewOnes();
        if (pData.saveVersion < 10)
          this.convertPermafrost();
        for (int index3 = 0; index3 < pData.tileAmounts[index1][index2]; ++index3)
        {
          WorldTile tiles = World.world.tiles_list[num2++];
          if (string.IsNullOrEmpty(this._tile_id_top))
            tiles.setTileType(this._tile_id_main);
          else
            tiles.setTileTypes(this._tile_id_main, AssetManager.top_tiles.get(this._tile_id_top));
          tiles.Height = tiles.Type.height_min;
        }
      }
    }
    while (num2 + 1 < num1)
      World.world.tiles_list[num2++].setTileType("deep_ocean");
  }

  private void loadConway(List<int> conv0, List<int> conv1)
  {
    if (conv0.Count == 0 && conv1.Count == 0)
      return;
    for (int index = 0; index < conv0.Count; ++index)
    {
      World.world.tiles_list[conv0[index]].data.conwayType = ConwayType.Eater;
      World.world.conway_layer.add(World.world.tiles_list[conv0[index]], "conway");
    }
    for (int index = 0; index < conv1.Count; ++index)
    {
      World.world.tiles_list[conv1[index]].data.conwayType = ConwayType.Creator;
      World.world.conway_layer.add(World.world.tiles_list[conv1[index]], "conway_inverse");
    }
  }

  private void loadFrozen(List<int> pTileList)
  {
    if (pTileList.Count == 0)
      return;
    for (int index = 0; index < pTileList.Count; ++index)
      World.world.tiles_list[pTileList[index]].freeze(10);
  }

  private void loadFire(List<int> pTileList)
  {
    if (pTileList.Count == 0)
      return;
    for (int index = 0; index < pTileList.Count; ++index)
      World.world.tiles_list[pTileList[index]].setFireData(true);
  }

  public void loadData(SavedMap pData, string pPath = null)
  {
    this.data = pData;
    Debug.Log((object) ("Save Version " + this.data.saveVersion.ToString()));
    SaveConverter.convert(this.data);
    World.world.addClearWorld(pData.width, pData.height);
    SmoothLoader.add((MapLoaderAction) (() => ScrollWindow.hideAllEvent()), "Hiding All Windows");
    SmoothLoader.add((MapLoaderAction) (() =>
    {
      World.world.setMapSize(pData.width, pData.height);
      World.world.hotkey_tabs_data = pData.hotkey_tabs_data;
      World.world.map_stats = pData.mapStats;
      World.world.map_stats.load();
      World.world.world_laws = pData.worldLaws;
      this.checkWorldLawsLoad();
      AssetManager.gene_library.regenerateBasicDNACodesWithLifeSeed(pData.mapStats.life_dna);
      World.world.era_manager.loadAge();
    }), "Setting Map Size");
    if (!Config.disable_db)
    {
      int num = string.IsNullOrEmpty(pPath) ? 1 : (!SaveManager.doesStatsDBExist(pPath) ? 1 : 0);
      if (num == 0)
        SmoothLoader.add((MapLoaderAction) (() =>
        {
          if (SaveManager.loadStatsFrom(pPath))
            return;
          DBManager.createDB();
        }), "Loading Stats DB");
      else
        SmoothLoader.add((MapLoaderAction) (() => DBManager.createDB()), "Creating Stats DB");
      SmoothLoader.add((MapLoaderAction) (() => DBTables.checkTablesOK(true)), "Checking Stats DB");
      DBTables.createOrMigrateTablesLoader(num != 0);
    }
    WindowPreloader.addWaitForWindowResources();
    if (pData.saveVersion < 8)
    {
      if (pData.saveVersion == 0)
        SmoothLoader.add(new MapLoaderAction(this.loadAncientTiles), "LOADING ANCIENT TILES");
      SmoothLoader.add((MapLoaderAction) (() => this.loadTiles(pData.tileString)), "Loading Very Old Tiles. Like super old. Maybe you should like, re-save your world?");
    }
    else if (pData.saveVersion > 7)
    {
      SmoothLoader.add((MapLoaderAction) (() => this.loadTileArray(pData)), "Loading Tiles");
      SmoothLoader.add((MapLoaderAction) (() => this.loadFrozen(pData.frozen_tiles)), "Loading Frozen");
      SmoothLoader.add((MapLoaderAction) (() => this.loadFire(pData.fire)), "Loading Fires");
      SmoothLoader.add((MapLoaderAction) (() => this.loadConway(pData.conwayEater, pData.conwayCreator)), "Loading Conway");
    }
    SmoothLoader.add(new MapLoaderAction(this.loadSubspecies), "Loading Subspecies");
    SmoothLoader.add(new MapLoaderAction(this.loadFamilies), "Loading Families");
    SmoothLoader.add(new MapLoaderAction(this.loadLanguages), "Loading Languages");
    SmoothLoader.add(new MapLoaderAction(this.loadReligions), "Loading Religions");
    SmoothLoader.add(new MapLoaderAction(this.loadItems), "Loading Items");
    SmoothLoader.add(new MapLoaderAction(this.loadBooks), "Loading Books");
    SmoothLoader.add(new MapLoaderAction(this.loadCultures), "Loading Cultures");
    SmoothLoader.add(new MapLoaderAction(this.loadClans), "Loading Clans");
    SmoothLoader.add(new MapLoaderAction(this.loadKingdoms), "Loading Kingdoms");
    SmoothLoader.add(new MapLoaderAction(this.loadCities), "Loading Cities");
    SmoothLoader.add(new MapLoaderAction(this.loadWars), "Loading Wars");
    SmoothLoader.add(new MapLoaderAction(this.loadArmies), "Loading Armies");
    SmoothLoader.add(new MapLoaderAction(this.loadAlliances), "Loading Alliances");
    SmoothLoader.add(new MapLoaderAction(this.loadPlots), "Loading Plots");
    SmoothLoader.add(new MapLoaderAction(this.loadActors), "Finish Loading Actors");
    SmoothLoader.add(new MapLoaderAction(this.randomDecisionCooldowns), "Set random Decision Cooldowns");
    SmoothLoader.add(new MapLoaderAction(this.loadActorLovers), "Loading Lovers");
    SmoothLoader.add(new MapLoaderAction(this.loadArmyCaptain), "Loading Army Captains");
    SmoothLoader.add(new MapLoaderAction(this.loadPlotAuthors), "Loading Plot Authors");
    SmoothLoader.add((MapLoaderAction) (() => SaveConverter.checkOldCityZones(this.data)), "Check Old City Zones");
    SmoothLoader.add(new MapLoaderAction(this.loadBuildings), "Loading Buildings");
    SmoothLoader.add((MapLoaderAction) (() => World.world.checkSimManagerLists()), "Check Meta List Stuff");
    SmoothLoader.add(new MapLoaderAction(this.setHomeBuildings), "Set Home Buildings");
    SmoothLoader.add(new MapLoaderAction(this.loadCivs02), "Loading Civs");
    SmoothLoader.add(new MapLoaderAction(this.loadLeaders), "Loading Leaders");
    SmoothLoader.add(new MapLoaderAction(this.loadDiplomacy), "Loading Diplomacy");
    World.world.addUnloadResources();
    SmoothLoader.add((MapLoaderAction) (() => World.world.map_chunk_manager.allDirty()), "Map Chunk Manager (1/2)");
    SmoothLoader.add((MapLoaderAction) (() => World.world.map_chunk_manager.update(0.0f, true)), "Map Chunk Manager (2/2)");
    SmoothLoader.add(new MapLoaderAction(this.loadBoatStates), "Loading Boats. Ahoy Ahoy");
    SmoothLoader.add((MapLoaderAction) (() => World.world.cleanUpWorld()), "Cleaning Up The World", true);
    SmoothLoader.add((MapLoaderAction) (() =>
    {
      foreach (Subspecies subspecies in (CoreSystemManager<Subspecies, SubspeciesData>) World.world.subspecies)
        subspecies.checkPhenotypeColor();
    }), "Checking Phenotypes", true);
    SmoothLoader.add((MapLoaderAction) (() => World.world.redrawTiles()), "Drawing Up The World", true);
    SmoothLoader.add((MapLoaderAction) (() => World.world.preloadRenderedSprites()), "Preload rendered sprites...", pNewWaitTimerValue: 0.2f);
    SmoothLoader.add((MapLoaderAction) (() => World.world.finishMakingWorld()), "Tidying Up The World", true);
    SmoothLoader.add((MapLoaderAction) (() => World.world.lastGC()), "Rewriting The World", true);
    World.world.addLoadAutoTester();
    World.world.addKillAllUnits();
    World.world.addLoadWorldCallbacks();
    SmoothLoader.add((MapLoaderAction) (() =>
    {
      if ((double) this.data.camera_pos_x != 0.0 && (double) this.data.camera_pos_y != 0.0)
      {
        ((Component) World.world.camera).transform.position = Vector2.op_Implicit(new Vector2(this.data.camera_pos_x, this.data.camera_pos_y));
        MoveCamera.instance.forceZoom(this.data.camera_zoom);
        MoveCamera.instance.skipResetZoom();
      }
      this.data = (SavedMap) null;
      World.world.finishingUpLoading();
    }), "Finishing up...", pNewWaitTimerValue: 0.2f);
  }

  private void checkWorldLawsLoad()
  {
    foreach (WorldLawAsset worldLawAsset in AssetManager.world_laws_library.list)
    {
      OnWorldLoadAction onWorldLoad = worldLawAsset.on_world_load;
      if (onWorldLoad != null)
        onWorldLoad();
    }
  }

  private void loadBoatStates()
  {
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
    {
      if (unit.data.transportID.hasValue())
      {
        Actor actor = World.world.units.get(unit.data.transportID);
        if (actor != null)
        {
          unit.embarkInto(actor.getSimpleComponent<Boat>());
          unit.setTask("sit_inside_boat");
          unit.ai.update();
        }
      }
    }
  }

  private void loadDiplomacy() => World.world.diplomacy.loadFromSave(this.data.relations);

  private void loadLeaders()
  {
    foreach (City city in (CoreSystemManager<City, CityData>) World.world.cities)
      city.loadLeader();
  }

  private void loadCivs02()
  {
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
      kingdom.load2();
  }

  private void setHomeBuildings()
  {
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
    {
      long homeBuildingId = unit.data.homeBuildingID;
      if (homeBuildingId.hasValue())
      {
        Building pBuilding = World.world.buildings.get(homeBuildingId);
        if (pBuilding != null && pBuilding.isUsable())
        {
          if (unit.asset.is_boat)
            pBuilding.component_docks.addBoatToDock(unit);
          else if (pBuilding.component_unit_spawner != null)
            pBuilding.component_unit_spawner.setUnitFromHere(unit);
          else
            unit.setHomeBuilding(pBuilding);
        }
      }
    }
  }

  private void loadBuildings() => World.world.buildings.loadFromSave(this.data.buildings);

  private void loadActors()
  {
    World.world.units.loadFromSave(this.data.actors_data);
    this.data.actors_data.Clear();
  }

  private void randomDecisionCooldowns()
  {
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
      unit.setupRandomDecisionCooldowns();
  }

  private void loadPlotAuthors()
  {
    foreach (Plot plot in (CoreSystemManager<Plot, PlotData>) World.world.plots)
      plot.loadAuthors();
  }

  private void loadActorLovers()
  {
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
    {
      if (unit.data.lover.hasValue())
      {
        Actor pActor = World.world.units.get(unit.data.lover);
        if (pActor != null)
          unit.setLover(pActor);
      }
    }
  }

  private void loadActorsOld(int startIndex = 0, int pAmount = 0)
  {
  }

  private void loadCities()
  {
    if (this.data.cities == null)
      return;
    World.world.cities.loadFromSave(this.data.cities);
  }

  private void loadWars()
  {
    if (this.data.wars == null)
      return;
    World.world.wars.loadFromSave(this.data.wars);
  }

  private void loadPlots()
  {
    if (this.data.plots == null)
      return;
    World.world.plots.loadFromSave(this.data.plots);
  }

  private void loadAlliances()
  {
    if (this.data.alliances == null)
      return;
    World.world.alliances.loadFromSave(this.data.alliances);
  }

  private void loadClans()
  {
    if (this.data.clans == null)
      return;
    World.world.clans.loadFromSave(this.data.clans);
  }

  private void loadKingdoms()
  {
    if (this.data.kingdoms == null)
      return;
    World.world.kingdoms.loadFromSave(this.data.kingdoms);
  }

  private void loadCultures()
  {
    if (this.data.cultures == null)
      return;
    World.world.cultures.loadFromSave(this.data.cultures);
  }

  private void loadBooks()
  {
    if (this.data.books == null)
      return;
    World.world.books.loadFromSave(this.data.books);
  }

  private void loadSubspecies()
  {
    if (this.data.subspecies == null)
      return;
    World.world.subspecies.loadFromSave(this.data.subspecies);
  }

  private void loadLanguages()
  {
    if (this.data.languages == null)
      return;
    World.world.languages.loadFromSave(this.data.languages);
  }

  private void loadReligions()
  {
    if (this.data.religions == null)
      return;
    World.world.religions.loadFromSave(this.data.religions);
  }

  private void loadItems()
  {
    if (this.data.items == null)
      return;
    World.world.items.loadFromSave(this.data.items);
  }

  private void loadFamilies()
  {
    if (this.data.families == null)
      return;
    World.world.families.loadFromSave(this.data.families);
  }

  private void loadArmies()
  {
    if (this.data.armies == null)
      return;
    World.world.armies.loadFromSave(this.data.armies);
  }

  private void loadArmyCaptain()
  {
    foreach (Army army in (CoreSystemManager<Army, ArmyData>) World.world.armies)
      army.loadDataCaptains();
  }

  private void loadAncientTiles()
  {
    using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
    {
      for (int index = 0; index < this.data.tiles.Count; ++index)
      {
        if (index > 0)
          stringBuilderPool.Append(",");
        WorldTileData tile = this.data.tiles[index];
        stringBuilderPool.Append(tile.type);
      }
      this.data.tileString = stringBuilderPool.ToString();
      this.data.tiles = new List<WorldTileData>();
    }
  }

  public static string folderPath(string pFolder)
  {
    if (string.IsNullOrEmpty(pFolder))
      return string.Empty;
    string str1 = Path.DirectorySeparatorChar.ToString();
    string str2 = Path.AltDirectorySeparatorChar.ToString();
    if (!pFolder.EndsWith(str1) && !pFolder.EndsWith(str2))
      pFolder += str1;
    return pFolder;
  }

  public static bool isLoadingSaveAnimationActive() => SaveManager._loading_animation_active;
}
