// Decompiled with JetBrains decompiler
// Type: System.Text.StringBuilderExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Globalization;

#nullable disable
namespace System.Text;

public static class StringBuilderExtensions
{
  public static StringBuilder Remove(this StringBuilder sb, params char[] removeChars)
  {
    int num = 0;
    while (num < sb.Length)
    {
      if (removeChars.IndexOf<char>(sb[num]) > -1)
        sb.Remove(num, 1);
      else
        ++num;
    }
    return sb;
  }

  public static StringBuilder Remove(this StringBuilder sb, int startIndex)
  {
    return startIndex >= sb.Length ? sb : sb.Remove(startIndex, sb.Length - startIndex);
  }

  private static bool IsBOMWhitespace(char c) => false;

  private static StringBuilder TrimHelper(this StringBuilder sb, int trimType)
  {
    int num1 = sb.Length - 1;
    int num2 = 0;
    if (trimType != 1)
    {
      num2 = 0;
      while (num2 < sb.Length && (char.IsWhiteSpace(sb[num2]) || StringBuilderExtensions.IsBOMWhitespace(sb[num2])))
        ++num2;
    }
    if (trimType != 0)
    {
      num1 = sb.Length - 1;
      while (num1 >= num2 && (char.IsWhiteSpace(sb[num1]) || StringBuilderExtensions.IsBOMWhitespace(sb[num2])))
        --num1;
    }
    return sb.CreateTrimmedString(num2, num1);
  }

  internal static StringBuilder CreateTrimmedString(this StringBuilder sb, int start, int end)
  {
    int num = end - start + 1;
    if (num == sb.Length)
      return sb;
    if (num != 0)
      return sb.InternalSubstring(start, end);
    sb.Length = 0;
    return sb;
  }

  private static StringBuilder InternalSubstring(this StringBuilder sb, int startIndex, int end)
  {
    sb.Length = end + 1;
    if (startIndex > 0)
      sb.Remove(0, startIndex);
    return sb;
  }

  private static StringBuilder TrimHelper(this StringBuilder sb, char[] trimChars, int trimType)
  {
    int num1 = sb.Length - 1;
    int num2 = 0;
    if (trimType != 1)
    {
      for (num2 = 0; num2 < sb.Length; ++num2)
      {
        int index = 0;
        char ch = sb[num2];
        while (index < trimChars.Length && (int) trimChars[index] != (int) ch)
          ++index;
        if (index == trimChars.Length)
          break;
      }
    }
    if (trimType != 0)
    {
      for (num1 = sb.Length - 1; num1 >= num2; --num1)
      {
        int index = 0;
        char ch = sb[num1];
        while (index < trimChars.Length && (int) trimChars[index] != (int) ch)
          ++index;
        if (index == trimChars.Length)
          break;
      }
    }
    return sb.CreateTrimmedString(num2, num1);
  }

  public static StringBuilder TrimStart(this StringBuilder sb, params char[] trimChars)
  {
    return trimChars != null && trimChars.Length != 0 ? sb.TrimHelper(trimChars, 0) : sb.TrimHelper(0);
  }

  public static StringBuilder TrimEnd(this StringBuilder sb, params char[] trimChars)
  {
    return trimChars != null && trimChars.Length != 0 ? sb.TrimHelper(trimChars, 1) : sb.TrimHelper(1);
  }

  public static StringBuilder Trim(this StringBuilder sb) => sb.TrimHelper(2);

  public static StringBuilder Trim(this StringBuilder sb, params char[] trimChars)
  {
    return trimChars != null && trimChars.Length != 0 ? sb.TrimHelper(trimChars, 2) : sb.TrimHelper(2);
  }

  public static int IndexOf(this StringBuilder sb, char value) => sb.IndexOf(value, 0, sb.Length);

  public static int IndexOf(this StringBuilder sb, char value, int startIndex)
  {
    return sb.IndexOf(value, startIndex, sb.Length - startIndex);
  }

  public static int IndexOf(this StringBuilder sb, char value, int startIndex, int count)
  {
    if (sb.Length == 0 || count == 0)
      return -1;
    for (int index = startIndex; index < startIndex + count; ++index)
    {
      if ((int) sb[index] == (int) value)
        return index;
    }
    return -1;
  }

  public static int IndexOf(this StringBuilder sb, string value, bool ignoreCase = false)
  {
    return value == string.Empty ? 0 : StringBuilderExtensions.IndexOfInternal(sb, value, 0, sb.Length, ignoreCase);
  }

