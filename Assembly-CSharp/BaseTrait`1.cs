// Decompiled with JetBrains decompiler
// Type: BaseTrait`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
[Serializable]
public class BaseTrait<TTrait> : 
  BaseAugmentationAsset,
  IDescription2Asset,
  IDescriptionAsset,
  ILocalizedAsset
  where TTrait : BaseTrait<TTrait>
{
  public float value;
  public WorldAction action_death;
  public WorldAction action_growth;
  public WorldAction action_birth;
  public GetHitAction action_get_hit;
  [DefaultValue(true)]
  public bool spawn_random_trait_allowed = true;
  [DefaultValue(5)]
  public int spawn_random_rate = 5;
  public bool special_icon_logic;
  public BaseStats base_stats_meta;
  public List<string> opposite_list;
  [NonSerialized]
  public HashSet<TTrait> opposite_traits;
  public string[] traits_to_remove_ids;
  [NonSerialized]
  public TTrait[] traits_to_remove;
  [DefaultValue("")]
  public string special_locale_description = string.Empty;
  [DefaultValue("")]
  public string special_locale_description_2 = string.Empty;
  [DefaultValue(true)]
  public bool has_localized_id = true;
  [DefaultValue(true)]
  public bool has_description_1 = true;
  [DefaultValue(true)]
  public bool has_description_2 = true;
  [DefaultValue(Rarity.R1_Rare)]
  public Rarity rarity = Rarity.R1_Rare;
  [DefaultValue(true)]
  public bool can_be_in_book = true;
  [DefaultValue("")]
  public string plot_id = string.Empty;
  [JsonIgnore]
  public List<ActorAsset> default_for_actor_assets;

  [JsonIgnore]
  public virtual string typed_id => throw new NotImplementedException(this.GetType().Name);

  public bool hasPlotAsset() => !string.IsNullOrEmpty(this.plot_id);

  [JsonIgnore]
  public PlotAsset plot_asset => AssetManager.plots_library.get(this.plot_id);

  public string getId() => this.id;

  public WorldAction getSpecialEffect() => this.action_special_effect;

  public float getSpecialEffectInterval() => this.special_effect_interval;

  public void addOpposite(string pID)
  {
    if (this.opposite_list == null)
      this.opposite_list = new List<string>();
    this.opposite_list.Add(pID);
  }

  public void addOpposites(IEnumerable<string> pListIDS)
  {
    if (this.opposite_list == null)
      this.opposite_list = new List<string>(pListIDS);
    else
      this.opposite_list.AddRange(pListIDS);
  }

  public void removeOpposite(string pID) => this.opposite_list.Remove(pID);

  public override string getLocaleID()
  {
    if (!this.has_localized_id)
      return (string) null;
    return !string.IsNullOrEmpty(this.special_locale_id) ? this.special_locale_id : $"{this.typed_id}_{this.id}";
  }

  public string getDescriptionID()
  {
    if (!this.has_description_1)
      return (string) null;
    return !string.IsNullOrEmpty(this.special_locale_description) ? this.special_locale_description : $"{this.typed_id}_{this.id}_info";
  }

  public string getDescriptionID2()
  {
    if (!this.has_description_2)
      return (string) null;
    return !string.IsNullOrEmpty(this.special_locale_description_2) ? this.special_locale_description_2 : $"{this.typed_id}_{this.id}_info_2";
  }

  public string getTranslatedName() => LocalizedTextManager.getText(this.getLocaleID());

  public string getTranslatedDescription()
  {
    string descriptionId = this.getDescriptionID();
    return LocalizedTextManager.stringExists(descriptionId) ? LocalizedTextManager.getText(descriptionId) : (string) null;
  }

  public string getTranslatedDescription2()
  {
    string descriptionId2 = this.getDescriptionID2();
    return LocalizedTextManager.stringExists(descriptionId2) ? LocalizedTextManager.getText(descriptionId2) : (string) null;
  }

  protected override bool isDebugUnlockedAll() => DebugConfig.isOn(DebugOption.UnlockAllTraits);

  protected virtual IEnumerable<ITraitsOwner<TTrait>> getRelatedMetaList()
  {
    throw new NotImplementedException(this.GetType().Name);
  }

  private ListPool<ITraitsOwner<TTrait>> getOwnersList()
  {
    ListPool<ITraitsOwner<TTrait>> ownersList = new ListPool<ITraitsOwner<TTrait>>();
    TTrait pTraitId = (TTrait) this;
    foreach (ITraitsOwner<TTrait> relatedMeta in this.getRelatedMetaList())
    {
      if (relatedMeta.hasTrait(pTraitId))
        ownersList.Add(relatedMeta);
    }
    return ownersList;
  }

  private (int pTotal, int pCivs, int pMobs) countTraitOwnersByCategories()
  {
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    TTrait pTraitId = (TTrait) this;
    foreach (ITraitsOwner<TTrait> relatedMeta in this.getRelatedMetaList())
    {
      if (relatedMeta.hasTrait(pTraitId))
      {
        ++num1;
        if (this.isSapient(relatedMeta))
          ++num2;
        else
          ++num3;
      }
    }
    return (num1, num2, num3);
  }

  public virtual string getCountRows()
  {
    // ISSUE: unable to decompile the method.
  }

  private string getColoredNumber(string pText) => Toolbox.coloredString(pText, "#F3961F");

  protected string getCountRowsByCategories()
  {
    (int pTotal, int pCivs, int pMobs) tuple = this.countTraitOwnersByCategories();
    using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
    {
      stringBuilderPool.Append(LocalizedTextManager.getText("trait_owners_civs").Replace("$amount$", this.getColoredNumber(tuple.pCivs.ToString())));
      stringBuilderPool.AppendLine();
      stringBuilderPool.Append(LocalizedTextManager.getText("trait_owners_mobs").Replace("$amount$", this.getColoredNumber(tuple.pMobs.ToString())));
      return stringBuilderPool.ToString();
    }
  }

  protected virtual bool isSapient(ITraitsOwner<TTrait> pObject)
  {
    throw new NotImplementedException(this.GetType().Name);
  }

  public bool ShouldSerializebase_stats_meta() => !this.base_stats_meta.isEmpty();

  public void setTraitInfoToGrinMark()
  {
    this.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_grin_mark";
    this.special_locale_id = "subspecies_trait_grin_mark";
    this.special_locale_description = "subspecies_trait_grin_mark_info";
    this.special_locale_description_2 = "subspecies_trait_grin_mark_info_2";
    this.show_for_unlockables_ui = false;
    this.action_on_augmentation_add = this.action_on_augmentation_add + new WorldActionTrait(WorldBehaviourActions.addForGrinReaper);
    this.action_on_augmentation_load = this.action_on_augmentation_load + new WorldActionTrait(WorldBehaviourActions.addForGrinReaper);
    this.action_on_augmentation_remove = this.action_on_augmentation_remove + new WorldActionTrait(WorldBehaviourActions.removeUsedForGrinReaper);
    this.action_on_object_remove = this.action_on_object_remove + new WorldActionTrait(WorldBehaviourActions.removeUsedForGrinReaper);
  }
}
