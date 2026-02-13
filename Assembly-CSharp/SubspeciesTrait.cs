// Decompiled with JetBrains decompiler
// Type: SubspeciesTrait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

#nullable disable
[Serializable]
public class SubspeciesTrait : BaseTrait<SubspeciesTrait>, IAnimationFrames
{
  public bool in_mutation_pot_add;
  public bool in_mutation_pot_remove;
  public bool phenotype_skin;
  public bool phenotype_egg;
  public bool is_diet_related;
  [DefaultValue("")]
  public string id_phenotype = string.Empty;
  [DefaultValue("")]
  public string id_egg = string.Empty;
  public AfterHatchFromEggAction after_hatch_from_egg_action;
  public bool has_after_hatch_from_egg_action;
  public bool remove_for_zombies;
  public bool is_mutation_skin;
  public string sprite_path;
  public ActorTextureSubAsset texture_asset;
  public bool prevent_unconscious_rotation;
  public bool render_heads_for_children;
  public List<string> skin_citizen_male;
  public List<string> skin_citizen_female;
  public List<string> skin_warrior;
  public string[] animation_walk = ActorAnimationSequences.walk_0;
  [DefaultValue(10f)]
  public float animation_walk_speed = 10f;
  public string[] animation_swim = ActorAnimationSequences.swim_0;
  [DefaultValue(8f)]
  public float animation_swim_speed = 8f;
  public string[] animation_idle = ActorAnimationSequences.walk_0;
  [DefaultValue(10f)]
  public float animation_idle_speed = 10f;
  [DefaultValue(true)]
  public bool shadow = true;
  [DefaultValue("unitShadow_5")]
  public string shadow_texture = "unitShadow_5";
  [DefaultValue("unitShadow_2")]
  public string shadow_texture_egg = "unitShadow_2";
  [DefaultValue("unitShadow_4")]
  public string shadow_texture_baby = "unitShadow_4";

  protected override HashSet<string> progress_elements
  {
    get => this._progress_data?.unlocked_traits_subspecies;
  }

  public override string typed_id => "subspecies_trait";

  public override BaseCategoryAsset getGroup()
  {
    return (BaseCategoryAsset) AssetManager.subspecies_trait_groups.get(this.group_id);
  }

  public override Sprite getSprite()
  {
    if (this.cached_sprite == null)
    {
      Sprite sprite = SpriteTextureLoader.getSprite(this.path_icon);
      if (this.special_icon_logic)
      {
        if (this.phenotype_skin)
        {
          PhenotypeAsset phenotypeAsset = this.getPhenotypeAsset();
          this.cached_sprite = DynamicSprites.getIconWithColors(sprite, phenotypeAsset, (ColorAsset) null);
        }
      }
      else
        this.cached_sprite = sprite;
    }
    return this.cached_sprite;
  }

  public PhenotypeAsset getPhenotypeAsset()
  {
    return this.phenotype_skin ? AssetManager.phenotype_library.get(this.id_phenotype) : (PhenotypeAsset) null;
  }

  protected override IEnumerable<ITraitsOwner<SubspeciesTrait>> getRelatedMetaList()
  {
    return (IEnumerable<ITraitsOwner<SubspeciesTrait>>) World.world.subspecies;
  }

  public override string getCountRows() => this.getCountRowsByCategories();

  protected override bool isSapient(ITraitsOwner<SubspeciesTrait> pObject)
  {
    return ((Subspecies) pObject).isSapient();
  }

  public string[] getWalk() => this.animation_walk;

  public string[] getIdle() => this.animation_idle;

  public string[] getSwim() => this.animation_swim;
}
