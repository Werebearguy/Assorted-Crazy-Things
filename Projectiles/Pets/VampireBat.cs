using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Projectiles.Pets
{
	public class VampireBat : ModProjectile
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Vampire Bat");
					Main.projFrames[projectile.type] = 4;
					Main.projPet[projectile.type] = true;
				}
			public override void SetDefaults()
				{
					projectile.CloneDefaults(ProjectileID.ZephyrFish);
					aiType = ProjectileID.ZephyrFish;
					projectile.width = 46;
					projectile.height = 40;
				}
			public override bool PreAI()
				{
					Player player = Main.player[projectile.owner];
					player.zephyrfish = false; // Relic from aiType
					return true;
				}
			public override void AI()
				{
					Player player = Main.player[projectile.owner];
					MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
					if (player.dead)
						{
							modPlayer.VampireBat = false;
						}
					if (modPlayer.VampireBat)
						{
							projectile.timeLeft = 2;
						}
				}
		}
}