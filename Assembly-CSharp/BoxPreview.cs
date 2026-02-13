// Decompiled with JetBrains decompiler
// Type: BoxPreview
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class BoxPreview : MonoBehaviour
{
  [SerializeField]
  private Sprite _preview_default;
  [SerializeField]
  private Image _icon_gift;
  [SerializeField]
  private Image _icon_premium;
  [SerializeField]
  private Image _icon_broken;
  [SerializeField]
  private Image _icon_modded;
  [SerializeField]
  private Image _cursed_bg;
  [SerializeField]
  private Image _cursed_overlay;
  [SerializeField]
  private GameObject _favorited;
  [SerializeField]
  private Image _preview_image;
  [SerializeField]
  private Button _button;
  [SerializeField]
  private Text _text_id;
  private bool _wantLoad_preview;
  private float _timer_preview;
  private string _world_path;
  private int _slot_id;
  private MapMetaData _metaData;

  private void Awake()
  {
    // ISSUE: method pointer
    this._button.OnHover(new UnityAction((object) this, __methodptr(\u003CAwake\u003Eb__16_0)));
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: method pointer
    this._button.OnHoverOut(BoxPreview.\u003C\u003Ec.\u003C\u003E9__16_1 ?? (BoxPreview.\u003C\u003Ec.\u003C\u003E9__16_1 = new UnityAction((object) BoxPreview.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CAwake\u003Eb__16_1))));
  }

  public void setSlot(int pID)
  {
    this._metaData = (MapMetaData) null;
    this._text_id.text = "#" + pID.ToString();
    this._slot_id = pID;
    this._world_path = SaveManager.getSlotSavePath(pID);
    if (SaveManager.doesSaveExist(this._world_path))
      this._metaData = SaveManager.getMetaFor(this._world_path);
    this._preview_image.sprite = this._preview_default;
    ((Component) this._icon_gift).gameObject.SetActive(false);
    ((Component) this._icon_premium).gameObject.SetActive(false);
    ((Component) this._icon_broken).gameObject.SetActive(false);
    ((Component) this._icon_modded).gameObject.SetActive(false);
    ((Behaviour) this._cursed_bg).enabled = false;
    ((Behaviour) this._cursed_overlay).enabled = false;
    if (this._metaData != null)
    {
      if (this._metaData.saveVersion > Config.WORLD_SAVE_VERSION)
        ((Component) this._icon_broken).gameObject.SetActive(true);
      if (this._metaData.modded)
        ((Component) this._icon_modded).gameObject.SetActive(true);
      if (this._metaData.cursed)
      {
        ((Behaviour) this._cursed_bg).enabled = true;
        ((Behaviour) this._cursed_overlay).enabled = true;
      }
    }
    this._wantLoad_preview = true;
    this._timer_preview = 0.02f * (float) pID;
    ((Object) ((Component) this).gameObject).name = "BoxPreview " + pID.ToString();
    this._favorited.SetActive(PlayerConfig.instance.data.favorite_world == pID);
  }

  private void showHoverTooltip()
  {
    if (this._metaData == null || !Config.tooltips_active)
      return;
    this._metaData.temp_date_string = SaveManager.getMapCreationTime(this._world_path);
    Tooltip.show((object) this._button, "map_meta", new TooltipData()
    {
      map_meta = this._metaData
    });
  }

  private void Update()
  {
    if (!this._wantLoad_preview)
      return;
    if ((double) this._timer_preview > 0.0)
    {
      this._timer_preview -= Time.deltaTime;
    }
    else
    {
      this._wantLoad_preview = false;
      this.StartCoroutine(this.loadSaveSlotImage());
    }
  }

  public void showDefaultImage() => this._preview_image.sprite = this._preview_default;

  private void showPreview(Texture2D pTexture)
  {
    this._preview_image.sprite = Sprite.Create(Toolbox.ScaleTexture(pTexture, 100, 100), new Rect(0.0f, 0.0f, 100f, 100f), new Vector2(0.5f, 0.5f));
  }

  private IEnumerator loadSaveSlotImage()
  {
    BoxPreview boxPreview = this;
    string tPath = SaveManager.generatePngPreviewPath(boxPreview._world_path);
    if (string.IsNullOrEmpty(tPath) || !File.Exists(tPath))
    {
      boxPreview.showDefaultImage();
    }
    else
    {
      yield return (object) CoroutineHelper.wait_for_next_frame;
      Texture2D pTexture = new Texture2D(100, 100);
      ((Object) pTexture).name = "preview_" + boxPreview._slot_id.ToString();
      try
      {
        byte[] numArray = File.ReadAllBytes(tPath);
        if (ImageConversion.LoadImage(pTexture, numArray))
        {
          if (Object.op_Equality((Object) pTexture, (Object) null))
          {
            Debug.LogError((object) $"{((Object) ((Component) boxPreview).gameObject).name} texture is null from {tPath}");
            boxPreview.showDefaultImage();
          }
          else
            boxPreview.showPreview(pTexture);
        }
        else
        {
          Debug.LogError((object) $"{((Object) ((Component) boxPreview).gameObject).name} cannot load image from {tPath}");
          boxPreview.showDefaultImage();
        }
      }
      catch (Exception ex)
      {
        Debug.LogError((object) $"{((Object) ((Component) boxPreview).gameObject).name} {ex.Message} when trying to load {tPath}");
        boxPreview.showDefaultImage();
      }
      Object.Destroy((Object) pTexture);
    }
  }

  public void click()
  {
    if (ScrollWindow.isAnimationActive())
      return;
    if (Input.GetKey((KeyCode) 304))
    {
      Application.OpenURL("file://" + this._world_path);
    }
    else
    {
      SaveManager.setCurrentPathAndId(this._world_path, this._slot_id);
      if (SaveManager.currentSlotExists())
        ScrollWindow.showWindow("save_slot");
      else
        ScrollWindow.showWindow("save_slot_new");
    }
  }
}
