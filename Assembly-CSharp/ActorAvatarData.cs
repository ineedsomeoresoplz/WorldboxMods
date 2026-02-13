// Decompiled with JetBrains decompiler
// Type: ActorAvatarData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ActorAvatarData
{
  public ActorAsset asset;
  public SubspeciesTrait mutation_skin_asset;
  public ActorSex sex;
  public Sprite sprite_head;
  public int head_id;
  public long actor_id;
  public int phenotype_index;
  public int phenotype_skin_shade;
  public ColorAsset kingdom_color;
  public bool is_egg;
  public bool is_king;
  public bool is_warrior;
  public bool is_wise;
  public SubspeciesTrait egg_asset;
  public bool is_adult;
  public bool is_lying;
  public bool is_touching_liquid;
  public bool is_inside_boat;
  public bool is_hovering;
  public bool is_immovable;
  public bool is_unconscious;
  public bool is_stop_idle_animation;
  public IHandRenderer item_renderer;
  public int actor_hash;
  public IEnumerable<string> statuses;
  public IReadOnlyDictionary<string, Status> statuses_gameplay;

  public void setData(Actor pActor)
  {
    ActorAsset actorAsset = pActor.getActorAsset();
    ActorData data = pActor.data;
    this.setData(actorAsset, pActor.subspecies?.mutation_skin_asset, data.sex, data.id, data.head, pActor.cached_sprite_head, data.phenotype_index, data.phenotype_shade, pActor.kingdom.getColor(), pActor.isEgg(), pActor.isKing(), pActor.isWarrior() && !pActor.equipment.helmet.isEmpty(), pActor.hasTrait("wise"), pActor.subspecies?.egg_asset, pActor.isAdult(), !actorAsset.prevent_unconscious_rotation && pActor.isLying(), pActor.isTouchingLiquid(), pActor.is_inside_boat, pActor.isHovering(), pActor.isImmovable(), !actorAsset.prevent_unconscious_rotation && pActor.is_unconscious, pActor.hasStopIdleAnimation(), pActor.getHandRendererAsset(), pActor.GetHashCode(), (IEnumerable<string>) pActor.getStatusesIds(), pActor.getStatusesDict());
  }

  public void setData(
    ActorAsset pAsset,
    SubspeciesTrait pMutation,
    ActorSex pSex,
    long pActorId,
    int pHeadId,
    Sprite pSpriteHead,
    int pPhenotypeIndex,
    int pPhenotypeSkinShade,
    ColorAsset pKingdomColor,
    bool pIsEgg,
    bool pIsKing,
    bool pIsWarrior,
    bool pIsWise,
    SubspeciesTrait pEggAsset,
    bool pIsAdult,
    bool pIsLying,
    bool pIsTouchingLiquid,
    bool pIsInsideBoat,
    bool pIsHovering,
    bool pIsImmovable,
    bool pIsUnconscious,
    bool pIsStopIdleAnimation,
    IHandRenderer pItemPath,
    int pActorHash,
    IEnumerable<string> pStatuses,
    IReadOnlyDictionary<string, Status> pGameplayStatuses)
  {
    this.asset = pAsset;
    this.mutation_skin_asset = pMutation;
    this.sex = pSex;
    this.sprite_head = pSpriteHead;
    this.actor_id = pActorId;
    this.head_id = pHeadId;
    this.phenotype_index = pPhenotypeIndex;
    this.phenotype_skin_shade = pPhenotypeSkinShade;
    this.kingdom_color = pKingdomColor;
    this.is_egg = pIsEgg;
    this.is_king = pIsKing;
    this.is_warrior = pIsWarrior;
    this.is_wise = pIsWise;
    this.egg_asset = pEggAsset;
    this.is_adult = pIsAdult;
    this.is_lying = pIsLying;
    this.is_touching_liquid = pIsTouchingLiquid;
    this.is_inside_boat = pIsInsideBoat;
    this.is_hovering = pIsHovering;
    this.is_immovable = pIsImmovable;
    this.is_unconscious = pIsUnconscious;
    this.is_stop_idle_animation = pIsStopIdleAnimation;
    this.item_renderer = pItemPath;
    this.actor_hash = pActorHash;
    this.statuses = pStatuses;
    this.statuses_gameplay = pGameplayStatuses;
  }

  public ActorTextureSubAsset getTextureAsset()
  {
    return this.mutation_skin_asset == null ? this.asset.texture_asset : this.mutation_skin_asset.texture_asset;
  }

  public Sprite getColoredSprite(Sprite pSprite, AnimationContainerUnit pContainer)
  {
    return DynamicActorSpriteCreatorUI.getUnitSpriteForUI(this.asset, pSprite, pContainer, this.is_adult, this.sex, this.phenotype_index, this.phenotype_skin_shade, this.kingdom_color, this.actor_id, this.head_id, this.is_egg, this.is_king, this.is_warrior, this.is_wise);
  }

  public bool hasRenderedSpriteHead()
  {
    return Object.op_Inequality((Object) this.sprite_head, (Object) null);
  }
}
