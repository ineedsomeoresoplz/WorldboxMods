// Decompiled with JetBrains decompiler
// Type: Tutorial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Tutorial : MonoBehaviour
{
  public Sprite icon_default;
  public Sprite icon_bear;
  public Sprite icon_ivilizations;
  public Sprite icon_nuke;
  public Sprite icon_dragon;
  public Sprite icon_tornado;
  public Sprite icon_saveBox;
  public Sprite icon_customWorld;
  public Sprite icon_worldLaws;
  public Sprite icon_greyGoo;
  public Sprite icon_ufo;
  public Sprite icon_heart;
  public Sprite icon_finger;
  public GameObject bear;
  public GameObject centerObject;
  public GameObject brushSize;
  public GameObject adButton;
  public GameObject saveButton;
  public GameObject customMapButton;
  public GameObject worldRules;
  public GameObject tabDrawing;
  public GameObject tabCivs;
  public GameObject tabCreatures;
  public GameObject tabNature;
  public GameObject tabBombs;
  public GameObject tabOther;
  public GameObject settingsButton;
  public Text text;
  public Image attentionBox;
  public Text pressAnywhere;
  private int curPage;
  private List<TutorialPage> pages;
  private float waitTimer;
  private Color color_red;
  private Color color_white;
  private Color color_yellow;
  private Color color_yellow_transparent;
  private Tweener textTypeTween;

  private void create()
  {
    this.pages = new List<TutorialPage>();
    this.color_red = Toolbox.makeColor("#FF3700");
    this.color_red.a = 0.4f;
    this.color_white = Toolbox.makeColor("#4AA5FF");
    this.color_white.a = 0.4f;
    this.color_yellow = Toolbox.makeColor("#FEFE00");
    this.color_yellow_transparent = Toolbox.makeColor("#FEFE00");
    this.color_yellow_transparent.a = 0.0f;
    this.add(new TutorialPage()
    {
      text = "tut_page1",
      wait = 1f
    });
    this.add(new TutorialPage()
    {
      text = "tut_page2",
      wait = 0.3f
    });
    this.add(new TutorialPage()
    {
      text = "tut_page3_mobile",
      mobileOnly = true,
      centerImage = this.icon_finger,
      wait = 1f
    });
    this.add(new TutorialPage()
    {
      text = "tut_page3_pc",
      pcOnly = true,
      wait = 1f
    });
    this.add(new TutorialPage() { text = "tut_page4" });
    this.add(new TutorialPage()
    {
      text = "tut_page5",
      object1 = this.saveButton,
      centerImage = this.icon_saveBox,
      wait = 1.5f
    });
    this.add(new TutorialPage()
    {
      text = "tut_page6",
      object1 = this.customMapButton,
      centerImage = this.icon_customWorld,
      wait = 1.5f
    });
    this.add(new TutorialPage()
    {
      text = "tut_page7",
      object1 = this.worldRules,
      centerImage = this.icon_worldLaws
    });
    this.add(new TutorialPage()
    {
      text = "tut_page8",
      object1 = this.tabDrawing
    });
    this.add(new TutorialPage()
    {
      text = "tut_page9",
      icon = "brush"
    });
    this.add(new TutorialPage()
    {
      text = "tut_page10",
      object1 = this.tabCivs,
      centerImage = this.icon_ivilizations
    });
    this.add(new TutorialPage()
    {
      text = "tut_page11",
      object1 = this.tabCreatures,
      centerImage = this.icon_dragon
    });
    this.add(new TutorialPage()
    {
      text = "tut_page12",
      object1 = this.tabNature,
      centerImage = this.icon_tornado
    });
    this.add(new TutorialPage()
    {
      text = "tut_page13",
      object1 = this.tabBombs,
      centerImage = this.icon_nuke
    });
    this.add(new TutorialPage()
    {
      text = "tut_page14",
      object1 = this.tabOther,
      centerImage = this.icon_greyGoo
    });
    this.add(new TutorialPage()
    {
      text = "tut_page15",
      centerImage = this.icon_ufo
    });
    this.add(new TutorialPage()
    {
      text = "tut_page16",
      mobileOnly = true,
      icon = "reward",
      wait = 1.5f
    });
    this.add(new TutorialPage()
    {
      text = "tut_page17",
      centerImage = this.icon_heart,
      wait = 0.5f
    });
  }

  public void startTutorial()
  {
    if (this.pages == null)
      this.create();
    ((Component) this).gameObject.SetActive(true);
    this.curPage = -1;
    PowerButtonSelector.instance.unselectAll();
    PowerButtonSelector.instance.unselectTabs();
    ((Component) this.attentionBox).gameObject.SetActive(false);
    this.nextPage();
  }

  private void nextPage()
  {
    ++this.curPage;
    if (this.curPage >= this.pages.Count)
    {
      this.endTutorial();
    }
    else
    {
      ((Component) this.pressAnywhere).GetComponent<LocalizedText>().updateText();
      TutorialPage page = this.pages[this.curPage];
      if (!Config.isMobile && page.mobileOnly)
        this.nextPage();
      else if (Config.isMobile && page.pcOnly)
      {
        this.nextPage();
      }
      else
      {
        if (Object.op_Equality((Object) page.object1, (Object) null))
        {
          ((Component) this.attentionBox).gameObject.SetActive(false);
        }
        else
        {
          ((Component) this.attentionBox).gameObject.SetActive(true);
          Vector3 position = page.object1.transform.position;
          Vector2 sizeDelta = page.object1.GetComponent<RectTransform>().sizeDelta;
          sizeDelta.x += 10f;
          sizeDelta.y += 10f;
          ((Component) this.attentionBox).transform.position = position;
          ((Graphic) this.attentionBox).rectTransform.sizeDelta = sizeDelta;
          ShortcutExtensions.DOKill((Component) this.attentionBox, false);
          ((Component) this.attentionBox).transform.localScale = new Vector3(0.5f, 0.5f);
          TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(((Component) this.attentionBox).transform, new Vector3(1f, 1f, 1f), 0.3f), (Ease) 27);
          ((Graphic) this.attentionBox).color = this.color_white;
        }
        if (Object.op_Equality((Object) page.centerImage, (Object) null))
          page.centerImage = this.icon_default;
        this.centerObject.GetComponent<Image>().sprite = page.centerImage;
        this.centerObject.gameObject.SetActive(false);
        this.adButton.gameObject.SetActive(false);
        this.brushSize.gameObject.SetActive(false);
        ((Component) this.text).transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        ShortcutExtensions.DOKill((Component) this.text, false);
        this.text.text = "";
        string text1 = LocalizedTextManager.getText(page.text);
        float num = (float) (text1.Length / 25);
        if ((double) num <= 1.0)
          num = 1f;
        this.text.text = text1;
        ((Component) this.text).GetComponent<LocalizedText>().checkTextFont();
        ((Component) this.text).GetComponent<LocalizedText>().checkSpecialLanguages();
        string text2 = this.text.text;
        this.text.text = "";
        this.textTypeTween = (Tweener) DOTweenModuleUI.DOText(this.text, text2, num, false, (ScrambleMode) 0, (string) null);
        TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(((Component) this.text).transform, new Vector3(1f, 1f, 1f), 0.3f), (Ease) 26);
        this.waitTimer = page.wait;
        if (this.canSkipTutorial())
          this.waitTimer = 0.0f;
        if ((double) this.waitTimer > 0.0)
          ((Component) this.pressAnywhere).gameObject.SetActive(false);
        ShortcutExtensions.DOKill((Component) this.bear.transform, false);
        this.bear.transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        ShortcutExtensions.DOShakeRotation(this.bear.transform, num, 90f, 10, 90f, true, (ShakeRandomnessMode) 0);
        if (page.icon == "default")
        {
          this.centerObject.SetActive(true);
          ShortcutExtensions.DOKill((Component) this.centerObject.transform, false);
          this.centerObject.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
          TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(this.centerObject.transform, new Vector3(1f, 1f, 1f), 0.5f), (Ease) 26);
        }
        else if (page.icon == "reward")
        {
          this.adButton.SetActive(true);
          ShortcutExtensions.DOKill((Component) this.adButton.transform, false);
          this.adButton.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
          TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(this.adButton.transform, new Vector3(1f, 1f, 1f), 0.5f), (Ease) 26);
        }
        else
        {
          if (!(page.icon == "brush"))
            return;
          this.brushSize.SetActive(true);
          ShortcutExtensions.DOKill((Component) this.brushSize.transform, false);
          this.brushSize.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
          TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(this.brushSize.transform, new Vector3(1f, 1f, 1f), 0.5f), (Ease) 26);
        }
      }
    }
  }

  internal void completeText()
  {
  }

  private bool canSkipTutorial() => true;

  public static void restartTutorial()
  {
    PlayerConfig.instance.data.tutorialFinished = false;
    PlayerConfig.saveData();
  }

  internal bool isActive() => ((Component) this).gameObject.activeSelf;

  internal void endTutorial()
  {
    ((Component) this).gameObject.SetActive(false);
    PlayerConfig.instance.data.tutorialFinished = true;
    PlayerConfig.saveData();
    ScrollWindow.clearQueue();
  }

  private void LateUpdate()
  {
    if (((Component) this.attentionBox).gameObject.activeSelf)
    {
      if (Color.op_Equality(((Graphic) this.attentionBox).color, this.color_red))
        DOTweenModuleUI.DOColor(this.attentionBox, this.color_white, 1f);
      else if (Color.op_Equality(((Graphic) this.attentionBox).color, this.color_white))
        DOTweenModuleUI.DOColor(this.attentionBox, this.color_red, 1f);
    }
    if (this.canSkipTutorial())
    {
      if (TweenExtensions.IsActive((Tween) this.textTypeTween) && Input.GetMouseButtonUp(0))
      {
        TweenExtensions.Kill((Tween) this.textTypeTween, true);
        return;
      }
    }
    else if (TweenExtensions.IsActive((Tween) this.textTypeTween))
      return;
    if ((double) this.waitTimer > 0.0)
    {
      this.waitTimer -= Time.deltaTime;
    }
    else
    {
      if (!((Component) this.pressAnywhere).gameObject.activeSelf)
      {
        ((Component) this.pressAnywhere).gameObject.SetActive(true);
        ((Component) this.pressAnywhere).transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(((Component) this.pressAnywhere).transform, new Vector3(1f, 1f, 1f), 1f), (Ease) 27);
        ((Graphic) this.pressAnywhere).color = this.color_yellow_transparent;
        DOTweenModuleUI.DOColor(this.pressAnywhere, this.color_yellow, 1f);
      }
      if (!Input.GetMouseButtonUp(0))
        return;
      this.nextPage();
    }
  }

  private void add(TutorialPage pPage) => this.pages.Add(pPage);
}
