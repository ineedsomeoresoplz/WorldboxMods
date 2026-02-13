// Decompiled with JetBrains decompiler
// Type: IMetaObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public interface IMetaObject : ICoreObject
{
  int getMaxPossibleLifespan()
  {
    int num1 = 0;
    int num2 = 0;
    foreach (BaseSimObject unit in this.getUnits())
    {
      int stat = (int) unit.stats["lifespan"];
      if (stat > num2)
        num2 = stat;
      ++num1;
    }
    return num1 == 0 ? 100 : num2;
  }

  float getRatioAdults()
  {
    int num1 = 0;
    float num2 = 0.0f;
    foreach (Actor unit in this.getUnits())
    {
      if (unit.isAdult())
        ++num2;
      ++num1;
    }
    return num1 <= 0 ? 0.0f : num2 / (float) num1;
  }

  float getRatioMales()
  {
    int num1 = 0;
    float num2 = 0.0f;
    foreach (Actor unit in this.getUnits())
    {
      if (unit.isSexMale())
        ++num2;
      ++num1;
    }
    return num1 <= 0 ? 0.0f : num2 / (float) num1;
  }

  float getRatioFemales()
  {
    int num1 = 0;
    float num2 = 0.0f;
    foreach (Actor unit in this.getUnits())
    {
      if (unit.isSexFemale())
        ++num2;
      ++num1;
    }
    return num1 <= 0 ? 0.0f : num2 / (float) num1;
  }

  float getRatioChildren()
  {
    int num1 = 0;
    float num2 = 0.0f;
    foreach (Actor unit in this.getUnits())
    {
      if (!unit.isAdult())
        ++num2;
      ++num1;
    }
    return num1 <= 0 ? 0.0f : num2 / (float) num1;
  }

  float getRatioHoused()
  {
    int num1 = 0;
    float num2 = 0.0f;
    foreach (Actor unit in this.getUnits())
    {
      if (unit.hasHouse())
        ++num2;
      ++num1;
    }
    return num1 <= 0 ? 0.0f : num2 / (float) num1;
  }

  float getRatioHomeless()
  {
    int num1 = 0;
    float num2 = 0.0f;
    foreach (Actor unit in this.getUnits())
    {
      if (!unit.hasHouse())
        ++num2;
      ++num1;
    }
    return num1 <= 0 ? 0.0f : num2 / (float) num1;
  }

  float getRatioHungry()
  {
    int num1 = 0;
    float num2 = 0.0f;
    foreach (Actor unit in this.getUnits())
    {
      if (unit.isHungry())
        ++num2;
      ++num1;
    }
    return num1 <= 0 ? 0.0f : num2 / (float) num1;
  }

  float getRatioStarving()
  {
    int num1 = 0;
    float num2 = 0.0f;
    foreach (Actor unit in this.getUnits())
    {
      if (unit.isStarving())
        ++num2;
      ++num1;
    }
    return num1 <= 0 ? 0.0f : num2 / (float) num1;
  }

  float getRatioSick()
  {
    int num1 = 0;
    float num2 = 0.0f;
    foreach (Actor unit in this.getUnits())
    {
      if (unit.isSick())
        ++num2;
      ++num1;
    }
    return num1 <= 0 ? 0.0f : num2 / (float) num1;
  }

  float getRatioHappy()
  {
    int num1 = 0;
    float num2 = 0.0f;
    foreach (Actor unit in this.getUnits())
    {
      if (unit.isHappy())
        ++num2;
      ++num1;
    }
    return num1 <= 0 ? 0.0f : num2 / (float) num1;
  }

  float getRatioUnhappy()
  {
    int num1 = 0;
    float num2 = 0.0f;
    foreach (Actor unit in this.getUnits())
    {
      if (unit.isUnhappy())
        ++num2;
      ++num1;
    }
    return num1 <= 0 ? 0.0f : num2 / (float) num1;
  }

  MetaTypeAsset getMetaTypeAsset();

  bool hasUnits();

  int countUnits();

  IEnumerable<Actor> getUnits();

  Actor getRandomUnit();

  Actor getRandomActorForReaper();

  int countFamilies();

  IEnumerable<Family> getFamilies();

  bool hasFamilies();

  ActorAsset getActorAsset();

  Sprite getSpriteIcon();

  bool isCursorOver();

  void setCursorOver();

  ColorAsset getColor();

  MetaObjectData getMetaData();

  int getRenown();

  int getPopulationPeople();

  long getTotalKills();

  long getTotalDeaths();

  bool isSelected();

  Actor getOldestVisibleUnit();

  Actor getOldestVisibleUnitForNameplatesCached();

  bool hasCities();

  IEnumerable<City> getCities();

  bool hasKingdoms();

  IEnumerable<Kingdom> getKingdoms();

  bool hasDied();
}
