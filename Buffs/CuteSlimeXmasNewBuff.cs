using AssortedCrazyThings.Projectiles.Pets;

namespace AssortedCrazyThings.Buffs
{
    public class CuteSlimeXmasNewBuff : CuteSlimeBaseBuff
    {
        protected override void MoreSetDefaults()
        {
            DisplayName.SetDefault("Cute Christmas Slime");
            Description.SetDefault("A cute christmas slime girl is following you");
        }

        protected override void MoreUpdate(PetPlayer mPlayer)
        {
            mPlayer.CuteSlimeXmasNew = true;
            projType = mod.ProjectileType<CuteSlimeXmasNewProj>();
        }
    }
}
