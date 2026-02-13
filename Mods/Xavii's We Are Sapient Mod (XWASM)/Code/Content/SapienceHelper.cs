namespace XWASM.Code.Content
{
    public static class SapienceHelper
    {
        public static bool HasSapientTrait(Actor actor)
        {
            return actor != null && actor.hasTrait(TraitRegistry.SapientTraitId);
        }

        public static bool HasSapientSpecies(Subspecies subspecies)
        {
            return subspecies != null && subspecies.hasTrait(TraitRegistry.SapientSpeciesTraitId);
        }

        public static bool IsSapientByTrait(Actor actor)
        {
            return HasSapientTrait(actor) || HasSapientSpecies(actor?.subspecies);
        }

        public static string GetKingdomId(Actor actor)
        {
            if (actor == null)
                return TraitRegistry.DefaultKingdomId;
            if (string.IsNullOrEmpty(actor.asset.kingdom_id_civilization) && IsSapientByTrait(actor))
                return TraitRegistry.DefaultKingdomId;
            return actor.asset.kingdom_id_civilization;
        }
    }
}
