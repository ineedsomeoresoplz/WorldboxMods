// Decompiled with JetBrains decompiler
// Type: OnomasticsCache
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public static class OnomasticsCache
{
  [ThreadStatic]
  private static Dictionary<string, OnomasticsData> _cache;

  public static OnomasticsData getOriginalData(string pShortTemplate)
  {
    if (OnomasticsCache._cache == null)
      OnomasticsCache._cache = new Dictionary<string, OnomasticsData>();
    OnomasticsData originalData;
    if (!OnomasticsCache._cache.TryGetValue(pShortTemplate, out originalData))
    {
      originalData = new OnomasticsData();
      originalData.loadFromShortTemplate(pShortTemplate);
      OnomasticsCache._cache.Add(pShortTemplate, originalData);
    }
    return originalData;
  }
}
