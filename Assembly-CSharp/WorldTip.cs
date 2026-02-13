// Decompiled with JetBrains decompiler
// Type: WorldTip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WorldTip : MonoBehaviour
{
  public Transform transform_toolbar;
  public Transform transform_main;
  public Transform transform_positionTop;
  public static WorldTip instance;
  public Canvas canvas;
  public Text text;
  public TipStatus status;
  public CanvasGroup canvasGroup;
  private float timeout;
  private float scale = 1f;
  public static Dictionary<string, string> replacementDict;

  private void Awake()
  {
    this.status = TipStatus.Hidden;
    this.canvasGroup.alpha = 0.0f;
    WorldTip.instance = this;
  }

  public void show(string pText, bool pTranslate = true, string pPosition = "center", float pTime = 3f, string pColor = "#F3961F")
  {
    if (Object.op_Equality((Object) WorldTip.instance, (Object) null))
      return;
    ((Graphic) WorldTip.instance.text).color = Toolbox.makeColor(pColor);
    if (pTranslate)
    {
      WorldTip.instance.text.text = LocalizedTextManager.getText(pText);
      if (WorldTip.replacementDict != null)
        WorldTip.instance.text.text = WorldTip.replaceWords(WorldTip.instance.text.text);
      ((Component) WorldTip.instance.text).GetComponent<LocalizedText>().checkSpecialLanguages();
    }
    else
      WorldTip.instance.text.text = pText;
    this.updateTextWidth();
    ((Component) this).transform.SetParent(this.transform_main);
    WorldTip.instance.startShow(pTime);
    switch (pPosition)
    {
      case "center":
        ((Component) this).transform.position = Vector3.zero;
        break;
      case "top":
        ((Component) this).transform.position = this.transform_positionTop.position;
        break;
      default:
        ((Component) WorldTip.instance).transform.position = Input.mousePosition;
        break;
    }
  }

  public static void showNowCenter(string pText) => WorldTip.showNow(pText);

  public static void showNowTop(string pText, bool pTranslate = true)
  {
    WorldTip.showNow(pText, pTranslate, "top");
  }

  public static void addWordReplacement(string key, string value)
  {
    if (WorldTip.replacementDict == null)
      WorldTip.replacementDict = new Dictionary<string, string>();
    WorldTip.replacementDict[key] = value;
  }

  public static string replaceWords(string text)
  {
    foreach (string key in WorldTip.replacementDict.Keys)
      text = text.Replace(key, WorldTip.replacementDict[key]);
    WorldTip.replacementDict = (Dictionary<string, string>) null;
    return text;
  }

  public static void showNow(
    string pText,
    bool pTranslate = true,
    string pPosition = "center",
    float pTime = 3f,
    string pColor = "#F3961F")
  {
    if (Object.op_Equality((Object) WorldTip.instance, (Object) null))
      return;
    WorldTip.instance.show(pText, pTranslate, pPosition, pTime, pColor);
  }

  public void showToolbarText(string pText)
  {
    this.text.text = pText;
    LocalizedText localizedText;
    if (((Component) this.text).TryGetComponent<LocalizedText>(ref localizedText))
      localizedText.checkSpecialLanguages();
    this.updateTextWidth();
    this.startShow();
    ((Component) this).transform.position = this.transform_toolbar.position;
  }

  public void showToolbarText(GodPower pPower, bool pShowOnComputer = true)
  {
    if (!pShowOnComputer && Config.isComputer)
      return;
    string pTextMain;
    string pTextDescription;
    if (pPower.type == PowerActionType.PowerSpawnActor)
    {
      ActorAsset actorAsset = pPower.getActorAsset();
      pTextMain = actorAsset.getLocalizedName();
      pTextDescription = actorAsset.getLocalizedDescription();
    }
    else
    {
      pTextMain = LocalizedTextManager.getText(pPower.getLocaleID());
      pTextDescription = LocalizedTextManager.getText(pPower.getDescriptionID());
    }
    this.showToolbarText(pTextMain, pTextDescription, pShowOnComputer);
  }

  public void showToolbarText(string pTextMain, string pTextDescription, bool pShowOnComputer = true)
  {
    if (!pShowOnComputer && Config.isComputer)
      return;
    this.text.text = $"{pTextMain}\n{pTextDescription}";
    LocalizedText localizedText;
    if (((Component) this.text).TryGetComponent<LocalizedText>(ref localizedText))
      localizedText.checkSpecialLanguages();
    this.updateTextWidth();
    this.startShow();
    ((Component) this).transform.position = this.transform_toolbar.position;
  }

  public void setText(string pText, bool pAddSKip = false)
  {
    this.text.text = LocalizedTextManager.getText(pText);
    ((Component) this.text).GetComponent<LocalizedText>().checkSpecialLanguages();
    if (pAddSKip)
      this.text.text = "\n" + this.text.text;
    this.updateTextWidth();
    ((Component) this).transform.position = this.transform_toolbar.position;
    this.startShow();
  }

  private void updateTextWidth()
  {
  }

  private void startShow(float pTime = 3f)
  {
    this.status = TipStatus.Shown;
    this.timeout = pTime;
    this.scale = 1.5f;
  }

  public static void hideNow()
  {
    if (Object.op_Equality((Object) WorldTip.instance, (Object) null) || !((Component) WorldTip.instance).gameObject.activeSelf)
      return;
    WorldTip.instance.startHide();
  }

  internal void startHide()
  {
    this.status = TipStatus.Hidden;
    this.timeout = 0.0f;
  }

  private void Update()
  {
    if ((double) this.scale > 1.0)
    {
      this.scale -= Time.deltaTime * 3f;
      if ((double) this.scale < 1.0)
        this.scale = 1f;
      ((Component) this).transform.localScale = new Vector3(this.scale, this.scale, 1f);
    }
    switch (this.status)
    {
      case TipStatus.Hidden:
        if ((double) this.canvasGroup.alpha <= 0.0)
          break;
        this.canvasGroup.alpha -= Time.deltaTime * 2f;
        break;
      case TipStatus.Shown:
        if ((double) this.canvasGroup.alpha < 1.0)
          this.canvasGroup.alpha += Time.deltaTime * 3f;
        if ((double) this.canvasGroup.alpha != 1.0)
          break;
        this.timeout -= Time.deltaTime;
        if ((double) this.timeout > 0.0)
          break;
        this.startHide();
        break;
    }
  }
}
