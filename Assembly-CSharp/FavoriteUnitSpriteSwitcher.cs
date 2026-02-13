// Decompiled with JetBrains decompiler
// Type: FavoriteUnitSpriteSwitcher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class FavoriteUnitSpriteSwitcher : SpriteSwitcher
{
  protected override bool hasAny()
  {
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
    {
      if (unit.isFavorite())
        return true;
    }
    return false;
  }
}
