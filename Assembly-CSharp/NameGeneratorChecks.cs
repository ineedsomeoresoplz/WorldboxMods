// Decompiled with JetBrains decompiler
// Type: NameGeneratorChecks
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public static class NameGeneratorChecks
{
  public static bool hasLatinKing(Actor pActor)
  {
    return NameGeneratorChecks.hasCivKingdom(pActor) && pActor.kingdom.hasKing() && Toolbox.isFirstLatin(pActor.kingdom.king.getName());
  }

  public static bool hasEnemyLatinKing(Actor pActor)
  {
    // ISSUE: unable to decompile the method.
  }

  public static bool hasCivKingdom(Actor pActor)
  {
    return pActor != null && pActor.kingdom != null && pActor.isKingdomCiv();
  }

  public static bool hasLatinKingdom(Actor pActor)
  {
    return NameGeneratorChecks.hasCivKingdom(pActor) && Toolbox.isFirstLatin(pActor.kingdom.name);
  }

  public static bool hasEnemyLatinKingdom(Actor pActor)
  {
    // ISSUE: unable to decompile the method.
  }

  public static bool hasLatinCity(Actor pActor)
  {
    return pActor != null && pActor.hasCity() && Toolbox.isFirstLatin(pActor.city.name);
  }

  public static bool hasLatinCulture(Actor pActor)
  {
    return pActor != null && pActor.hasCulture() && Toolbox.isFirstLatin(pActor.culture.name);
  }
}
