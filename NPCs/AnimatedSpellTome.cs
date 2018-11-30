using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.NPCs
{
    public class AnimatedSpellTome : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Animated Spell Tome");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.GiantBat];
        }

        public override void SetDefaults()
        {
            npc.width = 44;
            npc.height = 32;
            npc.damage = 13;
            npc.defense = 2;
            npc.lifeMax = 16;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 60f;
            npc.knockBackResist = 0.8f;
            npc.aiStyle = 14;
            npc.noGravity = true;
            aiType = NPCID.GiantBat;
            animationType = NPCID.GiantBat;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Dungeon.Chance * 0.005f;
        }

        public override void NPCLoot()
        {
            {
                Item.NewItem(npc.getRect(), ItemID.SpellTome);
            }
        }


        //golden dust particles on hit and passively spawning sparkles in the next two methods
        //make sure to do "using Microsoft.Xna.Framework;"
        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life > 0)
            {
                int num5;
                for (int num257 = 0; num257 < 10; num257 = num5 + 1)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, Main.rand.Next(232, 234), (float)hitDirection, -1f);
                    num5 = num257;
                }
            }
            else
            {
                int num5;
                for (int num258 = 0; num258 < 20; num258 = num5 + 1)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, Main.rand.Next(232, 234), (float)(2 * hitDirection), -2f);
                    num5 = num258;
                }
            }
        }

        public override void PostAI()
        {
            //using Microsoft.Xna.Framework;
            //change the npc. to projectile. if you port this to pets
            Color color = Lighting.GetColor((int)npc.Center.X / 16, (int)npc.Center.Y / 16);
            if (color.R > 20 || color.B > 20 || color.G > 20)
            {
                int num = color.R;
                if (color.G > num)
                {
                    num = color.G;
                }
                if (color.B > num)
                {
                    num = color.B;
                }
                num /= 30;
                if (Main.rand.Next(300) < num)
                {
                    int num2 = Dust.NewDust(npc.position, npc.width, npc.height, 43, 0f, 0f, 254, new Color(255, 255, 0), 0.5f);
                    Main.dust[num2].velocity *= 0f;
                }
            }
        }
    }
}