using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class GiantAnomalocaris : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Giant Anomalocaris");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Piranha];
        }

        public override void SetDefaults()
        {
            npc.width = 76;
            npc.height = 24;
            npc.damage = 30;
            npc.defense = 1;
            npc.lifeMax = 225;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 75f;
            npc.knockBackResist = 0.5f;
            npc.aiStyle = 16;
            aiType = NPCID.Piranha;
            animationType = NPCID.Piranha;
            npc.noGravity = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Ocean.Chance * 0.0075f;
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.Shrimp);
            if (Main.rand.NextBool(100)) // a 1 in 100 chance
                Item.NewItem(npc.getRect(), mod.ItemType("AnomalousWings"), prefixGiven: -1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GiantAnomalocarisGore_0"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GiantAnomalocarisGore_1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GiantAnomalocarisGore_1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GiantAnomalocarisGore_1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GiantAnomalocarisGore_1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GiantAnomalocarisGore_3"), 1f);
            }
        }
    }
}