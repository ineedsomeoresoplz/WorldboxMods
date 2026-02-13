// Decompiled with JetBrains decompiler
// Type: InsultStringGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public static class InsultStringGenerator
{
  private static string[] _insult_characters_2 = new string[4]
  {
    "#",
    "%",
    "&",
    "@"
  };
  private static string[] _insult_characters = new string[5]
  {
    "!",
    "#",
    "%",
    "&",
    "@"
  };
  private static List<string> _cached_bad_strings = new List<string>();
  private static List<string> _cached_bad_connections_string = new List<string>();
  private const int MAX_HARMFUL_DNA_SEQUENCES = 30;
  private const int MAX_BAD_CONNECTION_STRINGS = 30;

  public static string getRandomText(int pMin = 4, int pMax = 9, bool pUseSameSizeSet = false)
  {
    using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
    {
      int num = Randy.randomInt(pMin, pMax);
      for (int index = 0; index < num; ++index)
      {
        string str = pUseSameSizeSet ? InsultStringGenerator._insult_characters_2.GetRandom<string>() : InsultStringGenerator._insult_characters.GetRandom<string>();
        stringBuilderPool.Append(str);
      }
      return stringBuilderPool.ToString();
    }
  }

  public static string getDNASequenceBad()
  {
    string pText;
    if (InsultStringGenerator._cached_bad_strings.Count < 30)
    {
      using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
      {
        for (int index = 0; index < 6; ++index)
        {
          if (index > 0)
            stringBuilderPool.Append(" ");
          stringBuilderPool.Append(InsultStringGenerator.getRandomText(3, 3, true));
        }
        pText = stringBuilderPool.ToString();
        pText = Toolbox.coloredString(pText, "#B159FF");
        InsultStringGenerator._cached_bad_strings.Add(pText);
      }
    }
    else
      pText = InsultStringGenerator._cached_bad_strings.GetRandom<string>();
    return pText;
  }

  public static string getBadConnectionString()
  {
    string connectionString;
    if (InsultStringGenerator._cached_bad_connections_string.Count < 30)
    {
      connectionString = Toolbox.coloredString(InsultStringGenerator.getRandomText(7, 7, true), "#B159FF");
      InsultStringGenerator._cached_bad_connections_string.Add(connectionString);
    }
    else
      connectionString = InsultStringGenerator._cached_bad_connections_string.GetRandom<string>();
    return connectionString;
  }
}
