using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class ObservingEyeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Observing Eye");
            Main.projFrames[projectile.type] = 2;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.ZephyrFish);
            aiType = ProjectileID.ZephyrFish;
            projectile.width = 30;
            projectile.height = 48;
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
                modPlayer.ObservingEye = false;
            }
            if (modPlayer.ObservingEye)
            {
                projectile.timeLeft = 2;
            }
            AssAI.TeleportIfTooFar(projectile, player.MountedCenter);
        }

        public override void PostAI()
        {
            if (projectile.frame > 1) projectile.frame = 0;

            Vector2 between = projectile.Center - projectile.GetOwner().Center;
            projectile.rotation = (float)Math.Atan2(between.Y, between.X) + 1.57f;
            projectile.spriteDirection = projectile.direction = -(between.X < 0).ToDirectionInt();
        }
    }
}
