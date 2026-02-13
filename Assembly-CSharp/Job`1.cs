// Decompiled with JetBrains decompiler
// Type: Job`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class Job<T>
{
  public JobUpdater job_updater;
  public ObjectContainer<T> container;
  public string id;
  public double time_benchmark;
  public int counter;
  public int random_tick_skips;
  public int current_skips;
}
