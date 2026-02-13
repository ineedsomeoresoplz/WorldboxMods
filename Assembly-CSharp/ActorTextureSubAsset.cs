// Decompiled with JetBrains decompiler
// Type: ActorTextureSubAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using UnityEngine;

#nullable disable
[Serializable]
public class ActorTextureSubAsset : ICloneable
{
  [DefaultValue("male_1")]
  public const string skin_civ_default_male = "male_1";
  [DefaultValue("female_1")]
  public const string skin_civ_default_female = "female_1";
  public static List<Sprite> all_preloaded_sprites_units = new List<Sprite>();
  [NonSerialized]
  public readonly Dictionary<string, Sprite[]> dict_mains = new Dictionary<string, Sprite[]>();
  private static Dictionary<string, Sprite> _shadow_sprites = new Dictionary<string, Sprite>();
  public string texture_path_base;
  public string texture_path_base_male;
  public string texture_path_base_female;
  public string texture_path_main;
  public string texture_path_baby;
  public string texture_path_king;
  public string texture_path_leader;
  public string texture_path_warrior;
  public string texture_path_zombie;
  public bool has_advanced_textures;
  public bool has_old_heads;
  [DefaultValue("")]
  public string texture_heads = string.Empty;
  [DefaultValue("")]
  public string texture_head_king = string.Empty;
  [DefaultValue("")]
  public string texture_head_warrior = string.Empty;
  [DefaultValue("")]
  public string texture_heads_old_male = string.Empty;
  [DefaultValue("")]
  public string texture_heads_old_female = string.Empty;
  [DefaultValue("")]
  public string texture_heads_male = string.Empty;
  [DefaultValue("")]
  public string texture_heads_female = string.Empty;
  public bool render_heads_for_children;
  public bool prevent_unconscious_rotation;
  [DefaultValue(true)]
  public bool shadow = true;
  [DefaultValue("unitShadow_5")]
  public string shadow_texture = "unitShadow_5";
  [NonSerialized]
  internal Sprite shadow_sprite;
  [NonSerialized]
  internal Vector2 shadow_size;
  [DefaultValue("unitShadow_2")]
  public string shadow_texture_egg = "unitShadow_2";
  [NonSerialized]
  internal Sprite shadow_sprite_egg;
  [NonSerialized]
  internal Vector2 shadow_size_egg;
  [DefaultValue("unitShadow_4")]
  public string shadow_texture_baby = "unitShadow_4";
  [NonSerialized]
  internal Sprite shadow_sprite_baby;
  [NonSerialized]
  internal Vector2 shadow_size_baby;
  private string _base_path;
  private static int _total;
  private static readonly Regex _regex_heads_sorter = new Regex("(\\D*)(\\d+)");

  public ActorTextureSubAsset(string pBasePath, bool pHasAdvancedTextures)
  {
    ++ActorTextureSubAsset._total;
    this.has_advanced_textures = pHasAdvancedTextures;
    this._base_path = pBasePath;
    this.texture_path_base = pBasePath;
    this.texture_path_base_male = pBasePath + "male_1";
    this.texture_path_base_female = pBasePath + "female_1";
    if (string.IsNullOrEmpty(this.texture_head_warrior))
      this.texture_head_warrior = pBasePath + "head_warrior";
    if (string.IsNullOrEmpty(this.texture_head_king))
      this.texture_head_king = pBasePath + "head_king";
    if (string.IsNullOrEmpty(this.texture_heads_old_female))
      this.texture_heads_old_female = pBasePath + "head_old_female";
    if (string.IsNullOrEmpty(this.texture_heads_old_male))
      this.texture_heads_old_male = pBasePath + "head_old_male";
    if (string.IsNullOrEmpty(this.texture_heads_male))
      this.texture_heads_male = pBasePath + "heads_male";
    if (string.IsNullOrEmpty(this.texture_heads_female))
      this.texture_heads_female = pBasePath + "heads_female";
    this.texture_path_main = pBasePath + "main";
    if (!this.hasSpriteInResources(this.texture_path_main))
      this.texture_path_main = this.texture_path_base_male;
    if (string.IsNullOrEmpty(this.texture_path_king))
      this.texture_path_king = pBasePath + "king";
    if (string.IsNullOrEmpty(this.texture_path_leader))
      this.texture_path_leader = pBasePath + "leader";
    if (string.IsNullOrEmpty(this.texture_path_warrior))
      this.texture_path_warrior = pBasePath + "warrior_1";
    if (string.IsNullOrEmpty(this.texture_path_zombie))
      this.texture_path_zombie = pBasePath + "zombie";
    if (string.IsNullOrEmpty(this.texture_heads))
    {
      this.texture_heads = pBasePath + "heads";
      if (!this.hasSpriteInResources(this.texture_path_main))
        this.texture_path_main = this.texture_heads_male;
    }
    if (this.hasSpriteInResources(this.texture_heads_old_male))
      this.has_old_heads = true;
    this.texture_path_baby = pBasePath + "child";
  }

