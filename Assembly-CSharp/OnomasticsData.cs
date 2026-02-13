// Decompiled with JetBrains decompiler
// Type: OnomasticsData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
public class OnomasticsData : IDisposable
{
  public const char SEPARATOR_GROUPS = '|';
  public const char SEPARATOR_PARTS = ';';
  public const char SEPARATOR_PART_CONTAINER = ':';
  public const int MAX_NAME_LENGTH = 30;
  public Dictionary<string, OnomasticsDataGroup> groups = new Dictionary<string, OnomasticsDataGroup>();
  private List<string> _template_data = new List<string>();
  private List<string> _current_subgroup = new List<string>();
  public const string DEFAULT_NAME_FAILED = "Rebr";

  public void setTemplateData(IReadOnlyList<string> pTemplateData)
  {
    this._template_data.Clear();
    this._template_data.AddRange((IEnumerable<string>) pTemplateData);
  }

  public List<string> getFullTemplateData() => this._template_data;

  public List<string> getCurrentSubgroup() => this._current_subgroup;

  public void addToTemplateData(string pID) => this._template_data.Add(pID);

  public void shuffleAllCards() => this._template_data.Shuffle<string>();

  public void clearTemplateData() => this._template_data.Clear();

  public string getGroupString(string pGroupID)
  {
    OnomasticsDataGroup pGroup;
    return this.tryGetGroup(pGroupID, out pGroup) ? pGroup.characters_string : (string) null;
  }

  public bool isGroupEmpty(string pGroupID) => this.isGroupEmpty(this.getGroup(pGroupID));

  public bool isGroupEmpty(OnomasticsDataGroup pGroup) => pGroup == null || pGroup.isEmpty();

  public OnomasticsDataGroup getGroup(string pGroupID)
  {
    OnomasticsDataGroup group;
    this.groups.TryGetValue(pGroupID, out group);
    return group;
  }

  public bool tryGetGroup(string pGroupID, out OnomasticsDataGroup pGroup)
  {
    return this.groups.TryGetValue(pGroupID, out pGroup) && !pGroup.isEmpty();
  }

  public string getRandomPartFromGroup(string pGroupID)
  {
    OnomasticsDataGroup pGroup;
    return this.tryGetGroup(pGroupID, out pGroup) ? pGroup.characters.GetRandom<string>() : string.Empty;
  }

  private bool saveGroup(string pGroupID, string pCharacters)
  {
    bool flag = true;
    OnomasticsDataGroup onomasticsDataGroup;
    if (!this.groups.TryGetValue(pGroupID, out onomasticsDataGroup))
    {
      onomasticsDataGroup = new OnomasticsDataGroup();
      this.groups.Add(pGroupID, onomasticsDataGroup);
      flag = false;
    }
    string charactersString = onomasticsDataGroup.characters_string;
    pCharacters = pCharacters.Replace("\n", " ");
    pCharacters = pCharacters.Replace("\r", " ");
    while (pCharacters.Contains("  "))
      pCharacters = pCharacters.Replace("  ", " ");
    pCharacters = pCharacters.Trim('\n', '\r', ' ');
    if (pCharacters.Length == 0)
    {
      onomasticsDataGroup.characters_string = string.Empty;
      onomasticsDataGroup.characters = (string[]) null;
    }
    else
    {
      string[] array = pCharacters.Split(' ', StringSplitOptions.RemoveEmptyEntries);
      Array.Sort<string>(array, (IComparer<string>) StringComparer.OrdinalIgnoreCase);
      onomasticsDataGroup.characters_string = string.Join(' ', array);
      onomasticsDataGroup.characters = array;
    }
    if (string.Equals(charactersString, onomasticsDataGroup.characters_string, StringComparison.OrdinalIgnoreCase))
      flag = false;
    return flag;
  }

  public void cloneFrom(OnomasticsData pOriginalData)
  {
    this._template_data.AddRange((IEnumerable<string>) pOriginalData._template_data);
    foreach (KeyValuePair<string, OnomasticsDataGroup> group in pOriginalData.groups)
      this.setGroup(group.Key, group.Value.characters_string);
  }

  public void setDebugTest()
  {
    this.loadFromShortTemplate("0312|0:pl p s l d b;1:mp rp rt b;2:kin tin le ee;3:a e i o u y oo");
  }

  internal static bool hasCheckOnTheRight(OnomasticsData pData, int pIndex)
  {
    List<string> currentSubgroup = pData.getCurrentSubgroup();
    int index = pIndex + 1;
    if (index >= currentSubgroup.Count)
      return false;
    string pID = currentSubgroup[index];
    return AssetManager.onomastics_library.get(pID).check_delegate != null;
  }

