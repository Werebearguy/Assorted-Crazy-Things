using AssortedCrazyThings.Base;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
    public class CuteSlimeBlackNewProj : CuteSlimeBaseProj
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Black Slime");
            Main.projFrames[projectile.type] = 10;
            Main.projPet[projectile.type] = true;
            drawOffsetX = -18; //-20
            //drawOriginOffsetX = 0;
            drawOriginOffsetY = -18; //-22
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.PetLizard);
            projectile.width = Projwidth; //64 because of wings
            projectile.height = Projheight;
            aiType = ProjectileID.PetLizard;
            projectile.scale = 0.9f;
            projectile.alpha = 75;
        }

        public override void AI()
        {
            Player player = projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.CuteSlimeBlackNew = false;
            }
            if (modPlayer.CuteSlimeBlackNew)
            {
                projectile.timeLeft = 2;
            }
        }
    }
}
