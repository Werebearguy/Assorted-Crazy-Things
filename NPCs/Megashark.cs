using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class Megashark : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Megashark");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Shark];
        }

        public override void SetDefaults()
        {
            npc.width = 120;
            npc.height = 74;
            npc.damage = 45;
            npc.defense = 2;
            npc.lifeMax = 400;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 75f;
            npc.knockBackResist = 0.5f;
            npc.aiStyle = 16;
            aiType = NPCID.Shark;
            animationType = NPCID.Shark;
            npc.noGravity = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Ocean.Chance * 0.005f;
        }

        public override void NPCLoot()
        {
            if (Main.rand.NextBool(2))
                Item.NewItem(npc.getRect(), ItemID.SharkFin, 1);
            if (Main.rand.NextBool(98))
                Item.NewItem(npc.getRect(), ItemID.DivingHelmet, 1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MegasharkGore_0"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MegasharkGore_1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MegasharkGore_2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MegasharkGore_3"), 1f);
            }
        }
    }
}
