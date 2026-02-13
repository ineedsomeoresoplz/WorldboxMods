// Decompiled with JetBrains decompiler
// Type: ResourceAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class ResourceAsset : Asset, ILocalizedAsset, IHandRenderer
{
  public string path_icon;
  public string path_gameplay_sprite = "bag_default";
  [NonSerialized]
  public Sprite[] gameplay_sprites;
  public int drop_max = 30;
  public int drop_per_mass = 50;
  private Sprite _cached_sprite_icon;
  private Sprite _cached_gameplay_sprite;
  public ResType type = ResType.Ingredient;
  public int mine_rate;
  public int maximum = 999;
  public bool wood;
  public bool mineral;
  public ResourceEatAction eat_action;
  public int restore_nutrition;
  public int restore_happiness;
  public int restore_mana;
  public int restore_stamina;
  public int produce_min = 10;
  public int stack_size = 15;
  public int loot_value = 1;
  public int money_cost = 2;
  public float restore_health;
  public int give_experience;
  public int ingredients_amount = 1;
  public string[] ingredients;
  public string[] diet;
  public bool food;
  public int supply_bound_give = 30;
  public int supply_bound_take = 10;
  public float favorite_food_chance = 0.5f;
  public float tastiness = 1f;
  public int supply_give = 10;
  public int trade_bound = 40;
  public int trade_give = 10;
  public int trade_cost = 1;
  public int storage_max = 50;
  public float give_chance;
  public string[] give_trait_id;
  [NonSerialized]
  public ActorTrait[] give_trait;
  public string[] give_status_id;
  [NonSerialized]
  public StatusAsset[] give_status;
  public ResourceEatAction give_action;
  [NonSerialized]
  public int order = -1;
  public string tooltip = "city_resource";
  public string full_sprite_path;

  public Sprite getSpriteIcon()
  {
    if (Object.op_Equality((Object) this._cached_sprite_icon, (Object) null))
      this._cached_sprite_icon = SpriteTextureLoader.getSprite("ui/Icons/" + this.path_icon);
    return this._cached_sprite_icon;
  }

  public Sprite getGameplaySprite()
  {
    if (Object.op_Equality((Object) this._cached_gameplay_sprite, (Object) null))
      this._cached_gameplay_sprite = this.gameplay_sprites[0];
    return this._cached_gameplay_sprite;
  }

  public string getLocaleID() => this.id;

  public string getTranslatedName() => this.getLocaleID().Localize();

  public Sprite[] getSprites() => this.gameplay_sprites;

  public bool is_colored => false;

  public bool is_animated => false;
}
