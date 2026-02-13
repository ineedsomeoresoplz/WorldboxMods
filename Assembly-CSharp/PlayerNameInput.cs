// Decompiled with JetBrains decompiler
// Type: PlayerNameInput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
[RequireComponent(typeof (NameInput))]
public class PlayerNameInput : MonoBehaviour
{
  private NameInput _name_input;

  private void Awake()
  {
    this._name_input = ((Component) this).GetComponent<NameInput>();
    // ISSUE: method pointer
    this._name_input.addListener(new UnityAction<string>((object) this, __methodptr(inputAction)));
  }

  private void Update()
  {
    ((Graphic) this._name_input.textField).color = World.world.getArchitectMood().getColorText();
  }

  private void OnEnable() => this._name_input.setText(World.world.map_stats.player_name);

  private void inputAction(string pInput) => World.world.map_stats.player_name = pInput;
}
