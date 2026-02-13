// Decompiled with JetBrains decompiler
// Type: PatchLogLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PatchLogLoader : MonoBehaviour
{
  private const int FIRST_UNFOLDED_ELEMS_AMOUNT = 5;
  [SerializeField]
  private GameObject _prefab_text;
  [SerializeField]
  private GameObject _prefab_entry_bg;
  [SerializeField]
  private PatchLogElement _prefab_element;
  private readonly List<GameObject> _visual_elements = new List<GameObject>();
  private readonly Dictionary<string, PatchLogEntry> _entries_dict = new Dictionary<string, PatchLogEntry>();
  private readonly List<PatchLogEntry> _entries_list = new List<PatchLogEntry>();

  public void OnEnable()
  {
    this.loadEntries();
    this.StartCoroutine(this.createElements());
  }

  private void loadEntries()
  {
    if (this._entries_list.Count > 0)
      return;
    TextAsset[] textAssetArray = Resources.LoadAll<TextAsset>("texts/patch_notes");
    if (textAssetArray.Length == 0)
      return;
    foreach (TextAsset textAsset in textAssetArray)
    {
      string[] strArray1 = textAsset.text.Split(new string[1]
      {
        "##########"
      }, StringSplitOptions.None);
      string[] strArray2 = strArray1[0].Split(new string[3]
      {
        "\r\n",
        "\r",
        "\n"
      }, StringSplitOptions.None);
      string str1 = strArray2[0];
      string str2 = strArray2[1];
      string str3 = strArray2[2];
      string str4 = strArray2[3];
      string str5 = strArray1[1];
      if (str5.StartsWith("\r\n"))
        str5 = str5.Substring(2);
      else if (str5.StartsWith("\n"))
        str5 = str5.Substring(1);
      PatchLogEntry patchLogEntry = new PatchLogEntry()
      {
        version = str1,
        name = str2,
        date = str3,
        icon_path = str4,
        text = str5
      };
      this._entries_dict[patchLogEntry.version] = patchLogEntry;
      this._entries_list.Add(patchLogEntry);
    }
    Version result1;
    Version result2;
    this._entries_list.Sort((Comparison<PatchLogEntry>) ((pA, pB) => Version.TryParse(pA.version, out result1) && Version.TryParse(pB.version, out result2) ? result2.CompareTo(result1) : string.Compare(pB.version, pA.version, StringComparison.Ordinal)));
  }

  private IEnumerator createElements()
  {
    if (this._entries_dict.Count != 0)
    {
      for (int i = 0; i < this._entries_list.Count; ++i)
      {
        PatchLogElement patchLogElement = this.showEntry(this._entries_list[i]);
        if (!Object.op_Equality((Object) patchLogElement, (Object) null))
        {
          if (i < 5)
            patchLogElement.unfold();
          else
            patchLogElement.fold();
          yield return (object) new WaitForSeconds(0.05f);
        }
      }
    }
  }

  private PatchLogElement showEntry(PatchLogEntry pEntry)
  {
    if (!CursedSacrifice.isAllSacrificesDone() && !WorldLawLibrary.world_law_cursed_world.isEnabled() && pEntry.name == "VoidBox")
      return (PatchLogElement) null;
    using (ListPool<string> values = new ListPool<string>(10))
    {
      string[] strArray = pEntry.text.Split(new string[3]
      {
        "\r\n",
        "\r",
        "\n"
      }, StringSplitOptions.None);
      string str1;
      try
      {
        str1 = this.prettyDaysAgo(pEntry.date);
      }
      catch (Exception ex)
      {
        Console.WriteLine((object) ex);
        throw;
      }
      int num = this.isValidDate(pEntry.date) ? 1 : 0;
      string str2 = num != 0 ? pEntry.date : "???";
      string pColorHex1 = num != 0 ? "#23F3FF" : "#96adb3";
      string pColorHex2 = num != 0 ? "#FFFF51" : "#96adb3";
      string str3 = $"{pEntry.version.ColorHex(pColorHex1)} - {pEntry.name.ToUpper().ColorHex(pColorHex2)}";
      PatchLogElement patchLogElement = Object.Instantiate<PatchLogElement>(this._prefab_element, ((Component) this).gameObject.transform);
      ((Object) patchLogElement).name = "PatchLog " + pEntry.version;
      this._visual_elements.Add(((Component) patchLogElement).gameObject);
      PatchLogTitle title = patchLogElement.title;
      title.title.text = str3;
      patchLogElement.date.text = str2;
      patchLogElement.date_ago.text = str1;
      Sprite sprite = SpriteTextureLoader.getSprite(pEntry.icon_path);
      if (Object.op_Equality((Object) sprite, (Object) null))
        Debug.LogError((object) ("Failed to load icon in " + pEntry.version));
      title.icon_left.sprite = sprite;
      title.icon_right.sprite = sprite;
      for (int index1 = 0; index1 < strArray.Length; index1 += 10)
      {
        values.Clear();
        for (int index2 = index1; index2 < index1 + 10 && index2 < strArray.Length; ++index2)
          values.Add(strArray[index2]);
        ((Graphic) this.createTextField(this.colorElements(string.Join("\n", (IEnumerable<string>) values)), patchLogElement.texts.gameObject)).color = Toolbox.color_text_default_bright;
      }
      return patchLogElement;
    }
  }

  private string prettyDaysAgo(string pDateString)
  {
    return !this.isValidDate(pDateString) ? pDateString : (DateTime.UtcNow - DateTime.ParseExact(pDateString, "yyyy-MM-dd", (IFormatProvider) null)).Days.ToString() + " days ago";
  }

  private bool isValidDate(string pInput)
  {
    return DateTime.TryParseExact(pInput, "yyyy-MM-dd", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _);
  }

  private Text createTextField(string pText, GameObject pEntryBackground)
  {
    Text component = Object.Instantiate<GameObject>(this._prefab_text, pEntryBackground.transform).GetComponent<Text>();
    component.text = pText;
    this._visual_elements.Add(((Component) component).gameObject);
    return component;
  }

  private string colorElements(string pText)
  {
    pText = pText.Replace("added:", "<color=#4CCFFF>added:</color>");
    pText = pText.Replace("fixed:", "<color=#D95032>fixed:</color>");
    pText = pText.Replace("fixes:", "<color=#D95032>fixed:</color>");
    pText = pText.Replace("fxed:", "<color=#D95032>fixed:</color>");
    pText = pText.Replace("changes:", "<color=#F3961F>changed:</color>");
    pText = pText.Replace("changed:", "<color=#F3961F>changed:</color>");
    pText = pText.Replace("ongoing:", "<color=#4CCFFF>ongoing:</color>");
    pText = pText.Replace("modding:", "<color=#43FF43>modding:</color>");
    pText = pText.Replace("known issues:", "<color=#D95032>known issues:</color>");
    pText = pText.Replace("translations:", "<color=#d6abff>translation:</color>");
    return pText;
  }

  public void OnDisable()
  {
    foreach (GameObject visualElement in this._visual_elements)
      Object.Destroy((Object) visualElement.gameObject);
    this._visual_elements.Clear();
  }
}
