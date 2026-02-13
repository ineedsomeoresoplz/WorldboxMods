// Decompiled with JetBrains decompiler
// Type: IWorldBoxAd
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public interface IWorldBoxAd
{
  void Reset();

  void RequestAd();

  void KillAd();

  bool IsReady();

  void ShowAd();

  bool HasAd();

  bool IsInitialized();

  string GetProviderName();

  string GetColor();

  Action adResetCallback { get; set; }

  Action adFailedCallback { get; set; }

  Action adFinishedCallback { get; set; }

  Action adStartedCallback { get; set; }

  Action<string> logger { get; set; }
}
