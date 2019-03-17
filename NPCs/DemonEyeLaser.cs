using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class DemonEyeLaser : ModNPC
    {
        private const int TotalNumberOfThese = 3;

        /*LG = 0
        * LP = 1
        * LR = 2
        */
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/NPCs/DemonEyeLaser_0"; //use fixed texture
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mechanical Laser Eye");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.DemonEye];
        }

        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.DemonEye);
            npc.width = 32;
            npc.height = 32;
            npc.damage = 18;
            npc.defense = 2;
            npc.lifeMax = 60;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 75f;
            npc.knockBackResist = 0.5f;
            npc.aiStyle = 2;
            aiType = NPCID.DemonEye;
            animationType = NPCID.DemonEye;
            banner = Item.NPCtoBanner(NPCID.DemonEye);
            bannerItem = Item.BannerToItem(banner);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return !NPC.downedMechBoss2 ? 0f : SpawnCondition.OverworldNightMonster.Chance * 0.025f;
        }

        public override void NPCLoot()
        {
            if (Main.rand.NextBool(33))
                Item.NewItem(npc.getRect(), ItemID.Lens, 1);
            if (Main.rand.NextBool(100))
                Item.NewItem(npc.getRect(), ItemID.BlackLens, 1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                switch ((int)AiTexture)
                {
                    case 0:
                        Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DemonEyeGreenGore_0"), 1f);
                        break;
                    case 1:
                        Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DemonEyePurpleGore_0"), 1f);
                        break;
                    case 2:
                        Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DemonEyeRedGore_0"), 1f);
                        break;
                    default:
                        break;
                }
            }
        }

        public float AiTexture
        {
            get
            {
                return npc.ai[3];
            }
            set
            {
                npc.ai[3] = value;
            }
        }

        public override bool PreAI()
        {
            if (AiTexture == 0 && npc.localAI[0] == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                AiTexture = Main.rand.Next(TotalNumberOfThese);

                npc.localAI[0] = 1;
                npc.netUpdate = true;
            }

            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("NPCs/DemonEyeLaser_" + AiTexture);
            Vector2 stupidOffset = new Vector2(-4f, 0f); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(npc.width * 0.5f, npc.height * 0.5f);
            Vector2 drawPos = npc.position - Main.screenPosition + drawOrigin + stupidOffset;
            spriteBatch.Draw(texture, drawPos, new Rectangle?(npc.frame), drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effect, 0f);
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("NPCs/DemonEyeLaser_Glowmask");

            Vector2 stupidOffset = new Vector2(-4f, 0f);
            SpriteEffects effect = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(npc.width * 0.5f, npc.height * 0.5f);
            Vector2 drawPos = npc.position - Main.screenPosition + drawOrigin + stupidOffset;
            spriteBatch.Draw(texture, drawPos, new Rectangle?(npc.frame), Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, effect, 0f);
        }


        public Vector2 RotToNormal(float rotation)
        {
            return new Vector2((float)Math.Sin(rotation), (float)-Math.Cos(rotation));
        }

        public float AngleBetween(Vector2 v1, Vector2 v2)
        {
            double sin = v1.X * v2.Y - v2.X * v1.Y;
            double cos = v1.X * v2.X + v1.Y * v2.Y;

            return (float)Math.Atan2(sin, cos);
        }

        public float AiShootTimer
        {
            get
            {
                return npc.ai[0];
            }
            set
            {
                npc.ai[0] = value;
            }
        }

        public float AiShootCount
        {
            get
            {
                return npc.ai[1];
            }
            set
            {
                npc.ai[1] = value;
            }
        }

        public override void PostAI()
        {
            Vector2 npcposition = npc.Center;
            Vector2 distance = new Vector2(Main.player[npc.target].position.X + (Main.player[npc.target].width / 2) - npcposition.X,
                                           Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2) - npcposition.Y);
            float rot = npc.rotation;
            if (npc.spriteDirection == 1)
            {
                rot += MathHelper.TwoPi / 2;
            }

            //Vector fuckery
            bool canShoot = Math.Abs(AngleBetween(RotToNormal(rot - MathHelper.TwoPi / 4), distance / distance.Length())) < 0.3f;
            float shootDelay = 180f;

            //Main.NewText("rotation: " + (rot - MathHelper.TwoPi/4)); //(npc.rotation + MathHelper.TwoPi/4)
            //Main.NewText("distance: " + AngleBetween(RotToNormal(rot - MathHelper.TwoPi/4), distance / distance.Length()));
            //Main.NewText("rotation vector: " + RotToNormal(rot - MathHelper.TwoPi / 4));
            //Main.NewText("distance vector: " + distance/distance.Length());
            if (canShoot)
            {
                AiShootTimer++;
                if (AiShootCount < 2f)
                {
                    shootDelay = 30f;
                }
                else if (AiShootCount == 2f)
                {
                    shootDelay = 180f;
                }
                else if (AiShootCount > 2f)
                {
                    AiShootTimer = 0f;
                    AiShootCount = 0f;
                }
            }
            else
            {
                AiShootTimer = 0f;
                AiShootCount = 0f;
            }
            if (canShoot && (AiShootTimer > shootDelay) && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
            {
                AiShootCount++;
                AiShootTimer = 0f;
                float distancex = distance.X;
                float distancey = distance.Y;
                float num427 = 8.5f;
                int damage = 8;
                int type = ProjectileID.PinkLaser; //the gastropod one
                if (Main.expertMode)
                {
                    num427 = 10f;
                    damage = 6;
                }
                float distancen = (float)Math.Sqrt((double)(distancex * distancex + distancey * distancey));
                distancen = num427 / distancen;
                distancex *= distancen;
                distancey *= distancen;
                npcposition.X += distancex * 5f;
                npcposition.Y += distancey * 5f;
                Projectile.NewProjectile(npcposition.X, npcposition.Y, distancex, distancey, type, damage, 0f, Owner: Main.myPlayer);
            }
        }
    }
}
