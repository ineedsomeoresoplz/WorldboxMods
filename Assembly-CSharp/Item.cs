// Decompiled with JetBrains decompiler
// Type: Item
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Item : CoreSystemObject<ItemData>
{
  private EquipmentAsset _asset;
  private string _texture_id;
  public bool unit_has_it;
  public bool city_has_it;
  public bool shouldbe_removed;
  private Actor _actor;
  private City _city;
  private int _item_value;
  private readonly BaseStats _total_stats = new BaseStats();
  private Rarity _quality;
  public AttackAction action_attack_target;

  protected override MetaType meta_type => MetaType.Item;

  public override BaseSystemManager manager => (BaseSystemManager) World.world.items;

  public override void setFavorite(bool pState)
  {
    base.setFavorite(pState);
    if (!this.isReadyForRemoval())
      return;
    World.world.items.setDirty();
  }

  public bool isCursed() => this.hasMod("cursed");

  public bool isEternal() => this.hasMod("eternal");

  public bool isDestroyable() => !this.isFavorite() && !this.isEternal();

  public bool isReadyForRemoval() => !this.hasCity() && !this.hasActor() && this.isDestroyable();

  public void fullRepair() => this.data.durability = 100;

  public void getDamaged(int pDamage)
  {
    if (this.hasMod("eternal"))
      return;
    this.data.durability -= pDamage;
    if (this.data.durability >= 0)
      return;
    this.data.durability = 0;
  }

  public void newItem(EquipmentAsset pAsset)
  {
    this.setAsset(pAsset);
    this.data.durability = pAsset.durability;
  }

  public bool isBroken() => this.getDurabilityCurrent() <= 0;

  public bool needRepair() => this.getDurabilityCurrent() < this.getDurabilityMax();

  public int getDurabilityCurrent() => this.data.durability;

  public int getDurabilityMax() => this._asset.durability;

  public string getDurabilityString()
  {
    int num = this.getDurabilityCurrent();
    string str1 = num.ToString();
    num = this.getDurabilityMax();
    string str2 = num.ToString();
    return $"{str1}/{str2}";
  }

  private void setAsset(EquipmentAsset pAsset)
  {
    this._asset = pAsset;
    this.data.asset_id = this._asset.id;
    this.recalculateTexturePath();
  }

  private void recalculateTexturePath() => this._texture_id = this.asset.path_gameplay_sprite;

  protected sealed override void setDefaultValues()
  {
    base.setDefaultValues();
    this._asset = (EquipmentAsset) null;
    this.unit_has_it = false;
    this.city_has_it = false;
    this.shouldbe_removed = false;
    this._item_value = 0;
    this._quality = Rarity.R0_Normal;
  }

  public void reforge(int pTries)
  {
    this.clearForReforge();
    World.world.items.generateModsFor(this, pTries);
    this.setName(NameGenerator.getName("reforged_item"));
    this.calculateValues();
    if (this._actor == null)
      return;
    this._actor.addTrait("scar_of_divinity");
  }

  private void clearForReforge() => this.data.modifiers.Clear();

  public void transmute()
  {
    // ISSUE: unable to decompile the method.
  }

  public bool hasMod(string pID) => this.data.modifiers.Contains(pID);

  public bool hasMod(ItemModAsset pModAsset) => this.hasMod(pModAsset.id);

  public bool addMod(string pModId) => this.addMod(AssetManager.items_modifiers.get(pModId));

  public bool addMod(ItemModAsset pModAsset)
  {
    // ISSUE: unable to decompile the method.
  }

  public bool removeMod(string pModId)
  {
    World.world.items.setDirty();
    return this.data.modifiers.Remove(pModId);
  }

  public void setUnitHasIt(Actor pActor)
  {
    this._actor = pActor;
    this._city = (City) null;
    this.unit_has_it = true;
    this.city_has_it = false;
  }

  public void setInCityStorage(City pCity)
  {
    this._actor = (Actor) null;
    this._city = pCity;
    this.unit_has_it = false;
    this.city_has_it = true;
  }

  public void setShouldBeRemoved() => this.shouldbe_removed = true;

  public void clearUnit()
  {
    this._actor = (Actor) null;
    this.unit_has_it = false;
    World.world.items.setDirty();
  }

  public void clearCity()
  {
    this._city = (City) null;
    this.city_has_it = false;
    World.world.items.setDirty();
  }

  public Actor getActor() => this._actor;

  public City getCity() => this._city;

  public void calculateValues()
  {
    this.recalculateTexturePath();
    ItemTools.calcItemValues(this, this._total_stats);
    this._item_value = ItemTools.s_value;
    this._quality = ItemTools.s_quality;
    this.data.durability = this._asset.durability;
  }

  public void initItem()
  {
    this.initFields();
    this.calculateValues();
  }

  public override void loadData(ItemData pData)
  {
    base.loadData(pData);
    this.initFields();
    this.calculateValues();
  }

  private void initFields() => this.setAsset(AssetManager.items.get(this.data.asset_id));

  public EquipmentAsset getAsset() => this._asset;

  public EquipmentAsset asset => this.getAsset();

  public string getItemDescription()
  {
    string str1 = LocalizedTextManager.getText(!this.data.created_by_player ? "item_template_description_full" : "item_template_description_full_player");
    string text = LocalizedTextManager.getText("item_template_description_age_only");
    if (string.IsNullOrEmpty(this.data.by))
      str1 = text;
    int age = this.getAge();
    string str2 = str1.Replace("$item_creator_name$", Toolbox.coloredString(this.data.by, this.data.byColor)).Replace("$item_creator_kingdom$", Toolbox.coloredString(this.data.from, this.data.fromColor)).Replace("$item_creator_years$", age.ToString() ?? "");
    return age != 1 ? str2.Replace("$year_ending$", LocalizedTextManager.getText("item_template_description_years") ?? "") : str2.Replace("$year_ending$", LocalizedTextManager.getText("item_template_description_year") ?? "");
  }

  public string getItemKeyType() => this._asset.getLocaleRarity(this.getQuality());

  public void countKill() => ++this.data.kills;

  public bool isRangeAttack() => this.getAsset().attack_type == WeaponType.Range;

  public BaseStats getFullStats() => this._total_stats;

  public int getValue() => this._item_value;

  public Rarity getQuality() => this._quality;

  public string getTextureID() => this._texture_id;

  public override string name
  {
    get => this.getName(false);
    protected set => this.data.name = value;
  }

  public string getName(bool pWithMaterial = true)
  {
    if (!string.IsNullOrEmpty(this.data.name))
      return this.data.name;
    string pName;
    string pMaterial;
    ItemTools.getTooltipTitle(this.getAsset(), out pName, out pMaterial);
    return pMaterial + pName;
  }

  public string getQualityColor() => this.getQuality().getRarityColorHex();

  public static string getQualityString(Rarity pRarity)
  {
    string qualityString = "";
    switch (pRarity)
    {
      case Rarity.R0_Normal:
        qualityString = "";
        break;
      case Rarity.R1_Rare:
        qualityString = "_rare";
        break;
      case Rarity.R2_Epic:
        qualityString = "_epic";
        break;
      case Rarity.R3_Legendary:
        qualityString = "_legendary";
        break;
    }
    return qualityString;
  }

  public Sprite getSprite() => this._asset.getSprite();

  public bool hasActor() => this.getActor() != null;

  public bool hasCity() => this.getCity() != null;

  public override void Dispose()
  {
    this._asset = (EquipmentAsset) null;
    this._texture_id = (string) null;
    this._total_stats.reset();
    this._actor = (Actor) null;
    this.action_attack_target = (AttackAction) null;
    base.Dispose();
  }

  private bool isCheatEnabled()
  {
    return DebugConfig.isOn(DebugOption.UnlockAllEquipment) || WorldLawLibrary.world_law_cursed_world.isEnabled();
  }
}
