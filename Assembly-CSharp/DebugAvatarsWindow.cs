// Decompiled with JetBrains decompiler
// Type: DebugAvatarsWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class DebugAvatarsWindow : MonoBehaviour
{
  private static readonly bool _test_mutations = false;
  private static readonly bool _test_eggs = true;
  private static readonly bool _test_hand_items = false;
  private static readonly bool _test_statuses = false;
  [SerializeField]
  private Transform _avatars_parent;
  [SerializeField]
  private UnitAvatarLoader _avatar_prefab;
  [SerializeField]
  private Image _autotest_button_icon;
  [SerializeField]
  private Sprite _sprite_play;
  [SerializeField]
  private Sprite _sprite_pause;
  private ObjectPoolGenericMono<UnitAvatarLoader> _avatars;
  private List<SubspeciesTrait> _pool_mutations = new List<SubspeciesTrait>();
  private List<SubspeciesTrait> _pool_eggs = new List<SubspeciesTrait>();
  private List<PhenotypeAsset> _pool_phenotype = new List<PhenotypeAsset>();
  private List<AvatarCombineHandItem> _pool_hand_renderers = new List<AvatarCombineHandItem>();
  private List<StatusAsset> _pool_statuses = new List<StatusAsset>();
  private AvatarsCombineDataContainer _combine_data = new AvatarsCombineDataContainer();
  private HashSet<string> _statuses = new HashSet<string>();
  private HashSet<long> _check_collisions = new HashSet<long>();
  private bool _autotest_state;
  private Coroutine _autotest_routine;

  private void Awake() => this.init();

  private void init()
  {
    this._avatars = new ObjectPoolGenericMono<UnitAvatarLoader>(this._avatar_prefab, this._avatars_parent);
    this.preparePools();
  }

  private void OnEnable() => this.showAvatars();

  private void OnDisable() => this.clear();

  private void clear() => this._avatars.clear();

  private void showAvatars()
  {
    foreach (ActorAsset pAsset in AssetManager.actor_library.list)
    {
      if (!pAsset.has_override_sprite && pAsset.has_sprite_renderer)
      {
        SubspeciesTrait randomMutation = this.getRandomMutation();
        bool randomIsAdult = this.getRandomIsAdult();
        ActorSex randomSex = this.getRandomSex();
        ColorAsset random = AssetManager.kingdom_colors_library.list.GetRandom<ColorAsset>();
        bool randomIsUnconscious = this.getRandomIsUnconscious();
        bool pIsLying = randomIsUnconscious || this.getRandomIsLying();
        bool randomIsHovering = this.getRandomIsHovering();
        bool pIsTouchingLiquid = this.getRandomIsTouchingLiquid() && !randomIsHovering;
        bool randomIsImmovable = this.getRandomIsImmovable();
        AvatarCombineHandItem randomItemPath = this.getRandomItemPath();
        bool pStopIdleAnimation;
        List<string> randomStatuses = this.getRandomStatuses(out pStopIdleAnimation);
        PhenotypeAsset randomPhenotype = this.getRandomPhenotype();
        int randomPhenotypeShade = Actor.getRandomPhenotypeShade();
        SubspeciesTrait randomEgg = this.getRandomEgg();
        bool pIsEgg = !randomIsAdult && randomEgg != null;
        ActorTextureSubAsset textureAsset;
        if (randomMutation != null)
        {
          textureAsset = randomMutation.texture_asset;
          BaseStats baseStatsMeta = randomMutation.base_stats_meta;
          if (!baseStatsMeta.isEmpty() && baseStatsMeta.hasTag("always_idle_animation"))
            pStopIdleAnimation = false;
        }
        else
          textureAsset = pAsset.texture_asset;
        DynamicActorSpriteCreatorUI.getContainerForUI(pAsset, randomIsAdult, textureAsset, randomMutation, pIsEgg, randomEgg);
        ActorAvatarData pData = new ActorAvatarData();
        pData.setData(pAsset, randomMutation, randomSex, (long) Randy.randomInt(0, int.MaxValue), -1, (Sprite) null, randomPhenotype.phenotype_index, randomPhenotypeShade, random, pIsEgg, false, false, false, randomEgg, randomIsAdult, pIsLying, pIsTouchingLiquid, false, randomIsHovering, randomIsImmovable, randomIsUnconscious, pStopIdleAnimation, randomItemPath?.hand_renderer, 1, (IEnumerable<string>) randomStatuses, (IReadOnlyDictionary<string, Status>) null);
        this._avatars.getNext().load(pData);
      }
    }
  }

  private void preparePools()
  {
    foreach (SubspeciesTrait subspeciesTrait in AssetManager.subspecies_traits.list)
    {
      if (subspeciesTrait.is_mutation_skin)
        this._pool_mutations.Add(subspeciesTrait);
      if (subspeciesTrait.phenotype_egg)
        this._pool_eggs.Add(subspeciesTrait);
      if (subspeciesTrait.phenotype_skin)
        this._pool_phenotype.Add(AssetManager.phenotype_library.get(subspeciesTrait.id_phenotype));
    }
    foreach (IHandRenderer pHandRenderer in AssetManager.items.pot_weapon_assets_all)
      this._pool_hand_renderers.Add(new AvatarCombineHandItem(pHandRenderer));
    foreach (IHandRenderer pHandRenderer in AssetManager.resources.list)
      this._pool_hand_renderers.Add(new AvatarCombineHandItem(pHandRenderer));
    foreach (IHandRenderer pHandRenderer in AssetManager.unit_hand_tools.list)
      this._pool_hand_renderers.Add(new AvatarCombineHandItem(pHandRenderer));
    foreach (StatusAsset statusAsset in AssetManager.status.list)
    {
      if (statusAsset.need_visual_render)
        this._pool_statuses.Add(statusAsset);
    }
  }

  private SubspeciesTrait getRandomMutation()
  {
    return Randy.randomChance(0.75f) ? (SubspeciesTrait) null : this._pool_mutations.GetRandom<SubspeciesTrait>();
  }

  private SubspeciesTrait getRandomEgg()
  {
    return Randy.randomChance(0.9f) ? (SubspeciesTrait) null : this._pool_eggs.GetRandom<SubspeciesTrait>();
  }

  private PhenotypeAsset getRandomPhenotype() => this._pool_phenotype.GetRandom<PhenotypeAsset>();

  private ActorSex getRandomSex() => Randy.randomChance(0.5f) ? ActorSex.Male : ActorSex.Female;

  private bool getRandomIsAdult() => Randy.randomBool();

  private bool getRandomIsLying() => Randy.randomChance(0.2f);

  private bool getRandomIsTouchingLiquid() => Randy.randomBool();

  private bool getRandomIsHovering() => Randy.randomChance(0.2f);

  private bool getRandomIsImmovable() => Randy.randomChance(0.2f);

  private bool getRandomIsUnconscious() => Randy.randomChance(0.2f);

  private AvatarCombineHandItem getRandomItemPath()
  {
    return Randy.randomChance(0.4f) ? (AvatarCombineHandItem) null : this._pool_hand_renderers.GetRandom<AvatarCombineHandItem>();
  }

  private List<string> getRandomStatuses(out bool pStopIdleAnimation)
  {
    pStopIdleAnimation = false;
    List<string> randomStatuses = new List<string>();
    foreach (StatusAsset statusAsset in AssetManager.status.list)
    {
      if (statusAsset.need_visual_render && !Randy.randomChance(0.95f))
      {
        if (statusAsset.base_stats.hasTag("stop_idle_animation"))
          pStopIdleAnimation = true;
        randomStatuses.Add(statusAsset.id);
      }
    }
    return randomStatuses;
  }

  public void toggleAutotest()
  {
    this._autotest_state = !this._autotest_state;
    if (this._autotest_state)
    {
      this._autotest_button_icon.sprite = this._sprite_pause;
      this._autotest_routine = this.StartCoroutine(this.autotestRoutine());
    }
    else
    {
      this._autotest_button_icon.sprite = this._sprite_play;
      this.StopCoroutine(this._autotest_routine);
    }
  }

  private T getFromPool<T>(List<T> pPool, int pGlobalIndex, string pId) where T : class
  {
    int listIndex = this._combine_data.getListIndex(pGlobalIndex, pId);
    return pPool.Count - 1 < listIndex ? default (T) : pPool[listIndex];
  }

  private bool getBool(int pGlobalIndex, string pId)
  {
    return this._combine_data.getListIndex(pGlobalIndex, pId) == 1;
  }

  private IEnumerator autotestRoutine()
  {
    this._combine_data.clear();
    this._statuses.Clear();
    this._check_collisions.Clear();
    this._combine_data.add("tAdult", 2);
    this._combine_data.add("tTouchingLiquid", 2);
    this._combine_data.add("tLying", 2);
    this._combine_data.add("tImmovable", 2);
    this._combine_data.add("tUnconscious", 2);
    this._combine_data.add("tSex", 2);
    if (DebugAvatarsWindow._test_mutations)
      this._combine_data.add("_pool_mutations", this._pool_mutations.Count);
    if (DebugAvatarsWindow._test_eggs)
      this._combine_data.add("_pool_eggs", this._pool_eggs.Count);
    if (DebugAvatarsWindow._test_hand_items)
      this._combine_data.add("_pool_hand_renderers", this._pool_hand_renderers.Count);
    if (DebugAvatarsWindow._test_statuses)
      this._combine_data.add("_pool_statuses", this._pool_statuses.Count);
    int tTotal = this._combine_data.totalCombinations();
    for (int i = 0; i < tTotal; ++i)
    {
      bool flag1 = this.getBool(i, "tAdult");
      bool pIsTouchingLiquid = this.getBool(i, "tTouchingLiquid");
      bool pIsLying = this.getBool(i, "tLying");
      bool pIsImmovable = this.getBool(i, "tImmovable");
      bool pIsUnconscious = this.getBool(i, "tUnconscious");
      ActorSex pSex = this.getBool(i, "tSex") ? ActorSex.Male : ActorSex.Female;
      bool pIsStopIdleAnimation = false;
      bool flag2 = false;
      long num1 = (long) ((flag1 ? 1 : 2) + (pIsTouchingLiquid ? 1 : 2) * 10 + (pIsLying ? 1 : 2) * 100 + (pIsImmovable ? 1 : 2) * 1000 + (pIsUnconscious ? 1 : 2) * 10000 + (pSex == ActorSex.Male ? 1 : 2) * 100000 + (pIsStopIdleAnimation ? 1 : 2) * 1000000);
      SubspeciesTrait subspeciesTrait = (SubspeciesTrait) null;
      if (DebugAvatarsWindow._test_mutations)
      {
        subspeciesTrait = this.getFromPool<SubspeciesTrait>(this._pool_mutations, i, "_pool_mutations");
        num1 += (long) (this._pool_mutations.IndexOf(subspeciesTrait) * 100000000);
        BaseStats baseStatsMeta = subspeciesTrait.base_stats_meta;
        if (!baseStatsMeta.isEmpty() && baseStatsMeta.hasTag("always_idle_animation"))
          flag2 = true;
      }
      SubspeciesTrait pEggAsset = (SubspeciesTrait) null;
      if (subspeciesTrait == null && DebugAvatarsWindow._test_eggs)
      {
        pEggAsset = this.getFromPool<SubspeciesTrait>(this._pool_eggs, i, "_pool_eggs");
        num1 += (long) this._pool_eggs.IndexOf(pEggAsset) * 10000000000L;
      }
      bool pIsEgg = pEggAsset != null;
      long num2;
      IHandRenderer pItemPath;
      if (!pIsEgg && DebugAvatarsWindow._test_hand_items)
      {
        AvatarCombineHandItem fromPool = this.getFromPool<AvatarCombineHandItem>(this._pool_hand_renderers, i, "_pool_hand_renderers");
        num2 = num1 + (long) this._pool_hand_renderers.IndexOf(fromPool) * 10000000000000L;
        pItemPath = fromPool.hand_renderer;
      }
      else
      {
        pItemPath = (IHandRenderer) null;
        num2 = num1 + (long) this._pool_hand_renderers.Count * 10000000000000L;
      }
      StatusAsset statusAsset = (StatusAsset) null;
      if (DebugAvatarsWindow._test_statuses)
      {
        statusAsset = this.getFromPool<StatusAsset>(this._pool_statuses, i, "_pool_statuses");
        num2 += (long) this._pool_statuses.IndexOf(statusAsset) * 10000000000000000L;
      }
      int pActorHash = 1;
      foreach (UnitAvatarLoader unitAvatarLoader in (IEnumerable<UnitAvatarLoader>) this._avatars.getListTotal())
      {
        this._statuses.Clear();
        StatusAsset random1 = !DebugAvatarsWindow._test_statuses || !Randy.randomBool() ? (StatusAsset) null : this._pool_statuses.GetRandom<StatusAsset>();
        StatusAsset random2 = !DebugAvatarsWindow._test_statuses || !Randy.randomBool() ? (StatusAsset) null : this._pool_statuses.GetRandom<StatusAsset>();
        if (statusAsset != null)
        {
          this._statuses.Add(statusAsset.id);
          if (statusAsset.base_stats.hasTag("stop_idle_animation"))
            pIsStopIdleAnimation = true;
        }
        if (random1 != null)
        {
          this._statuses.Add(random1.id);
          if (random1.base_stats.hasTag("stop_idle_animation"))
            pIsStopIdleAnimation = true;
        }
        if (random2 != null)
        {
          this._statuses.Add(random2.id);
          if (random2.base_stats.hasTag("stop_idle_animation"))
            pIsStopIdleAnimation = true;
        }
        ++pActorHash;
        ActorAvatarData data = unitAvatarLoader.getData();
        ActorAsset asset = data.asset;
        ActorTextureSubAsset pTextureAsset = subspeciesTrait == null ? asset.texture_asset : subspeciesTrait.texture_asset;
        DynamicActorSpriteCreatorUI.getContainerForUI(asset, flag1, pTextureAsset, subspeciesTrait, pIsEgg, pEggAsset);
        if (flag2)
          pIsStopIdleAnimation = false;
        ActorAvatarData pData = new ActorAvatarData();
        pData.setData(data.asset, subspeciesTrait, pSex, (long) Randy.randomInt(0, int.MaxValue), -1, (Sprite) null, data.phenotype_index, data.phenotype_skin_shade, data.kingdom_color, pIsEgg, false, false, false, pEggAsset, flag1, pIsLying, pIsTouchingLiquid, false, data.is_hovering, pIsImmovable, pIsUnconscious, pIsStopIdleAnimation, pItemPath, pActorHash, (IEnumerable<string>) this._statuses, (IReadOnlyDictionary<string, Status>) null);
        unitAvatarLoader.load(pData);
      }
      this._check_collisions.Add(num2);
      Debug.Log((object) $"tested: {i + 1}/{tTotal}, hashset: {this._check_collisions.Count}/{tTotal} adult: {flag1}, liquid: {pIsTouchingLiquid}, lying: {pIsLying}, immovable: {pIsImmovable}, uncon: {pIsUnconscious}, sex: {pSex}, mut: {subspeciesTrait?.id ?? "null"}, egg: {pEggAsset?.id ?? "null"}, item: {pItemPath}, status: {statusAsset?.id ?? "null"}");
      yield return (object) null;
    }
  }
}
