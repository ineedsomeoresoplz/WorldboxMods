// Decompiled with JetBrains decompiler
// Type: AssetLibrary`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#nullable disable
[ObfuscateLiterals]
[Serializable]
public abstract class AssetLibrary<T> : BaseAssetLibrary where T : Asset
{
  public List<T> list = new List<T>();
  [NonSerialized]
  public Dictionary<string, T> dict = new Dictionary<string, T>();
  protected T t;
  private T[] _array;
  public string file_path;
  private HashSet<string> _not_found = new HashSet<string>();

  public virtual T get(string pID)
  {
    T obj;
    if (this.dict.TryGetValue(pID, out obj))
      return obj;
    this._not_found.Add(pID);
    return default (T);
  }

  public T getSimple(string pID)
  {
    if (!this.has(pID))
      return default (T);
    T obj;
    return this.dict.TryGetValue(pID, out obj) ? obj : default (T);
  }

  public virtual bool has(string pID) => this.dict.ContainsKey(pID);

  public virtual T add(T pAsset)
  {
    string id = pAsset.id;
    if (this.dict.ContainsKey(id))
    {
      for (int index = 0; index < this.list.Count; ++index)
      {
        if (!(this.list[index].id != id))
        {
          this.list.RemoveAt(index);
          break;
        }
      }
      this.dict.Remove(id);
      BaseAssetLibrary.logAssetError($"<e>AssetLibrary<{typeof (T).Name}></e>: duplicate asset - overwriting...", id);
    }
    this.t = pAsset;
    this.t.create();
    this.t.setHash(BaseAssetLibrary._latest_hash++);
    if (!pAsset.isTemplateAsset())
      this.list.Add(pAsset);
    this.t.setIndexID(this.list.Count);
    this.dict.Add(id, pAsset);
    return pAsset;
  }

  public virtual T clone(string pNew, string pFrom)
  {
    T pNew1;
    this.clone(out pNew1, this.dict[pFrom]);
    this.t = pNew1;
    this.t.id = pNew;
    this.add(this.t);
    return this.t;
  }

  public virtual void clone(out T pNew, T pFrom)
  {
    pNew = Activator.CreateInstance<T>();
    foreach (FieldInfo field in typeof (T).GetFields(BindingFlags.Instance | BindingFlags.Public))
    {
      if (!field.IsNotSerialized)
      {
        object obj = field.GetValue((object) pFrom);
        if (obj == null || field.isString())
          field.SetValue((object) pNew, obj);
        else if (field.isCloneable())
        {
          ICloneable cloneable = obj as ICloneable;
          field.SetValue((object) pNew, cloneable.Clone());
        }
        else if (field.isCollection())
        {
          ICollection collection = obj as ICollection;
          field.SetValue((object) pNew, Activator.CreateInstance(field.FieldType, (object) collection));
        }
        else if (field.isEnumerable())
        {
          IEnumerable enumerable = obj as IEnumerable;
          field.SetValue((object) pNew, Activator.CreateInstance(field.FieldType, (object) enumerable));
        }
        else
          field.SetValue((object) pNew, obj);
      }
    }
  }

  internal void loadFromFile<TAssetLib>() where TAssetLib : AssetLibrary<T>
  {
    foreach (T pAsset in JsonUtility.FromJson<TAssetLib>(Resources.Load<TextAsset>(this.file_path).text).list)
      this.add(pAsset);
  }

  public T[] getArray()
  {
    if (this._array == null)
      this._array = this.list.ToArray();
    return this._array;
  }

  public override void editorDiagnostic()
  {
    for (Type type = typeof (T); type != (Type) null; type = type.BaseType)
    {
      if (!type.IsSerializable)
        BaseAssetLibrary.logAssetError($"<e>AssetLibrary<{typeof (T).Name}></e>: Asset not marked serializable", type.Name);
    }
    foreach (FieldInfo field in typeof (T).GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
    {
      if (field.IsAssembly && !field.IsNotSerialized)
        BaseAssetLibrary.logAssetError($"<e>AssetLibrary<{typeof (T).Name}></e>: Asset field is marked <e>internal</e> - set it to <e>public</e> and/or <e>[NonSerialized]</e> instead. Currently it's not being cloned.", field.Name);
      if (field.IsFamily && !field.IsNotSerialized)
        BaseAssetLibrary.logAssetError($"<e>AssetLibrary<{typeof (T).Name}></e>: Asset field is marked <e>protected</e> - set it to <e>public</e> and/or <e>[NonSerialized]</e> instead. Currently it's not being cloned.", field.Name);
      if (field.IsPrivate && field.GetCustomAttribute<SerializeField>() != null)
        BaseAssetLibrary.logAssetError($"<e>AssetLibrary<{typeof (T).Name}></e>: Asset field is marked <e>private</e> and has <e>[SerializeField]</e> attribute - it won't be cloned. Set it to <e>public</e> instead", field.Name);
    }
    base.editorDiagnostic();
  }

  public override void checkLocale(Asset pAsset, string pLocaleID)
  {
    string pKey = pLocaleID != null ? pLocaleID.Underscore() : (string) null;
    if (pKey != pLocaleID)
      BaseAssetLibrary.logAssetError($"<e>AssetLibrary<{typeof (T).Name}></e>: Translation key is not in lowercase - <e>{pLocaleID}</e> should be <e>{pKey}</e>", pAsset.id);
    switch (pAsset)
    {
      case ILocalizedAsset _:
      case IMultiLocalesAsset _:
label_4:
        if (string.IsNullOrEmpty(pKey) || LocalizedTextManager.stringExists(pKey))
          break;
        BaseAssetLibrary.logAssetError($"<e>AssetLibrary<{typeof (T).Name}></e>: Missing translation key <e>{pKey}</e>", pAsset.id);
        AssetManager.missing_locale_keys.Add(pKey);
        break;
      default:
        BaseAssetLibrary.logAssetError($"<e>AssetLibrary<{typeof (T).Name}></e>: Interface missing for <e>{pKey}</e>", pAsset.id);
        goto label_4;
    }
  }

  public string getEditorPathForSave() => $"{Application.dataPath}/Resources/{this.file_path}.json";

  public void saveToFile(string pPath = "units.json")
  {
    string str = $"{Application.streamingAssetsPath}/modules/core/{pPath}";
  }

  protected bool checkSpriteExists(string pVariableID, string pPath, Asset pAsset)
  {
    if (string.IsNullOrEmpty(pPath) || this.hasSpriteInResourcesDebug(pPath))
      return true;
    BaseAssetLibrary.logAssetError($"{this.id}: <e>{pVariableID}</e> doesn't exist for <e>{pAsset.id}</e> at ", pPath);
    return false;
  }

  protected static TA[] a<TA>(params TA[] pArgs) => Toolbox.a<TA>(pArgs);

  protected static List<TL> l<TL>(params TL[] pArgs) => Toolbox.l<TL>(pArgs);

  protected static HashSet<TH> h<TH>(params TH[] pArgs) => Toolbox.h<TH>(pArgs);

  public override IEnumerable<Asset> getList() => (IEnumerable<Asset>) this.list;

  public override int total_items => this.list.Count;
}
