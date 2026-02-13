// Decompiled with JetBrains decompiler
// Type: ExplosionsEffects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ExplosionsEffects : MapLayer
{
  private Dictionary<WorldTile, TileTypeBase> explosionDict;
  private Dictionary<WorldTile, TileTypeBase> explosionDictCurrent;
  public List<WorldTile> explosionQueue;
  private List<WorldTile> explosionQueueCurrent;
  private float timerExplosionQueue;
  public float interval = 0.01f;
  internal Queue<WorldTile> nextWave = new Queue<WorldTile>();
  internal HashSetWorldTile hashset_bombs = new HashSetWorldTile();
  internal List<WorldTile> delayedBombs = new List<WorldTile>();
  internal List<WorldTile> timedBombs = new List<WorldTile>();

  internal override void create()
  {
    this.colorValues = new Color(1f, 1f, 1f, 1f);
    this.colors_amount = 60;
    this.explosionQueue = new List<WorldTile>();
    this.explosionQueueCurrent = new List<WorldTile>();
    this.explosionDict = new Dictionary<WorldTile, TileTypeBase>();
    this.explosionDictCurrent = new Dictionary<WorldTile, TileTypeBase>();
    this.hashsetTiles = new HashSetWorldTile();
    base.create();
  }

  internal override void clear()
  {
    this.explosionQueue.Clear();
    this.explosionQueueCurrent.Clear();
    this.explosionDict.Clear();
    this.explosionDictCurrent.Clear();
    this.hashsetTiles.Clear();
    this.timedBombs.Clear();
    this.delayedBombs.Clear();
    this.nextWave.Clear();
    this.hashset_bombs.Clear();
    base.clear();
  }

  internal void activateDelayedBomb(WorldTile pBomb)
  {
    if (this.delayedBombs.Contains(pBomb))
      return;
    this.delayedBombs.Add(pBomb);
    pBomb.delayed_bomb_type = pBomb.Type.id;
    pBomb.delayed_timer_bomb = 0.09f;
  }

  internal void addTimedTnt(WorldTile pTile)
  {
    if (this.timedBombs.Contains(pTile))
      return;
    pTile.delayed_timer_bomb = 5f;
    this.timedBombs.Add(pTile);
  }

  internal void explodeBomb(WorldTile pBombTile, bool pForce = false)
  {
    // ISSUE: unable to decompile the method.
  }

  public void prepareNewExplosion(WorldTile pTile)
  {
    if (this.explosionDict.ContainsKey(pTile))
      return;
    this.explosionQueue.Add(pTile);
    this.explosionDict.Add(pTile, pTile.Type);
  }

  private void updateExplosionQueue()
  {
    if ((double) this.timerExplosionQueue > 0.0)
    {
      this.timerExplosionQueue -= World.world.elapsed;
    }
    else
    {
      this.timerExplosionQueue = 0.1f;
      if (this.explosionQueue.Count == 0)
        return;
      for (int index = 0; index < this.explosionQueue.Count; ++index)
      {
        WorldTile explosion = this.explosionQueue[index];
        this.explosionQueueCurrent.Add(explosion);
        this.explosionDictCurrent.Add(explosion, this.explosionDict[explosion]);
      }
      this.explosionQueue.Clear();
      this.explosionDict.Clear();
      for (int index = 0; index < this.explosionQueueCurrent.Count; ++index)
      {
        WorldTile worldTile = this.explosionQueueCurrent[index];
        MapAction.damageWorld(worldTile, this.explosionDictCurrent[worldTile].explode_range, AssetManager.terraform.get("bomb"));
        MusicBox.playSound("event:/SFX/EXPLOSIONS/ExplosionMiddle", worldTile);
      }
      this.explosionQueueCurrent.Clear();
      this.explosionDictCurrent.Clear();
    }
  }

  public override void update(float pElapsed)
  {
    this.checkAutoDisable();
    if (this.timedBombs.Count <= 0 || World.world.isPaused())
      return;
    int index = 0;
    while (index < this.timedBombs.Count)
    {
      WorldTile timedBomb = this.timedBombs[index];
      if ((double) timedBomb.delayed_timer_bomb > 0.0)
      {
        timedBomb.delayed_timer_bomb -= pElapsed;
        ++index;
      }
      else
      {
        this.timedBombs.RemoveAt(index);
        if (timedBomb.Type.explodable_timed)
        {
          MapAction.damageWorld(timedBomb, timedBomb.Type.explode_range, AssetManager.terraform.get("bomb"));
          MusicBox.playSound("event:/SFX/EXPLOSIONS/ExplosionMiddle", timedBomb);
        }
      }
    }
  }

  public override void draw(float pElapsed)
  {
    if (!Object.op_Implicit((Object) this.sprRnd) && this.delayedBombs.Count <= 0)
      return;
    this.UpdateDirty(pElapsed);
  }

  protected override void UpdateDirty(float pElapsed)
  {
    if (this.delayedBombs.Count > 0)
    {
      int index = 0;
      while (index < this.delayedBombs.Count)
      {
        WorldTile delayedBomb = this.delayedBombs[index];
        delayedBomb.delayed_timer_bomb -= World.world.elapsed;
        if ((double) delayedBomb.delayed_timer_bomb <= 0.0)
        {
          delayedBomb.delayed_timer_bomb = -100f;
          this.delayedBombs.Remove(delayedBomb);
          TileTypeBase tileTypeBase = string.IsNullOrEmpty(delayedBomb.delayed_bomb_type) ? (TileTypeBase) TopTileLibrary.tnt_timed : (TileTypeBase) AssetManager.top_tiles.get(delayedBomb.delayed_bomb_type);
          MapAction.damageWorld(delayedBomb, tileTypeBase.explode_range, AssetManager.terraform.get("bomb"));
          MusicBox.playSound("event:/SFX/EXPLOSIONS/ExplosionMiddle", delayedBomb);
        }
        else
          ++index;
      }
    }
    if (this.hashset_bombs.Count > 0)
    {
      foreach (WorldTile hashsetBomb in (HashSet<WorldTile>) this.hashset_bombs)
      {
        hashsetBomb.explosion_wave = 0;
        hashsetBomb.explosion_power = 0;
      }
      this.hashset_bombs.Clear();
    }
    if ((double) this.timer > 0.0)
    {
      this.timer -= World.world.elapsed;
    }
    else
    {
      this.timer = this.interval;
      if (this.hashsetTiles.Count == 0)
        return;
      using (ListPool<WorldTile> listPool = new ListPool<WorldTile>())
      {
        foreach (WorldTile hashsetTile in (HashSet<WorldTile>) this.hashsetTiles)
        {
          if (hashsetTile.explosion_fx_stage > 0)
          {
            if (Randy.randomBool())
              this.pixels[hashsetTile.data.tile_id] = Toolbox.clear;
            else
              this.pixels[hashsetTile.data.tile_id] = this.colors[hashsetTile.explosion_fx_stage - 1];
            --hashsetTile.explosion_fx_stage;
            if (hashsetTile.explosion_fx_stage <= 0)
            {
              hashsetTile.explosion_fx_stage = 0;
              listPool.Add(hashsetTile);
            }
          }
        }
        if (listPool.Count > 0)
        {
          for (int index = 0; index < listPool.Count; ++index)
            this.hashsetTiles.Remove(listPool[index]);
        }
        this.updatePixels();
      }
    }
  }

  internal void setDirty(WorldTile pTile, float pDist, float pRadius)
  {
    int num = (int) (60.0 * (1.0 - (double) pDist / (double) pRadius));
    if (num == 0 || num < pTile.explosion_fx_stage)
      return;
    this.hashsetTiles.Add(pTile);
    pTile.explosion_fx_stage = num;
  }
}
