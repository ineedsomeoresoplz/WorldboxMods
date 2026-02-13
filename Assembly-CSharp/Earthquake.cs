// Decompiled with JetBrains decompiler
// Type: Earthquake
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Earthquake : BaseMapObject
{
  private PrintTemplate _current_print;
  private int _print_tick;
  private WorldTile _print_tile_origin;
  private float _timer;
  private const float INTERVAL = 0.05f;
  private bool _quake_active;
  private EarthquakeType _type;
  private int _current_print_index;
  private static Earthquake _instance;

  public void Awake() => Earthquake._instance = this;

  public static void startQuake(WorldTile pTile, EarthquakeType pType = EarthquakeType.RandomPower)
  {
    Earthquake._instance.spawnQuake(pTile, pType);
  }

  private void spawnQuake(WorldTile pTile, EarthquakeType pType)
  {
    if (Earthquake.isQuakeActive())
      return;
    MusicBox.playSound("event:/SFX/NATURE/EarthQuake", pTile);
    this._type = pType;
    this._quake_active = true;
    List<PrintTemplate> quakes = PrintLibrary.getQuakes();
    ++this._current_print_index;
    if (this._current_print_index >= quakes.Count)
    {
      quakes.Shuffle<PrintTemplate>();
      this._current_print_index = 0;
    }
    this._current_print = quakes[this._current_print_index];
    this._current_print.steps.Shuffle<PrintStep>();
    this._print_tile_origin = pTile;
    this._print_tick = 0;
    if (pType != EarthquakeType.RandomPower)
      return;
    if (Randy.randomChance(0.5f))
      this._type = EarthquakeType.BigDecrease;
    else
      this._type = EarthquakeType.BigIncrease;
  }

  private void Update()
  {
    if (!Earthquake.isQuakeActive())
      return;
    if ((double) this._timer > 0.0)
    {
      this._timer -= World.world.elapsed;
    }
    else
    {
      this._timer = 0.05f;
      for (int index = 0; index < 300; ++index)
      {
        if (this._print_tick >= this._current_print.steps.Length)
        {
          this.endQuake();
          break;
        }
        PrintStep step = this._current_print.steps[this._print_tick];
        ++this._print_tick;
        MapBox world = World.world;
        Vector2Int pos1 = this._print_tile_origin.pos;
        int pX = ((Vector2Int) ref pos1).x + step.x;
        Vector2Int pos2 = this._print_tile_origin.pos;
        int pY = ((Vector2Int) ref pos2).y + step.y;
        WorldTile tile = world.GetTile(pX, pY);
        if (tile != null)
        {
          this.tileAction(tile);
          if (this._print_tick >= this._current_print.steps.Length)
          {
            this.endQuake();
            break;
          }
        }
      }
      World.world.startShake(pIntensity: 0.23f, pShakeX: true);
    }
  }

  private void tileAction(WorldTile pTile)
  {
    if (MapAction.checkTileDamageGaiaCovenant(pTile, true))
    {
      switch (this._type)
      {
        case EarthquakeType.BigIncrease:
          MapAction.increaseTile(pTile, true, "earthquake");
          break;
        case EarthquakeType.BigDecrease:
          MapAction.decreaseTile(pTile, true, "earthquake");
          break;
        case EarthquakeType.SmallDisaster:
          MapAction.terraformMain(pTile, pTile.main_type, AssetManager.terraform.get("earthquake_disaster"));
          break;
      }
    }
    pTile.removeBurn();
    pTile.doUnits(new Action<Actor>(this.unitAction));
  }

  private void unitAction(Actor pActor)
  {
    if (Randy.randomBool())
      pActor.makeConfused();
    else
      pActor.applyRandomForce();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool isQuakeActive() => Earthquake._instance._quake_active;

  private void endQuake() => this._quake_active = false;
}
