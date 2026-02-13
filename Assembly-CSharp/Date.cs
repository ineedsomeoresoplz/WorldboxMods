// Decompiled with JetBrains decompiler
// Type: Date
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

#nullable disable
public static class Date
{
  public static string getAgoString(double pTimestamp)
  {
    return Date.formatSeconds(World.world.getWorldTimeElapsedSince(pTimestamp)) + " ago";
  }

  public static string formatSeconds(float pSeconds)
  {
    return (double) pSeconds >= 60.0 ? ((int) pSeconds / 60).ToText() + "m" : ((int) pSeconds).ToText() + "s";
  }

  public static float getMonthTime()
  {
    int monthsSince = Date.getMonthsSince(0.0);
    return (float) World.world.getCurWorldTime() - (float) monthsSince * 5f;
  }

  public static string getYearDate(double pTime) => Date.getYear(pTime).ToText();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int getYear(double pTime) => Date.getYear0(pTime) + 1;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int getYear0(double pTime) => (int) (pTime / 60.0);

  public static int[] getRawDate(double pTime)
  {
    int num1 = (int) (pTime / 5.0 / 12.0);
    while (pTime < 0.0)
      pTime += 600000.0;
    double num2 = pTime / 5.0;
    double num3 = num2 / 12.0;
    int num4 = (int) num2;
    int num5 = (int) num3;
    int num6 = (int) ((pTime - (double) num5 * 5.0 * 12.0) / 5.0);
    int num7 = (int) ((pTime - (double) num4 * 5.0) / 5.0 * 30.0);
    int num8 = num1 + 1;
    int num9 = num6 + 1;
    return new int[3]{ num7 + 1, num9, num8 };
  }

  public static string getDate(double pTime)
  {
    int[] rawDate = Date.getRawDate(pTime);
    int num1 = rawDate[0];
    int pMonth = rawDate[1];
    int num2 = rawDate[2];
    if (!(LocalizedTextManager.instance.language == "en"))
      return Date.formatDate(num1, pMonth, num2);
    using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
    {
      stringBuilderPool.Append(num1);
      stringBuilderPool.Append(Date.GetDaySuffix(num1));
      stringBuilderPool.Append(" of ");
      stringBuilderPool.Append(Date.formatMonth(pMonth));
      stringBuilderPool.Append(", ");
      stringBuilderPool.Append(num2.ToText());
      return stringBuilderPool.ToString();
    }
  }

  internal static string formatMonth(int pMonth)
  {
    return LocalizedTextManager.getText("month_" + pMonth.ToString());
  }

  internal static string formatDate(int pDay, int pMonth, int pYear)
  {
    CultureInfo culture = LocalizedTextManager.getCulture();
    string input1 = culture.DateTimeFormat.LongDatePattern;
    if (culture.TwoLetterISOLanguageName == "ar")
      input1 = "/ddMMMMyyyy/";
    string input2 = Regex.Replace(input1, "\\bdddd[,\\s]*", "").Trim();
    string text = LocalizedTextManager.getText("inflected_month_" + pMonth.ToString());
    MatchCollection matchCollection = (MatchCollection) null;
    if (input2.Contains("'"))
    {
      matchCollection = Regex.Matches(input2, "'[^']*'");
      for (int i = 0; i < matchCollection.Count; ++i)
        input2 = input2.Replace(matchCollection[i].Value, $"{{{{{{{i.ToString()}}}}}}}");
    }
    string str = Regex.Replace(input2.Replace("MMMM", "[[[1]]]").Replace("yyyy", "[[[2]]]").Replace("dd", "[[[3]]]"), "\\b[d]\\b", "[[[4]]]").Replace("MM", "[[[5]]]").Replace("M", "[[[6]]]").Replace("[[[1]]]", text).Replace("[[[2]]]", pYear.ToText()).Replace("[[[3]]]", pDay < 10 ? "0" + pDay.ToString() : pDay.ToString()).Replace("[[[4]]]", pDay.ToString()).Replace("[[[5]]]", pMonth < 10 ? "0" + pMonth.ToString() : pMonth.ToString()).Replace("[[[6]]]", pMonth.ToString());
    if (matchCollection != null && matchCollection.Count > 0)
    {
      for (int i = matchCollection.Count - 1; i >= 0; --i)
        str = str.Replace($"{{{{{{{i.ToString()}}}}}}}", matchCollection[i].Value.Trim('\''));
    }
    return str;
  }

  public static int getCurrentMonth() => Date.getMonth(World.world.getCurWorldTime());

  public static int getMonth(double pTimestamp)
  {
    float year0 = (float) Date.getYear0(pTimestamp);
    return (int) ((pTimestamp - (double) year0 * 12.0 * 5.0) / 5.0 + 1.0);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int getCurrentYear() => Date.getYear(World.world.getCurWorldTime());

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int getYearsSince(double pFrom)
  {
    return Date.getYear0(World.world.getCurWorldTime() - pFrom);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int getMonthsSince(double pFrom)
  {
    return (int) ((World.world.getCurWorldTime() - pFrom) / 5.0);
  }

  private static string GetDaySuffix(int day)
  {
    switch (day)
    {
      case 1:
      case 21:
      case 31 /*0x1F*/:
        return "st";
      case 2:
      case 22:
        return "nd";
      case 3:
      case 23:
        return "rd";
      default:
        return "th";
    }
  }

  public static bool isMonolithMonth() => Date.getCurrentMonth() == 4;

  public static string getUIStringYearMonthShort()
  {
    return $"y:{Date.getCurrentYear().ToText()}, m:{Date.getCurrentMonth().ToText()}";
  }

  public static string getUIStringYearMonth()
  {
    return $"y: {Date.getCurrentYear().ToText()}, m: {Date.getCurrentMonth().ToText()}";
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static string TimeNow()
  {
    DateTime now = DateTime.Now;
    char[] chars = new char[8];
    Date.Write2Chars(chars, 0, now.Hour);
    chars[2] = ':';
    Date.Write2Chars(chars, 3, now.Minute);
    chars[5] = ':';
    Date.Write2Chars(chars, 6, now.Second);
    return new string(chars);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static void Write2Chars(char[] chars, int offset, int value)
  {
    chars[offset] = Date.Digit(value / 10);
    chars[offset + 1] = Date.Digit(value % 10);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static char Digit(int value) => (char) (value + 48 /*0x30*/);
}
