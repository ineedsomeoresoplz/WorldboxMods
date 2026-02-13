// Decompiled with JetBrains decompiler
// Type: ListAccessHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
internal static class ListAccessHelper
{
  public static readonly int ItemsOffset = 0;
  public static readonly int SizeOffset = 8;
  public static readonly int VersionOffset = 12;

  internal record ListDataHelper<T>()
  {
    public 
    #nullable disable
    T[] _items;
    public int _size;
    public int _version;

    [CompilerGenerated]
    protected virtual bool PrintMembers(
    #nullable enable
    StringBuilder builder)
    {
      RuntimeHelpers.EnsureSufficientExecutionStack();
      builder.Append("_items = ");
      builder.Append((object) this._items);
      builder.Append(", _size = ");
      builder.Append(this._size.ToString());
      builder.Append(", _version = ");
      builder.Append(this._version.ToString());
      return true;
    }

    [CompilerGenerated]
    public override int GetHashCode()
    {
      return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<T[]>.Default.GetHashCode(this._items)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this._size)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this._version);
    }

    [CompilerGenerated]
    public virtual bool Equals(ListAccessHelper.ListDataHelper<
    #nullable disable
    T>
    #nullable enable
    ? other)
    {
      if ((object) this == (object) other)
        return true;
      return (object) other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<T[]>.Default.Equals(this._items, other._items) && EqualityComparer<int>.Default.Equals(this._size, other._size) && EqualityComparer<int>.Default.Equals(this._version, other._version);
    }

    [CompilerGenerated]
    protected ListDataHelper(ListAccessHelper.ListDataHelper<
    #nullable disable
    T> original)
    {
      this._items = original._items;
      this._size = original._size;
      this._version = original._version;
    }
  }
}
