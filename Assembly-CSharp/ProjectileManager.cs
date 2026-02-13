// Decompiled with JetBrains decompiler
// Type: ProjectileManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ProjectileManager : CoreSystemManager<Projectile, ProjectileData>
{
  private const float COLLISION_DISTANCE = 0.25f;
  private readonly Dictionary<Kingdom, bool> _kingdoms = new Dictionary<Kingdom, bool>();

  public ProjectileManager() => this.type_id = "projectile";

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    this.updateProjectiles(pElapsed);
    this.checkCollision();
    this.checkDead();
  }

  private void checkCollision()
  {
    if (this.list.Count < 2)
      return;
    for (int index1 = 0; index1 < this.list.Count - 1; ++index1)
    {
      Projectile projectile1 = this.list[index1];
      if (projectile1.canBeCollided())
      {
        for (int index2 = index1 + 1; index2 < this.list.Count; ++index2)
        {
          Projectile projectile2 = this.list[index2];
          if (projectile2.canBeCollided() && (projectile1.kingdom != projectile2.kingdom || projectile1.kingdom.asset.always_attack_each_other))
          {
            Vector3 pPos1 = Vector2.op_Implicit(projectile1.getTransformedPositionWithHeight());
            Vector3 pPos2 = Vector2.op_Implicit(projectile2.getTransformedPositionWithHeight());
            if ((double) Vector3.Distance(pPos1, pPos2) < 0.25)
            {
              Vector3 pPos3 = Vector3.op_Division(Vector3.op_Addition(pPos1, pPos2), 2f);
              pPos3.y += pPos3.z;
              pPos3.z = 0.0f;
              EffectsLibrary.spawnAt("fx_hit", pPos3, 0.1f);
              projectile2.getCollided(pPos1);
              projectile1.getCollided(pPos2);
            }
          }
        }
      }
    }
  }

  private void updateProjectiles(float pElapsed)
  {
    this._kingdoms.Clear();
    List<Projectile> list = this.list;
    for (int index = 0; index < list.Count; ++index)
    {
      Projectile projectile = list[index];
      if (!projectile.isTargetReached())
        this._kingdoms[projectile.kingdom] = true;
      projectile.update(pElapsed);
    }
  }

  internal bool hasActiveProjectiles(Kingdom pKingdom) => this._kingdoms.ContainsKey(pKingdom);

  private void checkDead()
  {
    for (int index = this.list.Count - 1; index >= 0; --index)
    {
      Projectile pObject = this.list[index];
      if (pObject.isFinished())
        this.removeObject(pObject);
    }
  }

  public Projectile spawn(
    BaseSimObject pInitiator,
    BaseSimObject pTargetObject,
    string pAssetID,
    Vector3 pLaunchPosition,
    Vector3 pTargetPosition,
    float pTargetZ = 0.0f,
    float pStartPosZ = 0.25f,
    Action pKillAction = null,
    Kingdom pForcedKingdom = null)
  {
    Projectile projectile1 = this.newObject();
    if (projectile1 == null)
      return (Projectile) null;
    Projectile projectile2 = projectile1;
    BaseSimObject pInitiator1 = pInitiator;
    BaseSimObject pTargetObject1 = pTargetObject;
    Vector3 pLaunchPosition1 = pLaunchPosition;
    Vector3 pTargetPosition1 = pTargetPosition;
    string pAssetID1 = pAssetID;
    float num = pStartPosZ;
    double pTargetPosZ = (double) pTargetZ;
    double pStartZ = (double) num;
    Action pKillAction1 = pKillAction;
    Kingdom pForcedKingdom1 = pForcedKingdom;
    projectile2.start(pInitiator1, pTargetObject1, pLaunchPosition1, pTargetPosition1, pAssetID1, (float) pTargetPosZ, (float) pStartZ, pKillAction: pKillAction1, pForcedKingdom: pForcedKingdom1);
    return projectile1;
  }
}
