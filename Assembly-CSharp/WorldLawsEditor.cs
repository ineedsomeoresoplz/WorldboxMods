// Decompiled with JetBrains decompiler
// Type: WorldLawsEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WorldLawsEditor : MonoBehaviour
{
  [SerializeField]
  private WorldLawElement _element_prefab;
  [SerializeField]
  private WorldLawCategory _category_prefab;
  [SerializeField]
  private Transform _categories_parent;
  private Dictionary<string, WorldLawCategory> _categories_dict = new Dictionary<string, WorldLawCategory>();

  private void Awake() => this.create();

  private void OnEnable() => this.updateButtons();

  private void create()
  {
    this.createCategories();
    this.createElements();
  }

  private void createCategories()
  {
    foreach (WorldLawGroupAsset pGroupAsset in AssetManager.world_law_groups.list)
    {
      WorldLawCategory worldLawCategory = Object.Instantiate<WorldLawCategory>(this._category_prefab, this._categories_parent);
      this._categories_dict.Add(pGroupAsset.id, worldLawCategory);
      worldLawCategory.init(pGroupAsset);
    }
  }

  private void createElements()
  {
    foreach (WorldLawAsset pAsset in AssetManager.world_laws_library.list)
    {
      if (!string.IsNullOrEmpty(pAsset.group_id))
      {
        WorldLawCategory worldLawCategory = this._categories_dict[pAsset.group_id];
        WorldLawElement pElement = Object.Instantiate<WorldLawElement>(this._element_prefab, ((Component) worldLawCategory.grid).transform);
        ((Object) pElement).name = pAsset.id;
        pElement.init(pAsset);
        worldLawCategory.addElement(pElement);
      }
    }
  }

  private void updateButtons()
  {
    foreach (WorldLawCategory worldLawCategory in this._categories_dict.Values)
      worldLawCategory.updateButtons();
  }

  public void resetToDefault()
  {
    foreach (WorldLawAsset worldLawAsset in AssetManager.world_laws_library.list)
    {
      if (worldLawAsset.can_turn_off)
      {
        PlayerOptionData option = worldLawAsset.getOption();
        bool boolVal = option.boolVal;
        option.boolVal = worldLawAsset.default_state;
        if (option.boolVal && !boolVal)
        {
          PlayerOptionAction onStateEnabled = worldLawAsset.on_state_enabled;
          if (onStateEnabled != null)
            onStateEnabled(option);
        }
        PlayerOptionAction onStateChange = worldLawAsset.on_state_change;
        if (onStateChange != null)
          onStateChange(option);
      }
    }
    World.world.world_laws.updateCaches();
    this.updateButtons();
  }
}
