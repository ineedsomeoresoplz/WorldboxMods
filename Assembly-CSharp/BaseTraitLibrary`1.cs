// Decompiled with JetBrains decompiler
// Type: BaseTraitLibrary`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class BaseTraitLibrary<T> : BaseLibraryWithUnlockables<T> where T : BaseTrait<T>
{
  protected List<T> _pot_allowed_to_be_given_randomly = new List<T>();

  public override void post_init()
  {
    base.post_init();
    this.list.Sort((Comparison<T>) ((pT1, pT2) => StringComparer.Ordinal.Compare(pT2.id, pT1.id)));
    this.autoSetRarity();
    this.checkIcons();
  }

  protected virtual void autoSetRarity()
  {
    foreach (T obj in this.list)
    {
      if (obj.unlocked_with_achievement)
      {
        obj.rarity = Rarity.R3_Legendary;
      }
      else
      {
        int num1 = obj.action_death != null || obj.action_special_effect != null || obj.action_get_hit != null || obj.action_birth != null || obj.action_attack_target != null || obj.action_on_augmentation_add != null || obj.action_on_augmentation_remove != null ? 1 : (obj.action_on_augmentation_load != null ? 1 : 0);
        bool flag1 = obj.decision_ids != null;
        bool flag2 = obj.spells_ids != null;
        bool flag3 = obj.combat_actions_ids != null;
        bool flag4 = obj.base_stats.hasTags();
        bool flag5 = !string.IsNullOrEmpty(obj.plot_id);
        int num2 = 0;
        if (num1 != 0)
          ++num2;
        if (flag1)
          ++num2;
        if (flag2)
          ++num2;
        if (flag3)
          ++num2;
        if (flag4)
          ++num2;
        if (flag5)
          ++num2;
        if (num2 > 0)
        {
          if (num2 == 1)
            obj.rarity = Rarity.R1_Rare;
          else
            obj.rarity = Rarity.R2_Epic;
          obj.needs_to_be_explored = true;
        }
        else if (obj.rarity == Rarity.R0_Normal)
          obj.needs_to_be_explored = false;
      }
    }
  }

  public override void linkAssets()
  {
    base.linkAssets();
    this.fillOppositeHashsetsWithAssets();
    this.linkDecisions();
    this.linkCombatActions();
    this.linkSpells();
    this.linkActorAssets();
    foreach (T pObject in this.list)
    {
      if (pObject.spawn_random_trait_allowed)
        this._pot_allowed_to_be_given_randomly.AddTimes<T>(pObject.spawn_random_rate, pObject);
    }
  }

  private void linkCombatActions()
  {
    foreach (T obj in this.list)
      obj.linkCombatActions();
  }

  private void linkSpells()
  {
    foreach (T obj in this.list)
      obj.linkSpells();
  }

  private void linkDecisions()
  {
    foreach (T obj in this.list)
    {
      if (obj.decision_ids != null)
      {
        obj.decisions_assets = new DecisionAsset[obj.decision_ids.Count];
        for (int index = 0; index < obj.decision_ids.Count; ++index)
        {
          string decisionId = obj.decision_ids[index];
          DecisionAsset decisionAsset = AssetManager.decisions_library.get(decisionId);
          obj.decisions_assets[index] = decisionAsset;
        }
      }
    }
  }

  private void linkActorAssets()
  {
    foreach (ActorAsset pAsset in AssetManager.actor_library.list)
    {
      List<string> defaultTraitsForMeta = this.getDefaultTraitsForMeta(pAsset);
      if (defaultTraitsForMeta != null)
      {
        foreach (string pID in defaultTraitsForMeta)
        {
          T obj = this.get(pID);
          if (obj.default_for_actor_assets == null)
            obj.default_for_actor_assets = new List<ActorAsset>();
          obj.default_for_actor_assets.Add(pAsset);
        }
      }
    }
  }

  public override void editorDiagnostic()
  {
    this.checkOppositeErrors();
    foreach (T obj in this.list)
    {
      if (string.IsNullOrEmpty(obj.group_id))
        BaseAssetLibrary.logAssetError("Group id not assigned", obj.id);
      if (!obj.special_icon_logic && Object.op_Equality((Object) SpriteTextureLoader.getSprite(obj.path_icon), (Object) null))
        BaseAssetLibrary.logAssetError("Missing icon file", obj.path_icon);
    }
    base.editorDiagnostic();
  }

  public override void editorDiagnosticLocales()
  {
    foreach (T pAsset in this.list)
    {
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
      this.checkLocale((Asset) pAsset, pAsset.getDescriptionID());
      this.checkLocale((Asset) pAsset, pAsset.getDescriptionID2());
    }
  }

  private void checkOppositeErrors()
  {
    foreach (T obj1 in this.list)
    {
      HashSet<T> oppositeTraits1 = obj1.opposite_traits;
      if (oppositeTraits1 != null)
      {
        foreach (T obj2 in oppositeTraits1)
        {
          HashSet<T> oppositeTraits2 = obj2.opposite_traits;
          if (oppositeTraits2 == null || !oppositeTraits2.Contains(obj1))
            this.logErrorOpposites(obj1.id, obj2.id);
        }
      }
    }
  }

  private void fillOppositeHashsetsWithAssets()
  {
    foreach (T obj1 in this.list)
    {
      if (obj1.opposite_list != null && obj1.opposite_list.Count > 0)
      {
        obj1.opposite_traits = new HashSet<T>(obj1.opposite_list.Count);
        foreach (string opposite in obj1.opposite_list)
        {
          T obj2 = this.get(opposite);
          obj1.opposite_traits.Add(obj2);
        }
      }
    }
    foreach (T obj3 in this.list)
    {
      if (obj3.traits_to_remove_ids != null)
      {
        int length = obj3.traits_to_remove_ids.Length;
        obj3.traits_to_remove = new T[length];
        for (int index = 0; index < length; ++index)
        {
          T obj4 = this.get(obj3.traits_to_remove_ids[index]);
          obj3.traits_to_remove[index] = obj4;
        }
      }
    }
  }

  private void checkIcons()
  {
    foreach (T obj in this.list)
    {
      if (string.IsNullOrEmpty(obj.path_icon))
        obj.path_icon = this.icon_path + obj.getLocaleID();
    }
  }

  public override T add(T pAsset)
  {
    T obj = base.add(pAsset);
    if (obj.base_stats == null)
      obj.base_stats = new BaseStats();
    if (obj.base_stats_meta == null)
      obj.base_stats_meta = new BaseStats();
    return obj;
  }

  public string addToGameplayReportShort(string pWhatFor)
  {
    string str1 = $"{string.Empty}{pWhatFor}\n";
    foreach (T obj in this.list)
    {
      string id = obj.id;
      if (!(id == "Phenotype"))
      {
        string translatedDescription = obj.getTranslatedDescription();
        string str2 = "\n" + id;
        if (!string.IsNullOrEmpty(translatedDescription))
          str2 = $"{str2}: {translatedDescription}";
        str1 += str2;
      }
    }
    return str1 + "\n\n";
  }

  public string addToGameplayReport(string pWhatFor)
  {
    string str1 = $"{string.Empty}{pWhatFor}\n";
    foreach (T obj in this.list)
    {
      string translatedName = obj.getTranslatedName();
      if (!(translatedName == "Phenotype"))
      {
        string translatedDescription = obj.getTranslatedDescription();
        string translatedDescription2 = obj.getTranslatedDescription2();
        string str2 = "\n" + translatedName + "\n";
        if (!string.IsNullOrEmpty(translatedDescription))
          str2 = $"{str2}1: {translatedDescription}";
        if (!string.IsNullOrEmpty(translatedDescription2))
          str2 = $"{str2}\n2: {translatedDescription2}";
        str1 += str2;
      }
    }
    return str1 + "\n\n";
  }

  public T getRandomSpawnTrait() => this._pot_allowed_to_be_given_randomly.GetRandom<T>();

  protected virtual string icon_path => throw new NotImplementedException(this.GetType().Name);

  protected virtual List<string> getDefaultTraitsForMeta(ActorAsset pAsset)
  {
    throw new NotImplementedException();
  }
}
