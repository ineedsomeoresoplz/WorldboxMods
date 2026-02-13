// Decompiled with JetBrains decompiler
// Type: SimpleMod
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class SimpleMod
{
  public SimpleMod()
  {
    ProjectileAsset projectileAsset = AssetManager.projectiles.clone("skeleton_arrow", "arrow");
    projectileAsset.trail_effect_enabled = true;
    projectileAsset.texture = "fireball";
    projectileAsset.scale_target = 0.5f;
    EquipmentAsset equipmentAsset = AssetManager.items.clone("skeleton_bow", "_range");
    equipmentAsset.base_stats["range"] = 22f;
    equipmentAsset.base_stats["critical_chance"] = 0.1f;
    equipmentAsset.base_stats["critical_damage_multiplier"] = 0.5f;
    equipmentAsset.projectile = "skeleton_arrow";
    ActorAsset actorAsset = AssetManager.actor_library.clone("super_skeleton", "skeleton");
    actorAsset.default_attack = "skeleton_bow";
    actorAsset.default_weapons = (string[]) null;
    actorAsset.base_stats["health"] = 100000f;
    actorAsset.base_stats["damage"] = 500f;
    actorAsset.base_stats["speed"] = 500f;
    actorAsset.job = Toolbox.a<string>("super_skeleton_job");
    AssetManager.actor_library.addTrait("regeneration");
    AssetManager.actor_library.addTrait("immortal");
    ActorJobLibrary jobActor = AssetManager.job_actor;
    ActorJob pAsset1 = new ActorJob();
    pAsset1.id = "super_skeleton_job";
    ActorJob actorJob = jobActor.add(pAsset1);
    actorJob.addTask("mod_destroy_trees");
    actorJob.addTask("random_move");
    actorJob.addTask("wait");
    actorJob.addTask("attack_golden_brain");
    BehaviourTaskActorLibrary tasksActor = AssetManager.tasks_actor;
    BehaviourTaskActor pAsset2 = new BehaviourTaskActor();
    pAsset2.id = "mod_destroy_trees";
    BehaviourTaskActor behaviourTaskActor = tasksActor.add(pAsset2);
    behaviourTaskActor.addBeh((BehaviourActionActor) new BehFindBuilding("type_tree", true, true));
    behaviourTaskActor.addBeh((BehaviourActionActor) new BehGoToBuildingTarget());
    behaviourTaskActor.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(1f));
    behaviourTaskActor.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(1f));
    behaviourTaskActor.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(1f));
    behaviourTaskActor.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(1f));
    behaviourTaskActor.addBeh((BehaviourActionActor) new BehExtractResourcesFromBuilding());
    behaviourTaskActor.addBeh((BehaviourActionActor) new BehRandomWait(1f, 2f));
  }
}
