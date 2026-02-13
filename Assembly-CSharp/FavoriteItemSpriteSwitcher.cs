// Decompiled with JetBrains decompiler
// Type: FavoriteItemSpriteSwitcher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class FavoriteItemSpriteSwitcher : SpriteSwitcher
{
  protected override bool hasAny()
  {
    foreach (CoreSystemObject<ItemData> coreSystemObject in (CoreSystemManager<Item, ItemData>) World.world.items)
    {
      if (coreSystemObject.isFavorite())
        return true;
    }
    return false;
  }
}
