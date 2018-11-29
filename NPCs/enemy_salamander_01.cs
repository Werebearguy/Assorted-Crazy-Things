using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.NPCs
{
    public class enemy_salamander_01 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Deep Salamander");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Salamander];
        }

        public override void SetDefaults()
        {
            npc.width = 56;
            npc.height = 66;
            npc.damage = 36;
            npc.defense = 20;
            npc.lifeMax = 130;
            npc.HitSound = SoundID.NPCHit50;
            npc.DeathSound = SoundID.NPCDeath53;
            npc.value = 240f;
            npc.knockBackResist = 0.2f;
            npc.aiStyle = 3;
            aiType = NPCID.Salamander;
            animationType = NPCID.Salamander;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode == true)
            {
                return SpawnCondition.Cavern.Chance * 0.005f;
            }
            else
            {
                return 0f;
            }
        }

        public override void NPCLoot()
        {
            {
                if (Main.rand.NextBool(90))
                    Item.NewItem(npc.getRect(), ItemID.DepthMeter, 1);
            }
            {
                if (Main.rand.NextBool(95))
                    Item.NewItem(npc.getRect(), ItemID.Compass, 1);
            }
            {
                if (Main.rand.NextBool(97))
                    Item.NewItem(npc.getRect(), ItemID.Gradient, 1);
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            {
                if (npc.life <= 0)
                {
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_salamander_01_03"), 1f);
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_salamander_01_02"), 1f);
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_salamander_01_01"), 1f);
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_salamander_01_03"), 1f);
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_salamander_01_02"), 1f);
                }
            }
        }

        public override bool PreAI()
        {
            //ai[1] is the internal timer for the attack delay, it naturally starts at 70,
            //and at roughly 30 it shoots the projectile, which takes 30 ticks
            //here the timer is now capped at 40 instead of 70, more than halving the delay
            if (npc.ai[1] > 40) npc.ai[1] = 40; //attack speed roughly doubled
            //Main.NewText("newline");
            //Main.NewText(npc.ai[0]);
            //Main.NewText(npc.ai[1]);
            //Main.NewText(npc.ai[2]);
            //Main.NewText(npc.ai[3]);
            return true;
        }

        public override void PostAI()
        {
            //can't really increase velocity without having to go and fix 1200 lines of vanilla code just to change two variables
            /* salamander speed cap
            velocity.X *= 2f;
			if (velocity.X > 3f)
			{
				velocity.X = 3f;
			}
			if (velocity.X < -3f)
			{
				velocity.X = -3f;
			}
			velocity.Y = -4f;
			netUpdate = true;
            */
        }
    }
}
