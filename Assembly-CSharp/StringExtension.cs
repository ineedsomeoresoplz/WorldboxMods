// Decompiled with JetBrains decompiler
// Type: StringExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Buffers;

#nullable disable
public static class StringExtension
{
  public static int[] AllIndexesOf(this string pString, string pValue)
  {
    int startIndex1 = 0;
    int length1 = pValue.Length;
    int length2 = 0;
    int num1;
    for (; startIndex1 < pString.Length; startIndex1 = num1 + length1)
    {
      num1 = pString.IndexOf(pValue, startIndex1, StringComparison.Ordinal);
      if (num1 != -1)
        ++length2;
      else
        break;
    }
    int[] numArray = new int[length2];
    int startIndex2 = 0;
    int index = 0;
    int num2;
    for (; startIndex2 < pString.Length; startIndex2 = num2 + length1)
    {
      num2 = pString.IndexOf(pValue, startIndex2, StringComparison.Ordinal);
      if (num2 != -1)
      {
        numArray[index] = num2;
        ++index;
      }
      else
        break;
    }
    return numArray;
  }

  public static char Last(this string pString) => pString[pString.Length - 1];

  public static char First(this string pString) => pString[0];

  public static string Reverse(this string pString)
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: method pointer
    return string.Create<string>(pString.Length, pString, StringExtension.\u003C\u003Ec.\u003C\u003E9__3_0 ?? (StringExtension.\u003C\u003Ec.\u003C\u003E9__3_0 = new SpanAction<char, string>((object) StringExtension.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CReverse\u003Eb__3_0))));
  }

  public static string Shuffle(this string pString)
  {
    using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
    {
      using (ListPool<char> list = new ListPool<char>())
      {
        for (int index = 0; index < pString.Length; ++index)
          list.Add(pString[index]);
        list.Shuffle<char>();
        for (int index = 0; index < list.Count; ++index)
        {
          char ch = list[index];
          stringBuilderPool.Append(ch);
        }
        string str = stringBuilderPool.ToString();
        return char.ToUpper(str[0]).ToString() + str.Substring(1).ToLower();
      }
    }
  }

  public static string FirstToUpper(this string pString)
  {
    if (pString.Length == 0)
      return pString;
    string upper = pString.Substring(0, 1).ToUpper();
    pString = pString.Substring(1, pString.Length - 1);
    string str = pString;
    return upper + str;
  }

  public static string ColorHex(this string pString, string pColorHex, bool pLocalize = false)
  {
    return Toolbox.coloredText(pString, pColorHex, pLocalize);
  }

  public static string blue(this string pString)
  {
    return !string.IsNullOrEmpty(pString) ? pString.ColorHex("#4CCFFF") : "";
  }

  public static string blue(this object pString)
  {
    return pString == null ? (string) null : pString.ToString().blue();
  }

  public static string red(this string pString)
  {
    return !string.IsNullOrEmpty(pString) ? pString.ColorHex("#FF637D") : "";
  }

  public static string red(this object pString)
  {
    return pString == null ? (string) null : pString.ToString().red();
  }

  public static string teal(this string pString)
  {
    return !string.IsNullOrEmpty(pString) ? pString.ColorHex("#23F3FF") : "";
  }

  public static string teal(this object pString)
  {
    return pString == null ? (string) null : pString.ToString().teal();
  }

  public static string yellow(this string pString)
  {
    return !string.IsNullOrEmpty(pString) ? pString.ColorHex("#FFFF51") : "";
  }

  public static string yellow(this object pString)
  {
    return pString == null ? (string) null : pString.ToString().yellow();
  }

  public static string Localize(this string pString)
  {
    return LocalizedTextManager.getText(pString.Underscore());
  }

  public static string Description(this string pString) => pString + "_description";

  public static bool EndsWithAny(this string pString, string[] pTrimString)
  {
    foreach (string str in pTrimString)
    {
      if (pString.EndsWith(str))
        return true;
    }
    return false;
  }

  public static string TrimEnd(this string pString, string pTrimString)
  {
    return pString.EndsWith(pTrimString) ? pString.Substring(0, pString.Length - pTrimString.Length) : pString;
  }

  public static bool HasUpperCase(this string pString)
  {
    ReadOnlySpan<char> readOnlySpan = MemoryExtensions.AsSpan(pString);
    for (int index = 0; index < readOnlySpan.Length; ++index)
    {
      if (char.IsUpper(readOnlySpan[index]))
        return true;
    }
    return false;
  }

  public static bool ShouldUnderscore(this string pString)
  {
    ReadOnlySpan<char> readOnlySpan = MemoryExtensions.AsSpan(pString);
    for (int index = 0; index < readOnlySpan.Length; ++index)
    {
      if (!char.IsLetterOrDigit(readOnlySpan[index]) && readOnlySpan[index] != '_' || char.IsWhiteSpace(readOnlySpan[index]) || char.IsUpper(readOnlySpan[index]))
        return true;
    }
    return false;
  }

  public static string Truncate(this string pString, int pMaxLength)
  {
    return string.IsNullOrEmpty(pString) || pString.Length <= pMaxLength ? pString : pString.Substring(0, pMaxLength);
  }

  public static string Underscore(this string pString)
  {
    if (string.IsNullOrEmpty(pString) || !pString.ShouldUnderscore())
      return pString;
    using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
    {
      ReadOnlySpan<char> readOnlySpan = MemoryExtensions.AsSpan(pString);
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      for (int index = 0; index < readOnlySpan.Length; ++index)
      {
        if (char.IsLower(readOnlySpan[index]))
        {
          flag3 = true;
          break;
        }
      }
      for (int index = 0; index < readOnlySpan.Length; ++index)
      {
        if (char.IsLetter(readOnlySpan[index]))
        {
          if (char.IsUpper(readOnlySpan[index]))
          {
            if (index > 0 && !flag1 && !flag2 | flag3)
              stringBuilderPool.Append('_');
            stringBuilderPool.Append(char.ToLower(readOnlySpan[index]));
            flag2 = true;
          }
          else
          {
            stringBuilderPool.Append(readOnlySpan[index]);
            flag2 = false;
          }
          flag1 = false;
        }
        else if (char.IsDigit(readOnlySpan[index]))
        {
          stringBuilderPool.Append(readOnlySpan[index]);
          flag1 = false;
          flag2 = false;
        }
        else if (!flag1)
        {
          stringBuilderPool.Append('_');
          flag1 = true;
          flag2 = false;
        }
      }
      if (flag1)
        stringBuilderPool.Remove(stringBuilderPool.Length - 1, 1);
      return stringBuilderPool.ToString();
    }
  }
}
