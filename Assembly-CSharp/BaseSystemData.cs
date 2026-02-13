// Decompiled with JetBrains decompiler
// Type: BaseSystemData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using UnityEngine.Scripting;
using UnityPools;

#nullable disable
[Serializable]
public abstract class BaseSystemData : IDisposable
{
  [DefaultValue(null)]
  public List<NameEntry> past_names;
  [DefaultValue(null)]
  public CustomDataContainer<int> custom_data_int;
  [DefaultValue(null)]
  public CustomDataContainer<long> custom_data_long;
  [DefaultValue(null)]
  public CustomDataContainer<float> custom_data_float;
  [DefaultValue(null)]
  public CustomDataContainer<bool> custom_data_bool;
  [DefaultValue(null)]
  public CustomDataContainer<string> custom_data_string;
  [DefaultValue(null)]
  public HashSet<string> custom_data_flags;
  [JsonIgnore]
  public bool from_db;

  [PrimaryKey]
  [NotNull]
  [JsonProperty]
  [DefaultValue(-1)]
  public long id { get; set; } = -1;

  [JsonProperty]
  public string name { get; set; }

  public bool custom_name { get; set; }

  [DefaultValue(-1)]
  public long name_culture_id { get; set; } = -1;

  [JsonProperty]
  [DefaultValue(0.0)]
  public double created_time { get; set; }

  [DefaultValue(0.0)]
  public double died_time { get; set; }

  [DefaultValue(false)]
  public bool favorite { get; set; }

  [JsonProperty]
  [Preserve]
  [DefaultValue(1)]
  [Obsolete("Use created_time instead")]
  public int age
  {
    set
    {
      if (value < 1 || this.created_time > 0.0)
        return;
      this.created_time = (double) (-1 * value) * 60.0;
    }
  }

  public void cloneCustomDataFrom(BaseSystemData pTarget)
  {
    if (pTarget.custom_data_int != null)
    {
      foreach (KeyValuePair<string, int> keyValuePair in pTarget.custom_data_int.dict)
        this.set(keyValuePair.Key, keyValuePair.Value);
    }
    if (pTarget.custom_data_long != null)
    {
      foreach (KeyValuePair<string, long> keyValuePair in pTarget.custom_data_long.dict)
        this.set(keyValuePair.Key, keyValuePair.Value);
    }
    if (pTarget.custom_data_float != null)
    {
      foreach (KeyValuePair<string, float> keyValuePair in pTarget.custom_data_float.dict)
      {
        double num = (double) this.set(keyValuePair.Key, keyValuePair.Value);
      }
    }
    if (pTarget.custom_data_bool != null)
    {
      foreach (KeyValuePair<string, bool> keyValuePair in pTarget.custom_data_bool.dict)
        this.set(keyValuePair.Key, keyValuePair.Value);
    }
    if (pTarget.custom_data_string != null)
    {
      foreach (KeyValuePair<string, string> keyValuePair in pTarget.custom_data_string.dict)
        this.set(keyValuePair.Key, keyValuePair.Value);
    }
    if (pTarget.custom_data_flags == null)
      return;
    foreach (string customDataFlag in pTarget.custom_data_flags)
      this.addFlag(customDataFlag);
  }