  public static int IndexOf(this StringBuilder sb, string value, int startIndex, bool ignoreCase = false)
  {
    return StringBuilderExtensions.IndexOfInternal(sb, value, startIndex, sb.Length - startIndex, ignoreCase);
  }

  public static int IndexOf(
    this StringBuilder sb,
    string value,
    int startIndex,
    int count,
    bool ignoreCase = false)
  {
    return StringBuilderExtensions.IndexOfInternal(sb, value, startIndex, count, ignoreCase);
  }

  private static int IndexOfInternal(
    StringBuilder sb,
    string value,
    int startIndex,
    int count,
    bool ignoreCase)
  {
    if (value == string.Empty)
      return startIndex;
    if (sb.Length == 0 || count == 0 || startIndex + 1 + value.Length > sb.Length)
      return -1;
    int length = value.Length;
    int num = startIndex + count - value.Length;
    if (!ignoreCase)
    {
      for (int index1 = startIndex; index1 <= num; ++index1)
      {
        if ((int) sb[index1] == (int) value[0])
        {
          int index2 = 1;
          while (index2 < length && (int) sb[index1 + index2] == (int) value[index2])
            ++index2;
          if (index2 == length)
            return index1;
        }
      }
    }
    else
    {
      for (int index3 = startIndex; index3 <= num; ++index3)
      {
        if ((int) char.ToLower(sb[index3]) == (int) char.ToLower(value[0]))
        {
          int index4 = 1;
          while (index4 < length && (int) char.ToLower(sb[index3 + index4]) == (int) char.ToLower(value[index4]))
            ++index4;
          if (index4 == length)
            return index3;
        }
      }
    }
    return -1;
  }

  public static int IndexOfAny(this StringBuilder sb, char[] anyOf)
  {
    return sb.IndexOfAny(anyOf, 0, sb.Length);
  }

  public static int IndexOfAny(this StringBuilder sb, char[] anyOf, int startIndex)
  {
    return sb.IndexOfAny(anyOf, startIndex, sb.Length - startIndex);
  }

  public static int IndexOfAny(this StringBuilder sb, char[] anyOf, int startIndex, int count)
  {
    if (sb.Length == 0 || count == 0)
      return -1;
    for (int index = startIndex; index < startIndex + count; ++index)
    {
      if (anyOf.IndexOf<char>(sb[index]) > -1)
        return index;
    }
    return -1;
  }

  public static int LastIndexOf(this StringBuilder sb, char value)
  {
    return sb.LastIndexOf(value, sb.Length - 1, sb.Length);
  }

  public static int LastIndexOf(this StringBuilder sb, char value, int startIndex)
  {
    return sb.LastIndexOf(value, startIndex, startIndex + 1);
  }

  public static int LastIndexOf(this StringBuilder sb, char value, int startIndex, int count)
  {
    if (sb.Length == 0 || count == 0)
      return -1;
    for (int index = startIndex; index > startIndex - count; --index)
    {
      if ((int) sb[index] == (int) value)
        return index;
    }
    return -1;
  }

  public static int LastIndexOf(this StringBuilder sb, string value, bool ignoreCase = false)
  {
    return value == string.Empty ? (sb.Length == 0 ? 0 : sb.Length - 1) : (sb.Length == 0 ? -1 : StringBuilderExtensions.LastIndexOfInternal(sb, value, sb.Length - 1, sb.Length, ignoreCase));
  }

  public static int LastIndexOf(
    this StringBuilder sb,
    string value,
    int startIndex,
    bool ignoreCase = false)
  {
    return StringBuilderExtensions.LastIndexOfInternal(sb, value, startIndex, startIndex + 1, ignoreCase);
  }

  public static int LastIndexOf(
    this StringBuilder sb,
    string value,
    int startIndex,
    int count,
    bool ignoreCase = false)
  {
    return StringBuilderExtensions.LastIndexOfInternal(sb, value, startIndex, count, ignoreCase);
  }

  private static int LastIndexOfInternal(
    StringBuilder sb,
    string value,
    int startIndex,
    int count,
    bool ignoreCase)
  {
    if (value == string.Empty)
      return startIndex;
    if (sb.Length == 0 || count == 0 || startIndex + 1 - count + value.Length > sb.Length)
      return -1;
    int length = value.Length;
    int index1 = length - 1;
    int num1 = startIndex - count + value.Length;
    if (!ignoreCase)
    {
      for (int index2 = startIndex; index2 >= num1; --index2)
      {
        if ((int) sb[index2] == (int) value[index1])
        {
          int num2 = 1;
          while (num2 < length && (int) sb[index2 - num2] == (int) value[index1 - num2])
            ++num2;
          if (num2 == length)
            return index2 - num2 + 1;
        }
      }
    }
    else
    {
      for (int index3 = startIndex; index3 >= num1; --index3)
      {
        if ((int) char.ToLower(sb[index3]) == (int) char.ToLower(value[index1]))
        {
          int num3 = 1;
          while (num3 < length && (int) char.ToLower(sb[index3 - num3]) == (int) char.ToLower(value[index1 - num3]))
            ++num3;
          if (num3 == length)
            return index3 - num3 + 1;
        }
      }
    }
    return -1;
  }

