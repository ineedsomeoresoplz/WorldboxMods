// Decompiled with JetBrains decompiler
// Type: RandomListEnumerator`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable enable
public struct RandomListEnumerator<T> : 
  IEnumerator<
  #nullable disable
  T>,
  IEnumerator,
  IDisposable,
  IEnumerable<T>,
  IEnumerable
{
  private readonly List<T> _source;
  private readonly int _itemsCount;
  private readonly int _maxItems;
  private int _index;
  private readonly int _offset;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public RandomListEnumerator(List<T> source)
    : this(source, source.Count)
  {
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public RandomListEnumerator(List<T> source, int itemsCount)
  {
    this._source = source;
    this._itemsCount = this._maxItems = itemsCount;
    this._index = -1;
    this._offset = Randy.randomInt(0, this._itemsCount);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public RandomListEnumerator(List<T> source, int itemsCount, int maxItems)
  {
    this._source = source;
    this._itemsCount = itemsCount;
    this._maxItems = Mathf.Min(maxItems, this._itemsCount);
    this._index = -1;
    this._offset = Randy.randomInt(0, this._itemsCount);
  }

  private readonly int Index => (this._index + this._offset) % this._itemsCount;

  public readonly T Current
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] [return: MaybeNull] get
    {
      return this._source[this.Index];
    }
  }

  readonly T IEnumerator<T>.Current
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] [return: MaybeNull] get
    {
      return this._source[this.Index];
    }
  }

  readonly 
  #nullable enable
  object? IEnumerator.Current
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] [return: MaybeNull] get
    {
      return (object) this._source[this.Index];
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool MoveNext() => ++this._index < this._maxItems;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void Reset() => this._index = -1;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public RandomListEnumerator<
  #nullable disable
  T> GetEnumerator() => this;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>) this;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this;

  public readonly void Dispose()
  {
  }
}
