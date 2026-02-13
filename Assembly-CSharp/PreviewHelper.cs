// Decompiled with JetBrains decompiler
// Type: PreviewHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.IO;
using UnityEngine;

#nullable disable
public static class PreviewHelper
{
  public static Sprite loadWorkshopMapPreview()
  {
    string pngPreviewPath = SaveManager.generatePngPreviewPath(SaveManager.currentWorkshopMapData.main_path);
    if (string.IsNullOrEmpty(pngPreviewPath) || !File.Exists(pngPreviewPath))
      return (Sprite) null;
    byte[] numArray = File.ReadAllBytes(pngPreviewPath);
    Texture2D texture2D = new Texture2D(64 /*0x40*/, 64 /*0x40*/);
    return ImageConversion.LoadImage(texture2D, numArray) ? Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float) ((Texture) texture2D).width, (float) ((Texture) texture2D).height), new Vector2(0.5f, 0.5f)) : (Sprite) null;
  }

  public static Sprite getCurrentWorldPreview()
  {
    World.world.redrawMiniMap(true);
    Texture2D texture2D = Toolbox.ScaleTexture(World.world.world_layer.texture, 512 /*0x0200*/, 512 /*0x0200*/);
    return Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float) ((Texture) texture2D).width, (float) ((Texture) texture2D).height), new Vector2(0.0f, 0.0f));
  }

  public static Texture2D convertMapToTexture()
  {
    Texture2D texture1 = World.world.world_layer.texture;
    Texture2D texture2 = new Texture2D(((Texture) texture1).width, ((Texture) texture1).height);
    texture2.SetPixels32(texture1.GetPixels32());
    texture2.Apply();
    return texture2;
  }

  public static int getMaxAdSlots()
  {
    int maxAdSlots = 1;
    if (World.world.game_stats.data.gameLaunches > 10L && World.world.game_stats.data.gameTime > 36000.0)
      maxAdSlots = 3;
    if (World.world.game_stats.data.gameLaunches > 30L && World.world.game_stats.data.gameTime > 72000.0)
      maxAdSlots = 6;
    for (int pSlot = maxAdSlots + 1; pSlot <= 6; ++pSlot)
    {
      if (SaveManager.slotExists(pSlot))
        return 6;
    }
    return maxAdSlots;
  }
}
