// Decompiled with JetBrains decompiler
// Type: DebugKingdomFoes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class DebugKingdomFoes : MonoBehaviour
{
  [SerializeField]
  private DebugKingdomButton _prefab_button;
  [SerializeField]
  private Image _selector;
  [SerializeField]
  private GridLayoutGroup _grid_main;
  [SerializeField]
  private GridLayoutGroup _grid_civs;
  [SerializeField]
  private GridLayoutGroup _grid_minicivs;
  [SerializeField]
  private GridLayoutGroup _grid_minicivs_special;
  [SerializeField]
  private GridLayoutGroup _grid_concepts;
  [SerializeField]
  private GridLayoutGroup _grid_mobs;
  [SerializeField]
  private GridLayoutGroup _grid_creeps;
  [SerializeField]
  private GridLayoutGroup _grid_others;
  private List<DebugKingdomButton> _buttons = new List<DebugKingdomButton>();
  private KingdomAsset _current_selected;
  private bool _initialized;

  private void Awake() => this.create();

  private void create()
  {
    if (this._initialized)
      return;
    this._initialized = true;
    AssetManager.kingdoms.checkForMissingTags();
    foreach (KingdomAsset pAsset in AssetManager.kingdoms.list)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      DebugKingdomFoes.\u003C\u003Ec__DisplayClass14_0 cDisplayClass140 = new DebugKingdomFoes.\u003C\u003Ec__DisplayClass14_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass140.\u003C\u003E4__this = this;
      if (!pAsset.isTemplateAsset())
      {
        Transform transform = !pAsset.group_main ? (!pAsset.group_creeps ? (!pAsset.concept ? (!pAsset.is_forced_by_trait ? (!pAsset.group_minicivs_cool ? (!pAsset.group_miniciv ? (!pAsset.civ ? (!pAsset.mobs ? ((Component) this._grid_others).transform : ((Component) this._grid_mobs).transform) : ((Component) this._grid_civs).transform) : ((Component) this._grid_minicivs).transform) : ((Component) this._grid_minicivs_special).transform) : ((Component) this._grid_others).transform) : ((Component) this._grid_concepts).transform) : ((Component) this._grid_creeps).transform) : ((Component) this._grid_main).transform;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass140.tNewButton = Object.Instantiate<DebugKingdomButton>(this._prefab_button, transform);
        // ISSUE: reference to a compiler-generated field
        cDisplayClass140.tNewButton.setAsset(pAsset);
        // ISSUE: reference to a compiler-generated field
        this._buttons.Add(cDisplayClass140.tNewButton);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        ((UnityEvent) ((Component) cDisplayClass140.tNewButton).GetComponent<Button>().onClick).AddListener(new UnityAction((object) cDisplayClass140, __methodptr(\u003Ccreate\u003Eb__0)));
      }
    }
    this.select(this._buttons.GetRandom<DebugKingdomButton>());
  }

  private void select(DebugKingdomButton pButton)
  {
    this._current_selected = pButton.kingdom_asset;
    ((Component) this._selector).transform.position = ((Component) pButton).transform.position;
    this.updateButtons();
  }

  private void updateButtons()
  {
    foreach (DebugKingdomButton button in this._buttons)
      button.checkSelected(this._current_selected);
  }
}
