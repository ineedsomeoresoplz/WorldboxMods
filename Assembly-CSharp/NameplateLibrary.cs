// Decompiled with JetBrains decompiler
// Type: NameplateLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class NameplateLibrary : AssetLibrary<NameplateAsset>
{
  public readonly Dictionary<MetaType, NameplateAsset> map_modes_nameplates = new Dictionary<MetaType, NameplateAsset>();
  private NameplateAsset _plate_kingdom;
  private NameplateAsset _plate_city;
  private const int OFFSET_UNIT_Y = -2;

  public override void init()
  {
    base.init();
    NameplateAsset pAsset1 = new NameplateAsset();
    pAsset1.id = "plate_subspecies";
    pAsset1.path_sprite = "ui/nameplates/nameplate_subspecies";
    pAsset1.padding_left = 11;
    pAsset1.padding_right = 13;
    pAsset1.banner_only_mode_scale = 1.8f;
    pAsset1.map_mode = MetaType.Subspecies;
    pAsset1.overlap_for_fluid_mode = true;
    pAsset1.action_main = new NameplateBase(this.actionSubspecies);
    this.add(pAsset1);
    NameplateAsset pAsset2 = new NameplateAsset();
    pAsset2.id = "plate_army";
    pAsset2.path_sprite = "ui/nameplates/nameplate_army";
    pAsset2.padding_left = 26;
    pAsset2.padding_right = 18;
    pAsset2.padding_top = -2;
    pAsset2.banner_only_mode_scale = 1.5f;
    pAsset2.map_mode = MetaType.Army;
    pAsset2.action_main = new NameplateBase(this.actionArmy);
    this.add(pAsset2);
    NameplateAsset pAsset3 = new NameplateAsset();
    pAsset3.id = "plate_family";
    pAsset3.path_sprite = "ui/nameplates/nameplate_family";
    pAsset3.padding_left = 11;
    pAsset3.padding_right = 13;
    pAsset3.banner_only_mode_scale = 1.5f;
    pAsset3.map_mode = MetaType.Family;
    pAsset3.overlap_for_fluid_mode = true;
    pAsset3.action_main = new NameplateBase(this.actionFamily);
    this.add(pAsset3);
    NameplateAsset pAsset4 = new NameplateAsset();
    pAsset4.id = "plate_religion";
    pAsset4.path_sprite = "ui/nameplates/nameplate_religion";
    pAsset4.padding_left = 11;
    pAsset4.padding_right = 13;
    pAsset4.map_mode = MetaType.Religion;
    pAsset4.action_main = new NameplateBase(this.actionReligion);
    this.add(pAsset4);
    NameplateAsset pAsset5 = new NameplateAsset();
    pAsset5.id = "plate_culture";
    pAsset5.path_sprite = "ui/nameplates/nameplate_culture";
    pAsset5.padding_left = 11;
    pAsset5.padding_right = 13;
    pAsset5.map_mode = MetaType.Culture;
    pAsset5.action_main = new NameplateBase(this.actionCulture);
    this.add(pAsset5);
    NameplateAsset pAsset6 = new NameplateAsset();
    pAsset6.id = "plate_language";
    pAsset6.path_sprite = "ui/nameplates/nameplate_language";
    pAsset6.padding_left = 11;
    pAsset6.padding_right = 13;
    pAsset6.map_mode = MetaType.Language;
    pAsset6.action_main = new NameplateBase(this.actionLanguage);
    this.add(pAsset6);
    NameplateAsset pAsset7 = new NameplateAsset();
    pAsset7.id = "plate_alliance";
    pAsset7.path_sprite = "ui/nameplates/nameplate_alliance";
    pAsset7.map_mode = MetaType.Alliance;
    pAsset7.banner_only_mode_scale = 3f;
    pAsset7.padding_left = 14;
    pAsset7.padding_top = 2;
    pAsset7.action_main = new NameplateBase(this.actionAlliance);
    this.add(pAsset7);
    NameplateAsset pAsset8 = new NameplateAsset();
    pAsset8.id = "plate_kingdom";
    pAsset8.path_sprite = "ui/nameplates/nameplate_kingdom";
    pAsset8.padding_left = 26;
    pAsset8.padding_right = 26;
    pAsset8.padding_top = -2;
    pAsset8.banner_only_mode_scale = 2.5f;
    pAsset8.map_mode = MetaType.Kingdom;
    pAsset8.action_main = new NameplateBase(this.actionKingdom);
    this._plate_kingdom = this.add(pAsset8);
    NameplateAsset pAsset9 = new NameplateAsset();
    pAsset9.id = "plate_city";
    pAsset9.path_sprite = "ui/nameplates/nameplate_city";
    pAsset9.map_mode = MetaType.City;
    pAsset9.banner_only_mode_scale = 2.5f;
    pAsset9.padding_left = 6;
    pAsset9.padding_right = 7;
    pAsset9.padding_top = -2;
    pAsset9.action_main = new NameplateBase(this.actionCity);
    this._plate_city = this.add(pAsset9);
    NameplateAsset pAsset10 = new NameplateAsset();
    pAsset10.id = "plate_clan";
    pAsset10.path_sprite = "ui/nameplates/nameplate_clan";
    pAsset10.map_mode = MetaType.Clan;
    pAsset10.padding_left = 17;
    pAsset10.padding_right = 24;
    pAsset10.action_main = new NameplateBase(this.actionClan);
    this.add(pAsset10);
  }

  private bool isWithinCamera(Vector2 pVector)
  {
    return World.world.move_camera.isWithinCameraViewNotPowerBar(pVector);
  }

  public override NameplateAsset add(NameplateAsset pAsset)
  {
    this.map_modes_nameplates.Add(pAsset.map_mode, pAsset);
    return base.add(pAsset);
  }

  private void actionAlliance(NameplateManager pManager, NameplateAsset pAsset)
  {
    int num = 0;
    foreach (Alliance alliance in (CoreSystemManager<Alliance, AllianceData>) World.world.alliances)
    {
      City pCity = (City) null;
      Kingdom kingdom1 = (Kingdom) null;
      foreach (Kingdom kingdom2 in alliance.kingdoms_hashset)
      {
        if (kingdom2.hasCapital() && this.isWithinCamera(kingdom2.capital.city_center) && (kingdom1 == null || kingdom1.power < kingdom2.power))
          kingdom1 = kingdom2;
      }
      if (kingdom1 != null && kingdom1.hasCapital())
        pCity = kingdom1.capital;
      if (pCity != null)
        pManager.prepareNext(pAsset, (NanoObject) alliance).showTextAlliance(alliance, pCity);
    }
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
    {
      if (kingdom.hasCapital() && !kingdom.hasAlliance() && this.isWithinCamera(kingdom.capital.city_center))
      {
        if (num >= pAsset.max_nameplate_count)
          break;
        ++num;
        pManager.prepareNext(this._plate_kingdom, (NanoObject) kingdom).showTextKingdom(kingdom, kingdom.capital.city_center);
      }
    }
  }

  private void actionReligion(NameplateManager pManager, NameplateAsset pAsset)
  {
    int num = 0;
    switch (MetaTypeLibrary.religion.getZoneOptionState())
    {
      case 0:
        using (IEnumerator<Kingdom> enumerator = World.world.kingdoms.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Kingdom current = enumerator.Current;
            if (current.hasReligion() && current.hasCapital() && this.isWithinCamera(current.capital.city_center))
              pManager.prepareNext(pAsset, (NanoObject) current.religion).showTextReligion(current.religion, Vector2.op_Implicit(current.capital.city_center));
          }
          break;
        }
      case 1:
        using (IEnumerator<City> enumerator = World.world.cities.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            City current = enumerator.Current;
            if (current.hasReligion())
            {
              Religion religion = current.getReligion();
              if (this.isWithinCamera(current.city_center))
                pManager.prepareNext(pAsset, (NanoObject) religion).showTextReligion(religion, Vector2.op_Implicit(current.city_center));
            }
          }
          break;
        }
      default:
        using (IEnumerator<Religion> enumerator = World.world.religions.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Religion current = enumerator.Current;
            if (num >= pAsset.max_nameplate_count)
              break;
            Vector3 pPosition;
            if (this.getPositionForMeta((IMetaObject) current, out pPosition))
            {
              pManager.prepareNext(pAsset, (NanoObject) current).showTextReligion(current, pPosition);
              ++num;
            }
          }
          break;
        }
    }
  }

  private void actionLanguage(NameplateManager pManager, NameplateAsset pAsset)
  {
    int num = 0;
    switch (MetaTypeLibrary.language.getZoneOptionState())
    {
      case 0:
        using (IEnumerator<Kingdom> enumerator = World.world.kingdoms.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Kingdom current = enumerator.Current;
            if (current.hasLanguage() && current.hasCapital() && this.isWithinCamera(current.capital.city_center))
              pManager.prepareNext(pAsset, (NanoObject) current.language).showTextLanguage(current.language, Vector2.op_Implicit(current.capital.city_center));
          }
          break;
        }
      case 1:
        using (IEnumerator<City> enumerator = World.world.cities.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            City current = enumerator.Current;
            if (current.hasLanguage())
            {
              Language language = current.getLanguage();
              if (this.isWithinCamera(current.city_center))
                pManager.prepareNext(pAsset, (NanoObject) language).showTextLanguage(language, Vector2.op_Implicit(current.city_center));
            }
          }
          break;
        }
      default:
        using (IEnumerator<Language> enumerator = World.world.languages.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Language current = enumerator.Current;
            if (num >= pAsset.max_nameplate_count)
              break;
            Vector3 pPosition;
            if (this.getPositionForMeta((IMetaObject) current, out pPosition))
            {
              pManager.prepareNext(pAsset, (NanoObject) current).showTextLanguage(current, pPosition);
              ++num;
            }
          }
          break;
        }
    }
  }

  private void actionCulture(NameplateManager pManager, NameplateAsset pAsset)
  {
    int num = 0;
    switch (MetaTypeLibrary.culture.getZoneOptionState())
    {
      case 0:
        using (IEnumerator<Kingdom> enumerator = World.world.kingdoms.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Kingdom current = enumerator.Current;
            if (current.hasCulture() && current.hasCapital() && this.isWithinCamera(current.capital.city_center))
              pManager.prepareNext(pAsset, (NanoObject) current.culture).showTextCulture(current.culture, Vector2.op_Implicit(current.capital.city_center));
          }
          break;
        }
      case 1:
        using (IEnumerator<City> enumerator = World.world.cities.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            City current = enumerator.Current;
            if (current.hasCulture())
            {
              Culture culture = current.getCulture();
              if (this.isWithinCamera(current.city_center))
                pManager.prepareNext(pAsset, (NanoObject) culture).showTextCulture(culture, Vector2.op_Implicit(current.city_center));
            }
          }
          break;
        }
      default:
        using (IEnumerator<Culture> enumerator = World.world.cultures.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Culture current = enumerator.Current;
            if (num >= pAsset.max_nameplate_count)
              break;
            Vector3 pPosition;
            if (this.getPositionForMeta((IMetaObject) current, out pPosition))
            {
              pManager.prepareNext(pAsset, (NanoObject) current).showTextCulture(current, pPosition);
              ++num;
            }
          }
          break;
        }
    }
  }

  private void actionCity(NameplateManager pManager, NameplateAsset pAsset)
  {
    // ISSUE: unable to decompile the method.
  }

  private void actionKingdom(NameplateManager pManager, NameplateAsset pAsset)
  {
    int num = 0;
    if (MetaTypeLibrary.kingdom.getZoneOptionState() == 0)
    {
      foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
      {
        if (kingdom.hasCapital() && this.isWithinCamera(kingdom.capital.city_center))
          pManager.prepareNext(pAsset, (NanoObject) kingdom).showTextKingdom(kingdom, kingdom.capital.city_center);
      }
    }
    else
    {
      foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
      {
        if (num >= pAsset.max_nameplate_count)
          break;
        Actor pForceActor = (Actor) null;
        if (kingdom.hasKing() && !kingdom.king.isRekt() && kingdom.king.is_visible)
          pForceActor = kingdom.king;
        Vector3 pPosition;
        if (this.getPositionForMeta((IMetaObject) kingdom, out pPosition, pForceActor))
        {
          pManager.prepareNext(pAsset, (NanoObject) kingdom).showTextKingdom(kingdom, Vector2.op_Implicit(pPosition));
          ++num;
        }
      }
    }
  }

  private void actionSubspecies(NameplateManager pManager, NameplateAsset pAsset)
  {
    int num = 0;
    switch (MetaTypeLibrary.subspecies.getZoneOptionState())
    {
      case 0:
        using (IEnumerator<Kingdom> enumerator = World.world.kingdoms.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Kingdom current = enumerator.Current;
            Subspecies mainSubspecies = current.getMainSubspecies();
            if (!mainSubspecies.isRekt() && current.hasCapital() && this.isWithinCamera(current.capital.city_center))
              pManager.prepareNext(pAsset, (NanoObject) mainSubspecies).showTextSubspecies(mainSubspecies, Vector2.op_Implicit(current.capital.city_center));
          }
          break;
        }
      case 1:
        using (IEnumerator<City> enumerator = World.world.cities.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            City current = enumerator.Current;
            Subspecies mainSubspecies = current.getMainSubspecies();
            if (!mainSubspecies.isRekt() && this.isWithinCamera(current.city_center))
              pManager.prepareNext(pAsset, (NanoObject) mainSubspecies).showTextSubspecies(mainSubspecies, Vector2.op_Implicit(current.city_center));
          }
          break;
        }
      default:
        using (IEnumerator<Subspecies> enumerator = World.world.subspecies.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Subspecies current = enumerator.Current;
            if (num >= pAsset.max_nameplate_count)
              break;
            Vector3 pPosition;
            if (this.getPositionForMeta((IMetaObject) current, out pPosition))
            {
              pManager.prepareNext(pAsset, (NanoObject) current).showTextSubspecies(current, pPosition);
              ++num;
            }
          }
          break;
        }
    }
  }

  private bool getPositionForMeta(
    IMetaObject pMetaObject,
    out Vector3 pPosition,
    Actor pForceActor = null)
  {
    if (!pMetaObject.isAlive() || !pMetaObject.hasUnits())
    {
      pPosition = Vector3.zero;
      return false;
    }
    Actor actor = pForceActor ?? pMetaObject.getOldestVisibleUnitForNameplatesCached();
    if (actor == null)
    {
      pPosition = Vector3.zero;
      return false;
    }
    Vector3 vector3 = Vector2.op_Implicit(actor.current_position);
    vector3.y += actor.getHeight();
    vector3.y += -2f;
    pPosition = vector3;
    return true;
  }

  private int sortByMembers(IMetaObject pObject1, IMetaObject pObject2)
  {
    int num1 = pObject2.isFavorite().CompareTo(pObject1.isFavorite());
    if (num1 != 0)
      return num1;
    int num2 = pObject2.isSelected().CompareTo(pObject1.isSelected());
    return num2 != 0 ? num2 : pObject2.countUnits().CompareTo(pObject1.countUnits());
  }

  private void actionArmy(NameplateManager pManager, NameplateAsset pAsset)
  {
    // ISSUE: unable to decompile the method.
  }

  private void actionFamily(NameplateManager pManager, NameplateAsset pAsset)
  {
    int num = 0;
    switch (MetaTypeLibrary.family.getZoneOptionState())
    {
      case 0:
        using (IEnumerator<Kingdom> enumerator = World.world.kingdoms.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Kingdom current = enumerator.Current;
            if (current.hasCapital() && current.hasKing() && current.king.hasFamily() && this.isWithinCamera(current.capital.city_center))
              pManager.prepareNext(pAsset, (NanoObject) current.king.family).showTextFamily(current.king.family, Vector2.op_Implicit(current.capital.city_center));
          }
          break;
        }
      case 1:
        using (IEnumerator<City> enumerator = World.world.cities.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            City current = enumerator.Current;
            if (current.hasLeader() && current.leader.hasFamily())
            {
              Family family = current.leader.family;
              if (family != null && this.isWithinCamera(current.city_center))
                pManager.prepareNext(pAsset, (NanoObject) family).showTextFamily(family, Vector2.op_Implicit(current.city_center));
            }
          }
          break;
        }
      default:
        using (ListPool<Family> listPool = new ListPool<Family>((ICollection<Family>) World.world.families.list))
        {
          listPool.Sort(new Comparison<Family>(this.sortByMembers));
          using (ListPool<Family>.Enumerator enumerator = listPool.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Family family = enumerator.Current;
              if (num >= pAsset.max_nameplate_count)
                break;
              Vector3 pPosition;
              if (this.getPositionForMeta((IMetaObject) family, out pPosition))
              {
                pManager.prepareNext(pAsset, (NanoObject) family).showTextFamily(family, pPosition);
                ++num;
              }
            }
            break;
          }
        }
    }
  }

  private void actionClan(NameplateManager pManager, NameplateAsset pAsset)
  {
    int num = 0;
    switch (MetaTypeLibrary.clan.getZoneOptionState())
    {
      case 0:
        using (IEnumerator<Kingdom> enumerator = World.world.kingdoms.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Kingdom current = enumerator.Current;
            if (current.hasCapital() && current.hasKing() && current.king.hasClan() && this.isWithinCamera(current.capital.city_center))
              pManager.prepareNext(pAsset, (NanoObject) current.king.clan).showTextClanCity(current.king.clan, current.capital);
          }
          break;
        }
      case 1:
        using (IEnumerator<City> enumerator = World.world.cities.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            City current = enumerator.Current;
            Clan royalClan = current.getRoyalClan();
            if (royalClan != null && this.isWithinCamera(current.city_center))
              pManager.prepareNext(pAsset, (NanoObject) royalClan).showTextClanCity(royalClan, current);
          }
          break;
        }
      default:
        using (IEnumerator<Clan> enumerator = World.world.clans.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Clan current = enumerator.Current;
            if (num >= pAsset.max_nameplate_count)
              break;
            Vector3 pPosition;
            if (this.getPositionForMeta((IMetaObject) current, out pPosition))
            {
              pManager.prepareNext(pAsset, (NanoObject) current).showTextClanFluid(current, pPosition);
              ++num;
            }
          }
          break;
        }
    }
  }
}
