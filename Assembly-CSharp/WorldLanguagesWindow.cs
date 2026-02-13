// Decompiled with JetBrains decompiler
// Type: WorldLanguagesWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WorldLanguagesWindow : MonoBehaviour
{
  [SerializeField]
  private LocalizationButton _language_button;
  private ObjectPoolGenericMono<LocalizationButton> _pool;
  [SerializeField]
  private Transform _content;
  private static HashSet<LocalizationButton> _all_buttons = new HashSet<LocalizationButton>();
  private Dictionary<string, int> _percentage_data;

  private void Start()
  {
    this._percentage_data = JsonConvert.DeserializeObject<Dictionary<string, int>>(Resources.Load<TextAsset>("locales/percentages").text);
    this._pool = new ObjectPoolGenericMono<LocalizationButton>(this._language_button, this._content);
    foreach (GameLanguageAsset pAsset in AssetManager.game_language_library.list)
    {
      if (pAsset != null)
      {
        LocalizationButton next = this._pool.getNext();
        WorldLanguagesWindow._all_buttons.Add(next);
        int pDone;
        this._percentage_data.TryGetValue(pAsset.id, out pDone);
        next.SetAsset(pAsset, pDone);
      }
    }
    this.checkDebug();
  }

  private void OnEnable() => this.checkDebug();

  private void checkDebug()
  {
    bool flag = DebugConfig.isOn(DebugOption.DebugButton);
    foreach (LocalizationButton allButton in WorldLanguagesWindow._all_buttons)
    {
      if (allButton.getAsset().debug_only)
        ((Component) allButton).gameObject.SetActive(flag);
    }
  }

  public static void updateButtons()
  {
    foreach (LocalizationButton allButton in WorldLanguagesWindow._all_buttons)
      allButton.checkSprite();
  }
}