  public Dictionary<string, string> debug()
  {
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    if (this.custom_data_int != null)
    {
      foreach (KeyValuePair<string, int> keyValuePair in this.custom_data_int.dict)
        dictionary.Add(keyValuePair.Key, keyValuePair.Value.ToString());
    }
    if (this.custom_data_long != null)
    {
      foreach (KeyValuePair<string, long> keyValuePair in this.custom_data_long.dict)
        dictionary.Add(keyValuePair.Key, keyValuePair.Value.ToString());
    }
    if (this.custom_data_float != null)
    {
      foreach (KeyValuePair<string, float> keyValuePair in this.custom_data_float.dict)
        dictionary.Add(keyValuePair.Key, keyValuePair.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }
    if (this.custom_data_bool != null)
    {
      foreach (KeyValuePair<string, bool> keyValuePair in this.custom_data_bool.dict)
        dictionary.Add(keyValuePair.Key, keyValuePair.Value.ToString());
    }
    if (this.custom_data_string != null)
    {
      foreach (KeyValuePair<string, string> keyValuePair in this.custom_data_string.dict)
        dictionary.Add(keyValuePair.Key, keyValuePair.Value);
    }
    if (this.custom_data_flags != null)
    {
      foreach (string customDataFlag in this.custom_data_flags)
        dictionary.Add("Flag", customDataFlag);
    }
    return dictionary;
  }

  public void save()
  {
    this.checkInt();
    this.checkLong();
    this.checkFloat();
    this.checkBool();
    this.checkString();
    this.checkFlags();
  }

  public void checkInt()
  {
    CustomDataContainer<int> customDataInt = this.custom_data_int;
    if ((customDataInt != null ? (customDataInt.dict.Count == 0 ? 1 : 0) : 0) == 0)
      return;
    this.custom_data_int.Dispose();
    this.custom_data_int = (CustomDataContainer<int>) null;
  }

  public void checkLong()
  {
    CustomDataContainer<long> customDataLong = this.custom_data_long;
    if ((customDataLong != null ? (customDataLong.dict.Count == 0 ? 1 : 0) : 0) == 0)
      return;
    this.custom_data_long.Dispose();
    this.custom_data_long = (CustomDataContainer<long>) null;
  }

  public void checkFloat()
  {
    CustomDataContainer<float> customDataFloat = this.custom_data_float;
    if ((customDataFloat != null ? (customDataFloat.dict.Count == 0 ? 1 : 0) : 0) == 0)
      return;
    this.custom_data_float.Dispose();
    this.custom_data_float = (CustomDataContainer<float>) null;
  }

  public void checkBool()
  {
    CustomDataContainer<bool> customDataBool = this.custom_data_bool;
    if ((customDataBool != null ? (customDataBool.dict.Count == 0 ? 1 : 0) : 0) == 0)
      return;
    this.custom_data_bool.Dispose();
    this.custom_data_bool = (CustomDataContainer<bool>) null;
  }

  public void checkString()
  {
    CustomDataContainer<string> customDataString = this.custom_data_string;
    if ((customDataString != null ? (customDataString.dict.Count == 0 ? 1 : 0) : 0) == 0)
      return;
    this.custom_data_string.Dispose();
    this.custom_data_string = (CustomDataContainer<string>) null;
  }

  public void checkFlags()
  {
    if (this.custom_data_flags == null || this.custom_data_flags.Count != 0)
      return;
    UnsafeCollectionPool<HashSet<string>, string>.Release(this.custom_data_flags);
    this.custom_data_flags = (HashSet<string>) null;
  }

  public void load()
  {
  }

  public void get(string pKey, out int pResult, int pDefault = 0)
  {
    if (this.custom_data_int != null && this.custom_data_int.TryGetValue(pKey, out pResult))
      return;
    pResult = pDefault;
  }

  public void get(string pKey, out long pResult, long pDefault = 0)
  {
    if (this.custom_data_long != null && this.custom_data_long.TryGetValue(pKey, out pResult))
      return;
    pResult = pDefault;
  }

  public void get(string pKey, out float pResult, float pDefault = 0.0f)
  {
    if (this.custom_data_float != null && this.custom_data_float.TryGetValue(pKey, out pResult))
      return;
    pResult = pDefault;
  }

  public void get(string pKey, out string pResult, string pDefault = null)
  {
    if (this.custom_data_string != null && this.custom_data_string.TryGetValue(pKey, out pResult))
      return;
    pResult = pDefault;
  }

  public void get(string pKey, out bool pResult, bool pDefault = false)
  {
    if (this.custom_data_bool != null && this.custom_data_bool.TryGetValue(pKey, out pResult))
      return;
    pResult = pDefault;
  }

  public int set(string pKey, int pData)
  {
    if (this.custom_data_int == null)
      this.custom_data_int = new CustomDataContainer<int>();
    this.custom_data_int[pKey] = pData;
    return pData;
  }

  public long set(string pKey, long pData)
  {
    if (this.custom_data_long == null)
      this.custom_data_long = new CustomDataContainer<long>();
    this.custom_data_long[pKey] = pData;
    return pData;
  }

  public float set(string pKey, float pData)
  {
    if (this.custom_data_float == null)
      this.custom_data_float = new CustomDataContainer<float>();
    this.custom_data_float[pKey] = pData;
    return pData;
  }

  public string set(string pKey, string pData)
  {
    if (this.custom_data_string == null)
      this.custom_data_string = new CustomDataContainer<string>();
    this.custom_data_string[pKey] = pData;
    return pData;
  }

  public bool set(string pKey, bool pData)
  {
    if (this.custom_data_bool == null)
      this.custom_data_bool = new CustomDataContainer<bool>();
    this.custom_data_bool[pKey] = pData;
    return pData;
  }

  public void change(string pKey, int pValue, int pMin = 0, int pMax = 1000)
  {
    int pResult;
    this.get(pKey, out pResult);
    int pData = pResult + pValue;
    if (pData < pMin)
      pData = pMin;
    if (pData > pMax)
      pData = pMax;
    this.set(pKey, pData);
  }

  public void removeInt(string pKey)
  {
    if (this.custom_data_int == null)
      return;
    this.custom_data_int.Remove(pKey);
    this.checkInt();
  }

  public void removeLong(string pKey)
  {
    if (this.custom_data_long == null)
      return;
    this.custom_data_long.Remove(pKey);
    this.checkLong();
  }

  public void removeFloat(string pKey)
  {
    if (this.custom_data_float == null)
      return;
    this.custom_data_float.Remove(pKey);
    this.checkFloat();
  }

  public void removeString(string pKey)
  {
    if (this.custom_data_string == null)
      return;
    this.custom_data_string.Remove(pKey);
    this.checkString();
  }

  public void removeBool(string pKey)
  {
    if (this.custom_data_bool == null)
      return;
    this.custom_data_bool.Remove(pKey);
    this.checkBool();
  }

  public bool addFlag(string pID)
  {
    if (this.custom_data_flags == null)
      this.custom_data_flags = UnsafeCollectionPool<HashSet<string>, string>.Get();
    return this.custom_data_flags.Add(pID);
  }

  public bool hasFlag(string pID)
  {
    HashSet<string> customDataFlags = this.custom_data_flags;
    // ISSUE: explicit non-virtual call
    return customDataFlags != null && __nonvirtual (customDataFlags.Contains(pID));
  }

  public void removeFlag(string pID)
  {
    if (this.custom_data_flags == null)
      return;
    this.custom_data_flags.Remove(pID);
    this.checkFlags();
  }

  public virtual void Dispose()
  {
    this.custom_data_int?.Dispose();
    this.custom_data_long?.Dispose();
    this.custom_data_float?.Dispose();
    this.custom_data_bool?.Dispose();
    this.custom_data_string?.Dispose();
    this.custom_data_int = (CustomDataContainer<int>) null;
    this.custom_data_long = (CustomDataContainer<long>) null;
    this.custom_data_float = (CustomDataContainer<float>) null;
    this.custom_data_bool = (CustomDataContainer<bool>) null;
    this.custom_data_string = (CustomDataContainer<string>) null;
    this.custom_data_flags?.Clear();
    this.checkFlags();
    this.past_names?.Clear();
    this.past_names = (List<NameEntry>) null;
  }

  [JsonIgnore]
  [Ignore]
  public float this[string pKey]
  {
    get
    {
      float pResult;
      this.get(pKey, out pResult);
      return pResult;
    }
    set
    {
      double num = (double) this.set(pKey, value);
    }
  }

  [JsonIgnore]
  public string obsidian_name_id => $"{this.name}({this.id.ToString()})";
}
