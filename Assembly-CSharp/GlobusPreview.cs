// Decompiled with JetBrains decompiler
// Type: GlobusPreview
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

#nullable disable
public class GlobusPreview : MonoBehaviour
{
  public bool use_current_world_info;
  public Image main_image_1;
  public Image main_image_2;
  public GameObject images_parent;
  public Image clouds;
  public Sprite preview_default;
  private float _tweenSpeed = 18f;
  private float _gap_size = 25f;
  private float _box_size = 100f;

  private void OnEnable()
  {
    if (!Config.game_loaded)
      return;
    if (this.use_current_world_info)
      this.setCurrentWorldSprite();
    else if (SaveManager.currentWorkshopMapData != null)
      this.setWorkshopSlotSprite();
    else
      this.startLoadCurrentSaveSlotSprite();
    this.startTweenGlobus();
  }

  private void startLoadCurrentSaveSlotSprite() => this.StartCoroutine(this.loadSaveSlotImage());

  private void setCurrentWorldSprite() => this.setSprites(PreviewHelper.getCurrentWorldPreview());

  private void setWorkshopSlotSprite() => this.setSprites(PreviewHelper.loadWorkshopMapPreview());

  private void setSprites(Sprite pSprite)
  {
    this.makeGradient(pSprite);
    this.main_image_1.sprite = pSprite;
    this.main_image_2.sprite = pSprite;
  }

  private void showDefaultImage()
  {
    this.main_image_1.sprite = this.preview_default;
    this.main_image_2.sprite = this.preview_default;
  }

  private IEnumerator loadSaveSlotImage()
  {
    string path = SaveManager.getCurrentPreviewPath();
    if (string.IsNullOrEmpty(path) || !File.Exists(path))
    {
      this.showDefaultImage();
    }
    else
    {
      using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture("file://" + path))
      {
        yield return (object) webRequest.SendWebRequest();
        if (webRequest.result == 3 || webRequest.result == 2)
        {
          this.showDefaultImage();
        }
        else
        {
          Texture2D content = DownloadHandlerTexture.GetContent(webRequest);
          ((Object) content).name = "save_slot_preview_" + Path.GetFileNameWithoutExtension(path);
          this.setSprites(Sprite.Create(content, new Rect(0.0f, 0.0f, (float) ((Texture) content).width, (float) ((Texture) content).height), new Vector2(0.5f, 0.5f)));
        }
      }
    }
  }

  private void makeGradient(Sprite pSprite)
  {
    float num1 = (float) ((Texture) pSprite.texture).width * 0.1f;
    Texture2D texture = pSprite.texture;
    ((Object) texture).name = "gradient_" + ((Object) texture).name;
    for (int index1 = 0; (double) index1 < (double) num1; ++index1)
    {
      for (int index2 = 0; index2 < ((Texture) texture).height; ++index2)
      {
        int num2 = index1;
        Color pixel1 = texture.GetPixel(num2, index2);
        pixel1.a = (float) num2 / num1;
        texture.SetPixel(num2, index2, pixel1);
        int num3 = ((Texture) pSprite.texture).width - index1;
        Color pixel2 = texture.GetPixel(num3, index2);
        pixel2.a = (float) index1 / num1;
        texture.SetPixel(num3, index2, pixel2);
      }
    }
    texture.Apply();
  }

  private void startTweenGlobus()
  {
    float num1 = this._box_size + this._gap_size;
    float num2 = num1 / this._tweenSpeed;
    ShortcutExtensions.DOKill((Component) this.images_parent.transform, false);
    this.images_parent.transform.localPosition = new Vector3(this._gap_size, 0.0f, 0.0f);
    ((Tween) TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOLocalMove(this.images_parent.transform, new Vector3(-num1, 0.0f, 0.0f), num2, false), (Ease) 1)).onComplete = new TweenCallback(this.tweenLoop);
  }

  private void tweenLoop()
  {
    float num1 = this._box_size + this._gap_size;
    float num2 = num1 / this._tweenSpeed;
    this.images_parent.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    ((Tween) TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOLocalMove(this.images_parent.transform, new Vector3(-num1, 0.0f, 0.0f), num2, false), (Ease) 1)).onComplete = new TweenCallback(this.tweenLoop);
  }
}
