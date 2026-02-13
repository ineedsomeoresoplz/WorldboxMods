// Decompiled with JetBrains decompiler
// Type: ActorAnimationLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class ActorAnimationLoader
{
  public static readonly Dictionary<Sprite, int> int_ids_heads = new Dictionary<Sprite, int>();
  private static readonly Dictionary<string, AnimationContainerUnit> _dict_units = new Dictionary<string, AnimationContainerUnit>();
  private static readonly Dictionary<string, AnimationDataBoat> _dict_boats = new Dictionary<string, AnimationDataBoat>();
  private static readonly Dictionary<string, Sprite> _dict_civ_heads = new Dictionary<string, Sprite>();

  public static int count_units => ActorAnimationLoader._dict_units.Count;

  public static int count_boats => ActorAnimationLoader._dict_boats.Count;

  public static int count_heads => ActorAnimationLoader._dict_civ_heads.Count;

  public static Sprite getHeadSpecial(string pPath)
  {
    string key = pPath;
    Sprite dictCivHead;
    if (!ActorAnimationLoader._dict_civ_heads.TryGetValue(key, out dictCivHead))
    {
      foreach (Sprite sprite in SpriteTextureLoader.getSpriteList(pPath))
      {
        string str = pPath;
        ActorAnimationLoader._dict_civ_heads.TryAdd(str, sprite);
      }
      dictCivHead = ActorAnimationLoader._dict_civ_heads[key];
    }
    return dictCivHead;
  }

  public static Sprite getHead(string pPath, int pHeadIndex)
  {
    string key = $"{pPath}_head_{pHeadIndex}";
    Sprite dictCivHead;
    if (!ActorAnimationLoader._dict_civ_heads.TryGetValue(key, out dictCivHead))
    {
      foreach (Sprite sprite in SpriteTextureLoader.getSpriteList(pPath))
      {
        string str = $"{pPath}_{((Object) sprite).name}";
        ActorAnimationLoader._dict_civ_heads.TryAdd(str, sprite);
      }
      dictCivHead = ActorAnimationLoader._dict_civ_heads[key];
    }
    return dictCivHead;
  }

  public static AnimationDataBoat loadAnimationBoat(string pTexturePath)
  {
    AnimationDataBoat pAnimationData;
    if (!ActorAnimationLoader._dict_boats.TryGetValue(pTexturePath, out pAnimationData))
    {
      Dictionary<string, Sprite> pDict = new Dictionary<string, Sprite>();
      Sprite[] spriteList = SpriteTextureLoader.getSpriteList("actors/boats/" + pTexturePath);
      foreach (Sprite sprite in spriteList)
        pDict.Add(((Object) sprite).name, sprite);
      pAnimationData = new AnimationDataBoat()
      {
        broken = new ActorAnimation()
      };
      pAnimationData.broken.frames = new Sprite[1]
      {
        pDict["broken"]
      };
      pAnimationData.normal = new ActorAnimation();
      pAnimationData.normal.frames = new Sprite[1]
      {
        pDict["normal"]
      };
      foreach (Sprite sprite in spriteList)
      {
        if (!((Object) sprite).name.Contains("@1") && ((Object) sprite).name.Contains("@"))
          ActorAnimationLoader.createBoatAnimationArray(pAnimationData, pDict, ((Object) sprite).name);
      }
      ActorAnimationLoader._dict_boats[pTexturePath] = pAnimationData;
    }
    return pAnimationData;
  }

  private static void createBoatAnimationArray(
    AnimationDataBoat pAnimationData,
    Dictionary<string, Sprite> pDict,
    string pID,
    float pTimeBetween = 0.2f)
  {
    int key = int.Parse(pID.Split('@', StringSplitOptions.None)[0]);
    ActorAnimation actorAnimation = new ActorAnimation();
    actorAnimation.frames = new Sprite[2];
    actorAnimation.frames[0] = pDict[$"{key.ToString()}@{0.ToString()}"];
    actorAnimation.frames[1] = pDict[$"{key.ToString()}@{1.ToString()}"];
    pAnimationData.dict.Add(key, actorAnimation);
  }

  public static AnimationContainerUnit getAnimationContainer(
    string pTexturePath,
    ActorAsset pAsset,
    SubspeciesTrait pEggAsset = null,
    SubspeciesTrait pMutationSkinAsset = null)
  {
    AnimationContainerUnit animationContainer;
    if (!ActorAnimationLoader._dict_units.TryGetValue(pTexturePath, out animationContainer))
      animationContainer = ActorAnimationLoader.createAnimationContainer(pTexturePath, pAsset, pEggAsset, pMutationSkinAsset);
    return animationContainer;
  }

  private static AnimationContainerUnit createAnimationContainer(
    string pTexturePath,
    ActorAsset pAsset,
    SubspeciesTrait pEggAsset,
    SubspeciesTrait pMutationSkinAsset = null)
  {
    AnimationContainerUnit pAnimContainer = new AnimationContainerUnit(pTexturePath);
    ActorAnimationLoader._dict_units.Add(pTexturePath, pAnimContainer);
    string[] animationWalk;
    string[] animationSwim;
    string[] animationIdle;
    if (pTexturePath.Contains("eggs/"))
    {
      animationWalk = pEggAsset.animation_walk;
      animationSwim = pEggAsset.animation_swim;
      animationIdle = pEggAsset.animation_idle;
    }
    else if (pTexturePath.Contains("species/mutations"))
    {
      animationWalk = pMutationSkinAsset.animation_walk;
      animationSwim = pMutationSkinAsset.animation_swim;
      animationIdle = pMutationSkinAsset.animation_idle;
    }
    else
    {
      animationWalk = pAsset.animation_walk;
      animationSwim = pAsset.animation_swim;
      animationIdle = pAsset.animation_idle;
    }
    ActorAnimationLoader.generateFrameData(pTexturePath, pAnimContainer, pAnimContainer.sprites, animationSwim);
    ActorAnimationLoader.generateFrameData(pTexturePath, pAnimContainer, pAnimContainer.sprites, animationWalk);
    ActorAnimationLoader.generateFrameData(pTexturePath, pAnimContainer, pAnimContainer.sprites, animationIdle);
    if (animationSwim != null && animationSwim.Length != 0)
    {
      pAnimContainer.swimming = ActorAnimationLoader.createAnim(0, pAnimContainer.sprites, animationSwim);
      if (pAnimContainer.swimming != null)
        pAnimContainer.has_swimming = true;
    }
    if (animationWalk != null && animationWalk.Length != 0)
    {
      pAnimContainer.walking = ActorAnimationLoader.createAnim(1, pAnimContainer.sprites, animationWalk);
      if (pAnimContainer.walking != null)
        pAnimContainer.has_walking = true;
    }
    if (animationIdle != null && animationIdle.Length != 0)
    {
      pAnimContainer.idle = ActorAnimationLoader.createAnim(2, pAnimContainer.sprites, animationIdle);
      if (pAnimContainer.idle != null)
        pAnimContainer.has_idle = true;
    }
    if (pTexturePath.Contains("/child"))
      pAnimContainer.child = true;
    ActorTextureSubAsset actorTextureSubAsset = pMutationSkinAsset == null || !pMutationSkinAsset.is_mutation_skin ? pAsset.texture_asset : pMutationSkinAsset.texture_asset;
    if (actorTextureSubAsset.texture_heads != string.Empty)
      pAnimContainer.heads = SpriteTextureLoader.getSpriteList(actorTextureSubAsset.texture_heads);
    if (actorTextureSubAsset.texture_heads_male != string.Empty)
      pAnimContainer.heads_male = SpriteTextureLoader.getSpriteList(actorTextureSubAsset.texture_heads_male);
    if (actorTextureSubAsset.texture_heads_female != string.Empty)
      pAnimContainer.heads_female = SpriteTextureLoader.getSpriteList(actorTextureSubAsset.texture_heads_female);
    if (pAnimContainer.heads == null || pAnimContainer.heads.Length == 0)
      pAnimContainer.heads = pAnimContainer.heads_male;
    if (actorTextureSubAsset.render_heads_for_children)
      pAnimContainer.render_heads_for_children = true;
    return pAnimContainer;
  }

  private static void generateFrameData(
    string pFrameString,
    AnimationContainerUnit pAnimContainer,
    Dictionary<string, Sprite> pFrames,
    string[] pStringIDs)
  {
    if (string.IsNullOrEmpty(pFrameString) || pStringIDs == null)
      return;
    foreach (string pStringId in pStringIDs)
    {
      if (!pAnimContainer.dict_frame_data.ContainsKey(pStringId) && pFrames.ContainsKey(pStringId))
      {
        AnimationFrameData animationFrameData1 = new AnimationFrameData();
        animationFrameData1.id = pStringId;
        animationFrameData1.sheet_path = pFrameString;
        Sprite pFrame = pFrames[pStringId];
        AnimationFrameData animationFrameData2 = animationFrameData1;
        Rect rect = pFrame.rect;
        Vector2 size = ((Rect) ref rect).size;
        animationFrameData2.size_unit = size;
        string key1 = pStringId + "_head";
        Sprite sprite1;
        if (pFrames.TryGetValue(key1, out sprite1))
        {
          rect = sprite1.rect;
          double x1 = (double) ((Rect) ref rect).x;
          rect = pFrame.rect;
          double x2 = (double) ((Rect) ref rect).x;
          float num1 = (float) (x1 - x2) - pFrame.pivot.x + sprite1.pivot.x;
          rect = sprite1.rect;
          double y1 = (double) ((Rect) ref rect).y;
          rect = pFrame.rect;
          double y2 = (double) ((Rect) ref rect).y;
          float num2 = (float) (y1 - y2) - pFrame.pivot.y + sprite1.pivot.y;
          animationFrameData1.pos_head = new Vector2(num1, num2);
          rect = sprite1.rect;
          double x3 = (double) ((Rect) ref rect).x;
          rect = pFrame.rect;
          double x4 = (double) ((Rect) ref rect).x;
          float num3 = (float) (x3 - x4);
          rect = sprite1.rect;
          double y3 = (double) ((Rect) ref rect).y;
          rect = pFrame.rect;
          double y4 = (double) ((Rect) ref rect).y;
          float num4 = (float) (y3 - y4);
          animationFrameData1.pos_head_new = new Vector2(num3, num4);
          animationFrameData1.show_head = true;
        }
        string key2 = pStringId + "_item";
        Sprite sprite2;
        if (pFrames.TryGetValue(key2, out sprite2))
        {
          rect = sprite2.rect;
          double x5 = (double) ((Rect) ref rect).x;
          rect = pFrame.rect;
          double x6 = (double) ((Rect) ref rect).x;
          float num5 = (float) (x5 - x6) - pFrame.pivot.x + sprite2.pivot.x;
          rect = sprite2.rect;
          double y5 = (double) ((Rect) ref rect).y;
          rect = pFrame.rect;
          double y6 = (double) ((Rect) ref rect).y;
          float num6 = (float) (y5 - y6) - pFrame.pivot.y + sprite2.pivot.y;
          animationFrameData1.pos_item = new Vector2(num5, num6);
          animationFrameData1.show_item = true;
        }
        pAnimContainer.dict_frame_data.Add(pStringId, animationFrameData1);
      }
    }
  }

  private static ActorAnimation createAnim(
    int pID,
    Dictionary<string, Sprite> pDict,
    string[] pStringIDs)
  {
    Sprite[] array = ActorAnimationLoader.createArray(pDict, pStringIDs);
    if (array.Length == 0)
      return (ActorAnimation) null;
    return new ActorAnimation() { id = pID, frames = array };
  }

  private static Sprite[] createArray(Dictionary<string, Sprite> pDict, string[] pStringIDs)
  {
    using (ListPool<Sprite> list = new ListPool<Sprite>(pStringIDs.Length))
    {
      foreach (string pStringId in pStringIDs)
      {
        Sprite sprite;
        if (pDict.TryGetValue(pStringId, out sprite))
          list.Add(sprite);
        else
          break;
      }
      return list.ToArray<Sprite>();
    }
  }
}
