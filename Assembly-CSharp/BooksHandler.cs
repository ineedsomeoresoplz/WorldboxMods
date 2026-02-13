// Decompiled with JetBrains decompiler
// Type: BooksHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class BooksHandler
{
  private readonly List<long> _books = new List<long>();
  private bool _books_dirty = true;
  private Culture _culture;
  private Language _language;
  private Religion _religion;

  public void setMeta(Culture pCulture = null, Language pLanguage = null, Religion pReligion = null)
  {
    this._culture = pCulture;
    this._language = pLanguage;
    this._religion = pReligion;
    this.setDirty();
  }

  public List<long> getList()
  {
    this.checkBooks();
    return this._books;
  }

  public int count() => this.getList().Count;

  public bool hasBooks() => this.count() > 0;

  public void setDirty() => this._books_dirty = true;

  private void checkBooks()
  {
    if (!this._books_dirty)
      return;
    this._books_dirty = false;
    this._books.Clear();
    foreach (Book book in (CoreSystemManager<Book, BookData>) World.world.books)
    {
      if (this._culture != null && book.getCulture() == this._culture)
        this._books.Add(book.id);
      else if (this._language != null && book.getLanguage() == this._language)
        this._books.Add(book.id);
      else if (this._religion != null && book.getReligion() == this._religion)
        this._books.Add(book.id);
    }
  }

  public void clear()
  {
    this.setDirty();
    this._culture = (Culture) null;
    this._language = (Language) null;
    this._religion = (Religion) null;
  }
}
