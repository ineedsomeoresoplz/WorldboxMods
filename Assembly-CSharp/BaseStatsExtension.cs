// Decompiled with JetBrains decompiler
// Type: BaseStatsExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public static class BaseStatsExtension
{
  public static bool isEmpty(this BaseStats pBaseStats)
  {
    if (pBaseStats == null)
      return true;
    return !pBaseStats.hasTags() && !pBaseStats.hasStats();
  }
}
