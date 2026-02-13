// Decompiled with JetBrains decompiler
// Type: EpPathFinding.cs.IntervalHeap`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using C5;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EpPathFinding.cs;

public class IntervalHeap<T> : 
  CollectionValueBase<T>,
  IPriorityQueue<T>,
  IExtensible<T>,
  ICollectionValue<T>,
  IEnumerable<T>,
  IEnumerable,
  IShowable,
  IFormattable
  where T : class
{
  private int stamp;
  private readonly IComparer<T> comparer;
  private readonly IEqualityComparer<T> itemequalityComparer;
  private IntervalHeap<T>.Interval[] heap;
  private int size;

  public virtual EventTypeEnum ListenableEvents => (EventTypeEnum) 15;

  private void SwapFirstWithLast(int cell1, int cell2)
  {
    T first = this.heap[cell1].first;
    this.UpdateFirst(cell1, this.heap[cell2].last);
    this.UpdateLast(cell2, first);
  }

  private void SwapLastWithLast(int cell1, int cell2)
  {
    T last = this.heap[cell2].last;
    this.UpdateLast(cell2, this.heap[cell1].last);
    this.UpdateLast(cell1, last);
  }

  private void SwapFirstWithFirst(int cell1, int cell2)
  {
    T first = this.heap[cell2].first;
    this.UpdateFirst(cell2, this.heap[cell1].first);
    this.UpdateFirst(cell1, first);
  }

  private bool HeapifyMin(int cell)
  {
    bool flag = false;
    if (2 * cell + 1 < this.size && this.comparer.Compare(this.heap[cell].first, this.heap[cell].last) > 0)
    {
      flag = true;
      this.SwapFirstWithLast(cell, cell);
    }
    int index1 = cell;
    int index2 = 2 * cell + 1;
    int index3 = index2 + 1;
    if (2 * index2 < this.size && this.comparer.Compare(this.heap[index2].first, this.heap[index1].first) < 0)
      index1 = index2;
    if (2 * index3 < this.size && this.comparer.Compare(this.heap[index3].first, this.heap[index1].first) < 0)
      index1 = index3;
    if (index1 != cell)
    {
      this.SwapFirstWithFirst(index1, cell);
      this.HeapifyMin(index1);
    }
    return flag;
  }

  private bool HeapifyMax(int cell)
  {
    bool flag1 = false;
    if (2 * cell + 1 < this.size && this.comparer.Compare(this.heap[cell].last, this.heap[cell].first) < 0)
    {
      flag1 = true;
      this.SwapFirstWithLast(cell, cell);
    }
    int index1 = cell;
    int index2 = 2 * cell + 1;
    int index3 = index2 + 1;
    bool flag2 = false;
    if (2 * index2 + 1 < this.size)
    {
      if (this.comparer.Compare(this.heap[index2].last, this.heap[index1].last) > 0)
        index1 = index2;
    }
    else if (2 * index2 + 1 == this.size && this.comparer.Compare(this.heap[index2].first, this.heap[index1].last) > 0)
    {
      index1 = index2;
      flag2 = true;
    }
    if (2 * index3 + 1 < this.size)
    {
      if (this.comparer.Compare(this.heap[index3].last, this.heap[index1].last) > 0)
        index1 = index3;
    }
    else if (2 * index3 + 1 == this.size && this.comparer.Compare(this.heap[index3].first, this.heap[index1].last) > 0)
    {
      index1 = index3;
      flag2 = true;
    }
    if (index1 != cell)
    {
      if (flag2)
        this.SwapFirstWithLast(index1, cell);
      else
        this.SwapLastWithLast(index1, cell);
      this.HeapifyMax(index1);
    }
    return flag1;
  }

  private void BubbleUpMin(int i)
  {
    if (i <= 0)
      return;
    T first1 = this.heap[i].first;
    int num1 = (i + 1) / 2;
    int num2;
    T first2;
    for (; i > 0 && this.comparer.Compare(first1, first2 = this.heap[num2 = (i + 1) / 2 - 1].first) < 0; i = num2)
      this.UpdateFirst(i, first2);
    this.UpdateFirst(i, first1);
  }

  private void BubbleUpMax(int i)
  {
    if (i <= 0)
      return;
    T last1 = this.heap[i].last;
    int num1 = (i + 1) / 2;
    int num2;
    T last2;
    for (; i > 0 && this.comparer.Compare(last1, last2 = this.heap[num2 = (i + 1) / 2 - 1].last) > 0; i = num2)
      this.UpdateLast(i, last2);
    this.UpdateLast(i, last1);
  }

  public IntervalHeap()
    : this(16 /*0x10*/)
  {
  }

  public IntervalHeap(int capacity)
    : this(capacity, (IComparer<T>) System.Collections.Generic.Comparer<T>.Default, C5.EqualityComparer<T>.Default)
  {
  }

  private IntervalHeap(
    int capacity,
    IComparer<T> comparer,
    IEqualityComparer<T> itemequalityComparer)
  {
    this.comparer = comparer ?? throw new NullReferenceException("Item comparer cannot be null");
    this.itemequalityComparer = itemequalityComparer ?? throw new NullReferenceException("Item equality comparer cannot be null");
    int length = 1;
    while (length < capacity)
      length <<= 1;
    this.heap = new IntervalHeap<T>.Interval[length];
  }

  public T FindMin()
  {
    if (this.size == 0)
      throw new NoSuchItemException();
    return this.heap[0].first;
  }

  public T DeleteMin() => this.DeleteMin(out IPriorityQueueHandle<T> _);

  public T FindMax()
  {
    if (this.size == 0)
      throw new NoSuchItemException("Heap is empty");
    return this.size == 1 ? this.heap[0].first : this.heap[0].last;
  }

  public T DeleteMax() => this.DeleteMax(out IPriorityQueueHandle<T> _);

  public IComparer<T> Comparer => this.comparer;

  public void Clear()
  {
    ++this.stamp;
    if (this.size == 0)
      return;
    int num = this.size % 2 == 0 ? this.size / 2 : this.size / 2 + 1;
    IntervalHeap<T>.Interval[] heap = this.heap;
    for (int index = 0; index < num; ++index)
    {
      heap[index].first = default (T);
      heap[index].last = default (T);
    }
    this.size = 0;
  }

  public bool IsReadOnly => false;

  public bool AllowsDuplicates => true;

  public virtual IEqualityComparer<T> EqualityComparer => this.itemequalityComparer;

  public virtual bool DuplicatesByCounting => false;

  public bool Add(T item)
  {
    ++this.stamp;
    if (!this.add(item))
      return false;
    this.raiseItemsAdded(item, 1);
    this.raiseCollectionChanged();
    return true;
  }

  private bool add(T item)
  {
    if (this.size == 0)
    {
      this.size = 1;
      this.UpdateFirst(0, item);
      return true;
    }
    if (this.size == 2 * this.heap.Length)
    {
      IntervalHeap<T>.Interval[] destinationArray = new IntervalHeap<T>.Interval[2 * this.heap.Length];
      Array.Copy((Array) this.heap, (Array) destinationArray, this.heap.Length);
      this.heap = destinationArray;
    }
    if (this.size % 2 == 0)
    {
      int num = this.size / 2;
      int index = (num + 1) / 2 - 1;
      T last = this.heap[index].last;
      if (this.comparer.Compare(item, last) > 0)
      {
        this.UpdateFirst(num, last);
        this.UpdateLast(index, item);
        this.BubbleUpMax(index);
      }
      else
      {
        this.UpdateFirst(num, item);
        if (this.comparer.Compare(item, this.heap[index].first) < 0)
          this.BubbleUpMin(num);
      }
    }
    else
    {
      int index = this.size / 2;
      T first = this.heap[index].first;
      if (this.comparer.Compare(item, first) < 0)
      {
        this.UpdateLast(index, first);
        this.UpdateFirst(index, item);
        this.BubbleUpMin(index);
      }
      else
      {
        this.UpdateLast(index, item);
        this.BubbleUpMax(index);
      }
    }
    ++this.size;
    return true;
  }

  private void UpdateLast(int cell, T item) => this.heap[cell].last = item;

  private void UpdateFirst(int cell, T item) => this.heap[cell].first = item;

  public void AddAll(IEnumerable<T> items)
  {
    ++this.stamp;
    int size = this.size;
    foreach (T obj in items)
      this.add(obj);
    if (this.size == size)
      return;
    if ((this.ActiveEvents & 4) != null)
    {
      foreach (T obj in items)
        this.raiseItemsAdded(obj, 1);
    }
    this.raiseCollectionChanged();
  }

  public virtual bool IsEmpty => this.size == 0;

  public virtual int Count => this.size;

  public virtual Speed CountSpeed => (Speed) 4;

  public virtual T Choose()
  {
    if (this.size == 0)
      throw new NoSuchItemException("Collection is empty");
    return this.heap[0].first;
  }

  public virtual IEnumerator<T> GetEnumerator()
  {
    int mystamp = this.stamp;
    for (int i = 0; i < this.size; ++i)
    {
      if (mystamp != this.stamp)
        throw new CollectionModifiedException();
      yield return i % 2 == 0 ? this.heap[i >> 1].first : this.heap[i >> 1].last;
    }
  }

  private bool Check(int i, T min, T max)
  {
    bool flag = true;
    IntervalHeap<T>.Interval interval = this.heap[i];
    T first = interval.first;
    T last = interval.last;
    if (2 * i + 1 == this.size)
    {
      if (this.comparer.Compare(min, first) > 0)
      {
        Logger.Log($"Cell {i}: parent.first({min}) > first({first})  [size={this.size}]");
        flag = false;
      }
      if (this.comparer.Compare(first, max) > 0)
      {
        Logger.Log($"Cell {i}: first({first}) > parent.last({max})  [size={this.size}]");
        flag = false;
      }
      return flag;
    }
    if (this.comparer.Compare(min, first) > 0)
    {
      Logger.Log($"Cell {i}: parent.first({min}) > first({first})  [size={this.size}]");
      flag = false;
    }
    if (this.comparer.Compare(first, last) > 0)
    {
      Logger.Log($"Cell {i}: first({first}) > last({last})  [size={this.size}]");
      flag = false;
    }
    if (this.comparer.Compare(last, max) > 0)
    {
      Logger.Log($"Cell {i}: last({last}) > parent.last({max})  [size={this.size}]");
      flag = false;
    }
    int i1 = 2 * i + 1;
    int i2 = i1 + 1;
    if (2 * i1 < this.size)
      flag = flag && this.Check(i1, first, last);
    if (2 * i2 < this.size)
      flag = flag && this.Check(i2, first, last);
    return flag;
  }

  public bool Check()
  {
    if (this.size == 0)
      return true;
    return this.size == 1 ? (object) this.heap[0].first != null : this.Check(0, this.heap[0].first, this.heap[0].last);
  }

  public T this[IPriorityQueueHandle<T> handle]
  {
    get
    {
      int cell;
      bool isfirst;
      this.CheckHandle(handle, out cell, out isfirst);
      return !isfirst ? this.heap[cell].last : this.heap[cell].first;
    }
    set => this.Replace(handle, value);
  }

  public bool Find(IPriorityQueueHandle<T> handle, out T item)
  {
    if (!(handle is IntervalHeap<T>.Handle handle1))
    {
      item = default (T);
      return false;
    }
    int index1 = handle1.index;
    int index2 = index1 / 2;
    bool flag = index1 % 2 == 0;
    if (index1 == -1 || index1 >= this.size)
    {
      item = default (T);
      return false;
    }
    if (null != handle1)
    {
      item = default (T);
      return false;
    }
    item = flag ? this.heap[index2].first : this.heap[index2].last;
    return true;
  }

  public bool Add(ref IPriorityQueueHandle<T> handle, T item)
  {
    ++this.stamp;
    IntervalHeap<T>.Handle handle1 = (IntervalHeap<T>.Handle) handle;
    if (handle1 == null)
    {
      IntervalHeap<T>.Handle handle2;
      handle = (IPriorityQueueHandle<T>) (handle2 = new IntervalHeap<T>.Handle());
    }
    else if (handle1.index != -1)
      throw new InvalidPriorityQueueHandleException("Handle not valid for reuse");
    if (!this.add(item))
      return false;
    this.raiseItemsAdded(item, 1);
    this.raiseCollectionChanged();
    return true;
  }

  public T Delete(IPriorityQueueHandle<T> handle)
  {
    ++this.stamp;
    int cell;
    bool isfirst;
    this.CheckHandle(handle, out cell, out isfirst).index = -1;
    int index = (this.size - 1) / 2;
    T obj;
    if (cell == index)
    {
      if (isfirst)
      {
        obj = this.heap[cell].first;
        if (this.size % 2 == 0)
        {
          this.UpdateFirst(cell, this.heap[cell].last);
          this.heap[cell].last = default (T);
        }
        else
          this.heap[cell].first = default (T);
      }
      else
      {
        obj = this.heap[cell].last;
        this.heap[cell].last = default (T);
      }
      --this.size;
    }
    else if (isfirst)
    {
      obj = this.heap[cell].first;
      if (this.size % 2 == 0)
      {
        this.UpdateFirst(cell, this.heap[index].last);
        this.heap[index].last = default (T);
      }
      else
      {
        this.UpdateFirst(cell, this.heap[index].first);
        this.heap[index].first = default (T);
      }
      --this.size;
      if (this.HeapifyMin(cell))
        this.BubbleUpMax(cell);
      else
        this.BubbleUpMin(cell);
    }
    else
    {
      obj = this.heap[cell].last;
      if (this.size % 2 == 0)
      {
        this.UpdateLast(cell, this.heap[index].last);
        this.heap[index].last = default (T);
      }
      else
      {
        this.UpdateLast(cell, this.heap[index].first);
        this.heap[index].first = default (T);
      }
      --this.size;
      if (this.HeapifyMax(cell))
        this.BubbleUpMin(cell);
      else
        this.BubbleUpMax(cell);
    }
    this.raiseItemsRemoved(obj, 1);
    this.raiseCollectionChanged();
    return obj;
  }

  private IntervalHeap<T>.Handle CheckHandle(
    IPriorityQueueHandle<T> handle,
    out int cell,
    out bool isfirst)
  {
    IntervalHeap<T>.Handle handle1 = (IntervalHeap<T>.Handle) handle;
    int index = handle1.index;
    cell = index / 2;
    isfirst = index % 2 == 0;
    if (index == -1 || index >= this.size)
      throw new InvalidPriorityQueueHandleException("Invalid handle, index out of range");
    return null == handle1 ? handle1 : throw new InvalidPriorityQueueHandleException("Invalid handle, doesn't match queue");
  }

  public T Replace(IPriorityQueueHandle<T> handle, T item)
  {
    ++this.stamp;
    int cell;
    bool isfirst;
    this.CheckHandle(handle, out cell, out isfirst);
    if (this.size == 0)
      throw new NoSuchItemException();
    T obj;
    if (isfirst)
    {
      obj = this.heap[cell].first;
      this.heap[cell].first = item;
      if (this.size != 1)
      {
        if (this.size == 2 * cell + 1)
        {
          int index = (cell + 1) / 2 - 1;
          if (this.comparer.Compare(item, this.heap[index].last) > 0)
          {
            this.UpdateFirst(cell, this.heap[index].last);
            this.UpdateLast(index, item);
            this.BubbleUpMax(index);
          }
          else
            this.BubbleUpMin(cell);
        }
        else if (this.HeapifyMin(cell))
          this.BubbleUpMax(cell);
        else
          this.BubbleUpMin(cell);
      }
    }
    else
    {
      obj = this.heap[cell].last;
      this.heap[cell].last = item;
      if (this.HeapifyMax(cell))
        this.BubbleUpMin(cell);
      else
        this.BubbleUpMax(cell);
    }
    this.raiseItemsRemoved(obj, 1);
    this.raiseItemsAdded(item, 1);
    this.raiseCollectionChanged();
    return obj;
  }

  public T FindMin(out IPriorityQueueHandle<T> handle)
  {
    if (this.size == 0)
      throw new NoSuchItemException();
    handle = (IPriorityQueueHandle<T>) null;
    return this.heap[0].first;
  }

  public T FindMax(out IPriorityQueueHandle<T> handle)
  {
    if (this.size == 0)
      throw new NoSuchItemException();
    if (this.size == 1)
    {
      handle = (IPriorityQueueHandle<T>) null;
      return this.heap[0].first;
    }
    handle = (IPriorityQueueHandle<T>) null;
    return this.heap[0].last;
  }

  public T DeleteMin(out IPriorityQueueHandle<T> handle)
  {
    ++this.stamp;
    if (this.size == 0)
      throw new NoSuchItemException();
    T first = this.heap[0].first;
    handle = (IPriorityQueueHandle<T>) null;
    if (this.size == 1)
    {
      this.size = 0;
      this.heap[0].first = default (T);
    }
    else
    {
      int index = (this.size - 1) / 2;
      if (this.size % 2 == 0)
      {
        this.UpdateFirst(0, this.heap[index].last);
        this.heap[index].last = default (T);
      }
      else
      {
        this.UpdateFirst(0, this.heap[index].first);
        this.heap[index].first = default (T);
      }
      --this.size;
      this.HeapifyMin(0);
    }
    this.raiseItemsRemoved(first, 1);
    this.raiseCollectionChanged();
    return first;
  }

  public T DeleteMax(out IPriorityQueueHandle<T> handle)
  {
    ++this.stamp;
    if (this.size == 0)
      throw new NoSuchItemException();
    handle = (IPriorityQueueHandle<T>) null;
    T obj;
    if (this.size == 1)
    {
      this.size = 0;
      obj = this.heap[0].first;
      this.heap[0].first = default (T);
    }
    else
    {
      obj = this.heap[0].last;
      int index = (this.size - 1) / 2;
      if (this.size % 2 == 0)
      {
        this.UpdateLast(0, this.heap[index].last);
        this.heap[index].last = default (T);
      }
      else
      {
        this.UpdateLast(0, this.heap[index].first);
        this.heap[index].first = default (T);
      }
      --this.size;
      this.HeapifyMax(0);
    }
    this.raiseItemsRemoved(obj, 1);
    this.raiseCollectionChanged();
    return obj;
  }

  private struct Interval
  {
    internal T first;
    internal T last;

    public override string ToString() => $"[{this.first}; {this.last}]";
  }

  private class Handle : IPriorityQueueHandle<T>
  {
    internal int index = -1;

    public override string ToString() => $"[{this.index}]";
  }
}
