using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	public class BabyIchorSticker : ModProjectile
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Baby Ichor Sticker");
					Main.projFrames[projectile.type] = 4;
					Main.projPet[projectile.type] = true;
				}
			public override void SetDefaults()
				{
					projectile.CloneDefaults(ProjectileID.BabyHornet);
					aiType = ProjectileID.BabyHornet;
					projectile.width = 34;
					projectile.height = 38;
				}
			public override bool PreAI()
				{
					Player player = Main.player[projectile.owner];
					player.hornet = false; // Relic from aiType
					return true;
				}
			public override void AI()
				{
					Player player = Main.player[projectile.owner];
					MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
					if (player.dead)
						{
							modPlayer.BabyIchorSticker = false;
						}
					if (modPlayer.BabyIchorSticker)
						{
							projectile.timeLeft = 2;
						}
				}
		}
}