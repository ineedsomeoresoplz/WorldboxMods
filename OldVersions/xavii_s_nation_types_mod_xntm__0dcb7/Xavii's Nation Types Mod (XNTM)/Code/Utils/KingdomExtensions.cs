namespace XNTM.Code.Utils
{
    public static class KingdomExtensions
    {
        public static bool isInWar(this Kingdom kingdom)
        {
            return kingdom != null && kingdom.isAlive() && World.world?.wars != null && World.world.wars.hasWars(kingdom);
        }
    }
}
