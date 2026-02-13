// Decompiled with JetBrains decompiler
// Type: OnomasticsLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
public class OnomasticsLibrary : AssetLibrary<OnomasticsAsset>
{
  private Dictionary<string, OnomasticsAsset> _dict_short_id = new Dictionary<string, OnomasticsAsset>();
  [NonSerialized]
  public List<OnomasticsAsset> list_special = new List<OnomasticsAsset>();
  private static readonly char[] _word_separators = new char[3]
  {
    ' ',
    ',',
    '-'
  };
  private static Random _rand = new Random();
  private Dictionary<string, string> _test_templates = new Dictionary<string, string>()
  {
    {
      "01|0:a;1:b",
      "Ab"
    },
    {
      "0_1|0:a;1:b",
      "A B"
    },
    {
      "01b|0:a;1:b",
      "A"
    },
    {
      "o|0:a;1:b;2:c;5:f",
      "Abcf"
    },
    {
      "110c|0:a;1:b",
      "Bab"
    },
    {
      "001c|0:a;1:b",
      "Aa"
    },
    {
      "001v|0:a;1:b",
      "Aba"
    },
    {
      "110v|0:a;1:b",
      "Bb"
    },
    {
      "110C|0:a;1:b",
      "Bba"
    },
    {
      "001C|0:a;1:b",
      "Aa"
    },
    {
      "001V|0:a;1:b",
      "Aab"
    },
    {
      "110V|0:a;1:b",
      "Bb"
    },
    {
      "10E|0:a;1:b",
      "A"
    },
    {
      "01E|0:a;1:b",
      "A"
    },
    {
      "01e|0:a;1:b",
      "B"
    },
    {
      "10e|0:a;1:b",
      "B"
    },
    {
      "1,10E|0:a;1:b",
      "Ba"
    },
    {
      "1-10E|0:a;1:b",
      "B-A"
    },
    {
      "0,01e|0:a;1:b",
      "Ab"
    },
    {
      "0-01e|0:a;1:b",
      "A-B"
    },
    {
      "01D|0:a;1:b",
      "Abb"
    },
    {
      "00D|0:a;1:b",
      "Aa"
    },
    {
      "01d|0:a;1:b",
      "Aab"
    },
    {
      "11d|0:a;1:b",
      "Bb"
    },
    {
      "0r|0:a",
      "Aaa"
    },
    {
      "0rr|0:a",
      "Aaaaaaa"
    },
    {
      "0rbu|0:a",
      "AA"
    }
  };

  public override void init()
  {
    base.init();
    this.addGroups();
    this.addDice();
    this.addUnique();
    this.addSpecialCharacters();
    this.addVowelConsonantSpecials();
    this.addSexes();
  }

