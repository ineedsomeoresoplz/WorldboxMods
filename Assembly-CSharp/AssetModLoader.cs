// Decompiled with JetBrains decompiler
// Type: AssetModLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#nullable disable
public static class AssetModLoader
{
  private static string path_log;

  public static void load()
  {
    AssetModLoader.path_log = Application.streamingAssetsPath + "/mod_loading_logs.log";
    File.WriteAllText(AssetModLoader.path_log, "");
    string pPath = Application.streamingAssetsPath + "/mods/";
    List<string> directories = AssetModLoader.getDirectories(pPath);
    AssetModLoader.log("# HELLO");
    AssetModLoader.log("# GOTTA LOAD MODS FAST");
    AssetModLoader.log("# LOADING MODS NOW");
    AssetModLoader.log("########");
    AssetModLoader.log("");
    AssetModLoader.log("# MAIN PATH: " + pPath);
    AssetModLoader.log("# TOTAL MODS: " + directories.Count.ToString());
    AssetModLoader.log("");
    for (int index = 0; index < directories.Count; ++index)
    {
      string str = directories[index];
      AssetModLoader.log("---------START------------------------------------------------------------------------------------");
      AssetModLoader.log("## LOADING MOD N " + (index + 1).ToString());
      AssetModLoader.log(str);
      AssetModLoader.loadMod(str);
      AssetModLoader.log("---------FINISH-----------------------------------------------------------------------------------");
      AssetModLoader.log("");
      AssetModLoader.log("");
    }
  }

  private static void loadMod(string pPath)
  {
    string[] strArray = pPath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
    AssetModLoader.log("# CHECKING MOD... " + strArray[strArray.Length - 1]);
    foreach (string directory in AssetModLoader.getDirectories(pPath))
      AssetModLoader.checkModAssets(directory);
  }

  private static void checkModAssets(string pPath)
  {
    List<string> directories = AssetModLoader.getDirectories(pPath);
    string[] strArray = pPath.Split(Path.DirectorySeparatorChar, StringSplitOptions.None);
    AssetModLoader.log("");
    string pType = strArray[strArray.Length - 1];
    AssetModLoader.log("## CHECKING MOD FOLDER... " + pType);
    AssetModLoader.log("## SUB FOLDERS FOUND: " + directories.Count.ToString());
    AssetModLoader.log("");
    foreach (string pPath1 in directories)
      AssetModLoader.checkModFolder(pPath1, pType);
  }

  private static void checkModFolder(string pPath, string pType)
  {
    List<string> files = AssetModLoader.getFiles(pPath);
    string[] strArray = pPath.Split(Path.DirectorySeparatorChar, StringSplitOptions.None);
    AssetModLoader.log("");
    AssetModLoader.log("# CHECKING PATH... " + strArray[strArray.Length - 1]);
    AssetModLoader.log("FILES: " + files.Count.ToString());
    AssetModLoader.log("");
    foreach (string str in files)
    {
      AssetModLoader.log(str);
      if (str.Contains("json"))
        AssetModLoader.loadFileJson(str, pType);
      if (str.Contains("png"))
        AssetModLoader.loadTexture(str);
    }
  }

  private static void loadTexture(string pPath)
  {
    string[] strArray = pPath.Split(Path.DirectorySeparatorChar, StringSplitOptions.None);
    string str = strArray[strArray.Length - 1];
    AssetModLoader.log("# LOAD TEXTURE: " + str);
    byte[] pBytes = File.ReadAllBytes(pPath);
    string pPathID = "@wb_" + str;
    AssetModLoader.log("ADDING TEXTURE... " + pPathID);
    SpriteTextureLoader.addSprite(pPathID, pBytes);
  }

  private static void loadFileJson(string pPath, string pType)
  {
    string[] strArray = pPath.Split(Path.DirectorySeparatorChar, StringSplitOptions.None);
    AssetModLoader.log("# LOAD ASSET: " + strArray[strArray.Length - 1]);
    string pData = File.ReadAllText(pPath);
    switch (pType)
    {
      case "actors":
        break;
      case "buildings":
        AssetModLoader.loadAssetBuilding(pData);
        break;
      case "kingdoms":
        break;
      case "powers":
        AssetModLoader.loadAssetPowers(pData);
        break;
      default:
        int num = pType == "traits" ? 1 : 0;
        break;
    }
  }

  private static void loadAssetActor(string pData)
  {
    ActorAsset pAsset = JsonUtility.FromJson<ActorAsset>(pData);
    AssetManager.actor_library.add(pAsset);
  }

  private static void loadAssetBuilding(string pData)
  {
    BuildingAsset pAsset = JsonUtility.FromJson<BuildingAsset>(pData);
    AssetManager.buildings.add(pAsset);
  }

  private static void loadAssetKingdom(string pData)
  {
    KingdomAsset pAsset = JsonUtility.FromJson<KingdomAsset>(pData);
    AssetManager.kingdoms.add(pAsset);
  }

  private static void loadAssetPowers(string pData)
  {
    GodPower pAsset = JsonUtility.FromJson<GodPower>(pData);
    AssetManager.powers.add(pAsset);
  }

  private static void loadAssetTraits(string pData)
  {
    ActorTrait pAsset = JsonUtility.FromJson<ActorTrait>(pData);
    AssetManager.traits.add(pAsset);
  }

  private static void log(string pLog) => File.AppendAllText(AssetModLoader.path_log, pLog + "\n");

  private static List<string> getDirectories(string pPath)
  {
    List<string> directories = new List<string>();
    foreach (string directory in Directory.GetDirectories(pPath))
    {
      if (!directory.Contains(".meta"))
        directories.Add(directory);
    }
    return directories;
  }

  private static List<string> getFiles(string pPath)
  {
    List<string> files = new List<string>();
    foreach (string file in Directory.GetFiles(pPath))
    {
      if (!file.Contains(".meta"))
        files.Add(file);
    }
    return files;
  }
}
