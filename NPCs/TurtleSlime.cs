using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class TurtleSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Turtle Slime");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.ToxicSludge];
        }

        public override void SetDefaults()
        {
            npc.width = 36;
            npc.height = 30;
            npc.damage = 7;
            npc.defense = 2;
            npc.lifeMax = 25;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 25f;
            npc.knockBackResist = 0.25f;
            npc.aiStyle = 1;
            aiType = NPCID.ToxicSludge;
            animationType = NPCID.ToxicSludge;
            Main.npcCatchable[mod.NPCType("TurtleSlime")] = true;
            npc.catchItem = (short)mod.ItemType("TurtleSlimeItem");
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.SurfaceJungle.Chance * 0.015f;
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.Gel);
        }
    }
}
