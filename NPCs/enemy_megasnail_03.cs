using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.NPCs
{
	public class enemy_megasnail_03 : ModNPC
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Juggerllusc");
					Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.SeaSnail];
				}
			public override void SetDefaults()
				{
					npc.width = 112;
					npc.height = 67;
					npc.damage = 80;
					npc.defense = 100;
					npc.lifeMax = 900;
					npc.HitSound = SoundID.NPCHit1;
					npc.DeathSound = SoundID.NPCDeath1;
					npc.value = 640f;
					npc.knockBackResist = 0f;
					npc.aiStyle = 3;
					aiType = NPCID.SeaSnail;
					animationType = NPCID.SeaSnail;
				}
			public override void OnHitPlayer(Player player, int damage, bool crit)
				{
						player.AddBuff(BuffID.Slow, 240, true);
				}
			public override float SpawnChance(NPCSpawnInfo spawnInfo)
				{
					return SpawnCondition.Ocean.Chance * 0.005f;
				}
			public override void NPCLoot()
				{
					{
						Item.NewItem(npc.getRect(), ItemID.PurpleMucos);
					}
				}
			public override void HitEffect(int hitDirection, double damage)
				{
					{
						if (npc.life <= 0)
							{
								Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_megasnail_01"), 1f);
							}
					}
				}
		}
}