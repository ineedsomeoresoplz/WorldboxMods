// Decompiled with JetBrains decompiler
// Type: KingdomOpinion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityPools;

#nullable disable
public class KingdomOpinion : IDisposable
{
  public readonly Dictionary<OpinionAsset, int> results = UnsafeCollectionPool<Dictionary<OpinionAsset, int>, KeyValuePair<OpinionAsset, int>>.Get();
  public int total;
  public Kingdom main;
  public Kingdom target;

  public KingdomOpinion(Kingdom k1, Kingdom k2)
  {
    this.main = k1;
    this.target = k2;
  }

  internal void calculate(Kingdom pMain, Kingdom pTarget, DiplomacyRelation pRelation)
  {
    this.clear();
    foreach (OpinionAsset key in AssetManager.opinion_library.list)
    {
      int num = key.calc(pMain, pTarget);
      this.total += num;
      if (num != 0)
        this.results.Add(key, num);
    }
  }

  private void clear()
  {
    this.total = 0;
    this.results.Clear();
  }

  public void Dispose()
  {
    this.clear();
    UnsafeCollectionPool<Dictionary<OpinionAsset, int>, KeyValuePair<OpinionAsset, int>>.Release(this.results);
    this.main = (Kingdom) null;
    this.target = (Kingdom) null;
  }
}
