// Decompiled with JetBrains decompiler
// Type: BookManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BookManager : CoreSystemManager<Book, BookData>
{
  public const string COVER_PATH = "books/book_covers/";
  public const string ICON_PATH = "books/book_icons/";
  private static Sprite[] _cached_covers;

  public BookManager() => this.type_id = "book";

  public Book generateNewBook(Actor pActor)
  {
    City city = pActor.getCity();
    Building buildingWithBookSlot = city.getBuildingWithBookSlot();
    if (buildingWithBookSlot == null)
      return (Book) null;
    Book pBook = this.newBook(pActor);
    if (pBook == null)
      return (Book) null;
    ++World.world.game_stats.data.booksWritten;
    ++World.world.map_stats.booksWritten;
    pActor.changeHappiness("wrote_book");
    buildingWithBookSlot.addBook(pBook);
    city.setStatusDirty();
    return pBook;
  }

  public string getNewCoverPath()
  {
    if (BookManager._cached_covers == null)
      BookManager._cached_covers = SpriteTextureLoader.getSpriteList("books/book_covers/");
    return ((Object) BookManager._cached_covers.GetRandom<Sprite>()).name;
  }

  private BookTypeAsset getPossibleBookType(Actor pActor)
  {
    using (ListPool<BookTypeAsset> list = new ListPool<BookTypeAsset>(AssetManager.book_types.list.Count * 5))
    {
      for (int index1 = 0; index1 < AssetManager.book_types.list.Count; ++index1)
      {
        BookTypeAsset pAsset = AssetManager.book_types.list[index1];
        if (pAsset.requirement_check == null || pAsset.requirement_check(pActor, pAsset))
        {
          int num1 = pAsset.writing_rate;
          if (pAsset.rate_calc != null)
            num1 = pAsset.rate_calc(pActor, pAsset);
          int num2 = Mathf.Min(num1, 10);
          for (int index2 = 0; index2 < num2; ++index2)
            list.Add(pAsset);
        }
      }
      return list.Count == 0 ? (BookTypeAsset) null : list.GetRandom<BookTypeAsset>();
    }
  }

  public Book newBook(Actor pActor)
  {
    BookTypeAsset possibleBookType = this.getPossibleBookType(pActor);
    if (possibleBookType == null)
      return (Book) null;
    Book book = this.newObject();
    ActorTrait bookTrait = this.getBookTrait(pActor);
    LanguageTrait traitForBook1 = pActor.language?.getTraitForBook();
    ReligionTrait traitForBook2 = pActor.religion?.getTraitForBook();
    CultureTrait traitForBook3 = pActor.culture?.getTraitForBook();
    book.newBook(pActor, possibleBookType, bookTrait, traitForBook3, traitForBook1, traitForBook2);
    return book;
  }

  private ActorTrait getBookTrait(Actor pActor)
  {
    IReadOnlyCollection<ActorTrait> traits = pActor.getTraits();
    using (ListPool<ActorTrait> list = new ListPool<ActorTrait>(traits.Count))
    {
      foreach (ActorTrait actorTrait in (IEnumerable<ActorTrait>) traits)
      {
        if (actorTrait.group_id == "mind")
          list.Add(actorTrait);
      }
      return list.Count == 0 ? (ActorTrait) null : list.GetRandom<ActorTrait>();
    }
  }

  public void copyBook(Book pBook)
  {
  }

  public void burnBook(Book pBook)
  {
    pBook.getLanguage()?.books.setDirty();
    pBook.getCulture()?.books.setDirty();
    pBook.getReligion()?.books.setDirty();
    this.removeObject(pBook);
  }

  public override void removeObject(Book pObject)
  {
    ++World.world.game_stats.data.booksBurnt;
    ++World.world.map_stats.booksBurnt;
    base.removeObject(pObject);
  }
}