  public static int LastIndexOfAny(this StringBuilder sb, char[] anyOf)
  {
    return sb.LastIndexOfAny(anyOf, sb.Length - 1, sb.Length);
  }

  public static int LastIndexOfAny(this StringBuilder sb, char[] anyOf, int startIndex)
  {
    return sb.LastIndexOfAny(anyOf, startIndex, startIndex + 1);
  }

  public static int LastIndexOfAny(this StringBuilder sb, char[] anyOf, int startIndex, int count)
  {
    if (sb.Length == 0 || count == 0)
      return -1;
    for (int index = startIndex; index > startIndex - count; --index)
    {
      if (anyOf.IndexOf<char>(sb[index]) > -1)
        return index;
    }
    return -1;
  }

  public static bool StartsWith(this StringBuilder sb, string value, bool ignoreCase = false)
  {
    int length = value.Length;
    if (length > sb.Length)
      return false;
    if (!ignoreCase)
    {
      for (int index = 0; index < length; ++index)
      {
        if ((int) sb[index] != (int) value[index])
          return false;
      }
    }
    else
    {
      for (int index = 0; index < length; ++index)
      {
        if ((int) char.ToLower(sb[index]) != (int) char.ToLower(value[index]))
          return false;
      }
    }
    return true;
  }

  public static bool EndsWith(this StringBuilder sb, string value, bool ignoreCase = false)
  {
    int length = value.Length;
    int num1 = sb.Length - 1;
    int num2 = length - 1;
    if (length > sb.Length)
      return false;
    if (!ignoreCase)
    {
      for (int index = 0; index < length; ++index)
      {
        if ((int) sb[num1 - index] != (int) value[num2 - index])
          return false;
      }
    }
    else
    {
      for (int index = length - 1; index >= 0; --index)
      {
        if ((int) char.ToLower(sb[num1 - index]) != (int) char.ToLower(value[num2 - index]))
          return false;
      }
    }
    return true;
  }

  public static StringBuilder ToLower(this StringBuilder sb)
  {
    for (int index = 0; index < sb.Length; ++index)
      sb[index] = char.ToLower(sb[index]);
    return sb;
  }

  public static StringBuilder Reverse(this StringBuilder sb)
  {
    int length = sb.Length;
    for (int index = 0; index < length / 2; ++index)
    {
      char ch = sb[index];
      sb[index] = sb[length - index - 1];
      sb[length - index - 1] = ch;
    }
    return sb;
  }

  public static StringBuilder ToLower(this StringBuilder sb, CultureInfo culture)
  {
    for (int index = 0; index < sb.Length; ++index)
      sb[index] = char.ToLower(sb[index], culture);
    return sb;
  }

  public static StringBuilder ToLowerInvariant(this StringBuilder sb)
  {
    return sb.ToLower(CultureInfo.InvariantCulture);
  }

  public static StringBuilder ToUpper(this StringBuilder sb)
  {
    for (int index = 0; index < sb.Length; ++index)
      sb[index] = char.ToUpper(sb[index]);
    return sb;
  }

  public static StringBuilder ToUpper(this StringBuilder sb, CultureInfo culture)
  {
    for (int index = 0; index < sb.Length; ++index)
      sb[index] = char.ToUpper(sb[index], culture);
    return sb;
  }

  public static StringBuilder ToUpperInvariant(this StringBuilder sb)
  {
    return sb.ToUpper(CultureInfo.InvariantCulture);
  }

  public static StringBuilder ToTitleCase(this StringBuilder sb)
  {
    return sb.ToTitleCase(CultureInfo.InvariantCulture);
  }

  public static StringBuilder ToTitleCase(this StringBuilder sb, CultureInfo culture)
  {
    bool flag = true;
    for (int index = 0; index < sb.Length; ++index)
    {
      if (flag)
      {
        sb[index] = char.ToUpper(sb[index], culture);
        flag = false;
      }
      else
        sb[index] = char.ToLower(sb[index], culture);
      if (char.IsWhiteSpace(sb[index]) || char.IsPunctuation(sb[index]) || sb[index] == '\'')
        flag = true;
    }
    return sb;
  }
}