  internal static bool passesCheckOnTheRight(
    OnomasticsData pData,
    StringBuilderPool pLocalNameBuilder,
    string pLastPart,
    int pIndex,
    ActorSex pSex)
  {
    List<string> currentSubgroup = pData.getCurrentSubgroup();
    int num = pIndex + 1;
    if (num >= currentSubgroup.Count)
      return true;
    string pID = currentSubgroup[num];
    OnomasticsAsset pAsset = AssetManager.onomastics_library.get(pID);
    bool flag = pAsset.check_delegate(pAsset, pData, pLocalNameBuilder, pLastPart, num, pSex);
    return flag && OnomasticsData.hasCheckOnTheRight(pData, num) ? OnomasticsData.passesCheckOnTheRight(pData, pLocalNameBuilder, pLastPart, num, pSex) : flag;
  }

  public string generateName(ActorSex pSex = ActorSex.None, int pCalls = 0, long? pSeed = null)
  {
    if (pCalls > 100)
      return "Rebr";
    if (pSex == ActorSex.None)
      pSex = !Randy.randomBool() ? ActorSex.Male : ActorSex.Female;
    string pLastPart = string.Empty;
    if (pSeed.HasValue)
      Randy.resetSeed(pSeed.Value);
    using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
    {
      using (ListPool<string> subgroup = this.getSubgroup(this._template_data))
      {
        this._current_subgroup.Clear();
        this._current_subgroup.AddRange((IEnumerable<string>) subgroup);
        int count = this._current_subgroup.Count;
        bool flag = false;
        for (int index = 0; index < count; ++index)
        {
          string pID = this._current_subgroup[index];
          OnomasticsAsset pAsset = AssetManager.onomastics_library.get(pID);
          if (!OnomasticsData.hasCheckOnTheRight(this, index) || OnomasticsData.passesCheckOnTheRight(this, stringBuilderPool, pLastPart, index, pSex))
          {
            switch (pAsset.type)
            {
              case OnomasticsAssetType.Group:
              case OnomasticsAssetType.Special:
                OnomasticsNameMakerDelegate namemakerDelegate = pAsset.namemaker_delegate;
                string str = namemakerDelegate != null ? namemakerDelegate(pAsset, this, stringBuilderPool, pLastPart, index, pSex) : (string) null;
                if (str != null && str.Length > 0)
                {
                  pLastPart = str;
                  stringBuilderPool.Append(str);
                }
                if (pAsset.is_upper)
                {
                  flag = true;
                  continue;
                }
                continue;
              default:
                continue;
            }
          }
        }
        stringBuilderPool.Remove(',');
        stringBuilderPool.TrimEnd(' ', '-');
        if (stringBuilderPool.Length > 30)
          stringBuilderPool.Cut(0, 30);
        if (stringBuilderPool.Length == 0)
          return this.generateName(pSex, ++pCalls);
        if (Blacklist.checkBlackList(stringBuilderPool))
          return this.generateName(pSex, ++pCalls);
        if (flag)
          stringBuilderPool.ToUpperInvariant();
        else
          stringBuilderPool.ToTitleCase();
        return stringBuilderPool.ToString();
      }
    }
  }

  private ListPool<string> getSubgroup(List<string> pTemplateData)
  {
    int pMaxExclusive = OnomasticsData.countDividers(pTemplateData) + 1;
    return pMaxExclusive <= 1 ? new ListPool<string>((ICollection<string>) pTemplateData) : this.getSubgroupByIndex(Randy.randomInt(0, pMaxExclusive));
  }

  private ListPool<string> getSubgroupByIndex(int pIndex)
  {
    ListPool<string> subgroupByIndex = new ListPool<string>();
    int num = 0;
    foreach (string str in this._template_data)
    {
      if (str == "divider")
      {
        ++num;
        if (num > pIndex)
          break;
      }
      else if (num == pIndex)
        subgroupByIndex.Add(str);
    }
    return subgroupByIndex;
  }

  private static int countDividers(List<string> pTemplateData)
  {
    int num = 0;
    foreach (string str in pTemplateData)
    {
      if (str == "divider")
        ++num;
    }
    return num;
  }

  public static string convertToCultureTitleCase(string pString)
  {
    string pInput = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(pString.ToLower());
    if (pInput.Contains('\''))
      pInput = OnomasticsData.capitalizeAfterApostrophe(pInput);
    return pInput;
  }

