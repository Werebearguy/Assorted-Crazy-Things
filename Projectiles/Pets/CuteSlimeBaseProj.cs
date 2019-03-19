﻿using AssortedCrazyThings.Items.PetAccessories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public abstract class CuteSlimeBaseProj : ModProjectile
    {
        public const int Projwidth = 28;
        public const int Projheight = 32;
        public const int Texwidth = 28;
        public const int Texheight = 52;

        protected int frame2Counter = 0;
        protected int frame2 = 0;

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            player.lizard = false;
            return true;
        }

        public override void PostAI()
        {
            if (projectile.ai[0] != 0f) //frame 6 to 9 flying
            {
                frame2Counter += 3;
                if (frame2Counter > 6)
                {
                    frame2++;
                    frame2Counter = 0;
                }
                if (frame2 <= 5 || frame2 > 9)
                {
                    frame2 = 6;
                }
            }
            else //frame 1 to 4 walking
            {
                if (projectile.velocity.Y == 0.1f)
                {
                    if (projectile.velocity.X == 0f)
                    {
                        frame2 = 0; //0 //idle frame
                        frame2Counter = 0;
                    }
                    else if (Math.Abs(projectile.velocity.X) > 0.1)
                    {
                        frame2Counter += (int)(Math.Abs(projectile.velocity.X) * 0.25f);
                        frame2Counter++;
                        if (frame2Counter > 6)
                        {
                            frame2++;
                            frame2Counter = 0;
                        }
                        if (frame2 > 4) //5
                        {
                            frame2 = 1; //0
                        }
                    }
                    else
                    {
                        frame2 = 1; //0
                        frame2Counter = 0;
                    }
                }
                else //frame 6 to 9 flying
                {
                    frame2Counter++;
                    if (projectile.velocity.Y < 0f)
                    {
                        frame2Counter += 2;
                    }
                    if (frame2Counter > 6)
                    {
                        frame2++;
                        frame2Counter = 0;
                    }
                    if (frame2 > 9)
                    {
                        frame2 = 6;
                    }
                    if (frame2 < 6)
                    {
                        frame2 = 6;
                    }
                }
            }

            if (projectile.velocity.Y != 0.1f) projectile.rotation = projectile.velocity.X * 0.01f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (!SlimePets.slimePetLegacy.Contains(projectile.type)) //if not a legacy slime
            {
                DrawAccessories(spriteBatch, drawColor, preDraw: true);
            }

            DrawBaseSprite(spriteBatch, drawColor);
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (!SlimePets.slimePetLegacy.Contains(projectile.type)) //if not a legacy slime
            {
                DrawAccessories(spriteBatch, drawColor);
            }
        }

        private void DrawBaseSprite(SpriteBatch spriteBatch, Color drawColor)
        {
            PetPlayer pPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>();
            //check if it wears a "useNoHair" hat, then if it does, change the texture to that,
            //otherwise use default one
            bool useNoHair = false;
            uint slimeAccessoryHat = pPlayer.GetAccessory((byte)SlotType.Hat);
            if (slimeAccessoryHat != 0 &&
                PetAccessory.UseNoHair[slimeAccessoryHat] &&
                !SlimePets.slimePetLegacy.Contains(projectile.type) && //if its not legacy
                SlimePets.GetPet(projectile.type).HasNoHair) //if it has a NoHair tex
            {
                useNoHair = true;
            }

            bool drawPreAddition = true;
            bool drawPostAddition = true;
            //handle if pre/post additions are drawn based on the slimePet(Pre/Post)AdditionSlot
            if (!SlimePets.slimePetLegacy.Contains(projectile.type))
            {
                for (byte slotNumber = 1; slotNumber < 5; slotNumber++)
                {
                    uint slimeAccessory = pPlayer.GetAccessory(slotNumber);
                    
                    if (slimeAccessory != 0 && SlimePets.GetPet(projectile.type).PreAdditionSlot == slotNumber)
                    {
                        drawPreAddition = false;
                    }
                    if (slimeAccessory != 0 && SlimePets.GetPet(projectile.type).PostAdditionSlot == slotNumber)
                    {
                        drawPostAddition = false;
                    }
                }
            }

            bool drawnPreDraw = drawPreAddition ? MorePreDrawBaseSprite(spriteBatch, drawColor, useNoHair) : true;

            if (drawnPreDraw) //do a pre-draw for the rainbow slimes
            {
                //Draw NoHair if necessary, otherwise regular sprite
                Texture2D texture = Main.projectileTexture[projectile.type];
                if (useNoHair) //only if not legacy
                {
                    texture = ModLoader.GetTexture(texture.Name + "NoHair");
                }
                Rectangle frameLocal = new Rectangle(0, frame2 * Texheight, texture.Width, texture.Height / 10);
                SpriteEffects effect = projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Vector2 drawOrigin = new Vector2(Texwidth * 0.5f, Texheight * 0.5f);
                Vector2 stupidOffset = new Vector2(0f, projectile.gfxOffY + drawOriginOffsetY);
                Vector2 drawPos = projectile.position - Main.screenPosition + drawOrigin + stupidOffset;
                Color color = drawColor * ((255f - projectile.alpha) / 255f);

                spriteBatch.Draw(texture, drawPos, new Rectangle?(frameLocal), color, projectile.rotation, frameLocal.Size() / 2, projectile.scale, effect, 0f);
            }

            if (drawPostAddition) MorePostDrawBaseSprite(spriteBatch, drawColor); //used for xmas slime bow
        }

        public virtual bool MorePreDrawBaseSprite(SpriteBatch spriteBatch, Color drawColor, bool useNoHair)
        {
            return true;
        }

        public virtual void MorePostDrawBaseSprite(SpriteBatch spriteBatch, Color drawColor)
        {

        }

        private void DrawAccessories(SpriteBatch spriteBatch, Color drawColor, bool preDraw = false)
        {
            PetPlayer pPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>();

            for (byte slotNumber = 1; slotNumber < 5; slotNumber++) //0 is None, reserved
            {
                //slimeAccessory is the indexed number of the accessory (from 0 to 255)
                
                uint slimeAccessory = pPlayer.GetAccessory(slotNumber);

                if ((preDraw || !PetAccessory.PreDraw[slimeAccessory]) &&
                    slimeAccessory != 0 &&
                    !SlimePets.GetPet(projectile.type).IsSlotTypeBlacklisted[slotNumber])
                {
                    Texture2D texture = PetAccessory.Texture[slimeAccessory];

                    int altTextureNumber = PetAccessory.AltTexture[slimeAccessory, (byte)SlimePets.GetPet(projectile.type).Color];
                       
                    if (altTextureNumber > 0) //change texture if not -1 and not -0
                    {
                        texture = ModLoader.GetTexture(texture.Name + altTextureNumber);
                    }
                    else if (altTextureNumber == -1)
                    {
                        continue;
                    }
                    //else if 0: normal behavior

                    Rectangle frameLocal = new Rectangle(0, frame2 * Texheight, texture.Width, texture.Height / 10);

                    //get necessary properties and parameters for draw
                    SpriteEffects effect = projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                    Vector2 drawOrigin = new Vector2(Texwidth * 0.5f, Texheight * 0.5f);
                    Vector2 stupidOffset = new Vector2(0f, drawOriginOffsetY + projectile.gfxOffY);
                    Color color = drawColor * ((255 - PetAccessory.Alpha[slimeAccessory]) / 255f);

                    Vector2 originOffset = - PetAccessory.Offset[slimeAccessory];
                    if (projectile.spriteDirection == -1)
                    {
                        originOffset.X = -originOffset.X;
                    }
                    
                    Vector2 drawPos = projectile.position - Main.screenPosition + drawOrigin + stupidOffset;
                    spriteBatch.Draw(texture, drawPos, new Rectangle?(frameLocal), color, projectile.rotation, frameLocal.Size() / 2 + originOffset, projectile.scale, effect, 0f);
                }
            }
        }
    }
}
