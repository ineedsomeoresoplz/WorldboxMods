// Decompiled with JetBrains decompiler
// Type: BehaviourElementAI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class BehaviourElementAI : Asset
{
  [NonSerialized]
  internal RateCounter rate_counter_calls;
  [NonSerialized]
  internal RateCounter rate_counter_performance;

  public override void create()
  {
    base.create();
    this.rate_counter_calls = new RateCounter("calls", 1);
    this.rate_counter_performance = new RateCounter("performance", 1);
  }
}