  private void addUnique()
  {
    OnomasticsAsset pAsset1 = new OnomasticsAsset();
    pAsset1.id = "clone_last";
    pAsset1.short_id = 'l';
    pAsset1.color_text = "#EB2931";
    pAsset1.type = OnomasticsAssetType.Special;
    pAsset1.namemaker_delegate = (OnomasticsNameMakerDelegate) ((_1, _2, pStringBuilder, _3, _4, _5) => pStringBuilder.Length == 0 ? string.Empty : this.getLastWord(pStringBuilder));
    pAsset1.affects_left_word = true;
    this.add(pAsset1);
    OnomasticsAsset pAsset2 = new OnomasticsAsset();
    pAsset2.id = "coin_flip";
    pAsset2.short_id = 'f';
    pAsset2.color_text = "#F5C308";
    pAsset2.type = OnomasticsAssetType.Special;
    pAsset2.check_delegate = (OnomasticsCheckDelegate) ((_6, _7, _8, _9, _10, _11) => Randy.randomBool());
    pAsset2.affects_left = true;
    this.add(pAsset2);
    OnomasticsAsset pAsset3 = new OnomasticsAsset();
    pAsset3.id = "mirror";
    pAsset3.short_id = 'm';
    pAsset3.color_text = "#FFFFFF";
    pAsset3.type = OnomasticsAssetType.Special;
    pAsset3.namemaker_delegate = (OnomasticsNameMakerDelegate) ((_12, _13, pStringBuilder, _14, _15, _16) => pStringBuilder.Length == 0 ? string.Empty : this.getLastWord(pStringBuilder).Reverse());
    pAsset3.affects_left_word = true;
    this.add(pAsset3);
    OnomasticsAsset pAsset4 = new OnomasticsAsset();
    pAsset4.id = "wild_6";
    pAsset4.short_id = 'w';
    pAsset4.color_text = "#A1B1A2";
    pAsset4.type = OnomasticsAssetType.Special;
    pAsset4.namemaker_delegate = (OnomasticsNameMakerDelegate) ((_, pData, pStringBuilder, pLastPart, pIndex, pSex) =>
    {
      using (ListPool<string> list = new ListPool<string>(pData.groups.Count))
      {
        foreach (KeyValuePair<string, OnomasticsDataGroup> group in pData.groups)
        {
          if (!group.Value.isEmpty() && AssetManager.onomastics_library.get(group.Key).group_id < 6)
            list.Add(group.Key);
        }
        if (list.Count == 0)
          return string.Empty;
        string random = OnomasticsLibrary.GetRandom<string>(list);
        OnomasticsAsset pAsset5 = AssetManager.onomastics_library.get(random);
        return pAsset5.namemaker_delegate(pAsset5, pData, pStringBuilder, pLastPart, pIndex, pSex);
      }
    });
    this.add(pAsset4);
    OnomasticsAsset pAsset6 = new OnomasticsAsset();
    pAsset6.id = "domino";
    pAsset6.short_id = 'o';
    pAsset6.color_text = "#7FA9BC";
    pAsset6.type = OnomasticsAssetType.Special;
    pAsset6.namemaker_delegate = (OnomasticsNameMakerDelegate) ((_, pData, pStringBuilder, pLastPart, pIndex, pSex) =>
    {
      string str = "";
      for (int index = 0; index < 6; ++index)
      {
        string pID = $"group_{index + 1}";
        OnomasticsAsset pAsset7 = AssetManager.onomastics_library.get(pID);
        str += pAsset7.namemaker_delegate(pAsset7, pData, pStringBuilder, pLastPart, pIndex, pSex);
      }
      return str;
    });
    this.add(pAsset6);
    OnomasticsAsset pAsset8 = new OnomasticsAsset();
    pAsset8.id = "repeater";
    pAsset8.short_id = 'r';
    pAsset8.color_text = "#F3AB11";
    pAsset8.type = OnomasticsAssetType.Special;
    pAsset8.namemaker_delegate = (OnomasticsNameMakerDelegate) ((_, pData, pStringBuilder, pLastPart, pIndex, pSex) =>
    {
      if (pIndex == 0)
        return string.Empty;
      int num = pIndex - 1;
      List<string> currentSubgroup = pData.getCurrentSubgroup();
      OnomasticsAsset pAsset9 = AssetManager.onomastics_library.get(currentSubgroup[num]);
      if (pAsset9.is_divider)
        return string.Empty;
      while (pAsset9.namemaker_delegate == null)
      {
        --num;
        if (num < 0)
          return string.Empty;
        pAsset9 = AssetManager.onomastics_library.get(currentSubgroup[num]);
        if (pAsset9.is_divider)
          return string.Empty;
      }
      if (pAsset9.namemaker_delegate == null)
        return string.Empty;
      string str = "";
      for (int index = 0; index < 2; ++index)
      {
        if (!OnomasticsData.hasCheckOnTheRight(pData, num) || OnomasticsData.passesCheckOnTheRight(pData, pStringBuilder, pLastPart, num, pSex))
          str += pAsset9.namemaker_delegate(pAsset9, pData, pStringBuilder, pLastPart, num, pSex);
      }
      return str;
    });
    pAsset8.affects_left = true;
    this.add(pAsset8);
    OnomasticsAsset pAsset10 = new OnomasticsAsset();
    pAsset10.id = "upper";
    pAsset10.short_id = 'u';
    pAsset10.color_text = "#B2FF75";
    pAsset10.is_upper = true;
    pAsset10.type = OnomasticsAssetType.Special;
    pAsset10.affects_everything = true;
    this.add(pAsset10);
    OnomasticsAsset pAsset11 = new OnomasticsAsset();
    pAsset11.id = "backspace";
    pAsset11.short_id = 'b';
    pAsset11.color_text = "#FF6A43";
    pAsset11.type = OnomasticsAssetType.Special;
    pAsset11.namemaker_delegate = (OnomasticsNameMakerDelegate) ((_17, _18, pStringBuilder, _19, _20, _21) =>
    {
      if (pStringBuilder.Length == 0)
        return string.Empty;
      pStringBuilder.Remove(pStringBuilder.Length - 1, 1);
      return string.Empty;
    });
    this.add(pAsset11);
  }

