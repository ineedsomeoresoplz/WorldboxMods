// Decompiled with JetBrains decompiler
// Type: BehTryToRead
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehTryToRead : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    if (!pActor.city.hasBooksToRead(pActor))
      return BehResult.Stop;
    Book book;
    if (pActor.hasTag("can_read_any_book"))
    {
      book = pActor.city.getRandomBook();
    }
    else
    {
      if (!pActor.hasLanguage() || !pActor.city.hasBooksOfLanguage(pActor.language))
        return BehResult.Stop;
      book = pActor.city.getRandomBookOfLanguage(pActor.language);
    }
    if (book == null)
      return BehResult.Stop;
    book.readIt();
    pActor.beh_book_target = book;
    return BehResult.Continue;
  }
}
