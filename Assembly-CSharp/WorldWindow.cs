// Decompiled with JetBrains decompiler
// Type: WorldWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine.Events;

#nullable disable
public class WorldWindow : TabbedWindow, IInterestingPeopleWindow
{
  public NameInput nameInput;
  public InterestingPeopleTab interesting_people;

  protected override void create()
  {
    base.create();
    // ISSUE: method pointer
    this.nameInput.addListener(new UnityAction<string>((object) this, __methodptr(applyInputName)));
  }

  private void applyInputName(string pInput)
  {
    if (string.IsNullOrEmpty(pInput))
      return;
    World.world.map_stats.name = pInput;
  }

  private void OnEnable()
  {
    if (World.world.map_stats == null)
      return;
    this.nameInput.setText(World.world.map_stats.name);
  }

  public IEnumerable<Actor> getInterestingUnitsList() => (IEnumerable<Actor>) World.world.units;
}
