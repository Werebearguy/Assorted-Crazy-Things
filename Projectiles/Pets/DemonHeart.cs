using AssortedCrazyThings.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class DemonHeart : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Demon Heart");
            Main.projFrames[projectile.type] = 4;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.ZephyrFish);
            aiType = ProjectileID.ZephyrFish;
        }

        public override bool PreAI()
        {
            Player player = projectile.GetOwner();
            player.zephyrfish = false; // Relic from aiType
            return true;
        }

        public override void AI()
        {
            Player player = projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.DemonHeart = false;
            }
            if (modPlayer.DemonHeart)
            {
                projectile.timeLeft = 2;
            }
            AssAI.TeleportIfTooFar(projectile, player.MountedCenter);
        }
    }
}
