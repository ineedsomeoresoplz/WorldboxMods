// Decompiled with JetBrains decompiler
// Type: BooksElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BooksElement : WindowMetaElementBase
{
  protected List<long> books;
  private IBooksWindow _books_window;

  protected override void Awake()
  {
    this._books_window = ((Component) this).GetComponentInParent<IBooksWindow>();
    base.Awake();
  }

  protected override void OnEnable()
  {
    this.books = this._books_window.getBooks();
    base.OnEnable();
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    this.books = (List<long>) null;
  }

  public override bool checkRefreshWindow()
  {
    if (this.books != null)
    {
      foreach (long book in this.books)
      {
        if (World.world.books.get(book).isRekt())
          return true;
      }
    }
    return base.checkRefreshWindow();
  }
}
