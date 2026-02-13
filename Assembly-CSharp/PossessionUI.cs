// Decompiled with JetBrains decompiler
// Type: PossessionUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PossessionUI : MonoBehaviour
{
  [SerializeField]
  private UiMover _mover;
  [SerializeField]
  private GameObject _inner;
  public static PossessionUI instance;
  private Text _text_backstep;
  private Text _text_dash;
  private Text _text_steal;
  private Text _text_yell;
  private Text _text_talk;
  private Text _text_walk;
  private Text _text_attack;
  private Text _text_special;
  private Text _text_jump;
  private Text _text_kick;
  private bool _planned_state;
  private bool _state_change_planned;

  private void Awake()
  {
    PossessionUI.instance = this;
    this._text_backstep = ((Component) ((Component) this).transform.FindRecursive("backstep")).GetComponent<Text>();
    this._text_dash = ((Component) ((Component) this).transform.FindRecursive("dash")).GetComponent<Text>();
    this._text_steal = ((Component) ((Component) this).transform.FindRecursive("steal")).GetComponent<Text>();
    this._text_yell = ((Component) ((Component) this).transform.FindRecursive("yell")).GetComponent<Text>();
    this._text_talk = ((Component) ((Component) this).transform.FindRecursive("talk")).GetComponent<Text>();
    this._text_walk = ((Component) ((Component) this).transform.FindRecursive("walk")).GetComponent<Text>();
    this._text_attack = ((Component) ((Component) this).transform.FindRecursive("attack")).GetComponent<Text>();
    this._text_special = ((Component) ((Component) this).transform.FindRecursive("special")).GetComponent<Text>();
    this._text_jump = ((Component) ((Component) this).transform.FindRecursive("jump")).GetComponent<Text>();
    this._text_kick = ((Component) ((Component) this).transform.FindRecursive("kick")).GetComponent<Text>();
    this.setText(this._text_backstep, "possession_tip_backstep", "CONTROL");
    this.setText(this._text_dash, "possession_tip_dash", "SHIFT");
    this.setText(this._text_steal, "possession_tip_steal", "Q");
    this.setText(this._text_yell, "possession_tip_yell", "F");
    this.setText(this._text_talk, "possession_tip_talk", "T");
    this.setText(this._text_walk, "possession_tip_walk", "WASD/ARROWS");
    this.setText(this._text_attack, "possession_tip_attack", "LEFT CLICK");
    this.setText(this._text_special, "possession_tip_special", "MIDDLE CLICK");
    this.setText(this._text_jump, "possession_tip_jump", "SPACE");
    this.setText(this._text_kick, "possession_tip_kick", "RIGHT CLICK");
    this.toggleInstance(false);
  }

  private void Update()
  {
    if (!this._state_change_planned)
      return;
    this._state_change_planned = false;
    if (this._mover.visible == this._planned_state)
      return;
    this.toggleInstance(this._planned_state);
  }

  private void setText(Text pTextField, string pLocaleID, string pKey)
  {
    string str = Toolbox.coloredString(LocalizedTextManager.getText(pLocaleID), "white");
    pKey = Toolbox.coloredString(pKey, "#F3961F");
    pTextField.text = $"{str} --> [ {pKey} ]";
  }

  public static void toggle(bool pState)
  {
    PossessionUI.instance._state_change_planned = true;
    PossessionUI.instance._planned_state = pState;
  }

  private void toggleInstance(bool pState)
  {
    if (pState)
      PossessionUI.instance._inner.SetActive(true);
    PossessionUI.instance._mover.setVisible(pState, pCompleteCallback: (TweenCallback) (() =>
    {
      if (pState || PossessionUI.instance._mover.visible)
        return;
      PossessionUI.instance._inner.SetActive(false);
    }));
  }
}
