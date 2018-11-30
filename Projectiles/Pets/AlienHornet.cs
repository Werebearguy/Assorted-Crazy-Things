using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Projectiles.Pets
{
	public class AlienHornet : ModProjectile
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Alien Hornet");
					Main.projFrames[projectile.type] = 4;
					Main.projPet[projectile.type] = true;
				}
			public override void SetDefaults()
				{
					projectile.CloneDefaults(ProjectileID.BabyHornet);
					aiType = ProjectileID.BabyHornet;
					projectile.width = 38;
					projectile.height = 36;
					projectile.alpha = 0;
				}
			public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
				{
					Texture2D texture = mod.GetTexture("Glowmasks/AlienHornet_glowmask");
					Vector2 drawPos = projectile.position - Main.screenPosition;
					Rectangle frame = new Rectangle(0, 0, texture.Width, texture.Height / 4);
					frame.Y = (int)projectile.frameCounter % 60;
					if (frame.Y > 24)
						{
							frame.Y = 24;
						}
					frame.Y *= projectile.height;
					spriteBatch.Draw(texture, drawPos, frame, Color.White * 0.7f, 0f, Vector2.Zero, 1f, projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
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
							modPlayer.AlienHornet = false;
						}
					if (modPlayer.AlienHornet)
						{
							projectile.timeLeft = 2;
						}
				}
		}
}