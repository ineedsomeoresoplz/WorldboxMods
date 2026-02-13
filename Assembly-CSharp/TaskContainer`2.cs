// Decompiled with JetBrains decompiler
// Type: TaskContainer`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class TaskContainer<TCondition, TSimObject> where TCondition : BehaviourBaseCondition<TSimObject>
{
  [JsonProperty(Order = -1)]
  public string id;
  public Dictionary<TCondition, bool> conditions;
  public bool has_conditions;

  public void addCondition(TCondition pCondition, bool pResult)
  {
    if (this.conditions == null)
      this.conditions = new Dictionary<TCondition, bool>();
    this.conditions.Add(pCondition, pResult);
    this.has_conditions = true;
  }
}