  private void addVowelConsonantSpecials()
  {
    OnomasticsAsset pAsset1 = new OnomasticsAsset();
    pAsset1.id = "consonant_separator";
    pAsset1.short_id = 'c';
    pAsset1.color_text = "#657CCE";
    pAsset1.type = OnomasticsAssetType.Special;
    pAsset1.namemaker_delegate = (OnomasticsNameMakerDelegate) ((_1, pData, pStringBuilder, _2, pIndex, _3) =>
    {
      if (pIndex == 0)
        return string.Empty;
      int index = pIndex - 1;
      List<string> currentSubgroup = pData.getCurrentSubgroup();
      OnomasticsAsset onomasticsAsset = AssetManager.onomastics_library.get(currentSubgroup[index]);
      if (!onomasticsAsset.isGroupType() || pData.isGroupEmpty(onomasticsAsset.id))
        return string.Empty;
      ConsonantSeparator.addRandomVowels(pStringBuilder, pData.getGroup(onomasticsAsset.id).characters);
      return string.Empty;
    });
    pAsset1.check_delegate = new OnomasticsCheckDelegate(this.blockGroupOnLeft);
    pAsset1.affects_left = true;
    pAsset1.affects_left_word = true;
    pAsset1.affects_left_group_only = true;
    this.add(pAsset1);
    OnomasticsAsset pAsset2 = new OnomasticsAsset();
    pAsset2.id = "vowel_separator";
    pAsset2.short_id = 'v';
    pAsset2.color_text = "#FF68C5";
    pAsset2.type = OnomasticsAssetType.Special;
    pAsset2.namemaker_delegate = (OnomasticsNameMakerDelegate) ((_4, pData, pStringBuilder, _5, pIndex, _6) =>
    {
      if (pIndex == 0)
        return string.Empty;
      int index = pIndex - 1;
      List<string> currentSubgroup = pData.getCurrentSubgroup();
      OnomasticsAsset onomasticsAsset = AssetManager.onomastics_library.get(currentSubgroup[index]);
      if (!onomasticsAsset.isGroupType() || pData.isGroupEmpty(onomasticsAsset.id))
        return string.Empty;
      VowelSeparator.addRandomConsonants(pStringBuilder, pData.getGroup(onomasticsAsset.id).characters);
      return string.Empty;
    });
    pAsset2.check_delegate = new OnomasticsCheckDelegate(this.blockGroupOnLeft);
    pAsset2.affects_left = true;
    pAsset2.affects_left_word = true;
    pAsset2.affects_left_group_only = true;
    this.add(pAsset2);
    OnomasticsAsset pAsset3 = new OnomasticsAsset();
    pAsset3.id = "vowel_duplicator";
    pAsset3.short_id = 'd';
    pAsset3.color_text = "#D9B03A";
    pAsset3.type = OnomasticsAssetType.Special;
    pAsset3.namemaker_delegate = (OnomasticsNameMakerDelegate) ((_7, _8, pStringBuilder, _9, _10, _11) =>
    {
      if (pStringBuilder.Length == 0)
        return string.Empty;
      (int num3, int num4) = this.getLastWordBounds(pStringBuilder);
      using (ListPool<int> allSingleVowels = VowelSeparator.findAllSingleVowels(pStringBuilder, num3, num4))
      {
        if (allSingleVowels.Count == 0)
          return string.Empty;
        int random = OnomasticsLibrary.GetRandom<int>(allSingleVowels);
        pStringBuilder.Insert(random, pStringBuilder[random]);
        return string.Empty;
      }
    });
    pAsset3.affects_left_word = true;
    this.add(pAsset3);
    OnomasticsAsset pAsset4 = new OnomasticsAsset();
    pAsset4.id = "consonant_duplicator";
    pAsset4.short_id = 'D';
    pAsset4.color_text = "#2E76AD";
    pAsset4.type = OnomasticsAssetType.Special;
    pAsset4.namemaker_delegate = (OnomasticsNameMakerDelegate) ((_12, _13, pStringBuilder, _14, _15, _16) =>
    {
      if (pStringBuilder.Length == 0)
        return string.Empty;
      (int num7, int num8) = this.getLastWordBounds(pStringBuilder);
      using (ListPool<int> singleConsonants = ConsonantSeparator.findAllSingleConsonants(pStringBuilder, num7, num8))
      {
        if (singleConsonants.Count == 0)
          return string.Empty;
        int random = OnomasticsLibrary.GetRandom<int>(singleConsonants);
        pStringBuilder.Insert(random, pStringBuilder[random]);
        return string.Empty;
      }
    });
    pAsset4.affects_left_word = true;
    this.add(pAsset4);
    OnomasticsAsset pAsset5 = new OnomasticsAsset();
    pAsset5.id = "vowel_replacer";
    pAsset5.short_id = 'e';
    pAsset5.color_text = "#FF68C5";
    pAsset5.type = OnomasticsAssetType.Special;
    pAsset5.namemaker_delegate = (OnomasticsNameMakerDelegate) ((_17, pData, pStringBuilder, _18, pIndex, _19) =>
    {
      if (pIndex == 0)
        return string.Empty;
      int index = pIndex - 1;
      List<string> currentSubgroup = pData.getCurrentSubgroup();
      OnomasticsAsset onomasticsAsset = AssetManager.onomastics_library.get(currentSubgroup[index]);
      if (!onomasticsAsset.isGroupType() || pData.isGroupEmpty(onomasticsAsset.id))
        return string.Empty;
      (int num11, int num12) = this.getLastWordBounds(pStringBuilder);
      using (ListPool<int> allVowels = VowelSeparator.findAllVowels(pStringBuilder, num11, num12))
      {
        if (allVowels.Count == 0)
          return string.Empty;
        string random1 = OnomasticsLibrary.GetRandom<string>(pData.getGroup(onomasticsAsset.id).characters);
        int random2 = OnomasticsLibrary.GetRandom<int>(allVowels);
        pStringBuilder.Remove(random2, 1);
        pStringBuilder.Insert(random2, random1);
        return string.Empty;
      }
    });
    pAsset5.check_delegate = new OnomasticsCheckDelegate(this.blockGroupOnLeft);
    pAsset5.affects_left = true;
    pAsset5.affects_left_word = true;
    pAsset5.affects_left_group_only = true;
    this.add(pAsset5);
    OnomasticsAsset pAsset6 = new OnomasticsAsset();
    pAsset6.id = "consonant_replacer";
    pAsset6.short_id = 'E';
    pAsset6.color_text = "#A8A8A8";
    pAsset6.type = OnomasticsAssetType.Special;
    pAsset6.namemaker_delegate = (OnomasticsNameMakerDelegate) ((_20, pData, pStringBuilder, _21, pIndex, _22) =>
    {
      if (pIndex == 0)
        return string.Empty;
      int index = pIndex - 1;
      List<string> currentSubgroup = pData.getCurrentSubgroup();
      OnomasticsAsset onomasticsAsset = AssetManager.onomastics_library.get(currentSubgroup[index]);
      if (!onomasticsAsset.isGroupType() || pData.isGroupEmpty(onomasticsAsset.id))
        return string.Empty;
      (int num15, int num16) = this.getLastWordBounds(pStringBuilder);
      using (ListPool<int> allConsonants = ConsonantSeparator.findAllConsonants(pStringBuilder, num15, num16))
      {
        if (allConsonants.Count == 0)
          return string.Empty;
        string random3 = OnomasticsLibrary.GetRandom<string>(pData.getGroup(onomasticsAsset.id).characters);
        int random4 = OnomasticsLibrary.GetRandom<int>(allConsonants);
        pStringBuilder.Remove(random4, 1);
        pStringBuilder.Insert(random4, random3);
        return string.Empty;
      }
    });
    pAsset6.check_delegate = new OnomasticsCheckDelegate(this.blockGroupOnLeft);
    pAsset6.affects_left = true;
    pAsset6.affects_left_word = true;
    pAsset6.affects_left_group_only = true;
    this.add(pAsset6);
    OnomasticsAsset pAsset7 = new OnomasticsAsset();
    pAsset7.id = "vowel_requirer";
    pAsset7.short_id = 'V';
    pAsset7.color_text = "#FBFDC2";
    pAsset7.type = OnomasticsAssetType.Special;
    pAsset7.check_delegate = (OnomasticsCheckDelegate) ((_23, _24, pStringBuilder, _25, _26, _27) =>
    {
      if (pStringBuilder.Length == 0)
        return false;
      StringBuilderPool stringBuilderPool = pStringBuilder;
      return VowelSeparator.isVowel(stringBuilderPool[stringBuilderPool.Length - 1]);
    });
    pAsset7.affects_left = true;
    this.add(pAsset7);
    OnomasticsAsset pAsset8 = new OnomasticsAsset();
    pAsset8.id = "consonant_requirer";
    pAsset8.short_id = 'C';
    pAsset8.color_text = "#CAB2A8";
    pAsset8.type = OnomasticsAssetType.Special;
    pAsset8.check_delegate = (OnomasticsCheckDelegate) ((_28, _29, pStringBuilder, _30, _31, _32) =>
    {
      if (pStringBuilder.Length == 0)
        return false;
      StringBuilderPool stringBuilderPool = pStringBuilder;
      return ConsonantSeparator.isConsonant(stringBuilderPool[stringBuilderPool.Length - 1]);
    });
    pAsset8.affects_left = true;
    this.add(pAsset8);
  }

