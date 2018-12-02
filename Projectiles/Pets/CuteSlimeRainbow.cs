using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	public class CuteSlimeRainbow : ModProjectile
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Cute Rainbow Slime");
					Main.projFrames[projectile.type] = 10;
					Main.projPet[projectile.type] = true;
				}
			public override void SetDefaults()
				{
					projectile.CloneDefaults(ProjectileID.PetLizard);
					aiType = ProjectileID.PetLizard;
					projectile.scale = 1.2f;
					projectile.alpha = 0;
				}
			public override bool PreAI()
				{
					Player player = Main.player[projectile.owner];
					return true;
				}
			public override void AI()
				{
					Player player = Main.player[projectile.owner];
					MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
					if (player.dead)
						{
							modPlayer.CuteSlimeRainbow = false;
						}
					if (modPlayer.CuteSlimeRainbow)
						{
							projectile.timeLeft = 2;
						}
				}
			public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
				{
					double cX = projectile.position.X + projectile.width * 2;
					double cY = projectile.position.Y + projectile.height * 2;
					Color baseColor = new Color();
					baseColor.R = (byte)Main.DiscoR;
					baseColor.G = (byte)Main.DiscoG;
					baseColor.B = (byte)Main.DiscoB;
					lightColor = Lighting.GetColor((int)(cX / 16), (int)(cY / 16), baseColor);
					SpriteEffects effects = SpriteEffects.None;
					if (projectile.direction != -1)
						{
							effects = SpriteEffects.FlipHorizontally;
						}
					Texture2D image = Main.projectileTexture[projectile.type];
					Rectangle bounds = new Rectangle();
					bounds.X = 0;
					bounds.Width = image.Bounds.Width;
					bounds.Height = (int) (image.Bounds.Height / Main.projFrames[projectile.type]);
					bounds.Y = projectile.frame * bounds.Height;
					spriteBatch.Draw(image, projectile.position - Main.screenPosition + new Vector2( -20f, -25f), bounds, lightColor, projectile.rotation, new Vector2(0, 0), projectile.scale, effects, 0f);
					return false;
				}
		}
}