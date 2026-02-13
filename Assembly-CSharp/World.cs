// Decompiled with JetBrains decompiler
// Type: World
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Runtime.CompilerServices;

#nullable disable
public static class World
{
  public static MapBox world
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get => MapBox.instance;
  }

  public static WorldAgeAsset world_era
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get
    {
      return MapBox.instance.era_manager.getCurrentAge();
    }
  }
}