  private void addDice()
  {
  }

  private void addGroups()
  {
    OnomasticsAsset pAsset1 = new OnomasticsAsset();
    pAsset1.id = "group_1";
    pAsset1.short_id = '0';
    pAsset1.color_text = "#31EB29";
    pAsset1.forced_locale_subname_id = "onomastics_group_subname";
    pAsset1.forced_locale_description_id = "onomastics_group_info";
    pAsset1.forced_locale_description_id_2 = "onomastics_group_info_2";
    pAsset1.type = OnomasticsAssetType.Group;
    pAsset1.namemaker_delegate = new OnomasticsNameMakerDelegate(this.groupAction);
    pAsset1.group_id = 0;
    this.add(pAsset1);
    OnomasticsAsset pAsset2 = new OnomasticsAsset();
    pAsset2.id = "group_2";
    pAsset2.short_id = '1';
    pAsset2.color_text = "#28ECA0";
    pAsset2.forced_locale_subname_id = "onomastics_group_subname";
    pAsset2.forced_locale_description_id = "onomastics_group_info";
    pAsset2.forced_locale_description_id_2 = "onomastics_group_info_2";
    pAsset2.type = OnomasticsAssetType.Group;
    pAsset2.namemaker_delegate = new OnomasticsNameMakerDelegate(this.groupAction);
    pAsset2.group_id = 1;
    this.add(pAsset2);
    OnomasticsAsset pAsset3 = new OnomasticsAsset();
    pAsset3.id = "group_3";
    pAsset3.short_id = '2';
    pAsset3.color_text = "#3FABE9";
    pAsset3.forced_locale_subname_id = "onomastics_group_subname";
    pAsset3.forced_locale_description_id = "onomastics_group_info";
    pAsset3.forced_locale_description_id_2 = "onomastics_group_info_2";
    pAsset3.type = OnomasticsAssetType.Group;
    pAsset3.namemaker_delegate = new OnomasticsNameMakerDelegate(this.groupAction);
    pAsset3.group_id = 2;
    this.add(pAsset3);
    OnomasticsAsset pAsset4 = new OnomasticsAsset();
    pAsset4.id = "group_4";
    pAsset4.short_id = '3';
    pAsset4.color_text = "#6A78FF";
    pAsset4.forced_locale_subname_id = "onomastics_group_subname";
    pAsset4.forced_locale_description_id = "onomastics_group_info";
    pAsset4.forced_locale_description_id_2 = "onomastics_group_info_2";
    pAsset4.type = OnomasticsAssetType.Group;
    pAsset4.namemaker_delegate = new OnomasticsNameMakerDelegate(this.groupAction);
    pAsset4.group_id = 3;
    this.add(pAsset4);
    OnomasticsAsset pAsset5 = new OnomasticsAsset();
    pAsset5.id = "group_5";
    pAsset5.short_id = '4';
    pAsset5.color_text = "#A129EB";
    pAsset5.forced_locale_subname_id = "onomastics_group_subname";
    pAsset5.forced_locale_description_id = "onomastics_group_info";
    pAsset5.forced_locale_description_id_2 = "onomastics_group_info_2";
    pAsset5.type = OnomasticsAssetType.Group;
    pAsset5.namemaker_delegate = new OnomasticsNameMakerDelegate(this.groupAction);
    pAsset5.group_id = 4;
    this.add(pAsset5);
    OnomasticsAsset pAsset6 = new OnomasticsAsset();
    pAsset6.id = "group_6";
    pAsset6.short_id = '5';
    pAsset6.color_text = "#EB29B3";
    pAsset6.forced_locale_subname_id = "onomastics_group_subname";
    pAsset6.forced_locale_description_id = "onomastics_group_info";
    pAsset6.forced_locale_description_id_2 = "onomastics_group_info_2";
    pAsset6.type = OnomasticsAssetType.Group;
    pAsset6.namemaker_delegate = new OnomasticsNameMakerDelegate(this.groupAction);
    pAsset6.group_id = 5;
    this.add(pAsset6);
    OnomasticsAsset pAsset7 = new OnomasticsAsset();
    pAsset7.id = "group_7";
    pAsset7.short_id = '6';
    pAsset7.color_text = "#EB2931";
    pAsset7.forced_locale_subname_id = "onomastics_group_subname";
    pAsset7.forced_locale_description_id = "onomastics_group_info";
    pAsset7.forced_locale_description_id_2 = "onomastics_group_info_2";
    pAsset7.type = OnomasticsAssetType.Group;
    pAsset7.namemaker_delegate = new OnomasticsNameMakerDelegate(this.groupAction);
    pAsset7.group_id = 6;
    this.add(pAsset7);
    OnomasticsAsset pAsset8 = new OnomasticsAsset();
    pAsset8.id = "group_8";
    pAsset8.short_id = '7';
    pAsset8.color_text = "#FF8D1A";
    pAsset8.forced_locale_subname_id = "onomastics_group_subname";
    pAsset8.forced_locale_description_id = "onomastics_group_info";
    pAsset8.forced_locale_description_id_2 = "onomastics_group_info_2";
    pAsset8.type = OnomasticsAssetType.Group;
    pAsset8.namemaker_delegate = new OnomasticsNameMakerDelegate(this.groupAction);
    pAsset8.group_id = 7;
    this.add(pAsset8);
    OnomasticsAsset pAsset9 = new OnomasticsAsset();
    pAsset9.id = "group_9";
    pAsset9.short_id = '8';
    pAsset9.color_text = "#EFCB00";
    pAsset9.forced_locale_subname_id = "onomastics_group_subname";
    pAsset9.forced_locale_description_id = "onomastics_group_info";
    pAsset9.forced_locale_description_id_2 = "onomastics_group_info_2";
    pAsset9.type = OnomasticsAssetType.Group;
    pAsset9.namemaker_delegate = new OnomasticsNameMakerDelegate(this.groupAction);
    pAsset9.group_id = 8;
    this.add(pAsset9);
    OnomasticsAsset pAsset10 = new OnomasticsAsset();
    pAsset10.id = "group_10";
    pAsset10.short_id = '9';
    pAsset10.color_text = "#D2DAC0";
    pAsset10.forced_locale_subname_id = "onomastics_group_subname";
    pAsset10.forced_locale_description_id = "onomastics_group_info";
    pAsset10.forced_locale_description_id_2 = "onomastics_group_info_2";
    pAsset10.type = OnomasticsAssetType.Group;
    pAsset10.namemaker_delegate = new OnomasticsNameMakerDelegate(this.groupAction);
    pAsset10.group_id = 9;
    this.add(pAsset10);
  }

