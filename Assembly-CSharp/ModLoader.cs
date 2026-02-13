// Decompiled with JetBrains decompiler
// Type: ModLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ModLoader : MonoBehaviour
{
  private bool initialized;
  private static List<string> modsLoaded = new List<string>();
  private const string MODS_FOLDER = "mods";

  public void Update()
  {
    if (!Config.game_loaded || !Config.experimental_mode || this.initialized)
      return;
    this.initialized = true;
    this.Initialize();
    ((Behaviour) this).enabled = false;
  }

  internal static List<string> getModsLoaded() => ModLoader.modsLoaded;

  public void Initialize()
  {
    // ISSUE: unable to decompile the method.
  }
}
