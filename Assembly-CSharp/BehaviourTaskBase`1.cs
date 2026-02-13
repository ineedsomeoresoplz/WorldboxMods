// Decompiled with JetBrains decompiler
// Type: BehaviourTaskBase`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
[Serializable]
public class BehaviourTaskBase<T> : BehaviourElementAI, ILocalizedAsset where T : BehaviourElementAI
{
  [DefaultValue(1f)]
  public float single_interval = 1f;
  public float single_interval_random;
  public List<T> list = new List<T>();
  public T task_verifier;
  public bool has_verifier;
  [DefaultValue("")]
  public string locale_key = string.Empty;
  public bool debug_flag;

  [DefaultValue(true)]
  protected virtual bool has_locales => true;

  protected virtual string locale_key_prefix
  {
    get => throw new NotImplementedException(this.GetType().Name);
  }

  public BehaviourTaskBase() => this.create();

  public T get(int pIndex) => this.list[pIndex];

  public string getLocaleID()
  {
    if (!this.has_locales)
      return (string) null;
    return string.IsNullOrEmpty(this.locale_key) ? $"{this.locale_key_prefix}_{this.id}" : this.locale_key;
  }

  public string getLocalizedText() => !this.has_locales ? "???" : this.getLocaleID().Localize();

  public void addRepeatActions(int pIndexAmount, int pHowManyTimes)
  {
    List<T> collection = new List<T>();
    int count = this.list.Count;
    for (int index1 = 0; index1 < pHowManyTimes; ++index1)
    {
      for (int index2 = 0; index2 < pIndexAmount; ++index2)
      {
        T obj = this.list[count - (pIndexAmount - index2)];
        collection.Add(obj);
      }
    }
    this.list.AddRange((IEnumerable<T>) collection);
  }

  public void addBeh(T pAction)
  {
    pAction.id = pAction.GetType().ToString();
    pAction.id = pAction.id.Replace("ai.behaviours.", "");
    this.list.Add(pAction);
    pAction.create();
  }

  public void addTaskVerifier(T pAction)
  {
    this.task_verifier = pAction;
    this.has_verifier = true;
  }
}
