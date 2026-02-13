// Decompiled with JetBrains decompiler
// Type: ArchitectTab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ArchitectTab : MonoBehaviour
{
  private Dictionary<ArchitectMood, ArchitectMoodButton> _buttons = new Dictionary<ArchitectMood, ArchitectMoodButton>();
  [SerializeField]
  private ArchitectMoodButton _mood_prefab;
  [SerializeField]
  private Transform _grid_placement;

  private void Awake() => this.initButtons();

  private void initButtons()
  {
    for (int index = 0; index < AssetManager.architect_mood_library.list.Count; ++index)
    {
      ArchitectMood architectMood = AssetManager.architect_mood_library.list[index];
      ArchitectMoodButton architectMoodButton = this.initButton(architectMood);
      this._buttons.Add(architectMood, architectMoodButton);
    }
  }

  private ArchitectMoodButton initButton(ArchitectMood pAsset)
  {
    ArchitectMoodButton architectMoodButton = Object.Instantiate<ArchitectMoodButton>(this._mood_prefab, this._grid_placement);
    architectMoodButton.setAsset(pAsset);
    architectMoodButton.addClickCallback(new ArchitectMoodAction(this.buttonAction));
    return architectMoodButton;
  }

  private void buttonAction(ArchitectMoodButton pElement)
  {
    World.world.map_stats.player_mood = pElement.getAsset().id;
    World.world.clearArchitectMood();
    this.updateElements();
  }

  private void updateElements()
  {
    ArchitectMood architectMood = World.world.getArchitectMood();
    foreach (ArchitectMoodButton architectMoodButton in this._buttons.Values)
    {
      bool pState = architectMoodButton.getAsset() == architectMood;
      architectMoodButton.toggleSelectedButton(pState);
      architectMoodButton.setIconActiveColor(pState);
    }
  }

  private void OnEnable() => this.updateElements();
}
