// Decompiled with JetBrains decompiler
// Type: WorldLawsWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class WorldLawsWindow : TabbedWindow
{
  [SerializeField]
  private WorldLawElement _cursed_world_button;
  [SerializeField]
  private GameObject _blackhole_butt;
  [SerializeField]
  private Image _center_blackhole;
  [SerializeField]
  private Transform _blackhole_container;
  [SerializeField]
  private Sprite _blackhole_normal;
  [SerializeField]
  private Sprite _blackhole_normal_eye;
  [SerializeField]
  private Sprite _blackhole_normal_eye_open;
  [SerializeField]
  private LayoutElement _background_star_mark_element;
  [SerializeField]
  private GameObject _description_forbidden_knowledge_1_before_sacrifice;
  [SerializeField]
  private GameObject _description_forbidden_knowledge_2_non_cursed;
  [SerializeField]
  private GameObject _description_forbidden_knowledge_3_cursed;
  [SerializeField]
  private GameObject _description_forbidden_knowledge_warn;

  protected override void create()
  {
    base.create();
    this.initCursedWorld();
  }

  private void initCursedWorld()
  {
    this._cursed_world_button.init(WorldLawLibrary.world_law_cursed_world);
    // ISSUE: method pointer
    this._cursed_world_button.addListener(new UnityAction((object) this, __methodptr(\u003CinitCursedWorld\u003Eb__13_0)));
  }

  private void checkShakeAndClose()
  {
    if (CursedSacrifice.justGotCursedWorld())
    {
      World.world.startShake(pShakeX: true);
      this.checkForbiddenKnowledgeElements();
    }
    WorldLawsTextInsult.removeInsultTimeout();
    this.scroll_window.shake();
  }

  private void OnEnable() => this.checkForbiddenKnowledgeElements();

  private void checkForbiddenKnowledgeElements()
  {
    bool flag1 = WorldLawLibrary.world_law_cursed_world.isEnabled();
    bool flag2 = CursedSacrifice.isWorldReadyForCURSE();
    this._description_forbidden_knowledge_3_cursed.SetActive(flag1);
    if (flag1)
    {
      this._description_forbidden_knowledge_1_before_sacrifice.SetActive(false);
      this._description_forbidden_knowledge_2_non_cursed.SetActive(false);
    }
    else
    {
      this._description_forbidden_knowledge_1_before_sacrifice.SetActive(!flag2);
      this._description_forbidden_knowledge_2_non_cursed.SetActive(flag2);
    }
    this._description_forbidden_knowledge_warn.SetActive(flag2);
    this._background_star_mark_element.minHeight = !flag2 ? 205f : 180f;
    float num = Mathf.Lerp(0.5f, 1f, CursedSacrifice.getCurseProgressRatioForBlackhole());
    ((Component) this._blackhole_container).transform.localScale = new Vector3(num, num, num);
    this._center_blackhole.sprite = !flag1 ? (!flag2 ? this._blackhole_normal : this._blackhole_normal_eye) : this._blackhole_normal_eye_open;
    ((Component) this._cursed_world_button).gameObject.SetActive(flag2);
    this._blackhole_butt.SetActive(flag2);
  }
}
