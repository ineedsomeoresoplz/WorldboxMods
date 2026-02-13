// Decompiled with JetBrains decompiler
// Type: DynamicSpritesConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine.Device;

#nullable disable
public static class DynamicSpritesConfig
{
  public const int EDGE_PIXEL = 1;
  public const int TEXTURE_SIZE_512 = 512 /*0x0200*/;
  public const int TEXTURE_SIZE_1024 = 1024 /*0x0400*/;
  public const int TEXTURE_SIZE_2048 = 2048 /*0x0800*/;
  private static int _cached_texture_size;

  public static int texture_size
  {
    get
    {
      if (DynamicSpritesConfig._cached_texture_size == 0)
        DynamicSpritesConfig._cached_texture_size = DynamicSpritesConfig.calculateTargetTextureSize();
      return DynamicSpritesConfig._cached_texture_size;
    }
  }

  private static int calculateTargetTextureSize()
  {
    int targetTextureSize = !Config.isMobile ? 1024 /*0x0400*/ : 512 /*0x0200*/;
    int maxTextureSize = SystemInfo.maxTextureSize;
    if (maxTextureSize < targetTextureSize)
      targetTextureSize = maxTextureSize;
    return targetTextureSize;
  }
}
