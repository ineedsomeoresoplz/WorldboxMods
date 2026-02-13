// Decompiled with JetBrains decompiler
// Type: BooksContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class BooksContainer : BooksElement
{
  private ObjectPoolGenericMono<CultureBookButton> _pool_books;
  private CultureBookButton _prefab_book;
  [SerializeField]
  private Transform _title;
  [SerializeField]
  private Transform _books_grid;

  protected override void Awake()
  {
    this._prefab_book = Resources.Load<CultureBookButton>("ui/PrefabBook");
    this._pool_books = new ObjectPoolGenericMono<CultureBookButton>(this._prefab_book, this._books_grid);
    base.Awake();
  }

  protected override IEnumerator showContent()
  {
    // ISSUE: unable to decompile the method.
  }

  public void loadBookButton(long pBookID) => this._pool_books.getNext().load(pBookID);

  protected override void clear()
  {
    this._pool_books.clear();
    if (Object.op_Inequality((Object) this._title, (Object) null))
      ((Component) this._title).gameObject.SetActive(false);
    ((Component) this._books_grid).gameObject.SetActive(false);
    base.clear();
  }

  protected override void clearInitial()
  {
    for (int index = 0; index < this._books_grid.childCount; ++index)
      Object.Destroy((Object) ((Component) this._books_grid.GetChild(index)).gameObject);
    base.clearInitial();
  }
}
