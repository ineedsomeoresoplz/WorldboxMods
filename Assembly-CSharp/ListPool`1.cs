// Decompiled with JetBrains decompiler
// Type: ListPool`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

#nullable enable
[Serializable]
public sealed class ListPool<T> : 
  IList<
  #nullable disable
  T>,
  ICollection<T>,
  IEnumerable<T>,
  IEnumerable,
  IList,
  ICollection,
  IReadOnlyList<T>,
  IReadOnlyCollection<T>,
  IDisposable
{
  private const int MinimumCapacity = 32 /*0x20*/;
  private T[] _items;
  [NonSerialized]
  private 
  #nullable enable
  object? _syncRoot;
  private static readonly 
  #nullable disable
  ArrayPool<T> _arrayPool = ArrayPool<T>.Shared;
  private static readonly bool _should_clean = !typeof (T).IsValueType && typeof (string) != typeof (T);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public ListPool() => this._items = ListPool<T>._arrayPool.Rent(32 /*0x20*/);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public ListPool(int capacity)
  {
    this._items = ListPool<T>._arrayPool.Rent(capacity < 32 /*0x20*/ ? 32 /*0x20*/ : capacity);
  }

  public ListPool(ICollection<T> collection)
  {
    if (collection == null)
      throw new ArgumentNullException(nameof (collection));
    T[] array = ListPool<T>._arrayPool.Rent(collection.Count > 32 /*0x20*/ ? collection.Count : 32 /*0x20*/);
    collection.CopyTo(array, 0);
    this._items = array;
    this.Count = collection.Count;
  }

  public ListPool(IEnumerable<T> source)
  {
    int num = source != null ? source.Count<T>() : throw new ArgumentNullException(nameof (source));
    this._items = ListPool<T>._arrayPool.Rent(num > 32 /*0x20*/ ? num : 32 /*0x20*/);
    T[] items = this._items;
    this.Count = 0;
    int index = 0;
    using (IEnumerator<T> enumerator = source.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        if (index < items.Length)
        {
          items[index] = enumerator.Current;
          ++index;
        }
        else
        {
          this.Count = index;
          this.AddWithResize(enumerator.Current);
          ++index;
          items = this._items;
        }
      }
      this.Count = index;
    }
  }

  public ListPool(T[] source)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    int num = source.Length > 32 /*0x20*/ ? source.Length : 32 /*0x20*/;
    T[] objArray = ListPool<T>._arrayPool.Rent(num);
    source.CopyTo((Array) objArray, 0);
    this._items = objArray;
    this.Count = source.Length;
  }

  public ListPool(ReadOnlySpan<T> source)
  {
    int num = source.Length > 32 /*0x20*/ ? source.Length : 32 /*0x20*/;
    T[] objArray = ListPool<T>._arrayPool.Rent(num);
    source.CopyTo(Span<T>.op_Implicit(objArray));
    this._items = objArray;
    this.Count = source.Length;
  }

  public int Capacity => this._items.Length;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void Dispose()
  {
    if (ListPool<T>._should_clean)
      this.Clear();
    this.Count = 0;
    ListPool<T>._arrayPool.Return(this._items, false);
  }

  int ICollection.Count => this.Count;

  bool IList.IsFixedSize => false;

  bool ICollection.IsSynchronized => false;

  bool IList.IsReadOnly => false;

  object ICollection.SyncRoot
  {
    get
    {
      if (this._syncRoot == null)
        Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), (object) null);
      return this._syncRoot;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  int IList.Add(object item)
  {
    if (!(item is T obj))
      throw new ArgumentException($"Wrong value type. Expected {typeof (T)}, got: '{item}'.", nameof (item));
    this.Add(obj);
    return this.Count - 1;
  }

  bool IList.Contains(object item)
  {
    return item is T obj ? this.Contains(obj) : throw new ArgumentException($"Wrong value type. Expected {typeof (T)}, got: '{item}'.", nameof (item));
  }

  int IList.IndexOf(object item)
  {
    return item is T obj ? this.IndexOf(obj) : throw new ArgumentException($"Wrong value type. Expected {typeof (T)}, got: '{item}'.", nameof (item));
  }

  void IList.Remove(object item)
  {
    if (item is T obj)
      this.Remove(obj);
    else if (item != null)
      throw new ArgumentException($"Wrong value type. Expected {typeof (T)}, got: '{item}'.", nameof (item));
  }

  void IList.Insert(int index, object item)
  {
    if (!(item is T obj))
      throw new ArgumentException($"Wrong value type. Expected {typeof (T)}, got: '{item}'.", nameof (item));
    this.Insert(index, obj);
  }

  void ICollection.CopyTo(Array array, int arrayIndex)
  {
    Array.Copy((Array) this._items, 0, array, arrayIndex, this.Count);
  }

  object IList.this[int index]
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] [return: MaybeNull] get
    {
      return index < this.Count ? (object) this._items[index] : throw new IndexOutOfRangeException(nameof (index));
    }
    set
    {
      if (index >= this.Count)
        throw new IndexOutOfRangeException(nameof (index));
      this._items[index] = value is T obj ? obj : throw new ArgumentException($"Wrong value type. Expected {typeof (T)}, got: '{value}'.", nameof (value));
    }
  }

  public int Count { get; private set; }

  public bool IsReadOnly => false;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void Add(T item)
  {
    T[] items = this._items;
    int count = this.Count;
    if (count < items.Length)
    {
      items[count] = item;
      this.Count = count + 1;
    }
    else
      this.AddWithResize(item);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void Clear()
  {
    if (this.Count <= 0)
      return;
    Array.Clear((Array) this._items, 0, this.Count);
    this.Count = 0;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void Clear(int pAfterIndex)
  {
    if (this.Count <= 0 || pAfterIndex >= this.Count)
      return;
    Array.Clear((Array) this._items, pAfterIndex, this.Count - pAfterIndex);
    this.Count = pAfterIndex;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool Contains(T item) => this.IndexOf(item) > -1;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int IndexOf(T item) => Array.IndexOf<T>(this._items, item, 0, this.Count);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void CopyTo(T[] array, int arrayIndex)
  {
    Array.Copy((Array) this._items, 0, (Array) array, arrayIndex, this.Count);
  }

  public bool Remove(T item)
  {
    if ((object) item == null)
      return false;
    int index = this.IndexOf(item);
    if (index == -1)
      return false;
    this.RemoveAt(index);
    return true;
  }

  public void Insert(int index, T item)
  {
    int count = this.Count;
    T[] items = this._items;
    if (items.Length == count)
    {
      this.EnsureCapacity(count * 2);
      items = this._items;
    }
    if (index < count)
    {
      Array.Copy((Array) items, index, (Array) items, index + 1, count - index);
      items[index] = item;
      ++this.Count;
    }
    else
    {
      if (index != count)
        throw new IndexOutOfRangeException(nameof (index));
      items[index] = item;
      ++this.Count;
    }
  }

  public void RemoveAt(int index)
  {
    int count = this.Count;
    T[] items = this._items;
    if (index >= count)
      throw new IndexOutOfRangeException(nameof (index));
    int index1 = count - 1;
    Array.Copy((Array) items, index + 1, (Array) items, index, index1 - index);
    if (ListPool<T>._should_clean)
      items[index1] = default (T);
    this.Count = index1;
  }

  public int RemoveAll(Predicate<T> match)
  {
    int count = this.Count;
    T[] items = this._items;
    int index1 = 0;
    while (index1 < count && !match(items[index1]))
      ++index1;
    if (index1 >= count)
      return 0;
    int index2 = index1 + 1;
    while (index2 < count)
    {
      while (index2 < count && match(items[index2]))
        ++index2;
      if (index2 < count)
        items[index1++] = items[index2++];
    }
    if (ListPool<T>._should_clean)
      Array.Clear((Array) items, index1, count - index1);
    int num = count - index1;
    this.Count = index1;
    return num;
  }

  public T this[int index]
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] [return: MaybeNull] get
    {
      return index < this.Count ? this._items[index] : throw new IndexOutOfRangeException(nameof (index));
    }
    set
    {
      if (index >= this.Count)
        throw new IndexOutOfRangeException(nameof (index));
      this._items[index] = value;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  IEnumerator<T> IEnumerable<T>.GetEnumerator()
  {
    return (IEnumerator<T>) new ListPool<T>.Enumerator(this._items, this.Count);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  IEnumerator IEnumerable.GetEnumerator()
  {
    return (IEnumerator) new ListPool<T>.Enumerator(this._items, this.Count);
  }

  public void AddRange(Span<T> items)
  {
    int count = this.Count;
    T[] items1 = this._items;
    if (items1.Length - items.Length - count < 0)
    {
      this.EnsureCapacity(items1.Length + items.Length);
      items1 = this._items;
    }
    items.CopyTo(MemoryExtensions.AsSpan<T>(items1).Slice(count));
    this.Count += items.Length;
  }

  public void AddRange(ReadOnlySpan<T> items)
  {
    int count = this.Count;
    T[] items1 = this._items;
    if (items1.Length - items.Length - count < 0)
    {
      this.EnsureCapacity(items1.Length + items.Length);
      items1 = this._items;
    }
    items.CopyTo(MemoryExtensions.AsSpan<T>(items1).Slice(count));
    this.Count += items.Length;
  }

  public void AddRange(T[] items)
  {
    int count = this.Count;
    T[] items1 = this._items;
    if (items1.Length - items.Length - count < 0)
    {
      this.EnsureCapacity(items1.Length + items.Length);
      items1 = this._items;
    }
    Array.Copy((Array) items, 0, (Array) items1, count, items.Length);
    this.Count += items.Length;
  }

  public void AddRange(IEnumerable<T> items)
  {
    int count = this.Count;
    T[] items1 = this._items;
    if (items is ICollection<T> objs)
    {
      if (items1.Length - objs.Count - count < 0)
      {
        this.EnsureCapacity(items1.Length + objs.Count);
        items1 = this._items;
      }
      objs.CopyTo(items1, count);
      this.Count += objs.Count;
    }
    else
    {
      foreach (T obj in items)
      {
        if (count < items1.Length)
        {
          items1[count] = obj;
          ++count;
        }
        else
        {
          this.Count = count;
          this.AddWithResize(obj);
          ++count;
          items1 = this._items;
        }
      }
      this.Count = count;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<T> AsSpan() => MemoryExtensions.AsSpan<T>(this._items, 0, this.Count);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Memory<T> AsMemory() => MemoryExtensions.AsMemory<T>(this._items, 0, this.Count);

  [MethodImpl(MethodImplOptions.NoInlining)]
  private void AddWithResize(T item)
  {
    ArrayPool<T> arrayPool = ListPool<T>._arrayPool;
    T[] items = this._items;
    T[] destinationArray = arrayPool.Rent(items.Length * 2);
    int length = items.Length;
    Array.Copy((Array) items, 0, (Array) destinationArray, 0, length);
    destinationArray[length] = item;
    this._items = destinationArray;
    this.Count = length + 1;
    arrayPool.Return(items, ListPool<T>._should_clean);
  }

  public void EnsureCapacity(int capacity)
  {
    if (capacity <= this.Capacity)
      return;
    ArrayPool<T> arrayPool = ListPool<T>._arrayPool;
    T[] destinationArray = arrayPool.Rent(capacity);
    T[] items = this._items;
    Array.Copy((Array) items, 0, (Array) destinationArray, 0, items.Length);
    this._items = destinationArray;
    arrayPool.Return(items, ListPool<T>._should_clean);
  }

  public T[] GetRawBuffer() => this._items;

  public void SetOffsetManually(int offset) => this.Count = offset;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public ListPool<T>.Enumerator GetEnumerator()
  {
    return new ListPool<T>.Enumerator(this._items, this.Count);
  }

  public void Sort() => this.Sort(0, this.Count, (IComparer<T>) null);

  public void Sort(IComparer<T> comparer) => this.Sort(0, this.Count, comparer);

  public void Sort(int index, int count, IComparer<T> comparer)
  {
    Array.Sort<T>(this._items, index, count, comparer);
  }

  public void Sort(Comparison<T> comparison)
  {
    Array.Sort<T>(this._items, 0, this.Count, (IComparer<T>) Comparer<T>.Create(comparison));
  }

  public void Reverse() => Array.Reverse<T>(this._items, 0, this.Count);

  public void Reverse(int index, int count) => Array.Reverse<T>(this._items, index, count);

  [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
  public struct Enumerator(T[] source, int itemsCount) : IEnumerator<T>, IEnumerator, IDisposable
  {
    private readonly T[] _source = source;
    private readonly int _itemsCount = itemsCount;
    private int _index = -1;

    public readonly ref T Current
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] [return: MaybeNull] get
      {
        return ref this._source[this._index];
      }
    }

    readonly T IEnumerator<T>.Current
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] [return: MaybeNull] get
      {
        return this._source[this._index];
      }
    }

    readonly 
    #nullable enable
    object? IEnumerator.Current
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] [return: MaybeNull] get
      {
        return (object) this._source[this._index];
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool MoveNext() => ++this._index < this._itemsCount;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reset() => this._index = -1;

    public readonly void Dispose()
    {
    }
  }
}
