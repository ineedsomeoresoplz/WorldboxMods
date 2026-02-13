using System;
using System.Threading;
using NCMS;
using UnityEngine;
using ReflectionUtility;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ai;

namespace spellcraft
{
    class Spells
    {
        //SPELLS
        public static void init()
        {
          
         ActorTrait spellacidburst = new ActorTrait();
         spellacidburst.id = "Common Spell: Acid Burst";
         spellacidburst.path_icon = "ui/Icons/iconAcidBurst";
         spellacidburst.birth = 2.5f;
         spellacidburst.action_attack_target = (WorldAction)Delegate.Combine(spellacidburst.action_attack_target, new WorldAction(AcidBurst));
         spellacidburst.action_special_effect = (WorldAction)Delegate.Combine(spellacidburst.action_special_effect, new WorldAction(SpellNovice));
         spellacidburst.action_special_effect = (WorldAction)Delegate.Combine(spellacidburst.action_special_effect, new WorldAction(AcidProofTrait));
         AssetManager.traits.add(spellacidburst);
         addTraitToLocalizedLibrary(spellacidburst.id, "50% chance to cast Acid Burst upon attack.");
         PlayerConfig.unlockTrait("Common Spell: Acid Burst");

         ActorTrait spellfireenchantment = new ActorTrait();
         spellfireenchantment.id = "Common Spell: Fire Enchantment";
         spellfireenchantment.path_icon = "ui/Icons/iconFireEnchantment";
         spellfireenchantment.birth = 2.5f;
         spellfireenchantment.action_attack_target = new WorldAction(ActionLibrary.addBurningEffectOnTarget);
         spellfireenchantment.action_special_effect = (WorldAction)Delegate.Combine(spellfireenchantment.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spellfireenchantment);
         addTraitToLocalizedLibrary(spellfireenchantment.id, "Attacks inflict fire.");
         PlayerConfig.unlockTrait("Common Spell: Fire Enchantment");

         ActorTrait spellfrostenchantment = new ActorTrait();
         spellfrostenchantment.id = "Common Spell: Frost Enchantment";
         spellfrostenchantment.path_icon = "ui/Icons/iconFrostEnchantment";
         spellfrostenchantment.birth = 2.5f;
         spellfrostenchantment.action_attack_target = new WorldAction(ActionLibrary.addFrozenEffectOnTarget);
         spellfrostenchantment.action_special_effect = (WorldAction)Delegate.Combine(spellfrostenchantment.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spellfrostenchantment);
         addTraitToLocalizedLibrary(spellfrostenchantment.id, "Attacks freeze enemies.");
         PlayerConfig.unlockTrait("Common Spell: Frost Enchantment");

         ActorTrait spellpoisonenchantment = new ActorTrait();
         spellpoisonenchantment.id = "Common Spell: Poison Enchantment";
         spellpoisonenchantment.path_icon = "ui/Icons/iconPoisonEnchantment";
         spellpoisonenchantment.birth = 2.5f;
         spellpoisonenchantment.action_attack_target = new WorldAction(ActionLibrary.addPoisonedEffectOnTarget);
         spellpoisonenchantment.action_special_effect = (WorldAction)Delegate.Combine(spellpoisonenchantment.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spellpoisonenchantment);
         addTraitToLocalizedLibrary(spellpoisonenchantment.id, "Attacks inflict poison.");
         PlayerConfig.unlockTrait("Common Spell: Poison Enchantment");

         ActorTrait spellarcanistshield = new ActorTrait();
         spellarcanistshield.id = "Rare Spell: Arcanist Shield";
         spellarcanistshield.path_icon = "ui/Icons/iconArcanistShield";
         spellarcanistshield.birth = 0.5f;
         spellarcanistshield.action_attack_self = (WorldAction)Delegate.Combine(spellarcanistshield.action_attack_self, new WorldAction(ArcanistShield));
         spellarcanistshield.action_special_effect = (WorldAction)Delegate.Combine(spellarcanistshield.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spellarcanistshield);
         addTraitToLocalizedLibrary(spellarcanistshield.id, "10% to summon a shield in combat.");
         PlayerConfig.unlockTrait("Rare Spell: Arcanist Shield");

         ActorTrait spellslowenchantment = new ActorTrait();
         spellslowenchantment.id = "Common Spell: Slow Enchantment";
         spellslowenchantment.path_icon = "ui/Icons/iconSlowEnchantment";
         spellslowenchantment.birth = 2.5f;
         spellslowenchantment.action_attack_target = new WorldAction(ActionLibrary.addSlowEffectOnTarget);
         spellslowenchantment.action_special_effect = (WorldAction)Delegate.Combine(spellslowenchantment.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spellslowenchantment);
         addTraitToLocalizedLibrary(spellslowenchantment.id, "Attacks slow enemies.");
         PlayerConfig.unlockTrait("Common Spell: Slow Enchantment");

         ActorTrait spellbloodaura = new ActorTrait();
         spellbloodaura.id = "Rare Spell: Blood Aura";
         spellbloodaura.path_icon = "ui/Icons/iconBloodAura";
         spellbloodaura.birth = 0.5f;
         spellbloodaura.action_special_effect = (WorldAction)Delegate.Combine(spellbloodaura.action_special_effect, new WorldAction(BloodAura));
         spellbloodaura.action_special_effect = (WorldAction)Delegate.Combine(spellbloodaura.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spellbloodaura);
         addTraitToLocalizedLibrary(spellbloodaura.id, "Casts Blood Rain on itself.");
         PlayerConfig.unlockTrait("Rare Spell: Blood Aura");

         ActorTrait spelldivinelight = new ActorTrait();
         spelldivinelight.id = "Common Spell: Divine Light";
         spelldivinelight.path_icon = "ui/Icons/iconDivineLightSpell";
         spelldivinelight.birth = 2.5f;
         spelldivinelight.action_special_effect = (WorldAction)Delegate.Combine(spelldivinelight.action_special_effect, new WorldAction(DivineLight));
         spelldivinelight.action_special_effect = (WorldAction)Delegate.Combine(spelldivinelight.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spelldivinelight);
         addTraitToLocalizedLibrary(spelldivinelight.id, "Casts Divine Light on itself.");
         PlayerConfig.unlockTrait("Common Spell: Divine Light");

         ActorTrait spellunholylight = new ActorTrait();
         spellunholylight.id = "Rare Spell: Unholy Light";
         spellunholylight.path_icon = "ui/Icons/iconUnholyLight";
         spellunholylight.birth = 0.5f;
         spellunholylight.opposite = "cursed";
         spellunholylight.action_attack_target = (WorldAction)Delegate.Combine(spellunholylight.action_attack_target, new WorldAction(UnholyLight));
         spellunholylight.action_special_effect = (WorldAction)Delegate.Combine(spellunholylight.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spellunholylight);
         addTraitToLocalizedLibrary(spellunholylight.id, "Attacks have a 20% chance to curse all nearby creatures.");
         PlayerConfig.unlockTrait("Rare Spell: Unholy Light");

         ActorTrait spellbleedingblade = new ActorTrait();
         spellbleedingblade.id = "Common Spell: Bleeding Blade";
         spellbleedingblade.path_icon = "ui/Icons/iconBleedingBlade";
         spellbleedingblade.birth = 2.5f;
         spellbleedingblade.action_attack_target = (WorldAction)Delegate.Combine(spellbleedingblade.action_attack_target, new WorldAction(BleedingBlade));
         spellbleedingblade.action_special_effect = (WorldAction)Delegate.Combine(spellbleedingblade.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spellbleedingblade);
         addTraitToLocalizedLibrary(spellbleedingblade.id, "Attacks inflict Cursed.");
         PlayerConfig.unlockTrait("Common Spell: Bleeding Blade");

         ActorTrait spelllifematter = new ActorTrait();
         spelllifematter.id = "Rare Spell: Life Matter";
         spelllifematter.path_icon = "ui/Icons/iconLifeMatter";
         spelllifematter.birth = 0.5f;
         spelllifematter.action_attack_self = new WorldAction(ActionLibrary.restoreHealthOnHit);
         spelllifematter.action_special_effect = (WorldAction)Delegate.Combine(spelllifematter.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spelllifematter);
         addTraitToLocalizedLibrary(spelllifematter.id, "Attacks steal life from enemies and return it to the wizard.");
         PlayerConfig.unlockTrait("Rare Spell: Life Matter");

         ActorTrait spellfireball = new ActorTrait();
         spellfireball.id = "Rare Spell: Fireball";
         spellfireball.path_icon = "ui/Icons/iconFireball";
         spellfireball.birth = 0.5f;
         spellfireball.action_attack_target = (WorldAction)Delegate.Combine(spellfireball.action_attack_target, new WorldAction(Fireball));
         spellfireball.action_special_effect = (WorldAction)Delegate.Combine(spellfireball.action_special_effect, new WorldAction(SpellNovice));
         spellfireball.action_special_effect = (WorldAction)Delegate.Combine(spellfireball.action_special_effect, new WorldAction(FireProofTrait));
         AssetManager.traits.add(spellfireball);
         addTraitToLocalizedLibrary(spellfireball.id, "Attacks have a 50% to summon fire from the sky.");
         PlayerConfig.unlockTrait("Rare Spell: Fireball");

         ActorTrait spellthunder = new ActorTrait();
         spellthunder.id = "Rare Spell: Thunder";
         spellthunder.path_icon = "ui/Icons/iconThunder";
         spellthunder.birth = 0.5f;
         spellthunder.action_attack_target = new WorldAction(ActionLibrary.castLightning);
         spellthunder.action_special_effect = (WorldAction)Delegate.Combine(spellthunder.action_special_effect, new WorldAction(SpellNovice));
         spellthunder.action_special_effect = (WorldAction)Delegate.Combine(spellthunder.action_special_effect, new WorldAction(FireProofTrait));
         AssetManager.traits.add(spellthunder);
         addTraitToLocalizedLibrary(spellthunder.id, "Attacks summon thunder from the sky.");
         PlayerConfig.unlockTrait("Rare Spell: Thunder");

         ActorTrait spelltranquility = new ActorTrait();
         spelltranquility.id = "Rare Spell: Tranquility";
         spelltranquility.path_icon = "ui/Icons/iconTranquility";
         spelltranquility.birth = 0.5f;
         spelltranquility.action_special_effect = (WorldAction)Delegate.Combine(spelltranquility.action_special_effect, new WorldAction(Tranquility));
         spelltranquility.action_special_effect = (WorldAction)Delegate.Combine(spelltranquility.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spelltranquility);
         addTraitToLocalizedLibrary(spelltranquility.id, "The wizard grows trees and flowers.");
         PlayerConfig.unlockTrait("Rare Spell: Tranquility");

         ActorTrait spellraisedead = new ActorTrait();
         spellraisedead.id = "Rare Spell: Raise Dead";
         spellraisedead.path_icon = "ui/Icons/iconRaiseDead";
         spellraisedead.birth = 0.5f;
         spellraisedead.action_special_effect = (WorldAction)Delegate.Combine(spellraisedead.action_special_effect, new WorldAction(RaiseDead));
         spellraisedead.action_special_effect = (WorldAction)Delegate.Combine(spellraisedead.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spellraisedead);
         addTraitToLocalizedLibrary(spellraisedead.id, "The wizard can summon skeletons.");
         PlayerConfig.unlockTrait("Rare Spell: Raise Dead");

         ActorTrait spellanimatedead = new ActorTrait();
         spellanimatedead.id = "Rare Spell: Animate Dead";
         spellanimatedead.path_icon = "ui/Icons/iconAnimateDead";
         spellanimatedead.birth = 0.5f;
         spellanimatedead.action_special_effect = (WorldAction)Delegate.Combine(spellanimatedead.action_special_effect, new WorldAction(AnimateDead));
         spellanimatedead.action_special_effect = (WorldAction)Delegate.Combine(spellanimatedead.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spellanimatedead);
         addTraitToLocalizedLibrary(spellanimatedead.id, "The wizard can summon zombies.");
         PlayerConfig.unlockTrait("Rare Spell: Animate Dead");

         ActorTrait spellbansheescream = new ActorTrait();
         spellbansheescream.id = "Rare Spell: Banshee Scream";
         spellbansheescream.path_icon = "ui/Icons/iconBansheeScream";
         spellbansheescream.birth = 0.5f;
         spellbansheescream.action_special_effect = (WorldAction)Delegate.Combine(spellbansheescream.action_special_effect, new WorldAction(BansheeScream));
         spellbansheescream.action_special_effect = (WorldAction)Delegate.Combine(spellbansheescream.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spellbansheescream);
         addTraitToLocalizedLibrary(spellbansheescream.id, "If the wizard is low on health, he regenerates, summons a ghost and teleports randomly.");
         PlayerConfig.unlockTrait("Rare Spell: Banshee Scream");    

         ActorTrait spellrequiemofthedead = new ActorTrait();
         spellrequiemofthedead.id = "Legendary Spell: Requiem of the Dead";
         spellrequiemofthedead.path_icon = "ui/Icons/iconRequiemOfTheDead";
         spellrequiemofthedead.birth = 0.05f;
         spellrequiemofthedead.action_special_effect = (WorldAction)Delegate.Combine(spellrequiemofthedead.action_special_effect, new WorldAction(SpiritDead));
         spellrequiemofthedead.action_special_effect = (WorldAction)Delegate.Combine(spellrequiemofthedead.action_special_effect, new WorldAction(AnimateDead));
         spellrequiemofthedead.action_special_effect = (WorldAction)Delegate.Combine(spellrequiemofthedead.action_special_effect, new WorldAction(RaiseDead));
         spellrequiemofthedead.action_special_effect = (WorldAction)Delegate.Combine(spellrequiemofthedead.action_special_effect, new WorldAction(GhostDefend));
         spellrequiemofthedead.action_special_effect = (WorldAction)Delegate.Combine(spellrequiemofthedead.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spellrequiemofthedead);
         addTraitToLocalizedLibrary(spellrequiemofthedead.id, "The wizard can summon zombies, skeletons and ghosts. When low on health, regenerates and summons a ghost.");
         PlayerConfig.unlockTrait("Legendary Spell: Requiem of the Dead");

         ActorTrait spellwhirlwind = new ActorTrait();
         spellwhirlwind.id = "Rare Spell: Whirlwind";
         spellwhirlwind.path_icon = "ui/Icons/iconWhirlwind";
         spellwhirlwind.birth = 0.5f;
         spellwhirlwind.action_attack_target = (WorldAction)Delegate.Combine(spellwhirlwind.action_attack_target, new WorldAction(Whirlwind));
         spellwhirlwind.action_special_effect = (WorldAction)Delegate.Combine(spellwhirlwind.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spellwhirlwind);
         addTraitToLocalizedLibrary(spellwhirlwind.id, "Attacks have a 3% to summon a tornado.");
         PlayerConfig.unlockTrait("Rare Spell: Whirlwind");              

         ActorTrait spelldevotionaura = new ActorTrait();
         spelldevotionaura.id = "Rare Spell: Devotion Aura";
         spelldevotionaura.path_icon = "ui/Icons/iconDevotionAura";
         spelldevotionaura.birth = 0.5f;
         spelldevotionaura.action_special_effect = (WorldAction)Delegate.Combine(spelldevotionaura.action_attack_target, new WorldAction(DevotionAura));
         spelldevotionaura.action_special_effect = (WorldAction)Delegate.Combine(spelldevotionaura.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spelldevotionaura);
         addTraitToLocalizedLibrary(spelldevotionaura.id, "The wizard has a strong healing aura that cures nearby creatures.");
         PlayerConfig.unlockTrait("Rare Spell: Devotion Aura");

         ActorTrait spellexplosivereaction = new ActorTrait();
         spellexplosivereaction.id = "Rare Spell: Explosive Reaction";
         spellexplosivereaction.path_icon = "ui/Icons/iconExplosiveReaction";
         spellexplosivereaction.birth = 0.5f;
         spellexplosivereaction.action_attack_target = (WorldAction)Delegate.Combine(spellexplosivereaction.action_attack_target, new WorldAction(ExplosiveReaction));
         spellexplosivereaction.action_special_effect = (WorldAction)Delegate.Combine(spellexplosivereaction.action_special_effect, new WorldAction(SpellNovice));
         spellexplosivereaction.action_special_effect = (WorldAction)Delegate.Combine(spellexplosivereaction.action_special_effect, new WorldAction(FireProofTrait));
         AssetManager.traits.add(spellexplosivereaction);
         addTraitToLocalizedLibrary(spellexplosivereaction.id, "Attacks are explosive.");
         PlayerConfig.unlockTrait("Rare Spell: Explosive Reaction");

         ActorTrait spellshockwave = new ActorTrait();
         spellshockwave.id = "Common Spell: Shockwave";
         spellshockwave.path_icon = "ui/Icons/iconShockwave";
         spellshockwave.birth = 2.5f;
         spellshockwave.action_attack_target = (WorldAction)Delegate.Combine(spellshockwave.action_attack_target, new WorldAction(Shockwave));
         spellshockwave.action_special_effect = (WorldAction)Delegate.Combine(spellshockwave.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spellshockwave);
         addTraitToLocalizedLibrary(spellshockwave.id, "Attacks may cause a shockwave.");
         PlayerConfig.unlockTrait("Common Spell: Shockwave");

         ActorTrait spellfoolishploy = new ActorTrait();
         spellfoolishploy.id = "Forbidden Spell: Foolish Ploy";
         spellfoolishploy.path_icon = "ui/Icons/iconFoolishPloy";
         spellfoolishploy.birth = 0.01f;
         spellfoolishploy.action_death = new WorldAction(ActionLibrary.deathNuke);
         spellfoolishploy.action_special_effect = (WorldAction)Delegate.Combine(spellfoolishploy.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spellfoolishploy);
         addTraitToLocalizedLibrary(spellfoolishploy.id, "Will cause a huge nuclear explosion upon death.");
         PlayerConfig.unlockTrait("Forbidden Spell: Foolish Ploy");

         ActorTrait spellfieryveins = new ActorTrait();
         spellfieryveins.id = "Common Spell: Fiery Veins";
         spellfieryveins.path_icon = "ui/Icons/iconFieryVeins";
         spellfieryveins.birth = 2.5f;
         spellfieryveins.action_attack_self = new WorldAction(ActionLibrary.burningFeetEffect);
         spellfieryveins.action_special_effect = (WorldAction)Delegate.Combine(spellfieryveins.action_special_effect, new WorldAction(SpellNovice));
         spellfieryveins.action_special_effect = (WorldAction)Delegate.Combine(spellfieryveins.action_special_effect, new WorldAction(FireProofTrait));
         AssetManager.traits.add(spellfieryveins);
         addTraitToLocalizedLibrary(spellfieryveins.id, "Upon hitting a creature, the wizard emits fire.");
         PlayerConfig.unlockTrait("Common Spell: Fiery Veins");

         ActorTrait spellreincarnationone = new ActorTrait();
         spellreincarnationone.id = "Rare Spell: Reincarnation (1 use left)";
         spellreincarnationone.path_icon = "ui/Icons/iconReincarnation";
         spellreincarnationone.birth = 0.5f;
         spellreincarnationone.action_death = (WorldAction)Delegate.Combine(spellreincarnationone.action_death, new WorldAction(Reincarnation));
         spellreincarnationone.action_special_effect = (WorldAction)Delegate.Combine(spellreincarnationone.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spellreincarnationone);
         addTraitToLocalizedLibrary(spellreincarnationone.id, "Upon death, the wizard reincarnates into a new human body with 50% weaker stats.");
         PlayerConfig.unlockTrait("Rare Spell: Reincarnation (1 use left)");

         ActorTrait spelleternalfive = new ActorTrait();
         spelleternalfive.id = "Legendary Spell: Eternal (5 uses left)";
         spelleternalfive.path_icon = "ui/Icons/iconEternalSpell";
         spelleternalfive.birth = 0.05f;
         spelleternalfive.action_death = (WorldAction)Delegate.Combine(spelleternalfive.action_death, new WorldAction(EternalFive));
         spelleternalfive.action_special_effect = (WorldAction)Delegate.Combine(spelleternalfive.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spelleternalfive);
         addTraitToLocalizedLibrary(spelleternalfive.id, "Each time the wizard dies, it reincarnates into a new human body with 15% weaker stats.");
         PlayerConfig.unlockTrait("Legendary Spell: Eternal (5 uses left)");

         ActorTrait spellfireburst = new ActorTrait();
         spellfireburst.id = "Common Spell: Fire Burst";
         spellfireburst.path_icon = "ui/Icons/iconFireBurst";
         spellfireburst.birth = 2.5f;
         spellfireburst.action_attack_target = (WorldAction)Delegate.Combine(spellfireburst.action_attack_target, new WorldAction(FireBurst));
         spellfireburst.action_special_effect = (WorldAction)Delegate.Combine(spellfireburst.action_special_effect, new WorldAction(SpellNovice));
         spellfireburst.action_special_effect = (WorldAction)Delegate.Combine(spellfireburst.action_special_effect, new WorldAction(FireProofTrait));
         AssetManager.traits.add(spellfireburst);
         addTraitToLocalizedLibrary(spellfireburst.id, "50% chance to cast Fire Burst upon attack.");
         PlayerConfig.unlockTrait("Common Spell: Fire Burst");

         ActorTrait spellgravitationalpull = new ActorTrait();
         spellgravitationalpull.id = "Legendary Spell: Gravitational Pull";
         spellgravitationalpull.path_icon = "ui/Icons/iconGravitationalPull";
         spellgravitationalpull.birth = 0.05f;
         spellgravitationalpull.action_attack_target = (WorldAction)Delegate.Combine(spellgravitationalpull.action_attack_target, new WorldAction(GravitationalPull));
         spellgravitationalpull.action_special_effect = (WorldAction)Delegate.Combine(spellgravitationalpull.action_special_effect, new WorldAction(SpellNovice));
         spellgravitationalpull.action_special_effect = (WorldAction)Delegate.Combine(spellgravitationalpull.action_special_effect, new WorldAction(FireProofTrait));
         AssetManager.traits.add(spellgravitationalpull);
         addTraitToLocalizedLibrary(spellgravitationalpull.id, "Attacks have a 3% chance to summon a meteorite.");
         PlayerConfig.unlockTrait("Legendary Spell: Gravitational Pull");

         ActorTrait spellearthcracking = new ActorTrait();
         spellearthcracking.id = "Forbidden Spell: Earth Cracking";
         spellearthcracking.path_icon = "ui/Icons/iconEarthCracking";
         spellearthcracking.birth = 0.01f;
         spellearthcracking.action_attack_target = (WorldAction)Delegate.Combine(spellearthcracking.action_attack_target, new WorldAction(EarthCracking));
         spellearthcracking.action_special_effect = (WorldAction)Delegate.Combine(spellearthcracking.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spellearthcracking);
         addTraitToLocalizedLibrary(spellearthcracking.id, "Attacks have a 5% chance to cause an earthquake.");
         PlayerConfig.unlockTrait("Forbidden Spell: Earth Cracking");

         ActorTrait spellexplosiveshockwave = new ActorTrait();
         spellexplosiveshockwave.id = "Rare Spell: Explosive Shockwave";
         spellexplosiveshockwave.path_icon = "ui/Icons/iconExplosiveShockwave";
         spellexplosiveshockwave.birth = 0.05f;
         spellexplosiveshockwave.action_attack_target = (WorldAction)Delegate.Combine(spellexplosiveshockwave.action_attack_target, new WorldAction(ExplosiveShockwave));
         spellexplosiveshockwave.action_special_effect = (WorldAction)Delegate.Combine(spellexplosiveshockwave.action_special_effect, new WorldAction(SpellNovice));
         spellexplosiveshockwave.action_special_effect = (WorldAction)Delegate.Combine(spellexplosiveshockwave.action_special_effect, new WorldAction(FireProofTrait));
         AssetManager.traits.add(spellexplosiveshockwave);
         addTraitToLocalizedLibrary(spellexplosiveshockwave.id, "Attacks have a 10% chance to cause an explosive force.");
         PlayerConfig.unlockTrait("Rare Spell: Explosive Shockwave");

        //GIFTS

         ActorTrait giftoffire = new ActorTrait();
         giftoffire.id = "Gift: Fire";
         giftoffire.path_icon = "ui/Icons/iconFireGift";
         giftoffire.birth = 0.01f;
         giftoffire.action_special_effect = (WorldAction)Delegate.Combine(giftoffire.action_special_effect, new WorldAction(GiftedTrait));
         giftoffire.action_attack_target = (WorldAction)Delegate.Combine(giftoffire.action_attack_target, new WorldAction(Fireball));
         giftoffire.action_attack_target = (WorldAction)Delegate.Combine(giftoffire.action_attack_target, new WorldAction(FireBurst));
         giftoffire.action_attack_target = (WorldAction)Delegate.Combine(giftoffire.action_attack_target, new WorldAction(ExplosiveReaction));
         giftoffire.action_death = (WorldAction)Delegate.Combine(giftoffire.action_death, new WorldAction(ExplosiveShockwave));
         giftoffire.action_special_effect = (WorldAction)Delegate.Combine(giftoffire.action_special_effect, new WorldAction(FireProofTrait));
         AssetManager.traits.add(giftoffire);
         addTraitToLocalizedLibrary(giftoffire.id, "One with Fire.");
         PlayerConfig.unlockTrait("Gift: Fire");

         ActorTrait giftoffrost = new ActorTrait();
         giftoffrost.id = "Gift: Frost";
         giftoffrost.path_icon = "ui/Icons/iconFrostGift";
         giftoffrost.birth = 0.01f;
         giftoffrost.action_attack_target = new WorldAction(ActionLibrary.addFrozenEffectOnTarget);
         giftoffrost.action_special_effect = (WorldAction)Delegate.Combine(giftoffire.action_special_effect, new WorldAction(GiftedTrait));
         giftoffrost.action_special_effect = (WorldAction)Delegate.Combine(giftoffrost.action_special_effect, new WorldAction(IceProofTrait));
         AssetManager.traits.add(giftoffrost);
         addTraitToLocalizedLibrary(giftoffrost.id, "One with Frost.");
         PlayerConfig.unlockTrait("Gift: Frost");

         ActorTrait giftofdeath = new ActorTrait();
         giftofdeath.id = "Gift: Death";
         giftofdeath.path_icon = "ui/Icons/iconDeathGift";
         giftofdeath.birth = 0.01f;
         giftofdeath.opposite = "cursed";
         giftofdeath.action_special_effect = (WorldAction)Delegate.Combine(giftoffire.action_special_effect, new WorldAction(GiftedTrait));
         giftofdeath.action_attack_target = (WorldAction)Delegate.Combine(giftofdeath.action_attack_target, new WorldAction(AcidBurst));
         giftofdeath.action_attack_target = (WorldAction)Delegate.Combine(giftofdeath.action_attack_target, new WorldAction(UnholyLight));
         giftofdeath.action_attack_target = (WorldAction)Delegate.Combine(giftofdeath.action_attack_target, new WorldAction(BleedingBlade));
         giftofdeath.action_special_effect = (WorldAction)Delegate.Combine(giftofdeath.action_special_effect, new WorldAction(RaiseDead));
         giftofdeath.action_special_effect = (WorldAction)Delegate.Combine(giftofdeath.action_special_effect, new WorldAction(AnimateDead));
         giftofdeath.action_special_effect = (WorldAction)Delegate.Combine(giftofdeath.action_special_effect, new WorldAction(GhostDefend));
         giftofdeath.action_special_effect = (WorldAction)Delegate.Combine(giftofdeath.action_special_effect, new WorldAction(SpiritDead));
         giftofdeath.action_special_effect = (WorldAction)Delegate.Combine(giftofdeath.action_special_effect, new WorldAction(AcidProofTrait));
         AssetManager.traits.add(giftofdeath);
         addTraitToLocalizedLibrary(giftofdeath.id, "One with Death.");
         PlayerConfig.unlockTrait("Gift: Death");

        //MISC TRAITS

         ActorTrait spellreincarnationzero = new ActorTrait();
         spellreincarnationzero.id = "Rare Spell: Reincarnation (0 uses left)";
         spellreincarnationzero.path_icon = "ui/Icons/iconReincarnation";
         spellreincarnationzero.baseStats.mod_armor = -50f;
         spellreincarnationzero.baseStats.mod_attackSpeed = -50f;
         spellreincarnationzero.baseStats.mod_crit = -50f;
         spellreincarnationzero.baseStats.mod_damage = -50f;
         spellreincarnationzero.baseStats.mod_health = -50f;
         spellreincarnationzero.baseStats.mod_speed = -50f;
         spellreincarnationzero.baseStats.mod_diplomacy = -50f;
         spellreincarnationzero.can_be_given = false;
         spellreincarnationzero.action_special_effect = (WorldAction)Delegate.Combine(spellreincarnationzero.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spellreincarnationzero);
         addTraitToLocalizedLibrary(spellreincarnationzero.id, "The wizard has reincarnated into a new human body with 50% weaker stats.");
         PlayerConfig.unlockTrait("Rare Spell: Reincarnation (0 uses left)");

         ActorTrait spelleternalfour = new ActorTrait();
         spelleternalfour.id = "Legendary Spell: Eternal (4 uses left)";
         spelleternalfour.path_icon = "ui/Icons/iconEternalSpell";
         spelleternalfour.baseStats.mod_armor = -15f;
         spelleternalfour.baseStats.mod_attackSpeed = -15f;
         spelleternalfour.baseStats.mod_crit = -15f;
         spelleternalfour.baseStats.mod_damage = -15f;
         spelleternalfour.baseStats.mod_health = -15f;
         spelleternalfour.baseStats.mod_speed = -15f;
         spelleternalfour.baseStats.mod_diplomacy = -15f;
         spelleternalfour.can_be_given = false;
         spelleternalfour.action_death = (WorldAction)Delegate.Combine(spelleternalfour.action_death, new WorldAction(EternalFour));
         spelleternalfour.action_special_effect = (WorldAction)Delegate.Combine(spelleternalfour.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spelleternalfour);
         addTraitToLocalizedLibrary(spelleternalfour.id, "The wizard has reincarnated into a new human body with 15% weaker stats.");
         PlayerConfig.unlockTrait("Legendary Spell: Eternal (4 uses left)");     

         ActorTrait spelleternalthree = new ActorTrait();
         spelleternalthree.id = "Legendary Spell: Eternal (3 uses left)";
         spelleternalthree.path_icon = "ui/Icons/iconEternalSpell";
         spelleternalthree.baseStats.mod_armor = -30f;
         spelleternalthree.baseStats.mod_attackSpeed = -30f;
         spelleternalthree.baseStats.mod_crit = -30f;
         spelleternalthree.baseStats.mod_damage = -30f;
         spelleternalthree.baseStats.mod_health = -30f;
         spelleternalthree.baseStats.mod_speed = -30f;
         spelleternalthree.baseStats.mod_diplomacy = -30f;
         spelleternalthree.can_be_given = false;
         spelleternalthree.action_death = (WorldAction)Delegate.Combine(spelleternalthree.action_death, new WorldAction(EternalThree));
         spelleternalthree.action_special_effect = (WorldAction)Delegate.Combine(spelleternalthree.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spelleternalthree);
         addTraitToLocalizedLibrary(spelleternalthree.id, "The wizard has reincarnated into a new human body with 30% weaker stats.");
         PlayerConfig.unlockTrait("Legendary Spell: Eternal (3 uses left)");

         ActorTrait spelleternaltwo = new ActorTrait();
         spelleternaltwo.id = "Legendary Spell: Eternal (2 uses left)";
         spelleternaltwo.path_icon = "ui/Icons/iconEternalSpell";
         spelleternaltwo.baseStats.mod_armor = -45f;
         spelleternaltwo.baseStats.mod_attackSpeed = -45f;
         spelleternaltwo.baseStats.mod_crit = -45f;
         spelleternaltwo.baseStats.mod_damage = -45f;
         spelleternaltwo.baseStats.mod_health = -45f;
         spelleternaltwo.baseStats.mod_speed = -45f;
         spelleternaltwo.baseStats.mod_diplomacy = -45f;
         spelleternaltwo.can_be_given = false;
         spelleternaltwo.action_death = (WorldAction)Delegate.Combine(spelleternaltwo.action_death, new WorldAction(EternalTwo));
         spelleternaltwo.action_special_effect = (WorldAction)Delegate.Combine(spelleternaltwo.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spelleternaltwo);
         addTraitToLocalizedLibrary(spelleternaltwo.id, "The wizard has reincarnated into a new human body with 45% weaker stats.");
         PlayerConfig.unlockTrait("Legendary Spell: Eternal (2 uses left)");

         ActorTrait spelleternalone = new ActorTrait();
         spelleternalone.id = "Legendary Spell: Eternal (1 use left)";
         spelleternalone.path_icon = "ui/Icons/iconEternalSpell";
         spelleternalone.baseStats.mod_armor = -60f;
         spelleternalone.baseStats.mod_attackSpeed = -60f;
         spelleternalone.baseStats.mod_crit = -60f;
         spelleternalone.baseStats.mod_damage = -60f;
         spelleternalone.baseStats.mod_health = -60f;
         spelleternalone.baseStats.mod_speed = -60f;
         spelleternalone.baseStats.mod_diplomacy = -60f;
         spelleternalone.can_be_given = false;
         spelleternalone.action_death = (WorldAction)Delegate.Combine(spelleternalone.action_death, new WorldAction(EternalOne));
         spelleternalone.action_special_effect = (WorldAction)Delegate.Combine(spelleternalone.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spelleternalone);
         addTraitToLocalizedLibrary(spelleternalone.id, "The wizard has reincarnated into a new human body with 60% weaker stats.");
         PlayerConfig.unlockTrait("Legendary Spell: Eternal (1 use left)");

         ActorTrait spelleternalzero = new ActorTrait();
         spelleternalzero.id = "Legendary Spell: Eternal (0 uses left)";
         spelleternalzero.path_icon = "ui/Icons/iconEternalSpell";
         spelleternalzero.baseStats.mod_armor = -75f;
         spelleternalzero.baseStats.mod_attackSpeed = -75f;
         spelleternalzero.baseStats.mod_crit = -75f;
         spelleternalzero.baseStats.mod_damage = -75f;
         spelleternalzero.baseStats.mod_health = -75f;
         spelleternalzero.baseStats.mod_speed = -75f;
         spelleternalzero.baseStats.mod_diplomacy = -75f;
         spelleternalzero.can_be_given = false;
         spelleternalzero.action_special_effect = (WorldAction)Delegate.Combine(spelleternalzero.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spelleternalzero);
         addTraitToLocalizedLibrary(spelleternalzero.id, "The wizard has reincarnated into a new human body with 75% weaker stats.");
         PlayerConfig.unlockTrait("Legendary Spell: Eternal (0 uses left)");    


        //ITEM SPELLS
         ActorTrait spellenragingshield = new ActorTrait();
         spellenragingshield.id = "Unknown Spell: Enraging Shield";
         spellenragingshield.path_icon = "ui/Icons/iconEnragingShield";
         spellenragingshield.action_attack_self = (WorldAction)Delegate.Combine(spellenragingshield.action_attack_self, new WorldAction(EnragingShield));
         spellenragingshield.action_special_effect = (WorldAction)Delegate.Combine(spellenragingshield.action_special_effect, new WorldAction(SpellNovice));
         AssetManager.traits.add(spellenragingshield);
         addTraitToLocalizedLibrary(spellenragingshield.id, "10% to summon a powerful shield in combat which also increases damage.");
         PlayerConfig.unlockTrait("Unknown Spell: Enraging Shield");
        //RANKS

         ActorTrait novice = new ActorTrait();
         novice.id = "Novice Wizard";
         novice.baseStats.health = 50;
         novice.baseStats.damage = 5;
         novice.path_icon = "ui/Icons/iconNoviceWizard";
         novice.can_be_given = false;
         novice.action_special_effect = (WorldAction)Delegate.Combine(novice.action_special_effect, new WorldAction(NoviceToApprentice));
         AssetManager.traits.add(novice);
         addTraitToLocalizedLibrary(novice.id, "This is the first Wizardry Rank.");
         PlayerConfig.unlockTrait("Novice Wizard");

         ActorTrait apprentice = new ActorTrait();
         apprentice.id = "Apprentice Wizard";
         apprentice.baseStats.health = 150;
         apprentice.baseStats.damage = 15;
         apprentice.can_be_given = false;
         apprentice.path_icon = "ui/Icons/iconApprenticeWizard";
         apprentice.action_special_effect = (WorldAction)Delegate.Combine(apprentice.action_special_effect, new WorldAction(ApprenticeToEducated));
         AssetManager.traits.add(apprentice);
         addTraitToLocalizedLibrary(apprentice.id, "This is the second Wizardry Rank.");
         PlayerConfig.unlockTrait("Apprentice Wizard");

         ActorTrait educated = new ActorTrait();
         educated.id = "Educated Wizard";
         educated.baseStats.health = 450;
         educated.baseStats.damage = 45;
         educated.baseStats.armor += 5;
         educated.can_be_given = false;
         educated.path_icon = "ui/Icons/iconEducatedWizard";
         educated.action_special_effect = (WorldAction)Delegate.Combine(educated.action_special_effect, new WorldAction(EducatedToSkillful));
         AssetManager.traits.add(educated);
         addTraitToLocalizedLibrary(educated.id, "This is the third Wizardry Rank.");
         PlayerConfig.unlockTrait("Educated Wizard");

         ActorTrait skilled = new ActorTrait();
         skilled.id = "Skilled Wizard";
         skilled.baseStats.health = 600;
         skilled.baseStats.damage = 65;
         skilled.baseStats.armor += 15;
         skilled.can_be_given = false;
         skilled.path_icon = "ui/Icons/iconSkilledWizard";
         skilled.action_special_effect = (WorldAction)Delegate.Combine(skilled.action_special_effect, new WorldAction(SkillfulToMaster));
         AssetManager.traits.add(skilled);
         addTraitToLocalizedLibrary(skilled.id, "This is the fourth Wizardry Rank.");
         PlayerConfig.unlockTrait("Skilled Wizard");

         ActorTrait master = new ActorTrait();
         master.id = "Master Wizard";
         master.baseStats.health = 800;
         master.baseStats.damage = 85;
         master.baseStats.armor += 35;
         master.can_be_given = false;
         master.path_icon = "ui/Icons/iconMasterWizard";
         master.action_special_effect = (WorldAction)Delegate.Combine(master.action_special_effect, new WorldAction(MasterToLegendary));
         AssetManager.traits.add(master);
         addTraitToLocalizedLibrary(master.id, "This is the fifth Wizardry Rank.");
         PlayerConfig.unlockTrait("Master Wizard");

         ActorTrait legendary = new ActorTrait();
         legendary.id = "Legendary Wizard";
         legendary.baseStats.mod_armor = 8500f;
         legendary.baseStats.mod_attackSpeed = 8500f;
         legendary.baseStats.mod_crit = 8500f;
         legendary.baseStats.mod_damage = 8500f;
         legendary.baseStats.mod_health = 8500f;
         legendary.baseStats.mod_speed = 8500f;
         legendary.baseStats.mod_diplomacy = 8500f;
         legendary.can_be_given = false;
         legendary.path_icon = "ui/Icons/iconLegendaryWizard";
         AssetManager.traits.add(legendary);
         addTraitToLocalizedLibrary(legendary.id, "This is the final Wizardry Rank.");
         PlayerConfig.unlockTrait("Legendary Wizard");

         ActorTrait gifted = new ActorTrait();
         gifted.id = "Gifted Wizard";
         gifted.baseStats.health = 1200;
         gifted.baseStats.damage = 150;
         gifted.baseStats.armor += 60;
         gifted.can_be_given = false;
         gifted.path_icon = "ui/Icons/iconGiftedWizard";
         gifted.action_special_effect = (WorldAction)Delegate.Combine(gifted.action_special_effect, new WorldAction(GiftedTrait));
         AssetManager.traits.add(gifted);
         addTraitToLocalizedLibrary(gifted.id, "This is the Wizardry Rank for the Gifted.");
         PlayerConfig.unlockTrait("Gifted Wizard");


        //SPELL EFFECTS
        }
        public static bool AcidBurst(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
            if(Toolbox.randomChance(0.5f)){
              ActionLibrary.acidBloodEffect(pTarget, pTile);
             }
          }
      		return true;

        }
        public static bool ArcanistShield(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
            if(Toolbox.randomChance(0.1f)){
             ActionLibrary.castShieldOnHimself(pTarget, pTile);
             }
          }
      		return true;
        }
        public static bool BloodAura(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
            if(Toolbox.randomChance(0.05f)){
              ActionLibrary.castBloodRain(pTarget, pTile);
             }
          }
      		return true;

        }
        public static bool DivineLight(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
            if(Toolbox.randomChance(0.1f)){
              ActionLibrary.castCure(pTarget, pTile);
              a.spawnParticle(Toolbox.color_heal);
             }
          }
      		return true;
        }
        public static bool UnholyLight(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
            if(Toolbox.randomChance(0.2f)){
              MapBox.instance.CallMethod("getObjectsInChunks", pTile, 3, MapObjectType.Actor);
              var temp_map_objects = Reflection.GetField(MapBox.instance.GetType(), MapBox.instance, "temp_map_objects") as List<BaseSimObject>;
              for (int i = 0; i < temp_map_objects.Count; i++)
              {
                  Actor actor = (Actor)temp_map_objects[i];
                      actor.addTrait("cursed");
                      actor.spawnParticle(Toolbox.color_plague);
                   }
                }
              }
              return true;
        }
        public static bool RaiseDead(BaseSimObject pTarget, WorldTile pTile = null)
        {
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          MapBox.instance.CallMethod("getObjectsInChunks", pTile, 5, MapObjectType.Actor);
          var temp_map_objects = Reflection.GetField(MapBox.instance.GetType(), MapBox.instance, "temp_map_objects") as List<BaseSimObject>;
          for (int i = 0; i < temp_map_objects.Count; i++)
           {
              Actor actor = (Actor)temp_map_objects[i];
                if(Toolbox.randomChance(0.0025f)){
                  Reflection.CallStaticMethod(typeof(Toolbox), "findSameUnitInChunkAround", pTile.chunk, "skeleton");
                   if ( ((List<Actor>)Reflection.GetField(typeof(Toolbox), null, "temp_list_units")).Count < 3 ){
                    var act = MapBox.instance.createNewUnit("skeleton", pTile, null, 0f, null);
                    act.kingdom = pTarget.kingdom;
                  }
               }
           }
          return true;

        }
        public static bool BansheeScream(BaseSimObject pTarget, WorldTile pTile = null)
        {
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          ActorStatus Data = Reflection.GetField(a.GetType(), pTarget, "data") as ActorStatus;
          if(Data.health <= 30){
            var act = MapBox.instance.createNewUnit("ghost", pTile, null, 0f, null);
            act.kingdom = pTarget.kingdom;
            a.restoreHealth(5);
            ActionLibrary.teleportRandom(pTarget, pTile);
                          
           }
          return true;

        }
        public static bool AnimateDead(BaseSimObject pTarget, WorldTile pTile = null)
        {
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          MapBox.instance.CallMethod("getObjectsInChunks", pTile, 5, MapObjectType.Actor);
          var temp_map_objects = Reflection.GetField(MapBox.instance.GetType(), MapBox.instance, "temp_map_objects") as List<BaseSimObject>;
          for (int i = 0; i < temp_map_objects.Count; i++)
           {
              Actor actor = (Actor)temp_map_objects[i];
                if(Toolbox.randomChance(0.0012f)){
                  Reflection.CallStaticMethod(typeof(Toolbox), "findSameUnitInChunkAround", pTile.chunk, "zombie");
                   if ( ((List<Actor>)Reflection.GetField(typeof(Toolbox), null, "temp_list_units")).Count < 3 ){
                    var act = MapBox.instance.createNewUnit("zombie", pTile, null, 0f, null);
                    act.kingdom = pTarget.kingdom;
                  }
               }
           }
          return true;

        }
        public static bool BleedingBlade(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          a.addTrait("cursed");
             }
      		return true;
        
        }
        public static bool Fireball(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          if(Toolbox.randomChance(0.5f)){
            ActionLibrary.castFire(pTarget, pTile);
          }
       }
      		return true;

        }
        public static bool Tranquility(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
            if(Toolbox.randomChance(0.25f)){
              ActionLibrary.flowerPrintsEffect(pTarget, pTile);
              ActionLibrary.castSpawnFertilizer(pTarget, pTile);
             }
          }
      		return true;

        }
        public static bool SpiritDead(BaseSimObject pTarget, WorldTile pTile = null)
        {
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          MapBox.instance.CallMethod("getObjectsInChunks", pTile, 5, MapObjectType.Actor);
          var temp_map_objects = Reflection.GetField(MapBox.instance.GetType(), MapBox.instance, "temp_map_objects") as List<BaseSimObject>;
          for (int i = 0; i < temp_map_objects.Count; i++)
           {
              Actor actor = (Actor)temp_map_objects[i];
                if(Toolbox.randomChance(0.0012f)){
                  Reflection.CallStaticMethod(typeof(Toolbox), "findSameUnitInChunkAround", pTile.chunk, "ghost");
                   if ( ((List<Actor>)Reflection.GetField(typeof(Toolbox), null, "temp_list_units")).Count < 3 ){
                    var act = MapBox.instance.createNewUnit("ghost", pTile, null, 0f, null);
                    act.kingdom = pTarget.kingdom;
                  }
               }
           }
          return true;

        }
        public static bool GhostDefend(BaseSimObject pTarget, WorldTile pTile = null)
        {
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          ActorStatus Data = Reflection.GetField(a.GetType(), pTarget, "data") as ActorStatus;
          if(Data.health <= 30){
            var act = MapBox.instance.createNewUnit("ghost", pTile, null, 0f, null);
            act.kingdom = pTarget.kingdom;
            a.restoreHealth(5);
                          
           }
          return true;

        }
        public static bool Whirlwind(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          if(Toolbox.randomChance(0.03f)){
            ActionLibrary.castTornado(pTarget, pTile);
          }
       }
      		return true;          

        }
        public static bool DevotionAura(BaseSimObject pTarget, WorldTile pTile = null)
        {
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          MapBox.instance.CallMethod("getObjectsInChunks", pTile, 5, MapObjectType.Actor);
          var temp_map_objects = Reflection.GetField(MapBox.instance.GetType(), MapBox.instance, "temp_map_objects") as List<BaseSimObject>;
          for (int i = 0; i < temp_map_objects.Count; i++)
           {
              Actor actor = (Actor)temp_map_objects[i];
               if(Toolbox.randomChance(0.01f)){
                 if(actor.stats.id != "demon" || actor.stats.id != "ghost" || actor.stats.id != "skeleton"){
                  actor.restoreHealth(25);
                  actor.spawnParticle(Toolbox.color_heal);
                  actor.removeTrait("crippled");
                  actor.removeTrait("skin_burns");
                  actor.removeTrait("eyepatch");
                  actor.removeTrait("plague");
                  actor.removeTrait("infected");
                  actor.removeTrait("cursed");
               }
           }
           }
          return true;          

        }
        public static bool Shockwave(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
            if(Toolbox.randomChance(0.2f)){
              MapBox.instance.spawnFlash(pTile, 1);
              MapBox.instance.applyForce(pTile, 10, 0.5f, true, false, 3, null, null, null);
             }
          }
      		return true;

        }
        public static bool Reincarnation(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          var act = MapBox.instance.createNewUnit(a.stats.id, pTile, null, 0f, null);
          var actorData = (ActorStatus)Reflection.GetField(typeof(Actor), act, "data");
          a.removeTrait("Rare Spell: Reincarnation (1 use left)");
          a.addTrait("Rare Spell: Reincarnation (0 uses left)");
          ActorTool.copyUnitToOtherUnit(a, act);
          act.kingdom = pTarget.kingdom;
          }
      		return true;

        }
        public static bool EternalFive(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          var act = MapBox.instance.createNewUnit(a.stats.id, pTile, null, 0f, null);
          var actorData = (ActorStatus)Reflection.GetField(typeof(Actor), act, "data");
          a.removeTrait("Legendary Spell: Eternal (5 uses left)");
          ActorTool.copyUnitToOtherUnit(a, act);
          act.kingdom = pTarget.kingdom;
          act.addTrait("Legendary Spell: Eternal (4 uses left)");
          }
      		return true;

        }
        public static bool EternalFour(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          var act = MapBox.instance.createNewUnit(a.stats.id, pTile, null, 0f, null);
          var actorData = (ActorStatus)Reflection.GetField(typeof(Actor), act, "data");
          a.removeTrait("Legendary Spell: Eternal (4 uses left)");
          ActorTool.copyUnitToOtherUnit(a, act);
          act.kingdom = pTarget.kingdom;
          act.addTrait("Legendary Spell: Eternal (3 uses left)");
          }
      		return true;

        }
        public static bool EternalThree(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          var act = MapBox.instance.createNewUnit(a.stats.id, pTile, null, 0f, null);
          var actorData = (ActorStatus)Reflection.GetField(typeof(Actor), act, "data");
          a.removeTrait("Legendary Spell: Eternal (3 uses left)");
          ActorTool.copyUnitToOtherUnit(a, act);
          act.kingdom = pTarget.kingdom;
          act.addTrait("Legendary Spell: Eternal (2 uses left)");
          }
      		return true;

        }
        public static bool EternalTwo(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          var act = MapBox.instance.createNewUnit(a.stats.id, pTile, null, 0f, null);
          var actorData = (ActorStatus)Reflection.GetField(typeof(Actor), act, "data");
          a.removeTrait("Legendary Spell: Eternal (2 uses left)");
          ActorTool.copyUnitToOtherUnit(a, act);
          act.kingdom = pTarget.kingdom;
          act.addTrait("Legendary Spell: Eternal (1 use left)");
          }
      		return true;

        }
        public static bool EternalOne(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          var act = MapBox.instance.createNewUnit(a.stats.id, pTile, null, 0f, null);
          var actorData = (ActorStatus)Reflection.GetField(typeof(Actor), act, "data");
          a.removeTrait("Legendary Spell: Eternal (1 use left)");
          ActorTool.copyUnitToOtherUnit(a, act);
          act.kingdom = pTarget.kingdom;
          act.addTrait("Legendary Spell: Eternal (0 uses left)");
          }
      		return true;

        }
        public static bool FireBurst(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
            if(Toolbox.randomChance(0.5f)){
              ActionLibrary.fireBlood(pTarget, pTile);
             }
          }
      		return true;
        }
        public static bool EarthCracking(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
            if(Toolbox.randomChance(0.05f)){
              MapBox.instance.earthquakeManager.startQuake(pTile, EarthquakeType.RandomPower);
             }
          }
      		return true;

        }
        public static bool GravitationalPull(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
            if(Toolbox.randomChance(0.03f)){
              ActionLibrary.unluckyMeteorite(pTarget, pTile);
             }
          }
      		return true;

        }
        public static bool ExplosiveShockwave(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
            if(Toolbox.randomChance(0.1f)){
              ActionLibrary.deathBomb(pTarget, pTile);
              MapBox.instance.spawnFlash(pTile, 1);
              MapBox.instance.applyForce(pTile, 10, 0.5f, true, false, 3, null, null, null);
             }
          }
      		return true;
        }
        public static bool ExplosiveReaction(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
            if(Toolbox.randomChance(0.2f)){
              ActionLibrary.deathBomb(pTarget, pTile);
             }
          }
      		return true;
        }
        public static bool EnragingShield(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
            if(Toolbox.randomChance(0.1f)){
            a.addStatusEffect("redShield", 20f);
             }
          }
      		return true;

        //TRAIT GIVERS

        }
        public static bool AcidProofTrait(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          a.addTrait("acid_proof");
          }
      		return true;
        }
        public static bool FireProofTrait(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          a.addTrait("fire_proof");
          }
      		return true;
        }
        public static bool IceProofTrait(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
          if(pTarget != null){
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          a.addTrait("freeze_proof");
          }
      		return true;
        //BORING

        }
        public static void addTraitToLocalizedLibrary(string id, string description)
      	{
      		string language = Reflection.GetField(LocalizedTextManager.instance.GetType(), LocalizedTextManager.instance, "language") as string;
      		Dictionary<string, string> localizedText = Reflection.GetField(LocalizedTextManager.instance.GetType(), LocalizedTextManager.instance, "localizedText") as Dictionary<string, string>;
      		localizedText.Add("trait_" + id, id);
      		localizedText.Add("trait_" + id + "_info", description);

        //RANK GIVE

        }
        public static bool SpellNovice(BaseSimObject pTarget, WorldTile pTile = null)
        {
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          var actorData = (ActorStatus)Reflection.GetField(typeof(Actor), a, "data");
          if(actorData.kills < 3){
            if(actorData.level < 2){
             a.addTrait("Novice Wizard");
            }
          }

            return true;
        }
        public static bool NoviceToApprentice(BaseSimObject pTarget, WorldTile pTile = null)
        {
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          var actorData = (ActorStatus)Reflection.GetField(typeof(Actor), a, "data");
          if(actorData.kills > 2){
            if(actorData.level > 1){
             a.addTrait("Apprentice Wizard");
             a.removeTrait("Novice Wizard");
            }
          }

            return true;
        }
        public static bool ApprenticeToEducated(BaseSimObject pTarget, WorldTile pTile = null)
        {
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          var actorData = (ActorStatus)Reflection.GetField(typeof(Actor), a, "data");
          if(actorData.kills > 8){
            if(actorData.level > 2){
             a.addTrait("Educated Wizard");
             a.removeTrait("Apprentice Wizard");
            }
          }

            return true;
        }
        public static bool EducatedToSkillful(BaseSimObject pTarget, WorldTile pTile = null)
        {
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          var actorData = (ActorStatus)Reflection.GetField(typeof(Actor), a, "data");
          if(actorData.kills > 26){
            if(actorData.level > 4){
             a.addTrait("Skilled Wizard");
             a.removeTrait("Educated Wizard");
            }
          }

            return true;
        
        }
        public static bool SkillfulToMaster(BaseSimObject pTarget, WorldTile pTile = null)
        {
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          var actorData = (ActorStatus)Reflection.GetField(typeof(Actor), a, "data");
          if(actorData.kills > 80){
            if(actorData.level > 7){
             a.addTrait("Master Wizard");
             a.removeTrait("Skilled Wizard");
            }
          }

            return true;
        }
        public static bool MasterToLegendary(BaseSimObject pTarget, WorldTile pTile = null)
        {
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
          var actorData = (ActorStatus)Reflection.GetField(typeof(Actor), a, "data");
          if(actorData.kills > 849){
            if(actorData.level > 9){
             a.addTrait("Legendary Wizard");
             a.removeTrait("Master Wizard");
            }
          }

            return true;
        }
        public static bool GiftedTrait(BaseSimObject pTarget, WorldTile pTile = null)
        {
          Actor a = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
             a.addExperience(5000);
             a.addTrait("Gifted Wizard");
             a.removeTrait("Novice Wizard");
             a.removeTrait("Skilled Wizard");
             a.removeTrait("Master Wizard");
             a.removeTrait("Apprentice Wizard");
             a.removeTrait("Educated Wizard");
             
              
            
            return true;
        }
     }
  }
    


    