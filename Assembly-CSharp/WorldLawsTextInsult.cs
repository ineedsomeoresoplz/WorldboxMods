// Decompiled with JetBrains decompiler
// Type: WorldLawsTextInsult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WorldLawsTextInsult : MonoBehaviour
{
  [SerializeField]
  private Transform _follow_object;
  [SerializeField]
  private RectTransform _size_parent;
  [SerializeField]
  private Text _text;
  private static float _global_wait_timeout;
  private const float RARE_INSULT_CHANCE = 0.005f;
  private float _wait_time;
  private Tweener _text_tweener;
  private string[] _insults_rare = new string[14]
  {
    "UPDATE?",
    "WHEN",
    "GEB",
    "BRE",
    "REBR",
    "MODERN?",
    "HELP",
    "CAKE",
    "BRURSE",
    "MAXIM",
    "MASTEF",
    "HUGO",
    "NIKON",
    "JECO"
  };

  public static void removeInsultTimeout() => WorldLawsTextInsult._global_wait_timeout = 0.0f;

  public void Update()
  {
    if (!this.shouldInsultNow())
      return;
    if ((double) this._wait_time > 0.0)
      this._wait_time -= Time.deltaTime;
    else if ((double) WorldLawsTextInsult._global_wait_timeout > 0.0 && !this.isTweening())
    {
      WorldLawsTextInsult._global_wait_timeout -= Time.deltaTime;
    }
    else
    {
      if (this.isTweening())
        return;
      this.startNewTween();
    }
  }

  private void startNewTween()
  {
    this.killTweens();
    WorldLawsTextInsult._global_wait_timeout = !WorldLawLibrary.world_law_cursed_world.isEnabled() ? 2f + Randy.randomFloat(0.0f, 3f) : 0.6f + Randy.randomFloat(0.0f, 2f);
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(0.0f, 0.0f, Randy.randomFloat(-30f, 30f));
    ((Transform) this._size_parent).localRotation = Quaternion.Euler(vector3);
    this._text.text = this.getInsultText();
    ((Component) this._text).transform.position = Vector3.op_Addition(this._follow_object.position, new Vector3(0.0f, (float) Randy.randomInt(8, 12), 0.0f));
    this._text.fontSize = Randy.randomInt(7, 9);
    this._text_tweener = (Tweener) TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOMove(((Component) this._text).transform, Vector3.op_Addition(((Component) this._text).transform.position, new Vector3(0.0f, Randy.randomFloat(30f, 60f), 0.0f)), 6f, false), (Ease) 9);
    ((Tween) DOTweenModuleUI.DOColor(this._text, Color.white, 2f)).onComplete = new TweenCallback(this.doTextFade);
  }

  private string getInsultText()
  {
    return !Randy.randomChance(0.005f) ? InsultStringGenerator.getRandomText() : this._insults_rare.GetRandom<string>();
  }

  private void doTextFade()
  {
    ((Tween) DOTweenModuleUI.DOFade(this._text, 0.0f, 2f)).onComplete = new TweenCallback(this.doWait);
  }

  private bool shouldInsultNow()
  {
    return CursedSacrifice.isWorldReadyForCURSE() || WorldLawLibrary.world_law_cursed_world.isEnabled();
  }

  private bool isTweening() => TweenExtensions.IsActive((Tween) this._text_tweener);

  private void OnEnable() => this.doWait();

  private void doWait()
  {
    this.killTweens();
    this._wait_time = Randy.randomFloat(0.0f, 7f);
    ((Graphic) this._text).color = Toolbox.color_white_transparent;
  }

  private void killTweens()
  {
    Tweener textTweener = this._text_tweener;
    if (textTweener != null)
      TweenExtensions.Kill((Tween) textTweener, false);
    ShortcutExtensions.DOKill((Component) this._text, false);
  }
}
