// Decompiled with JetBrains decompiler
// Type: WarWinnerExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public static class WarWinnerExtensions
{
  public static string getLocaleID(this WarWinner pWinner)
  {
    string localeId;
    switch (pWinner)
    {
      case WarWinner.Attackers:
        localeId = "attackers";
        break;
      case WarWinner.Defenders:
        localeId = "defenders";
        break;
      case WarWinner.Peace:
        localeId = "peace";
        break;
      case WarWinner.Merged:
        localeId = "war_winner_merged";
        break;
      default:
        localeId = "war_winner_nobody";
        break;
    }
    return localeId;
  }
}
