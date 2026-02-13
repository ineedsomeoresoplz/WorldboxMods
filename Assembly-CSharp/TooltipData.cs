// Decompiled with JetBrains decompiler
// Type: TooltipData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.ComponentModel;

#nullable disable
public class TooltipData : IDisposable
{
  [NonSerialized]
  private string _tip_name;
  [NonSerialized]
  private string _tip_description;
  [NonSerialized]
  private string _tip_description_2;
  public War war;
  public Army army;
  public Subspecies subspecies;
  public Family family;
  public Language language;
  public Culture culture;
  public Religion religion;
  public City city;
  public Clan clan;
  public Kingdom kingdom;
  public Alliance alliance;
  public Plot plot;
  public PlotAsset plot_asset;
  public Book book;
  public ResourceAsset resource;
  public Item item;
  public EquipmentAsset item_asset;
  public bool is_editor_augmentation_button;
  public Actor actor;
  public ActorTrait trait;
  public ActorAsset actor_asset;
  public Status status;
  public KingdomAsset kingdom_asset;
  public KingdomTrait kingdom_trait;
  public CultureTrait culture_trait;
  public LanguageTrait language_trait;
  public SubspeciesTrait subspecies_trait;
  public ClanTrait clan_trait;
  public ReligionTrait religion_trait;
  public OnomasticsAsset onomastics_asset;
  public OnomasticsData onomastics_data;
  public NanoObject nano_object;
  public Chromosome chromosome;
  public GeneAsset gene;
  public LocusElement locus;
  public NeuronElement neuron;
  public MapMetaData map_meta;
  public GodPower power;
  public Achievement achievement;
  public WorldLawAsset world_law;
  public ListPool<NameEntry> past_names;
  public ListPool<LeaderEntry> past_rulers;
  public GameLanguageAsset game_language_asset;
  public MetaType meta_type;
  public CustomDataContainer<long> custom_data_long;
  public CustomDataContainer<int> custom_data_int;
  public CustomDataContainer<float> custom_data_float;
  public CustomDataContainer<bool> custom_data_bool;
  public CustomDataContainer<string> custom_data_string;
  [DefaultValue(1f)]
  public float tooltip_scale = 1f;
  public bool sound_allowed = true;
  public bool is_sim_tooltip;

  public string tip_name
  {
    set => this._tip_name = value;
    get => this._tip_name;
  }

  public string tip_description
  {
    set => this._tip_description = value;
    get => this._tip_description;
  }

  public string tip_description_2
  {
    set => this._tip_description_2 = value;
    get => this._tip_description_2;
  }

  public void Dispose()
  {
    this.custom_data_long?.Dispose();
    this.custom_data_int?.Dispose();
    this.custom_data_float?.Dispose();
    this.custom_data_bool?.Dispose();
    this.custom_data_string?.Dispose();
    this.war = (War) null;
    this.army = (Army) null;
    this.subspecies = (Subspecies) null;
    this.family = (Family) null;
    this.language = (Language) null;
    this.culture = (Culture) null;
    this.religion = (Religion) null;
    this.city = (City) null;
    this.clan = (Clan) null;
    this.kingdom = (Kingdom) null;
    this.alliance = (Alliance) null;
    this.plot = (Plot) null;
    this.plot_asset = (PlotAsset) null;
    this.book = (Book) null;
    this.resource = (ResourceAsset) null;
    this.item = (Item) null;
    this.item_asset = (EquipmentAsset) null;
    this.actor = (Actor) null;
    this.trait = (ActorTrait) null;
    this.actor_asset = (ActorAsset) null;
    this.status = (Status) null;
    this.kingdom_asset = (KingdomAsset) null;
    this.kingdom_trait = (KingdomTrait) null;
    this.culture_trait = (CultureTrait) null;
    this.language_trait = (LanguageTrait) null;
    this.subspecies_trait = (SubspeciesTrait) null;
    this.clan_trait = (ClanTrait) null;
    this.religion_trait = (ReligionTrait) null;
    this.onomastics_asset = (OnomasticsAsset) null;
    this.onomastics_data = (OnomasticsData) null;
    this.nano_object = (NanoObject) null;
    this.chromosome = (Chromosome) null;
    this.gene = (GeneAsset) null;
    this.locus = (LocusElement) null;
    this.neuron = (NeuronElement) null;
    this.map_meta = (MapMetaData) null;
    this.power = (GodPower) null;
    this.achievement = (Achievement) null;
    this.world_law = (WorldLawAsset) null;
    this.past_names?.Dispose();
    this.past_names = (ListPool<NameEntry>) null;
    this.past_rulers?.Dispose();
    this.past_rulers = (ListPool<LeaderEntry>) null;
    this.game_language_asset = (GameLanguageAsset) null;
    this.is_editor_augmentation_button = false;
    this.meta_type = MetaType.None;
    this.sound_allowed = true;
  }
}
