// Decompiled with JetBrains decompiler
// Type: AutoSaveElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Humanizer;
using System;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class AutoSaveElement : MonoBehaviour, IPointerMoveHandler, IEventSystemHandler
{
  [SerializeField]
  private Image _preview;
  [SerializeField]
  private Text _save_name;
  [SerializeField]
  private Text _save_time_ago;
  [SerializeField]
  private CountUpOnClick _kingdoms;
  [SerializeField]
  private CountUpOnClick _cities;
  [SerializeField]
  private CountUpOnClick _population;
  [SerializeField]
  private CountUpOnClick _mobs;
  [SerializeField]
  private CountUpOnClick _age;
  [SerializeField]
  private Button _button;
  [SerializeField]
  private GameObject _premium_icon;
  private string _world_path;
  private MapMetaData _meta_data;

  private void Awake()
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: method pointer
    this._button.OnHoverOut(AutoSaveElement.\u003C\u003Ec.\u003C\u003E9__12_0 ?? (AutoSaveElement.\u003C\u003Ec.\u003C\u003E9__12_0 = new UnityAction((object) AutoSaveElement.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CAwake\u003Eb__12_0))));
  }

  public void OnPointerMove(PointerEventData pData)
  {
    if (!InputHelpers.mouseSupported || Tooltip.anyActive())
      return;
    this.tooltipAction();
  }

  private void tooltipAction()
  {
    if (this._meta_data == null || !Config.tooltips_active)
      return;
    this._meta_data.temp_date_string = SaveManager.getMapCreationTime(this._world_path);
    Tooltip.show((object) this._button, "map_meta", new TooltipData()
    {
      map_meta = this._meta_data
    });
  }

  public void load(AutoSaveData pData)
  {
    this._world_path = pData.path;
    string smallPreviewPath = SaveManager.generatePngSmallPreviewPath(pData.path);
    if (!string.IsNullOrEmpty(smallPreviewPath) && File.Exists(smallPreviewPath))
    {
      byte[] numArray = File.ReadAllBytes(smallPreviewPath);
      Texture2D texture2D = new Texture2D(32 /*0x20*/, 32 /*0x20*/);
      ((Texture) texture2D).anisoLevel = 0;
      ((Texture) texture2D).filterMode = (FilterMode) 0;
      if (ImageConversion.LoadImage(texture2D, numArray))
        this._preview.sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, 32f, 32f), new Vector2(0.5f, 0.5f));
    }
    this._meta_data = SaveManager.getMetaFor(pData.path);
    this._save_name.text = this._meta_data.mapStats.name;
    ((Graphic) this._save_name).color = this._meta_data.mapStats.getArchitectMood().getColorText();
    this._kingdoms.setValue(this._meta_data.kingdoms);
    this._cities.setValue(this._meta_data.cities);
    this._population.setValue(this._meta_data.population);
    this._mobs.setValue(this._meta_data.mobs);
    this._age.setValue(Date.getYear(this._meta_data.mapStats.world_time));
    string str1 = "";
    string str2 = "";
    try
    {
      DateTime dateTime1 = Epoch.toDateTime(pData.timestamp);
      CultureInfo culture = LocalizedTextManager.getCulture();
      DateTime dateTime2 = DateTime.UtcNow.AddDays(7.0);
      if (dateTime1.Year < 2017)
        str1 = "GREG";
      else if (dateTime1 > dateTime2)
        str1 = "DREDD";
      else if (LocalizedTextManager.cultureSupported())
      {
        DateTime dateTime3 = dateTime1;
        CultureInfo cultureInfo1 = culture;
        DateTime? nullable = new DateTime?();
        CultureInfo cultureInfo2 = cultureInfo1;
        str1 = DateHumanizeExtensions.Humanize(dateTime3, true, nullable, cultureInfo2);
      }
      else
      {
        string shortDatePattern = culture.DateTimeFormat.ShortDatePattern;
        str1 = dateTime1.ToString(shortDatePattern, (IFormatProvider) culture);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ("failed with " + str2));
      Debug.LogError((object) ex);
    }
    this._save_time_ago.text = str1;
    ((Object) ((Component) this).gameObject).name = "AutoSaveElement_" + pData.timestamp.ToString();
  }

  public void clickLoadAutoSave()
  {
    SaveManager.setCurrentPath(this._world_path);
    ScrollWindow.showWindow("load_world");
  }

  private void OnDisable()
  {
    this._meta_data = (MapMetaData) null;
    if (!Object.op_Inequality((Object) this._preview, (Object) null))
      return;
    this._preview.sprite = (Sprite) null;
  }
}