  private void addSpecialCharacters()
  {
    OnomasticsAsset pAsset1 = new OnomasticsAsset();
    pAsset1.id = "space";
    pAsset1.short_id = '_';
    pAsset1.color_text = "#D2DAC0";
    pAsset1.type = OnomasticsAssetType.Special;
    pAsset1.namemaker_delegate = (OnomasticsNameMakerDelegate) ((_1, _2, pStringBuilder, _3, _4, _5) =>
    {
      if (pStringBuilder.Length == 0)
        return string.Empty;
      StringBuilderPool stringBuilderPool = pStringBuilder;
      return stringBuilderPool[stringBuilderPool.Length - 1] == ' ' ? string.Empty : " ";
    });
    pAsset1.is_word_divider = true;
    this.add(pAsset1);
    OnomasticsAsset pAsset2 = new OnomasticsAsset();
    pAsset2.id = "silent_space";
    pAsset2.short_id = ',';
    pAsset2.color_text = "#C93F3F";
    pAsset2.type = OnomasticsAssetType.Special;
    pAsset2.namemaker_delegate = (OnomasticsNameMakerDelegate) ((_6, _7, pStringBuilder, _8, _9, _10) =>
    {
      if (pStringBuilder.Length == 0)
        return string.Empty;
      StringBuilderPool stringBuilderPool1 = pStringBuilder;
      if (stringBuilderPool1[stringBuilderPool1.Length - 1] == ' ')
        return string.Empty;
      StringBuilderPool stringBuilderPool2 = pStringBuilder;
      return stringBuilderPool2[stringBuilderPool2.Length - 1] == ',' ? string.Empty : ",";
    });
    pAsset2.is_word_divider = true;
    this.add(pAsset2);
    OnomasticsAsset pAsset3 = new OnomasticsAsset();
    pAsset3.id = "hyphen";
    pAsset3.short_id = '-';
    pAsset3.color_text = "#BBA826";
    pAsset3.type = OnomasticsAssetType.Special;
    pAsset3.namemaker_delegate = (OnomasticsNameMakerDelegate) ((_11, _12, _13, _14, _15, _16) => "-");
    pAsset3.is_word_divider = true;
    this.add(pAsset3);
    OnomasticsAsset pAsset4 = new OnomasticsAsset();
    pAsset4.id = "numbers";
    pAsset4.short_id = '#';
    pAsset4.color_text = "#35B929";
    pAsset4.type = OnomasticsAssetType.Special;
    pAsset4.namemaker_delegate = (OnomasticsNameMakerDelegate) ((_17, _18, _19, _20, _21, _22) => Randy.randomInt(0, 10).ToString());
    this.add(pAsset4);
    OnomasticsAsset pAsset5 = new OnomasticsAsset();
    pAsset5.id = "divider";
    pAsset5.short_id = '/';
    pAsset5.color_text = "#D2DAC0";
    pAsset5.type = OnomasticsAssetType.Special;
    pAsset5.is_divider = true;
    pAsset5.is_word_divider = true;
    pAsset5.is_immune = true;
    this.add(pAsset5);
  }

