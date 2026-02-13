// Decompiled with JetBrains decompiler
// Type: IntExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public static class IntExtensions
{
  private static readonly (int value, string symbol)[] _roman_number_map = new (int, string)[13]
  {
    (1000, "M"),
    (900, "CM"),
    (500, "D"),
    (400, "CD"),
    (100, "C"),
    (90, "XC"),
    (50, "L"),
    (40, "XL"),
    (10, "X"),
    (9, "IX"),
    (5, "V"),
    (4, "IV"),
    (1, "I")
  };

  public static string ToText(this int pInt) => pInt.ToString("##,0.#");

  public static string ToText(this long pLong) => pLong.ToString("##,0.#");

  public static string ToText(this float pFloat) => pFloat.ToString("##,0.#");

  public static string ToText(this double pDouble) => pDouble.ToString("##,0.#");

  public static string ToText(this int pInt, int pMaxLength)
  {
    return Toolbox.formatNumber((long) pInt, pMaxLength);
  }

  public static string ToText(this long pLong, int pMaxLength)
  {
    return Toolbox.formatNumber(pLong, pMaxLength);
  }

  public static string ToRoman(this int pNumber)
  {
    if (pNumber < 1)
      return "N";
    if (pNumber > 3999)
      return "MMMM";
    using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
    {
      foreach ((int value, string symbol) in IntExtensions._roman_number_map)
      {
        for (; pNumber >= value; pNumber -= value)
          stringBuilderPool.Append(symbol);
      }
      return stringBuilderPool.ToString();
    }
  }
}
