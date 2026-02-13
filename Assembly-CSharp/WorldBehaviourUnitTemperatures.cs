// Decompiled with JetBrains decompiler
// Type: WorldBehaviourUnitTemperatures
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class WorldBehaviourUnitTemperatures
{
  public static Dictionary<Actor, int> temperatures = new Dictionary<Actor, int>();
  private static List<TemperatureMod> _temperature_updaters = new List<TemperatureMod>();

  public static void updateUnitTemperatures()
  {
    if (WorldBehaviourUnitTemperatures.temperatures.Count == 0)
      return;
    WorldBehaviourUnitTemperatures._temperature_updaters.Clear();
    bool flag1 = true;
    bool flag2 = false;
    foreach (Actor key in WorldBehaviourUnitTemperatures.temperatures.Keys)
    {
      if (key.isRekt())
      {
        flag2 = true;
      }
      else
      {
        flag1 = false;
        int temperature = WorldBehaviourUnitTemperatures.temperatures[key];
        int num = temperature;
        int pNewTemperature;
        if (num > 0)
        {
          pNewTemperature = num - 1;
          if (pNewTemperature < 0)
            pNewTemperature = 0;
        }
        else
        {
          pNewTemperature = num + 1;
          if (pNewTemperature > 0)
            pNewTemperature = 0;
        }
        if (pNewTemperature != temperature)
          WorldBehaviourUnitTemperatures._temperature_updaters.Add(new TemperatureMod(key, pNewTemperature));
      }
    }
    if (flag1)
      WorldBehaviourUnitTemperatures.temperatures.Clear();
    else if (WorldBehaviourUnitTemperatures._temperature_updaters.Count > 0)
    {
      for (int index = 0; index < WorldBehaviourUnitTemperatures._temperature_updaters.Count; ++index)
      {
        TemperatureMod temperatureUpdater = WorldBehaviourUnitTemperatures._temperature_updaters[index];
        if (temperatureUpdater.new_temperature == 0)
          WorldBehaviourUnitTemperatures.temperatures.Remove(temperatureUpdater.actor);
        else
          WorldBehaviourUnitTemperatures.temperatures[temperatureUpdater.actor] = temperatureUpdater.new_temperature;
      }
    }
    if (flag2)
      WorldBehaviourUnitTemperatures.temperatures.RemoveByKey<Actor, int>((Predicate<Actor>) (tActor => tActor.isRekt()));
    WorldBehaviourUnitTemperatures._temperature_updaters.Clear();
  }

  private static void changeUnitTemperature(Actor pActor, int pTemperatureChangeSpeed)
  {
    if (!WorldBehaviourUnitTemperatures.temperatures.ContainsKey(pActor))
      WorldBehaviourUnitTemperatures.temperatures.Add(pActor, 0);
    WorldBehaviourUnitTemperatures.temperatures[pActor] += pTemperatureChangeSpeed;
    float temperature = (float) WorldBehaviourUnitTemperatures.temperatures[pActor];
    if (pTemperatureChangeSpeed < 0)
    {
      pActor.finishStatusEffect("burning");
      if ((double) temperature >= -200.0)
        return;
      pActor.addStatusEffect("frozen");
      WorldBehaviourUnitTemperatures.temperatures[pActor] = 0;
    }
    else
    {
      pActor.finishStatusEffect("frozen");
      if ((double) temperature <= 300.0)
        return;
      pActor.addStatusEffect("burning");
      WorldBehaviourUnitTemperatures.temperatures[pActor] = 0;
    }
  }

  public static void checkTile(WorldTile pTile, int pTemperatureChangeSpeed)
  {
    pTile.doUnits((Action<Actor>) (tActor => WorldBehaviourUnitTemperatures.changeUnitTemperature(tActor, pTemperatureChangeSpeed)));
  }

  public static void clear()
  {
    WorldBehaviourUnitTemperatures.temperatures.Clear();
    WorldBehaviourUnitTemperatures._temperature_updaters.Clear();
  }

  public static void removeUnit(Actor pActor)
  {
    WorldBehaviourUnitTemperatures.temperatures.Remove(pActor);
  }

  public static void debug(DebugTool pTool)
  {
    pTool.setText("units", (object) WorldBehaviourUnitTemperatures.temperatures.Count);
    int num = 5;
    foreach (Actor key in WorldBehaviourUnitTemperatures.temperatures.Keys)
    {
      if (key.isAlive())
      {
        pTool.setText(": " + key.getName(), (object) WorldBehaviourUnitTemperatures.temperatures[key].ToText());
        --num;
        if (num == 0)
          break;
      }
    }
  }
}
