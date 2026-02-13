// Decompiled with JetBrains decompiler
// Type: FavoriteItemListComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class FavoriteItemListComponent : 
  ComponentListBase<FavoriteItemListElement, Item, ItemData, FavoriteItemListComponent>
{
  private List<NanoObject> _meta_objects = new List<NanoObject>();

  protected override MetaType meta_type => MetaType.Item;

  protected override void setupSortingTabs()
  {
    this.sorting_tab.tryAddButton("ui/Icons/iconAge", "sort_by_age", new SortButtonAction(((ComponentListBase<FavoriteItemListElement, Item, ItemData, FavoriteItemListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Item>(FavoriteItemListComponent.sortByAge)));
    this.sorting_tab.tryAddButton("ui/Icons/iconKills", "sort_by_kills", new SortButtonAction(((ComponentListBase<FavoriteItemListElement, Item, ItemData, FavoriteItemListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Item>(FavoriteItemListComponent.sortByKills)));
    this.sorting_tab.tryAddButton("ui/Icons/iconDamage", "sort_by_damage", new SortButtonAction(((ComponentListBase<FavoriteItemListElement, Item, ItemData, FavoriteItemListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Item>(FavoriteItemListComponent.sortByDamage)));
    this.sorting_tab.tryAddButton("ui/Icons/iconArmor", "sort_by_armor", new SortButtonAction(((ComponentListBase<FavoriteItemListElement, Item, ItemData, FavoriteItemListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Item>(FavoriteItemListComponent.sortByArmor)));
    this.sorting_tab.tryAddButton("ui/Icons/iconItemType", "sort_by_type", new SortButtonAction(((ComponentListBase<FavoriteItemListElement, Item, ItemData, FavoriteItemListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Item>(FavoriteItemListComponent.sortByType)));
    this.sorting_tab.tryAddButton("ui/Icons/iconItemQuality", "sort_by_quality", new SortButtonAction(((ComponentListBase<FavoriteItemListElement, Item, ItemData, FavoriteItemListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Item>(FavoriteItemListComponent.sortByQuality)));
    this.sorting_tab.tryAddButton("ui/Icons/iconCity", "sort_by_city", new SortButtonAction(((ComponentListBase<FavoriteItemListElement, Item, ItemData, FavoriteItemListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Item>(FavoriteItemListComponent.sortByCity)));
    this.sorting_tab.tryAddButton("ui/Icons/iconHumans", "sort_by_owner", new SortButtonAction(((ComponentListBase<FavoriteItemListElement, Item, ItemData, FavoriteItemListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Item>(FavoriteItemListComponent.sortByOwner)));
  }

  protected override IEnumerable<Item> getObjectsList()
  {
    this._meta_objects.Clear();
    foreach (Item pObject in (CoreSystemManager<Item, ItemData>) World.world.items)
    {
      if (!pObject.isRekt() && pObject.isFavorite())
      {
        this._meta_objects.Add((NanoObject) pObject);
        if (pObject.hasCity())
          this._meta_objects.Add((NanoObject) pObject.getCity());
        if (pObject.hasActor())
          this._meta_objects.Add((NanoObject) pObject.getActor());
        yield return pObject;
      }
    }
  }

  public static int sortByAge(Item pItem1, Item pItem2)
  {
    return -pItem2.data.created_time.CompareTo(pItem1.data.created_time);
  }

  public static int sortByKills(Item pItem1, Item pItem2)
  {
    return pItem2.data.kills.CompareTo(pItem1.data.kills);
  }

  public static int sortByType(Item pItem1, Item pItem2)
  {
    return pItem2.getAsset().equipment_type.CompareTo((object) pItem1.getAsset().equipment_type);
  }

  public static int sortByQuality(Item pItem1, Item pItem2)
  {
    return pItem2.getQuality().CompareTo((object) pItem1.getQuality());
  }

  public static int sortByCity(Item pItem1, Item pItem2)
  {
    int num1 = pItem1.hasCity().CompareTo(pItem2.hasCity());
    if (num1 != 0)
      return num1;
    if (!pItem1.hasCity() || !pItem2.hasCity())
      return pItem2.name.CompareTo(pItem1.name);
    int num2 = pItem2.getCity().kingdom.CompareTo((NanoObject) pItem1.getCity().kingdom);
    return num2 != 0 ? num2 : pItem2.getCity().name.CompareTo(pItem1.getCity().name);
  }

  public static int sortByOwner(Item pItem1, Item pItem2)
  {
    int num1 = pItem1.hasActor().CompareTo(pItem2.hasActor());
    if (num1 != 0)
      return num1;
    if (!pItem1.hasActor() || !pItem2.hasActor())
      return pItem2.name.CompareTo(pItem1.name);
    Actor actor1 = pItem1.getActor();
    Actor actor2 = pItem2.getActor();
    int num2 = actor1.kingdom.CompareTo((NanoObject) actor2.kingdom);
    if (num2 != 0)
      return num2;
    int num3 = actor1.hasCity().CompareTo(actor2.hasCity());
    if (num3 != 0)
      return num3;
    if (actor1.hasCity() && actor2.hasCity())
    {
      int num4 = actor1.getCity().name.CompareTo(actor2.getCity().name);
      if (num4 != 0)
        return num4;
    }
    return pItem2.getActor().name.CompareTo(pItem1.getActor().name);
  }

  public static int sortByDamage(Item pItem1, Item pItem2)
  {
    return pItem2.getFullStats()["damage"].CompareTo(pItem1.getFullStats()["damage"]);
  }

  public static int sortByArmor(Item pItem1, Item pItem2)
  {
    return pItem2.getFullStats()["armor"].CompareTo(pItem1.getFullStats()["armor"]);
  }

  public override void clear()
  {
    base.clear();
    this._meta_objects.Clear();
  }

  public override bool checkRefreshWindow()
  {
    foreach (NanoObject metaObject in this._meta_objects)
    {
      if (metaObject.isRekt())
        return true;
    }
    return base.checkRefreshWindow();
  }
}
