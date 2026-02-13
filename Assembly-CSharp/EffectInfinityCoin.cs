// Decompiled with JetBrains decompiler
// Type: EffectInfinityCoin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EffectInfinityCoin : BaseEffect
{
  private static List<Actor> _temp_list = new List<Actor>();
  private bool used;

  internal override void create() => base.create();

  internal override void spawnOnTile(WorldTile pTile)
  {
    this.prepare(Vector2.op_Implicit(new Vector3(pTile.posV3.x, pTile.posV3.y - 1f)), 0.25f);
  }

  internal override void prepare(Vector2 pVector, float pScale = 1f)
  {
    base.prepare(pVector, pScale);
    Vector3 localPosition = ((Component) this).transform.localPosition;
    localPosition.z = -2f;
    this.current_position = Vector2.op_Implicit(localPosition);
    ((Component) this).transform.localPosition = localPosition;
    this.used = false;
    World.world.startShake(0.1f, 0.02f, 3f);
  }

  private void Update()
  {
    if (this.sprite_animation.currentFrameIndex < 32 /*0x20*/ || this.used)
      return;
    World.world.startShake(0.2f, pIntensity: 3f);
    this.used = true;
    Vector3 localPosition = ((Component) this).transform.localPosition;
    localPosition.y += 2f;
    BaseEffect baseEffect = EffectsLibrary.spawnAt("fx_boulder_impact", localPosition, ((Component) this).transform.localScale.x);
    if (Object.op_Inequality((Object) baseEffect, (Object) null))
    {
      localPosition = ((Component) baseEffect).transform.localPosition;
      localPosition.z = -1f;
      ((Component) baseEffect).transform.localPosition = localPosition;
    }
    EffectsLibrary.spawnExplosionWave(localPosition, 5f);
    this.doAction();
  }

  private void doAction()
  {
    int num1 = 0;
    List<Actor> simpleList = World.world.units.getSimpleList();
    for (int index = 0; index < simpleList.Count; ++index)
    {
      Actor actor = simpleList[index];
      if (actor.isAlive() && !actor.isFavorite() && !actor.asset.ignored_by_infinity_coin)
        ++num1;
    }
    int num2 = num1 % 2 != 0 ? num1 / 2 + 1 : num1 / 2;
    int num3 = 0;
    EffectInfinityCoin._temp_list.AddRange((IEnumerable<Actor>) World.world.units);
    for (int index = 0; index < EffectInfinityCoin._temp_list.Count; ++index)
    {
      EffectInfinityCoin._temp_list.ShuffleOne<Actor>(index);
      Actor temp = EffectInfinityCoin._temp_list[index];
      if (num2 != 0)
      {
        if (temp.isAlive() && !temp.isFavorite() && !temp.asset.ignored_by_infinity_coin && !temp.is_invincible)
        {
          ++num3;
          --num2;
          temp.getHitFullHealth(AttackType.Divine);
        }
      }
      else
        break;
    }
    WorldTip.addWordReplacement("$removed$", num3.ToString());
    WorldTip.showNow("infinity_coin_used", pPosition: "top");
    EffectInfinityCoin._temp_list.Clear();
  }
}
