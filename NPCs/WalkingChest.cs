using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class WalkingChest : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Walking Chest");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Crab];
        }

        public override void SetDefaults()
        {
            npc.width = 44;
            npc.height = 34;
            npc.damage = 0;
            npc.defense = 10;
            npc.lifeMax = 40;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 75f;
            npc.knockBackResist = 0.5f;
            npc.aiStyle = 3;
            aiType = NPCID.Crab;
            animationType = NPCID.Crab;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Underground.Chance * 0.025f * 0.083f;
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.Chest);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/WalkingTombstoneGore_01"), 1f);
            }
        }

        public override void PostAI()
        {
            if (Main.dayTime)
            {
                if (npc.velocity.X > 0 || npc.velocity.X < 0)
                {
                    npc.velocity.X = 0;
                }
                if (npc.velocity.Y < 0)
                {
                    npc.velocity.Y = 0;
                }
            }
        }
    }
}
