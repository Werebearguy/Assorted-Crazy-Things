using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class DemonEyeSeasonal : ModNPC
    {
        private const int TotalNumberOfThese = 4;

        /*OG = 0
        * OP = 1
        * UG = 2
        * UP = 3
        */
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/NPCs/DemonEyeSeasonal_0"; //use fixed texture
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Demon Eye");
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
            if (Main.halloween)
            {
                return SpawnCondition.OverworldNightMonster.Chance * 0.025f;
            }
            return 0f;
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
                        Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DemonEyeGreenGore_1"), 1f);
                        Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DemonEyeGreenGore_0"), 1f);
                        break;
                    case 1:
                        Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DemonEyePurpleGore_1"), 1f);
                        Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DemonEyePurpleGore_0"), 1f);
                        break;
                    case 2:
                        Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DemonEyeGreenGore_1"), 1f);
                        Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DemonEyeGreenGore_0"), 1f);
                        break;
                    case 3:
                        Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DemonEyePurpleGore_1"), 1f);
                        Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DemonEyePurpleGore_0"), 1f);
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
            Texture2D texture = mod.GetTexture("NPCs/DemonEyeSeasonal_" + AiTexture);
            Vector2 stupidOffset = new Vector2(0f, npc.height / 3); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(npc.width * 0.5f, npc.height * 0.5f);
            Vector2 drawPos = npc.position - Main.screenPosition + drawOrigin + stupidOffset;
            spriteBatch.Draw(texture, drawPos, new Rectangle?(npc.frame), drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effect, 0f);
            return false;
        }
    }
}
