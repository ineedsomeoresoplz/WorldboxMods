// Decompiled with JetBrains decompiler
// Type: Plot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Plot : MetaObject<PlotData>
{
  public static bool DEBUG_PLOTS = false;
  private static Dictionary<int, Sprite> _cache_sprites;
  public City target_city;
  public Kingdom target_kingdom;
  public Alliance target_alliance;
  public Actor target_actor;
  public War target_war;
  private PlotAsset _plot_asset;
  private PlotState _state;
  private int _power;
  private static int _total_sprites = 7;
  private int _last_index;
  public float transition_animation;
  private float _transition_animation_speed;
  private Actor _plot_author;
  public float progress_target;
  public double last_update_progress;

  protected override MetaType meta_type => MetaType.Plot;

  public override BaseSystemManager manager => (BaseSystemManager) World.world.plots;

  public void newPlot(Actor pAuthor, PlotAsset pAsset, bool pForced)
  {
    this.data.plot_type_id = pAsset.id;
    this._plot_asset = pAsset;
    this.setName(this._plot_asset.getLocaleID().Localize());
    this.data.forced = pForced;
    this.data.founder_name = pAuthor.getName();
    this.data.founder_id = pAuthor.getID();
    pAuthor.setPlot(this);
    this._plot_author = pAuthor;
  }

  protected sealed override void setDefaultValues()
  {
    base.setDefaultValues();
    this._state = PlotState.Active;
    this._power = 0;
    this._last_index = -1;
    this.transition_animation = 1f;
    this._transition_animation_speed = 0.8f;
    this.progress_target = 0.0f;
    this.last_update_progress = 0.0;
  }

  private bool isState(PlotState pState) => this._state == pState;

  public void finishPlot(PlotState pState, Actor pActor)
  {
    if (this._state == pState)
      return;
    this._state = pState;
    switch (this._state)
    {
      case PlotState.Finished:
        ++World.world.game_stats.data.plotsSucceeded;
        ++World.world.map_stats.plotsSucceeded;
        if (pActor != null)
        {
          pActor.changeHappiness("just_finished_plot");
          pActor.addStatusEffect("recovery_plot");
          break;
        }
        break;
      case PlotState.Cancelled:
        ++World.world.game_stats.data.plotsForgotten;
        ++World.world.map_stats.plotsForgotten;
        if (pActor != null)
        {
          pActor.addStatusEffect("recovery_plot");
          break;
        }
        break;
    }
    this.startRemovalAnimation();
    foreach (Actor unit in this.units)
      unit.leavePlot();
    this.startAnimation();
    this.data.progress_current = this.getProgressMax();
  }

  public PlotState getState() => this._state;

  public override bool isReadyForRemoval() => !this.isActive();

  public override void loadData(PlotData pData)
  {
    base.loadData(pData);
    this._plot_asset = AssetManager.plots_library.get(this.data.plot_type_id);
    this.target_city = World.world.cities.get(this.data.id_target_city);
    this.target_kingdom = World.world.kingdoms.get(this.data.id_target_kingdom);
    this.target_alliance = World.world.alliances.get(this.data.id_target_alliance);
    this.target_war = World.world.wars.get(this.data.id_target_war);
  }

  public void loadAuthors()
  {
    this._plot_author = World.world.units.get(this.data.founder_id);
    this.target_actor = World.world.units.get(this.data.id_target_actor);
  }

  public override void save()
  {
    base.save();
    PlotData data1 = this.data;
    Actor targetActor = this.target_actor;
    long num1 = targetActor != null ? targetActor.data.id : -1L;
    data1.id_target_actor = num1;
    PlotData data2 = this.data;
    City targetCity = this.target_city;
    long num2 = targetCity != null ? targetCity.data.id : -1L;
    data2.id_target_city = num2;
    PlotData data3 = this.data;
    Kingdom targetKingdom = this.target_kingdom;
    long num3 = targetKingdom != null ? targetKingdom.data.id : -1L;
    data3.id_target_kingdom = num3;
    PlotData data4 = this.data;
    Alliance targetAlliance = this.target_alliance;
    long num4 = targetAlliance != null ? targetAlliance.data.id : -1L;
    data4.id_target_alliance = num4;
    PlotData data5 = this.data;
    War targetWar = this.target_war;
    long num5 = targetWar != null ? targetWar.data.id : -1L;
    data5.id_target_war = num5;
  }

  public void updateAnimations(float pElapsed)
  {
    if ((double) this.transition_animation <= 1.0)
      return;
    this.transition_animation -= Time.deltaTime * this._transition_animation_speed;
    if ((double) this.transition_animation >= 1.0)
      return;
    this.transition_animation = 1f;
  }

  public override ColorAsset getColor()
  {
    Actor author = this.getAuthor();
    ColorAsset color = (ColorAsset) null;
    if (author != null)
      color = author.kingdom.getColor();
    return color;
  }

  public bool updateProgressTarget(Actor pActor, float pIntelligence)
  {
    this.last_update_progress = World.world.getCurWorldTime();
    this.progress_target += pIntelligence + 1f;
    if ((double) this.progress_target > (double) this.getProgressMax())
      this.progress_target = this.getProgressMax();
    this._transition_animation_speed = 1.5f * this.getProgressMod();
    if ((double) this.transition_animation <= 1.0)
      this.startAnimation();
    if ((double) this.data.progress_current < (double) this.getProgressMax())
      return false;
    if (this._plot_asset.action(pActor))
    {
      PlotAction postAction = this._plot_asset.post_action;
      if (postAction != null)
      {
        int num = postAction(pActor) ? 1 : 0;
      }
    }
    this.finishPlot(PlotState.Finished, pActor);
    return true;
  }

  public void updateProgress(float pElapsed)
  {
    if ((double) this.data.progress_current >= (double) this.progress_target)
      return;
    this.data.progress_current += pElapsed * 2f;
    if ((double) this.data.progress_current <= (double) this.progress_target)
      return;
    this.data.progress_current = this.progress_target;
  }

  public int getProgressPercentage()
  {
    float progress = this.getProgress();
    float progressMax = this.getProgressMax();
    return (double) progressMax == 0.0 ? 0 : (int) ((double) progress / (double) progressMax * 100.0);
  }

  public float getProgressMod() => this.getProgress() / this.getProgressMax();

  public bool checkInitiatorAndTargets()
  {
    return (!this._plot_asset.check_target_actor || this.target_actor.isAlive()) && (!this._plot_asset.check_target_alliance || this.target_alliance.isAlive()) && (!this._plot_asset.check_target_city || this.target_city.isAlive()) && (!this._plot_asset.check_target_kingdom || this.target_kingdom.isAlive() && this.target_kingdom.hasCities()) && (!this._plot_asset.check_target_war || this.target_war.isAlive());
  }

  public PlotAsset getAsset() => this._plot_asset;

  public int getPower() => this._power;

  public int getMaxSupporters() => 15;

  public int getSupporters() => this.units.Count;

  public float getProgressMax() => this._plot_asset.progress_needed;

  public float getProgress() => this.data.progress_current;

  public void startAnimation() => this.transition_animation = 1.3f;

  public void startRemovalAnimation()
  {
    foreach (Actor unit in this.units)
    {
      if (unit.isAlive())
        World.world.stack_effects.plot_removals.Add(new PlotIconData()
        {
          actor = unit,
          sprite = this.getSpritePath(),
          timestamp = World.world.getCurSessionTime()
        });
    }
  }

  public Sprite getSprite() => SpriteTextureLoader.getSprite(this.getSpritePath());

  public string getSpritePath()
  {
    if (this.isFinished())
      return "plots/icons/progress/plot_finished";
    return this.isCancelled() ? "plots/icons/progress/plot_cancelled" : this._plot_asset.path_icon;
  }

  public Sprite getSpriteIconProgress()
  {
    int key1 = (int) ((double) this.getProgress() / (double) this.getProgressMax() * (double) (Plot._total_sprites + 1));
    if (key1 > Plot._total_sprites)
      key1 = Plot._total_sprites;
    if (Plot._cache_sprites == null)
    {
      Plot._cache_sprites = new Dictionary<int, Sprite>();
      for (int key2 = 0; key2 <= Plot._total_sprites; ++key2)
        Plot._cache_sprites.Add(key2, SpriteTextureLoader.getSprite("plots/speech_bubbles/speech_bubble_0" + key2.ToString()));
    }
    Sprite cacheSprite = Plot._cache_sprites[key1];
    if (key1 == this._last_index)
      return cacheSprite;
    this._last_index = key1;
    this.startAnimation();
    return cacheSprite;
  }

  public bool hasSupporter(Actor pActor) => this.units.Contains(pActor);

  public bool isActive() => this.isState(PlotState.Active);

  public bool isCancelled() => this.isState(PlotState.Cancelled);

  public bool isFinished() => this.isState(PlotState.Finished);

  public bool isSameType(PlotAsset pAsset) => pAsset == this.getAsset();

  public Actor getAuthor()
  {
    this._plot_author = World.world.units.get(this.data.founder_id);
    return this._plot_author;
  }

  public override void clearLastYearStats()
  {
    throw new NotImplementedException(this.GetType().Name);
  }

  public override void increaseBirths() => throw new NotImplementedException(this.GetType().Name);

  public override void increaseDeaths(AttackType pType)
  {
    throw new NotImplementedException(this.GetType().Name);
  }

  public override void Dispose()
  {
    this.target_city = (City) null;
    this.target_kingdom = (Kingdom) null;
    this.target_alliance = (Alliance) null;
    this.target_actor = (Actor) null;
    this.target_war = (War) null;
    this._plot_asset = (PlotAsset) null;
    this._plot_author = (Actor) null;
    base.Dispose();
  }

  public bool isAuthorDead() => this._plot_author == null || !this._plot_author.isAlive();
}
