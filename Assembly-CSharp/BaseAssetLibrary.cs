// Decompiled with JetBrains decompiler
// Type: BaseAssetLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.UnityConverters.Math;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

#nullable disable
public abstract class BaseAssetLibrary
{
  private const string ERROR_COLOR_WHITE = "#FFFFFF";
  private const string ERROR_COLOR_RED = "#FF3232";
  private const string ERROR_COLOR_YELLOW = "#FFF832";
  private const string ERROR_COLOR_MAIN = "#D2B7FF";
  [JsonProperty(Order = -1)]
  public string id = "ASSET_LIBRARY";
  protected static int _latest_hash = 1;
  private static JsonSerializer _json_serializer_internal = (JsonSerializer) null;

  public virtual void init()
  {
    string version = Application.version;
  }

  private static JsonSerializer _json_serializer
  {
    get
    {
      if (BaseAssetLibrary._json_serializer_internal == null)
      {
        JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
        serializerSettings.DefaultValueHandling = (DefaultValueHandling) 1;
        serializerSettings.Formatting = (Formatting) 1;
        serializerSettings.ReferenceLoopHandling = (ReferenceLoopHandling) 1;
        serializerSettings.Culture = CultureInfo.InvariantCulture;
        serializerSettings.ContractResolver = (IContractResolver) new OrderedContractResolver();
        serializerSettings.Converters.Add((JsonConverter) new DelegateConverter());
        serializerSettings.Converters.Add((JsonConverter) new StringEnumConverter());
        serializerSettings.Converters.Add((JsonConverter) new Color32Converter());
        serializerSettings.Converters.Add((JsonConverter) new ColorConverter());
        serializerSettings.Converters.Add((JsonConverter) new Vector2Converter());
        serializerSettings.Converters.Add((JsonConverter) new Vector2IntConverter());
        serializerSettings.Converters.Add((JsonConverter) new Vector3Converter());
        serializerSettings.Converters.Add((JsonConverter) new Vector3IntConverter());
        serializerSettings.Converters.Add((JsonConverter) new Vector4Converter());
        BaseAssetLibrary._json_serializer_internal = JsonSerializer.Create(serializerSettings);
      }
      return BaseAssetLibrary._json_serializer_internal;
    }
  }

  public void exportAssets()
  {
    if (this.id.StartsWith("beh") || this.id.StartsWith("debug"))
      return;
    string path = $"GenAssets/wbassets/{this.id}.json";
    try
    {
      using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
      {
        StreamWriter streamWriter1 = new StreamWriter((Stream) fileStream);
        streamWriter1.NewLine = "\n";
        using (StreamWriter streamWriter2 = streamWriter1)
        {
          JsonTextWriter jsonTextWriter1 = new JsonTextWriter((TextWriter) streamWriter2);
          ((JsonWriter) jsonTextWriter1).Formatting = (Formatting) 1;
          jsonTextWriter1.Indentation = 4;
          jsonTextWriter1.IndentChar = ' ';
          using (JsonTextWriter jsonTextWriter2 = jsonTextWriter1)
            BaseAssetLibrary._json_serializer.Serialize((JsonWriter) jsonTextWriter2, (object) this);
        }
      }
    }
    catch (Exception ex)
    {
      Debug.LogError((object) $"Failed to export assets to {path}: {ex.Message}");
    }
  }

  public void importAssets()
  {
    string path = $"GenAssets/wbassets/{this.id}.json";
    if (!File.Exists(path))
    {
      Debug.LogError((object) ("File not found: " + path));
    }
    else
    {
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        using (StreamReader streamReader = new StreamReader((Stream) fileStream))
        {
          using (JsonTextReader jsonTextReader = new JsonTextReader((TextReader) streamReader))
            BaseAssetLibrary._json_serializer.Populate((JsonReader) jsonTextReader, (object) this);
        }
      }
    }
  }

  public virtual void post_init()
  {
  }

  public virtual void linkAssets()
  {
  }

  public virtual void editorDiagnostic() => this.editorDiagnosticLocales();

  public virtual void editorDiagnosticLocales()
  {
  }

  public virtual void checkLocale(Asset pAsset, string pLocaleID)
  {
    if (string.IsNullOrEmpty(pLocaleID) || LocalizedTextManager.stringExists(pLocaleID))
      return;
    BaseAssetLibrary.logAssetError($"<e>{pAsset.id}</e>: Missing translation key", pLocaleID);
    AssetManager.missing_locale_keys.Add(pLocaleID);
  }

  internal bool hasSpriteInResources(string pPath)
  {
    return !Object.op_Equality((Object) SpriteTextureLoader.getSprite(pPath), (Object) null);
  }

  internal bool hasSpriteInResourcesDebug(string pPath)
  {
    string path = Path.Combine(Path.Combine("Assets/Resources", pPath).Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
    return File.Exists(path + ".png") || Directory.Exists(path) && Directory.GetFiles(path, "*.png", SearchOption.TopDirectoryOnly).Length != 0;
  }

  protected void logErrorOpposites(string pMainTraitID, string pOppositeTraitID)
  {
    Debug.LogError((object) $"<color=#FF3232>[{pMainTraitID}]</color> has opposite <color=#FFF832>[{pOppositeTraitID}]</color>, but <color=#FFF832>[{pOppositeTraitID}]</color> doesn't have opposite <color=#FF3232>[{pMainTraitID}]</color>");
  }

  private static string formatLog(string pMessage, string pRightPart = null)
  {
    if (pMessage.Contains("<"))
    {
      pMessage = pMessage.Replace("<e>", "<b><color=#FFFFFF>");
      pMessage = pMessage.Replace("</e>", "</color></b>");
    }
    string str = $"<color=#D2B7FF>{pMessage.Trim()}</color>";
    if (!string.IsNullOrEmpty(pRightPart))
      str = $"{str} : <b><color=#FFF832>{pRightPart.Trim()}</color></b>";
    return str;
  }

  public static void logAssetLog(string pMessage, string pRightPart = null)
  {
    Debug.Log((object) BaseAssetLibrary.formatLog(pMessage, pRightPart));
  }

  public static void logAssetError(string pMessage, string pRightPart = null)
  {
    Debug.LogError((object) BaseAssetLibrary.formatLog(pMessage, pRightPart));
  }

  public virtual IEnumerable<Asset> getList()
  {
    yield break;
  }

  public virtual int total_items => 0;
}
