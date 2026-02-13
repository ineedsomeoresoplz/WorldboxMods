// Decompiled with JetBrains decompiler
// Type: GodFinger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GodFinger : BaseActorComponent
{
  public const float FLYING_SPEED = 8f;
  public const int MAX_TARGET_TILES = 1200;
  internal GodPower god_power;
  internal string brush;
  internal float flying_target = 8f;
  private float _rotate_wiggle = 30f;
  internal static string[] power_over_water = Toolbox.splitStringIntoArray("tile_high_soil#10", "tile_soil#10", "tile_hills", "tile_mountains", "tile_summit", "shovel_plus");
  internal static string[] power_over_ground = Toolbox.splitStringIntoArray("seeds_candy", "seeds_corrupted", "seeds_crystal", "seeds_desert", "seeds_enchanted", "seeds_grass", "seeds_infernal", "seeds_jungle", "seeds_lemon", "seeds_mushroom", "seeds_permafrost", "seeds_savanna", "seeds_swamp", "seeds_birch", "seeds_maple", "seeds_flower", "seeds_garlic", "seeds_rocklands", "seeds_celestial", "seeds_singularity", "seeds_clover", "seeds_paradox", "fertilizer_plants#4", "fertilizer_trees#4");
  internal HashSet<WorldTile> target_tiles = new HashSet<WorldTile>(1800);
  internal FingerTarget finger_target;
  private SpriteAnimation fingerTip;
  internal Color debug_color;
  private static Color[] _random_colors = new Color[20]
  {
    Toolbox.color_green,
    Toolbox.color_red,
    Toolbox.color_blue,
    Toolbox.color_yellow,
    Toolbox.color_purple,
    Color32.op_Implicit(Toolbox.color_fire),
    Color32.op_Implicit(Toolbox.color_phenotype_green_0),
    Color32.op_Implicit(Toolbox.color_phenotype_green_1),
    Color32.op_Implicit(Toolbox.color_phenotype_green_2),
    Color32.op_Implicit(Toolbox.color_phenotype_green_3),
    Color32.op_Implicit(Toolbox.color_magenta_0),
    Color32.op_Implicit(Toolbox.color_magenta_1),
    Color32.op_Implicit(Toolbox.color_magenta_2),
    Color32.op_Implicit(Toolbox.color_magenta_3),
    Color32.op_Implicit(Toolbox.color_magenta_4),
    Color32.op_Implicit(Toolbox.color_teal_0),
    Color32.op_Implicit(Toolbox.color_teal_1),
    Color32.op_Implicit(Toolbox.color_teal_2),
    Color32.op_Implicit(Toolbox.color_teal_3),
    Color32.op_Implicit(Toolbox.color_teal_4)
  };

  internal bool is_drawing
  {
    get
    {
      return this.actor.ai.hasTask() && this.actor.ai.action is BehFinger action && action.drawing_action;
    }
  }

  internal bool drawing_over_water => this.finger_target == FingerTarget.Water;

  internal bool drawing_over_ground => this.finger_target == FingerTarget.Ground;

  internal override void create(Actor pActor)
  {
    base.create(pActor);
    this.debug_color = GodFinger._random_colors.GetRandom<Color>();
    ((Object) ((Component) this).gameObject).name = "GF " + this.actor.getID().ToString();
    this.fingerTip = ((Component) ((Component) this).transform.Find("Tip")).gameObject.GetComponent<SpriteAnimation>();
    ((Component) this.fingerTip).gameObject.SetActive(false);
    this.actor.target_angle = Vector3.zero;
    this.actor.setFlying(true);
    this.actor.position_height = 8f;
  }

  internal void lightAction() => AchievementLibrary.god_finger_lightning.check();

  public override void update(float pElapsed)
  {
    if (!this.actor.isAlive() || World.world.isPaused())
      return;
    bool isDrawing = this.is_drawing;
    bool flag = !isDrawing && (double) this.flying_target < 2.0;
    if (isDrawing)
    {
      this.actor.target_angle.z = Mathf.Clamp(this.actor.target_angle.z, 25f, 35f);
      if ((double) this.actor.target_angle.z < (double) this._rotate_wiggle)
        this.actor.target_angle.z += 100f * pElapsed;
      else if ((double) this.actor.target_angle.z > (double) this._rotate_wiggle)
        this.actor.target_angle.z -= 100f * pElapsed;
      else
        this._rotate_wiggle = (float) Randy.randomInt(25, 35);
      this.actor.rotation_cooldown = 300f;
    }
    else if (flag)
    {
      if ((double) this.actor.target_angle.z < 30.0)
        this.actor.target_angle.z += 100f * pElapsed;
      this.actor.rotation_cooldown = 300f;
    }
    else
      this.actor.rotation_cooldown = 0.0f;
    if ((double) this.flying_target != (double) this.actor.position_height)
      this.actor.position_height = Mathf.MoveTowards(this.actor.position_height, this.flying_target, pElapsed * 8f);
    ((Component) this.fingerTip).gameObject.SetActive(isDrawing);
    if (!isDrawing)
      return;
    this.fingerTip.update(pElapsed);
    if (!this.isInMapBounds(Vector2.op_Implicit(this.actor.current_position)))
      return;
    this.drawOnTile(this.actor.current_tile);
  }

  private bool isInMapBounds(Vector3 pPos)
  {
    return (double) pPos.x > 0.0 && (double) pPos.y > 0.0 && (double) pPos.x < (double) MapBox.width && (double) pPos.y < (double) MapBox.height;
  }

  public void drawOnTile(WorldTile pTile)
  {
    World.world.conway_layer.checkKillRange(pTile.pos, 2);
    string currentBrush = Config.current_brush;
    Config.current_brush = this.brush;
    if (this.god_power.click_power_action != null || this.god_power.click_power_brush_action != null)
    {
      if (this.god_power.click_power_brush_action != null)
      {
        int num1 = this.god_power.click_power_brush_action(pTile, this.god_power) ? 1 : 0;
      }
      else if (this.god_power.click_power_action != null)
      {
        int num2 = this.god_power.click_power_action(pTile, this.god_power) ? 1 : 0;
      }
    }
    if (this.god_power.click_action != null || this.god_power.click_brush_action != null)
    {
      if (this.god_power.click_brush_action != null)
      {
        int num3 = this.god_power.click_brush_action(pTile, this.god_power.id) ? 1 : 0;
      }
      else if (this.god_power.click_action != null)
      {
        int num4 = this.god_power.click_action(pTile, this.god_power.id) ? 1 : 0;
      }
    }
    World.world.loopWithBrush(pTile, Config.current_brush_data, new PowerActionWithID(this.clearTargets), "god_finger");
    World.world.loopWithBrush(pTile, Brush.get(2), new PowerActionWithID(this.fingerTile), "god_finger");
    Config.current_brush = currentBrush;
  }

  public bool clearTargets(WorldTile pTile, string pPowerID)
  {
    this.target_tiles.Remove(pTile);
    return true;
  }

  public bool fingerTile(WorldTile pTile, string pPowerID)
  {
    pTile.doUnits((Action<Actor>) (pActor =>
    {
      if (pActor.asset.flag_finger || !pActor.asset.can_be_killed_by_stuff)
        return;
      pActor.getHitFullHealth(AttackType.Gravity);
    }));
    return true;
  }

  public override void Dispose()
  {
    this.target_tiles.Clear();
    this.finger_target = FingerTarget.None;
    base.Dispose();
  }

  internal static bool deathFlip(BaseSimObject pTarget, WorldTile pTile, float pElapsed)
  {
    Actor a = pTarget.a;
    if (a.isFalling())
    {
      a.updateFall();
      return true;
    }
    if ((double) a.target_angle.z < 90.0)
    {
      a.target_angle.z = Mathf.Lerp(a.target_angle.z, 90f, pElapsed * 4f);
      if ((double) a.target_angle.z > 90.0)
        a.target_angle.z = 90f;
      if (!a.is_visible)
        a.updateRotation();
      if ((double) Mathf.Abs(a.current_rotation.z) >= 89.0)
      {
        a.dieAndDestroy(AttackType.None);
        return true;
      }
    }
    a.updateDeadBlackAnimation(pElapsed);
    return true;
  }

  public static void debug_trail(GodFinger pFinger)
  {
    if (!DebugConfig.isOn(DebugOption.ShowGodFingerTargetting) || !MapBox.isRenderGameplay() || !pFinger.actor.is_visible)
      return;
    AiSystemActor ai = pFinger.actor.ai;
    if (!ai.hasTask())
      return;
    Color colorWhite = Toolbox.color_white;
    Color pColor;
    switch (ai.task.id)
    {
      case "godfinger_move":
        pColor = Toolbox.color_blue;
        break;
      case "godfinger_find_target":
        pColor = Toolbox.color_red;
        break;
      case "godfinger_random_fun_move":
        pColor = Toolbox.color_green;
        break;
      case "godfinger_circle_move":
        pColor = Toolbox.color_purple;
        break;
      case "godfinger_circle_move_big":
        pColor = Toolbox.color_yellow;
        break;
      case "godfinger_circle_move_small":
        pColor = Color32.op_Implicit(Toolbox.color_fire);
        break;
      default:
        return;
    }
    BaseEffect baseEffect = EffectsLibrary.spawn("fx_weapon_particle", pX: ((Component) pFinger.fingerTip).transform.position.x, pY: ((Component) pFinger.fingerTip).transform.position.y);
    if (!Object.op_Inequality((Object) baseEffect, (Object) null))
      return;
    ((StatusParticle) baseEffect).spawnParticle(((Component) baseEffect).transform.position, pColor);
  }
}