  private static string capitalizeAfterApostrophe(string pInput)
  {
    TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
    char[] charArray = pInput.ToCharArray();
    bool flag = false;
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (flag && char.IsLetter(charArray[index]))
      {
        charArray[index] = textInfo.ToUpper(charArray[index]);
        flag = false;
      }
      if (charArray[index] == '\'')
        flag = true;
    }
    return new string(charArray);
  }

  public string getShortTemplate()
  {
    using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
    {
      foreach (string pID in this._template_data)
      {
        OnomasticsAsset onomasticsAsset = AssetManager.onomastics_library.get(pID);
        stringBuilderPool.Append(onomasticsAsset.short_id);
      }
      stringBuilderPool.Append('|');
      bool flag = true;
      foreach (KeyValuePair<string, OnomasticsDataGroup> keyValuePair in (IEnumerable<KeyValuePair<string, OnomasticsDataGroup>>) this.groups.OrderBy<KeyValuePair<string, OnomasticsDataGroup>, char>((Func<KeyValuePair<string, OnomasticsDataGroup>, char>) (p => AssetManager.onomastics_library.get(p.Key).short_id)))
      {
        if (!keyValuePair.Value.isEmpty())
        {
          if (!flag)
            stringBuilderPool.Append(';');
          OnomasticsAsset onomasticsAsset = AssetManager.onomastics_library.get(keyValuePair.Key);
          stringBuilderPool.Append(onomasticsAsset.short_id);
          stringBuilderPool.Append(':');
          stringBuilderPool.Append(keyValuePair.Value.characters_string.ToLower());
          flag = false;
        }
      }
      return stringBuilderPool.ToString();
    }
  }

  public bool templateIsValid(string pShortTemplate)
  {
    return !string.IsNullOrEmpty(pShortTemplate) && pShortTemplate.Contains('|') && pShortTemplate.Split('|', StringSplitOptions.None).Length == 2;
  }

  public void loadFromShortTemplate(string pShortTemplate)
  {
    if (string.IsNullOrEmpty(pShortTemplate))
      pShortTemplate = "|";
    pShortTemplate = pShortTemplate.Contains('|') ? pShortTemplate.Trim('\n', '\r', ' ', '"', '`') : throw new ArgumentException("Invalid template format: (NS) " + pShortTemplate);
    if (pShortTemplate.Contains('\n'))
      throw new ArgumentException("Invalid template format: (NL) " + pShortTemplate);
    string[] strArray1 = !pShortTemplate.Contains('\r') ? pShortTemplate.Split('|', StringSplitOptions.None) : throw new ArgumentException("Invalid template format: (NL) " + pShortTemplate);
    char[] chArray = strArray1.Length == 2 ? strArray1[0].ToCharArray() : throw new ArgumentException($"Invalid template format: (L{strArray1.Length}) " + pShortTemplate);
    using (ListPool<string> pTemplateData = new ListPool<string>(chArray.Length))
    {
      foreach (char ch in chArray)
        pTemplateData.Add((AssetManager.onomastics_library.getByShortId(ch.ToString()) ?? throw new ArgumentException($"Invalid template format: (0{ch}) " + pShortTemplate)).id);
      this.setTemplateData((IReadOnlyList<string>) pTemplateData);
      string[] strArray2 = strArray1[1].Split(';', StringSplitOptions.None);
      this.groups.Clear();
      foreach (string str in strArray2)
      {
        string[] strArray3 = str.Split(':', StringSplitOptions.None);
        if (strArray3.Length == 2)
        {
          string pShortID = strArray3[0];
          string pCharacters = strArray3[1];
          this.saveGroup((AssetManager.onomastics_library.getByShortId(pShortID) ?? throw new ArgumentException($"Invalid template format: (G{pShortID}) {pShortTemplate}")).id, pCharacters);
        }
      }
    }
  }

  public bool isGendered()
  {
    foreach (string pID in this._template_data)
    {
      OnomasticsAsset onomasticsAsset = AssetManager.onomastics_library.get(pID);
      if (onomasticsAsset.id == "sex_male" || onomasticsAsset.id == "sex_female")
        return true;
    }
    return false;
  }

  public bool isEmpty() => this._template_data.Count == 0;

  public bool setGroup(string pID, string pString) => this.saveGroup(pID, pString);

  public void Dispose()
  {
    this.groups.Clear();
    this.groups = (Dictionary<string, OnomasticsDataGroup>) null;
    this._template_data.Clear();
    this._template_data = (List<string>) null;
    this._current_subgroup.Clear();
    this._current_subgroup = (List<string>) null;
  }
}
