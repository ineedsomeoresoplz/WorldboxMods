// Decompiled with JetBrains decompiler
// Type: LoadingScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#nullable disable
public class LoadingScreen : MonoBehaviour
{
  public Image background;
  public CanvasGroup canvasGroup;
  public Text percents;
  public LocalizedText topText;
  public LocalizedText tipText;
  public Image bar;
  public Image mask;
  private AsyncOperation asyncLoad;
  private bool appearDone;
  public bool inGameScreen;
  internal bool modeIn;
  public LoadingScreen.TransitionAction action;
  private float outTimer;
  public Canvas canvas;
  public Text loadingHelperText;
  private static int _last_tip = -1;
  private static int _max_tip = 0;
  private float lastBgWidth;
  private float lastBgHeight;
  private float lastCScale;
  public bool debugg;

  private void setupBg()
  {
    float width = (float) Screen.width;
    float height = (float) Screen.height;
    if ((double) this.lastBgHeight == (double) height && (double) this.lastBgWidth == (double) width && (double) this.canvas.scaleFactor == (double) this.lastCScale)
      return;
    this.lastBgWidth = width;
    this.lastBgHeight = height;
    this.lastCScale = this.canvas.scaleFactor;
    float num1 = (float) ((Graphic) this.background).mainTexture.width * this.canvas.scaleFactor;
    float num2 = (float) ((Graphic) this.background).mainTexture.height * this.canvas.scaleFactor;
    float num3 = (float) Screen.width / num1;
    float num4 = (float) Screen.height / num2;
    if ((double) num3 > (double) num4)
      ((Component) this.background).transform.localScale = new Vector3(num3, num3, 1f);
    else
      ((Component) this.background).transform.localScale = new Vector3(num4, num4, 1f);
  }

  private void Awake()
  {
    InitLibraries.initMainLibs();
    Config.enableAutoRotation(false);
    ((Component) this).transform.localPosition = new Vector3();
    if (this.inGameScreen)
    {
      this.outTimer = 0.3f;
      this.canvasGroup.alpha = 1f;
      this.appearDone = true;
      ((Component) this.bar).transform.localScale = new Vector3(1f, 1f, 1f);
    }
    else
    {
      this.canvasGroup.alpha = 0.0f;
      ((Component) this.bar).transform.localScale = new Vector3(0.0f, 1f, 1f);
    }
  }

  private void startAction()
  {
    ScrollWindow.hideAllEvent(false);
    this.modeIn = false;
    if (Config.isMobile && !Config.hasPremium)
    {
      Debug.Log((object) ("PremiumElementsChecker.goodForInterstitialAd(): " + PremiumElementsChecker.goodForInterstitialAd().ToString()));
      if (PremiumElementsChecker.goodForInterstitialAd())
      {
        if (PlayInterstitialAd.instance.isReady())
        {
          PlayInterstitialAd.instance.showAd();
          PremiumElementsChecker.setInterstitialAdTimer();
        }
        else
          PlayInterstitialAd.instance.initAds();
      }
    }
    this.action();
  }

  internal void startTransition(LoadingScreen.TransitionAction pAction)
  {
    Config.enableAutoRotation(false);
    this.action = pAction;
    ((Component) this.bar).gameObject.SetActive(false);
    ((Component) this.percents).gameObject.SetActive(false);
    ((Component) this.topText).gameObject.SetActive(false);
    ((Component) this.tipText).gameObject.SetActive(false);
    ((Component) this.mask).gameObject.SetActive(false);
    ((Component) this).gameObject.SetActive(true);
    this.canvasGroup.alpha = 0.0f;
    this.modeIn = true;
  }

  private void OnEnable()
  {
    this.topText.key = "loading_screen_" + Randy.randomInt(1, 22).ToString();
    this.tipText.key = LoadingScreen.getTipID();
    this.topText.updateText();
    this.tipText.updateText();
    ((Component) this.topText).gameObject.SetActive(true);
    ((Component) this.tipText).gameObject.SetActive(true);
  }

  internal static string getTipID()
  {
    if (LoadingScreen._max_tip == 0)
    {
      for (int pTip = 0; pTip < 1000 && LocalizedTextManager.stringExists(LoadingScreen.getTip(pTip)); ++pTip)
        LoadingScreen._max_tip = pTip;
    }
    int pTip1 = Randy.randomInt(0, LoadingScreen._max_tip + 1);
    if (pTip1 == LoadingScreen._last_tip)
      return LoadingScreen.getTipID();
    LoadingScreen._last_tip = pTip1;
    return LoadingScreen.getTip(pTip1);
  }

  internal static string getTip(int pTip) => "tip" + Toolbox.fillLeft(pTip.ToString(), 3, '0');

  private void Update()
  {
    this.loadingHelperText.text = string.IsNullOrEmpty(SmoothLoader.latest_called_id) ? "" : $"{SmoothLoader.latest_called_id}:{SmoothLoader.latest_time}";
    if (this.inGameScreen)
    {
      if (this.modeIn)
      {
        if ((double) this.canvasGroup.alpha >= 1.0)
          this.startAction();
        this.canvasGroup.alpha += Time.deltaTime * 2f;
      }
      else if ((double) this.outTimer > 0.0)
      {
        this.outTimer -= Time.deltaTime;
      }
      else
      {
        if ((double) this.canvasGroup.alpha <= 0.0)
        {
          Config.enableAutoRotation(true);
          ((Component) this).gameObject.SetActive(false);
        }
        if (SmoothLoader.isLoading())
          return;
        this.canvasGroup.alpha -= Time.fixedDeltaTime * 2f;
      }
    }
    else
    {
      if (!this.appearDone)
      {
        this.canvasGroup.alpha += Time.deltaTime;
        if ((double) this.canvasGroup.alpha < 1.0)
          return;
        this.appearDone = true;
        this.StartCoroutine(this.LoadGame());
      }
      float num = ((Component) this.bar).transform.localScale.x;
      if ((double) ((Component) this.bar).transform.localScale.x < (double) this.asyncLoad.progress)
      {
        num = ((Component) this.bar).transform.localScale.x + Time.deltaTime * 2f;
        if ((double) num > (double) this.asyncLoad.progress)
          num = this.asyncLoad.progress;
        ((Component) this.bar).transform.localScale = new Vector3(num, 1f, 1f);
      }
      this.percents.text = Mathf.CeilToInt(this.asyncLoad.progress * 100f).ToString() + " %";
      if ((double) num < 0.89999997615814209)
        return;
      if (!this.asyncLoad.allowSceneActivation)
        Analytics.LogEvent("preloading_done");
      this.asyncLoad.allowSceneActivation = true;
    }
  }

  private IEnumerator LoadGame()
  {
    this.asyncLoad = SceneManager.LoadSceneAsync("World");
    this.asyncLoad.allowSceneActivation = false;
    while (!this.asyncLoad.isDone)
      yield return (object) null;
  }

  public delegate void TransitionAction();
}
