// Decompiled with JetBrains decompiler
// Type: MetaTextReportHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class MetaTextReportHelper
{
  public static string color_text_main => ColorStyleLibrary.m.color_text_grey;

  public static string color_text_quote => ColorStyleLibrary.m.color_text_grey_dark;

  public static string addSingleUnitText(Actor pActor, bool pAddGap = true, bool pAddNameQuote = true)
  {
    if (!pActor.hasHappinessHistory())
      return string.Empty;
    using (ListPool<HappinessHistory> list = new ListPool<HappinessHistory>((IEnumerable<HappinessHistory>) pActor.happiness_change_history))
    {
      HappinessHistory random = list.GetRandom<HappinessHistory>();
      string pString1 = $"<i>\"{random.asset.getRandomTextSingleReportLocalized()}\"</i>";
      string pString2 = "\n— " + pActor.name;
      string str1 = pActor.getAge().ToString();
      string pString3 = !pActor.isSexFemale() ? str1 + " M" : str1 + " F";
      string str2 = random.getAgoString().ColorHex(ColorStyleLibrary.m.color_text_grey_dark);
      string str3 = "";
      if (pAddGap)
        str3 = "\n\n";
      string str4 = $"{str3}{pString1.ColorHex(MetaTextReportHelper.color_text_quote)}  {str2}";
      if (pAddNameQuote)
        str4 = $"{str4}{pString2.ColorHex(pActor.kingdom.getColor().color_text)}  {pString3.ColorHex(ColorStyleLibrary.m.color_text_grey_dark)}";
      return str4;
    }
  }

  public static string addSingleUnitTextRandomUnit(IMetaObject pMetaObject, out Actor pActorResult)
  {
    pActorResult = (Actor) null;
    int num = 10;
    while (num-- > 0)
    {
      Actor randomUnit = pMetaObject.getRandomUnit();
      if (randomUnit != null && randomUnit.isAlive() && randomUnit.hasHappinessHistory())
      {
        string str = MetaTextReportHelper.addSingleUnitText(randomUnit);
        if (!string.IsNullOrEmpty(str))
        {
          pActorResult = randomUnit;
          return str;
        }
      }
    }
    return string.Empty;
  }

  public static string getText(
    IMetaObject pMetaObject,
    MetaTypeAsset pAsset,
    out Actor pActorResult)
  {
    pActorResult = (Actor) null;
    MetaTypeAsset metaTypeAsset = pAsset;
    string text = string.Empty;
    bool flag = false;
    foreach (string report in metaTypeAsset.reports)
    {
      MetaTextReportAsset metaTextReportAsset = AssetManager.meta_text_report_library.get(report);
      if (metaTextReportAsset.report_action(pMetaObject))
      {
        if (flag)
          text += " ";
        flag = true;
        string pString = metaTextReportAsset.get_random_text;
        if (metaTextReportAsset.color != null)
          pString = pString.ColorHex(metaTextReportAsset.color);
        text += pString;
      }
    }
    if (flag)
    {
      Actor pActorResult1;
      string pString = text + MetaTextReportHelper.addSingleUnitTextRandomUnit(pMetaObject, out pActorResult1);
      pActorResult = pActorResult1;
      text = pString.ColorHex(MetaTextReportHelper.color_text_main);
    }
    return text;
  }
}
