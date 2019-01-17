using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class DocileMechanicalEyeGreen : ModProjectile
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/DocileDemonEye_7";
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Docile Mechanical Eye");
            Main.projFrames[projectile.type] = 2;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BabyEater);
            aiType = ProjectileID.BabyEater;
        }

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            player.eater = false; // Relic from aiType
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.DocileMechanicalEyeGreen = false;
            }
            if (modPlayer.DocileMechanicalEyeGreen)
            {
                projectile.timeLeft = 2;
            }
        }
    }
}
