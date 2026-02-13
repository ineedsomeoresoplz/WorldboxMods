// Decompiled with JetBrains decompiler
// Type: LevelPreviewButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

#nullable disable
public class LevelPreviewButton : MonoBehaviour
{
  public bool premiumOnly = true;
  public bool worldNetUpload;
  public Image premiumIcon;
  public Image rewardAdIcon;
  public Button button;
  public SlotButtonCallback slotData;
  public Sprite defaultSprite;
  private ButtonAnimation buttonAnimation;
  public bool loaded;
  public bool loading;
  public bool autoload;

  public void click()
  {
    if (ScrollWindow.isAnimationActive())
      return;
    if (Object.op_Equality((Object) this.buttonAnimation, (Object) null))
      this.buttonAnimation = ((Component) ((Component) this).transform.parent.parent.parent).GetComponent<ButtonAnimation>();
    this.buttonAnimation.clickAnimation();
    SaveManager.setCurrentSlot(this.slotData.slotID);
    if (this.worldNetUpload)
    {
      if (!SaveManager.currentSlotExists() || !SaveManager.currentPreviewExists() || !SaveManager.currentMetaExists())
        return;
      ScrollWindow.showWindow("worldnet_upload_world_name");
    }
    else if (SaveManager.currentSlotExists())
      ScrollWindow.showWindow("save_slot");
    else
      ScrollWindow.showWindow("save_slot_new");
  }

  public void checkTextureDestroy()
  {
    if (!Object.op_Inequality((Object) ((Selectable) this.button).image.sprite.texture, (Object) this.defaultSprite.texture))
      return;
    Object.Destroy((Object) ((Selectable) this.button).image.sprite.texture);
  }

  private void OnEnable()
  {
    if (!this.autoload)
      return;
    this.reloadImage();
  }

  private void OnDisable()
  {
    if (Object.op_Equality((Object) ((Selectable) this.button)?.image?.sprite?.texture, (Object) this.defaultSprite.texture))
      return;
    Object.Destroy((Object) ((Selectable) this.button)?.image?.sprite?.texture);
    Object.Destroy((Object) ((Selectable) this.button)?.image?.sprite);
  }

  public void reloadImage()
  {
    if (Object.op_Equality((Object) this, (Object) null) || !((Behaviour) this).isActiveAndEnabled || this.loaded && Object.op_Inequality((Object) ((Selectable) this.button)?.image?.sprite, (Object) null) || this.loading)
      return;
    this.loading = true;
    if (SaveManager.currentWorkshopMapData != null)
    {
      this.loadWorkshopMapPreview();
    }
    else
    {
      bool flag = SaveManager.currentSlotExists();
      if (this.slotData.slotID == -1 && !flag)
        this.loadImage(PreviewHelper.getCurrentWorldPreview());
      else
        this.StartCoroutine(this.loadSaveSlotImage(this.slotData.slotID));
    }
  }

  private void loadWorkshopMapPreview() => this.loadImage(PreviewHelper.loadWorkshopMapPreview());

  private IEnumerator loadSaveSlotImage(int slotID)
  {
    LevelPreviewButton levelPreviewButton = this;
    string path = SaveManager.getPngSlotPath(slotID);
    if (string.IsNullOrEmpty(path) || !File.Exists(path))
    {
      levelPreviewButton.loadImage((Sprite) null);
    }
    else
    {
      using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture("file://" + path))
      {
        yield return (object) webRequest.SendWebRequest();
        if (webRequest.result == 3 || webRequest.result == 2)
        {
          Debug.LogError((object) $"{((Object) ((Component) levelPreviewButton).gameObject).name} {webRequest.error} {path}");
          levelPreviewButton.loadImage((Sprite) null);
        }
        else
        {
          Texture2D content = DownloadHandlerTexture.GetContent(webRequest);
          Sprite pSource = Sprite.Create(content, new Rect(0.0f, 0.0f, (float) ((Texture) content).width, (float) ((Texture) content).height), new Vector2(0.5f, 0.5f));
          levelPreviewButton.loadImage(pSource);
        }
      }
    }
  }

  public void loadImage(Sprite pSource)
  {
    if (Object.op_Equality((Object) this, (Object) null) || !((Behaviour) this).isActiveAndEnabled)
    {
      this.loaded = false;
      this.loading = false;
    }
    else
    {
      if (!this.premiumOnly || Config.hasPremium)
        ((Component) this.premiumIcon).gameObject.SetActive(false);
      bool flag = false;
      if (Object.op_Inequality((Object) pSource, (Object) null))
      {
        flag = true;
        ((Texture) pSource.texture).anisoLevel = 0;
        ((Texture) pSource.texture).filterMode = (FilterMode) 0;
      }
      else
        pSource = this.defaultSprite;
      ((Selectable) this.button).image.sprite = pSource;
      RectTransform component1 = ((Component) this).gameObject.GetComponent<RectTransform>();
      Rect rect1 = pSource.rect;
      double width1 = (double) ((Rect) ref rect1).width;
      Rect rect2 = pSource.rect;
      double height1 = (double) ((Rect) ref rect2).height;
      Vector2 vector2 = new Vector2((float) width1, (float) height1);
      component1.sizeDelta = vector2;
      RectTransform component2 = ((Component) ((Component) this.button).transform.parent.parent).GetComponent<RectTransform>();
      double x = (double) component2.sizeDelta.x;
      rect2 = pSource.rect;
      double width2 = (double) ((Rect) ref rect2).width;
      float num1 = (float) (x / width2);
      double y = (double) component2.sizeDelta.y;
      rect2 = pSource.rect;
      double height2 = (double) ((Rect) ref rect2).height;
      float num2 = (float) (y / height2);
      float num3 = (double) num1 > (double) num2 ? num1 : num2;
      Transform parent = ((Component) this).transform.parent;
      if (!flag)
        num3 = 1f;
      Vector3 vector3 = new Vector3(num3, num3, 1f);
      parent.localScale = vector3;
      this.loaded = true;
      this.loading = false;
    }
  }
}
