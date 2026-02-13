// Decompiled with JetBrains decompiler
// Type: StorageBooks
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class StorageBooks : IDisposable
{
  public List<long> list_books = new List<long>();

  public void Dispose()
  {
    this.list_books.Clear();
    this.list_books = (List<long>) null;
  }

  public void addBook(Book pBook) => this.list_books.Add(pBook.id);

  public bool hasAny() => this.list_books.Count > 0;

  public int totalBooks() => this.list_books.Count;
}
