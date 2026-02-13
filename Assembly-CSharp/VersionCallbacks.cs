// Decompiled with JetBrains decompiler
// Type: VersionCallbacks
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;
using System;

#nullable disable
[ObfuscateLiterals]
internal static class VersionCallbacks
{
  internal static Action<string> versionCallbacks;
  internal static float timer = 0.0f;
  internal static string versionCheck = string.Empty;

  public static void init()
  {
    VersionCallbacks.versionCheck = VersionCheck._vsCheck;
    if (string.IsNullOrEmpty(VersionCallbacks.versionCheck) || VersionCallbacks.versionCheck.Split('.', StringSplitOptions.None).Length == 3 || VersionCallbacks.versionCallbacks != null && VersionCallbacks.versionCallbacks.GetInvocationList().Length != 0)
      return;
    TestingCB.init();
  }

  internal static void updateVC(float pElapsed)
  {
    VersionCallbacks.timer -= pElapsed;
    if ((double) VersionCallbacks.timer > 0.0)
      return;
    VersionCallbacks.timer = 0.0f;
    try
    {
      VersionCallbacks.init();
      if (!string.IsNullOrEmpty(VersionCallbacks.versionCheck))
      {
        Action<string> versionCallbacks = VersionCallbacks.versionCallbacks;
        if (versionCallbacks != null)
          versionCallbacks(VersionCallbacks.versionCheck);
      }
      if (VersionCallbacks.versionCheck.Split('.', StringSplitOptions.None).Length == 3)
        return;
      VersionCallbacks.timer = Randy.randomFloat(300f, 600f);
    }
    catch (Exception ex)
    {
    }
  }
}
