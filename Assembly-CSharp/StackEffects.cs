// Decompiled with JetBrains decompiler
// Type: StackEffects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class StackEffects : BaseMapObject
{
  public static double last_thunder_timestamp;
  public BaseEffectController prefabController;
  private Dictionary<string, BaseEffectController> dictionary;
  internal List<BaseEffectController> list;
  public List<LightBlobData> light_blobs = new List<LightBlobData>();
  public List<PlotIconData> plot_removals = new List<PlotIconData>();
  public List<ActorDamageEffectData> actor_effect_hit = new List<ActorDamageEffectData>();
  public List<ActorHighlightEffectData> actor_effect_highlight = new List<ActorHighlightEffectData>();
  public BaseEffectController controller_slash_effects;
  public BaseEffectController controller_tile_effects;

  internal override void create()
  {
    base.create();
    this.dictionary = new Dictionary<string, BaseEffectController>();
    this.list = new List<BaseEffectController>();
    this.checkInit();
    this.controller_slash_effects = this.get("fx_slash");
    this.controller_tile_effects = this.get("fx_tile_effect");
  }

  private void checkInit()
  {
    foreach (EffectAsset pAsset in AssetManager.effects_library.list)
    {
      if (!this.dictionary.ContainsKey(pAsset.id))
        this.add(pAsset);
    }
  }

  internal int countActive()
  {
    int num = 0;
    for (int index = 0; index < this.list.Count; ++index)
    {
      BaseEffectController effectController = this.list[index];
      num += effectController.getActiveIndex();
    }
    return num;
  }

  internal bool isLocked() => this.get("fx_spawn_big").getActiveIndex() > 0;

  internal BaseEffectController get(string pID) => this.dictionary[pID];

  private BaseEffectController add(EffectAsset pAsset)
  {
    GameObject gameObject = Object.Instantiate<GameObject>((GameObject) Resources.Load(!pAsset.use_basic_prefab ? pAsset.prefab_id : "effects/prefabs/PrefabEffectBasic", typeof (GameObject)), ((Component) this).transform);
    ((Object) gameObject.transform).name = "[base] " + pAsset.id;
    gameObject.gameObject.SetActive(false);
    if (pAsset.use_basic_prefab || pAsset.load_texture)
    {
      SpriteAnimation component = gameObject.GetComponent<SpriteAnimation>();
      component.timeBetweenFrames = pAsset.time_between_frames;
      component.frames = SpriteTextureLoader.getSpriteList(pAsset.sprite_path);
      if (component.frames == null || component.frames.Length == 0)
        Debug.LogError((object) $"NO SPRITES FOR EFFECT {pAsset.id} {pAsset.sprite_path}");
      ((Renderer) component.spriteRenderer).sortingLayerName = pAsset.sorting_layer_id;
    }
    BaseEffectController effectController = Object.Instantiate<BaseEffectController>(this.prefabController, ((Component) this).transform, true);
    effectController.create();
    effectController.asset = pAsset;
    ((Object) ((Component) effectController).transform).name = "[controller] " + pAsset.id;
    effectController.prefab = gameObject.transform;
    effectController.setLimits(pAsset.limit, pAsset.limit_unload);
    this.dictionary.Add(pAsset.id, effectController);
    this.list.Add(effectController);
    effectController.addNewObject(gameObject.GetComponent<BaseEffect>());
    return effectController;
  }

  internal void clear()
  {
    for (int index = 0; index < this.list.Count; ++index)
      this.list[index].clear();
    StackEffects.last_thunder_timestamp = 0.0;
    this.light_blobs.Clear();
    this.plot_removals.Clear();
    this.actor_effect_hit.Clear();
    this.actor_effect_highlight.Clear();
  }

  public override void update(float pElapsed)
  {
    if (AssetManager.effects_library.list.Count > this.list.Count)
      this.checkInit();
    Bench.bench("stack_effects", "game_total");
    for (int index = 0; index < this.list.Count; ++index)
      this.list[index].update(pElapsed);
    Bench.benchEnd("stack_effects", "game_total");
  }
}