  private void addSexes()
  {
    OnomasticsAsset pAsset1 = new OnomasticsAsset();
    pAsset1.id = "sex_male";
    pAsset1.short_id = 'M';
    pAsset1.color_text = "#86BC4E";
    pAsset1.type = OnomasticsAssetType.Special;
    pAsset1.check_delegate = (OnomasticsCheckDelegate) ((_1, _2, _3, _4, _5, pSex) => pSex == ActorSex.Male);
    pAsset1.affects_left = true;
    this.add(pAsset1);
    OnomasticsAsset pAsset2 = new OnomasticsAsset();
    pAsset2.id = "sex_female";
    pAsset2.short_id = 'F';
    pAsset2.color_text = "#EB29B3";
    pAsset2.type = OnomasticsAssetType.Special;
    pAsset2.check_delegate = (OnomasticsCheckDelegate) ((_6, _7, _8, _9, _10, pSex) => pSex == ActorSex.Female);
    pAsset2.affects_left = true;
    this.add(pAsset2);
  }

  private string getLastWord(StringBuilderPool pStringBuilder)
  {
    (int num1, int num2) = this.getLastWordBounds(pStringBuilder, true);
    return pStringBuilder.ToString(num1, num2);
  }

  private (int tStart, int tLength) getLastWordBounds(
    StringBuilderPool pStringBuilder,
    bool pPrevious = false)
  {
    if (pStringBuilder.Length == 0)
      return (0, 0);
    int num1 = pStringBuilder.LastIndexOfAny(OnomasticsLibrary._word_separators) + 1;
    int num2 = pStringBuilder.Length - num1;
    if (pPrevious && num2 == 0)
    {
      num1 = pStringBuilder.LastIndexOfAny(OnomasticsLibrary._word_separators, num1 - 2) + 1;
      num2 = pStringBuilder.Length - num1 - (num2 + 1);
    }
    return (num1, num2);
  }

