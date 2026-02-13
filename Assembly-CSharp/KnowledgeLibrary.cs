// Decompiled with JetBrains decompiler
// Type: KnowledgeLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class KnowledgeLibrary : AssetLibrary<KnowledgeAsset>
{
  private KnowledgeAsset units;
  private KnowledgeAsset items;
  private KnowledgeAsset genes;
  private KnowledgeAsset traits;
  private KnowledgeAsset subspecies_traits;
  private KnowledgeAsset culture_traits;
  private KnowledgeAsset language_traits;
  private KnowledgeAsset clan_traits;
  private KnowledgeAsset religion_traits;
  private KnowledgeAsset kingdom_traits;
  private KnowledgeAsset plots;

  public override void init()
  {
    base.init();
    KnowledgeAsset pAsset1 = new KnowledgeAsset();
    pAsset1.id = "units";
    pAsset1.path_icon = "ui/Icons/iconInterestingPeople";
    pAsset1.path_icon_easter_egg = "ui/Icons/iconBre";
    pAsset1.button_prefab_path = "ui/RunningIcon";
    pAsset1.window_id = "list_favorite_units";
    pAsset1.get_library = (LibraryGetter) (() => (ILibraryWithUnlockables) AssetManager.actor_library);
    pAsset1.get_asset_sprite = new SpriteGetter(this.getActorSprite);
    pAsset1.load_button = (KnowledgeButtonLoader) ((pButton, pAsset) =>
    {
      RunningIcon component = ((Component) pButton).GetComponent<RunningIcon>();
      Sprite pIcon = this.units.get_asset_sprite(pAsset);
      component.setIcon(pIcon);
      this.checkButtonColor(component.getIconImage(), pAsset);
    });
    pAsset1.tip_button_loader = (ButtonTipLoader) ((pButton, pAsset) =>
    {
      ActorAsset tAsset = pAsset as ActorAsset;
      ((Component) pButton).GetComponent<TipButton>().setHoverAction((TooltipAction) (() => Tooltip.show((object) pButton, "unit_button", new TooltipData()
      {
        actor_asset = tAsset
      })));
    });
    pAsset1.show_tooltip = (ButtonTooltipLoader) ((pButton, pAsset) =>
    {
      ActorAsset actorAsset = (ActorAsset) pAsset;
      TooltipData pData = new TooltipData()
      {
        actor_asset = actorAsset
      };
      Tooltip.show((object) pButton, "unit_button", pData);
      return pData;
    });
    this.units = this.add(pAsset1);
    KnowledgeAsset pAsset2 = new KnowledgeAsset();
    pAsset2.id = "items";
    pAsset2.path_icon = "ui/Icons/iconEquipmentEditor";
    pAsset2.path_icon_easter_egg = "ui/Icons/civs/civ_armadillo";
    pAsset2.button_prefab_path = "ui/EquipmentButton";
    pAsset2.window_id = "equipment_rain_editor";
    pAsset2.get_library = (LibraryGetter) (() => (ILibraryWithUnlockables) AssetManager.items);
    pAsset2.get_asset_sprite = new SpriteGetter(this.getAugmentationSprite);
    pAsset2.load_button = (KnowledgeButtonLoader) ((pButton, pAsset) => this.augmentationButtonLoader<EquipmentButton, EquipmentAsset>(pButton, pAsset));
    pAsset2.show_tooltip = (ButtonTooltipLoader) ((pButton, pAsset) =>
    {
      EquipmentAsset equipmentAsset = (EquipmentAsset) pAsset;
      TooltipData pData = new TooltipData()
      {
        item_asset = equipmentAsset
      };
      Tooltip.show((object) pButton, "equipment_in_editor", pData);
      return pData;
    });
    this.items = this.add(pAsset2);
    KnowledgeAsset pAsset3 = new KnowledgeAsset();
    pAsset3.id = "genes";
    pAsset3.path_icon = "ui/Icons/iconGene";
    pAsset3.path_icon_easter_egg = "ui/Icons/iconGreg";
    pAsset3.button_prefab_path = "ui/genetic_elements/GeneButton";
    pAsset3.window_id = "list_subspecies";
    pAsset3.get_library = (LibraryGetter) (() => (ILibraryWithUnlockables) AssetManager.gene_library);
    pAsset3.get_asset_sprite = new SpriteGetter(this.getAugmentationSprite);
    pAsset3.load_button = (KnowledgeButtonLoader) ((pButton, pAsset) => this.augmentationButtonLoader<GeneButton, GeneAsset>(pButton, pAsset));
    pAsset3.show_tooltip = (ButtonTooltipLoader) ((pButton, pAsset) =>
    {
      GeneAsset geneAsset = (GeneAsset) pAsset;
      TooltipData pData = new TooltipData()
      {
        gene = geneAsset
      };
      Tooltip.show((object) pButton, "gene", pData);
      return pData;
    });
    this.genes = this.add(pAsset3);
    KnowledgeAsset pAsset4 = new KnowledgeAsset();
    pAsset4.id = "traits";
    pAsset4.path_icon = "ui/Icons/iconEditTrait";
    pAsset4.path_icon_easter_egg = "ui/Icons/iconZombie";
    pAsset4.button_prefab_path = "ui/unit_window_elements/ActorTraitButton";
    pAsset4.window_id = "trait_rain_editor";
    pAsset4.get_library = (LibraryGetter) (() => (ILibraryWithUnlockables) AssetManager.traits);
    pAsset4.get_asset_sprite = new SpriteGetter(this.getAugmentationSprite);
    pAsset4.load_button = (KnowledgeButtonLoader) ((pButton, pAsset) => this.traitButtonLoader<ActorTraitButton, ActorTrait>(pButton, pAsset));
    pAsset4.show_tooltip = (ButtonTooltipLoader) ((pButton, pAsset) =>
    {
      ActorTrait actorTrait = (ActorTrait) pAsset;
      TooltipData pData = new TooltipData()
      {
        trait = actorTrait,
        is_editor_augmentation_button = true
      };
      Tooltip.show((object) pButton, actorTrait.typed_id, pData);
      return pData;
    });
    pAsset4.click_icon_action = (OnKnowledgeIconClick) (pAsset =>
    {
      Config.selected_trait_editor = PowerLibrary.traits_delta_rain_edit.id;
      ScrollWindow.showWindow(pAsset.window_id);
    });
    this.traits = this.add(pAsset4);
    KnowledgeAsset pAsset5 = new KnowledgeAsset();
    pAsset5.id = "subspecies_traits";
    pAsset5.path_icon = "ui/Icons/iconSpecies";
    pAsset5.path_icon_easter_egg = "ui/Icons/subspecies_traits/subspecies_trait_reproduction_budding";
    pAsset5.button_prefab_path = "ui/subspecies_window_elements/SubspeciesTraitButton";
    pAsset5.window_id = "list_subspecies";
    pAsset5.get_library = (LibraryGetter) (() => (ILibraryWithUnlockables) AssetManager.subspecies_traits);
    pAsset5.get_asset_sprite = new SpriteGetter(this.getAugmentationSprite);
    pAsset5.load_button = (KnowledgeButtonLoader) ((pButton, pAsset) => this.traitButtonLoader<SubspeciesTraitButton, SubspeciesTrait>(pButton, pAsset));
    pAsset5.show_tooltip = (ButtonTooltipLoader) ((pButton, pAsset) =>
    {
      SubspeciesTrait subspeciesTrait = (SubspeciesTrait) pAsset;
      TooltipData pData = new TooltipData()
      {
        subspecies_trait = subspeciesTrait,
        is_editor_augmentation_button = true
      };
      Tooltip.show((object) pButton, subspeciesTrait.typed_id, pData);
      return pData;
    });
    this.subspecies_traits = this.add(pAsset5);
    KnowledgeAsset pAsset6 = new KnowledgeAsset();
    pAsset6.id = "culture_traits";
    pAsset6.path_icon = "ui/Icons/iconCulture";
    pAsset6.path_icon_easter_egg = "ui/Icons/iconEvilMage";
    pAsset6.button_prefab_path = "ui/culture_window_elements/CultureTraitButton";
    pAsset6.window_id = "list_cultures";
    pAsset6.get_library = (LibraryGetter) (() => (ILibraryWithUnlockables) AssetManager.culture_traits);
    pAsset6.get_asset_sprite = new SpriteGetter(this.getAugmentationSprite);
    pAsset6.load_button = (KnowledgeButtonLoader) ((pButton, pAsset) => this.traitButtonLoader<CultureTraitButton, CultureTrait>(pButton, pAsset));
    pAsset6.show_tooltip = (ButtonTooltipLoader) ((pButton, pAsset) =>
    {
      CultureTrait cultureTrait = (CultureTrait) pAsset;
      TooltipData pData = new TooltipData()
      {
        culture_trait = cultureTrait,
        is_editor_augmentation_button = true
      };
      Tooltip.show((object) pButton, cultureTrait.typed_id, pData);
      return pData;
    });
    this.culture_traits = this.add(pAsset6);
    KnowledgeAsset pAsset7 = new KnowledgeAsset();
    pAsset7.id = "language_traits";
    pAsset7.path_icon = "ui/Icons/iconLanguage";
    pAsset7.path_icon_easter_egg = "ui/Icons/iconShrug";
    pAsset7.button_prefab_path = "ui/language_window_elements/LanguageTraitButton";
    pAsset7.window_id = "list_languages";
    pAsset7.get_library = (LibraryGetter) (() => (ILibraryWithUnlockables) AssetManager.language_traits);
    pAsset7.get_asset_sprite = new SpriteGetter(this.getAugmentationSprite);
    pAsset7.load_button = (KnowledgeButtonLoader) ((pButton, pAsset) => this.traitButtonLoader<LanguageTraitButton, LanguageTrait>(pButton, pAsset));
    pAsset7.show_tooltip = (ButtonTooltipLoader) ((pButton, pAsset) =>
    {
      LanguageTrait languageTrait = (LanguageTrait) pAsset;
      TooltipData pData = new TooltipData()
      {
        language_trait = languageTrait,
        is_editor_augmentation_button = true
      };
      Tooltip.show((object) pButton, languageTrait.typed_id, pData);
      return pData;
    });
    this.language_traits = this.add(pAsset7);
    KnowledgeAsset pAsset8 = new KnowledgeAsset();
    pAsset8.id = "clan_traits";
    pAsset8.path_icon = "ui/Icons/iconClan";
    pAsset8.path_icon_easter_egg = "ui/Icons/iconLivingPlants";
    pAsset8.button_prefab_path = "ui/clan_window_elements/ClanTraitButton";
    pAsset8.window_id = "list_clans";
    pAsset8.get_library = (LibraryGetter) (() => (ILibraryWithUnlockables) AssetManager.clan_traits);
    pAsset8.get_asset_sprite = new SpriteGetter(this.getAugmentationSprite);
    pAsset8.load_button = (KnowledgeButtonLoader) ((pButton, pAsset) => this.traitButtonLoader<ClanTraitButton, ClanTrait>(pButton, pAsset));
    pAsset8.show_tooltip = (ButtonTooltipLoader) ((pButton, pAsset) =>
    {
      ClanTrait clanTrait = (ClanTrait) pAsset;
      TooltipData pData = new TooltipData()
      {
        clan_trait = clanTrait,
        is_editor_augmentation_button = true
      };
      Tooltip.show((object) pButton, clanTrait.typed_id, pData);
      return pData;
    });
    this.clan_traits = this.add(pAsset8);
    KnowledgeAsset pAsset9 = new KnowledgeAsset();
    pAsset9.id = "religion_traits";
    pAsset9.path_icon = "ui/Icons/iconReligion";
    pAsset9.path_icon_easter_egg = "ui/Icons/iconWhiteMage";
    pAsset9.button_prefab_path = "ui/religion_window_elements/ReligionTraitButton";
    pAsset9.window_id = "list_religions";
    pAsset9.get_library = (LibraryGetter) (() => (ILibraryWithUnlockables) AssetManager.religion_traits);
    pAsset9.get_asset_sprite = new SpriteGetter(this.getAugmentationSprite);
    pAsset9.load_button = (KnowledgeButtonLoader) ((pButton, pAsset) => this.traitButtonLoader<ReligionTraitButton, ReligionTrait>(pButton, pAsset));
    pAsset9.show_tooltip = (ButtonTooltipLoader) ((pButton, pAsset) =>
    {
      ReligionTrait religionTrait = (ReligionTrait) pAsset;
      TooltipData pData = new TooltipData()
      {
        religion_trait = religionTrait,
        is_editor_augmentation_button = true
      };
      Tooltip.show((object) pButton, religionTrait.typed_id, pData);
      return pData;
    });
    this.religion_traits = this.add(pAsset9);
    KnowledgeAsset pAsset10 = new KnowledgeAsset();
    pAsset10.id = "kingdom_traits";
    pAsset10.show_in_knowledge_window = false;
    pAsset10.path_icon = "ui/Icons/iconKingdom";
    pAsset10.path_icon_easter_egg = "ui/Icons/iconWhiteMage";
    pAsset10.button_prefab_path = "ui/kingdom_window_elements/KingdomTraitButton";
    pAsset10.window_id = "list_kingdoms";
    pAsset10.get_library = (LibraryGetter) (() => (ILibraryWithUnlockables) AssetManager.kingdoms_traits);
    pAsset10.get_asset_sprite = new SpriteGetter(this.getAugmentationSprite);
    pAsset10.load_button = (KnowledgeButtonLoader) ((pButton, pAsset) => this.traitButtonLoader<KingdomTraitButton, KingdomTrait>(pButton, pAsset));
    pAsset10.show_tooltip = (ButtonTooltipLoader) ((pButton, pAsset) =>
    {
      KingdomTrait kingdomTrait = (KingdomTrait) pAsset;
      TooltipData pData = new TooltipData()
      {
        kingdom_trait = kingdomTrait,
        is_editor_augmentation_button = true
      };
      Tooltip.show((object) pButton, kingdomTrait.typed_id, pData);
      return pData;
    });
    this.kingdom_traits = this.add(pAsset10);
    KnowledgeAsset pAsset11 = new KnowledgeAsset();
    pAsset11.id = "plots";
    pAsset11.path_icon = "ui/Icons/iconPlot";
    pAsset11.path_icon_easter_egg = "ui/Icons/actor_traits/iconGenius";
    pAsset11.button_prefab_path = "ui/PlotButton";
    pAsset11.window_id = "list_plots";
    pAsset11.get_library = (LibraryGetter) (() => (ILibraryWithUnlockables) AssetManager.plots_library);
    pAsset11.get_asset_sprite = new SpriteGetter(this.getAugmentationSprite);
    pAsset11.load_button = (KnowledgeButtonLoader) ((pButton, pAsset) => this.augmentationButtonLoader<PlotButton, PlotAsset>(pButton, pAsset));
    pAsset11.show_tooltip = (ButtonTooltipLoader) ((pButton, pAsset) =>
    {
      PlotAsset plotAsset = (PlotAsset) pAsset;
      TooltipData pData = new TooltipData()
      {
        plot_asset = plotAsset
      };
      Tooltip.show((object) pButton, "plot_in_editor", pData);
      return pData;
    });
    this.plots = this.add(pAsset11);
  }

  private Sprite getActorSprite(BaseUnlockableAsset pAsset)
  {
    return ((ActorAsset) pAsset).getSpriteIcon();
  }

  private Sprite getAugmentationSprite(BaseUnlockableAsset pAsset) => pAsset.getSprite();

  private TButton augmentationButtonLoader<TButton, TAsset>(
    Transform pButton,
    BaseUnlockableAsset pAsset)
    where TButton : AugmentationButton<TAsset>
    where TAsset : BaseAugmentationAsset
  {
    TButton component = ((Component) pButton).GetComponent<TButton>();
    component.load(pAsset as TAsset);
    ((Behaviour) component.locked_bg).enabled = false;
    this.checkButtonColor(component.image, pAsset);
    component.is_editor_button = true;
    return component;
  }

  private TButton traitButtonLoader<TButton, TAsset>(Transform pButton, BaseUnlockableAsset pAsset)
    where TButton : TraitButton<TAsset>
    where TAsset : BaseTrait<TAsset>
  {
    TButton button = this.augmentationButtonLoader<TButton, TAsset>(pButton, pAsset);
    this.checkButtonColor(button.image, pAsset);
    return button;
  }

  private void checkButtonColor(Image pImage, BaseUnlockableAsset pAsset)
  {
    if (pAsset.isUnlockedByPlayer())
      ((Graphic) pImage).color = Toolbox.color_white;
    else
      ((Graphic) pImage).color = Toolbox.color_black;
  }

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (KnowledgeAsset pAsset in this.list)
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
  }
}
