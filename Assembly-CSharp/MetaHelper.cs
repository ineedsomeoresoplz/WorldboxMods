// Decompiled with JetBrains decompiler
// Type: MetaHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public static class MetaHelper
{
  public static void addRandomTrait<TTrait>(
    ITraitsOwner<TTrait> pMetaObject,
    BaseTraitLibrary<TTrait> pLibrary)
    where TTrait : BaseTrait<TTrait>
  {
    int pMinInclusive = 1;
    int pMaxExclusive = 3;
    if (WorldLawLibrary.world_law_glitched_noosphere.isEnabled())
    {
      pMinInclusive = 3;
      pMaxExclusive = 6;
    }
    int num = Randy.randomInt(pMinInclusive, pMaxExclusive);
    for (int index = 0; index < num; ++index)
    {
      TTrait randomSpawnTrait = pLibrary.getRandomSpawnTrait();
      if (randomSpawnTrait.isAvailable())
        pMetaObject.addTrait(randomSpawnTrait, true);
    }
  }
}
