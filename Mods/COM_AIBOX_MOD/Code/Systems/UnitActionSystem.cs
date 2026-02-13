using System;
using UnityEngine;
using ai.behaviours; // Assuming this is where the game's behaviors are
using AIBox;

namespace AIBox
{
    public static class UnitActionSystem
    {
        // =========================================================================================
        // COMMUNICATION
        // =========================================================================================
        
        public static void Talk(Actor pActor, Actor pTarget)
        {
            if (pActor == null || pTarget == null) return;
            // Behavior requires a target to be set on the actor
             pActor.beh_actor_target = pTarget;
            new BehDoTalk().execute(pActor);
        }

        public static void FinishTalk(Actor pActor)
        {
            if (pActor == null) return;
            new BehFinishTalk().execute(pActor);
        }

        // =========================================================================================
        // AGGRESSION / INTERACTION
        // =========================================================================================

        public static void StealFromTarget(Actor pActor, Actor pTarget)
        {
            if (pActor == null || pTarget == null) return;
            pActor.beh_actor_target = pTarget;
            new BehStealFromTarget().execute(pActor);
        }

        public static void AttackHuntingTarget(Actor pActor, Actor pTarget)
        {
            if (pActor == null || pTarget == null) return;
             // Usually hunting behaviors look for beh_actor_target or attack_target
             // We set both to be safe/sure
            pActor.beh_actor_target = pTarget;
            new BehAttackActorHuntingTarget().execute(pActor);
        }

        // =========================================================================================
        // MOVEMENT
        // =========================================================================================

        public static void GoToActorTarget(Actor pActor, Actor pTarget, 
            GoToActorTargetType pType = GoToActorTargetType.SameTile, 
            bool pPathOnWater = false, 
            bool pCheckCanAttackTarget = false, 
            bool pCalibrateTargetPosition = false, 
            float pCheckDistance = 2f, 
            bool pCheckSameIsland = true, 
            bool pCheckInsideSomething = true)
        {
            if (pActor == null || pTarget == null) return;
            pActor.beh_actor_target = pTarget;

            new BehGoToActorTarget(pType, pPathOnWater, pCheckCanAttackTarget, 
                                   pCalibrateTargetPosition, pCheckDistance, 
                                   pCheckSameIsland, pCheckInsideSomething).execute(pActor);
        }

        public static void GoToBuildingTarget(Actor pActor, Building pTarget, bool pPathOnWater = false)
        {
            if (pActor == null || pTarget == null) return;
             // Assuming there's a beh_building_target definition found or generic target
             // Based on previous reads, we didn't fully check BehGoToBuildingTarget source yet, 
             // but it likely uses beh_building_target.
             pActor.beh_building_target = pTarget; 
            new BehGoToBuildingTarget(pPathOnWater).execute(pActor);
        }

        // =========================================================================================
        // STATUS & EFFECTS
        // =========================================================================================

        public static void AddStatus(Actor pActor, string pStatusID, float pOverrideTimer = -1f, bool pEffectOn = true, bool pAddActionTimer = false)
        {
            if (pActor == null) return;
            new BehActorAddStatus(pStatusID, pOverrideTimer, pEffectOn, pAddActionTimer).execute(pActor);
        }

        public static void RemoveStatus(Actor pActor, string pStatusID)
        {
            if (pActor == null) return;
            new BehActorRemoveStatus(pStatusID).execute(pActor);
        }

        public static void SpawnHmmEffect(Actor pActor, int pAmount = 1)
        {
            if (pActor == null) return;
            new BehSpawnHmmEffect(pAmount).execute(pActor);
        }

        // =========================================================================================
        // SURVIVAL & NEEDS
        // =========================================================================================

        public static void FindMeatSource(Actor pActor, MeatTargetType pMeatTargetType = MeatTargetType.Meat, bool pCheckForFactions = true)
        {
            if (pActor == null) return;
            new BehFindMeatSource(pMeatTargetType, pCheckForFactions).execute(pActor);
        }

        public static void CheckNeeds(Actor pActor, int pRestarts)
        {
            if (pActor == null) return;
            new BehCheckNeeds(pRestarts).execute(pActor);
        }

        // =========================================================================================
        // MISC
        // =========================================================================================

        public static void SetNextTask(Actor pActor, string pTaskID)
        {
            if (pActor == null) return;
            new BehSetNextTask(pTaskID).execute(pActor);
        }

        public static void GiveTax(Actor pActor)
        {
            if (pActor == null) return;
            new BehActorGiveTax().execute(pActor);
        }
    }
}
