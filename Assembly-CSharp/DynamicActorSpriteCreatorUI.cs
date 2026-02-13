// Decompiled with JetBrains decompiler
// Type: DynamicActorSpriteCreatorUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class DynamicActorSpriteCreatorUI
{
  private static int[] _boat_angles = new int[8]
  {
    0,
    -45,
    -90,
    -135,
    180,
    135,
    90,
    45
  };

  public static AnimationContainerUnit getContainerForUI(
    ActorAsset pAsset,
    bool pAdult,
    ActorTextureSubAsset pTextureAsset,
    SubspeciesTrait pMutationAsset = null,
    bool pIsEgg = false,
    SubspeciesTrait pEggAsset = null,
    string pTexturePath = null)
  {
    return ActorAnimationLoader.getAnimationContainer(string.IsNullOrEmpty(pTexturePath) ? (!pIsEgg ? (pAdult || !pAsset.has_baby_form ? pTextureAsset.getUnitTexturePathForUI(pAsset) : pTextureAsset.texture_path_baby) : pEggAsset.sprite_path) : pTexturePath, pAsset, pEggAsset, pMutationAsset);
  }

  public static AnimationContainerUnit getContainerForUI(Actor pActor)
  {
    Subspecies subspecies = pActor.subspecies;
    ActorAsset asset = pActor.asset;
    SubspeciesTrait pMutationAsset = (SubspeciesTrait) null;
    ActorTextureSubAsset textureAsset;
    if (pActor.hasSubspecies() && subspecies.has_mutation_reskin)
    {
      pMutationAsset = subspecies.mutation_skin_asset;
      textureAsset = pMutationAsset.texture_asset;
    }
    else
      textureAsset = asset.texture_asset;
    return DynamicActorSpriteCreatorUI.getContainerForUI(asset, pActor.isAdult(), textureAsset, pMutationAsset);
  }

  public static Sprite getUnitSpriteForUI(
    ActorAsset pAsset,
    Sprite pMainSprite,
    AnimationContainerUnit pContainer,
    bool pAdult,
    ActorSex pSex,
    int pPhenotypeIndex,
    int pPhenotypeShade,
    ColorAsset pKingdomColor,
    long pActorId,
    int pHeadId,
    bool pEgg = false,
    bool pKing = false,
    bool pWarrior = false,
    bool pWise = false)
  {
    long num1 = 0;
    long num2 = (long) pPhenotypeIndex;
    long num3 = 0;
    long num4 = 0;
    long bodySpriteSmallId = (long) DynamicSpriteCreator.getBodySpriteSmallID(pMainSprite);
    if (num2 != 0L)
      num3 = (long) (pPhenotypeShade + 1);
    Sprite spriteHeadForUi = DynamicActorSpriteCreatorUI.getSpriteHeadForUI(pAsset, pSex, pContainer, pActorId, pHeadId, pAdult, pEgg, pKing, pWarrior, pWise);
    int num5 = 0;
    if (Object.op_Inequality((Object) spriteHeadForUi, (Object) null))
    {
      ActorAnimationLoader.int_ids_heads.TryGetValue(spriteHeadForUi, out num5);
      if (num5 == 0)
      {
        int num6 = ActorAnimationLoader.int_ids_heads.Count + 1;
        ActorAnimationLoader.int_ids_heads.Add(spriteHeadForUi, num6);
        num5 = num6;
      }
      num4 = (long) num5;
    }
    if (pKingdomColor != null)
      num1 = (long) (pKingdomColor.index_id + 1);
    long num7 = num1 * 10000000L + num4 * 1000000L + bodySpriteSmallId * 1000L + num2 * 10L + num3;
    AnimationFrameData pFrameData = (AnimationFrameData) null;
    pContainer?.dict_frame_data.TryGetValue(((Object) pMainSprite).name, out pFrameData);
    DynamicSpritesAsset units = DynamicSpritesLibrary.units;
    Sprite pSprite = units.getSprite(num7);
    if (pSprite == null)
    {
      pSprite = DynamicSpriteCreator.createNewSpriteUnit(pFrameData, pMainSprite, spriteHeadForUi, pKingdomColor, pAsset, pPhenotypeIndex, pPhenotypeShade, pAsset.texture_atlas);
      units.addSprite(num7, pSprite);
    }
    return pSprite;
  }

  public static Sprite getUnitSpriteForUI(Actor pActor, Sprite pSprite)
  {
    ActorAsset asset = pActor.asset;
    AnimationContainerUnit animationContainer = pActor.animation_container;
    Sprite unitSpriteForUi;
    if (asset.has_override_avatar_frames)
    {
      unitSpriteForUi = asset.get_override_avatar_frames(pActor)[0];
    }
    else
    {
      int phenotypeShade = pActor.data.phenotype_shade;
      int phenotypeIndex = pActor.data.phenotype_index;
      unitSpriteForUi = DynamicActorSpriteCreatorUI.getUnitSpriteForUI(pActor.asset, pSprite, animationContainer, pActor.isAdult(), pActor.data.sex, phenotypeIndex, phenotypeShade, pActor.kingdom.getColor(), pActor.data.id, pActor.data.head, pActor.isEgg());
    }
    return unitSpriteForUi;
  }

  public static Sprite getSpriteHeadForUI(
    ActorAsset pAsset,
    ActorSex pSex,
    AnimationContainerUnit pContainer,
    long pActorId,
    int pHeadId,
    bool pAdult = true,
    bool pEgg = false,
    bool pKing = false,
    bool pWarrior = false,
    bool pWise = false,
    bool pRandom = false)
  {
    if (pEgg)
      return (Sprite) null;
    if (pAsset.is_boat)
      return (Sprite) null;
    if (!pAdult && !pContainer.render_heads_for_children)
      return (Sprite) null;
    string pPath = "";
    bool flag = false;
    ActorTextureSubAsset textureAsset = pAsset.texture_asset;
    if (!textureAsset.has_advanced_textures)
    {
      Sprite[] heads = pContainer.heads;
      if ((heads != null ? (heads.Length != 0 ? 1 : 0) : 0) == 0)
        return (Sprite) null;
      if (pRandom)
        return pContainer.heads.GetRandom<Sprite>();
      int spriteIndex = AnimationHelper.getSpriteIndex(pActorId, pContainer.heads.Length);
      return pContainer.heads[spriteIndex];
    }
    if (pKing)
    {
      pPath = textureAsset.texture_head_king;
      flag = true;
    }
    else if (pWarrior)
    {
      pPath = textureAsset.texture_head_warrior;
      flag = true;
    }
    else if (pWise && textureAsset.has_old_heads)
    {
      flag = true;
      pPath = pSex != ActorSex.Male ? textureAsset.texture_heads_old_female : textureAsset.texture_heads_old_male;
    }
    if (flag)
      return ActorAnimationLoader.getHeadSpecial(pPath);
    if (pSex == ActorSex.Male)
    {
      if (pRandom)
        return pContainer.heads_male.GetRandom<Sprite>();
      int index = pHeadId != -1 ? pHeadId : AnimationHelper.getSpriteIndex(pActorId, pContainer.heads_male.Length);
      return pContainer.heads_male[index];
    }
    if (pRandom)
      return pContainer.heads_female.GetRandom<Sprite>();
    int index1 = pHeadId != -1 ? pHeadId : AnimationHelper.getSpriteIndex(pActorId, pContainer.heads_female.Length);
    return pContainer.heads_female[index1];
  }

  public static ActorAnimation getBoatAnimation(AnimationDataBoat pBoatAnimation)
  {
    ActorAnimation boatAnimation = new ActorAnimation();
    Sprite[] spriteArray = new Sprite[DynamicActorSpriteCreatorUI._boat_angles.Length * 2];
    for (int index1 = 0; index1 < DynamicActorSpriteCreatorUI._boat_angles.Length; ++index1)
    {
      int boatAngle = DynamicActorSpriteCreatorUI._boat_angles[index1];
      ActorAnimation actorAnimation = pBoatAnimation.dict[boatAngle];
      int index2 = index1 * 2;
      spriteArray[index2] = actorAnimation.frames[0];
      spriteArray[index2 + 1] = actorAnimation.frames[1];
    }
    boatAnimation.frames = spriteArray;
    return boatAnimation;
  }
}
