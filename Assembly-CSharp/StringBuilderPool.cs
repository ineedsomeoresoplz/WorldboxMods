// Decompiled with JetBrains decompiler
// Type: StringBuilderPool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
public class StringBuilderPool : 
  IDisposable,
  IEquatable<StringBuilderPool>,
  IEquatable<StringBuilder>
{
  [ThreadStatic]
  private static StackPool<StringBuilder> _pool;
  private StringBuilder _string_builder = StringBuilderPool.pool.get();
  private bool _disposed;

  private static StackPool<StringBuilder> pool
  {
    get
    {
      if (StringBuilderPool._pool == null)
        StringBuilderPool._pool = new StackPool<StringBuilder>();
      return StringBuilderPool._pool;
    }
  }

  public StringBuilderPool()
  {
  }

  public void Dispose()
  {
    if (this._disposed)
      return;
    this._string_builder.Clear();
    StringBuilderPool.pool.release(this._string_builder);
    this._disposed = true;
  }

  public StringBuilder string_builder
  {
    get
    {
      if (this._disposed)
        throw new ObjectDisposedException(nameof (StringBuilderPool));
      return this._string_builder;
    }
  }

  public StringBuilderPool ToTitleCase()
  {
    this._string_builder.ToTitleCase();
    return this;
  }

  public StringBuilderPool ToUpper()
  {
    this._string_builder.ToUpper();
    return this;
  }

  public StringBuilderPool ToUpperInvariant()
  {
    this._string_builder.ToUpperInvariant();
    return this;
  }

  public StringBuilderPool ToLower()
  {
    this._string_builder.ToLower();
    return this;
  }

  public StringBuilderPool ToLowerInvariant()
  {
    this._string_builder.ToLowerInvariant();
    return this;
  }

  public StringBuilderPool TrimEnd(params char[] trimChars)
  {
    this._string_builder.TrimEnd(trimChars);
    return this;
  }

  public StringBuilderPool Cut(int startIndex, int end)
  {
    this._string_builder.CreateTrimmedString(startIndex, end);
    return this;
  }

  public StringBuilderPool Remove(params char[] chars)
  {
    this._string_builder.Remove(chars);
    return this;
  }

  public int IndexOf(char value) => this._string_builder.IndexOf(value);

  public int IndexOfAny(params char[] anyOf) => this._string_builder.IndexOfAny(anyOf);

  public int LastIndexOf(char value) => this._string_builder.LastIndexOf(value);

  public int LastIndexOfAny(char[] anyOf, int startIndex)
  {
    return this._string_builder.LastIndexOfAny(anyOf, startIndex);
  }

  public int LastIndexOfAny(char[] anyOf, int startIndex, int count)
  {
    return this._string_builder.LastIndexOfAny(anyOf, startIndex, count);
  }

  public int LastIndexOfAny(params char[] anyOf) => this._string_builder.LastIndexOfAny(anyOf);

  public StringBuilderPool(int capacity) => this._string_builder = new StringBuilder(capacity);

  public StringBuilderPool(string value) => this._string_builder = new StringBuilder(value);

  public StringBuilderPool(int capacity, int maxCapacity)
  {
    this._string_builder = new StringBuilder(capacity, maxCapacity);
  }

  public StringBuilderPool(string value, int capacity)
  {
    this._string_builder = new StringBuilder(value, capacity);
  }

  public StringBuilderPool(string value, int startIndex, int length, int capacity)
  {
    this._string_builder = new StringBuilder(value, startIndex, length, capacity);
  }

  public int Capacity
  {
    get => this._string_builder.Capacity;
    set => this._string_builder.Capacity = value;
  }

  public int MaxCapacity => this._string_builder.MaxCapacity;

  public int Length
  {
    get => this._string_builder.Length;
    set => this._string_builder.Length = value;
  }

  public char this[int index]
  {
    get => this._string_builder[index];
    set => this._string_builder[index] = value;
  }

  public StringBuilderPool Append(bool value)
  {
    this._string_builder.Append(value);
    return this;
  }

  public StringBuilderPool Append(byte value)
  {
    this._string_builder.Append(value);
    return this;
  }

  public StringBuilderPool Append(char value)
  {
    this._string_builder.Append(value);
    return this;
  }

  public StringBuilderPool Append(char[] value)
  {
    this._string_builder.Append(value);
    return this;
  }

  public StringBuilderPool Append(char[] value, int startIndex, int charCount)
  {
    this._string_builder.Append(value, startIndex, charCount);
    return this;
  }

  public StringBuilderPool Append(Decimal value)
  {
    this._string_builder.Append(value);
    return this;
  }

  public StringBuilderPool Append(double value)
  {
    this._string_builder.Append(value);
    return this;
  }

  public StringBuilderPool Append(short value)
  {
    this._string_builder.Append(value);
    return this;
  }

  public StringBuilderPool Append(int value)
  {
    this._string_builder.Append(value);
    return this;
  }

  public StringBuilderPool Append(long value)
  {
    this._string_builder.Append(value);
    return this;
  }

  public StringBuilderPool Append(object value)
  {
    this._string_builder.Append(value);
    return this;
  }

  public StringBuilderPool Append(ReadOnlyMemory<char> value)
  {
    this._string_builder.Append((object) value);
    return this;
  }

  public StringBuilderPool Append(ReadOnlySpan<char> value)
  {
    this._string_builder.Append(value);
    return this;
  }

  public StringBuilderPool Append(sbyte value)
  {
    this._string_builder.Append(value);
    return this;
  }

  public StringBuilderPool Append(float value)
  {
    this._string_builder.Append(value);
    return this;
  }

  public StringBuilderPool Append(string value)
  {
    this._string_builder.Append(value);
    return this;
  }

  public StringBuilderPool Append(string value, int startIndex, int count)
  {
    this._string_builder.Append(value, startIndex, count);
    return this;
  }

  public StringBuilderPool Append(StringBuilder value)
  {
    this._string_builder.Append(value);
    return this;
  }

  public StringBuilderPool Append(StringBuilder value, int startIndex, int count)
  {
    this._string_builder.Append(value, startIndex, count);
    return this;
  }

  public StringBuilderPool Append(StringBuilderPool value)
  {
    this._string_builder.Append(value.string_builder);
    return this;
  }

  public StringBuilderPool Append(StringBuilderPool value, int startIndex, int count)
  {
    this._string_builder.Append(value.string_builder, startIndex, count);
    return this;
  }

  public StringBuilderPool Append(ushort value)
  {
    this._string_builder.Append(value);
    return this;
  }

  public StringBuilderPool Append(uint value)
  {
    this._string_builder.Append(value);
    return this;
  }

  public StringBuilderPool Append(ulong value)
  {
    this._string_builder.Append(value);
    return this;
  }

  public StringBuilderPool AppendFormat(
    IFormatProvider provider,
    string format,
    params object[] args)
  {
    this._string_builder.AppendFormat(provider, format, args);
    return this;
  }

  public StringBuilderPool AppendFormat(string format, object arg0)
  {
    this._string_builder.AppendFormat(format, arg0);
    return this;
  }

  public StringBuilderPool AppendFormat(string format, object arg0, object arg1)
  {
    this._string_builder.AppendFormat(format, arg0, arg1);
    return this;
  }

  public StringBuilderPool AppendFormat(string format, object arg0, object arg1, object arg2)
  {
    this._string_builder.AppendFormat(format, arg0, arg1, arg2);
    return this;
  }

  public StringBuilderPool AppendFormat(string format, params object[] args)
  {
    this._string_builder.AppendFormat(format, args);
    return this;
  }

  public StringBuilderPool AppendJoin(char separator, params object[] values)
  {
    this._string_builder.AppendJoin(separator, values);
    return this;
  }

  public StringBuilderPool AppendJoin(char separator, params string[] values)
  {
    this._string_builder.AppendJoin(separator, values);
    return this;
  }

  public StringBuilderPool AppendJoin(string separator, params object[] values)
  {
    this._string_builder.AppendJoin(separator, values);
    return this;
  }

  public StringBuilderPool AppendJoin(string separator, params string[] values)
  {
    this._string_builder.AppendJoin(separator, values);
    return this;
  }

  public StringBuilderPool AppendJoin<T>(char separator, IEnumerable<T> values)
  {
    this._string_builder.AppendJoin<T>(separator, values);
    return this;
  }

  public StringBuilderPool AppendJoin<T>(string separator, IEnumerable<T> values)
  {
    this._string_builder.AppendJoin<T>(separator, values);
    return this;
  }

  public StringBuilderPool AppendLine()
  {
    this._string_builder.AppendLine();
    return this;
  }

  public StringBuilderPool AppendLine(string value)
  {
    this._string_builder.AppendLine(value);
    return this;
  }

  public StringBuilderPool Clear()
  {
    this._string_builder.Clear();
    return this;
  }

  public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
  {
    this._string_builder.CopyTo(sourceIndex, destination, destinationIndex, count);
  }

  public void CopyTo(int sourceIndex, Span<char> destination, int count)
  {
    this._string_builder.CopyTo(sourceIndex, destination, count);
  }

  public int EnsureCapacity(int capacity) => this._string_builder.EnsureCapacity(capacity);

  public bool Equals(ReadOnlySpan<char> span) => this._string_builder.Equals(span);

  public bool Equals(StringBuilder sb) => this._string_builder.Equals(sb);

  public bool Equals(StringBuilderPool sb) => this._string_builder.Equals(sb.string_builder);

  public StringBuilderPool Insert(int index, bool value)
  {
    this._string_builder.Insert(index, value);
    return this;
  }

  public StringBuilderPool Insert(int index, byte value)
  {
    this._string_builder.Insert(index, value);
    return this;
  }

  public StringBuilderPool Insert(int index, char value)
  {
    this._string_builder.Insert(index, value);
    return this;
  }

  public StringBuilderPool Insert(int index, char[] value)
  {
    this._string_builder.Insert(index, value);
    return this;
  }

  public StringBuilderPool Insert(int index, char[] value, int startIndex, int charCount)
  {
    this._string_builder.Insert(index, value, startIndex, charCount);
    return this;
  }

  public StringBuilderPool Insert(int index, Decimal value)
  {
    this._string_builder.Insert(index, value);
    return this;
  }

  public StringBuilderPool Insert(int index, double value)
  {
    this._string_builder.Insert(index, value);
    return this;
  }

  public StringBuilderPool Insert(int index, short value)
  {
    this._string_builder.Insert(index, value);
    return this;
  }

  public StringBuilderPool Insert(int index, int value)
  {
    this._string_builder.Insert(index, value);
    return this;
  }

  public StringBuilderPool Insert(int index, long value)
  {
    this._string_builder.Insert(index, value);
    return this;
  }

  public StringBuilderPool Insert(int index, object value)
  {
    this._string_builder.Insert(index, value);
    return this;
  }

  public StringBuilderPool Insert(int index, ReadOnlySpan<char> value)
  {
    this._string_builder.Insert(index, value);
    return this;
  }

  public StringBuilderPool Insert(int index, sbyte value)
  {
    this._string_builder.Insert(index, value);
    return this;
  }

  public StringBuilderPool Insert(int index, float value)
  {
    this._string_builder.Insert(index, value);
    return this;
  }

  public StringBuilderPool Insert(int index, string value)
  {
    this._string_builder.Insert(index, value);
    return this;
  }

  public StringBuilderPool Insert(int index, string value, int count)
  {
    this._string_builder.Insert(index, value, count);
    return this;
  }

  public StringBuilderPool Insert(int index, ushort value)
  {
    this._string_builder.Insert(index, value);
    return this;
  }

  public StringBuilderPool Insert(int index, uint value)
  {
    this._string_builder.Insert(index, value);
    return this;
  }

  public StringBuilderPool Insert(int index, ulong value)
  {
    this._string_builder.Insert(index, value);
    return this;
  }

  public StringBuilderPool Remove(int startIndex, int length)
  {
    this._string_builder.Remove(startIndex, length);
    return this;
  }

  public StringBuilderPool Replace(char oldChar, char newChar)
  {
    this._string_builder.Replace(oldChar, newChar);
    return this;
  }

  public StringBuilderPool Replace(char oldChar, char newChar, int startIndex, int count)
  {
    this._string_builder.Replace(oldChar, newChar, startIndex, count);
    return this;
  }

  public StringBuilderPool Replace(string oldValue, string newValue)
  {
    this._string_builder.Replace(oldValue, newValue);
    return this;
  }

  public StringBuilderPool Replace(string oldValue, string newValue, int startIndex, int count)
  {
    this._string_builder.Replace(oldValue, newValue, startIndex, count);
    return this;
  }

  public string ToString(int startIndex, int length)
  {
    return this._string_builder.ToString(startIndex, length);
  }

  public override string ToString() => this._string_builder.ToString();

  public Span<char> AsSpan()
  {
    Span<char> span = Span<char>.op_Implicit(new char[this._string_builder.Length]);
    this._string_builder.CopyTo(0, span, this._string_builder.Length);
    return span;
  }
}