  private string groupAction(
    OnomasticsAsset pAsset,
    OnomasticsData pData,
    StringBuilderPool pStringBuilder,
    string pLastPart,
    int pIndex,
    ActorSex pSex)
  {
    return pData.getRandomPartFromGroup(pAsset.id);
  }

  private bool blockGroupOnLeft(
    OnomasticsAsset pAsset,
    OnomasticsData pData,
    StringBuilderPool pStringBuilder,
    string pLastPart,
    int pIndex,
    ActorSex pSex)
  {
    return !this.isGroupOnLeft(pAsset, pData, pStringBuilder, pLastPart, pIndex, pSex);
  }

  private bool isGroupOnLeft(
    OnomasticsAsset pAsset,
    OnomasticsData pData,
    StringBuilderPool pStringBuilder,
    string pLastPart,
    int pIndex,
    ActorSex pSex)
  {
    if (pIndex == 0)
      return false;
    int index = pIndex - 1;
    List<string> currentSubgroup = pData.getCurrentSubgroup();
    OnomasticsAsset onomasticsAsset = AssetManager.onomastics_library.get(currentSubgroup[index]);
    return onomasticsAsset.isGroupType() && !pData.isGroupEmpty(onomasticsAsset.id);
  }

  public override OnomasticsAsset add(OnomasticsAsset pAsset)
  {
    base.add(pAsset);
    this._dict_short_id.Add(pAsset.short_id.ToString(), pAsset);
    return pAsset;
  }