  private void logAssetError(string pMessage, string pPath)
  {
    BaseAssetLibrary.logAssetError(pMessage, pPath);
  }

  public string getUnitTexturePath(Actor pActor)
  {
    Subspecies subspecies = pActor.subspecies;
    if (pActor.isEgg())
      return subspecies.egg_sprite_path;
    if (pActor.isBaby())
      return this.texture_path_baby;
    if (pActor.hasSubspecies() && pActor.subspecies.has_mutation_reskin && pActor.asset.unit_zombie)
      return this.texture_path_zombie;
    string texturePathMain = this.texture_path_main;
    ProfessionAsset professionAsset = pActor.profession_asset;
    if (professionAsset == null || professionAsset.profession_id == UnitProfession.Nothing || !this.has_advanced_textures)
      return texturePathMain;
    string unitTexturePath;
    switch (professionAsset.profession_id)
    {
      case UnitProfession.King:
        unitTexturePath = this.texture_path_king;
        break;
      case UnitProfession.Leader:
        unitTexturePath = this.texture_path_leader;
        break;
      case UnitProfession.Warrior:
        string pValue = this.texture_path_warrior;
        if (pActor.hasSubspecies())
          pValue = pActor.subspecies.getSkinWarrior();
        if (subspecies.has_mutation_reskin)
        {
          List<string> skinWarrior = subspecies.mutation_skin_asset.skin_warrior;
          int index = Toolbox.loopIndex(pActor.asset.skin_warrior.IndexOf<string>(pValue), skinWarrior.Count);
          pValue = skinWarrior[index];
        }
        unitTexturePath = this.texture_path_base + pValue;
        break;
      default:
        unitTexturePath = this.getTextureSkinBasedOnSex(pActor);
        break;
    }
    return unitTexturePath;
  }

  private string getTextureSkinBasedOnSex(Actor pActor)
  {
    return !pActor.isSexFemale() ? (!pActor.hasSubspecies() ? this.texture_path_base_male : this.texture_path_base + pActor.subspecies.getSkinMale()) : (!pActor.hasSubspecies() ? this.texture_path_base_female : this.texture_path_base + pActor.subspecies.getSkinFemale());
  }

  public string getUnitTexturePathForUI(ActorAsset pAsset)
  {
    string texturePathMain = this.texture_path_main;
    return !pAsset.civ ? texturePathMain : (AssetsDebugManager.actors_sex != ActorSex.Male ? this.texture_path_base + pAsset.skin_citizen_female[0] : this.texture_path_base + pAsset.skin_citizen_male[0]);
  }

  private bool hasSpriteInResources(string pPath)
  {
    Sprite[] spriteList = SpriteTextureLoader.getSpriteList(pPath, true);
    if (spriteList == null)
      return false;
    ActorTextureSubAsset.all_preloaded_sprites_units.AddRange((IEnumerable<Sprite>) spriteList);
    return spriteList.Length != 0;
  }

