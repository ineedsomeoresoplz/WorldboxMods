// Decompiled with JetBrains decompiler
// Type: CommunicationTopicLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CommunicationTopicLibrary : AssetLibrary<CommunicationAsset>
{
  private List<Sprite> _cached_sprites_religion = new List<Sprite>();
  private List<Sprite> _cached_sprites_culture = new List<Sprite>();
  private List<Sprite> _cached_sprites_family = new List<Sprite>();
  private List<Sprite> _cached_sprites_kingdom = new List<Sprite>();
  private List<Sprite> _cached_sprites_city = new List<Sprite>();
  private List<Sprite> _cached_sprites_clan = new List<Sprite>();
  private List<Sprite> _cached_sprites_time_and_death = new List<Sprite>();
  private List<Sprite> _cached_sprites_general_topics = new List<Sprite>();
  private List<Sprite> _cached_sprites_boats_water = new List<Sprite>();
  private List<Sprite> _cached_sprites_housed = new List<Sprite>();
  private List<Sprite> _cached_sprites_homeless = new List<Sprite>();
  private const int MAX_TOPIC_SPRITES = 10;

  public override void init()
  {
    CommunicationAsset pAsset1 = new CommunicationAsset();
    pAsset1.id = "emotions";
    pAsset1.rate = 0.9f;
    pAsset1.check = (TopicCheck) (pActor => pActor.hasEmotions());
    pAsset1.pot_fill = (TopicPotFill) ((pActor, pPotSprites) =>
    {
      Sprite onHappinessValue = HappinessHelper.getSpriteBasedOnHappinessValue(pActor.getHappiness());
      for (int index = 0; index < 3; ++index)
        pPotSprites.Add(onHappinessValue);
      if (!pActor.hasHappinessHistory())
        return;
      foreach (HappinessHistory happinessHistory in pActor.happiness_change_history)
      {
        Sprite sprite = happinessHistory.asset.getSprite();
        pPotSprites.Add(sprite);
      }
    });
    this.add(pAsset1);
    CommunicationAsset pAsset2 = new CommunicationAsset();
    pAsset2.id = "is_housed";
    pAsset2.rate = 0.2f;
    pAsset2.check = (TopicCheck) (pActor => pActor.hasCity() && pActor.hasHouse());
    pAsset2.pot_fill = (TopicPotFill) ((_, pPotSprites) => pPotSprites.AddRange((IEnumerable<Sprite>) this._cached_sprites_housed));
    this.add(pAsset2);
    CommunicationAsset pAsset3 = new CommunicationAsset();
    pAsset3.id = "is_homeless";
    pAsset3.rate = 0.4f;
    pAsset3.check = (TopicCheck) (pActor => pActor.hasCity() && !pActor.hasHouse());
    pAsset3.pot_fill = (TopicPotFill) ((_, pPotSprites) => pPotSprites.AddRange((IEnumerable<Sprite>) this._cached_sprites_homeless));
    this.add(pAsset3);
    CommunicationAsset pAsset4 = new CommunicationAsset();
    pAsset4.id = "favorite_food";
    pAsset4.rate = 0.4f;
    pAsset4.check = (TopicCheck) (pActor => pActor.hasFavoriteFood());
    pAsset4.pot_fill = (TopicPotFill) ((pActor, pPotSprites) =>
    {
      Sprite spriteIcon = pActor.favorite_food_asset.getSpriteIcon();
      if (!Object.op_Inequality((Object) spriteIcon, (Object) null))
        return;
      pPotSprites.Add(spriteIcon);
    });
    this.add(pAsset4);
    CommunicationAsset pAsset5 = new CommunicationAsset();
    pAsset5.id = "religion";
    pAsset5.rate = 0.2f;
    pAsset5.check = (TopicCheck) (pActor => pActor.hasReligion());
    pAsset5.pot_fill = (TopicPotFill) ((pActor, pPotSprites) =>
    {
      Sprite topicSprite = pActor.religion.getTopicSprite();
      if (Object.op_Inequality((Object) topicSprite, (Object) null))
        pPotSprites.Add(topicSprite);
      pPotSprites.AddRange((IEnumerable<Sprite>) this._cached_sprites_religion);
    });
    this.add(pAsset5);
    CommunicationAsset pAsset6 = new CommunicationAsset();
    pAsset6.id = "culture";
    pAsset6.rate = 0.15f;
    pAsset6.check = (TopicCheck) (pActor => pActor.hasCulture());
    pAsset6.pot_fill = (TopicPotFill) ((pActor, pPotSprites) =>
    {
      Sprite topicSprite = pActor.culture.getTopicSprite();
      if (Object.op_Inequality((Object) topicSprite, (Object) null))
        pPotSprites.Add(topicSprite);
      pPotSprites.AddRange((IEnumerable<Sprite>) this._cached_sprites_culture);
    });
    this.add(pAsset6);
    CommunicationAsset pAsset7 = new CommunicationAsset();
    pAsset7.id = "equipment";
    pAsset7.rate = 0.2f;
    pAsset7.check = (TopicCheck) (pActor => pActor.hasEquipment());
    pAsset7.pot_fill = (TopicPotFill) ((pActor, pPotSprites) =>
    {
      foreach (ActorEquipmentSlot actorEquipmentSlot in pActor.equipment)
      {
        Sprite sprite = actorEquipmentSlot.getItem().getAsset().getSprite();
        if (Object.op_Inequality((Object) sprite, (Object) null))
          pPotSprites.Add(sprite);
      }
    });
    this.add(pAsset7);
    CommunicationAsset pAsset8 = new CommunicationAsset();
    pAsset8.id = "language";
    pAsset8.rate = 0.15f;
    pAsset8.check = (TopicCheck) (pActor => pActor.hasLanguage());
    pAsset8.pot_fill = (TopicPotFill) ((pActor, pPotSprites) =>
    {
      Sprite topicSprite = pActor.language.getTopicSprite();
      if (!Object.op_Inequality((Object) topicSprite, (Object) null))
        return;
      pPotSprites.Add(topicSprite);
    });
    this.add(pAsset8);
    CommunicationAsset pAsset9 = new CommunicationAsset();
    pAsset9.id = "actor_traits";
    pAsset9.rate = 0.3f;
    pAsset9.check = (TopicCheck) (pActor => pActor.hasTraits());
    pAsset9.pot_fill = (TopicPotFill) ((pActor, pPotSprites) =>
    {
      Sprite topicSpriteTrait = pActor.getTopicSpriteTrait();
      if (!Object.op_Inequality((Object) topicSpriteTrait, (Object) null))
        return;
      pPotSprites.Add(topicSpriteTrait);
    });
    this.add(pAsset9);
    CommunicationAsset pAsset10 = new CommunicationAsset();
    pAsset10.id = "family";
    pAsset10.rate = 0.3f;
    pAsset10.check = (TopicCheck) (pActor => pActor.hasFamily());
    pAsset10.pot_fill = (TopicPotFill) ((_, pPotSprites) => pPotSprites.AddRange((IEnumerable<Sprite>) this._cached_sprites_family));
    this.add(pAsset10);
    CommunicationAsset pAsset11 = new CommunicationAsset();
    pAsset11.id = "kingdom_civ";
    pAsset11.rate = 0.2f;
    pAsset11.check = (TopicCheck) (pActor => pActor.isKingdomCiv());
    pAsset11.pot_fill = (TopicPotFill) ((pActor, pPotSprites) =>
    {
      Sprite topicSprite = pActor.kingdom.getTopicSprite();
      if (Object.op_Inequality((Object) topicSprite, (Object) null))
        pPotSprites.Add(topicSprite);
      pPotSprites.AddRange((IEnumerable<Sprite>) this._cached_sprites_kingdom);
    });
    this.add(pAsset11);
    CommunicationAsset pAsset12 = new CommunicationAsset();
    pAsset12.id = "statuses";
    pAsset12.rate = 0.7f;
    pAsset12.check = (TopicCheck) (pActor => pActor.hasAnyStatusEffect());
    pAsset12.pot_fill = (TopicPotFill) ((pActor, pPotSprites) =>
    {
      foreach (Status statuse in pActor.getStatuses())
        pPotSprites.Add(statuse.asset.getSprite());
    });
    this.add(pAsset12);
    CommunicationAsset pAsset13 = new CommunicationAsset();
    pAsset13.id = "city";
    pAsset13.rate = 0.3f;
    pAsset13.check = (TopicCheck) (pActor => pActor.hasCity());
    pAsset13.pot_fill = (TopicPotFill) ((pActor, pPotSprites) =>
    {
      pPotSprites.AddRange((IEnumerable<Sprite>) this._cached_sprites_city);
      if (!pActor.city.hasStorages())
        return;
      ResourceAsset randomFoodAsset = pActor.city.storages.GetRandom<Building>().resources.getRandomFoodAsset();
      if (randomFoodAsset == null)
        return;
      Sprite spriteIcon = randomFoodAsset.getSpriteIcon();
      if (!Object.op_Inequality((Object) spriteIcon, (Object) null))
        return;
      pPotSprites.Add(spriteIcon);
    });
    this.add(pAsset13);
    CommunicationAsset pAsset14 = new CommunicationAsset();
    pAsset14.id = "city_boats";
    pAsset14.rate = 0.1f;
    pAsset14.check = (TopicCheck) (pActor => pActor.hasCity() && pActor.city.countBoats() > 0);
    pAsset14.pot_fill = (TopicPotFill) ((_, pPotSprites) => pPotSprites.AddRange((IEnumerable<Sprite>) this._cached_sprites_boats_water));
    this.add(pAsset14);
    CommunicationAsset pAsset15 = new CommunicationAsset();
    pAsset15.id = "clan";
    pAsset15.rate = 0.3f;
    pAsset15.check = (TopicCheck) (pActor => pActor.hasClan());
    pAsset15.pot_fill = (TopicPotFill) ((pActor, pPotSprites) =>
    {
      Sprite topicSprite = pActor.clan.getTopicSprite();
      if (Object.op_Inequality((Object) topicSprite, (Object) null))
        pPotSprites.Add(topicSprite);
      pPotSprites.AddRange((IEnumerable<Sprite>) this._cached_sprites_clan);
    });
    this.add(pAsset15);
    CommunicationAsset pAsset16 = new CommunicationAsset();
    pAsset16.id = "time_and_death";
    pAsset16.rate = 0.3f;
    pAsset16.check = (TopicCheck) (_ => true);
    pAsset16.pot_fill = (TopicPotFill) ((_, pPotSprites) => pPotSprites.AddRange((IEnumerable<Sprite>) this._cached_sprites_time_and_death));
    this.add(pAsset16);
    CommunicationAsset pAsset17 = new CommunicationAsset();
    pAsset17.id = "world_subspecies";
    pAsset17.rate = 0.1f;
    pAsset17.check = (TopicCheck) (_ => World.world.subspecies.hasAny());
    pAsset17.pot_fill = (TopicPotFill) ((_, pPotSprites) => pPotSprites.AddRange((IEnumerable<Sprite>) this._cached_sprites_general_topics));
    this.add(pAsset17);
    CommunicationAsset pAsset18 = new CommunicationAsset();
    pAsset18.id = "general_topics";
    pAsset18.rate = 1f;
    pAsset18.check = (TopicCheck) (_ => true);
    pAsset18.pot_fill = (TopicPotFill) ((_, pPotSprites) => pPotSprites.AddRange((IEnumerable<Sprite>) this._cached_sprites_general_topics));
    this.add(pAsset18);
  }

  public override void linkAssets()
  {
    this.cacheSpritesGeneralTopics();
    base.linkAssets();
  }

  public Sprite getTopicSprite(Actor pActor)
  {
    using (ListPool<Sprite> listPool = new ListPool<Sprite>())
    {
      this.list.Shuffle<CommunicationAsset>();
      foreach (CommunicationAsset communicationAsset in this.list)
      {
        if (Randy.randomChance(communicationAsset.rate) && communicationAsset.check(pActor))
        {
          communicationAsset.pot_fill(pActor, listPool);
          if (listPool.Count > 10)
            break;
        }
      }
      return listPool.GetRandom<Sprite>();
    }
  }

  private void cacheSpritesGeneralTopics()
  {
    this._cached_sprites_housed.Add(SpriteTextureLoader.getSprite("ui/Icons/iconHoused"));
    this._cached_sprites_homeless.Add(SpriteTextureLoader.getSprite("ui/Icons/iconHomeless"));
    this._cached_sprites_religion.Add(SpriteTextureLoader.getSprite("ui/Icons/iconReligion"));
    this._cached_sprites_religion.Add(SpriteTextureLoader.getSprite("ui/Icons/iconReligionList"));
    this._cached_sprites_culture.Add(SpriteTextureLoader.getSprite("ui/Icons/iconCulture"));
    this._cached_sprites_culture.Add(SpriteTextureLoader.getSprite("ui/Icons/iconCultureList"));
    this._cached_sprites_family.Add(SpriteTextureLoader.getSprite("ui/Icons/iconFamily"));
    this._cached_sprites_family.Add(SpriteTextureLoader.getSprite("ui/Icons/iconFamilyList"));
    this._cached_sprites_family.Add(SpriteTextureLoader.getSprite("ui/Icons/iconChildren"));
    this._cached_sprites_kingdom.Add(SpriteTextureLoader.getSprite("ui/Icons/iconKingdom"));
    this._cached_sprites_kingdom.Add(SpriteTextureLoader.getSprite("ui/Icons/iconKingdomList"));
    this._cached_sprites_kingdom.Add(SpriteTextureLoader.getSprite("ui/Icons/iconRebellion"));
    this._cached_sprites_kingdom.Add(SpriteTextureLoader.getSprite("ui/Icons/iconKings"));
    this._cached_sprites_city.Add(SpriteTextureLoader.getSprite("ui/Icons/iconCity"));
    this._cached_sprites_city.Add(SpriteTextureLoader.getSprite("ui/Icons/iconCityList"));
    this._cached_sprites_city.Add(SpriteTextureLoader.getSprite("ui/Icons/iconLeaders"));
    this._cached_sprites_clan.Add(SpriteTextureLoader.getSprite("ui/Icons/iconClan"));
    this._cached_sprites_clan.Add(SpriteTextureLoader.getSprite("ui/Icons/iconClanList"));
    this._cached_sprites_time_and_death.Add(SpriteTextureLoader.getSprite("ui/Icons/iconClock"));
    this._cached_sprites_time_and_death.Add(SpriteTextureLoader.getSprite("ui/Icons/iconDead"));
    this._cached_sprites_time_and_death.Add(SpriteTextureLoader.getSprite("ui/Icons/iconSkulls"));
    this._cached_sprites_time_and_death.Add(SpriteTextureLoader.getSprite("ui/Icons/iconKills"));
    this._cached_sprites_time_and_death.Add(SpriteTextureLoader.getSprite("ui/Icons/iconAge"));
    this._cached_sprites_time_and_death.Add(SpriteTextureLoader.getSprite("ui/Icons/iconRenown"));
    this._cached_sprites_general_topics.Add(SpriteTextureLoader.getSprite("ui/Icons/iconGodFinger"));
    this._cached_sprites_general_topics.Add(SpriteTextureLoader.getSprite("ui/Icons/iconBre"));
    this._cached_sprites_boats_water.Add(SpriteTextureLoader.getSprite("ui/Icons/iconBoat"));
    this._cached_sprites_boats_water.Add(SpriteTextureLoader.getSprite("ui/Icons/iconTileDeepOcean"));
    this._cached_sprites_boats_water.Add(SpriteTextureLoader.getSprite("ui/Icons/iconTileCloseOcean"));
  }
}