  public OnomasticsAsset getByShortId(string pShortID)
  {
    return this._dict_short_id.ContainsKey(pShortID) ? this._dict_short_id[pShortID] : (OnomasticsAsset) null;
  }

  public static T GetRandom<T>(T[] list)
  {
    ((Random) ref OnomasticsLibrary._rand).InitState(Randy.rand.state);
    return list[((Random) ref OnomasticsLibrary._rand).NextInt(list.Length)];
  }

  public static T GetRandom<T>(ListPool<T> list)
  {
    ((Random) ref OnomasticsLibrary._rand).InitState(Randy.rand.state);
    return list[((Random) ref OnomasticsLibrary._rand).NextInt(list.Count)];
  }

  public override void post_init()
  {
    base.post_init();
    foreach (OnomasticsAsset onomasticsAsset in this.list)
    {
      if (string.IsNullOrEmpty(onomasticsAsset.path_icon))
        onomasticsAsset.path_icon = "ui/Icons/onomastics/onomastics_" + onomasticsAsset.id;
    }
  }

  public override void editorDiagnostic()
  {
    foreach (OnomasticsAsset onomasticsAsset in this.list)
    {
      if (Object.op_Equality((Object) SpriteTextureLoader.getSprite(onomasticsAsset.path_icon), (Object) null))
        BaseAssetLibrary.logAssetError("Missing icon file", onomasticsAsset.path_icon);
    }
    foreach (OnomasticsAsset onomasticsAsset1 in this.list)
    {
      foreach (OnomasticsAsset onomasticsAsset2 in this.list)
      {
        if (onomasticsAsset1 != onomasticsAsset2 && (int) onomasticsAsset1.short_id == (int) onomasticsAsset2.short_id)
          Debug.LogError((object) $"OnomasticsAsset: Duplicate short_id: {onomasticsAsset1.short_id.ToString()} for {onomasticsAsset1.id} and {onomasticsAsset2.id}");
      }
    }
    foreach (KeyValuePair<string, string> testTemplate in this._test_templates)
    {
      string key = testTemplate.Key;
      string str = testTemplate.Value;
      string nameFromOnomastics = NameGenerator.generateNameFromOnomastics((NameGeneratorAsset) null, key);
      if (nameFromOnomastics != str)
        BaseAssetLibrary.logAssetError($"<e>Onomastics</e>: Template test failed: {nameFromOnomastics} != {str}", key);
    }
    base.editorDiagnostic();
  }

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (OnomasticsAsset pAsset in this.list)
    {
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
      this.checkLocale((Asset) pAsset, pAsset.getDescriptionID());
    }
  }
}
