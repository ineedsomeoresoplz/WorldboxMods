// Decompiled with JetBrains decompiler
// Type: BenchmarkGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class BenchmarkGroup
{
  public string id;
  public Dictionary<string, ToolBenchmarkData> dict_data = new Dictionary<string, ToolBenchmarkData>();

  public void flatten()
  {
    foreach (ToolBenchmarkData toolBenchmarkData in this.dict_data.Values)
      toolBenchmarkData.end(0.0);
  }
}