  public object Clone()
  {
    return (object) new ActorTextureSubAsset(this._base_path, this.has_advanced_textures);
  }

  public void preloadSprites(
    bool pCivTextures,
    bool pHasBabyForm,
    IAnimationFrames pAnimationAsset)
  {
    if (!pCivTextures)
      this.preloadSpritePath("texture_path_main", this.texture_path_main, pAnimationAsset);
    if (pHasBabyForm)
      this.preloadSpritePath("texture_path_baby", this.texture_path_baby, pAnimationAsset);
    if (this.has_advanced_textures)
    {
      this.preloadSpritePath("texture_path_base_male", this.texture_path_base_male, pAnimationAsset);
      this.preloadSpritePath("texture_path_base_female", this.texture_path_base_female, pAnimationAsset);
      this.preloadSpritePath("texture_path_king", this.texture_path_king, pAnimationAsset);
      this.preloadSpritePath("texture_path_leader", this.texture_path_leader, pAnimationAsset);
      this.preloadSpritePath("texture_path_warrior", this.texture_path_warrior, pAnimationAsset);
      this.preloadSpritePath("texture_head_king", this.texture_head_king, pAnimationAsset, true, pSpecialHead: true);
      this.preloadSpritePath("texture_head_warrior", this.texture_head_warrior, pAnimationAsset, true, pSpecialHead: true);
      this.preloadSpritePath("texture_heads_male", this.texture_heads_male, pAnimationAsset, true);
      this.preloadSpritePath("texture_heads_female", this.texture_heads_female, pAnimationAsset, true);
    }
    else
      this.preloadSpritePath("texture_heads", this.texture_heads, pAnimationAsset, true, false);
  }

  private bool preloadSpritePath(
    string pType,
    string pPath,
    IAnimationFrames pAnimationAsset,
    bool pLoadHeads = false,
    bool pThrowError = true,
    bool pSpecialHead = false)
  {
    if (string.IsNullOrEmpty(pPath) || this.dict_mains.ContainsKey(pPath))
      return false;
    Sprite[] spriteList = SpriteTextureLoader.getSpriteList(pPath);
    if (!pLoadHeads)
      this.dict_mains.Add(pPath, spriteList);
    ActorTextureSubAsset.all_preloaded_sprites_units.AddRange((IEnumerable<Sprite>) spriteList);
    if (spriteList.Length == 0)
    {
      if (pThrowError)
        this.logAssetError($"ActorAssetLibrary: <e>{pType}</e> doesn't exist for <e>{((Asset) pAnimationAsset).id}</e> at ", pPath);
      return false;
    }
    if (pLoadHeads)
    {
      if (this.has_advanced_textures)
      {
        for (int pHeadIndex = 0; pHeadIndex < spriteList.Length; ++pHeadIndex)
        {
          if (pSpecialHead)
            ActorAnimationLoader.getHeadSpecial(pPath);
          else
            ActorAnimationLoader.getHead(pPath, pHeadIndex);
        }
      }
      this.checkHeads(spriteList, pPath, pAnimationAsset);
    }
    else
      this.checkAnimations(spriteList, pPath, (Asset) pAnimationAsset, pAnimationAsset);
    return true;
  }

  internal void loadShadow()
  {
    Sprite shadowSprite1 = this.getShadowSprite(this.shadow_texture);
    this.shadow_sprite = shadowSprite1;
    Rect rect1 = shadowSprite1.rect;
    this.shadow_size = ((Rect) ref rect1).size;
    Sprite shadowSprite2 = this.getShadowSprite(this.shadow_texture_egg);
    this.shadow_sprite_egg = shadowSprite2;
    Rect rect2 = shadowSprite2.rect;
    this.shadow_size_egg = ((Rect) ref rect2).size;
    Sprite shadowSprite3 = this.getShadowSprite(this.shadow_texture_baby);
    this.shadow_sprite_baby = shadowSprite3;
    Rect rect3 = shadowSprite3.rect;
    this.shadow_size_baby = ((Rect) ref rect3).size;
  }

