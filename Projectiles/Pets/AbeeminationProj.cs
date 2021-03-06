using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets
{
    //check this file for more info vvvvvvvv
    public class AbeeminationProj : BabySlimeBase
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/AbeeminationProj_0"; //temp
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abeemination");
            Main.projFrames[projectile.type] = 6;
            Main.projPet[projectile.type] = true;
            drawOffsetX = -10;
            drawOriginOffsetY = -4;
        }

        public override void MoreSetDefaults()
        {
            //used to set dimensions (if necessary) //also use to set projectile.minion
            projectile.width = 32;
            projectile.height = 30;

            projectile.minion = false;
        }

        public override bool PreAI()
        {
            PetPlayer modPlayer = projectile.GetOwner().GetModPlayer<PetPlayer>();
            if (projectile.GetOwner().dead)
            {
                modPlayer.Abeemination = false;
            }
            if (modPlayer.Abeemination)
            {
                projectile.timeLeft = 2;
            }
            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            lightColor = Lighting.GetColor((int)(projectile.Center.X / 16), (int)(projectile.Center.Y / 16), Color.White);
            SpriteEffects effects = SpriteEffects.None;
            if (projectile.direction != -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            PetPlayer mPlayer = projectile.GetOwner().GetModPlayer<PetPlayer>();
            Texture2D image = mod.GetTexture("Projectiles/Pets/AbeeminationProj_" + mPlayer.abeeminationType);
            Rectangle bounds = new Rectangle();
            bounds.X = 0;
            bounds.Width = image.Bounds.Width;
            bounds.Height = image.Bounds.Height / Main.projFrames[projectile.type];
            bounds.Y = projectile.frame * bounds.Height;
            Vector2 stupidOffset = new Vector2(projectile.width * 0.5f/* + drawOffsetX * 0.5f*/, projectile.height * 0.5f + projectile.gfxOffY);
            spriteBatch.Draw(image, projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, projectile.rotation, bounds.Size() / 2, projectile.scale, effects, 0f);

            return false;
        }
    }
}
