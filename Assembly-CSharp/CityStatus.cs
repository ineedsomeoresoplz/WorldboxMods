// Decompiled with JetBrains decompiler
// Type: CityStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class CityStatus
{
  public int population_adults;
  public int population_children;
  public int population;
  public int housing_total;
  public int housing_free;
  public int housing_occupied;
  public int houses_max;
  public int warrior_slots;
  public int warriors_current;
  public int hungry;
  public int maximum_items;
  public int sick;
  public int homeless;
  public int housed;
  public int males;
  public int females;
  public int families;

  public void clear()
  {
    this.population_adults = 0;
    this.population_children = 0;
    this.population = 0;
    this.housing_total = 0;
    this.housing_free = 0;
    this.housing_occupied = 0;
    this.houses_max = 0;
    this.warrior_slots = 0;
    this.warriors_current = 0;
    this.hungry = 0;
    this.maximum_items = 0;
    this.sick = 0;
    this.homeless = 0;
    this.housed = 0;
    this.males = 0;
    this.females = 0;
    this.families = 0;
  }
}