  private Sprite getShadowSprite(string pTexturePath)
  {
    if (!ActorTextureSubAsset._shadow_sprites.ContainsKey(pTexturePath))
    {
      Sprite sprite = SpriteTextureLoader.getSprite("shadows/" + pTexturePath);
      if (Object.op_Equality((Object) sprite, (Object) null))
        Debug.LogError((object) ("Shadow not found " + pTexturePath));
      ActorTextureSubAsset._shadow_sprites.Add(pTexturePath, sprite);
    }
    Sprite shadowSprite = ActorTextureSubAsset._shadow_sprites[pTexturePath];
    return DynamicSprites.getShadowUnit(shadowSprite, shadowSprite.GetHashCode());
  }

  private void checkHeads(Sprite[] pSprites, string pPath, IAnimationFrames pAnimationAsset)
  {
    // ISSUE: unable to decompile the method.
  }

  private int headSorter(string x, string y)
  {
    Match match1 = ActorTextureSubAsset._regex_heads_sorter.Match(x);
    Match match2 = ActorTextureSubAsset._regex_heads_sorter.Match(y);
    int result1;
    int result2;
    return match1.Success && match2.Success && match1.Groups[1].Value == match2.Groups[1].Value && int.TryParse(match1.Groups[2].Value, out result1) && int.TryParse(match2.Groups[2].Value, out result2) ? result1.CompareTo(result2) : x.CompareTo(y);
  }

  private void checkAnimations(
    Sprite[] pSprites,
    string pPath,
    Asset pAsset,
    IAnimationFrames pAnimationFrames)
  {
    using (ListPool<string> listPool = new ListPool<string>())
    {
      foreach (Sprite pSprite in pSprites)
        listPool.Add(((Object) pSprite).name);
      using (ListPool<string> values = new ListPool<string>())
      {
        string[] walk = pAnimationFrames.getWalk();
        if ((walk != null ? (walk.Length != 0 ? 1 : 0) : 0) != 0)
        {
          values.Clear();
          bool flag = false;
          foreach (string str in pAnimationFrames.getWalk())
          {
            if (!listPool.Contains(str))
              values.Add(str);
            else
              flag = true;
          }
          if (!flag)
            this.logAssetError($"ActorAssetLibrary: <e>{pAsset.id}</e> missing all animation_walk sprites: <e>{string.Join(", ", (IEnumerable<string>) values)}</e> at ", pPath);
        }
        string[] swim = pAnimationFrames.getSwim();
        if ((swim != null ? (swim.Length != 0 ? 1 : 0) : 0) != 0)
        {
          values.Clear();
          bool flag = false;
          foreach (string str in pAnimationFrames.getSwim())
          {
            if (!listPool.Contains(str))
              values.Add(str);
            else
              flag = true;
          }
          if (!flag)
            this.logAssetError($"ActorAssetLibrary: <e>{pAsset.id}</e> missing all animation_swim sprites: <e>{string.Join(", ", (IEnumerable<string>) values)}</e> at ", pPath);
        }
        string[] idle = pAnimationFrames.getIdle();
        if ((idle != null ? (idle.Length != 0 ? 1 : 0) : 0) == 0)
          return;
        values.Clear();
        bool flag1 = false;
        foreach (string str in pAnimationFrames.getIdle())
        {
          if (!listPool.Contains(str))
            values.Add(str);
          else
            flag1 = true;
        }
        if (flag1)
          return;
        this.logAssetError($"ActorAssetLibrary: <e>{pAsset.id}</e> missing all animation_idle sprites: <e>{string.Join(", ", (IEnumerable<string>) values)}</e> at ", pPath);
      }
    }
  }

  public static int getTotal() => ActorTextureSubAsset._total;
}
